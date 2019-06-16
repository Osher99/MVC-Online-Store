using OsherEx.DAL;
using OsherEx.DAL.Models;
using OsherEx.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace OsherEx.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            var ProductRepo = new ProductRepo();
            ProductRepo.RestoreExpiredProducts();
            List<ProductPartialView> productCards = new List<ProductPartialView>();
            var productList = ProductRepo.GetAllProductsNotInCart();
            foreach (var item in productList)
            {
                ProductPartialView newCard = new ProductPartialView()
                {
                    Id = item.Id,
                    Picture1 = item.Picture1,
                    Picture2 = item.Picture2,
                    Picture3 = item.Picture3,
                    Price = item.Price,
                    ShortDescription = item.ShortDescription,
                    Title = item.Title,
                    PublishedTime = item.PublishedTime,
                    Category = item.Category
                };
                productCards.Add(newCard);
            }
            Session["newList"] = productCards;
            return View(productCards);
        }
        [HttpPost]
        public ActionResult Search(string searchData, Category? category)
        {
            var ProductRepo = new ProductRepo();
            List<ProductPartialView> PorductList = new List<ProductPartialView>();
            var productList = ProductRepo.GetAllProductsNotInCart();
            foreach (var item in productList)
            {
                ProductPartialView newProduct = new ProductPartialView()
                {
                    Id = item.Id,
                    Picture1 = item.Picture1,
                    Picture2 = item.Picture2,
                    Picture3 = item.Picture3,
                    Price = item.Price,
                    ShortDescription = item.ShortDescription,
                    Title = item.Title,
                    PublishedTime = item.PublishedTime,
                    Category = item.Category
                };
                PorductList.Add(newProduct);

            }

            if (searchData != ""  && searchData != null && category == null)
            {
                var searchedList = PorductList.Where(p => p.Title.ToLower().Contains(searchData.ToLower())).ToList();
                Session["newList"] = searchedList;
                return View("Index", searchedList);
            }
            if (searchData != "" && searchData != null && category != null)
            {
                var searchedList = PorductList.Where(p => p.Category == category && p.Title.ToLower().Contains(searchData.ToLower())).ToList();
                Session["newList"] = searchedList;
                return View("Index", searchedList);
            }
            if ((searchData == "" || searchData == null) && category == null)
            {
                Session["newList"] = PorductList;
                return View("Index", PorductList);
            }
            if ((searchData == "" || searchData == null) && category != null)
            {
                var searchedList = PorductList.Where(p => p.Category == category).ToList();
                Session["newList"] = searchedList;
                return View("Index", searchedList);
            }
            Session["newList"] = PorductList;
            return View("Index", PorductList);
        }
        public ActionResult AboutUs()
        {
            return View("AboutUs");
        }

        public ActionResult FilterBy(string filter)
        {
            if (Session["newList"] == null)
            {
                RedirectToAction("Index", "Home");
            }
            var products = (IEnumerable<ProductPartialView>)Session["newList"];
            if (filter == "ByName")
            {
                var byname = products.OrderBy(p => p.Title).ToList();
                return View("Index", byname);
            }

            else if (filter == "LowestPrice")
            {
                var lowest = products.OrderBy(p => p.Price).ToList();
                return View("Index", lowest);
            }

            else if(filter == "HighestPrice")
            {
                var highest = products.OrderByDescending(p => p.Price).ToList();
                return View("Index", highest);
            }

            else if (filter == "Newest")
            {
                var newest = products.OrderByDescending(p => p.PublishedTime).ToList();
                return View("Index", newest);
            }

            else if (filter == "Oldest")
            {
                var oldest = products.OrderBy(p => p.PublishedTime).ToList();
                return View("Index", oldest);
            }
            else
            {
                return View("Index", products);
            }
        }
    }
}
