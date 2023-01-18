using DiplomskiRadMostarsMosques_Data.DAL;
using DiplomskiRadMostarsMosques_Data.Models;
using DiplomskiRadMostarsMosques_Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using DiplomskiRadMostarsMosques_Web.Areas.Administrator.Models;
using PagedList;

namespace DiplomskiRadMostarsMosques_Web.Areas.Administrator.Controllers
{
    [Autorizacija(KorisnickeUloge.Administrator)]
    public class MedzlisiController : Controller
    {
        MMContext ctx = new MMContext();
        // GET: Administrator/Medzlis
        public ActionResult Index(int? page)
        {
            MedzlisVM list = new MedzlisVM();
            ViewBag.Current = "L_Jedinice";
            list.listMedzlis = ctx.Medzlis.Include(x=>x.MedzlisTranslations).Include(x => x.Opstina).Include(x => x.Opstina.Kanton).Select(x=>new MedzlisVM.MedzlisInfo {
                Id  = x.Id,
                Naziv = x.MedzlisTranslations.Where(y=>y.JezikId==Global.JezikId && y.MedzlisId==x.Id).FirstOrDefault().Naziv,
                Jezici = x.MedzlisTranslations.Select(y => y.Jezik.Naziv).ToList(),
                Opis = x.MedzlisTranslations.Where(y => y.JezikId == Global.JezikId && y.MedzlisId == x.Id).FirstOrDefault().Opis,
                Kanton=x.Opstina.Kanton.Naziv,
                Opcina = x.Opstina.Naziv
            }).OrderBy(x => x.Naziv).ToPagedList(page ?? 1, 6);
            var model = list;
            return View("Index",model);
        }


        public ActionResult Pretraga(string tekst)
        {
            int? page = 1;
            MedzlisVM model = new MedzlisVM();
            ViewBag.Current = "L_Jedinice";
            model.listMedzlis = ctx.Medzlis.Include(x => x.Opstina).Include(x => x.Opstina.Kanton).Select(x=>new MedzlisVM.MedzlisInfo {
                Naziv = x.MedzlisTranslations.Where(y => y.JezikId == Global.JezikId && y.MedzlisId == x.Id).FirstOrDefault().Naziv,
                Id = x.Id,
                Jezici = x.MedzlisTranslations.Select(y=>y.Jezik.Naziv).ToList(),
                Kanton = x.Opstina.Kanton.Naziv,
                Opcina = x.Opstina.Naziv,
                Opis = x.MedzlisTranslations.Where(y => y.JezikId == Global.JezikId && y.MedzlisId == x.Id).FirstOrDefault().Opis
            }).OrderBy(x => x.Naziv).Where(x => x.Naziv.ToLower().Contains(tekst.ToLower())
            || tekst == "").ToPagedList(page ?? 1, 6);

            return View("Index", model);
        }

        public bool CheckValid(MedzlisEditVM model)
        {
            return ModelState.IsValid;
        }

        public ActionResult Dodaj()
        {
            MedzlisEditVM model = new MedzlisEditVM();
            model.opstinaStavke = ucitajOpstine();
            return View("Uredi", model);
        }

        public ActionResult Snimi(MedzlisEditVM model)
        {
            int JezikId = Global.GetJezik(HttpContext).Id;
            if (!ModelState.IsValid)
            {
                model.opstinaStavke = ucitajOpstine();
                return View("Uredi", model);
            }
            Medzlis m;
            MedzlisTranslation mt;
            if (model.Id == 0)
            {
                m = new Medzlis();
                mt = new MedzlisTranslation();
                ctx.Medzlis.Add(m);
                ctx.MedzlisTranslations.Add(mt);
            }
            else
            {
                m = ctx.Medzlis.Find(model.Id);
                mt= ctx.MedzlisTranslations.Where(x => x.MedzlisId == model.Id && x.JezikId == JezikId).FirstOrDefault();
                if (mt == null)
                {
                    mt = new MedzlisTranslation();
                    ctx.MedzlisTranslations.Add(mt);
                }
            }
            mt.JezikId = JezikId;
            mt.MedzlisId = m.Id;
            mt.Naziv = model.Naziv;
            mt.Opis = model.Opis;
            m.OpstinaId = model.opstinaId;

            ctx.SaveChanges();

            return RedirectToAction("Dodaj");
        }

        public ActionResult Uredi(int medzlisId)
        {
            int JezikId = Global.GetJezik(HttpContext).Id;
            MedzlisEditVM model = ctx.Medzlis.Where(x => x.Id == medzlisId).Select(x => new MedzlisEditVM
            {
                Id = x.Id,
                opstinaId = x.OpstinaId,
                Opis= ctx.MedzlisTranslations.Where(y => y.MedzlisId == x.Id && y.JezikId == JezikId).FirstOrDefault().Opis,
                
                Naziv = ctx.MedzlisTranslations.Where(y => y.MedzlisId == x.Id && y.JezikId == JezikId).FirstOrDefault().Naziv
            }).FirstOrDefault();
            model.opstinaStavke = ucitajOpstine();
            return View("Uredi", model);
        }

        public List<SelectListItem> ucitajOpstine()
        {
            List<SelectListItem> data = new List<SelectListItem>();
            data.Add(new SelectListItem { Text = "Odaberite opštinu" ,Value=""});
            data.AddRange(ctx.Opstinas.Select(x => new SelectListItem
            {
                Text = x.Naziv,
                Value = x.Id.ToString()
            }).ToList());
            return data;
        }

        public ActionResult Obrisi(int medzlisId)
        {
            Medzlis m = ctx.Medzlis.Include(a=>a.MedzlisTranslations).Where(x=>x.Id==medzlisId).FirstOrDefault();
            ctx.MedzlisTranslations.RemoveRange(m.MedzlisTranslations);
            ctx.Medzlis.Remove(m);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}