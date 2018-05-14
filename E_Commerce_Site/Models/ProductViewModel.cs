using System.Collections.Generic;
namespace E_Commerce_Site.Models
{
    public class ProductViewModel
    {
        public string BrandName { get; set; }
        public IEnumerable<Product> Products { get; set; }
    }
}