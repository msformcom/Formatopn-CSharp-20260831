namespace CantineApiContracts
{
    // Enveloppe commune a toutes les reponses de l'API
    // Partagee entre l'API qui la produit et le client qui la consomme :
    // les deux cotes ne peuvent plus diverger sans erreur de compilation
    // La version non generique sert aux endpoints qui ne renvoient pas de donnees
    public class ResponseWrapper
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
    }

    public class ResponseWrapper<T> : ResponseWrapper
    {
        public T? Data { get; set; }
    }
}
