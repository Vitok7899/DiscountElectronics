using System;
using System.Collections.Generic;

namespace DiscountElectronicsApi.Models
{
    public partial class Order
    {
        public int? IdOrder { get; set; }
        public string? OrderNumber { get; set; }
        public DateTime? OrderDate { get; set; }
        public decimal OrderCost { get; set; }
        public int? IdBranches { get; set; }
        public int? IdStatus { get; set; }
        public int? IdUser { get; set; }
    }
}
