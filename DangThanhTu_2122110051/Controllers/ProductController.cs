using DangThanhTu_2122110051.Context;
using DangThanhTu_2122110051.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
//using DangThanhTu_2122110051.Models;

namespace DangThanhTu_2122110051.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        AspNetWebEntities db = new AspNetWebEntities();
        public ActionResult Index()
        {
            var products = db.Products.ToList();
            return View(products);
          
        }
        public ActionResult ListingListGrid()
        {
            var products = db.Products.ToList();
            return View(products);
        }


        public ActionResult Detail(int id)
        {
            Product product = db.Products.SingleOrDefault(p => p.Id == id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
      

        //public ActionResult TestDetail()
        //{
        //    List<Product> products = ProductData.GetSampleProducts();
        //    return View(products);
        //}


    }
}