using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace DiscountElectronicsApi.Models
{
    [Keyless]
    public partial class AllGood
    {
        public int IdGoods { get; set; }
        public string? Goods { get; set; } 
        public int? IdCharacteristics { get; set; }
        public string? CharactericName { get; set; } 
        public string? Characteristics { get; set; } 
        public int? IdCategory { get; set; }
        public string? Category { get; set; } 
    }
}
