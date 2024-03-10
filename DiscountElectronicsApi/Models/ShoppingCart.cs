using System;
using System.Collections.Generic;

namespace DiscountElectronicsApi.Models
{
    public partial class ShoppingCart
    {
        public int? IdShoppingCart { get; set; }
        public int Count { get; set; }
        public int IdGoods { get; set; }
        public int? IdUser { get; set; }
        public int? IdOrder { get; set; }
    }
}
