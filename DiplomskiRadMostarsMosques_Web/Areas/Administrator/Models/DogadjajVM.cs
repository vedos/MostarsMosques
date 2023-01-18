using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;

namespace DiplomskiRadMostarsMosques_Web.Areas.Administrator.Models
{
    public class DogadjajVM
    {
        public class DogadjajInfo
        {
            public int Id { get; set; }
            public string Naslov { get; set; }
            public string Opis { get; set; }
            public DateTime DatumPostavljanja { get; set; }
            public DateTime DatumOdrzavanja { get; set; }
        }

        public IPagedList<DogadjajInfo> dogadjaji { get; set; }

        public int? JezikId { get; set; }
        public List<SelectListItem> JezikStavke { get; set; }
    }
}