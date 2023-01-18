using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiplomskiRadMostarsMosques_Web.Areas.Administrator.Models
{
    public class OpstinaEditVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Naziv je obavezno polje")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Kanton je obavezno polje")]
        public int KantonId { get; set; }
        public List<SelectListItem> kantonStavke { get; set; }

    }
}