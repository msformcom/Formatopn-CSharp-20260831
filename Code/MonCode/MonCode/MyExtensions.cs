using System;
using System.Collections.Generic;
using System.Text;

namespace MonCode
{
    public static class MyExtensions
    {
        public static DateTime EndOfMonth(this DateTime d,int annees=0,int mois=0,int jours=0, int heures=0,int minutes=0,int secondes=0)
        {
            var j = d.AddYears(annees).AddMonths(mois).AddDays(jours)
                .AddHours(heures).AddMinutes(minutes).AddSeconds(secondes);
            return j.AddDays(DateTime.DaysInMonth(j.Year, j.Month) - j.Day);
        }



        public static IEnumerable<int> Next(this int start, int nbElements)
        {
            return Enumerable.Range(1, nbElements).Select(c => start + c);
            //for (var i = 1; i <= nbElements; i++)
            //{
            //    Cette boucle s'arrète si l'iterator cesse de demander les valeurs suivantes
            //   yield return start + i;
            //}
        }

        public static string Ellipsis(this string s, int maxLength)
        {
            if (s.Length <= maxLength) return s;
            return s.Substring(0, maxLength - 3) + "...";
        }
    }
}
