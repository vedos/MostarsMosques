using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace DiplomskiRadMostarsMosques_API.Models
{
    public class DzamijeVM
    {
        public class DzamijaInfo
        {
            public int Id { get; set; }
            public int lokacijaId { get; set; }

            public string naziv { get; set; }
            public byte[] SlikaThumbnail { get; set; }

            public string opis { get; set; }



        }
        public int totalItemsCount { get; set; }
        public IPagedList<DzamijaInfo> listDzamije { get; set; }
    }
}