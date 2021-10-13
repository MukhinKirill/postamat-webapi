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
        public IEnumerable<ProductInOrder> Products { get; set; }
        public decimal Cost { get; set; }
        [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(15, ErrorMessage = "Maximum length for the PhoneNumber is 15 characters.")]
        [MinLength(15, ErrorMessage = "Minimum length for the PhoneNumber is 15 characters.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Employee name is a required field.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the FullName is 100 characters.")]
        public string FullName { get; set; }
        [ForeignKey(nameof(Postamat))]
        public int PostamatId { get; set; }
        public Postamat Postamat { get; set; }
    }
}
