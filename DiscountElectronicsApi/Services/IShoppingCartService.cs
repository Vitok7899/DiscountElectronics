using DiscountElectronicsApi.Models;

namespace DiscountElectronicsApi.Services
{
    public interface IShoppingCartService
    {
        public ShoppingCartDTO GetShoppingCart(int userid);
    }
}
