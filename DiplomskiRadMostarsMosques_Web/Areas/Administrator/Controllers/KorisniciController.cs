using DiplomskiRadMostarsMosques_Data.DAL;
using DiplomskiRadMostarsMosques_Data.Models;
using DiplomskiRadMostarsMosques_Web.Areas.Administrator.Models;
using DiplomskiRadMostarsMosques_Web.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace DiplomskiRadMostarsMosques_Web.Areas.Administrator.Controllers
{
    [Autorizacija(KorisnickeUloge.Administrator)]
    public class KorisniciController : Controller
    {
        MMContext ctx = new MMContext();
        // GET: Administrator/Korisnici
        public ActionResult Index()
        {
            List<Korisnik> model = new List<Korisnik>();
            ViewBag.IsMaster = true;
            ViewBag.Current = "Korisnici";
            int masterId = Global.superuserId;
            int currentId = Autentifikacija.GetLogiraniKorisnik(this.HttpContext).Id;
                model = ctx.Korisniks.Where(x => x.Id != masterId).ToList();
            return View(model);
        }

        public bool CheckValid(KorisniciEditVM model)
        {
            return ModelState.IsValid;
        }

        public ActionResult Dodaj() {

            KorisniciEditVM model = new KorisniciEditVM();

            return View(model);
        }

        public ActionResult Snimi(KorisniciEditVM model)
        {
            if (!ModelState.IsValid)
            {
                return View("Dodaj",model);
            }
            try {
                DiplomskiRadMostarsMosques_Data.Models.Administrator adminDB;
                Korisnik korisnikDB;
                    if (model.Id == 0)
                    {
                        if (model.isAdministrator)
                        {
                            adminDB = new DiplomskiRadMostarsMosques_Data.Models.Administrator();
                            adminDB.Korisnik = new Korisnik();
                            adminDB.Korisnik.Email = model.Email;
                            adminDB.Korisnik.Ime = model.Ime;
                            adminDB.Korisnik.Prezime = model.Prezime;
                            adminDB.Korisnik.Username = model.Username;
                            adminDB.Korisnik.Password = model.Password;
                            ctx.Korisniks.Add(adminDB.Korisnik);
                            ctx.Administrators.Add(adminDB);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            korisnikDB = new Korisnik();
                            korisnikDB.Email = model.Email;
                            korisnikDB.Ime = model.Ime;
                            korisnikDB.Prezime = model.Prezime;
                            korisnikDB.Username = model.Username;
                            korisnikDB.Password = model.Password;
                            ctx.Korisniks.Add(korisnikDB);
                            ctx.SaveChanges();
                        }
                    }
                    else
                    {
                        if (model.isAdministrator)
                        {
                            var a = ctx.Administrators.Where(x => x.KorisnikId == model.Id).SingleOrDefault();
                            if (a == null)
                            {
                                adminDB = new DiplomskiRadMostarsMosques_Data.Models.Administrator();
                                adminDB.KorisnikId = model.Id;
                                ctx.Administrators.Add(adminDB);
                            }
                            else
                            {
                                adminDB = ctx.Administrators.Where(x => x.KorisnikId == model.Id).Single();
                            }

                            adminDB.Korisnik = ctx.Korisniks.Where(x => x.Id == model.Id).FirstOrDefault();
                            adminDB.Korisnik.Email = model.Email;
                            adminDB.Korisnik.Ime = model.Ime;
                            adminDB.Korisnik.Prezime = model.Prezime;
                            adminDB.Korisnik.Username = model.Username;
                            adminDB.Korisnik.Password = model.Password;
                            ctx.SaveChanges();
                        }
                        else
                        {
                            var a = ctx.Administrators.Where(x => x.KorisnikId == model.Id).SingleOrDefault();
                            if (a != null)
                            {
                                ctx.Administrators.Remove(a);
                            }
                            korisnikDB = ctx.Korisniks.Find(model.Id);
                            korisnikDB.Email = model.Email;
                            korisnikDB.Ime = model.Ime;
                            korisnikDB.Prezime = model.Prezime;
                            korisnikDB.Username = model.Username;
                            korisnikDB.Password = model.Password;
                            ctx.SaveChanges();
                        }
                    }
            }
            catch
            {
                
            }

            return RedirectToAction("Dodaj");
        }

        public ActionResult Obrisi(int korisnikId)
        {
            if(IsCurrent(korisnikId)){
                var a = ctx.Administrators.Where(x => x.KorisnikId == korisnikId).SingleOrDefault();
                if (a != null)
                {
                    ctx.Administrators.Remove(a);
                }

                Korisnik k = ctx.Korisniks.Find(korisnikId);
                ctx.Korisniks.Remove(k);
                ctx.SaveChanges();
            }
            
            return RedirectToAction("Index");
        }

        public ActionResult Uredi(int korisnikId)
        {
            KorisniciEditVM model = ctx.Korisniks.Where(x=>x.Id == korisnikId).Select(x=>new KorisniciEditVM() {
                Email = x.Email,
                Id = x.Id,
                Ime = x.Ime,
                Password = x.Password,
                Prezime = x.Prezime,
                Username = x.Username
            }).Single();

            model.isAdministrator = ctx.Administrators.Where(x=>x.KorisnikId==korisnikId).SingleOrDefault() == null ? false : true;
            

            return View("Dodaj",model);
        } 
       
        public bool IsCurrentOrIsAdmin(int korisnikId)
        {
            if (Autentifikacija.GetLogiraniKorisnik(this.HttpContext).Id != korisnikId && Autentifikacija.GetLogiraniKorisnik(this.HttpContext).Id == Global.superuserId)//omogući za master account hard kodirano
                return false;

            if (korisnikId == Autentifikacija.GetLogiraniKorisnik(this.HttpContext).Id || ctx.Administrators.Where(x=>x.KorisnikId==korisnikId).SingleOrDefault() != null )
            {
                return true;
            }
            return false;
        }

        public bool IsCurrent(int korisnikId)
        {
            if (Autentifikacija.GetLogiraniKorisnik(this.HttpContext).Id == korisnikId)//
                return false;

            return true;
        }
    }
}