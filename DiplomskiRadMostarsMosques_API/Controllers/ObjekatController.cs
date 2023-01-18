using DiplomskiRadMostarsMosques_API.Models;
using DiplomskiRadMostarsMosques_Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using PagedList;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using DiplomskiRadMostarsMosques_API.Helper;

namespace DiplomskiRadMostarsMosques_API.Controllers
{
    public class ObjekatController : ApiController
    {
        [HttpGet]
        // GET: Objekati
        public ObjekatVM GetAll(int jezikId)
        {
            MMContext ctx = new MMContext();
            ObjekatVM model = new ObjekatVM();
            model.listObjekti = ctx.Objekats.Where(x => x.ObjekatTranslations.Where(y => y.JezikId == jezikId && y.ObjekatId == x.Id).FirstOrDefault() != null).Select(x => new ObjekatVM.ObjekatInfo
            {
                Id = x.Id,
                //lokacijaId=x.LokacijaId,
                naziv=x.ObjekatTranslations.Where(y=>y.ObjekatId==x.Id&&y.JezikId==jezikId).FirstOrDefault().Naziv,
                AktivnaId=x.AktivnostId,
                TipObjektaId = x.TipObjektaId,
                //opis=x.Opis,
                latitude=x.Lokacija.latitude,
                longitude=x.Lokacija.longitude,
                //adresa=x.Lokacija.adresa
            }).ToList();

            return model;
        }

        [HttpGet]
        public int GetCountObjekti()
        {
            MMContext ctx = new MMContext();
            int count = ctx.Objekats.ToList().Count;

            return count;
        }


        [HttpGet]
        public ObjekatVM.ObjekatInfo GetObjekatById(int id,int jezikId)
        {
            MMContext ctx = new MMContext();
            ObjekatVM.ObjekatInfo model = new ObjekatVM.ObjekatInfo();
            model = ctx.Objekats.Where(x => x.ObjekatTranslations.Where(y => y.JezikId == jezikId && y.ObjekatId == x.Id).FirstOrDefault() != null).Where(x => x.Id == id).Select(x => new ObjekatVM.ObjekatInfo
            {
                Id = x.Id,
                lokacijaId = x.LokacijaId,
                naziv = x.ObjekatTranslations.Where(y => y.ObjekatId == x.Id && y.JezikId == jezikId).FirstOrDefault().Naziv,
                opis = x.ObjekatTranslations.Where(y => y.ObjekatId == x.Id && y.JezikId == jezikId).FirstOrDefault().Opis,
                latitude = x.Lokacija.latitude,
                longitude = x.Lokacija.longitude,
                adresa = x.Lokacija.adresa,
                IsUnesco = x.IsUnesco,
                Dzemat = x.Dzemat.Naziv,
                GodinaIzgradnje = x.GodinaIzgradnje.ToString(),
                GodinaObnove = x.GodinaObnove.ToString(),
                GodinaRusenja = x.GodinaRusenja.ToString(),
                IsRusena = x.IsRusena,
                Medzlis = x.Medzlis.MedzlisTranslations.Where(y=>y.MedzlisId==x.MedzlisId && y.JezikId==jezikId).FirstOrDefault().Naziv,
                TipObjekta = x.TipObjekta.Naziv,
                TipObjektaId = x.TipObjektaId,
                Aktivna = x.Aktivnost.Naziv
            }).FirstOrDefault();

            return model;
        }

        [HttpGet]
        public DzamijeVM GetTuristickiPotencijali(int page, int jezikId)
        {
            MMContext ctx = new MMContext();
            DzamijeVM model = new DzamijeVM();
            int? pageNumber = page;
            int numberOf = 10;
            //tip==3 turistički objekti
            model.listDzamije = ctx.Objekats.Where(x => x.ObjekatTranslations.Where(y => y.JezikId == jezikId && y.ObjekatId == x.Id).FirstOrDefault() != null).Where(x=>x.TipObjektaId==3).Select(x => new DzamijeVM.DzamijaInfo
            {
                Id = x.Id,
                lokacijaId = x.LokacijaId,
                naziv = x.ObjekatTranslations.Where(y => y.ObjekatId == x.Id && y.JezikId == jezikId).FirstOrDefault().Naziv,
                SlikaThumbnail = x.Galerija.SlikaInfos.FirstOrDefault().SlikaThumbnail
            }).OrderBy(x => x.naziv).ToPagedList(pageNumber ?? 1, numberOf);
            model.totalItemsCount = model.listDzamije.TotalItemCount;

            return model;
        }

    }
}
