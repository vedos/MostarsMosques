using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiplomskiRadMostarsMosques_Web.Areas.Administrator.Models
{
    public class VijestEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Naslov je obavezno polje")]
        public string Naslov { get; set; }

        [Required(ErrorMessage = "Tekst je obavezno polje")]
        public string Tekst { get; set; }
        public DateTime DatumPostavljanja { get; set; }


        public string NazivGalerije { get; set; }
        public int? GalerijaId { get; set; }
        public IEnumerable<HttpPostedFileBase> input_slika { get; set; }
        public List<int> slikeIds { get; set; }

        public List<SelectListItem> kategorijaStavke { get; set; }


        public int? JezikId { get; set; }
        public List<SelectListItem> JezikStavke { get; set; }
    }
}