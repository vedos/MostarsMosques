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
    [Autorizacija(KorisnickeUloge.Administrator)]
    public class KantoniController : Controller
    {
        MMContext ctx = new MMContext();
        // GET: Administrator/Kantoni
        public ActionResult Index()
        {
            List<Kanton> model = ctx.Kantons.ToList();
            ViewBag.Current = "L_Jedinice";
            return View(model);
        }

        public bool CheckValid(KantonEditVM model)
        {
            return ModelState.IsValid;
        }

        public ActionResult Dodaj()
        {
            KantonEditVM model = new KantonEditVM();
            return View("Uredi",model);
        }

        public ActionResult Uredi(int kantonId)
        {
            KantonEditVM model = ctx.Kantons.Select(x=>new KantonEditVM {
                Id = x.Id,
                Naziv = x.Naziv,
                Skracenica = x.Skracenica
            }).Where(x=>x.Id == kantonId).FirstOrDefault();
            return View(model);
        }

        public ActionResult Obrisi(int kantonId)
        {
            Kanton k = ctx.Kantons.Find(kantonId);
            ctx.Kantons.Remove(k);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }
        public ActionResult Snimi(KantonEditVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("Uredi",model);
            }
            Kanton kantonDB;
            if (model.Id == 0)
            {
                kantonDB = new Kanton();
                ctx.Kantons.Add(kantonDB);
            }
            else
            {
                kantonDB = ctx.Kantons.Find(model.Id);
            }
            kantonDB.Naziv = model.Naziv;
            kantonDB.Skracenica = model.Skracenica;
            ctx.SaveChanges();
            return RedirectToAction("Dodaj");
        }
    }
}