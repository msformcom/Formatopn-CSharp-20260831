using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
using System.Text;

namespace CodeTests
{
    [TestClass]
    public class StreamTests
    {
        [TestMethod]
        public void IDisposableTests()
        {
            // Flux ouvert vers un fichier
            using (var fs = File.OpenRead(@"c:\data\toto.txt"))
            {

                // Je suis dans un bloc using (déclaration de fs)
                // Quelle que soit la manière dont je sors de ce bloc de code
                // La méthode dispose de fs sera appelée
                // Dispose => Libérer immediatement les resources du système ouverte 
                var sr = new StreamReader(fs);

                var ligne1 = sr.ReadLine();

            }
            

     
   
          

        }
    }
}
