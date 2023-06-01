using ECommer.DAL.Entities;

namespace ECommer.Models
{
    public class HomeViewModel
    {
        #region Properties
        public ICollection<Product> Products { get; set; }

        //Esta propiedad me muestra cuánto productos llevo agregados al carrito de compras.
        public float Quantity { get; set; }
        #endregion
    }
}
