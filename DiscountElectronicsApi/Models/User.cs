using System;
using System.Collections.Generic;

namespace DiscountElectronicsApi.Models
{
    public partial class User
    {
        public int? IdUsers { get; set; }
        public string? Email { get; set; } 
        public string? Password { get; set; } 
        public string? Salt { get; set; } 
        public int? IdRole { get; set; }

    }
}
