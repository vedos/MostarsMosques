using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_Data.Models
{
    public class Dzemat
    {
        public int Id { get; set; }
        public string Naziv { get; set; }

        public int MedzlisId { get; set; }
        public Medzlis Medzlis { get; set; }
    }
}