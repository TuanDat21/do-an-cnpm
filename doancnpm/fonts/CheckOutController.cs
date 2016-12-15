using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doancnpm.Models;

namespace doancnpm.Controllers
{
    public class CheckOutController : Controller
    {

        Models.webcnpmEntities2 db = new Models.webcnpmEntities2();
        // GET: CheckOut
        public List<CheckOut> Laygiohang()
        {
            List<CheckOut> list = Session["CheckOut"] as List<CheckOut>;
            if (list==null)
            {
                list = new List<CheckOut>();
                Session["CheckOut"] = list;
            }
            return list;
        }

        public ActionResult Themgiohang(int Masp, string  strURL)
        {
            List<CheckOut> list = Laygiohang();
            CheckOut sp = list.Find(n => n.MASP == Masp);
            if (sp==null)
            {
                sp = new CheckOut(Masp);
                list.Add(sp);
                return Redirect(strURL);
            }
            else
            {
                sp.SOLUONG++;
                return Redirect(strURL);

            }

        }

        private int Tongsoluong()
        {
            int i = 0;
            List<CheckOut> list = Session["CheckOut"] as List<CheckOut>;
            if (list!=null)
            {
                i = list.Sum(n => n.SOLUONG);
            }
            return i;
        }

        private double Tongtien()
        {
            double i = 0;
            List<CheckOut> list = Session["CheckOut"] as List<CheckOut>;
            if (list !=null)
            {
                i = list.Sum(n => n.THANHTIEN);
            }
            return i;
        }

        public ActionResult Checkout()
        {
            List<CheckOut> list = Laygiohang();
            if(list.Count == 0)
            {
                return RedirectToAction("Index", "Home");
                 
            }
            ViewBag.Tongsongluong = Tongsoluong();
            ViewBag.Tongtien = Tongtien();
            return View(list);
        }

        [HttpGet]
        public ActionResult Dathang()
        {
            if (Session["Taikhoan"] == null || Session["Taikhoan"].ToString()=="")
            {
                return RedirectToAction("Login", "Home");
            }
            if (Session["CheckOut"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<CheckOut> list = Laygiohang();
            ViewBag.Tongsoluong = Tongsoluong();
            ViewBag.Tongtien = Tongtien();

            return View(list);
        }

        public ActionResult Dathang(FormCollection f)
        {
            DONHANG dh = new DONHANG();
            TAIKHOAN tk = (TAIKHOAN)Session["Taikhoan"];
            List<CheckOut> gh = Laygiohang();
            dh.MAKH = tk.MAKH;
            dh.NGAYDATHANG = DateTime.Now;
            db.DONHANGs.Add(dh);
            db.SaveChanges();
            foreach (var item in gh)
            {
                CHITIETDONHANG ct = new CHITIETDONHANG();
                ct.MADH = dh.MADH;
                ct.MASP = item.MASP;
                ct.SOLUONG = item.SOLUONG;
                ct.DONGIA = (decimal)item.GIABAN;
                db.CHITIETDONHANGs.Add(ct);
            }
            db.SaveChanges();
            Session["Giohang"] = null;
            return RedirectToAction("Active", "CheckOut");
        }

        public ActionResult Active()
        {
            return View();
        }

        public ActionResult Capnhatgiohang(int masp, FormCollection f)
        {
            List<CheckOut> list = Laygiohang();
            CheckOut sp = list.SingleOrDefault(n => n.MASP == masp);
            if (sp != null)
            {
                sp.SOLUONG = int.Parse(f["txtSoluong"].ToString());
            }
            return RedirectToAction("Checkout");
        }
    }
}