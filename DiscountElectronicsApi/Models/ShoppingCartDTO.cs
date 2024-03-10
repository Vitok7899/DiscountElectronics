namespace DiscountElectronicsApi.Models
{
    public class ShoppingCartDTO
    {
        public Dictionary<int, int> GoodCounts { get; set; }
        public List<Good> Goods { get; set; }
        public decimal TotalCost { get; set; }
    }
}
