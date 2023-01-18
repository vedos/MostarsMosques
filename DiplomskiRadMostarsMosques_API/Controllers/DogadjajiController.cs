using DiplomskiRadMostarsMosques_API.Helper;
using DiplomskiRadMostarsMosques_API.Models;
using DiplomskiRadMostarsMosques_Data.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DiplomskiRadMostarsMosques_API.Controllers
{
    public class DogadjajiController : ApiController
    {
        [HttpGet]
        public DogadjajiVM GetCurrentMonth(int jezikId)
        {
            MMContext ctx = new MMContext();
            DogadjajiVM model = new DogadjajiVM();

            model.listaDogadjaja = ctx.Dogadjajs.Where(x => x.DogadjajTranslations.Where(y => y.JezikId == jezikId && y.DogadjajId == x.Id).FirstOrDefault() != null).Where(x=>x.DatumOdrzavanja.Year == DateTime.Now.Year).Select(x => new DogadjajiVM.DogadjajInfo
            {
                datum = x.DatumOdrzavanja,
                Id = x.Id
            }).ToList();

            return model;
        }


        [HttpGet]
        public DogadjajiVM.DogadjajInfo GetDogadjajById(int id,int jezikId)
        {
            MMContext ctx = new MMContext();
            DogadjajiVM.DogadjajInfo model = new DogadjajiVM.DogadjajInfo();
            model = ctx.Dogadjajs.Where(x => x.Id == id).Where(x=>x.DogadjajTranslations.Where(y=>y.JezikId==jezikId && y.DogadjajId==x.Id).FirstOrDefault()!=null).Select(x => new DogadjajiVM.DogadjajInfo
            {
                datum = x.DatumOdrzavanja,
                Id = x.Id,
                Naziv = x.DogadjajTranslations.Where(y=>y.DogadjajId==x.Id && y.JezikId== jezikId).FirstOrDefault().Naziv,
                Opis = x.DogadjajTranslations.Where(y => y.DogadjajId == x.Id && y.JezikId == jezikId).FirstOrDefault().Opis,
                SlikaThumbnail = x.Galerija.SlikaInfos.FirstOrDefault().SlikaThumbnail
            }).FirstOrDefault();

            return model;
        }

    }
}
