using DiplomskiRadMostarsMosques_API.Models;
using DiplomskiRadMostarsMosques_Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using PagedList;
using DiplomskiRadMostarsMosques_Data.Models;
using System.Data.Entity;
using System.IO;
using System.Drawing;
using DiplomskiRadMostarsMosques_API.Helper;

namespace DiplomskiRadMostarsMosques_API.Controllers
{
    public class GalerijaController : ApiController
    {
        [HttpGet]
        // GET: Objekati
        public GalerijaVM GetAll(int page)
        {
            int? pageNumber = page;
            MMContext ctx = new MMContext();
            GalerijaVM model = new GalerijaVM();
            model.listSlika = ctx.SlikaInfos.Select(x => new GalerijaVM.SlikaInfo
            {
                id = x.Id,
                naziv = x.naziv,
                SlikaThumbnail = x.SlikaThumbnail
            }).OrderBy(x=>x.id).ToPagedList(pageNumber ?? 1,1);

            model.totalItemsCount = model.listSlika.TotalItemCount;
            return model;
        }

        [HttpGet]
        // GET: Objekati
        public GalerijaVM.SlikaInfo Get(int id)
        {
            MMContext ctx = new MMContext();
            GalerijaVM.SlikaInfo model = new GalerijaVM.SlikaInfo();
            model = ctx.SlikaInfos.Select(x => new GalerijaVM.SlikaInfo
            {
                id = x.Id,
                naziv = x.naziv,
                Slika = x.Slika
                
            }).Where(x=>x.id==id).FirstOrDefault();

            MemoryStream ms = new MemoryStream(model.Slika, 0, model.Slika.Length);
            ms.Position = 0;
            Image image = Image.FromStream(ms, true);

            image = GlobalApi.ResizeImage(image, new Size(860, 620), 440, 310);

            MemoryStream ms2 = new MemoryStream();
            image.Save(ms2, System.Drawing.Imaging.ImageFormat.Jpeg);
            model.Slika = ms2.ToArray();

            return model;
        }

        [HttpGet]
        // GET: Objekati
        public GalerijaVM GetByObjekatId(int id)
        {
            int? page = 1;
            int brojSlika = 0;
            MMContext ctx = new MMContext();
            GalerijaVM model = new GalerijaVM();


            //brojSlika = ctx.Objekats.Where(x => x.Id == id).FirstOrDefault().Galerija.SlikaInfos.Count;
            /* model.listSlika = ctx.Objekats.Select(x => x.Galerija).Where(x => x.Id == id).SelectMany(x => x.SlikaInfos).Select(x => new GalerijaVM.SlikaInfo
             {

                 id = x.Id,
                 naziv = x.naziv,
                 SlikaThumbnail = x.SlikaThumbnail
             }).OrderBy(x => x.id).ToPagedList(page ?? 1, brojSlika);*/

            Objekat o = ctx.Objekats.Include(x=>x.Galerija.SlikaInfos).Where(x => x.Id == id).FirstOrDefault();

            model.listSlika = o.Galerija.SlikaInfos.Select(y => new GalerijaVM.SlikaInfo
            {
                id = y.Id,
                naziv = y.naziv,
                SlikaThumbnail = y.SlikaThumbnail

            }).OrderBy(y => y.id).ToPagedList(page ?? 1, o.Galerija.SlikaInfos.Count);

            return model;
        }
    }
}
