using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DiscountElectronicsApi.Models
{
    public partial class Characteristic
    {
        [Required]
        public int? IdCharacteristics { get; set; }
        public string? Values { get; set; } 
        public int? IdCharacteristicName { get; set; }

        public override bool Equals(object? obj)
        {
            return obj is Characteristic characteristic &&
                   IdCharacteristics == characteristic.IdCharacteristics;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(IdCharacteristics);
        }
    }
}
