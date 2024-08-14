using DangThanhTu_2122110051.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DangThanhTu_2122110051.Controllers
{
    public class CategoryController : Controller
    {
        // GET: Category

        AspNetWebEntities db = new AspNetWebEntities();
        public ActionResult Index()
        {
            var listCategory = db.Categories.ToList();
            return View(listCategory);
        }

        public ActionResult ProductByCategoryId(int id)
        {
            var ListProduct = db.Products.Where(n=>n.Category_id == id).ToList();
            return View(ListProduct);
        }   
    }
}