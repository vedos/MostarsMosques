using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_Web.Areas.Administrator.Models
{
    public class MedzlisVM
    {
        public class MedzlisInfo
        {
            public int Id { get; set; }
            public string Naziv { get; set; }
            public string Opis { get; set; }
            public List<string> Jezici { get; set; }
            public string Opcina { get; set; }
            public string Kanton { get; set; }
        }
        public IPagedList<MedzlisInfo> listMedzlis { get; set; }
    }
}