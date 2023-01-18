using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_Data.Models
{
    public class Administrator
    {
        public int Id { get; set; }

        public int KorisnikId { get; set; }
        public Korisnik Korisnik { get; set; }
    }
}