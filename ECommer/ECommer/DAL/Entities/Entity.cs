using System.ComponentModel.DataAnnotations;

namespace ECommer.DAL.Entities
{
    public class Entity
    {
        #region Properties
        [Key]
        public virtual Guid Id { get; set; }
        [Display(Name = "Fecha de creacíon")]
        public virtual string? CreatedDate { get; set; }
        [Display(Name = "Fecha de modificacíon")]
        public virtual string? ModifiedDate { get; set; }
        #endregion
    }
}