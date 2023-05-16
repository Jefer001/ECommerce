using System.ComponentModel.DataAnnotations;

namespace ECommer.DAL.Entities
{
    public class Category : Entity
    {
		#region Properties
		[Display(Name = "Categoria")]
        [MaxLength(100)]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }

        [Display(Name = "Descripción")]
        public string? Description { get; set; }

        public ICollection<ProductCategory> ProductCategories { get; set; }
		#endregion
	}
}
