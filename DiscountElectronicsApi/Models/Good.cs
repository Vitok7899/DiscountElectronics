using System;
using System.Collections.Generic;

namespace DiscountElectronicsApi.Models
{
    public partial class Good
    {
        public int? IdGoods { get; set; }
        public string? Model { get; set; } 
        public decimal Price { get; set; }
        public string? Description { get; set; }  
        public string? Photo { get; set; }
        public int? IdManufacturer { get; set; }
        public int? IdCategory { get; set; }
    }
}
