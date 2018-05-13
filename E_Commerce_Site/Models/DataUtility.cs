using System;
using System.Collections.Generic;
using System.Linq;
namespace E_Commerce_Site.Models
{
    public class DataUtility
    {
        private AppDbContext _db;
        dynamic objectJson; // an element that is typed as dynamic is assumed to support any operation
        public DataUtility(AppDbContext context)
        {
            _db = context;
        }
        public bool loadVideoGameInfoFromWebToDb(string stringJson)
        {
            bool brandsLoaded = false;
            bool productsLoaded = false;

            try
            {
                objectJson = Newtonsoft.Json.JsonConvert.DeserializeObject(stringJson);
                brandsLoaded = loadBrands();
                productsLoaded = loadProducts();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return brandsLoaded && productsLoaded;
        }



        private bool loadBrands()
        {
            bool loadedBrands = false;
            try
            {
                // clear out the old rows
                _db.Brands.RemoveRange(_db.Brands);
                _db.SaveChanges();
                List<String> allBrands = new List<String>();
                foreach (var node in objectJson)
                {
                    allBrands.Add(Convert.ToString(node["BRAND"]));
                }
                // distinct will remove duplicates before we insert them into the db
                IEnumerable<String> brands = allBrands.Distinct<String>();
                foreach (string brand in brands)
                {
                    Brand brd = new Brand();
                    brd.Name = brand;
                    _db.Brands.Add(brd);
                    _db.SaveChanges();
                }
                loadedBrands = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }
            return loadedBrands;
        }


        private bool loadProducts()
        {
            bool loadedProducts = false;
            try
            {
                List<Brand> brands = _db.Brands.ToList();
                // clear out the old
                _db.Products.RemoveRange(_db.Products);
                _db.SaveChanges();
                foreach (var node in objectJson)
                {
                    Product item = new Product();
                    item.Id = Convert.ToString(node["PRODUCT"].Value);
                    item.ProductName = Convert.ToString(node["ProductName"].Value);
                    item.GraphicName = Convert.ToString(node["GraphicName"].Value);
                    item.CostPrice = Convert.ToDecimal(node["CostPrice"].Value);
                    item.MSRP = Convert.ToDecimal(node["MSRP"].Value);
                    item.QtyOnHand = Convert.ToInt32(node["QtyOnHand"].Value);
                    item.QtyOnBackOrder = Convert.ToInt32(node["QtyOnBackOrder"].Value);

                    item.Description = Convert.ToString(node["Description"].Value);
                    string brand = Convert.ToString(node["BRAND"].Value);

                    // add the FK here
                    foreach (Brand brnd in brands)
                    {
                        if (brnd.Name == brand)
                        {
                            item.Brand = brnd;
                            item.BrandId = brnd.Id;
                        }
                    }
                    _db.Products.Add(item);
                    _db.SaveChanges();
                }
               
                loadedProducts = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error - " + ex.Message);
            }
            return loadedProducts;
        }
    }
}