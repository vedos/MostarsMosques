using DiplomskiRadMostarsMosques_Data.DAL;
using DiplomskiRadMostarsMosques_Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using DiplomskiRadMostarsMosques_Web.Areas.Administrator.Models;
using PagedList;
using DiplomskiRadMostarsMosques_Web.Helper;

namespace DiplomskiRadMostarsMosques_Web.Areas.Administrator.Controllers
{
    [Autorizacija(KorisnickeUloge.Administrator)]
    public class DzematiController : Controller
    {
        MMContext ctx = new MMContext();
        // GET: Administrator/Dzemati
        public ActionResult Index(int? page)
        {

            DzematVM model = new DzematVM();
            ViewBag.Current = "L_Jedinice";
            model.listDzemati = ctx.Dzemats.Select(x=>new DzematVM.DzematInfo {
                Id=x.Id,
                Kanton=x.Medzlis.Opstina.Kanton.Naziv,
                Opstina = x.Medzlis.Opstina.Naziv,
                Medzlis = x.Medzlis.MedzlisTranslations.Where(y => y.JezikId == 1 && y.MedzlisId == x.MedzlisId).FirstOrDefault().Naziv,
                Naziv = x.Naziv
            }).OrderBy(x=>x.Naziv).ToPagedList(page ?? 1, 6);


            return View(model);
        }

        public ActionResult Pretraga(string tekst)
        {
            int? page = 1;
            DzematVM model = new DzematVM();
            ViewBag.Current = "L_Jedinice";
            model.listDzemati = ctx.Dzemats.Select(x => new DzematVM.DzematInfo
               {
                   Id = x.Id,
                   Kanton = x.Medzlis.Opstina.Kanton.Naziv,
                   Opstina = x.Medzlis.Opstina.Naziv,
                   Medzlis = x.Medzlis.MedzlisTranslations.Where(y => y.JezikId == 1 && y.MedzlisId == x.MedzlisId).FirstOrDefault().Naziv,
                   Naziv = x.Naziv
               }).Where(x => x.Naziv.ToLower().Contains(tekst.ToLower())
            || tekst=="").OrderBy(x => x.Naziv).ToPagedList(page ?? 1, 6);

            return View("Index", model);
        }

        public bool CheckValid(DzematEditVM model)
        {
            return ModelState.IsValid;
        }

        public ActionResult Dodaj()
        {
            DzematEditVM model = new DzematEditVM();
            model.medzlisiStavke = ucitajMedzlise();
            return View("Uredi",model);
        }

        public ActionResult Uredi(int dzematId)
        {
            DzematEditVM model = ctx.Dzemats.Select(x => new DzematEditVM
            {
                Id = x.Id,
                Naziv = x.Naziv,
                medzlisId = x.MedzlisId
            }).Where(x=>x.Id== dzematId).FirstOrDefault();
            model.medzlisiStavke = ucitajMedzlise();

            return View(model);
        }
        public ActionResult Izbrisi(int dzematId)
        {
            Dzemat d = ctx.Dzemats.Find(dzematId);
            ctx.Dzemats.Remove(d);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }

        public List<SelectListItem> ucitajMedzlise()
        {
            var data = new List<SelectListItem>();
            data.Add(new SelectListItem {Text="Odaberite medžlise",Value="" });
            data.AddRange(ctx.Medzlis.Select(x=>new SelectListItem {
                Text=x.MedzlisTranslations.Where(y=>y.JezikId==Global.JezikId && y.MedzlisId==x.Id).FirstOrDefault().Naziv,
                Value=x.Id.ToString()
            }).ToList());
            return data;
        }

        public ActionResult Snimi(DzematEditVM model) {
            if (!ModelState.IsValid)
            {
                model.medzlisiStavke = ucitajMedzlise();
                return View("Uredi",model);
            }
            Dzemat dzematDb;
            if (model.Id == 0)
            {
                dzematDb = new Dzemat();
                ctx.Dzemats.Add(dzematDb);
                
            }
            else
            {
                dzematDb = ctx.Dzemats.Find(model.Id);
            }
            dzematDb.MedzlisId = model.medzlisId;
            dzematDb.Naziv = model.Naziv;
            ctx.SaveChanges();

            return RedirectToAction("Dodaj");
        }

    }
}