using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Pizzeria.Controllers
{
    //[Authorize(Roles = "Admin")]
    public class StatisticheController : Controller
    {
        private ModelDBContext db = new ModelDBContext();

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        
        public async Task<JsonResult> GetNumeroOrdiniEvasi()
        {
            int numeroOrdiniEvasi = await db.Ordini.CountAsync(o => o.Stato == "Ordine evaso");
            return Json(numeroOrdiniEvasi, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public async Task<JsonResult> GetTotaleIncassato(DateTime data)
        {
            decimal totale = await db.Ordini
                .Where(o => DbFunctions.TruncateTime(o.DataOrdine) == data.Date && o.Stato == "Ordine evaso")
                .SumAsync(o => o.DettagliOrdine.Sum(d => d.PrezzoTotale));
            return Json(totale, JsonRequestBehavior.AllowGet);
        }
    }
}