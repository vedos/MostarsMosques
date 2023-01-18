using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_Data.Models
{
    public class Dogadjaj
    {
        public int Id { get; set; }
        public DateTime DatumPostavljanja { get; set; }
        public DateTime DatumOdrzavanja { get; set; }

        public int? GalerijaId { get; set; }
        public Galerija Galerija { get; set; }

        public Dogadjaj()
        {
            DogadjajTranslations = new HashSet<DogadjajiTranslation>();
        }
        public virtual ICollection<DogadjajiTranslation> DogadjajTranslations { get; set; }
    }
}