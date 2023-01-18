using DiplomskiRadMostarsMosques_Data.DAL;
using DiplomskiRadMostarsMosques_Data.Models;
using DiplomskiRadMostarsMosques_Web.Areas.Administrator.Models;
using DiplomskiRadMostarsMosques_Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Drawing;
using System.IO;
using PagedList;
using System.Configuration;

namespace DiplomskiRadMostarsMosques_Web.Areas.Administrator.Controllers
{
    [Autorizacija(KorisnickeUloge.Administrator)]
    public class DogadjajiController : Controller
    {
        MMContext ctx = new MMContext();
        // GET: Administrator/Dogadjaji
        public ActionResult Index(int? page, int? JezikId)
        {
            DogadjajVM model = new DogadjajVM();
            ViewBag.Current = "Dogadjaji";


            int jezikId = 1;
            if (JezikId.HasValue)
            {
                jezikId = JezikId.Value;
            }
            else
            {
                jezikId = 1; //hard code bosanski
            }

            model.JezikId = JezikId;
            model.JezikStavke = ucitajJezike();
            model.dogadjaji = ctx.Dogadjajs.Where(y => y.DogadjajTranslations.Where(a => a.JezikId == jezikId && a.DogadjajId == y.Id).FirstOrDefault() != null).OrderByDescending(x=>x.DatumPostavljanja).Select(x=>new DogadjajVM.DogadjajInfo {
                   DatumOdrzavanja=x.DatumOdrzavanja,
                   DatumPostavljanja=x.DatumPostavljanja,
                   Id=x.Id,
                   Naslov= x.DogadjajTranslations.Where(y => y.JezikId == jezikId && y.DogadjajId == x.Id).FirstOrDefault().Naziv,
                   Opis= x.DogadjajTranslations.Where(y => y.JezikId ==jezikId && y.DogadjajId == x.Id).FirstOrDefault().Opis.Substring(0,50)
               }).ToPagedList(page ?? 1, 6);
            return View(model);
        }

        private List<SelectListItem> ucitajJezike()
        {
            var jezici = new List<SelectListItem>();
            jezici.AddRange(ctx.Jeziks.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Naziv }).ToList());
            return jezici;
        }

        public bool CheckValid(DogadjajEditVM model)
        {
            return ModelState.IsValid;
        }

        public ActionResult Dodaj(int? JezikId)
        {
            DogadjajEditVM model = new DogadjajEditVM();
            if (JezikId.HasValue)
            {
                model.JezikId = JezikId;
            }
            else {
                model.JezikId = 1;
            }
            model.JezikStavke = ucitajJezike();
            model.DatumOdrzavanja = DateTime.Now.ToString();
            return View("Uredi",model);
        }

        public ActionResult Pretraga(string tekst, int? JezikId)
        {
            int? page = 1;
            ViewBag.Current = "Dogadjaji";
            int jezikId = 1;
            if (JezikId.HasValue)
            {
                jezikId = JezikId.Value;
            }
            else
            {
                jezikId = 1; //hard code bosanski
            }
            DogadjajVM model = new DogadjajVM();
             model.dogadjaji = ctx.Dogadjajs.Where(y => y.DogadjajTranslations.Where(a => a.JezikId == jezikId && a.DogadjajId == y.Id).FirstOrDefault() != null).OrderByDescending(x => x.DatumPostavljanja).Where(x => x.DogadjajTranslations.Where(y=>y.DogadjajId==x.Id && y.JezikId==JezikId).FirstOrDefault().Naziv.ToLower().Contains(tekst.ToLower())
            || x.DogadjajTranslations.Where(y => y.DogadjajId == x.Id && y.JezikId == jezikId).FirstOrDefault().Opis.ToLower().Contains(tekst.ToLower()) || tekst=="").OrderByDescending(x=>x.DatumPostavljanja).Select(x => new DogadjajVM.DogadjajInfo
            {
                DatumOdrzavanja = x.DatumOdrzavanja,
                DatumPostavljanja = x.DatumPostavljanja,
                Id = x.Id,
                Naslov = x.DogadjajTranslations.Where(y => y.DogadjajId == x.Id && y.JezikId == jezikId).FirstOrDefault().Naziv,
                Opis = x.DogadjajTranslations.Where(y => y.DogadjajId == x.Id && y.JezikId == jezikId).FirstOrDefault().Opis.Substring(0,50)
            }).ToPagedList(page ?? 1, 6);
            model.JezikId = JezikId.Value;
            model.JezikStavke = ucitajJezike();
            return View("Index", model);
        }

        public ActionResult Uredi(int dogadjajId)
        {
           
            DogadjajEditVM model = ctx.Dogadjajs.Where(x => x.Id == dogadjajId).Select(x => new DogadjajEditVM
            {
                Id = x.Id,
                Naziv = x.DogadjajTranslations.Where(y => y.DogadjajId == x.Id && y.JezikId == x.DogadjajTranslations.FirstOrDefault().JezikId).FirstOrDefault().Naziv,
                Tekst = x.DogadjajTranslations.Where(y => y.DogadjajId == x.Id && y.JezikId == x.DogadjajTranslations.FirstOrDefault().JezikId).FirstOrDefault().Opis,
                DatumOdrzavanja = x.DatumOdrzavanja.ToString(),
                DatumPostavljanja = x.DatumPostavljanja,
                slikeIds = ctx.SlikaInfos.Where(y => y.GalerijaId == x.GalerijaId).Select(y => y.Id).ToList(),
                JezikId = x.DogadjajTranslations.FirstOrDefault().JezikId
            }).FirstOrDefault();
            model.JezikStavke = ucitajJezike();
            return View(model);
        }

        public ActionResult ObrisiSliku(int slikaId, int dogadjajId)
        {
            SlikaInfo s = ctx.SlikaInfos.Find(slikaId);
            ctx.SlikaInfos.Remove(s);
            ctx.SaveChanges();
           // int JezikId = Global.GetJezik(HttpContext).Id;
            DogadjajEditVM model = ctx.Dogadjajs.Where(x => x.Id == dogadjajId).Select(x => new DogadjajEditVM
            {
                Id = x.Id,
                Naziv = x.DogadjajTranslations.Where(y => y.DogadjajId == x.Id && y.JezikId == x.DogadjajTranslations.FirstOrDefault().JezikId).FirstOrDefault().Naziv,
                Tekst = x.DogadjajTranslations.Where(y => y.DogadjajId == x.Id && y.JezikId == x.DogadjajTranslations.FirstOrDefault().JezikId).FirstOrDefault().Opis,
                DatumOdrzavanja = x.DatumOdrzavanja.ToString(),
                DatumPostavljanja = x.DatumPostavljanja,
                slikeIds = ctx.SlikaInfos.Where(y => y.GalerijaId == x.GalerijaId).Select(y => y.Id).ToList(),
                JezikId=x.DogadjajTranslations.FirstOrDefault().JezikId
            }).FirstOrDefault();
            model.JezikStavke = ucitajJezike();
            return View("Uredi", model);
        }

        public ActionResult Obrisi(int dogadjajId)
        {
            Dogadjaj dogadjajDB = ctx.Dogadjajs.Where(x=>x.Id==dogadjajId).Include(x=>x.DogadjajTranslations).Include(x=>x.Galerija).FirstOrDefault();
            int JezikId = dogadjajDB.DogadjajTranslations.FirstOrDefault().JezikId;
            ctx.DogadjajiTranslations.RemoveRange(dogadjajDB.DogadjajTranslations);
            ctx.Galerijas.Remove(dogadjajDB.Galerija);
            ctx.Dogadjajs.Remove(dogadjajDB);
            ctx.SaveChanges();
            return RedirectToAction("Index","Dogadjaji",new { JezikId=JezikId });
        }

        [HttpPost]
        public void Upload()
        {
            Global.input_slika = new List<HttpPostedFileBase>();
            for (int i = 0; i < Request.Files.Count; i++)
            {
                var file = Request.Files[i];
                var fileName = Path.GetFileName(file.FileName);

                Global.input_slika.Add(file);

            }
        }

        public ActionResult Snimi(DogadjajEditVM model)
        {
            int JezikId = model.JezikId.Value;
           
            model.input_slika = Global.input_slika;
            if (!ModelState.IsValid)
            {
                model.JezikStavke = ucitajJezike();
                return View("Uredi",model);
            }
            Dogadjaj dogadjajDB;
            DogadjajiTranslation dt;

            if (model.Id == 0)
            {
                dogadjajDB = new Dogadjaj();
                dt = new DogadjajiTranslation();
                ctx.Dogadjajs.Add(dogadjajDB);
                ctx.DogadjajiTranslations.Add(dt);
            }
            else
            {
                dogadjajDB = ctx.Dogadjajs.Include(x=>x.Galerija).Where(x=>x.Id==model.Id).FirstOrDefault();
                dt = ctx.DogadjajiTranslations.Where(x => x.JezikId == JezikId && x.DogadjajId == model.Id).FirstOrDefault();
                if (dt == null)
                {
                    dt= new DogadjajiTranslation();
                    ctx.DogadjajiTranslations.Add(dt);
                }
            }
            dt.Naziv = model.Naziv;
            dt.Opis = model.Tekst;
            dt.JezikId = JezikId;
            dt.DogadjajId = dogadjajDB.Id;
            dogadjajDB.DatumOdrzavanja = Convert.ToDateTime(model.DatumOdrzavanja);
            dogadjajDB.DatumPostavljanja = DateTime.Now;
            try
            {
                if (model.GalerijaId != 0)
                {
                    if (dogadjajDB.Galerija == null)
                    {
                        dogadjajDB.Galerija = new Galerija();
                        ctx.Galerijas.Add(dogadjajDB.Galerija);
                    }
                    if (model.input_slika != null)
                    {
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


                                    #region Slika Thumbnail
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
                                    #endregion
                                }

                                dogadjajDB.Galerija.SlikaInfos.Add(s);
                                dogadjajDB.Galerija.Naziv = model.NazivGalerije;
                            }
                        }
                    }


                }
            }
            catch { }

            ctx.SaveChanges();
            
            return RedirectToAction("Dodaj",model.JezikId);
        }



        public FileContentResult Show(int id)
        {
            SlikaInfo document = ctx.SlikaInfos.Find(id);
            return new FileContentResult(document.SlikaThumbnail, ".jpg");
        }
    }
}