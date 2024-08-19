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
    public class BannerController : Controller
    {
        // GET: Admin/Banner
        AspNetWebEntities db = new AspNetWebEntities();
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstBanner = new List<Banner>();
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
                // Lấy danh sáchBanner theo từ khóa tìm kiếm
                lstBanner = db.Banners.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                // Lấy tất cảBanner trong bảng Banner
                lstBanner = db.Banners.ToList();
            }

            ViewBag.CurrentFilter = SearchString;
            // Số lượng item của 1 trang = 4
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            // Sắp xếp theo idBanner,Banner mới đưa lên đầu
            lstBanner = lstBanner.OrderByDescending(n => n.Id).ToList();
            return View(lstBanner.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Banner objBanner)
        {
            try
            {
                if (objBanner.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objBanner.ImageUpload.FileName);
                    string extension = Path.GetExtension(objBanner.ImageUpload.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;
                    objBanner.Image = fileName;
                    objBanner.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/Banners/"), fileName));
                }

                db.Banners.Add(objBanner);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log error (ex) if needed
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêmBanner.");
                return View(objBanner);
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var objBanner = db.Banners.Where(x => x.Id == id).FirstOrDefault();
            return View(objBanner);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objBanner = db.Banners.Where(x => x.Id == id).FirstOrDefault();
            return View(objBanner);
        }

        [HttpPost]
        public ActionResult Delete(Banner objPro)
        {
            var objBanner = db.Banners.Where(x => x.Id == objPro.Id).FirstOrDefault();
            db.Banners.Remove(objBanner);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objBanner = db.Banners.Where(x => x.Id == id).FirstOrDefault();
            return View(objBanner);
        }

        [HttpPost]
        public ActionResult Edit(int id, Banner objBanner)
        {
            try
            {
                if (objBanner.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objBanner.ImageUpload.FileName);
                    string extension = Path.GetExtension(objBanner.ImageUpload.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;
                    objBanner.Image = fileName;
                    objBanner.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/Banners/"), fileName));
                }

                db.Entry(objBanner).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi chỉnh sửaBanner.");
                // Lấy danh sách cácBanner từ database
                var Banners = db.Banners.ToList();
                ViewBag.Banner_id = new SelectList(Banners, "Id", "Name");

                return View(objBanner);
            }
        }
    }
}