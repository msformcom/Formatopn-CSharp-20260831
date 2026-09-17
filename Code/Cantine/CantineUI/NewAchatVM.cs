using System;
using System.Collections.Generic;
using System.Text;
using CantineApi.Models;
using CantineInterfaces;
using CantineInterfaces.Tests;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace CantineUI
{
    public partial class NewAchatVM : ObservableObject
    {
        private readonly IServiceScope scope;
        public NewAchatVM()
        {
            this.scope = DI.Services.CreateScope();
            var cantineService = scope.ServiceProvider.GetRequiredService<ICantineService>();
            cantineService.ListeArticlesAsync(new ArticleSearchDTO() { SearchText="u"}).ContinueWith(t =>
            {
                this.ListeDesArticles = t.Result.ToList();
            });
        }

        
        [ObservableProperty]
        public NewAchatFormulaireVM formulaire=new NewAchatFormulaireVM();

        [RelayCommand]
        public async Task AjouterAchat()
        {
            try
            {
                // Même instance que dans le constructeur
            var service = scope.ServiceProvider.GetRequiredService<ICantineService>();
            await service.ConsommerArticleAsync(formulaire.Matricule,formulaire.ReferenceArticle,formulaire.Quantite);
                // Je change les éléments affichés dans l'interface
                this.MessageSuccess = "Bravo, bon appétit";
            }
            catch (Exception ex)
            {

                this.MessageFail = "Vous mangerez mieux ce soir et ça vous fera du bien";
            }

     
        }


        [ObservableProperty]
        public IEnumerable<IArticle> listeDesArticles;

        [ObservableProperty]
        public string messageFail;

        [ObservableProperty]
        public string messageSuccess;

    }
}
