using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CodeTests
{
    [TestClass]
    public sealed class Tasktests
    {
        int Addition(int a, int b)
        {
            return a + b;
        }

        Task<int> AdditionAsync(int a, int b)
        {
            return Task.Run(() =>
            {
                var r = 0;
                for (var i = 0; i < a; i++)
                {
                    r++;
                }
                for (var i = 0; i < b; i++)
                {
                    r++;
                }
                return r;
            });
        }

        [TestMethod]
        public void AdditionTest()
        {
            // Ajouter 1000,2000,3000,4000
            var r1 = Addition(1000, 2000);
            var r2 = Addition(3000, 4000);
            var r = Addition(r1, r2);
        }



        [TestMethod]
        public void AdditionTest2()
        {
            var t1 = AdditionAsync(1000, 2000); // 5s de travail
            var t2 = AdditionAsync(3000, 4000); // 5s de travail

            // Ici, on arrive immédiatement
            var t1et2 = Task.WhenAll(t1, t2);// Nouvelle tache qui se terminera quand t1 et t2 seront terminées

            t1et2.ContinueWith(r =>
            {
                // 5s si t1 et t2 ont été exécutées par des coeurs distincts
                // 10 s si un seul coeur est impliqué dans les opérations t1 et t2
                var r1 = r.Result[0]; // résultat de la première tache
                var r2 = r.Result[1]; // résultat de la deuxième tache
                var t3 = AdditionAsync(r1, r2);
                t3.ContinueWith(r =>
                {
                    Trace.WriteLine(r.Result);
                });
                return t3;
            });

        }

        [TestMethod]
        public async Task<int> AdditionTest2Async()
        {
            //var rIntermediare = await Task.WhenAll(AdditionAsync(1000, 2000), AdditionAsync(3000, 4000));
            //var r = await AdditionAsync(rIntermediare[0], rIntermediare[1]);


            var t1 = AdditionAsync(1000, 2000); // 5s de travail
            var t2 = AdditionAsync(3000, 4000); // 5s de travail

            // Ici, on arrive immédiatement
            var t1et2 = Task.WhenAll(t1, t2);

            var r = await t1et2;// tout le code suivant le await est intégré dans une phase de précompilation
            // dans un continue with
            var r1 = r[0]; // résultat de la première tache
            var r2 = r[1]; // résultat de la deuxième tache

            try
            {
                var r3 = await AdditionAsync(r1, r2);

                Trace.WriteLine(r3);

                return r3;
            }
            catch (Exception)
            {

                throw;
            }

        }









        [TestMethod]
        public void TaskTest()
        {
            // Task => Objet qui représente un code en cours d'exécution
            // Implémentation du code à exécuter
            var t = new Task(() =>
            {
                for (var i = 0; i < 10000000000; i++)
                {
                    var a = i;
                }
            });

            t.Start(); // La tache s'exécute dans un thread séparé
            // Pas d'attente => le code arrive ici instantanément

            t.ContinueWith((r) =>
            {
                // Code à exécuter lorsque la tache est termine
            });

            t.Wait(); // Je demande à attendre la fin de la tache
        }

        [TestMethod]
        public void GenericaskTest()
        {
            // Task => Objet qui représente un code en cours d'exécution
            // Task<int> résultat attendu est du type entier
            // Implémentation du code à exécuter
            // Run => exécution démarrée automatique
            var t = Task.Run(() =>
            {
                for (var i = 0; i < 10000000000; i++)
                {
                    var a = i;
                }
                return 6;
            });



            t.ContinueWith((r) =>
            {
                if (r.IsCompletedSuccessfully)
                {
                    // Code à exécuter lorsque la tache est termine
                    Trace.WriteLine(r.Result);
                }
                else
                {

                    // code de gestion de l'erreur
                }

            });

            t.Wait(); // Je demande à attendre la fin de la tache
        }


    }
}
