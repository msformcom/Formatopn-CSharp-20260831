using System;
using System.Collections.Generic;
using System.Text;

namespace AlloCineInterfaces
{
    public interface ISeance
    {
        DateTime Horaire { get; set; }
        string Code { get; set; }
        ICinema Cinema { get; set; }
        IFilm Film { get; set; }
    }
}
