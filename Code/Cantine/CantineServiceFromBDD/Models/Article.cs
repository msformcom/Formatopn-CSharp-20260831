using System;
using System.Collections.Generic;
using System.Text;
using CantineInterfaces;

namespace CantineServiceFromBDD.Models
{
    // Cette classe me permet d'assure la communication avec la couche appelante
    // Via l'interface IArticle
    public class Article : IArticle
    {
        public string Reference { get ; set ; }
        public string Libelle { get ; set ; }
        public decimal Prix { get ; set ; }
        public byte[] Photo { get ; set ; }

        public ICollection<string> Allergenes { get; internal set; }
    }
}
