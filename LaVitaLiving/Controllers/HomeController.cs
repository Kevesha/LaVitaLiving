
using LaVitaLiving.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LaVitaLiving.Controllers
{
    public class HomeController : Controller
    {

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Autherize(User userModel)
        {
            using (LaVitaEntities db = new LaVitaEntities())
            {
                var userDetails = db.Users.Where(x => x.Username == userModel.Username && x.Password == userModel.Password).FirstOrDefault();
              
                if (userDetails == null)
                {
                    return View("Index", userModel);
                }
                else
                {
                    Session["userId"] = userDetails.UserId;
                    Session["userName"] = userDetails.Username;
                    Session["password"] = userDetails.Password;
                    return RedirectToAction("Products");
                }
            }
        }

        [HttpPost]
        public ActionResult AddNewUser(User userModel)
        {
            Random rnd = new Random();
            using (LaVitaEntities db = new LaVitaEntities())
            {
                userModel.UserId = rnd.Next(1, 10000000);
                db.Users.Add(userModel);
                db.SaveChanges();
            }
            return RedirectToAction("Saved");

        }

        public ActionResult Saved()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        public string GetPassword(User userModel)
        {
            using (LaVitaEntities db = new LaVitaEntities())
            {
                var userDetails = db.Users.Where(x => x.Username == userModel.Username).FirstOrDefault();
                if (userDetails != null)
                {
                    ViewBag.Message = $"Your password is as follows:\n {userDetails.Password}";
                }
                return ViewBag.Message;
            }
        }

        public ActionResult Products()
        {
            return View();
        }

        [Route("Home/View")]
        [HttpGet]
        public ActionResult ViewProducts()
        {
            
        }
        [Route("Home/Add")]
        [HttpPost]
        public ActionResult AddProducts()
        {
            var x = "doubt";
            //LaVitaEntities db = new LaVitaEntities();
            //var query = $"Select * from [dbo].[Products] products where products.UserId = {UserID}";
            //db.
            return View();
        }
        
    }
}