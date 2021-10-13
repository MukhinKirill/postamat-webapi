using System.ComponentModel.DataAnnotations.Schema;

namespace PostamatService.Data.Models
{
    public class ProductInOrder
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}