using DiplomskiRadMostarsMosques_Data.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiplomskiRadMostarsMosques_Web.Areas.Administrator.Models
{
    public class ObjekatEditVM
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Opis je obavezno polje")]
        public string Opis { get; set; }

        [Required(ErrorMessage = "Naziv je obavezno polje")]
        public string Naziv { get; set; }

        [Required(ErrorMessage = "Tip objekta je obavezno polje")]
        public int TipObjektaId { get; set; }
        public List<SelectListItem> TipoviObjekataStavke { get; set; }

        [Required(ErrorMessage = "Aktivnost je obavezno polje")]
        public int? AktivnostId { get; set; }
        public List<SelectListItem> aktivnostStavke { get; set; }

        [Required(ErrorMessage = "Dzemat je obavezno polje")]
        public int? DzematId { get; set; }
        public List<SelectListItem> DzematiStavke { get; set; }

        [Required(ErrorMessage = "Medžlis je obavezno polje")]
        public int? MedzlisId { get; set; }
        public List<SelectListItem> MedzlisStavke { get; set; }


        [Required(ErrorMessage = "Jezik je obavezno polje")]
        public int? JezikId { get; set; }
        public List<SelectListItem> JezikStavke { get; set; }

        [Required(ErrorMessage = "Godina izgradnje je obavezno polje")]
        public string GodinaIzgradnje { get; set; }

        public bool IsRusena { get; set; }
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Moguće koristiti samo brojeve i bez razmaka")]
        public int? GodinaRusenja { get; set; }
        [RegularExpression(@"^[0-9]+$", ErrorMessage = "Moguće koristiti samo brojeve i bez razmaka")]
        public int? GodinaObnove { get; set; }


        public bool IsUnesco { get; set; }

        public int LokacijaId { get; set; }

        [Required(ErrorMessage = "latitude je obavezno polje")]
        [RegularExpression(@"^\d*[\.]\d*$", ErrorMessage = "Koordinate moraju biti u formatu eg. 17.258671")]

        public string latitude { get; set; }
        [Required(ErrorMessage = "longitude je obavezno polje")]
        [RegularExpression(@"^\d*[\.\,]\d*$", ErrorMessage = "Koordinate moraju biti u formatu eg. 43.1890189")]
        public string longitude { get; set; }

        public List<int> slikeIds { get; set; }
        public string NazivGalerije { get; set; }
        public List<HttpPostedFileBase> input_slika { get; set; }

        public byte[] Zvuk { get; set; }

        public string UcitajSlikeError { get; set; }

        //public int? GalerijaId { get; set; }
        //public Galerija Galerija { get; set; }
    }
}