
using System.Text;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MonCode
{
    [TestClass]  // metadata => Attribut TestClass => indique que cette classe contient des méthodes de test
    public sealed class Types
    {
        [TestMethod]
        public void DeclarationTypesNumerique()
        {
            // Choisir convention int (mot clé) ou Int32(Type commun)
            int a = 0; // Combien vaut a ? (pan null) => type numérique non nullable => valeur par defaut
            int? b = null; // valeur par defaut => null
            if (b.HasValue)
            {
                int h = a + b.Value;
            }
            int d = a + b == null ? 8 : b.Value;
            int c = a + b ?? 8;

            //c = a + (int)b; // ne marche que si b n'est pas null sinon exception

            var e = a + b; // Inférence de type => int? e=a+b;  // int? e = a+b en phase de précompil
            // Bonne pratique => utiliser var pour les types complexes et expliciter pour les types simples


            // types numeriques => int, long, float, double, decimal
            int r = 2;
            double t = 3;
            var u = r / t;
            try
            {
                int m = int.MaxValue;
                int n = m + 1;
            }
            catch (Exception)
            {

            
            }


            checked
            {
                int m2 = int.MaxValue;
                //int n2 = m2 + 1;
            }
            unchecked
            {
                int m3 = int.MaxValue;
                //int n3 = m3 + 1;
            }

            Double d1 = 1; // Pas de perte d'information
            Double d2 = 1D;
            Single d3 = 1F;
            Decimal d4 = 1M;
            Int32 d5 = (Int32)1D; // Perte de donnée potentielle


            decimal v1 = 0;
            for (var i = 0; i < 100; i++)
            {
                v1 += 0.3M;
            }
            if (v1 == 30M)
            {
                Assert.Fail("Erreur de précision");

            }

            // commande
            // planche => longueur dermandée 2000.00 => Decimal
            // planche => longueur mesuree 2000.00 => Double
            Decimal longueurDemandee = 2000.00M;
            double longueurMesuree = 2000.04;
            if (longueurDemandee >= ((decimal)longueurMesuree-0.01M) && longueurDemandee <= ((Decimal)longueurMesuree) - 0.01M)
            {

            }
        }

        [TestMethod]
        public void DeclarationChaines()
        {
            string s; // s=> null
            s = "Toto"; //  54 45 23 45 en mémoire
            s = s + " est à la plage";

            // Attention ! Chaines non mutables => chaque concaténation crée une nouvelle chaine en mémoire
            s = "*";
            for(var i = 0; i < 1000; i++)
            {
                s += "*";  // s=> * // s=> ** // s=> *** // s=> ****  // 
            }

            // Bonne pratique => utiliser StringBuilder pour les chaines de grande taille
            StringBuilder sb = new StringBuilder("*");
            for (var i = 0; i < 1000; i++)
            {
                sb.Append("*");
            }
            s = sb.ToString();
        }

        [TestMethod]
        public void Tuple()
        {
            // Structure de données => regroupement de plusieurs valeurs
            var p1 = (3,4);
            var pComment=(1, 2, "Toto");
            pComment.Item1 = pComment.Item1 + 1;

            (int a,int b) p2 = (1, 2);
            // ValueTuple
            p1 = p2;
            p2.a++; // ne modifie pas p1 car p1 et p2 sont 2 instances différentes
            (int a,int b) p4=( 1,  2);

            p1=(a:1,b:2);
            p2=(c:1,d:2);
            if (p1 == p2)
            {
                // Assert permet de vérifierdes conditions
                // si p1 et P2 sont égaux => le test échoue
                Assert.AreNotEqual(p1, p2,"P1 et P2 ne sont pas égales");
                // p1 et p2 sont égaux car les valeurs sont égales
            }
        }

        [TestMethod]
        public void DeclarationClass()
        {

            Produit p = new Produit() { Prix = 12.0M, NbStock = 4 };
            p.Prix = 1;
            // Ce code créé 10000 instances de Produit => 10000 objets en mémoire
            GC.AddMemoryPressure(1000000); // Indique au GC qu'on va créer beaucoup d'objets => optimisation du GC

            for (var i = 0; i < 10000; i++)
            {
                if (i % 1000 == 0)
                {
                    GC.Collect(); // Force le GC à libérer la mémoire
                }
                p = new Produit();
            }
        }
    }
}
