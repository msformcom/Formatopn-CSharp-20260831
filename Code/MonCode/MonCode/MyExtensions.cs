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

        // Fonction qui s'applique à tout IEnumerable quel que soit le type
        public static IEnumerable<TSource> Sample<TSource>(this IEnumerable<TSource> source, int pas)
        {
            if (source == null)
            {
                throw new ArgumentNullException("La source est nulle");
            }
            int i = 0;
            foreach(var e in source)
            {
                if(i % pas == 0)
                {
                    yield return e;
                }

                i++;
            }
        }

        public static IEnumerable<TTarget> Cast<TSource,TTarget>(this IEnumerable<TSource> source, int n) 
            where TTarget : TSource
        {
            foreach(var e in source)
            {
                yield return (TTarget)e;
            }
        }

        public static IEnumerable<TSource> MyShuffle<TSource>(this IEnumerable<TSource> source){
            var r = new Random();
            return source.OrderBy(c => r.Next());
        }

        public static IEnumerable<TSource> MySkip<TSource>(this IEnumerable<TSource> source, int n)
        {
            var iterator = source.GetEnumerator();

            for (var i = 0; i < n; i++)
            {
                if (!iterator.MoveNext())
                {
                    break;
             
                }
            }
            while (true)
            {
                iterator.MoveNext();
                yield return iterator.Current;
            }

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
