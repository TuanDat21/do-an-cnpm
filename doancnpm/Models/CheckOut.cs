using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using doancnpm.Models;

namespace doancnpm.Models
{
    public class CheckOut
    {
        webcnpmEntities2 db = new webcnpmEntities2();

        public int MASP { get; set; }
     
        public string TENSP { get; set; }
       
        public Double GIABAN { get; set; }
        
        public string MOTA { get; set; }
        
        public string HINHANH { get; set; }

        public int SOLUONG { set; get; }

        public Double THANHTIEN
        {
            get { return SOLUONG * GIABAN; }
        }

        public CheckOut(int MaSP)
        {
            MASP = MaSP;
            SANPHAM sp = db.SANPHAMs.Single(n => n.MASP == MASP);
            TENSP = sp.TENSP;
            HINHANH = sp.HINHANH;
            GIABAN = double.Parse(sp.GIABAN.ToString());
            SOLUONG = 1;
        }
    }
}