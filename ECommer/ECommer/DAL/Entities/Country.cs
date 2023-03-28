using System.ComponentModel.DataAnnotations;

namespace ECommer.DAL.Entities
{
    public class Country : Entity
    {
        #region Properties
        [Display(Name = "País")]
        [MaxLength(50)]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public virtual string? Name { get; set; }
        #endregion
    }
}
