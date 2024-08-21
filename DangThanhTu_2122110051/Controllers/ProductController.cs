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
using PagedList; 

namespace DangThanhTu_2122110051.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        AspNetWebEntities db = new AspNetWebEntities();
        public ActionResult Index(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 8;

            var products = db.Products.ToList();
            var pagedProducts = products.ToPagedList(pageNumber, pageSize);

            ViewBag.TotalItems = products.Count;

            return View(pagedProducts);
        }


        public ActionResult ListingListGrid(int? page)
    {
        int pageSize = 8; // số sản phẩm trên mỗi trang
        int pageNumber = (page ?? 1); // số trang hiện tại, mặc định là trang 1 nếu không có trang nào được chỉ định

        // Lấy danh sách sản phẩm từ cơ sở dữ liệu
        var products = db.Products.OrderBy(p => p.Name).ToList(); 

        // Phân trang sản phẩm
        var pagedProducts = products.ToPagedList(pageNumber, pageSize);

        // Trả về View với danh sách sản phẩm phân trang
        return View(pagedProducts);
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