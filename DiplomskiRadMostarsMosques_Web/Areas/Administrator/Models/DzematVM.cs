using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_Web.Areas.Administrator.Models
{
    public class DzematVM
    {
        public class DzematInfo
        {
            public int Id { get; set; }
            public string Naziv { get; set; }
            public string Medzlis { get; set; }
            public string Opstina { get; set; }
            public string Kanton { get; set; }
        }
        public IPagedList<DzematInfo> listDzemati { get; set; }
    }
}