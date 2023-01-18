﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DiplomskiRadMostarsMosques_Data.Models
{
    public class DogadjajiTranslation
    {
        public int Id { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }

        public int DogadjajId { get; set; }
        public Dogadjaj Dogadjaj { get; set; }

        public int JezikId { get; set; }
        public Jezik Jezik { get; set; }

    }
}