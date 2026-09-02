using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;

namespace MonCode
{
    public class ExtensionsTests
    {
        [TestMethod]
        public void StringExtensionsTests()
        {
            var s = "Cet autoradion est un super autoradionhkj KjhKJ kjh kshfkjsh dkfjskdjfh skjdfh ksjh dfkj hsdjkf ksj dfhk sdf ks";
            // Afficher s dans une interface
            // Cet autoradion est un super aut... (see more)
            // méthode qui prend une chaine 


            var e = StringExtensions.Ellipsis(s, 10); // => Cet aut...
            e = s.Ellipsis(10);

            new List<int>().Where(c => c < 10);

        }

       



    }


    // Methode d'extension
    // Methode static dans une class static
    // Premier parametre avec this
    // La méthode apparait sur le type du premier paramêtre
    // Syntaxi sugar 
    // var e = StringExtensions.Ellipsis(s, 10); // => Cet aut...
    // e = s.Ellipsis(10);

    public static class StringExtensions
    {
        public static string Ellipsis(this string s, int maxLength)
        {
            if (s.Length <= maxLength) return s;
            return s.Substring(0, maxLength - 3) + "...";
        }
    }
}
