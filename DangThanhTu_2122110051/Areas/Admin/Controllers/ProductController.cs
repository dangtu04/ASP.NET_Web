﻿using DangThanhTu_2122110051.Context;
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
    public class ProductController : Controller
    {
        AspNetWebEntities db = new AspNetWebEntities();

        // GET: Admin/AdProduct
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstProduct = new List<Product>();
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
                // Lấy danh sách sản phẩm theo từ khóa tìm kiếm
                lstProduct = db.Products.Where(n => n.Name.Contains(SearchString)).ToList();
            }
            else
            {
                // Lấy tất cả sản phẩm trong bảng product
                lstProduct = db.Products.ToList();
            }

            ViewBag.CurrentFilter = SearchString;
            // Số lượng item của 1 trang = 4
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            // Sắp xếp theo id sản phẩm, sản phẩm mới đưa lên đầu
            lstProduct = lstProduct.OrderByDescending(n => n.Id).ToList();
            return View(lstProduct.ToPagedList(pageNumber, pageSize));
        }


        [HttpGet]
        public ActionResult Create()
        {
            // Lấy danh sách các danh mục từ database
            var categories = db.Categories.ToList();
            ViewBag.Category_id = new SelectList(categories, "Id", "Name");

            // Lấy danh sách các thương hiệu từ database
            var brands = db.Brands.ToList();
            ViewBag.Brand_id = new SelectList(brands, "Id", "Name");

            return View();
        }

        [HttpPost]
        public ActionResult Create(Product objProduct)
        {
            try
            {
                if (objProduct.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                    string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;
                    objProduct.Image = fileName;
                    objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));
                }

                db.Products.Add(objProduct);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Log error (ex) if needed
                ModelState.AddModelError("", "Có lỗi xảy ra khi thêm sản phẩm.");
                return View(objProduct);
            }
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var objProduct = db.Products.Where(x => x.Id == id).FirstOrDefault();
            return View(objProduct);
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objProduct = db.Products.Where(x => x.Id == id).FirstOrDefault();
            return View(objProduct);
        }

        [HttpPost]
        public ActionResult Delete(Product objPro)
        {
            var objProduct = db.Products.Where(x => x.Id == objPro.Id).FirstOrDefault();
            db.Products.Remove(objProduct);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var objProduct = db.Products.Where(x => x.Id == id).FirstOrDefault();

            // Lấy danh sách các danh mục từ database
            var categories = db.Categories.ToList();
            ViewBag.Category_id = new SelectList(categories, "Id", "Name");

            // Lấy danh sách các thương hiệu từ database
            var brands = db.Brands.ToList();
            ViewBag.Brand_id = new SelectList(brands, "Id", "Name");

            return View(objProduct);
        }

        [HttpPost]
        public ActionResult Edit(int id, Product objProduct)
        {
            try
            {
                if (objProduct.ImageUpload != null)
                {
                    string fileName = Path.GetFileNameWithoutExtension(objProduct.ImageUpload.FileName);
                    string extension = Path.GetExtension(objProduct.ImageUpload.FileName);
                    fileName = fileName + "_" + DateTime.Now.ToString("yyyyMMddhhmmss") + extension;
                    objProduct.Image = fileName;
                    objProduct.ImageUpload.SaveAs(Path.Combine(Server.MapPath("~/Content/images/items/"), fileName));
                }

                db.Entry(objProduct).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Có lỗi xảy ra khi chỉnh sửa sản phẩm.");

                // Lấy danh sách các danh mục từ database
                var categories = db.Categories.ToList();
                ViewBag.Category_id = new SelectList(categories, "Id", "Name");

                // Lấy danh sách các thương hiệu từ database
                var brands = db.Brands.ToList();
                ViewBag.Brand_id = new SelectList(brands, "Id", "Name");

                return View(objProduct);
            }
        }


    }
}
