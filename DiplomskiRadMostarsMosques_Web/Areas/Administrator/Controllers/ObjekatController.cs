using DiplomskiRadMostarsMosques_Data.DAL;
using DiplomskiRadMostarsMosques_Data.Models;
using DiplomskiRadMostarsMosques_Web.Areas.Administrator.Models;
using DiplomskiRadMostarsMosques_Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using System.Configuration;

namespace DiplomskiRadMostarsMosques_Web.Areas.Administrator.Controllers
{
    [Autorizacija(KorisnickeUloge.Korisnik)]
    public class ObjekatController : Controller
    {
        // GET: Administrator/Objekat
        MMContext ctx = new MMContext();
        public ActionResult Index()
        {
            JeziciVM model = new JeziciVM();
            model = ucitajJezikPrikaz();
            ViewBag.Current = "Objekti";
            return View("PrikaziObjekte",model);
        }

        public FileContentResult Show(int id)
        {
            SlikaInfo document = ctx.SlikaInfos.Find(id);
            return new FileContentResult(document.SlikaThumbnail, ".jpg");
        }
        public ActionResult ObrisiSliku(int slikaId, int objekatId)
        {
            SlikaInfo s = ctx.SlikaInfos.Find(slikaId);
            ctx.SlikaInfos.Remove(s);
            ctx.SaveChanges();
            ObjekatEditVM model = new ObjekatEditVM();
            int JezikId = Global.GetJezik(HttpContext).Id;
            ObjekatTranslation o = ctx.ObjekatTranslations.Where(x => x.JezikId == JezikId && x.ObjekatId==objekatId).FirstOrDefault();
            model.Opis = o.Opis;
            model.Naziv = o.Naziv;

            model = ctx.Objekats.Select(x => new ObjekatEditVM
            {
                IsUnesco = x.IsUnesco,
                DzematId = x.DzematId,
                GodinaIzgradnje = x.GodinaIzgradnje,
                GodinaObnove = x.GodinaObnove,
                GodinaRusenja = x.GodinaRusenja,
                Id = x.Id,
                IsRusena = x.IsRusena,
                latitude = x.Lokacija.latitude,
                longitude = x.Lokacija.longitude,
                MedzlisId = x.MedzlisId,
                TipObjektaId = x.TipObjektaId,
                AktivnostId = x.AktivnostId,
                Zvuk = x.Zvuk,
                LokacijaId = x.LokacijaId,
                slikeIds = ctx.SlikaInfos.Where(y => y.GalerijaId == x.GalerijaId).Select(y => y.Id).ToList()
            }).Where(x => x.Id == objekatId).FirstOrDefault();
            model.aktivnostStavke = ucitajAktivnosti();
            model.DzematiStavke = ucitajDzemate();
            model.JezikStavke = ucitajJezike();
            model.MedzlisStavke = ucitajMedzlise();
            model.TipoviObjekataStavke = ucitajTipoveObjekata();

            return View("DodajObjekat", model);
        }


        public ActionResult Dodaj()
        {
            ObjekatEditVM model = new ObjekatEditVM();
            ViewBag.Current = "Objekti";
            model.TipoviObjekataStavke = ucitajTipoveObjekata();
            model.DzematiStavke = ucitajDzemate();
            model.MedzlisStavke = ucitajMedzlise();
            model.JezikStavke = ucitajJezike();
            model.aktivnostStavke = ucitajAktivnosti();
            return View("DodajObjekat", model);
        }

        public ActionResult Snimi(ObjekatEditVM model)
        {

           // int JezikId = Global.GetJezik(HttpContext).Id;
            if (!ModelState.IsValid)
            {
                model.TipoviObjekataStavke = ucitajTipoveObjekata();
                model.DzematiStavke = ucitajDzemate();
                model.MedzlisStavke = ucitajMedzlise();
                model.JezikStavke = ucitajJezike();
                model.aktivnostStavke = ucitajAktivnosti();
                return View("DodajObjekat",model);
            }
            ObjekatTranslation otDB;
            
            if (model.Id == 0)
            {
                otDB = new ObjekatTranslation();
                otDB.Objekat = new Objekat();
                otDB.Objekat.Lokacija = new Lokacija();
                ctx.ObjekatTranslations.Add(otDB);
            }
            else
            {
                otDB = ctx.ObjekatTranslations.Where(x => x.ObjekatId == model.Id && x.JezikId == model.JezikId.Value).FirstOrDefault();
                if (otDB == null) {
                    otDB = new ObjekatTranslation();
                    ctx.ObjekatTranslations.Add(otDB);
                }
                otDB.Objekat = ctx.Objekats.Where(x => x.Id == model.Id).Include(x => x.Galerija).FirstOrDefault(); 
                otDB.Objekat.Lokacija = ctx.Lokacijas.Where(x => x.Id == model.LokacijaId).FirstOrDefault();
                
            }
            otDB.Objekat.Lokacija.latitude = model.latitude;
            otDB.Objekat.Lokacija.longitude = model.longitude;
            otDB.Objekat.IsUnesco = model.IsUnesco;
            otDB.Objekat.DzematId = model.DzematId;
            otDB.Objekat.GodinaIzgradnje = model.GodinaIzgradnje;
            otDB.Objekat.GodinaObnove = model.GodinaObnove;
            otDB.Objekat.GodinaRusenja = model.GodinaRusenja;
            otDB.Objekat.IsRusena = model.IsRusena;
            otDB.Objekat.TipObjektaId = model.TipObjektaId;
            otDB.Objekat.AktivnostId = model.AktivnostId;
            otDB.Objekat.MedzlisId = model.MedzlisId;
            model.UcitajSlikeError = "";
            //
            otDB.ObjekatId = model.Id;
            otDB.JezikId = model.JezikId.Value; //jezik
            otDB.Naziv = model.Naziv;
            otDB.Opis = model.Opis;
            try {

                if (otDB.Objekat.GalerijaId != 0)
                {
                    if (otDB.Objekat.Galerija == null)
                    {
                        otDB.Objekat.Galerija = new Galerija();
                        ctx.Galerijas.Add(otDB.Objekat.Galerija);

                    }
                    foreach (HttpPostedFileBase input_slika in model.input_slika)
                    {
                        if (input_slika != null)
                        {
                            SlikaInfo s = new SlikaInfo();
                            if (input_slika.ContentLength > 0 && input_slika.ContentLength < 1500000)
                            {
                                byte[] imgBinaryData = new byte[input_slika.ContentLength];
                                int readresult = input_slika.InputStream.Read(imgBinaryData, 0, input_slika.ContentLength);
                                s.Slika = imgBinaryData;
                                s.naziv = input_slika.FileName;

                                MemoryStream ms = new MemoryStream(s.Slika, 0, s.Slika.Length);
                                ms.Position = 0;
                                Image image = Image.FromStream(ms, true);

                                int thumbWidth = Convert.ToInt32(ConfigurationManager.AppSettings["thumbWidth"]);
                                int thumbHeight = Convert.ToInt32(ConfigurationManager.AppSettings["thumbHeight"]);

                                if ((image.Width > thumbWidth && image.Height > thumbHeight)
                                    || (image.Width == thumbWidth && image.Height > thumbHeight)
                                    || (image.Width > thumbWidth && image.Height == thumbHeight))
                                {
                                    Image resizedImage = Global.ResizeImage(image, new Size(thumbWidth, thumbHeight), thumbHeight, thumbWidth);
                                    Image croppedImage = resizedImage;

                                    int croppedXpos = (resizedImage.Width - thumbWidth) / 2;
                                    int croppedYpos = (resizedImage.Height - thumbHeight) / 2;
                                    if (resizedImage.Width >= thumbWidth && resizedImage.Height >= thumbHeight)
                                    {
                                        croppedImage = Global.CropImage(resizedImage, new Rectangle(croppedXpos, croppedYpos, thumbWidth, thumbHeight));


                                        ms = new MemoryStream();
                                        croppedImage.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                        s.SlikaThumbnail = ms.ToArray();
                                    }

                                }
                                else if (image.Width == thumbWidth && image.Height == thumbHeight)
                                {
                                    ms = new MemoryStream();
                                    image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                    s.SlikaThumbnail = ms.ToArray();

                                }
                                otDB.Objekat.Galerija.SlikaInfos.Add(s);
                                otDB.Objekat.Galerija.Naziv = model.NazivGalerije;
                            }
                            else {
                                model.TipoviObjekataStavke = ucitajTipoveObjekata();
                                model.DzematiStavke = ucitajDzemate();
                                model.MedzlisStavke = ucitajMedzlise();
                                model.JezikStavke = ucitajJezike();
                                model.aktivnostStavke = ucitajAktivnosti();
                                model.UcitajSlikeError = "Slika mora biti veličine do 1,5 MB";
                                return View("DodajObjekat", model);
                            }
                        }
                    }
                }
            }
            catch (Exception e) { }

            ctx.SaveChanges();
            return RedirectToAction("Index");
        }

        public List<SelectListItem> ucitajAktivnosti()
        {
            var aktivnosti = new List<SelectListItem>();
            aktivnosti.Add(new SelectListItem { Value = "", Text = "Da li je objekat aktivan" });
            aktivnosti.AddRange(ctx.Aktivnosts.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Naziv }).ToList());
            return aktivnosti;
        }
        public List<SelectListItem> ucitajTipoveObjekata()
        {
            var tipoviObekata = new List<SelectListItem>();
            tipoviObekata.Add(new SelectListItem { Value = "", Text = "Odaberite tip objekta" });
            tipoviObekata.AddRange(ctx.TipObjektas.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Naziv }).ToList());
            return tipoviObekata;
        }

        public List<SelectListItem> ucitajDzemate()
        {
            var dzemati = new List<SelectListItem>();
            dzemati.Add(new SelectListItem { Value = "", Text = "Odaberite dzemat" });
            dzemati.AddRange(ctx.Dzemats.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Naziv }).ToList());
            return dzemati;
        }

        public List<SelectListItem> ucitajMedzlise()
        {
            var medzlisi = new List<SelectListItem>();
            medzlisi.Add(new SelectListItem { Value = "", Text = "Odaberite medžlis" });
            medzlisi.AddRange(ctx.Medzlis.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.MedzlisTranslations.Where(y=>y.JezikId==1).FirstOrDefault().Naziv }).ToList());
            return medzlisi;
        }

        public List<SelectListItem> ucitajJezike()
        {
            var jezici = new List<SelectListItem>();
            jezici.Add(new SelectListItem { Value = "", Text = "Odaberite jezik" });
            jezici.AddRange(ctx.Jeziks.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Naziv }).ToList());
            return jezici;
        }

        /* public JeziciVM ucitajJezikPrikaz(int objekatId) {

             JeziciVM model = new JeziciVM();
             var jezici = new List<SelectListItem>();
             jezici.Add(new SelectListItem { Value = "", Text = "Odaberite jezik" });
             jezici.AddRange(ctx.ObjekatTranslations.Where(x=>x.ObjekatId==objekatId).Select(y => new SelectListItem { Value = y.JezikId.ToString(), Text = y.Jezik.Naziv }).ToList());

             model.objekatId = objekatId;

             model.JezikStavke = jezici;

             return model;
         }*/

        public JeziciVM ucitajJezikPrikaz()
        {

            JeziciVM model = new JeziciVM();
            var jezici = new List<SelectListItem>();
            jezici.AddRange(ctx.Jeziks.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Naziv }).ToList());
            
            model.JezikStavke = jezici;

            return model;
        }


        public ActionResult Uredi(int objekatId, int JezikId)
        {
            ObjekatEditVM model = new ObjekatEditVM();
            ObjekatTranslation ot = ctx.ObjekatTranslations.Where(x => x.ObjekatId == objekatId && x.JezikId == JezikId).FirstOrDefault();
            if (ot == null) {
                ot = new ObjekatTranslation();
            }

            model = ctx.Objekats.Select(x => new ObjekatEditVM
            {
                IsUnesco = x.IsUnesco,
                DzematId = x.DzematId,
                GodinaIzgradnje = x.GodinaIzgradnje,
                GodinaObnove = x.GodinaObnove,
                GodinaRusenja = x.GodinaRusenja,
                Id = x.Id,
                IsRusena = x.IsRusena,
                latitude = x.Lokacija.latitude,
                longitude = x.Lokacija.longitude,
                MedzlisId = x.MedzlisId,
                TipObjektaId = x.TipObjektaId,
                AktivnostId = x.AktivnostId,
                Zvuk = x.Zvuk,
                LokacijaId = x.LokacijaId,
                slikeIds = ctx.SlikaInfos.Where(y => y.GalerijaId == x.GalerijaId).Select(y => y.Id).ToList()
            }).Where(x => x.Id == objekatId).FirstOrDefault();

            model.Opis = ot.Opis;
            model.Naziv = ot.Naziv;

            model.DzematiStavke = ucitajDzemate();
            model.MedzlisStavke = ucitajMedzlise();
            model.TipoviObjekataStavke = ucitajTipoveObjekata();
            model.JezikStavke = ucitajJezike();
            model.aktivnostStavke = ucitajAktivnosti();
            return View("DodajObjekat", model);
        }

        public ActionResult Obrisi(int objekatId)
        {
            Objekat o = ctx.Objekats.Include(a => a.Lokacija).Include(a=>a.ObjekatTranslations).Include(a => a.Galerija).Where(x => x.Id == objekatId).FirstOrDefault();
            ctx.ObjekatTranslations.RemoveRange(o.ObjekatTranslations);
            ctx.Lokacijas.Remove(o.Lokacija);
            ctx.Galerijas.Remove(o.Galerija);
            ctx.Objekats.Remove(o);
            ctx.SaveChanges();
            return RedirectToAction("Index");
        }
        //

        public class JsonLocation
        {
            public int Id { get; set; }
            public string Naziv { get; set; }
            public string miz { get; set; }
            public List<string> jezici { get; set; }
            public string tipObjekta { get; set; }
            public string latitude { get; set; }
            public string longitude { get; set; }
        }

        public class JsonLocations
        {
            // Pagination
            public int? Page { get; set; }
            public int TotalLocationsCount { get; set; }
            public IPagedList<JsonLocation> locationsPaged { get; set; }
        }

        public class JsonMedzlis
        {
            public int Id { get; set; }
            public string Naziv { get; set; }
        }

        public JsonResult GetMedzlisi()
        {
            var data = ctx.Medzlis.Select(x => new JsonMedzlis
            {
                Id = x.Id,
                Naziv = ctx.MedzlisTranslations.Where(y => y.JezikId == Global.JezikId && y.MedzlisId==x.Id).FirstOrDefault().Naziv
            }).OrderBy(x => x.Naziv).ToList();
           
            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetLatLng(int objekatId)
        {
            var data = ctx.Objekats.Select(x => new JsonLocation
            {
                Id = x.LokacijaId,
                latitude = x.Lokacija.latitude,
                longitude = x.Lokacija.longitude,
                Naziv = ctx.ObjekatTranslations.Where(y => y.JezikId == Global.JezikId && y.ObjekatId == x.Id).FirstOrDefault().Naziv
            }).Where(x => x.Id == objekatId).ToList();

           

            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult SearchLocations(string inputSearch,int? page)
        {
            var data = new JsonLocations();
              data.locationsPaged=  ctx.Objekats.Select(x => new JsonLocation
            {
                Id = x.LokacijaId,
                latitude = x.Lokacija.latitude,
                longitude = x.Lokacija.longitude,
                  Naziv = ctx.ObjekatTranslations.Where(y => y.JezikId == Global.JezikId && y.ObjekatId == x.Id).FirstOrDefault().Naziv,
                  jezici = x.ObjekatTranslations.Select(y=>y.Jezik.Naziv).ToList(),
                  miz = ctx.MedzlisTranslations.Where(y => y.JezikId == Global.JezikId && y.MedzlisId == x.MedzlisId).FirstOrDefault().Naziv,
                  tipObjekta =x.TipObjekta.Naziv
            }).Where(x => x.Naziv.ToLower().Contains(inputSearch.ToLower())).OrderBy(x => x.Naziv).Take(20).ToPagedList(page ?? 1, 5);

            data.TotalLocationsCount = ctx.Objekats.Where(x => x.ObjekatTranslations.Where(y => y.JezikId == Global.JezikId).FirstOrDefault().Naziv.ToLower().Contains(inputSearch.ToLower())).Count();
            data.Page = page;
            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult SelectObjektiByMedzlisId(int medzlisId,int? page)
        {
            var data = new JsonLocations();
              data.locationsPaged=  ctx.Objekats.Where(x=>x.MedzlisId==medzlisId).Select(x => new JsonLocation
            {
                Id = x.LokacijaId,
                latitude = x.Lokacija.latitude,
                longitude = x.Lokacija.longitude,
                  Naziv = ctx.ObjekatTranslations.Where(y => y.JezikId == Global.JezikId && y.ObjekatId == x.Id).FirstOrDefault().Naziv,
                  jezici = x.ObjekatTranslations.Select(y => y.Jezik.Naziv).ToList(),
                  miz = ctx.MedzlisTranslations.Where(y => y.JezikId == Global.JezikId && y.MedzlisId == x.MedzlisId).FirstOrDefault().Naziv,
                  tipObjekta = x.TipObjekta.Naziv
            }).OrderBy(x => x.Naziv).ToPagedList(page ?? 1, 5);
            data.TotalLocationsCount = ctx.Objekats.Where(x => x.MedzlisId == medzlisId).Count();
            data.Page = page;
            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /*public JsonResult GetLates20Location()
        {
            var data = ctx.Objekats.Select(x => new JsonLocations
            {
                Id = x.LokacijaId,
                latitude = x.Lokacija.latitude,
                longitude = x.Lokacija.longitude,
                Naziv = x.Naziv,
                MIZ = x.Medzlis.Naziv,
                tipObjekta = x.TipObjekta.Naziv
            }).OrderBy(x => x.Naziv).Take(20).ToList();
            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }*/

        public JsonResult GetAllLocation(int? page)
        {
            var data = new JsonLocations();
            data.locationsPaged = ctx.Objekats.Select(x=>new JsonLocation {
                Id = x.Id,
                latitude = x.Lokacija.latitude,
                longitude = x.Lokacija.longitude,
                Naziv = ctx.ObjekatTranslations.Where(y => y.JezikId == Global.JezikId && y.ObjekatId == x.Id).FirstOrDefault().Naziv,
                jezici = x.ObjekatTranslations.Select(y => y.Jezik.Naziv).ToList(),
                miz = ctx.MedzlisTranslations.Where(y => y.JezikId == Global.JezikId && y.MedzlisId == x.MedzlisId).FirstOrDefault().Naziv,
                tipObjekta = x.TipObjekta.Naziv
            }).OrderBy(x=>x.Naziv).ToPagedList(page ?? 1, 5);
            data.TotalLocationsCount = ctx.Objekats.Count();
            data.Page = page;
            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetMarkerInfo(int locationID)
        {
            JsonLocation l = null;
            l = ctx.Objekats.Select(x=>new JsonLocation {
                Id = x.LokacijaId,
                latitude = x.Lokacija.latitude,
                longitude = x.Lokacija.longitude,
                Naziv = ctx.ObjekatTranslations.Where(y => y.JezikId == Global.JezikId && y.ObjekatId == x.Id).FirstOrDefault().Naziv
            }).Where(x => x.Id.Equals(locationID)).FirstOrDefault();
            return new JsonResult { Data = l, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAllMarkersInfo()
        {
            var data = ctx.Objekats.Select(x => new JsonLocation
            {
                Id = x.LokacijaId,
                latitude = x.Lokacija.latitude,
                longitude = x.Lokacija.longitude,
                Naziv = ctx.ObjekatTranslations.Where(y => y.JezikId == Global.JezikId && y.ObjekatId == x.Id).FirstOrDefault().Naziv
            }).ToList();
            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetMarkersByMedzlisId(int medzlisId)
        {
            var data = ctx.Objekats.Where(x=>x.MedzlisId==medzlisId).Select(x => new JsonLocation
            {
                Id = x.LokacijaId,
                latitude = x.Lokacija.latitude,
                longitude = x.Lokacija.longitude,
                Naziv = ctx.ObjekatTranslations.Where(y => y.JezikId == Global.JezikId && y.ObjekatId == x.Id).FirstOrDefault().Naziv
            }).ToList();
            return new JsonResult { Data = data, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }

}