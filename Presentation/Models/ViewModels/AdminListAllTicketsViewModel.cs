using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Presentation.Models.ViewModels
{
    public class AdminListAllTicketsViewModel
    {
        [DisplayName("Ticket ID")]
        public Guid Id { get; set; }
        public int Row { get; set; }
        public int Column { get; set; }
        public string? Passport { get; set; }
        [DisplayName("Paid")]
        [DisplayFormat(DataFormatString = "{0:F2}")]
        public double PricePaid { get; set; }
        public Boolean Cancelled { get; set; }
    }
}
