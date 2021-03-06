using System.ComponentModel.DataAnnotations;

namespace PostamatService.Web.DTO
{
    public class OrderForCreateDto:OrderForManipulationDto
    {
        [Required(ErrorMessage = "Postamat number is a required field.")]
        [StringLength(8, ErrorMessage = "Length for the PostamatNumber is 8 characters.", MinimumLength = 8)]
        [RegularExpression(@"^\d{4}-\d{3}$", ErrorMessage = "PostamatNumber are not allowed.")]
        public string PostamatNumber { get; set; }
    }
}
