using DangThanhTu_2122110051.Context;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DangThanhTu_2122110051.Areas.Admin.Controllers
{
    public class OrderdetailController : Controller
    {
        AspNetWebEntities db = new AspNetWebEntities();

        // GET: Admin/Orderdetail
        public ActionResult Index(long? orderId, int? page, string currentFilter)
        {
            if (orderId == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var orderDetails = db.Orderdetails
                                 .Where(od => od.Order_id == orderId)
                                 .OrderBy(od => od.Id);

            int pageSize = 10;
            int pageNumber = (page ?? 1);

            return View(orderDetails.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Delete(int id)
        {
            var objOrderdetail = db.Orderdetails.SingleOrDefault(x => x.Id == id);
            if (objOrderdetail == null)
            {
                return HttpNotFound();
            }
            return View(objOrderdetail);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Orderdetail objPro)
        {
            var objOrderdetail = db.Orderdetails.SingleOrDefault(x => x.Id == objPro.Id);
            if (objOrderdetail == null)
            {
                return HttpNotFound();
            }

            db.Orderdetails.Remove(objOrderdetail);
            db.SaveChanges();

            return RedirectToAction("Index", new { orderId = objOrderdetail.Order_id });
        }
    }
}
