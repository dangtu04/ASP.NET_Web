using DangThanhTu_2122110051.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DangThanhTu_2122110051.Areas.Admin.Controllers
{
    public class AdCategoryController : Controller
    {
        // GET: Admin/AdCategory
        AspNetWebEntities db = new AspNetWebEntities();
        public ActionResult Index()
        {
            var listCategory = db.Categories.ToList();
            return View(listCategory);
        }


        [HttpGet]
        public ActionResult Create()
        {
            // Lấy danh sách các danh mục từ database
            var categories = db.Categories.ToList();

            categories.Insert(0, new Category { Id = 0, Name = "Không có" });
            ViewBag.Parent_id = new SelectList(categories, "Id", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult Create(Category objCategory)
        {
            try
            {
                if (objCategory.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpload.FileName);
                    string extension = Path.GetExtension(objCategory.ImageUpload.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;
                    objCategory.Image = fileName;
                    objCategory.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/categories/"), fileName));
                }

                // Nếu người dùng chọn "Không có", Parent_id sẽ là null
                if (objCategory.Parent_id == 0)
                {
                    objCategory.Parent_id = null;
                }

                Category category = db.Categories.Add(objCategory);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log error (ex) if needed
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm danh mục.");
                return View(objCategory);
            }
        }



        [HttpGet]
        public ActionResult Details(int id)
        {
            var objCategory = db.Categories.Where(x => x.Id == id).FirstOrDefault();
            return View(objCategory);
        }



        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objCategory = db.Categories.Where(x => x.Id == id).FirstOrDefault();
            return View(objCategory);
        }

        [HttpPost]
        public ActionResult Delete(Category objCate)
        {
            var objCategory = db.Categories.Where(x => x.Id == objCate.Id).FirstOrDefault();

            if (objCategory == null)
            {
                TempData["ErrorMessage"] = "Danh mục không tồn tại.";
                return RedirectToAction("Index");
            }

            var childCategories = db.Categories.Where(x => x.Parent_id == objCate.Id).ToList();
            foreach (var child in childCategories)
            {
                child.Parent_id = null;
            }
            db.SaveChanges();

            var hasChildCategories = db.Categories.Any(x => x.Parent_id == objCate.Id);
            if (hasChildCategories)
            {
                ViewBag.ErrorMessage = "Không thể xóa danh mục này vì có danh mục khác đang phụ thuộc vào nó.";
                return View(objCategory);
            }

            db.Categories.Remove(objCategory);
            db.SaveChanges();
            return RedirectToAction("Index");
        }



        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objCategory = db.Categories.Where(x => x.Id == id).FirstOrDefault();

            // Lấy danh sách các danh mục từ database
            var categories = db.Categories.ToList();
            ViewBag.Parent_id = new SelectList(categories, "Id", "Name");


            return View(objCategory);
        }

        [HttpPost]
        public ActionResult Edit(int id, Category objCategory)
        {
            try
            {
                if (objCategory.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objCategory.ImageUpload.FileName);
                    string extension = Path.GetExtension(objCategory.ImageUpload.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;
                    objCategory.Image = fileName;
                    objCategory.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/categories/"), fileName));
                }
                // Nếu người dùng chọn "Không có", Parent_id sẽ là null
                if (objCategory.Parent_id == 0)
                {
                    objCategory.Parent_id = null;
                }
                db.Entry(objCategory).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi chỉnh sửa danh mục.");

                // Lấy danh sách các danh mục từ database
                var categories = db.Categories.ToList();
                ViewBag.Parent_id = new SelectList(categories, "Id", "Name");


                return View(objCategory);
            }
        }


    }
}