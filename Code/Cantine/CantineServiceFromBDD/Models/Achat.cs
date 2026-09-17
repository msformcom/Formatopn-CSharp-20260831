using CantineInterfaces;

namespace CantineServiceFromBDD.Models
{
    // Cette classe me permet d'assurer la communication avec la couche appelante
    // Via l'interface IAchat
    public class Achat : IAchat
    {
        public string MatriculeEmploye { get; set; }
        public string ReferenceArticle { get; set; }
        public decimal Prix { get; set; }
        public int Quantite { get; set; }
        public DateTime DateAchat { get; set; }
    }
}
