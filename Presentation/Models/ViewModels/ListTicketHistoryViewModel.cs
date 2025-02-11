using Domain.Models;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Presentation.Models.ViewModels
{
    public class ListTicketHistoryViewModel
    {

        //Flight Properties
        [DisplayName("Departure Date & Time")]
        public DateTime DepartureDate { get; set; }
        [DisplayName("Arrival Date & Time")]
        public DateTime ArrivalDate { get; set; }
        [DisplayName("Country From")]
        public string? CountryFrom { get; set; }
        [DisplayName("Country To")]
        public string? CountryTo { get; set; }

        //Ticket Properties
        public Guid Id { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public double PricePaid { get; set; }
        public Boolean Cancelled { get; set; }
    }
}
