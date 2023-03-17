using System.ComponentModel.DataAnnotations;

namespace ECommer.DAL.Entities
{
    public class Entity
    {
        #region Properties
        [Key]
        public virtual Guid Id { get; set; }
        public virtual string? CreatedDate { get; set; }
        public virtual string? ModifiedDate { get; set; }
        #endregion
    }
}
