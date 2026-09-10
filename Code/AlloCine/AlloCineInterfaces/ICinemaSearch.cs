using System;
using System.Collections.Generic;
using System.Text;

namespace AlloCineInterfaces
{
    public interface ICinemaSearch : IEntitySearch
    {
        public string? CodePostal { get; set; }

        public string? NomPart { get; set; }
    }
}
