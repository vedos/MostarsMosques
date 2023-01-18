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
    public class MedzlisController : ApiController
    {
        public MedzlisVM GetAll(int jezikId)
        {
            MMContext ctx = new MMContext();
            MedzlisVM model = new MedzlisVM();

            model.medzlisi = ctx.Medzlis.Where(x => x.MedzlisTranslations.Where(y => y.JezikId == jezikId && y.MedzlisId == x.Id).FirstOrDefault() != null).Select(x => new MedzlisVM.MedzlisInfo
            {
                Id = x.Id,
                Naziv = x.MedzlisTranslations.Where(y=>y.MedzlisId==x.Id&&y.JezikId==jezikId).FirstOrDefault().Naziv,
                Opis = x.MedzlisTranslations.Where(y => y.MedzlisId == x.Id && y.JezikId == jezikId).FirstOrDefault().Opis
            }).ToList();
            return model;
        }
    }
}
