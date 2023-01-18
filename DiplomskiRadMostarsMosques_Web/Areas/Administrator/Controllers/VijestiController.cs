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
    public class VijestiController : Controller
    {
        MMContext ctx = new MMContext();
        // GET: Administrator/Vijesti
        public ActionResult Index(int? page, int? JezikId)
        {
            VijestVM model = new VijestVM();
            ViewBag.Current = "Vijesti";
            int jezikId = 1;
            if (JezikId.HasValue)
            {
                jezikId = JezikId.Value;
            }
            else
            {
                jezikId = 1; //hard code bosanski
            }
            model.JezikId = jezikId;
            model.JezikStavke = ucitajJezike();
            
            model.vijesti = ctx.Vijests.Where(y=>y.VijestTranslations.Where(a=>a.JezikId==jezikId && a.VijestId==y.Id).FirstOrDefault()!=null).Select(x=>new VijestVM.VijestInfo {
                Id = x.Id,
                Tekst = x.VijestTranslations.Where(y=>y.JezikId== jezikId && y.VijestId == x.Id).FirstOrDefault().Opis.Substring(0,80),
                DatumPostavljanja = x.DatumPostavljanja,
                Naslov = x.VijestTranslations.Where(y => y.JezikId == jezikId && y.VijestId == x.Id).FirstOrDefault().Naziv
               }).OrderByDescending(x=>x.DatumPostavljanja).ToPagedList(page ?? 1, 6);

            
            return View(model);
        }
        private List<SelectListItem> ucitajJezike()
        {
            var jezici = new List<SelectListItem>();
            jezici.AddRange(ctx.Jeziks.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Naziv }).ToList());
            return jezici;
        }

        public ActionResult Pretraga(string tekst,int? JezikId)
        {
            int? page = 1;
            VijestVM model = new VijestVM();
            ViewBag.Current = "Vijesti";
            int jezikId = 1;
            if (JezikId.HasValue)
            {
                jezikId = JezikId.Value;
            }
            else
            {
                jezikId = 1; //hard code bosanski
            }
            model.vijesti = ctx.Vijests.Where(y => y.VijestTranslations.Where(a => a.JezikId == jezikId && a.VijestId == y.Id).FirstOrDefault() != null).Select(x => new VijestVM.VijestInfo
            {
                Id = x.Id,
                Tekst = x.VijestTranslations.Where(y => y.JezikId == jezikId && y.VijestId == x.Id).FirstOrDefault().Opis.Substring(0, 80),
                DatumPostavljanja = x.DatumPostavljanja,
                Naslov = x.VijestTranslations.Where(y => y.JezikId == jezikId && y.VijestId == x.Id).FirstOrDefault().Naziv
            }).OrderByDescending(x => x.DatumPostavljanja).Where(x=>x.Naslov.ToLower().Contains(tekst.ToLower())
            || x.Tekst.ToLower().Contains(tekst.ToLower()) || tekst=="").ToPagedList(page ?? 1, 6);
            model.JezikId = JezikId.Value;
            model.JezikStavke = ucitajJezike();
            return View("Index",model);
        }

        public ActionResult Dodaj(int? JezikId)
        {
            VijestEditVM model = new VijestEditVM();
            if (JezikId.HasValue)
            {
                model.JezikId = JezikId;
            }
            else {
                model.JezikId = 1;
            }
            model.JezikStavke = ucitajJezike();
            return View("Uredi",model);
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
        public ActionResult Uredi(int vijestId)
        {
            
            VijestEditVM model = ctx.Vijests.Where(x=>x.Id==vijestId).Select(x=> new VijestEditVM {
                Id=x.Id,
                DatumPostavljanja = x.DatumPostavljanja,
                Naslov = x.VijestTranslations.Where(y => y.JezikId == x.VijestTranslations.FirstOrDefault().JezikId && y.VijestId == x.Id).FirstOrDefault().Naziv,
                Tekst = x.VijestTranslations.Where(y => y.JezikId == x.VijestTranslations.FirstOrDefault().JezikId && y.VijestId == x.Id).FirstOrDefault().Opis.Substring(0, 80),
                slikeIds = x.Galerija.SlikaInfos.Where(y=>y.GalerijaId == x.GalerijaId).Select(y=>y.Id).ToList(),
                JezikId = x.VijestTranslations.FirstOrDefault().JezikId
            }).FirstOrDefault();
            model.JezikStavke = ucitajJezike();
            return View("Uredi", model);
        }

        public ActionResult ObrisiSliku(int slikaId, int vijestId)
        {
            SlikaInfo s = ctx.SlikaInfos.Find(slikaId);
            ctx.SlikaInfos.Remove(s);
            ctx.SaveChanges();
            //int JezikId = Global.GetJezik(HttpContext).Id;
            VijestEditVM model = ctx.Vijests.Where(x => x.Id == vijestId).Select(x => new VijestEditVM
            {
                Id = x.Id,
                DatumPostavljanja = x.DatumPostavljanja,
                Naslov = x.VijestTranslations.Where(y => y.JezikId == x.VijestTranslations.FirstOrDefault().JezikId && y.VijestId == x.Id).FirstOrDefault().Naziv,
                Tekst = x.VijestTranslations.Where(y => y.JezikId == x.VijestTranslations.FirstOrDefault().JezikId && y.VijestId == x.Id).FirstOrDefault().Opis.Substring(0, 80),
                slikeIds = x.Galerija.SlikaInfos.Where(y => y.GalerijaId == x.GalerijaId).Select(y => y.Id).ToList()
            }).FirstOrDefault();
            model.JezikStavke = ucitajJezike();
            return View("Uredi", model);
        }

        public bool CheckValid(VijestEditVM model) {
                return ModelState.IsValid;
        }

        public ActionResult Snimi(VijestEditVM model)
        {
            model.input_slika = Global.input_slika;
            int JezikId = model.JezikId.Value;
            if (!ModelState.IsValid)
            {
                model.JezikStavke = ucitajJezike();
                ViewBag.Valid = "false";
                return View("Uredi",model);
            }
            else{
                ViewBag.Valid = "true";
            }
            Vijest vijestDB;
            VijestTranslation vt;
            if (model.Id == 0)
            {
                vijestDB = new Vijest();
                vt = new VijestTranslation();
                ctx.Vijests.Add(vijestDB);
                ctx.VijestTranslations.Add(vt);
            }
            else
            {
                vijestDB = ctx.Vijests.Include(x=>x.Galerija).Where(x=>x.Id==model.Id).FirstOrDefault();
                vt = ctx.VijestTranslations.Where(x => x.VijestId == model.Id && x.JezikId == JezikId).FirstOrDefault();
                if (vt == null)
                {
                    vt = new VijestTranslation();
                    ctx.VijestTranslations.Add(vt);
                }

            }
            vt.Naziv = model.Naslov;
            vt.Opis = model.Tekst;
            vt.JezikId = JezikId;
            vt.VijestId = model.Id;
            vijestDB.DatumPostavljanja = DateTime.Now;


            try
            {
                if (vijestDB.GalerijaId != 0)
                {
                    if (vijestDB.Galerija == null)
                    {
                        vijestDB.Galerija = new Galerija();
                        ctx.Galerijas.Add(vijestDB.Galerija);
                    }
                    if (model.input_slika != null) { 
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
                                }
                                #endregion

                                vijestDB.Galerija.SlikaInfos.Add(s);
                                vijestDB.Galerija.Naziv = model.NazivGalerije;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                Console.Write(ex.Message);
            }
            ctx.SaveChanges();

           return RedirectToAction("Dodaj",model.JezikId);
        }

        public ActionResult Obrisi(int vijestId)
        {
            Vijest vijestDB = ctx.Vijests.Include(a=>a.Galerija).Include(a=>a.VijestTranslations).Where(x=>x.Id == vijestId).FirstOrDefault();
            int JezikId = vijestDB.VijestTranslations.FirstOrDefault().JezikId;
            ctx.VijestTranslations.RemoveRange(vijestDB.VijestTranslations);
            ctx.Galerijas.Remove(vijestDB.Galerija);
            ctx.Vijests.Remove(vijestDB);
            ctx.SaveChanges();
            return RedirectToAction("Index","Vijesti", new { JezikId = JezikId });
        }

        public FileContentResult Show(int id)
        {
            SlikaInfo document = ctx.SlikaInfos.Find(id);
            return new FileContentResult(document.SlikaThumbnail,".jpg");
        }
    }
}