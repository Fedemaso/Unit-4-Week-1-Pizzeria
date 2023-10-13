using Microsoft.Win32;
using Pizzeria.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;

namespace Pizzeria.Controllers
{
    public class AccountController : Controller
    {
        private ModelDBContext db = new ModelDBContext();
        
        
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }




        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(Account model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = db.Utenti.FirstOrDefault(u => u.Email == model.LoginModel.Email && u.Password == model.LoginModel.Password);
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(user.Email, model.LoginModel.RememberMe);

                    if (Url.IsLocalUrl(returnUrl) && returnUrl.Length > 1 && returnUrl.StartsWith("/")
                        && !returnUrl.StartsWith("//") && !returnUrl.StartsWith("/\\"))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                    {
                        if (user.Ruolo == "Admin")
                        {
                            return RedirectToAction("IndexAdmin", "Prodotti");
                        }
                        else
                        {
                            return RedirectToAction("IndexUser", "Prodotti");
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "L'email o la password forniti non sono corretti.");
                }
            }

            return View(model);
        }






        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(Account model)
        {
            if (ModelState.IsValid)
            {
                var userExists = db.Utenti.Any(u => u.Email == model.RegisterModel.Email);
                if (!userExists)
                {
                    var newUser = new Utenti
                    {
                        Nome = model.RegisterModel.Nome,
                        Email = model.RegisterModel.Email,
                        Password = model.RegisterModel.Password,
                        Ruolo = "User"
                    };

                    db.Utenti.Add(newUser);
                    db.SaveChanges();

                    FormsAuthentication.SetAuthCookie(model.RegisterModel.Email, false);
                    return RedirectToAction("IndexUser", "Prodotti");
                }
                else
                {
                    ModelState.AddModelError("", "L'email fornita è già registrata.");
                }
            }

            return View("Login", model);
        }

        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
    }

}