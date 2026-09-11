using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using AlloCineInterfaces;

namespace AlloCineUI
{
    internal class FilmSearch : IFilmSearch, IValidatableObject
    {
        [Range(1D,1000D,ErrorMessage ="Le minimum de durée est de 1")]
        public int? DureeMin { get ; set ; }
        public int? DureeMax { get ; set ; }
        public string? TitrePart { get ; set ; }
        public int Page { get; set; } = 1;
        public int NbItemsPerPage { get; set; } = 10;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (DureeMax < DureeMin)
            {
                yield return new ValidationResult("Le min ne peut être superieur au max",
                        new List<string>() { nameof(DureeMin), nameof(DureeMax) });
            }
        }
    }
}
