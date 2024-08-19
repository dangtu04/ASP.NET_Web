using DangThanhTu_2122110051.Context;
using DangThanhTu_2122110051.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace DangThanhTu_2122110051.Controllers
{
    public class HomeController : Controller
    {
        AspNetWebEntities db = new AspNetWebEntities();

        public ActionResult Index()
        {
            HomeModel model = new HomeModel();
            model.ListProduct = db.Products.ToList();
            model.ListCategory = db.Categories.ToList();
            model.ListBanner = db.Banners.ToList();
            return View(model);
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

        public ActionResult Content()
        {
            return View();
        }

        

    }
}