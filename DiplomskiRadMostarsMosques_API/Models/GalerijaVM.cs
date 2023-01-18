using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_API.Models
{
    public class GalerijaVM
    {
        public class SlikaInfo
        {
            public int id { get; set; }
            public byte[] SlikaThumbnail;
            public string naziv { get; set; }
            public byte[] Slika;
        }
        public int totalItemsCount { get; set; }
        public IPagedList<SlikaInfo> listSlika { get; set; }
    }
}