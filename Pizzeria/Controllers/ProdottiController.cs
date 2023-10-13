using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Pizzeria.Controllers
{
    /*[Authorize(Roles = "Admin")]*/ 
    public class ProdottiController : Controller
    {
        private ModelDBContext db = new ModelDBContext();


        // GET: Prodotti
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View("IndexAdmin", db.Prodotti.ToList());
            }
            else
            {
                return View("IndexUser", db.Prodotti.ToList());
            }
        }

        // GET: Prodotti/IndexAdmin
        public ActionResult IndexAdmin()
        {
            var prodotti = db.Prodotti.ToList();
            return View(prodotti);
        }

        // GET: Prodotti/IndexUser
        [AllowAnonymous]
        public ActionResult IndexUser()
        {
            var prodotti = db.Prodotti.ToList();
            return View(prodotti);
        }

        public ActionResult Dettagli(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Prodotti prodotto = db.Prodotti.Find(id);
            if (prodotto == null)
            {
                return HttpNotFound();
            }

            return View(prodotto);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prodotti prodotto = db.Prodotti.Find(id);
            if (prodotto == null)
            {
                return HttpNotFound();
            }
            return View(prodotto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Nome,Prezzo,TempoConsegna,Ingredienti")] Prodotti prodotto, HttpPostedFileBase Foto)
        {
            if (ModelState.IsValid)
            {
                if (Foto != null && Foto.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Foto.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Imgs/"), fileName);
                    Foto.SaveAs(path);
                    prodotto.Foto = "~/Content/Imgs/" + fileName;
                }

                db.Entry(prodotto).State = EntityState.Modified; 
                db.SaveChanges();
                return RedirectToAction("IndexAdmin");
            }
            return View(prodotto);
        }




        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Prodotti prodotto = db.Prodotti.Find(id);
            if (prodotto == null)
            {
                return HttpNotFound();
            }
            db.Prodotti.Remove(prodotto);
            db.SaveChanges();
            return RedirectToAction("IndexAdmin");
        }


        // GET: Prodotti/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Prodotti/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Prodotti prodotto, HttpPostedFileBase Foto)
        {
            if (ModelState.IsValid)
            {
                if (Foto != null && Foto.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Foto.FileName);
                    var path = Path.Combine(Server.MapPath("~/Content/Imgs/"), fileName);
                    Foto.SaveAs(path);
                    prodotto.Foto = "~/Content/Imgs/" + fileName;
                }

                db.Prodotti.Add(prodotto);
                db.SaveChanges();
                return RedirectToAction("IndexAdmin");
            }

            return View(prodotto);
        }





        // POST: Prodotti/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Prodotti prodotto = db.Prodotti.Find(id);
            db.Prodotti.Remove(prodotto);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}