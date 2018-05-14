using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.Linq;
namespace E_Commerce_Site.Models
{
    public class BrandViewModel
    {
        public BrandViewModel() { }
        public string BrandName { get; set; }
        public int Id { get; set; }
        public List<Brand> Brands { get; set; }
        public IEnumerable<SelectListItem> GetBrands()
        {
            return Brands.Select(brand => new SelectListItem { Text = brand.Name, Value = brand.Id.ToString() });
        }
    }
}