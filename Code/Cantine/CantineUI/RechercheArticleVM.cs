using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using CantineApi.Models;
using CantineInterfaces;
using CantineInterfaces.Tests;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace CantineUI
{

    // Cette classe représente en mémoire les informations
    // Visibles ou accessibles (get, set)
    // dans la vue (morceau de l'interface)
    // Ici : Logique de l'interface 


    
    // Cette héritage permet de créer un VM qui est capable d'automatiser le
    // Déclenchement de l'évènement PropertyChanged
    // Afin d'avertir l'UI qu'une propriété a changé => MAJ de l'UI
    public partial class RechercheArticleVM : ObservableObject 
    {
        public RechercheArticleVM()
        {
            //Task.Delay(1000).ContinueWith(c =>
            //{
            //    this.Search.SearchText += "*";
            //});
        }
        // Dans le controle utilisateur de recherche d'article
        // L,'objectif de l'UI va être de fournir 
        // les informations pour le ArticleSearchDTO
        // A envoyer au service

        // Addon du compilateur qui génère automatiquement une propriété
        // Qui implemente PropertyChanged
        // à partir d'un champs
        [ObservableProperty]
        public ArticleSearchDTO search = new ArticleSearchDTO();

        // Articles à afficher dans le ListBox
        [ObservableProperty]
        public IEnumerable<IArticle> resultats;

        // Cette méthode doit être Bindée au bouton rechercher
        // La méthode est utilisée pour générer une commande
        // FetchArticlesCommand
        [RelayCommand]
        public async Task FetchArticles()
        {
        
            // Utiliser le service ICantineService
            // => CantineServiceBDD ou CantineServiceAPI suivant la config
            var service=DI.Services.GetRequiredService<ICantineService>();
            var resultat=await service.ListeArticlesAsync(this.Search);
            // Je change les éléments affichés dans l'interface
            Resultats = resultat.ToList();

        }



    }
}
