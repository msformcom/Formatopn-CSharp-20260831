using System.ComponentModel.DataAnnotations;
using AlloCineInterfaces;

namespace AlloCineWebApi.Models
{
    public class Cinema : ICinema
    {
        public string Code { get; set; }

        // Ces annotations pourront être utilisées par le Binder pour valider 
        // l'instance recue par le service 
        // Http => route => Methode de controller
        // => Binder => Créer l'objet Cinema => Validation avec attribut => erreur si non valide
        [Required(ErrorMessage ="Le {0} est requis")]
        // Message erreur si minLength non respecté : Le Nom doit avoir au moins 10 caractères
        [MinLength(10,ErrorMessage ="Le {0} doit avoir au moins {1} caractères")]
        public string Nom { get; set; }
        public string CodePostal { get; set; }
        public int NombreSalles { get; set; }
    }
}
