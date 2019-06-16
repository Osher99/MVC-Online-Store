using OsherEx.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OsherEx.DAL
{
    public class ProductRepo
    {
        public IEnumerable<Product> GetAllProducts()
        {
            using (var dbContext = new SiteContext())
            {
                return dbContext.Products.ToList();
            }
        }


        public IEnumerable<Product> GetAllProductsNotInCart()
        {
            using (var dbContext = new SiteContext())
            {
                return dbContext.Products.Where(p => p.State == State.Available).ToList();
            }
        }


        public void AddProduct(Product product)
        {
            using (var ctx = new SiteContext())
            {
                try
                {
                    ctx.Products.Add(product);
                    ctx.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            string errorMessage = string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }
        }

        public void ChangeToSold(int id)
        {  
            using (var dbContext = new SiteContext())
            {
                var producttoUpdate = dbContext.Set<Product>().Find(id);
                producttoUpdate.SoldTime = DateTime.Now;
                producttoUpdate.State = State.Sold;
                dbContext.SaveChanges();
            }
        }

        public void RestoreExpiredProducts()
        {
            using (var ctx = new SiteContext())
            {
                DateTime ExpiredTime = DateTime.Now.AddMinutes(-5);
                var UnregisteredList = ctx.Products.Where(p => p.CartAddedTime < ExpiredTime && p.State == State.Incart).ToList();
                foreach (var item in UnregisteredList)
                {
                    item.State = State.Available;
                    item.CartAddedTime = null;
                    item.UserId = null;
                }
                ctx.SaveChanges();
            }
        }

        public void RemoveAllProductsFromCart(IEnumerable<Product> Products)
        {
            using (var dbContext = new SiteContext())
            {
                foreach (var product in Products)
                {
                    var producttoUpdate = dbContext.Set<Product>().Find(product.Id);
                    producttoUpdate.CartAddedTime = null;
                    producttoUpdate.State = State.Available;
                    producttoUpdate.UserId = null;
                }
                dbContext.SaveChanges();

            }
        }

        public void RemoveProductFromCart(Product product)
        {
            using (var dbContext = new SiteContext())
            {
                var producttoUpdate = dbContext.Set<Product>().Find(product.Id);
                producttoUpdate.CartAddedTime = null;
                producttoUpdate.State = State.Available;
                producttoUpdate.UserId = null;
                dbContext.SaveChanges();
            }
        }
        public void UpdateProduct(Product product, DateTime dateNow, State state)
        {
            using (var dbContext = new SiteContext())
            {
                var producttoUpdate = dbContext.Set<Product>().Find(product.Id);
                producttoUpdate.CartAddedTime = dateNow;
                producttoUpdate.State = state;
                dbContext.SaveChanges();
            }
        }


        public void UserAddedAProductToCart(Product product, User user)
        {
            using (var dbContext = new SiteContext())
            {
                var producttoUpdate = dbContext.Set<Product>().Find(product.Id);
                var usertoUpdate = dbContext.Set<User>().Find(user.Id);

                producttoUpdate.UserId = usertoUpdate.Id;
                dbContext.SaveChanges();
            }
        }

        public IEnumerable<Product> GetAllProductsInCart(int buyerid)
        {
            using (var dbContext = new SiteContext())
            {
                return dbContext.Products.Where(p => p.State == State.Incart && p.UserId == buyerid).ToList();
            }
        }
    }
}
