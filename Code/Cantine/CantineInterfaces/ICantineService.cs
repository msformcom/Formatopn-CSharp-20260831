namespace CantineInterfaces
{
    public interface ICantineService
    {
        // Visualiser les articles disponibles
        // Les paramètres de la recheche sont eux-même une interface => changements dans les paramètres
        // ne changent pas l'interface ICantineService
        // Cette méthode peut avoir un temps d'exécution qui se comptabilise en ms
        // => Asynchronisme = lace la méthode => le résultat arrive plus tard
        // Task = Objet qui représente une opération en couurs d'eéxution
        Task<IEnumerable<IArticle>> ListeArticlesAsync(IArticleSearch search); // SEARCH

        Task<IEmploye> LireEmployeInfos(string matricule);

        Task SupprimerArticle(string referenceArticle); // DELETE

        Task ConsommerArticle(string matricule, string referenceArticle); // CREATE

        Task IncrementerCreditEmploye(string matricule, decimal montant); // UPDATE
    }
}
