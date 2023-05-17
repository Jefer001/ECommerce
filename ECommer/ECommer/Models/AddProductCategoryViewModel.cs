using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ECommer.Models
{
    public class AddProductCategoryViewModel
    {
        #region Properties
        public Guid ProductId { get; set; }

        [Display(Name = "Categoría")]
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        public Guid CategoryId { get; set; }

        public IEnumerable<SelectListItem> Categories { get; set; }
        #endregion
    }
}
