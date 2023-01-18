using DiplomskiRadMostarsMosques_Data.DAL;
using DiplomskiRadMostarsMosques_Data.Models;
using DiplomskiRadMostarsMosques_Web.Areas.Administrator.Models;
using DiplomskiRadMostarsMosques_Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiplomskiRadMostarsMosques_Web.Areas.Administrator.Controllers
{
    [Autorizacija(KorisnickeUloge.Korisnik)]
    public class HomeController : Controller
    {
        MMContext ctx = new MMContext();
        // GET: Administrator/Home
        public ActionResult Index()
        {
            HomeVM model = new HomeVM();
            ViewBag.Current = "Pocetna";
            return View(model);
        }

        public ActionResult ChangeLanguage(int id)
        {
            Jezik j = ctx.Jeziks.Find(id);
            if(j!=null)
                Global.PokreniNovuSesiju(j, HttpContext);


            return Redirect(Request.UrlReferrer.ToString());
        }
    }
}