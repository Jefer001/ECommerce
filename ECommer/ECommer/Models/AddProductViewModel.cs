using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ECommer.Models
{
    public class AddProductViewModel : EditProductViewModel
    {
        #region Properties
        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Guid CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }

        [Display(Name = "Foto")]
        public IFormFile? ImageFile { get; set; }
        #endregion
    }
}
