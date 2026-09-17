namespace HRDAL.DAO
{
    public class AchatDAO : IGestionData
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid IdEmploye { get; set; }

        // Propriété de navigation => permet en code d'accéder à l'enregistrement associé dans la tables des employés
        // Indique également la cardinalité de la relation 1 Achat => 1 Employé
        public virtual EmployeDAO Employe { get; set; }

        public Guid IdArticle { get; set; }
        public virtual ArticleDAO Article { get; set; }

        public int Quantite { get; set; }

        public DateTime DateAchat { get; set; } = DateTime.Now;

        public DateTime DateCreation { get; set; }
        public DateTime DateModification { get; set; }


    }
}
