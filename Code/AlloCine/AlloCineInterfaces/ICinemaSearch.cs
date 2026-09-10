using System;
using System.Collections.Generic;
using System.Text;

namespace AlloCineInterfaces
{
    public interface ICinemaSearch
    {
        public string? CodePostal { get; set; }

        public string? NomPart { get; set; }
    }
}
