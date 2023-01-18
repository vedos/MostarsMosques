using DiplomskiRadMostarsMosques_API.Models;
using DiplomskiRadMostarsMosques_Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PagedList;
using System.IO;
using System.Drawing;
using DiplomskiRadMostarsMosques_API.Helper;

namespace DiplomskiRadMostarsMosques_API.Controllers
{
    public class VijestiController : ApiController
    {
        [HttpGet]
        // GET: Objekati
        public VijestiVM GetAll(int page,int numberOf, int jezikId)
        {
            int? pageNumber = page;
            int takelength = 120;
            MMContext ctx = new MMContext();
            VijestiVM model = new VijestiVM();
            model.listVijesti = ctx.Vijests.Where(x => x.VijestTranslations.Where(y => y.JezikId == jezikId && y.VijestId == x.Id).FirstOrDefault() != null).Select(x => new VijestiVM.VijestInfo
            {
                Id = x.Id,
                naslov = x.VijestTranslations.Where(y=>y.VijestId==x.Id&&y.JezikId==jezikId).FirstOrDefault().Naziv,
                opis = x.VijestTranslations.Where(y => y.VijestId == x.Id && y.JezikId == jezikId).FirstOrDefault().Opis.Substring(0,takelength),
                SlikaThumbnail = x.Galerija.SlikaInfos.FirstOrDefault().SlikaThumbnail,
                DatumObjave=x.DatumPostavljanja
            }).OrderByDescending(x=>x.DatumObjave).ToPagedList(pageNumber?? 1,numberOf);

            model.totalItemCount = model.listVijesti.TotalItemCount;
            return model;
        }

        [HttpGet]
        // GET: Objekati
        public VijestiVM.VijestInfo Get(int id, int jezikId)
        {
            MMContext ctx = new MMContext();
            VijestiVM.VijestInfo model = new VijestiVM.VijestInfo();
            model = ctx.Vijests.Where(x => x.VijestTranslations.Where(y => y.JezikId == jezikId && y.VijestId == x.Id).FirstOrDefault() != null).Select(x => new VijestiVM.VijestInfo
            {
                Id = x.Id,
                naslov = x.VijestTranslations.Where(y => y.VijestId == x.Id && y.JezikId == jezikId).FirstOrDefault().Naziv,
                opis = x.VijestTranslations.Where(y => y.VijestId == x.Id && y.JezikId == jezikId).FirstOrDefault().Opis,
                Slika = x.Galerija.SlikaInfos.FirstOrDefault().Slika,
                DatumObjave = x.DatumPostavljanja
            }).Where(x => x.Id == id).FirstOrDefault();
            if (model.Slika != null)
            {
                MemoryStream ms = new MemoryStream(model.Slika, 0, model.Slika.Length);
                ms.Position = 0;
                Image image = Image.FromStream(ms, true);

                image = GlobalApi.ResizeImage(image, new Size(460, 320), 440, 310);

                MemoryStream ms2 = new MemoryStream();
                image.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
                model.Slika = ms2.ToArray();
            }

            return model;
        }
    }
}
