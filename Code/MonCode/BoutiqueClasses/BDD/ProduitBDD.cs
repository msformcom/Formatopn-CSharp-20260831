using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Boutique.BDD
{
    public class ProduitBDD : IProduit
    {
        // Comme on doit connaitre l'id lors de la modif d'un produit
        // on ajoute l'id mais seulement en iternal
        // Cet attribut permet d'identifier quelle propriété / colonne est la clé primaire
        // Ceci est remplacé par fluentapi
        //[Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        // Id du catalogue
        internal Guid IdCatalogue { get; set; }

        public int NbStock { get; internal set; }
        // public int NbStock => throw new NotImplementedException(); // <=> public int nbStock{get}

        public Decimal Prix { get; internal set; }

        //[MaxLength(50)]
        public string Nom{get; internal set; }
            //{

                // Reflexion => Obtenir les métadata des classes/propriétés
                // J'obtients le type de Produit => avec toutes les infos sur le type Produit
                //var tProduit = this.GetType();
                //// Infos sur la propriété Nom
                //var ProprieteNomProduit = tProduit.GetProperty(nameof(Nom));
                //// Liste des attributs MaxLength appliqués au Nom
                //var attributsMaxLength = ProprieteNomProduit.GetCustomAttributes<MaxLengthAttribute>();
                //// Je prends le premier
                //var attributMaxLength = attributsMaxLength.FirstOrDefault();
                //if(attributMaxLength != null)
                //{
                //    // Voilà la longuer
                //    var length = attributMaxLength.Length;
                //    if (value.Length >= length)
                //    {
                //        throw new ArgumentException("La chaine est trop longue");
                //    }
                //}
            //}
        //}

        public DateTime LastModif { get; set; } = DateTime.Now;
    }
}
