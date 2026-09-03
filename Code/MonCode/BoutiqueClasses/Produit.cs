using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Boutique
{
    /// <summary>
    /// Décrit un Produit dans l'entreprise
    /// </summary>
    public partial class Produit : IProduit
    {
        #region Constructeurs
        public Produit(string nom)
        {
            this.Nom = nom;
        }
        #endregion



        // Propriété auto-implémentée => pas de champs car les outils entity ou forms utilisent les propriétés pour générer automatiquement du code

        //public string Nom { get; set; }

        #region Propriété Nom

        private string _Nom;

        /// <summary>
        /// Le nom du produit affiché dans les UI
        /// </summary>
        public string Nom
        {

            get { return _Nom; }
            internal set
            {



                // TODO : Ajoutez ici la logique de validation pour la propriété Nom



                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Le nom ne doit pas être vide");
                }
                // var path = @"c:\temp"; @ devant une chaine => permet d'échapper les caractères spéciaux
                var reg = new Regex(@"^[A-Z \-0-9]{1,49}$");
                if (!reg.IsMatch(value))
                {
                    throw new ArgumentException("Le nom n'est pas correct");
                }
                _Nom = value;
            }
        }
        #endregion






    }
}
