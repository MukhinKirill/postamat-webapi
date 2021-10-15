using System.ComponentModel.DataAnnotations;

namespace PostamatService.Web.DTO
{
    public abstract class OrderForManipulationDto
    {
        [Required(ErrorMessage = "Products is a required field.")]
        [MinLength(1,ErrorMessage = "Min count of products is 1")]
        [MaxLength(10,ErrorMessage = "Max count of products is 10")]
        public string[] Products { get; set; }
        [Required(ErrorMessage = "Cost is a required field.")]
        [RegularExpression(@"^(\d*.)?\d+$", ErrorMessage = "Cost are not allowed.")]
        public decimal Cost { get; set; }
        [Required(ErrorMessage = "Client phone is a required field.")]
        [StringLength(15, ErrorMessage = "Length for the PhoneNumber is 15 characters.", MinimumLength = 15)]
        [RegularExpression(@"^\+7\d{3}-\d{3}-\d{2}-\d{2}$", ErrorMessage = "PhoneNumber are not allowed.")]
        public string PhoneNumber { get; set; }
        [Required(ErrorMessage = "Client name is a required field.")]
        [MaxLength(100, ErrorMessage = "Maximum length for the FullName is 100 characters.")]
        public string FullName { get; set; }
    }
}
