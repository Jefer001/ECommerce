using System.ComponentModel.DataAnnotations;

namespace ECommer.DAL.Entities
{
    public class State : Entity
    {
        [Display(Name = "Dpto/Estado")]
        [MaxLength(50)]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }
        public Country Country { get; set; }
    }
}
