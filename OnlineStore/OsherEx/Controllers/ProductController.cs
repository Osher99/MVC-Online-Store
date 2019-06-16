using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OsherEx.DAL;
using OsherEx.DAL.Models;
using OsherEx.Models;

namespace OsherEx.Controllers
{
    public class ProductController : Controller
    {

        public User GetUser()
        {
            var userRepo = new UserRepo();
            var usersList = userRepo.GetAllUsers();
            var usertoUpdate = usersList.FirstOrDefault(u => u.UserName == Request.Cookies["user"]["UserName"]);
            if (usertoUpdate == null)
                return null;
            return usertoUpdate;
        }
        public Product GetProduct(int? id)
        {
            var ProductRepo = new ProductRepo();
            var productList = ProductRepo.GetAllProducts();
            var product = productList.FirstOrDefault(p => p.Id == (int)id);
            if (product == null)
                return null;
            return product;
        }

        public ActionResult BuyNow()
        {
            var ProductRepo = new ProductRepo();
            if (Request.Cookies["user"] == null)
            {
                var listofcart = (List<Product>)Session["Cart"];
                foreach (var item in listofcart)
                {
                    ProductRepo.ChangeToSold(item.Id);
                }
                TempData["message"] = "<script>alert('Thank your for buying at OsherBuy4You, your Parcel will come within 2 weeks!');</script>";
                return RedirectToAction("index", "Home");

            }
            else
            {
                var listofcart = ProductRepo.GetAllProductsInCart(GetUser().Id);
                foreach (var item in listofcart)
                {
                    ProductRepo.ChangeToSold(item.Id);
                }
                TempData["message"] = "<script>alert('Thank your for buying at OsherBuy4You, your Parcel will come within 2 weeks!');</script>";
                return RedirectToAction("index", "Home");
            }
        }

        public ActionResult AddToCart(int? id)
        {
            if (id.HasValue == false || id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var ProductRepo = new ProductRepo();

            var foundProduct = GetProduct(id);
            if (foundProduct == null || foundProduct.State == State.Incart || foundProduct.State == State.Sold || foundProduct.UserId != null)
            {
                TempData["message"] = "<script>alert('The item has been either sold or in cart, check again later!');</script>";
                return RedirectToAction("Index", "Home");
            }
            ProductRepo.UpdateProduct(foundProduct, DateTime.Now, State.Incart);
            if (Request.Cookies["user"] != null)
            {
                ProductRepo.UserAddedAProductToCart(foundProduct, GetUser());
            }
            else
            {
                if (Session["Cart"] == null)
                {
                    Session["Cart"] = new List<Product>();
                }
                ((List<Product>)Session["Cart"]).Add(GetProduct(id));
                ProductRepo.UpdateProduct(GetProduct(id), DateTime.Now, State.Incart);
            }
            TempData["message"] = "<script>alert('The item is added to your cart successfuly');</script>";
            return RedirectToAction("Index", "Home");

        }


        public ActionResult MyCart()
        {
            if (Request.Cookies["user"] != null)
            {
                ProductRepo rep = new ProductRepo();
                MyCartModel cart = new MyCartModel();
                cart.Products = rep.GetAllProductsInCart(GetUser().Id);
                double sum = 0;
                foreach (var item in cart.Products)
                {
                    sum += item.Price;
                }
                cart.FullPrice = sum - (sum / 10);
                return View("MyCart", cart);
            }
            else
            {
                if (Session["Cart"] == null)
                {
                    return View("MyCart");
                }
                MyCartModel cart = new MyCartModel();
                cart.Products = (List<Product>)Session["Cart"];
                double sum = 0;
                foreach (var item in cart.Products)
                {
                    sum += item.Price;
                }
                cart.FullPrice = sum;
                return View("MyCart", cart);
            }
        }

        public ActionResult DeleteItemFromCart(int? id)
        {
            if (id.HasValue == false || id == null)
            {
                return RedirectToAction("Index", "Home");
            }
            if (Request.Cookies["user"] != null)
            {
                var ProductRepo = new ProductRepo();
                ProductRepo.RemoveProductFromCart(GetProduct(id));
                TempData["message"] = "<script>alert('The item has been deleted from your cart');</script>";
                return RedirectToAction("MyCart", "Product");
            }
            else
            {
                var ProductRepo = new ProductRepo();
                ((List<Product>)Session["Cart"]).RemoveAll(p => p.Id == id);
                ProductRepo.RemoveProductFromCart(GetProduct(id));
                TempData["message"] = "<script>alert('The item has been deleted from your cart');</script>";
                return RedirectToAction("MyCart", "Product");
            }
        }

        public ActionResult Advertise()
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("Advertise");
        }

        public void AddCookie(string userName)
        {
            HttpCookie myCookie = new HttpCookie("user");
            myCookie["UserName"] = userName;
            myCookie.Expires = DateTime.Now.AddDays(10);
            Response.Cookies.Add(myCookie);
        }

        public ActionResult DeleteCookie()
        {
            HttpCookie myCookie = new HttpCookie("user");
            myCookie.Expires = DateTime.Now.AddDays(-1);
            Response.Cookies.Add(myCookie);
            ViewBag.Titel = "Home";
            return RedirectToAction("Index", "Home");
        }


        public ActionResult ProductDetails(int? id)
        {
            if (id.HasValue == false)
            {
                return RedirectToAction("Index", "Home");
            }

            UserRepo userRepo = new UserRepo();
            var foundProduct = GetProduct(id);
            if (foundProduct == null)
                return RedirectToAction("Index", "Home");
            var userList = userRepo.GetAllUsers();
            var foundOwner = userList.FirstOrDefault(u => u.Id == foundProduct.OwnerId);
            if (foundOwner == null)
                return RedirectToAction("Index", "Home");
            ProductDetailsModel details = new ProductDetailsModel()
            {
                Id = foundProduct.Id,
                LongDescription = foundProduct.LongDescription,
                ShortDescription = foundProduct.ShortDescription,
                Email = foundOwner.Email,
                PublishedTime = foundProduct.PublishedTime,
                FirstName = foundOwner.FirstName,
                LastName = foundOwner.LastName,
                UserName = foundOwner.UserName,
                BirthDate = foundOwner.BirthDate,
                Title = foundProduct.Title,
                Picture1 = foundProduct.Picture1,
                Picture2 = foundProduct.Picture2,
                Picture3 = foundProduct.Picture3,
                Price = foundProduct.Price,
                State = foundProduct.State,
                Category = foundProduct.Category
            };
            return View("ShowDetails", details);
        }
        [HttpPost]
        public ActionResult AddNewProduct(ProductModel newProduct, HttpPostedFileBase image1, HttpPostedFileBase image2, HttpPostedFileBase image3)
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ProductRepo rep = new ProductRepo();
            User user = GetUser();
            if (ModelState.IsValid)
            {
                Product AddedProduct = new Product
                {
                    OwnerId = user.Id,
                    State = State.Available,
                    PublishedTime = DateTime.Now,
                    ShortDescription = newProduct.ShortDescription,
                    LongDescription = newProduct.LongDescription,
                    Title = newProduct.Title,
                    Price = newProduct.Price,
                    Category = newProduct.Category
                };

                if (image1 != null)
                {
                    AddedProduct.Picture1 = new byte[image1.ContentLength];
                    image1.InputStream.Read(AddedProduct.Picture1, 0, image1.ContentLength);
                }
                if (image2 != null)
                {
                    AddedProduct.Picture2 = new byte[image2.ContentLength];
                    image2.InputStream.Read(AddedProduct.Picture2, 0, image2.ContentLength);
                }
                if (image3 != null)
                {
                    AddedProduct.Picture3 = new byte[image3.ContentLength];
                    image3.InputStream.Read(AddedProduct.Picture3, 0, image3.ContentLength);
                }

                rep.AddProduct(AddedProduct);
                TempData["message"] = "<script>alert('Product Added successfuly!');</script>";
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["message"] = "<script>alert('Please check your text boxes');</script>";
                return View("Advertise");
            }

        }

        public ActionResult RemoveAll()
        {
            if (Request.Cookies["user"] != null)
            {
                ProductRepo rep = new ProductRepo();
                rep.RemoveAllProductsFromCart(rep.GetAllProductsInCart(GetUser().Id));
                return RedirectToAction("MyCart", "Product");
            }
            else
            {
                ProductRepo rep = new ProductRepo();

                rep.RemoveAllProductsFromCart((List<Product>)Session["Cart"]);
                Session["Cart"] = null;
                return RedirectToAction("MyCart", "Product");
            }
        }
    }
}