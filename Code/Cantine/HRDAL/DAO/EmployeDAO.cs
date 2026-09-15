using System;
using System.Collections.Generic;
using System.Text;

namespace HRDAL.DAO
{
    public class EmployeDAO 
    {
        public Guid Id { get; set; } = Guid.NewGuid();



        public string PublicId { get; set; }

        public DateOnly BirthDate { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public DateTime DateCreation { get; set; }

        public DateTime DateModification { get; set; }
    }
}
