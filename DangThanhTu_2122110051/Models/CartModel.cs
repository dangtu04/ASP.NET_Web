using DangThanhTu_2122110051.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DangThanhTu_2122110051.Models
{
    public class CartModel
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

    }
}