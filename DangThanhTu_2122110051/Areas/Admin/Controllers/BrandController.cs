using DangThanhTu_2122110051.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DangThanhTu_2122110051.Areas.Admin.Controllers
{
    public class BrandController : Controller
    {
        // GET: Admin/Brand
        AspNetWebEntities db = new AspNetWebEntities();
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstBrand = new List<Brand>();
            if (SearchString != null)
            {
                page = 1;
            }
            else
            {
                SearchString = currentFilter;
            }

            if (!string.IsNullOrEmpty(SearchString))
            {
                // Lấy danh sách thương hiệu theo từ khóa tìm kiếm
                lstBrand = db.Brands.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                // Lấy tất cả thương hiệu trong bảng Brand
                lstBrand = db.Brands.ToList();
            }

            ViewBag.CurrentFilter = SearchString;
            // Số lượng item của 1 trang = 4
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            // Sắp xếp theo id thương hiệu, thương hiệu mới đưa lên đầu
            lstBrand = lstBrand.OrderByDescending(n => n.Id).ToList();
            return View(lstBrand.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult Create()
        {
            // Lấy danh sách các thương hiệu từ database
            //var brands = db.Brands.ToList();
            return View();
        }

        [HttpPost]
        public ActionResult Create(Brand objBrand)
        {
            try
            {
                if (objBrand.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpload.FileName);
                    string extension = Path.GetExtension(objBrand.ImageUpload.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;
                    objBrand.Image = fileName;
                    objBrand.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/brands/"), fileName));
                }

                db.Brands.Add(objBrand);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log error (ex) if needed
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm thương hiệu.");
                return View(objBrand);
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var objBrand = db.Brands.Where(x => x.Id == id).FirstOrDefault();
            return View(objBrand);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objBrand = db.Brands.Where(x => x.Id == id).FirstOrDefault();
            return View(objBrand);
        }

        [HttpPost]
        public ActionResult Delete(Brand objPro)
        {
            var objBrand = db.Brands.Where(x => x.Id == objPro.Id).FirstOrDefault();
            db.Brands.Remove(objBrand);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objBrand = db.Brands.Where(x => x.Id == id).FirstOrDefault();
            return View(objBrand);
        }

        [HttpPost]
        public ActionResult Edit(int id, Brand objBrand)
        {
            try
            {
                if (objBrand.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objBrand.ImageUpload.FileName);
                    string extension = Path.GetExtension(objBrand.ImageUpload.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;
                    objBrand.Image = fileName;
                    objBrand.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/brands/"), fileName));
                }

                db.Entry(objBrand).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi chỉnh sửa thương hiệu.");
                // Lấy danh sách các thương hiệu từ database
                var brands = db.Brands.ToList();
                ViewBag.Brand_id = new SelectList(brands, "Id", "Name");

                return View(objBrand);
            }
        }

    }
}