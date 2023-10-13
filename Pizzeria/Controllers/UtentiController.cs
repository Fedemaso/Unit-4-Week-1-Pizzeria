using Pizzeria.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;

public class UtentiController : Controller
{
    private ModelDBContext 
        db = new ModelDBContext();

    //[Authorize(Roles = "Admin")]
    public ActionResult Index()
    {
        var utenti = db.Utenti.ToList();
        return View(utenti);
    }

    // GET: Utenti/Edit/5
    public ActionResult Edit(int? id)
    {
        if (id == null)
        {
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
        Utenti utente = db.Utenti.Find(id);
        if (utente == null)
        {
            return HttpNotFound();
        }
        return View(utente);
    }

    // POST: Utenti/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public ActionResult Edit([Bind(Include = "ID,Nome,Email")] Utenti utente)
    {
        if (ModelState.IsValid)
        {
            try
            {
                db.Entry(utente).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                // Qui puoi loggare l'errore o mostrare un messaggio all'utente
                ModelState.AddModelError("", "Si è verificato un errore durante il salvataggio delle modifiche.");
            }
        }
        return View(utente);
    }






    //// GET: Utenti/Delete/5
    //public ActionResult Delete(int? id)
    //{
    //    if (id == null)
    //    {
    //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
    //    }
    //    Utenti utente = db.Utenti.Find(id);
    //    if (utente == null)
    //    {
    //        return HttpNotFound();
    //    }
    //    return View(utente);
    //}

    //[HttpPost, ActionName("Delete")]
    //[ValidateAntiForgeryToken]
    //public ActionResult DeleteConfirmed(int id)
    //{
    //    Utenti utente = db.Utenti.Find(id);
    //    var hasActiveOrders = db.Ordini.Any(o => o.ID_Utente == id && o.Stato != "Ordine evaso");
    //    if (hasActiveOrders)
    //    {
    //        return Json(new { success = false, message = "L'utente ha ancora ordini attivi, impossibile eliminarlo." });
    //    }
    //    db.Utenti.Remove(utente);
    //    db.SaveChanges();
    //    return Json(new { success = true });
    //}




}
