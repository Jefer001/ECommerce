namespace ECommer.DAL.Entities
{
    public class ProductCategory : Entity
    {
        #region Properties
        public Product Product { get; set; }

        public Category Category { get; set; }
        #endregion
    }
}
