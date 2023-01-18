using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_Data.Models
{
    public class Opstina
    {
        public int Id { get; set; }
        public string Naziv { get; set; }

        public int KantonId { get; set; }
        public virtual Kanton Kanton { get; set; }
    }
}