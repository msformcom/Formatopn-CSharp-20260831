namespace CantineInterfaces
{
    public interface IEmploye
    {
        string Matricule { get; set; }

        DateOnly DateNaissance { get; set; }

        string Nom { get; set; }

        string Prenom { get; set; }


    }
}