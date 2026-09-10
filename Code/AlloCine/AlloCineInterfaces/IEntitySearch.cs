using System;
using System.Collections.Generic;
using System.Text;

namespace AlloCineInterfaces
{
    public interface IEntitySearch
    {
        public int Page { get; set; }
        public int NbItemsPerPage { get; set; }
    }
}
