using ECommer.DAL.Entities;

namespace ECommer.Models
{
    public class ProductHomeViewModel
    {
        #region Properties
        public ICollection<Product> Products { get; set; }
        #endregion
    }
}
