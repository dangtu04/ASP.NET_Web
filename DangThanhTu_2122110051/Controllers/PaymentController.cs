using DangThanhTu_2122110051.Context;
using DangThanhTu_2122110051.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DangThanhTu_2122110051.Controllers
{
    public class PaymentController : Controller
    {
        // GET: Payment
        AspNetWebEntities db = new AspNetWebEntities();
        public ActionResult Index()
        {
            if (Session["idUser"] == null)
            {
                return RedirectToAction("Login", "User");
            }
            else
            {
                // Lấy thông tin giỏ hàng từ biến session
                var lstCart = (List<CartModel>)Session["cart"];

                // Gán dữ liệu cho Order
                Order objOrder = new Order();
                objOrder.Delivery_name = "DonHang-" + DateTime.Now.ToString("yyyyMMddHHmmss");
                objOrder.User_id = int.Parse(Session["idUser"].ToString());
                objOrder.Delivery_email = Session["Email"].ToString();
                objOrder.Delivery_phone = Session["Phone"].ToString();
                objOrder.Delivery_address = Session["Address"].ToString();
                objOrder.Status = 1;
                db.Orders.Add(objOrder);

                // Lưu thông tin dữ liệu vào bảng order
                db.SaveChanges();

                // Lấy OrderId vừa mới tạo để lưu vào bảng Orderdetail
                long intOrderId = objOrder.Id;
                List<Orderdetail> lstOrderdetail = new List<Orderdetail>();

                foreach (var item in lstCart)
                {
                    Orderdetail obj = new Orderdetail();
                    obj.Qty = item.Quantity;
                    obj.Order_id = intOrderId;
                    obj.Product_id = item.Product.Id;
                    //obj.Price = item.Price;
                    //obj.Amount = item.Price * item.Quantity;

                    lstOrderdetail.Add(obj);
                }

                db.Orderdetails.AddRange(lstOrderdetail);
                db.SaveChanges();
            }
            return View();

        }
    }
}