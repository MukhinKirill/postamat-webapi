using System.Collections.Generic;
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
        public ICollection<ProductInOrder> Products { get; set; }
        public decimal Cost { get; set; }
        [Required(ErrorMessage = "Client phone is a required field.")]
        [StringLength(15, ErrorMessage = "Length for the PhoneNumber is 15 characters.", MinimumLength = 15)]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Client fullname name is a required field.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the FullName is 100 characters.")]
        public string FullName { get; set; }
        [ForeignKey(nameof(Postamat))]
        public int PostamatId { get; set; }
        public Postamat Postamat { get; set; }
    }
}
