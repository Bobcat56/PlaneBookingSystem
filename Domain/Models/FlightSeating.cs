using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    public class FlightSeating
    {
        //Why did I do all this? :C 
        //REMINDER: This is redundant, TO FIX AFTER ASSIGNMENTS DONE. 
        //Book() from File repository just uses GET TICKETS... Which gets the tickets for a flight
        //At the time of building the normal Repos, I didnt see that I could filter the tickets goten from it by row and col (Idk how)
        
        public Guid Id { get; set; }

        [ForeignKey("Flight")]
        public Guid FlightIdFK { get; set; }// Foreign Key to Flight
        public virtual Flight? Flight { get; set; } //Navigational Property

        [ForeignKey("Ticket")]
        public Guid? TicketIdFK { get; set; }// Foreign Key to Tickets
        //This is made nullable as a ticket may not be assigned to a customer yet
        public virtual Ticket? Ticket { get; set; } //Navigational Property

        public bool BookedSeat { get; set; }
        public string? SeatLocation { get; set; }

        /*
         *This can be set up for when the user is booking a seat
         *Take FlightID & SeatLocation (Will be made up from a concat string (row,column)
         *If seat is not found, and is not outside the scope of the rows and column, book & create seat
         *If seat exists, check BookedSeat. If True, exception, If false, book seat
        */
    }
}
