using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using AlloCineInterfaces;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace AlloCineUI
{
    // Cette classe permet de modéliser en code notre UI
    internal partial class ListeFilmsVM : ObservableObject
    {
        // Dans l'UI, cette propriété va être liée au TextBox de la recherche
        // Lié correctement par un evenement OnPropertyChanged
        [ObservableProperty]
        private string titrePart  = "Toto";

        // Va être lié à la liste des ficles affiches
        [ObservableProperty]
        private ObservableCollection<IFilm> listeDesFilms=new ();

        // Va être lié au bouton "Recherche"
        // Une condition : ICommand
        [RelayCommand]
        public async Task RechercheFilm()
        {
            // Obtention du service par Injection de dépendance
            var service=DI.GetInjector().GetRequiredService<IAlloCineService>();
            // Faire appel au modele pour obtenir les films en fonction de la valeur de TitrePart
            var films =await service.SearchFilmsAsync(new FilmSearch() { TitrePart = this.TitrePart });

            // Mise à jour des films Affiches
            ListeDesFilms.Clear();
            foreach (IFilm film in films)
            {
                ListeDesFilms.Add(film);
            }
            
        }
    }
}
