using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.ViewModels
{
    public class ListFlightViewModel
    {
        public Guid Id { get; set; }

        [DisplayName("Available Seats")]
        public int AvailableSeats { get; set; }
        [DisplayName("Departure Date & Time")]
        public DateTime DepartureDate { get; set; }
        [DisplayName("Arrival Date & Time")]
        public DateTime ArrivalDate { get; set; }
        [DisplayName("Country From")]
        public string? CountryFrom { get; set; }
        [DisplayName("Country To")]
        public string? CountryTo { get; set; }
        [DisplayName("Price")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double RetailPrice { get; set; }
        public Boolean Cancelled { get; set; }

        public bool CanBook { get; set; }

    }
}
