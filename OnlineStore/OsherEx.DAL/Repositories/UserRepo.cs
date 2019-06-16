using OsherEx.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace OsherEx.DAL
{
    public class UserRepo
    {
        public IEnumerable<User> GetAllUsers()
        {
            using (var dbContext = new SiteContext())
            {
                return dbContext.Users.ToList();
            }
        }



        public void AddtoDatabase(User user, string password)
        {
            try
            {
                using (var dbContext = new SiteContext())
                {
                    user.Password = SHA256Hash(password);
                    user.OldPassword = SHA256Hash(password);
                    dbContext.Users.Add(user);
                    dbContext.SaveChanges();
                }
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
        public void UpdateInfo(User userToUpdate, string firstName, string lastName, DateTime birthDay, string eMail)
        {
            using (var dbContext = new SiteContext())
            {
                try
                {
                    var user = dbContext.Set<User>().Find(userToUpdate.Id);
                    user.FirstName = firstName;
                    user.LastName = lastName;
                    user.BirthDate = birthDay;
                    user.Email = eMail;
                    dbContext.SaveChanges();
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

        public void UpdatePW(User userToUpdate, string password)
        {
            using (var dbContext = new SiteContext())
            {
                try
                {
                    var user = dbContext.Set<User>().Find(userToUpdate.Id);
                    user.Password = SHA256Hash(password);
                    user.OldPassword = SHA256Hash(password);
                    dbContext.SaveChanges();
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



        public bool CheckOldPassword(string oldPassword, User thisUser)
        {
            if (SHA256Hash(oldPassword) != thisUser.Password)
            {
                return false;
            }
            return true;
        }


        public static string SHA256Hash(string Data)
        {
            SHA256 sha = new SHA256Managed();
            byte[] hash = sha.ComputeHash(Encoding.ASCII.GetBytes(Data));
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hash)
            {
                stringBuilder.AppendFormat("{0:x2}", b);
            }
            return stringBuilder.ToString();
        }

        public User SignIn(string userName, string passWord)
        {
            using (var ctx = new SiteContext())
            {
                string hashPass = SHA256Hash(passWord);
                var foundUser = ctx.Users.Where(x => x.UserName.ToLower() == userName && x.Password == hashPass).FirstOrDefault();
                if (foundUser != null)
                    return foundUser;
                return null;
            }
            
        }
    }
}