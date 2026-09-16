using System;
using System.Collections.Generic;
using System.Text;

namespace HRDAL
{
    internal interface IGestionData
    {
        public DateTime DateCreation { get; set; }
        public DateTime DateModification { get; set; }
    }
}
