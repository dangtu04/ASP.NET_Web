using DangThanhTu_2122110051.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DangThanhTu_2122110051.Models
{
    public class HomeModel
    {
        public List<Product> ListProduct { get; set; }
        public List<Category> ListCategory { get; set; }
    }
}