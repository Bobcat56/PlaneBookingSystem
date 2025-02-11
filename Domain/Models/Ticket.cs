using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Models
{
    /* Ticket.cs should contain the following
     * Id, Row, Column, FlightIdFK, Passport, PricePaid, Cancelled */
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        [ForeignKey("Flight")]
        public Guid FlightIdFK { get; set; }// Foreign Key
        public virtual Flight? Flight { get; set; } //Navigational Property
        public string? Passport { get; set; }
        public double PricePaid { get; set; }
        public Boolean Cancelled { get; set; }
        public string? Owner { get; set; }

    }
}
