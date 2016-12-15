using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using doancnpm.Controllers;
using doancnpm.Models;
using PagedList;
using PagedList.Mvc;
using System.IO;

namespace doancnpm.Controllers
{

    public class AdminController : Controller
    {
        Models.webcnpmEntities2 db = new Models.webcnpmEntities2();
        // GET: Admin
        public ActionResult IndexAdmin(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            return View(db.SANPHAMs.ToList().OrderBy(n => n.MASP).ToPagedList(pageNumber, pageSize));
        }
        public ActionResult LoginAdmin(FormCollection cl)
        {
            var id = cl["username"];
            var pass = cl["password"];
            if (String.IsNullOrEmpty(id))
            {
                ViewData["Loi1"] = "Nhập tên đang nhập và mật khẩu";
            }
            else
            {
                ADMIN ad = db.ADMINs.SingleOrDefault(n => n.ID == id && n.PASS == pass);
                if (ad != null)
                {
                    Session["Taikhoanadmin"] = ad;
                    return RedirectToAction("IndexAdmin");
                }
                else
                    ViewBag.Thongbao = "Tên đăng nhập hoặc mật khẩu không đúng";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Create()
        {
            ViewBag.MaDM = new SelectList(db.DANHMUCs.ToList().OrderBy(n => n.TENDM), "MADM", "TENDM");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(SANPHAM sp, HttpPostedFileBase fileupload)
        {
            ViewBag.MaDM = new SelectList(db.DANHMUCs.ToList().OrderBy(n => n.TENDM), "MADM", "TENDM");                     
            if (ModelState.IsValid)
            {
                var filename = Path.GetFileName(fileupload.FileName);
                var path = Path.Combine(Server.MapPath("~/Content/Images/hinhsanpham"), filename);
                if (System.IO.File.Exists(path))
                {
                    ViewBag.Thongbao = "Hình đã tồn tại";

                }
                else
                {
                    fileupload.SaveAs(path);
                }
                sp.HINHANH = filename;
                db.SANPHAMs.Add(sp);
                db.SaveChanges();
            }
            return View();
        }

        public ActionResult Details(int id)
        {
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MASP == id);
            ViewBag.MASP = sp.MASP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;

            }
            return View(sp);
        }


        [HttpGet]
        public ActionResult Delete(int id)
        {
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MASP == id);
            ViewBag.MASP = sp.MASP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }           
            return View(sp);
        }


        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteAccept (int id)
        {
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MASP == id);
            ViewBag.MASP = sp.MASP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.SANPHAMs.Remove(sp);
            db.SaveChanges();
            return RedirectToAction("IndexAdmin");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(n => n.MASP == id);
            ViewBag.MASP = sp.MASP;
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            ViewBag.MaDM = new SelectList(db.DANHMUCs.ToList().OrderBy(n => n.TENDM), "MADM", "TENDM");
            return View(sp);
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(SANPHAM sp, HttpPostedFileBase fileupload)
        {
            ViewBag.MaDM = new SelectList(db.DANHMUCs.ToList().OrderBy(n => n.TENDM), "MADM", "TENDM");                     
            if (ModelState.IsValid)
            {
                if (fileupload != null) {
                    var filename = Path.GetFileName(fileupload.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Images/hinhsanpham"), filename);
                    if (System.IO.File.Exists(path))
                    {
                        ViewBag.Thongbao = "Hình đã tồn tại";

                    }
                    else
                    {
                        fileupload.SaveAs(path);
                    }
                    sp.HINHANH = filename;
                }

                db.Entry(sp).State = System.Data.Entity.EntityState.Modified;
                
                //UpdateModel(sp);
                db.SaveChanges();
            }
            return RedirectToAction("IndexAdmin");
        }

        public ActionResult Danhmuc()
        {
            
            return View(db.DANHMUCs.ToList());
        }

        public ActionResult Khachhang()
        {
            return View(db.TAIKHOANs.ToList());
        }


    }
}