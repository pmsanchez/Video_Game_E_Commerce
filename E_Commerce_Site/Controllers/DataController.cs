using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using E_Commerce_Site.Models;

namespace E_Commerce_Site.Controllers
{
    public class DataController : Controller
    {


        AppDbContext _ctx;
        public DataController(AppDbContext context)
        {
            _ctx = context;
        }


        public async Task<IActionResult> Index()
        {
            DataUtility util = new DataUtility(_ctx);
            string msg = "";
            var json = await getVideoGameJsonFromWeb();
            try
            {
                msg = (util.loadVideoGameInfoFromWebToDb(json)) ? "tables loaded" : "problem loading tables";
            }
            catch (Exception ex)
            {
                msg = ex.Message;
            }
            ViewData["msg"] = msg;
            return View();
        }

        private async Task<String> getVideoGameJsonFromWeb()
        {
            string url = "https://api.myjson.com/bins/1d329y";
            var httpClient = new HttpClient();
            var response = await httpClient.GetAsync(url);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }


    }
}