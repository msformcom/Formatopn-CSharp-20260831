using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Extensions.DependencyInjection;

namespace Boutique
{
    /// <summary>
    /// Décrit un Produit dans l'entreprise
    /// </summary>
    public partial class Produit : IProduit
    {
        #region Constructeurs
        public Produit(string nom, IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
            this.Nom = nom;

        }
        #endregion



        // Propriété auto-implémentée => pas de champs car les outils entity ou forms utilisent les propriétés pour générer automatiquement du code

        //public string Nom { get; set; }

        #region Propriété Nom

        private string _Nom;
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// Le nom du produit affiché dans les UI
        /// </summary>
        public string Nom
        {

            get { return _Nom; }
            internal set
            {
                // Demande du validateur de chaine associé au Nom
                // Si non configuré dans DI => null
                var validator = serviceProvider.GetKeyedService<StringValidator>("NomValidator");
                // si DI fournit la fonction de validation
                if (validator != null)
                {




                    var resultatValidation = validator(value);
                    if (!resultatValidation.valid)
                    {
                        throw new ArgumentException(resultatValidation.message);
                    }
                }
                _Nom = value;
            }
        }
        #endregion






    }
}
