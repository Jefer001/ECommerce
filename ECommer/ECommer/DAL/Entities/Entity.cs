using System.ComponentModel.DataAnnotations;

namespace ECommer.DAL.Entities
{
    public class Entity
    {
		#region Properties
		[Key]
        public Guid Id { get; set; }
        [Display(Name = "Fecha de creacíon")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "Fecha de modificacíon")]
        public DateTime? ModifiedDate { get; set; }
		#endregion
	}
}