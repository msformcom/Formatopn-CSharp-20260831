using System;
using System.Collections.Generic;
using System.Text;
using CantineInterfaces;

namespace CantineServiceFromAPI.Models
{
    // Cette classe me permet d'assurer la communication avec la couche appelante
    // Via l'interface IArticle
    // Les setters sont publics car la desserialisation JSON les alimente
    public class Article : IArticle
    {
        public string Reference { get; set; }
        public string Libelle { get; set; }
        public decimal Prix { get; set; }
        public byte[] Photo { get; set; }
        public ICollection<string> Allergenes { get; set; } = new List<string>();
    }
}
