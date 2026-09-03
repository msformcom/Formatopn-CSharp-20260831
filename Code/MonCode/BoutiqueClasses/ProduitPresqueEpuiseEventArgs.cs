namespace Boutique
{
    public class ProduitPresqueEpuiseEventArgs : EventArgs
    {
        public IProduit Produit { get; set; }
        public int Limite { get; set; }

        public int StockActualise { get; set; }

        public DateTime Date { get; set; }
    }
}