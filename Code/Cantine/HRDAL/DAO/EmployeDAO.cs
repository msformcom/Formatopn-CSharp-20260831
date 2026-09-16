using System;
using System.Collections.Generic;
using System.Text;

namespace HRDAL.DAO
{
    public class EmployeDAO : IGestionData
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public Decimal CreditRepas { get; set; } = 100;

        public string PublicId { get; set; }

        public DateOnly BirthDate { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime DateModification { get; set; }

        // Propriété de navigation => permet en code d'accéder aux enregistrements associés dans la tables des achats
        // Indique également la cardinalité de la relation 1 Employe => 0-n achats
        public ICollection<AchatDAO> Achats { get; set; } = new HashSet<AchatDAO>();
    }
}
