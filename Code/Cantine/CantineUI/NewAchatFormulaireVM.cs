using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using CommunityToolkit.Mvvm.ComponentModel;

namespace CantineUI
{
    public partial class NewAchatFormulaireVM : ObservableObject
    {
        //[Range(1, 10000, ErrorMessage = "{0} doit être supérieur ) {1}")]
        [ObservableProperty]
        private int quantite;

        [ObservableProperty]
        private string matricule;
        [ObservableProperty]
        private string referenceArticle;

    }
}
