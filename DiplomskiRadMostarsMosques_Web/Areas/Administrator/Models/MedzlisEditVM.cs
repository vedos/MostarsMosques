using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiplomskiRadMostarsMosques_Web.Areas.Administrator.Models
{
    public class MedzlisEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naziv je obavezno polje")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Opis je obavezno polje")]
        public string Opis { get; set; }

        [Required(ErrorMessage = "Opština je obavezno polje")]
        public int opstinaId { get; set; }
        public List<SelectListItem> opstinaStavke { get; set; }
    }
}