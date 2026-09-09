using System;
using System.Collections.Generic;
using System.Text;
using AlloCineInterfaces;

namespace MesTests
{
    public class Film : IFilm
    {
        public string Code { get; set; }
		#region Propriété Titre

		private string _Titre;

		public string Titre
		{
			get { return _Titre; }
			set
			{
				// TODO : Ajoutez ici la logique de validation pour la propriété Titre
				// if(condition)
				// {	
				// 	throw new ArgumentException("Message d'erreur");
				// }
				if (string.IsNullOrWhiteSpace(value))
				{
					throw new ArgumentException();
				}
				_Titre = value;
			}
		}
		#endregion

		public int Duree { get; set; }
        public DateOnly DateSortie { get; set; }
    }
}
