using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_API.Models
{
    public class DogadjajiVM
    {
        public class DogadjajInfo {
            public int Id { get; set; }
            public string Naziv { get; set; }
            public string Opis { get; set; }
            public DateTime datum { get; set; }
            public byte[] SlikaThumbnail;

        }
        public List<DogadjajInfo> listaDogadjaja { get; set; }
    }
}