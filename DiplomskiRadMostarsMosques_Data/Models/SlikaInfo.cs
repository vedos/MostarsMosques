using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_Data.Models
{
    public class SlikaInfo
    {
        public int Id { get; set; }

        public byte[] Slika { get; set; }
        public byte[] SlikaThumbnail { get; set; }

        public string width { get; set; }
        public string height { get; set; }
        public string ratio { get; set; }


        public string url { get; set; }
        public string format { get; set; }
        public string naziv { get; set; }

        public int GalerijaId { get; set; }
        public Galerija Galerija { get; set; }
    }
}