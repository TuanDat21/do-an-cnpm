using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doancnpm.Controllers;
using doancnpm.Models;
using PagedList;

namespace doancnpm.Controllers
{
    public class HomeController : Controller
    {
        Models.webcnpmEntities2 db = new Models.webcnpmEntities2();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Contact()
        {
          
            return View();
        }
        public ActionResult Checkout()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection f)
        {
            string tendangnhap = f["text"].ToString();
            string matkhau = f.Get("password").ToString();
            TAIKHOAN tk = db.TAIKHOANs.SingleOrDefault(n => n.TENDN == tendangnhap && n.MATKHAU == matkhau);
            if (tk != null)
            {
                
                Session["Taikhoan"] = tk;
                return RedirectToAction("Index" , "Home");
            }
            ViewBag.thongbao = "Tên tài khoản hoặc mất khẩu không đúng";
            return View();
            
        }

        public ActionResult Products(int ?page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 6;
            return View(db.SANPHAMs.ToList().OrderBy(n => n.MASP).ToPagedList(pageNumber, pageSize));
        }

        public ActionResult Single(int id)
        {
            var single = from SANPHAM in db.SANPHAMs
                         where SANPHAM.MASP  == id
                         select SANPHAM;
            return View(single.Single());
        }

        public ActionResult PartialViewSPlienquan()
        {
            var splienquan = db.SANPHAMs.Take(3).ToList();
            return PartialView(splienquan);
        }

        public ActionResult PartialViewSPBanchay()
        {
            var listbanchay = db.SANPHAMs.Take(2).ToList();
            return PartialView(listbanchay);
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }


        [HttpPost]       
        public ActionResult Register(TAIKHOAN tk)
        {                        
            db.TAIKHOANs.Add(tk);
            db.SaveChanges();           
            return View();
        }

        public ActionResult Blog()
        {
            return View();
        }

        


    }
}