using System.ComponentModel.DataAnnotations;

namespace BookingRoomHotel.ViewModels
{
    public class CustomerViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Pw { get; set; }
        public string PwCf { get; set; }
    }
}
