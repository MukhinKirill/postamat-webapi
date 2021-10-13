using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PostamatService.Data.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Number { get; set; }
        public OrderStatus Status { get; set; }
        public string[] Products { get; set; }
        public decimal Cost { get; set; }
        public string PhoneNumber { get; set; }
        public string FullName { get; set; }
        [ForeignKey(nameof(Postamat))]
        public string PostamatNumber { get; set; }
        public Postamat Postamat { get; set; }
    }
}
