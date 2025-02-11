using Data.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models.ViewModels;

namespace Presentation.Controllers
{
    public class TicketsController : Controller
    {
        /*Task 4*/
        private FlightDbRepository _flightDbRepository;
        private ITicketRepository _ticketDBRepository;
        public TicketsController(ITicketRepository ticketDBRepository, FlightDbRepository flightDbRepository) { 
            _ticketDBRepository = ticketDBRepository;
            _flightDbRepository = flightDbRepository;
        }


        /* Method (and View) which returns and shows on page a list of Flights that one can choose with RETAIL prices displayed.*/
        public IActionResult ListFlights()
        {
            /* 
                This is to return a web page which displays flights that can be booked
                    a.) Fully booked flights CAN NOT be selected, BUT still displayed 
                    b.) If Departure date is in the past, DO NOT DISPLAY
            */
            try
            {
                IQueryable<Flight> list = _flightDbRepository.GetFlights().Where(x => x.DepartureDate > DateTime.Now);

                var output = from flight in list
                             select new ListFlightViewModel()
                             {
                                 Id = flight.Id,
                                 AvailableSeats = flight.AvailableSeats,
                                 DepartureDate = flight.DepartureDate,
                                 ArrivalDate = flight.ArrivalDate,
                                 CountryFrom = flight.CountryFrom,
                                 CountryTo = flight.CountryTo,
                                 RetailPrice = flight.WholeSalePrice + (flight.WholeSalePrice * (flight.ComissionRate / 100)), //((comission% / 100) * wholesalePrice) + wholesalePrice = Retail price
                                 Cancelled = flight.CancelledFlight,
                                 CanBook = flight.AvailableSeats > 0 //To remove the ability to book a fully booked or cancelled flight
                             };

                return View(output);
            }
            catch (Exception ex)
            {
                TempData["errorMsg"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }

        /* Method (and View) which allows the user to book a flight after entering the requested details to book a ticket.*/
        [HttpGet] //Method injection
        public IActionResult BookFlight(Guid Id, [FromServices] IWebHostEnvironment host)
        {
            try
            {
                string seatIcon = Path.Combine(host.WebRootPath, "images", "seatIcons", "armchair.png");

                //Fetch the flight the user wanted to book
                var flight = _flightDbRepository.GetFlight(Id);

                /*      a.) Flight must NOT be fully booked 
                 *      c.) Flight must NOT be cancelled
                 *      b.) Flight must NOT be in the past
                 *      c.) PricePaid is filled in automatically after calculating commission on WholeSalePrice */
                if (flight == null || flight.AvailableSeats <= 0 || flight.CancelledFlight || flight.DepartureDate <= DateTime.Now) 
                {
                    TempData["errorMsg"] = "Error: Can't book flight";
                    return RedirectToAction("ListFlights");
                }
                //ViewBags helps pass data to the view
                ViewBag.MaxRows = flight.Rows;
                ViewBag.MaxColumns = flight.Columns;
            
                //Prefill the price and allocate the FK
                BookViewModel model = new BookViewModel()
                {
                    //Ticket details
                    FlightIdFK = flight.Id,
                    PricePaid = flight.WholeSalePrice + (flight.WholeSalePrice * (flight.ComissionRate / 100)), // Automatically fill in the PricePaid with the calculated retail price

                    //Flight Details
                    CountryFrom = flight.CountryFrom,
                    CountryTo = flight.CountryTo,
                    DepartureDate = flight.DepartureDate,
                    ArrivalDate = flight.ArrivalDate,   
                    OutlineIcon = seatIcon
                };
            
                return View(model);
            }
            catch (Exception ex)
            {
                TempData["errorMsg"] = ex.Message;
                return RedirectToAction("ListFlights", "Tickets");
            }
        }

        [HttpPost]
        //IWebHostEnviroment used with Method Injection
        public IActionResult BookFlight(BookViewModel myModel, [FromServices] IWebHostEnvironment host)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    TempData["errorMsg"] = "Error: Please provide the requeted details";
                    return View(myModel);
                }

                if (myModel.Passport?.Length > 0)
                {
                    string fileName = Guid.NewGuid() + System.IO.Path.GetExtension(myModel.Passport.FileName);
                    //string absolutePath = host.ContentRootPath + @"\Data\Images\" + fileName;
                    string absolutePath = Path.Combine(host.WebRootPath, "Images", "PassportPhotos", fileName);
                    string relativePath = @"/images/" + @"passportPhotos/" + fileName;

                    try
                    {
                        using (FileStream fs = new FileStream(absolutePath, FileMode.CreateNew))
                        {
                            myModel.Passport.CopyTo(fs);
                            fs.Flush();
                            fs.Close();
                        }

                        _ticketDBRepository.Book(new Ticket()
                        {
                            Row = myModel.Row,
                            Column = myModel.Column,
                            FlightIdFK = myModel.FlightIdFK,
                            PricePaid = myModel.PricePaid,
                            Passport = relativePath,
                            Cancelled = false,
                            Owner = User.Identity.Name
                        });
                        TempData["msg"] = "Flight successfully booked";
                        return RedirectToAction("ListFlights");
                    }
                    catch
                    {
                        if (System.IO.File.Exists(absolutePath))
                        {
                            System.IO.File.Delete(absolutePath);
                        }
                        TempData["errorMsg"] = "Error: The seat you selected has already been booked.";
                        return View(myModel);
                    }
                }
                else
                {
                    TempData["errorMsg"] = "Error: Please upload Passport Photo";
                    return View(myModel);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMsg"] = ex.Message;
                return RedirectToAction("ListFlights", "Tickets");
            }
        }//CLose POST BookFlight()

        /*Method (and View) which returns a list of Tickets (i.e. use GetTickets from Repository) which then returns a history list 
         *of tickets purchased by the logged in client (See Se3.3 for authentication) [1.5]*/

        [Authorize]
        [HttpGet]
        public IActionResult ListTicketHistory()
        {
            /* A LOGGED IN user can view a list of their previously purchased tickets */

            if (!User.Identity.IsAuthenticated)
            {
                TempData["errorMsg"] = "You must be logged in to view this";

                // Return to home (Index page) or other page? 
                return RedirectToAction("Index", "Home");
            }

            try
            {
                string currentuser = User.Identity.Name;

                IQueryable<Ticket> list = _ticketDBRepository.GetMyTickets(currentuser);

                if (list == null)
                {
                    TempData["errorMsg"] = "You haven't purchesd any tickets yet";
                    return RedirectToAction("Index", "Home");
                }

                var output = from ticket in list
                             select new ListTicketHistoryViewModel()
                             {
                                 //FLight details
                                 DepartureDate = ticket.Flight.DepartureDate,
                                 ArrivalDate = ticket.Flight.ArrivalDate,
                                 CountryFrom = ticket.Flight.CountryFrom,
                                 CountryTo = ticket.Flight.CountryTo, 

                                 //Ticket Details
                                 Id = ticket.Id,
                                 Row = ticket.Row,
                                 Column = ticket.Column,
                                 PricePaid = ticket.PricePaid,
                                 Cancelled = ticket.Cancelled
                             };

                return View(output); ;
            }
            catch (Exception ex)
            {
                TempData["errorMsg"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }//Close ListTicketHistory()

        [HttpPost]
        public IActionResult Cancel(Guid id)
        {
            try
            {
                _ticketDBRepository.Cancel(id);
                TempData["successMsg"] = "Ticket cancelled successfully.";
            }
            catch (Exception)
            {
                TempData["errorMsg"] = "Error cancelling ticket";
            }

            // Redirect back to the ticket history view.
            return RedirectToAction("ListTicketHistory");
        }

    }//Close Controller
}//Close Namespace
