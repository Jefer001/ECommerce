using System.ComponentModel.DataAnnotations;

namespace ECommer.DAL.Entities
{
    public class ProductImage : Entity
    {
        #region Properties
        public Product Product { get; set; }

        [Display(Name = "Foto")]
        public Guid ImageId { get; set; }

        [Display(Name = "Foto")]
        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://localhost:7120/images/noimage.png"
            : $"https://sales2023.blob.core.windows.net/products/{ImageId}";
        #endregion
    }
}
