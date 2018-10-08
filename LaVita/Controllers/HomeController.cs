using LaVita.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace LaVita.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
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
                    Session["ID"] = userDetails.UserID;
                    return RedirectToAction("ViewProducts");
                }
            }
        }
        public ActionResult GetPassword(User userModel)
        {
            using (LaVitaEntities db = new LaVitaEntities())
            {
                var userDetails = db.Users.Where(x => x.Username == userModel.Username).FirstOrDefault();
                if (userDetails != null)
                {
                    ViewBag.Message = "Your password is as follows: " + userDetails.Password + "\nYou will be redirected soon back to the index page. :)";
                }
                Thread.Sleep(4000);
                return View("Index");
            }
        }

        [HttpPost]
        public object AddNewUser(User userModel)
        {
            Random rnd = new Random();
            using (LaVitaEntities db = new LaVitaEntities())
            {
                userModel.UserID = rnd.Next(1, 1000);
                db.Users.Add(userModel);
                db.SaveChanges();
            }
            return "Saved";
        }

        public ActionResult ViewProducts()
        {
            LaVitaEntities db = new LaVitaEntities();
            ViewBag.Data = db.Products.ToList();
            return View("ViewProducts");
        }


        public ActionResult MyCart(string UserID)
        {
            using (LaVitaEntities db = new LaVitaEntities())
            {
                var carts = db.Carts.SqlQuery("Select * from Cart c Join Products p on p.ProductID = c.ProductId Where c.UserId ="+int.Parse(UserID)).ToList();
                List<object> tempList = new List<object>();
                int sum = 0;
                List<int> cartID = new List<int>();
                for (int i = 0; i < carts.Count; i++)
                {
                    var prodKey = carts[i].ProductId;
                    var allProducts = db.Products.SqlQuery("Select * From Products p Where p.ProductID =" + prodKey).FirstOrDefault();
                    cartID.Add(carts[i].CartID);
                    sum += int.Parse(allProducts.Price);
                    tempList.Add(allProducts);
                }
                ViewBag.Carts = carts;
                ViewBag.Data = tempList;
                ViewBag.OrderTotal = sum;
                
                return View();
            }
        }



        public ActionResult Register()
        {
            return View();
        }
        public ActionResult Forgot()
        {
            return View();
        }

        public ActionResult Receipt()
        {
            return View();
        }
        public void AddToCart(string UserID, string ProductID)
        {
            Cart addNewItem = new Cart();
            Random random = new Random();
            using (LaVitaEntities db = new LaVitaEntities())
            {
                addNewItem.CartID = random.Next(1, 5000);
                addNewItem.UserId = int.Parse(UserID);
                addNewItem.ProductId = int.Parse(ProductID);
                db.Carts.Add(addNewItem);
                db.SaveChanges();
            }
            RedirectToAction("ViewProducts");
        }
        public ActionResult Delete(int CartID)
        {
            using (LaVitaEntities db = new LaVitaEntities())
            {
                db.Carts.SqlQuery("DELETE FROM Carts c Where c.CartID="+CartID);
                db.SaveChanges();
            }
            return View("MyCart");
        }

    }
}