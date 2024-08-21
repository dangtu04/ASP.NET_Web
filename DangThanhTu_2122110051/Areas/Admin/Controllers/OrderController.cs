using DangThanhTu_2122110051.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DangThanhTu_2122110051.Areas.Admin.Controllers
{
    public class OrderController : Controller
    {
        AspNetWebEntities db = new AspNetWebEntities();
        // GET: Admin/Order
        public ActionResult Index(string currentFilter, string SearchString, int? page)
        {
            var lstOrder = new List<Order>();
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
                // Lấy danh sáchOrder theo từ khóa tìm kiếm
                lstOrder = db.Orders.Where(n => n.User.LastName.Contains(SearchString)).ToList();
            }
            else
            {
                // Lấy tất cả Order trong bảng Order
                lstOrder = db.Orders.ToList();
            }

            ViewBag.CurrentFilter = SearchString;
            // Số lượng item của 1 trang = 4
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            // Sắp xếp theo idOrder,Order mới đưa lên đầu
            lstOrder = lstOrder.OrderByDescending(n => n.Id).ToList();
            return View(lstOrder.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            var objOrder = db.Orders.Where(x => x.Id == id).FirstOrDefault();
            return View(objOrder);
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objOrder = db.Orders.Where(x => x.Id == id).FirstOrDefault();
            return View(objOrder);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Order objPro)
        {
            var objOrder = db.Orders.Where(x => x.Id == objPro.Id).FirstOrDefault();

            // Kiểm tra xem Order có liên kết với bất kỳ OrderDetail nào không
            var hasOrderDetails = db.Orderdetails.Any(od => od.Order_id == objPro.Id);

            if (hasOrderDetails)
            {
                // Thông báo không thể xóa vì có liên kết với OrderDetail
                TempData["ErrorMessage"] = "Không thể xóa đơn hàng vì có liên kết với OrderDetail.";
                return RedirectToAction("Delete", new { id = objPro.Id });
            }

            db.Orders.Remove(objOrder);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

    }
}