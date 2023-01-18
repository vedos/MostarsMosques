using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_Data.Models
{
    public class VijestTranslation
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }

        public int VijestId { get; set; }
        public Vijest Vijest { get; set; }

        public int JezikId { get; set; }
        public Jezik Jezik { get; set; }
    }
}