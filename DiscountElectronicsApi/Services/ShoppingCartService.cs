using DiscountElectronicsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscountElectronicsApi.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly discount_electronicContext _context;
        private IGoodService goodService;

        public ShoppingCartService(discount_electronicContext context, IGoodService goodService)
        {
            _context = context;
            this.goodService = goodService;
        }

        public ShoppingCartDTO GetShoppingCart(int userid)
        {
            var shoppingCart = _context.ShoppingCarts.Where(x => x.IdUser == userid && x.IdOrder == null).ToList();

            var shoppingCartDTO = new ShoppingCartDTO
            {
                GoodCounts = new Dictionary<int, int>(),
                Goods = new List<Good>(),
                TotalCost = 0
            };

            foreach (ShoppingCart cart in shoppingCart)
            {
                shoppingCartDTO.GoodCounts[cart.IdGoods] = cart.Count;
                var good = _context.Goods.Where(g => g.IdGoods == cart.IdGoods).FirstOrDefault();
                shoppingCartDTO.Goods.Add(good);
                shoppingCartDTO.TotalCost += goodService.CalcCartGoodSum(cart.IdGoods, cart.Count);
            }
            return shoppingCartDTO;

        }
    }
}
