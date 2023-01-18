using DiplomskiRadMostarsMosques_Data.DAL;
using DiplomskiRadMostarsMosques_Data.Models;
using DiplomskiRadMostarsMosques_Web.Areas.Administrator.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.Web.Mvc;
using PagedList;
using DiplomskiRadMostarsMosques_Web.Helper;

namespace DiplomskiRadMostarsMosques_Web.Areas.Administrator.Controllers
{
    [Autorizacija(KorisnickeUloge.Administrator)]
    public class OpstineController : Controller
    {
        MMContext ctx = new MMContext();
        // GET: Administrator/Opstine
        public ActionResult Index(int? page)
        {
            var model = ctx.Opstinas.Include(x=>x.Kanton).OrderBy(x=>x.Naziv).ToPagedList(page ?? 1,6);
            ViewBag.Current = "L_Jedinice";
            return View(model);
        }


        public ActionResult Pretraga(string tekst)
        {
            int? page = 1;
            ViewBag.Current = "L_Jedinice";
            IPagedList<Opstina> model = ctx.Opstinas.Include(x=>x.Kanton).Where(x => x.Naziv.ToLower().Contains(tekst.ToLower())
             || tekst == "").OrderBy(x => x.Naziv).ToPagedList(page ?? 1, 6);

            return View("Index", model);
        }
        public bool CheckValid(OpstinaEditVM model)
        {
            return ModelState.IsValid;
        }

        public ActionResult Dodaj()
        {
            OpstinaEditVM model = new OpstinaEditVM();
            model.kantonStavke = ucitajKantone();
            return View("Uredi",model);
        }
        public ActionResult Uredi(int opstinaId)
        {
            OpstinaEditVM model = ctx.Opstinas.Where(x=>x.Id == opstinaId).Select(x=> new OpstinaEditVM
            {
                Id = x.Id,
                KantonId = x.KantonId,
                Naziv = x.Naziv
            }).SingleOrDefault();
            model.kantonStavke = ucitajKantone();
            return View("Uredi", model);
        }
        public List<SelectListItem> ucitajKantone()
        {
            var kantoni = new List<SelectListItem>();
            kantoni.Add(new SelectListItem {Text="Odaberite kanton",Value="" });
            kantoni.AddRange(ctx.Kantons.Select(x => new SelectListItem { Text = x.Naziv, Value = x.Id.ToString() }).ToList());
            return kantoni;
        }

        public ActionResult Snimi(OpstinaEditVM model)
        {
            if (!ModelState.IsValid)
            {
                model.kantonStavke = ucitajKantone();
                return View("Uredi", model);
            }
            Opstina opstinaDB;
            if (model.Id == 0)
            {
                opstinaDB = new Opstina();
                ctx.Opstinas.Add(opstinaDB);
            }
            else
            {
                opstinaDB = ctx.Opstinas.Find(model.Id);
            }
                opstinaDB.Naziv = model.Naziv;
                opstinaDB.KantonId = model.KantonId;
                ctx.SaveChanges();
            
            return RedirectToAction("Dodaj");
        }

        public ActionResult Obrisi(int opstinaId)
        {
            Opstina m = ctx.Opstinas.Find(opstinaId);
            ctx.Opstinas.Remove(m);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}