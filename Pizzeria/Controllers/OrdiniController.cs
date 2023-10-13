using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Mvc;


namespace Pizzeria.Controllers
{
    //[Authorize]
    public class OrdiniController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        // GET: Ordini
        public ActionResult Index()
        {
            var ordiniConNomi = db.Ordini
                                  .Include(o => o.Utenti)
                                  .ToList();

            return View(ordiniConNomi);
        }



        [HttpPost]
        public ActionResult AggiungiAlCarrello(int prodottoId, int quantita)
        {
            var prodotto = db.Prodotti.Find(prodottoId);
            if (prodotto == null)
            {
                return HttpNotFound();
            }

            var carrello = Session["Carrello"] as List<CarrelloRiepilogo.Carrello> ?? new List<CarrelloRiepilogo.Carrello>();

            var itemCarrello = carrello.FirstOrDefault(i => i.Id == prodottoId);
            if (itemCarrello == null)
            {
                carrello.Add(new CarrelloRiepilogo.Carrello
                {
                    Id = prodotto.ID,
                    NomePizza = prodotto.Nome,
                    Quantita = quantita,
                    Prezzo = prodotto.Prezzo
                });
            }
            else
            {
                itemCarrello.Quantita += quantita;
            }

            Session["Carrello"] = carrello;

            return RedirectToAction("IndexUser", "Prodotti");
        }




        [HttpPost]
        public ActionResult RimuoviDalCarrello(int id)
        {
            var carrello = Session["Carrello"] as List<CarrelloRiepilogo.Carrello>;
            if (carrello != null)
            {
                var itemToRemove = carrello.FirstOrDefault(i => i.Id == id);
                if (itemToRemove != null)
                {
                    carrello.Remove(itemToRemove);
                }
            }
            Session["Carrello"] = carrello;
            return RedirectToAction("RiepilogoCarrello");
        }

        [HttpPost]
        public ActionResult SvuotaCarrello()
        {
            Session["Carrello"] = null;
            return PartialView("_RiepilogoCarrelloVuoto");
        }




        public ActionResult RiepilogoCarrello()
        {
            var carrello = Session["Carrello"] as List<CarrelloRiepilogo.Carrello>;
            if (carrello == null || !carrello.Any())
            {
                return PartialView("_RiepilogoCarrelloVuoto");
            }

            var riepilogo = new CarrelloRiepilogo.Riepilogo
            {
                Items = carrello
            };

            return PartialView("_RiepilogoCarrello", riepilogo);
        }



       

        [HttpGet]
        public ActionResult ConcludiOrdine()
        {
            
            return View("ConfermaOrdine");
        }

       

        [HttpPost]
        public ActionResult ConcludiOrdineFinal(CarrelloRiepilogo.Riepilogo riepilogo)
        {
            string userEmail = User.Identity.Name;
            var utente = db.Utenti.FirstOrDefault(u => u.Email == userEmail);
            if (utente == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var carrello = Session["Carrello"] as List<CarrelloRiepilogo.Carrello>;
            if (carrello == null || !carrello.Any())
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Ordini ordine = new Ordini
            {
                ID_Utente = utente.ID,
                DataOrdine = DateTime.Now,
                IndirizzoSpedizione = riepilogo.IndirizzoSpedizione,
                Note = riepilogo.Note,
                Stato = "In lavorazione"
            };

            db.Ordini.Add(ordine);
            db.SaveChanges();

            foreach (var item in carrello)
            {
                DettagliOrdine dettaglio = new DettagliOrdine
                {
                    ID_Ordine = ordine.ID,
                    ID_Prodotto = item.Id,
                    Quantità = item.Quantita,
                    PrezzoTotale = item.Totale
                };

                db.DettagliOrdine.Add(dettaglio);
            }

            db.SaveChanges();

            Session["UltimoOrdineID"] = ordine.ID;
            Session["Carrello"] = null;

            if (Request.IsAjaxRequest())
            {
                return PartialView("_SuccessModal");
            }
            else
            {
                return RedirectToAction("RiepilogoOrdine", "Ordini");
            }
        }





        public ActionResult RiepilogoOrdine()
        {
            int? ordineID = Session["UltimoOrdineID"] as int?;
            if (!ordineID.HasValue)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var ordine = db.Ordini.Include(o => o.DettagliOrdine)
                                  .FirstOrDefault(o => o.ID == ordineID.Value);
            if (ordine == null)
            {
                return HttpNotFound();
            }

            var riepilogo = new CarrelloRiepilogo.Riepilogo
            {
                Items = ordine.DettagliOrdine.Select(d => new CarrelloRiepilogo.Carrello
                {
                    Id = d.ID_Prodotto.Value,
                    NomePizza = d.Prodotti.Nome,
                    Quantita = d.Quantità,
                    Prezzo = d.Prodotti.Prezzo
                }).ToList(),
                IndirizzoSpedizione = ordine.IndirizzoSpedizione,
                Note = ordine.Note
            };

            return View(riepilogo);
        }



        // GET: Ordini/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordine = db.Ordini.Find(id);
            if (ordine == null)
            {
                return HttpNotFound();
            }
            return View(ordine);
        }

        // GET: Ordini/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ordini ordine = db.Ordini.Find(id);
            if (ordine == null)
            {
                return HttpNotFound();
            }
            return View(ordine);
        }

        // POST: Ordini/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,ID_Utente,DataOrdine,IndirizzoSpedizione,Note,Stato")] Ordini ordine)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordine).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ordine);
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