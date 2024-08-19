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
    public class UserController : Controller
    {
        // GET: Admin/User
        AspNetWebEntities db = new AspNetWebEntities();
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstUser = new List<User>();
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
                // Lấy danh sách người dùng theo từ khóa tìm kiếm
                lstUser = db.Users.Where(n => n.LastName.Contains(SearchString)).ToList();
            }
            else
            {
                // Lấy tất cả người dùng trong bảng User
                lstUser = db.Users.ToList();
            }

            ViewBag.CurrentFilter = SearchString;
            // Số lượng item của 1 trang = 4
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            // Sắp xếp theo id người dùng, người dùng mới đưa lên đầu
            lstUser = lstUser.OrderByDescending(n => n.Id).ToList();
            return View(lstUser.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(User objUser)
        {
            try
            {
                if (objUser.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objUser.ImageUpload.FileName);
                    string extension = Path.GetExtension(objUser.ImageUpload.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;
                    objUser.Image = fileName;
                    objUser.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/users/"), fileName));
                }

                db.Users.Add(objUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log error (ex) if needed
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm người dùng.");
                return View(objUser);
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var objUser = db.Users.Where(x => x.Id == id).FirstOrDefault();
            return View(objUser);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objUser = db.Users.Where(x => x.Id == id).FirstOrDefault();
            return View(objUser);
        }

        [HttpPost]
        public ActionResult Delete(User objPro)
        {
            var objUser = db.Users.Where(x => x.Id == objPro.Id).FirstOrDefault();
            db.Users.Remove(objUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objUser = db.Users.Where(x => x.Id == id).FirstOrDefault();
            return View(objUser);
        }

        [HttpPost]
        public ActionResult Edit(int id, User objUser)
        {
            try
            {
                if (objUser.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objUser.ImageUpload.FileName);
                    string extension = Path.GetExtension(objUser.ImageUpload.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;
                    objUser.Image = fileName;
                    objUser.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/users/"), fileName));
                }

                db.Entry(objUser).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi chỉnh sửa người dùng.");
                // Lấy danh sách các người dùng từ database
                var Users = db.Users.ToList();
                ViewBag.User_id = new SelectList(Users, "Id", "Name");

                return View(objUser);
            }
        }
    }
}