using Data.Repositories;
using Domain.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Hosting;
using Presentation.Models.ViewModels;

namespace Presentation.Controllers
{
    public class AdminController : Controller
    {

        private FlightDbRepository _flightDbRepository;
        private ITicketRepository _ticketDBRepository;
        private UserDBRepository _userDBRepository;
        //Constructor Injection
        public AdminController(ITicketRepository ticketDBRepository, FlightDbRepository flightDbRepository, UserDBRepository userDBRepository)
        {
            _ticketDBRepository = ticketDBRepository;
            _flightDbRepository = flightDbRepository;
            _userDBRepository = userDBRepository;
        }

        public IActionResult ListAllFlights()
        {
            if (!User.Identity.IsAuthenticated)
            {
                TempData["errorMsg"] = "You must be an Admin in to view this";
                return RedirectToAction("Index", "Home");
            }
            
            var isUserAdmin = _userDBRepository.CheckAdmin(User.Identity.Name);

            if (isUserAdmin == null || !isUserAdmin.IsAdmin)
            {
                TempData["errorMsg"] = "You must be an Admin in to view this";
                return RedirectToAction("Index", "Home");
            }

            try
            {
                IQueryable<Flight> list = _flightDbRepository.GetFlights();

                var output = from flight in list
                             select new AdminListAllFlightsViewModel()
                             {
                                 Id = flight.Id,
                                 DepartureDate = flight.DepartureDate,
                                 ArrivalDate = flight.ArrivalDate,
                                 CountryFrom = flight.CountryFrom,
                                 CountryTo = flight.CountryTo
                             };

                return View(output);
            }
            catch (Exception ex)
            {
                TempData["errorMsg"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }
        }//Close ListAllFlights


        public IActionResult ListAllTickets(Guid id) 
        {
            if (!User.Identity.IsAuthenticated)
            {
                //Can make a toast which says "Only logged in users can view history of purchased tickets"
                TempData["errorMsg"] = "You must be an Admin in to view this";

                // Return to home (Index page) or other page? 
                return RedirectToAction("Index", "Home");
                //return RedirectToAction("Index", Request) //Was used for error handling testing (Brings up html page displaying error)
            }

            //if (User.Identity.IsAdmin)

            try
            {
                IQueryable<Ticket> ticketList = _ticketDBRepository.GetTickets(id);

                if (!ticketList.Any())
                {
                    TempData["errorMsg"] = "There are no tickets booked for this flight";
                    return RedirectToAction("ListAllFlights");
                }

                var model = from ticket in ticketList
                            select new AdminListAllTicketsViewModel()
                            {
                                Id = ticket.Id,
                                Row = ticket.Row,
                                Column = ticket.Column,
                                Passport = ticket.Passport,
                                PricePaid = ticket.PricePaid,
                                Cancelled = ticket.Cancelled
                            };

                return View(model);
            }
            catch (Exception ex)
            {
                TempData["errorMsg"] = ex.Message;
                return RedirectToAction("Index", "Home");
            }//Close ListAllTickets()
        }

    }//Close controller
}
