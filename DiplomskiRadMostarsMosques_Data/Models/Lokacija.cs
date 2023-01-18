using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_Data.Models
{
    public class Lokacija
    {
        public int Id { get; set; }
        public string naziv_lokacije { get; set; }
        public string adresa { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }

    }
}