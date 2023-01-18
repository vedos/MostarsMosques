using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_Data.Models
{
    public class Vijest
    {
        public int Id { get; set; }
        public DateTime DatumPostavljanja { get; set; }
        
        public int? KategorijaVijestId { get; set; } 
        public KatgorijaVijest KategorijaVijest { get; set; }

        public int? GalerijaId { get; set; }
        public Galerija Galerija { get; set; }


        public Vijest()
        {
            VijestTranslations = new HashSet<VijestTranslation>();
        }
        public virtual ICollection<VijestTranslation> VijestTranslations { get; set; }
    }
}