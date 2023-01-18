using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_Web.Areas.Administrator.Models
{
    public class KorisniciEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Korisničko ime je obavezno polje")]
        [RegularExpression(@"^[A-Za-z\d_-]+$", ErrorMessage = "Koristite samo slova i brojeve")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Lozinka je obavezno polje")]
        [StringLength(50, MinimumLength = 5, ErrorMessage = "Unesite više od pet(5) karaktera")]
        [RegularExpression(@"^[A-Za-z\d_-]+$", ErrorMessage = "Koristite samo slova i brojeve")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Ime je obavezno polje")]
        public string Ime { get; set; }
        [Required(ErrorMessage = "Prezime je obavezno polje")]
        public string Prezime { get; set; }

        [EmailAddress(ErrorMessage = "e-mail mora biti u obliku: example@live.com")]
        public string Email { get; set; }

        public bool isAdministrator { get; set; }
    }
}