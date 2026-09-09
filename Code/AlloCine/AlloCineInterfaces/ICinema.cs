namespace AlloCineInterfaces
{
    public interface ICinema
    {
        string Code { get; set; }
        string Nom { get; set; }

        string CodePostal { get; set; }

        int NombreSalles { get; set; }
    }
}