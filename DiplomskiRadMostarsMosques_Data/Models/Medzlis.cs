using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_Data.Models
{
    public class Medzlis
    {
        public int Id { get; set; }

        
        public int OpstinaId { get; set; }
        public Opstina Opstina { get; set; }

        public Medzlis()
        {
            MedzlisTranslations = new HashSet<MedzlisTranslation>();
        }
        public virtual ICollection<MedzlisTranslation> MedzlisTranslations { get; set; }
    }
}