using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_Web.Areas.Administrator.Models
{
    public class KantonEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage ="Naziv je obavezno polje")]
        public string Naziv { get; set; }

        public string Skracenica { get; set; }
    }
}