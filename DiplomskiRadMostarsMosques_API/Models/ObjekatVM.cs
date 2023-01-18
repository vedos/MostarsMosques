using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_API.Models
{
    public class ObjekatVM
    {
        public class ObjekatInfo
        {
            public int Id { get; set; }
            public int lokacijaId { get; set; }

            public string naziv { get; set; }
            public string adresa { get; set; }
            public string opis { get; set; }

            public string longitude { get; set; }
            public string latitude { get; set; }

            //
                
            public string TipObjekta { get; set; }
            public int TipObjektaId { get; set; }
            public string Dzemat { get; set; }
            public string Medzlis { get; set; }
            public string GodinaIzgradnje { get; set; }
            public bool IsRusena { get; set; }
            public string GodinaRusenja { get; set; }
            public string GodinaObnove { get; set; }
            public bool IsUnesco { get; set; }
            public string Aktivna { get; set; }
            public int? AktivnaId { get; set; }

        }
        public List<ObjekatInfo> listObjekti { get; set; }
    }
}