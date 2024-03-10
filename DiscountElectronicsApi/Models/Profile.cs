using System;
using System.Collections.Generic;

namespace DiscountElectronicsApi.Models
{
    public partial class Profile
    {
        public int? IdUsers { get; set; }
        public string? Surname { get; set; }
        public string? Name { get; set; }
        public string? SecondName { get; set; }
        public string? Email { get; set; } 
        public string? NumPhone { get; set; } 

    }
}
