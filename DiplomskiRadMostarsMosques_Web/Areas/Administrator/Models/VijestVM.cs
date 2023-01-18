using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DiplomskiRadMostarsMosques_Web.Areas.Administrator.Models
{
    public class VijestVM
    {
        public class VijestInfo
        {
            public int Id { get; set; }
            public string Naslov { get; set; }
            public string Tekst { get; set; }
            public DateTime DatumPostavljanja { get; set; }
        }

        public IPagedList<VijestInfo> vijesti { get; set; }


        public int? JezikId { get; set; }
        public List<SelectListItem> JezikStavke { get; set; }
    }
}