using System.ComponentModel;

namespace Presentation.Models.ViewModels
{
    public class AdminListAllFlightsViewModel
    {
        public Guid Id { get; set; }
        [DisplayName("Departure Date & Time")]
        public DateTime DepartureDate { get; set; }
        [DisplayName("Arrival Date & Time")]
        public DateTime ArrivalDate { get; set; }
        [DisplayName("From")]
        public string? CountryFrom { get; set; }
        [DisplayName("To")]
        public string? CountryTo { get; set; }
    }
}
