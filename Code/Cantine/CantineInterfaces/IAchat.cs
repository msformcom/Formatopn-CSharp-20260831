namespace CantineInterfaces
{
    public interface IAchat
    {

        string MatriculeEmploye { get; }
        string ReferenceArticle { get; }
        decimal Prix { get; }
        int Quantite { get; }   
        DateTime DateAchat { get; }

    }
}
