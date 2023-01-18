using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_Data.Models
{
    public class Objekat
    {
        public int Id { get; set; }
        
        public int TipObjektaId { get; set; }
        public TipObjekta TipObjekta { get; set; }

        public int? DzematId { get; set; }
        public Dzemat Dzemat { get; set; }

        public int? MedzlisId { get; set; }
        public Medzlis Medzlis { get; set; }

        public string GodinaIzgradnje { get; set; }
        public bool IsRusena { get; set; }
        public int? GodinaRusenja { get; set; }
        public int? GodinaObnove { get; set; }
        public bool IsUnesco { get; set; }

        public int? AktivnostId { get; set; }
        public Aktivnost Aktivnost { get; set; }

        public int LokacijaId { get; set; }
        public Lokacija Lokacija { get; set; }

        public byte[] Zvuk { get; set; }

        public int? GalerijaId { get; set; }
        public Galerija Galerija { get; set; }

        public Objekat()
        {
            ObjekatTranslations = new HashSet<ObjekatTranslation>();
        }
        public virtual ICollection<ObjekatTranslation> ObjekatTranslations { get; set; }


    }
}