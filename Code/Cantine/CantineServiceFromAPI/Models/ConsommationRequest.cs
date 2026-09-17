using System;
using System.Collections.Generic;
using System.Text;

namespace CantineServiceFromAPI.Models
{
    // Corps de la requete POST api/achats
    // Cette classe n'existe que pour la serialisation JSON vers l'API
    public class ConsommationRequest
    {
        public string MatriculeEmploye { get; set; }
        public string ReferenceArticle { get; set; }
        public int Quantite { get; set; }
    }
}
