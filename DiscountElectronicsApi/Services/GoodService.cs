using DiscountElectronicsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscountElectronicsApi.Services
{
    public class GoodService : IGoodService
    {

        private readonly discount_electronicContext _context;

        public GoodService(discount_electronicContext context)
        {
            _context = context;
        }



        public List<Good> GetFilteredGoodsByCategoryId(int id, string? search, HashSet<Characteristic>? characteristics)
        {
            if (_context.Goods == null)
            {
                throw new Exception();
            }
            var allGood = _context.Goods.Where(c => c.IdCategory == id).ToList();

            if (allGood.Count == 0 || (
                (characteristics == null || characteristics.Count == 0) &&
                string.IsNullOrEmpty(search)
            ))
            {
                return allGood;
            }

            var filteredGoods = new List<Good>();
            foreach (var c in allGood)
            {
                if (characteristics == null || characteristics.IsSubsetOf(GetCharacteristicsByGoodId(c.IdGoods)))
                {
                    if (string.IsNullOrEmpty(search) || c.Model == null || c.Model.ToLower().Contains(search.ToLower()))
                    {
                        filteredGoods.Add(c);
                    }
                }
            }

            return filteredGoods;
        }

        public List<Good> GetSortedGoods(string? order, List<Good> goods)
        {

            if ((!string.IsNullOrEmpty(order)) && order.Equals("cheaper"))
            {
                 goods
                    .Sort((g1, g2) =>
                    {
                        if (g2.Price > g1.Price)
                        {
                            return 1;
                        }
                        else if (g2.Price < g1.Price)
                        {
                            return -1;
                        }
                        return 0;

                    });
                return goods;
            }
            else
            {
                goods
                    .Sort((g1, g2) =>
                    {
                        if (g2.Price > g1.Price)
                        {
                            return -1;
                        }
                        else if (g2.Price < g1.Price)
                        {
                            return 1;
                        }
                        return 0;

                    });
                return goods;
            }
        }
          
        private HashSet<Characteristic> GetCharacteristicsByGoodId(int? goodId)
        {
            HashSet<Characteristic> result = new HashSet<Characteristic>();
            var allGoods = _context.AllGoods.Where(c => c.IdGoods == goodId).ToList();
            foreach (var c in allGoods)
            {
                var characteristic = _context.Characteristics
                    .Where(b => b.IdCharacteristics == c.IdCharacteristics).ToList();
                if (characteristic != null)
                {
                    result.Add(characteristic[0]);
                }
            }
            return result;
        }

        public bool CheckGoodExist(int? goodId)
        {
            var good = _context.Goods.Where(g => g.IdGoods == goodId).FirstOrDefault();
            return good != null;
        }

        public decimal CalcCartGoodSum(int goodId, int count)
        {
            var good = _context.Goods.Where(g => g.IdGoods == goodId).FirstOrDefault();
            return good.Price * count;
        }
    }
}
