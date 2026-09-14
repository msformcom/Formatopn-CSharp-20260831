namespace CantineInterfaces
{
    public interface IArticle
    {
        string Reference { get; set; }
        string Libelle { get; set; }

        decimal Prix { get; set; }

        byte[] Photo { get; set; }  

        ICollection<string> Allergenes { get;} // List //ObservableCollection => CollectionChanged
    }
}