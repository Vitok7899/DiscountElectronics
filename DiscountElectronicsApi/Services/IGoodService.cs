using DiscountElectronicsApi.Models;
using Microsoft.EntityFrameworkCore;

namespace DiscountElectronicsApi.Services
{
    public interface IGoodService
    {
        public List<Good> GetFilteredGoodsByCategoryId(int id, string? search, HashSet<Characteristic>? characteristics);
        public List<Good> GetSortedGoods(string? order, List<Good> goods);
        public bool CheckGoodExist(int? goodId);
        public decimal CalcCartGoodSum(int goodId, int count);
    }
}
