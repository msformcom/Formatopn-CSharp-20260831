using CantineInterfaces;

namespace CantineServiceFromAPI.Models
{
    // Cette classe me permet d'assurer la communication avec la couche appelante
    // Via l'interface IArticle
    // Les setters sont publics car la desserialisation JSON les alimente
    public class Article : IArticle
    {
        public string Reference { get; set; } = string.Empty;
        public string Libelle { get; set; } = string.Empty;
        public decimal Prix { get; set; }
        public byte[] Photo { get; set; } = Array.Empty<byte>();
        public ICollection<string> Allergenes { get; set; } = new List<string>();
    }
}
