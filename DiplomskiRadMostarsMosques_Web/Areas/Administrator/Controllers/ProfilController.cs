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
    public class ProfilController : Controller
    {
        MMContext ctx = new MMContext();
        // GET: Administrator/Profil
        public ActionResult Index()
        {
            ProfilEditVM model = new ProfilEditVM();
            int logiraniKorisnikId = Autentifikacija.GetLogiraniKorisnik(this.HttpContext).Id;

            model = ctx.Korisniks.Where(x => x.Id == logiraniKorisnikId).Select(x => new ProfilEditVM
            {
                Id = x.Id,
                Email = x.Email,
                Ime = x.Ime,
                Prezime = x.Prezime,
                Username = x.Username
            }).FirstOrDefault();
            return View(model);
        }

        public ActionResult Snimi(ProfilEditVM model)
        {

            Korisnik korisnikDB = ctx.Korisniks.Find(model.Id);
            if (korisnikDB.Password == model.Password)
            {
                if (model.PasswordNovi != null)
                {

                    korisnikDB.Password = model.PasswordNovi;
                    ctx.SaveChanges();
                }
                else {
                    
                    korisnikDB.Ime = model.Ime;
                    korisnikDB.Prezime = model.Prezime;
                    korisnikDB.Email = model.Email;
                    korisnikDB.Username = model.Username;
                    ctx.SaveChanges();
                }
            }



            return RedirectToAction("Index");
        }

        public bool ProvjeriPassword(ProfilEditVM model)
        {
            
            return model.PasswordNovi == model.PasswordNovi2;
        }

        public ActionResult PotvrdaUnosa(ProfilEditVM model)
        {
            if (model.PasswordNovi!=null || model.PasswordNovi2!=null)
            {
                if (!ModelState.IsValidField("PasswordNovi") ||
                    !ModelState.IsValidField("PasswordNovi2"))
                {
                    return View("PromijenaLozinke", model);
                }
                else {
                    if (ProvjeriPassword(model))
                    {
                        return View(model);
                    }
                    else
                    {
                        return View("PromijenaLozinke", model);
                    }
                }
            }

            if (!ModelState.IsValidField("Ime") ||
           !ModelState.IsValidField("Prezime") ||
            !ModelState.IsValidField("Username") ||
            !ModelState.IsValidField("Email"))
            {
                return View("Index", model);
            }
            else
            {
                return View(model);
            }
            
        }

        public ActionResult PromijenaLozinke(int id)
        {
            ProfilEditVM model = ctx.Korisniks.Where(x => x.Id == id).Select(x => new ProfilEditVM
            {
                Id = x.Id,
                Email = x.Email,
                Ime = x.Ime,
                Prezime = x.Prezime,
                Username = x.Username
            }).FirstOrDefault();


            return View(model);
        }
    }
}