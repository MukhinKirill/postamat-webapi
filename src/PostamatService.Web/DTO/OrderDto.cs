using PostamatService.Data.Models;

namespace PostamatService.Web.DTO
{
    public class OrderDto
    {
        public int Number { get; set; }
        public OrderStatus Status { get; set; }
        public string[] Products { get; set; }
        public decimal Cost { get; set; }
        public string PostamatNumber { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
    }
}
