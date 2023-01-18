using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiplomskiRadMostarsMosques_Web.Areas.Administrator.Models
{
    public class DogadjajEditVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Naziv je obavezno polje")]
        public string Naziv { get; set; }
        [Required(ErrorMessage = "Opis je obavezno polje")]
        [StringLength(200, MinimumLength = 5, ErrorMessage = "Unesite više od 5 a manje od 200 karaktera")]
        public string Tekst { get; set; }

        [Required(ErrorMessage = "Morate unijeti datum")]
        [Range(typeof(DateTime), "01/01/1900", "01/01/2179", ErrorMessage = "Datum nije ispravan")]
        [DataType(DataType.Date, ErrorMessage = "Unesite ispravan datum")]
        public string DatumOdrzavanja { get; set; }

        public DateTime DatumPostavljanja { get; set; }

        public IEnumerable<HttpPostedFileBase> input_slika { get; set; }

        public string NazivGalerije { get; set; }
        public int? GalerijaId { get; set; }

        public List<int> slikeIds { get; set; }


        public int? JezikId { get; set; }
        public List<SelectListItem> JezikStavke { get; set; }
    }
}