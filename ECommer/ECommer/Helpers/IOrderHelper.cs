using ECommer.Common;
using ECommer.Models;

namespace ECommer.Helpers
{
    public interface IOrderHelper
    {
        Task<Response> ProcessOrderAsync(ShowCartViewModel showCartViewModel);
    }
}
