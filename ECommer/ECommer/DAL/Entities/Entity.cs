using System.ComponentModel.DataAnnotations;

namespace ECommer.DAL.Entities
{
    public class Entity
    {
        [Key]
        public Guid Id { get; set; }
        [Display(Name = "Fecha de creacíon")]
        public DateTime? CreatedDate { get; set; }
        [Display(Name = "Fecha de modificacíon")]
        public DateTime? ModifiedDate { get; set; }
    }
}