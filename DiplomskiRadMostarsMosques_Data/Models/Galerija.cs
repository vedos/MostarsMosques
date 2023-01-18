using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_Data.Models
{
    public class Galerija
    {
        public int Id { get; set; }
        public string Naziv { get; set; }

        public Galerija()
        {
            SlikaInfos = new HashSet<SlikaInfo>();
        }
        public virtual ICollection<SlikaInfo> SlikaInfos { get; set; }

    }
}