using DiplomskiRadMostarsMosques_Data.DAL;
using DiplomskiRadMostarsMosques_Data.Models;
using DiplomskiRadMostarsMosques_Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiplomskiRadMostarsMosques_Web.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        MMContext ctx = new MMContext();
        public ActionResult Provjera(string inputUsername, string inputPassword, string remember_me)
        {

            Korisnik k = ctx.Korisniks
                .SingleOrDefault(x => x.Username == inputUsername && x.Password == inputPassword);

            if (k != null)
            {
                Autentifikacija.PokreniNovuSesiju(k, HttpContext, (remember_me=="on"));
                if (k.zadnjaPrijava != null)
                    Global.lastLogin = k.zadnjaPrijava;
                else
                    Global.lastLogin = DateTime.Now;

                if (k.Id != 0)
                {
                    k.zadnjaPrijava = DateTime.Now;
                    ctx.SaveChanges();
                }
                return Redirect("/Administrator/Korisnici");


            }
            return Redirect("/Login");

            //return Redirect("/Login");
        }

        public void SetJezik(Jezik jezik)
        {
            if (jezik != null)
            {
                Global.PokreniNovuSesiju(jezik, HttpContext);
            }
        }




        public ActionResult Logout()
        {
            Autentifikacija.PokreniNovuSesiju(null, HttpContext, true);
            return RedirectToAction("Index");
        }
    }
}