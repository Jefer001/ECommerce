using System.ComponentModel.DataAnnotations;

namespace ECommer.DAL.Entities
{
    public class Country : Entity
    {
        [Display(Name = "País")]
        [MaxLength(50)]
        [Required(ErrorMessage = "El campo {0} es obligatorio")]
        public string Name { get; set; }

		[Display(Name = "Estados")]
		public ICollection<State> States { get; set; }

		[Display(Name = "Número Estados")]
		public int StateNumber => States == null ? 0 : States.Count;
    }
}
