using BigFatDiary.Models.Data;
using BigFatDiary.Models.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BigFatDiary.Controllers
{
    [Authorize(Roles = "Administrator,Moderator,User")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return RedirectToAction("Diary", "User");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}