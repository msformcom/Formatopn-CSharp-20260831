namespace Boutique
{
    public interface IProduit
    {
        int NbStock { get; }
        string Nom { get; }
        decimal Prix { get; }
    }
}