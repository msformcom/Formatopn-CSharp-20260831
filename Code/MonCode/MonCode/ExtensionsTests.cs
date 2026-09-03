using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using System.Text;


namespace MonCode
{
    [TestClass]
    public class ExtensionsTests
    {
        [TestMethod]
        public void ShuffleTests()
        {
            var l = Enumerable.Range(1, 100);

            var r= l.MyShuffle().ToList();
        }

        [TestMethod]
        public void SampleTests()
        {
            var liste = Enumerable.Range(1, 1000);
            var sample=liste.Sample(4);
        }

        [TestMethod]
        public void EnfOfMonthTests()
        {
            var d = DateTime.Now;
            var e = d.EndOfMonth(0, 0, 45);
            e = d.EndOfMonth( 0,2,  secondes: 10, 
                                jours: 45);
        }

        [TestMethod]
        public void NextTests()
        {
            var a = 6;
            foreach(var i in a.Next(10).Take(2))
            {
              
            }
        }
        [TestMethod]
        public void EllipsisTests()
        {
            var s = "Cet autoradion est un super autoradionhkj KjhKJ kjh kshfkjsh dkfjskdjfh skjdfh ksjh dfkj hsdjkf ksj dfhk sdf ks";
            // Afficher s dans une interface
            // Cet autoradion est un super aut... (see more)
            // méthode qui prend une chaine 


            var e = MyExtensions.Ellipsis(s, 10); // => Cet aut...
            e = s.Ellipsis(10);

            // Les méthodes de Linq (Where, OrderBy,etc)
            // Sont des méthodes d'extension sur IEnumerable<T>
        }

       



    }


    // Methode d'extension
    // Methode static dans une class static
    // Premier parametre avec this
    // La méthode apparait sur le type du premier paramêtre
    // Syntaxi sugar 
    // var e = StringExtensions.Ellipsis(s, 10); // => Cet aut...
    // e = s.Ellipsis(10);


}
