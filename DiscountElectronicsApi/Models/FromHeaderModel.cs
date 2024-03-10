using Microsoft.AspNetCore.Mvc;

namespace DiscountElectronicsApi.Models
{
    public class FromHeaderModel
    {
        [FromHeader]
        public string? Authorization { get; set; }

    }
}
