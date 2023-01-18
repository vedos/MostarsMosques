using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiplomskiRadMostarsMosques_Web.Areas.Administrator.Models
{
    public class DzematEditVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Naziv je obavezno polje")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Medžlis je obavezno polje")]
        public int medzlisId { get; set; }
        public List<SelectListItem> medzlisiStavke { get; set; }
    }
}