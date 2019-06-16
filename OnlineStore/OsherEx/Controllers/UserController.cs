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
    public class UserController : Controller
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

        public ActionResult ChangePassword()
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("ChangePassword");
        }
        public ActionResult UpdatePw(UserModel user)
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            var thisUser = GetUser();
            if (thisUser == null)
                return RedirectToAction("Index", "Home");
            var userRepo = new UserRepo();

            if (user.OldPassword == null || user.Password == null || user.ConfirmPassowrd == null)
            {
                TempData["message"] = "<script>alert('Please fill all the details!');</script>";
                return View("ChangePassword");
            }
            if (userRepo.CheckOldPassword(user.OldPassword, thisUser) == false)
            {
                TempData["message"] = "<script>alert('Old password is not correct!');</script>";
                return View("ChangePassword");
            }
            if (user.Password != user.ConfirmPassowrd)
            {
                TempData["message"] = "<script>alert('New Passwords are not matched!');</script>";
                return View("ChangePassword");
            }
            if (ModelState.IsValidField("OldPassword") && ModelState.IsValidField("Password") && ModelState.IsValidField("ConfirmPassowrd"))
            {
                userRepo.UpdatePW(thisUser, user.Password);
                TempData["message"] = "<script>alert('Password updated successfuly!');</script>";
                TempData["message"] = "<script>alert('Please log in with your new Password!');</script>";
                DeleteCookie();
                return RedirectToAction("Index", "Home");
            }
            TempData["message"] = "<script>alert('One of the passwords is invailed!');</script>";
            return View("ChangePassword");
        }

        public ActionResult Registery()
        {
            if (Request.Cookies["user"] != null)
            {
                return RedirectToAction("Index", "Home");
            }
            return View("Registery");
        }
        public ActionResult UpdateProfile()
        {
            if (Request.Cookies["user"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            User user = GetUser();
            UserModel m = new UserModel()
            {
                BirthDate = user.BirthDate,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Password = user.Password,
                OldPassword = user.OldPassword,
                Email = user.Email
            };
            if (user == null)
                return RedirectToAction("Index", "Home");
            return View("UpdateProfile", m);
        }


        public ActionResult Signin(LogInModel account)
        {
            var userRepo = new UserRepo();
            var Userlist = userRepo.GetAllUsers();

            if (ModelState.IsValid)
            {
                if (account.UserName != null && account.Password != null)
                {
                    var user = userRepo.SignIn(account.UserName.ToLower(), account.Password);
                    if (user != null)
                    {
                        AddCookie(user.UserName);
                        TempData["message"] = "<script>alert('Welcome!');</script>";
                        return RedirectToAction("Index", "Home");
                    }
                    TempData["message"] = "<script>alert('User name or Password is not correct');</script>";
                    return RedirectToAction("Index", "Home");
                }
                TempData["message"] = "<script>alert('User name or Password is not correct');</script>";
                return RedirectToAction("Index", "Home");
            }
            TempData["message"] = "<script>alert('User name or Password is not correct');</script>";
            return RedirectToAction("Index", "Home");
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

        public ActionResult UserToUpdate(UserModel user)
        {
            if (ModelState.IsValidField("FirstName") && ModelState.IsValidField("LastName") && ModelState.IsValidField("BirthDate") && ModelState.IsValidField("Email"))
            {
                if (CheckEmail(user.Email))
                {
                    if (user.BirthDate > DateTime.Now || user.BirthDate.Year < 1900)
                    {
                        TempData["message"] = "<script>alert('Date is not vaild!');</script>";
                        return RedirectToAction("UpdateProfile", "User");
                    }
                    var userRepo = new UserRepo();
                    var usertoUpdate = GetUser();
                    if (usertoUpdate == null)
                        return RedirectToAction("Index", "Home");
                    userRepo.UpdateInfo(usertoUpdate, user.FirstName, user.LastName, user.BirthDate, user.Email);
                    TempData["message"] = "<script>alert('Profile updated successfuly!');</script>";
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    TempData["message"] = "<script>alert('Email Is Already Exist!');</script>";
                    return RedirectToAction("UpdateProfile", "User");
                }
            }
            else
            {
                TempData["message"] = "<script>alert('Please fill the details correctly!');</script>";
                return RedirectToAction("UpdateProfile", "User");
            }
        }

        public ActionResult UserToRegister(UserModel user)
        {
            if (Request.Cookies["user"] != null)
            {
                RedirectToAction("Index", "Home");
            }
            if (user.Password == user.ConfirmPassowrd && ModelState.IsValid)
            {
                if (CheckUserName(user.UserName))
                {
                    if (CheckEmail(user.Email))
                    {
                        if (user.BirthDate > DateTime.Now || user.BirthDate.Year < 1900)
                        {
                            TempData["message"] = "<script>alert('Date is not vaild!');</script>";
                            return View("Registery", user);
                        }
                        User newUser = new User()
                        {
                            UserName = user.UserName,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            BirthDate = user.BirthDate,
                            Email = user.Email,
                            Password = user.Password,
                            OldPassword = user.Password
                        };
                        UserRepo Rep = new UserRepo();
                        Rep.AddtoDatabase(newUser, user.Password);

                        TempData["message"] = "<script>alert('Register succesfully');</script>";
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["message"] = "<script>alert('Email already exists!');</script>";
                        return View("Registery", user);
                    }
                }
                else
                {
                    TempData["message"] = "<script>alert('User Name already exists!');</script>";
                    return View("Registery", user);
                }
            }
            else
            {
                TempData["message"] = "<script>alert('Register failed, try again');</script>";
                return View("Registery", user);
            }
        }
        public bool CheckUserName(string userName)
        {
            var userRepo = new UserRepo();
            var userList = userRepo.GetAllUsers();
            var usertoUpdate = userList.FirstOrDefault(u => u.UserName == userName);
            if (usertoUpdate == null)
                return true;
            else
                return false;
        }
        public bool CheckEmail(string email)
        {
            var userRepo = new UserRepo();
            var userList = userRepo.GetAllUsers();
            var usertoUpdate = userList.FirstOrDefault(u => u.Email == email);
            if (usertoUpdate == null)
                return true;
            else
                return false;
        }

    }
}