﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pizzeria.Controllers
{
    public class InformazioniController : Controller
    {
        // GET: Informazioni
        public ActionResult ChiSiamo()
        {
            return View();
        }
    }
}