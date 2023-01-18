using DiplomskiRadMostarsMosques_API.Models;
using DiplomskiRadMostarsMosques_Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PagedList;
using DiplomskiRadMostarsMosques_API.Helper;

namespace DiplomskiRadMostarsMosques_API.Controllers
{
    public class DzamijeController : ApiController
    {

        private static int DZAMIJA_ID = 1;
        private static int MESDZID_ID = 2;


        [HttpGet]
        // GET: Objekati
        public DzamijeVM GetAll(int page,int numberOf,int jezikId)
        {
            int? pageNumber = page;
            MMContext ctx = new MMContext();
            DzamijeVM model = new DzamijeVM();
            model.listDzamije = ctx.Objekats.Where(x => x.ObjekatTranslations.Where(y => y.JezikId == jezikId && y.ObjekatId == x.Id).FirstOrDefault() != null && (x.TipObjektaId == DZAMIJA_ID || x.TipObjektaId == MESDZID_ID)).Select(x => new DzamijeVM.DzamijaInfo
            {
                Id = x.Id,
                lokacijaId = x.LokacijaId,
                naziv = x.ObjekatTranslations.Where(y => y.ObjekatId == x.Id && y.JezikId ==jezikId).FirstOrDefault().Naziv,
                SlikaThumbnail = x.Galerija.SlikaInfos.FirstOrDefault().SlikaThumbnail
            }).OrderBy(x => x.naziv).ToPagedList(pageNumber??1,numberOf);
            model.totalItemsCount = model.listDzamije.TotalItemCount;
            
            return model;
        }

        [HttpGet]
        // GET: Objekati
        public DzamijeVM Search(string tekst,int numberOf,int jezikId)
        {
            int? pageNumber = 1;
            MMContext ctx = new MMContext();
            DzamijeVM model = new DzamijeVM();
            model.listDzamije = ctx.Objekats.Where(x => x.ObjekatTranslations.Where(y => y.JezikId == jezikId && y.ObjekatId == x.Id).FirstOrDefault() != null && (x.TipObjektaId == DZAMIJA_ID || x.TipObjektaId == MESDZID_ID)).Select(x => new DzamijeVM.DzamijaInfo
            {
                Id = x.Id,
                lokacijaId = x.LokacijaId,
                naziv = x.ObjekatTranslations.Where(y=>y.ObjekatId==x.Id && y.JezikId==jezikId).FirstOrDefault().Naziv,
                SlikaThumbnail = x.Galerija.SlikaInfos.FirstOrDefault().SlikaThumbnail
            }).Where(x=>x.naziv.ToLower().Contains(tekst) || tekst==null).OrderBy(x=>x.naziv).ToPagedList(pageNumber??1,numberOf);//x.opis.ToLower().Contains(tekst) ||
            model.totalItemsCount = model.listDzamije.TotalItemCount;
            return model;
        }



        [HttpGet]
        public DzamijeVM GetDzamijeByMedzlisId(int medzlisId,int jezikId)
        {
            int? pageNumber = 1;
            MMContext ctx = new MMContext();
            DzamijeVM model = new DzamijeVM();
            model.listDzamije = ctx.Objekats.Where(x => x.ObjekatTranslations.Where(y => y.JezikId == jezikId && y.ObjekatId == x.Id).FirstOrDefault() != null && (x.TipObjektaId==DZAMIJA_ID || x.TipObjektaId==MESDZID_ID)).Where(x=>x.MedzlisId==medzlisId).Select(x => new DzamijeVM.DzamijaInfo
            {
                Id = x.Id,
                lokacijaId = x.LokacijaId,
                naziv = x.ObjekatTranslations.Where(y => y.ObjekatId == x.Id && y.JezikId == jezikId).FirstOrDefault().Naziv
                //SlikaThumbnail = x.Galerija.SlikaInfos.FirstOrDefault().SlikaThumbnail
            }).OrderBy(x=>x.naziv).ToPagedList(pageNumber??1, ctx.Objekats.Where(x => x.MedzlisId == medzlisId).Count());
            model.totalItemsCount = model.listDzamije.TotalItemCount;

            return model;
        }
    }
}
