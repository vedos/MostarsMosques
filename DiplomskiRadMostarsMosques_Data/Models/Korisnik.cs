using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_Data.Models
{
    public class Korisnik
    {
        public int Id { get; set; }

        [Index("UserNameIndex", IsUnique = true)]
        [MaxLength(100)]
        public string Username { get; set; }

        public string Password { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }

        public DateTime? zadnjaPrijava { get; set; }
        
    }
}