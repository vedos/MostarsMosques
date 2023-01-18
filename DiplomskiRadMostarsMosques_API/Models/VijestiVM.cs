using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace DiplomskiRadMostarsMosques_API.Models
{
    public class VijestiVM
    {
        public class VijestInfo
        {
            public int Id { get; set; }
            public string naslov { get; set; }
            public string opis { get; set; }
            public byte[] SlikaThumbnail { get; set; }
            public byte[] Slika { get; set; }
            public DateTime DatumObjave { get; set; }
        }
        public int totalItemCount { get; set; }
        public IPagedList<VijestInfo> listVijesti { get; set; }
    }
}