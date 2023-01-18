using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiplomskiRadMostarsMosques_Web.Areas.Administrator.Models
{
    public class JeziciVM
    {
        [Required(ErrorMessage = "Jezik je obavezno polje")]
        public int? JezikId { get; set; }
        public List<SelectListItem> JezikStavke { get; set; }

        public int objekatId { get; set; }
    }
}