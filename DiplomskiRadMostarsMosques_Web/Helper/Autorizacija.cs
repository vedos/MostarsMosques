using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DiplomskiRadMostarsMosques_Data.Models;
using DiplomskiRadMostarsMosques_Web.Helper;
using DiplomskiRadMostarsMosques_Data.DAL;

namespace DiplomskiRadMostarsMosques_Web.Helper
{
    public class Autorizacija : FilterAttribute, IAuthorizationFilter
    {
        private readonly KorisnickeUloge[] _dozvoljeneUloge;

        MMContext ctx = new MMContext();
        public Autorizacija(params KorisnickeUloge[] dozvoljeneUloge)
        {
            _dozvoljeneUloge = dozvoljeneUloge;
        }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            Korisnik k = Autentifikacija.GetLogiraniKorisnik(filterContext.HttpContext);
            bool isAdmin = false;
            if (k == null)
            {
                filterContext.HttpContext.Response.Redirect("/Login");
                return;
            }
            //provjera

            if (ctx.Administrators.Where(x => x.KorisnikId == k.Id).SingleOrDefault() != null)
            {
                isAdmin = true;
            }

            foreach (KorisnickeUloge x in _dozvoljeneUloge)
            {
                if (x == KorisnickeUloge.Administrator && isAdmin)
                    return;
                if (x == KorisnickeUloge.Korisnik && k != null)
                    return;
            }
            //ako funkcija nije prekinuta pomoću "return", onda korisnik nema pravo pistupa pa će se vršiti redirekcija na "/Home/Index"

            if (k != null && isAdmin == false)
            {
                filterContext.HttpContext.Response.Redirect("/Administrator/Home");
            }
            filterContext.HttpContext.Response.Redirect("/Login");
        }

    }
        public enum KorisnickeUloge
        {
            Korisnik,
            Administrator
        }
        
    
}