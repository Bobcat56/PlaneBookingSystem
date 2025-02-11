using Domain.Interfaces;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.Json;
using System.Net.Sockets;

namespace Data.Repositories
{
    public class TicketFileRepository : ITicketRepository
    {
        private string _ticketFile;
        public TicketFileRepository(string ticketFile)
        {
            _ticketFile = ticketFile;

            if (File.Exists(_ticketFile)) 
            {
                using (FileStream fs = File.Create(ticketFile))
                {
                    fs.Close();
                }
               
            }
        }

        public void Book(Ticket ticket)
        {
            if (!File.Exists(_ticketFile))
            {
                File.WriteAllText(_ticketFile, "[]");
            }
            try
            {
                //Ticket Id has to be created before due to FlightSeating being created in this code
                ticket.Id = Guid.NewGuid();
                
                //Using the newely made GetTickets()
                var flightTickets = GetTickets(ticket.FlightIdFK).ToList();

                /* Old double booking validation
                if (flightTickets.Any //Any ticket has the same row AND same column
                    (t => t.Row == ticket.Row && t.Column == ticket.Column)) 
                {
                    throw new InvalidOperationException("Seat is already booked.");
                }
                */

                //Add the new ticket to list
                flightTickets.Add(ticket);
                //re serialize them to Json
                var allNewTickets = JsonSerializer.Serialize(flightTickets);
                //And over-write everything in the file
                File.WriteAllText(_ticketFile, allNewTickets);

            }
            catch (JsonException) //Show Json errors for debugging 
            {
                throw new Exception("Error encountered while Saving/writing data from/to Json file");
            }
            catch (Exception)
            {
                throw new Exception("Error encountered while opening file");
            }
        }

        public void Cancel(Guid id)
        {
            if (!File.Exists(_ticketFile))
            {
                File.WriteAllText(_ticketFile, "[]");
            }
            try
            {
                string allText = "";
                using (StreamReader sr = File.OpenText(_ticketFile))
                {
                    allText = sr.ReadToEnd();
                    //sr.Close(); //Not needed since it closes itself
                }

                List<Ticket> listTickets = JsonSerializer.Deserialize<List<Ticket>>(allText);
                var cancelTicket = listTickets.FirstOrDefault(ticket => ticket.Id == id);

                if (cancelTicket == null)
                {
                    throw new InvalidOperationException($"Unable to cancel ticket {id}");
                }

                cancelTicket.Cancelled = true;

                string updatedJson = JsonSerializer.Serialize(cancelTicket);
                File.WriteAllText(_ticketFile, updatedJson);
            }
            catch (JsonException) //Show Json errors for debugging 
            {
                throw new Exception("Error encountered while transforming data from Json file");
            }
            catch (Exception)
            {
                throw new Exception("Error encountered while opening file");
                //throw new Exception("Error encountered while opening file", ex);
            }

        }

        public IQueryable<Ticket> GetTickets(Guid id)
        {
            if (!File.Exists(_ticketFile))
            {
                //More dynamic error handling. SHowing which file it is mapped to.
                throw new Exception($"Error: File '{_ticketFile}' doesn't exist");
            }
            try
            {
                string allText = "";
                using (StreamReader sr = File.OpenText(_ticketFile))
                {
                    allText = sr.ReadToEnd();
                    //sr.Close(); //Not needed since it closes itself
                }   
                    
                if (string.IsNullOrWhiteSpace(allText))
                {
                    return new List<Ticket>().AsQueryable();
                }

                List<Ticket> listTickets = JsonSerializer.Deserialize<List<Ticket>>(allText);
                var flightTickets = listTickets.Where(ticket => ticket.FlightIdFK == id);

                return flightTickets.AsQueryable();
            }
            catch (JsonException) //Show Json errors for debugging 
            {
                throw new Exception("Error encountered while transforming data from Json file");
            }
            catch (Exception)
            {
                throw new Exception("Error encountered while opening file");
            }
        }//Close GetTickets()

        public IQueryable<Ticket> GetMyTickets(string Owner)
        {
            if (!File.Exists(_ticketFile))
            {
                //More dynamic error handling. SHowing which file it is mapped to.
                throw new Exception($"Error: File '{_ticketFile}' doesn't exist");
            }
            try
            {
                string allText = "";
                using (StreamReader sr = File.OpenText(_ticketFile))
                {
                    allText = sr.ReadToEnd();
                    //sr.Close(); //Not needed since it closes itself
                }

                if (string.IsNullOrWhiteSpace(allText))
                {
                    return new List<Ticket>().AsQueryable();
                }

                List<Ticket> listTickets = JsonSerializer.Deserialize<List<Ticket>>(allText);
                var flightTickets = listTickets.Where(ticket => ticket.Owner == Owner);

                return flightTickets.AsQueryable();
            }
            catch (JsonException) //Show Json errors for debugging 
            {
                throw new Exception("Error encountered while transforming data from Json file");
            }
            catch (Exception)
            {
                throw new Exception("Error encountered while opening file");
            }
        }

        public bool IsSeatAlreadyBooked(Ticket ticket)
        {
            var flightTickets = GetTickets(ticket.FlightIdFK).ToList();
            return flightTickets.Any(t => t.Row == ticket.Row && t.Column == ticket.Column && !t.Cancelled);
        }
    }
}
