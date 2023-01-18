using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_API.Models
{
    public class MedzlisVM
    {
        public class MedzlisInfo
        {
            public int Id { get; set; }
            public string Naziv { get; set; }
            public string Opis { get; set; }
        }
        public List<MedzlisInfo> medzlisi { get; set; }
    }
}