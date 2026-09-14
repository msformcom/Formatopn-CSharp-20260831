namespace CodeTests
{
    [TestClass]
    public sealed class TaskTests
    {
        int Addtion(int a, int b)
        {
            var r = 0;
            for (int i = 0; i < a; i++) r++;
            for (int i = 0; i < b; i++) r++;
            return r;
        }

        // Version Asynchrone de l'addition longue
        Task<int> AdditionAsync(int a, int b)
        {
            // Lance une opération dans une tache
            // et retourne l'objet représentant la tache lancée
            return Task.Run(() =>
            {
                var r = 0;
                for (int i = 0; i < a; i++) r++;
                for (int i = 0; i < b; i++) r++;
                return r;
            });
        }

   
        public async Task<int> AdditionAsync(int a, int b, int c, int d)
        {
            //return AdditionAsync(a, b).ContinueWith((t1) =>
            //{
            //    var r1 = t1.Result;
            //  AdditionAsync(r1, c).ContinueWith((t2) =>
            //  {
            //      var r2 = t2.Result;
            //      AdditionAsync(r2, d).ContinueWith((t3) =>
            //      {
            //          var r3 = t3.Result;
            //          return r3;
            //      });
            //  });
            //});


            var r1 = await AdditionAsync(a, b);
            var r2 = await AdditionAsync(r1, c);
            var r3 = await AdditionAsync(r2, d);
            Console.WriteLine(r3);
            return r3;
            // code qui suit
        }

        [TestMethod]
        public async Task MyTestMethod()
        {
            AdditionAsync(1, 2, 3, 4).ContinueWith(t =>
            {
                Console.WriteLine(t.Result);
            });
            // on arrive instantanémernt ici


            Console.WriteLine(await AdditionAsync(1, 2, 3, 4));
            // Attente de la fin de AdditionAsync

            // Aucune attente
            AdditionAsync(1, 2); // .ContinueWith...

            // Attente
            var r = AdditionAsync(1, 2).Result;

            // Exploitation du résultat mais sans attente
            r = await AdditionAsync(1, 2);





        }


        [TestMethod]
        public void AdditionAsyncTest()
        {
            // Lance l'additionAsync Additionner 1,2,3,4
            AdditionAsync(1, 2).ContinueWith(t1 =>
            {
                var r1 = t1.Result;
                AdditionAsync(3, 4).ContinueWith(t2 =>
                {
                    var r2 = t2.Result;
                    AdditionAsync(r1, r2).ContinueWith(t3 =>
                    {
                        Console.WriteLine(t3.Result); // 15s
                    });
                });

            });

            Task.WhenAll(AdditionAsync(1, 2), AdditionAsync(3, 4)).ContinueWith(t1et2 =>
            {
                // les additions 1 et 2 sont démarées ensemble mais le nombre de coeur
                // détermine si elles sont réellement paralelles
                var r1 = t1et2.Result[0];
                var r2 = t1et2.Result[1];
                // Entre 5 et 10 s
                AdditionAsync(r1, r2).ContinueWith(tFinal =>
                {
                    // En 10 s minimum, 15 max
                    Console.WriteLine(tFinal.Result);
                });

            });



            // Le code ici  s'éxécute sans attente
        }


        [TestMethod]
        public void CreateTaskTest()
        {
            // Version de Addtion blocante
            var c = Addtion(int.MaxValue, int.MaxValue);






            var t = new Task<int>(() =>
            {
                // Debut 
                var r = 0;
                for (var i = 0; i < 100000000; i++)
                {
                    r++;
                }
                return r;
                // Fin après un temps de l'ordre du ms => Asynchrone
            });

            // t est une tache
            // variable qui représente l'opération définie plus haut
            // Cette opération n'est pas encore démarée
            t.Start(); //  t démarre dans un thread séparée (pas blocant dans le code ici
                       // Instantanément ici

            t.ContinueWith(tTerminee =>
            {
                if (tTerminee.IsCompletedSuccessfully)
                {
                    // Pas d'attente de thread car ici t est terminée
                    Console.WriteLine(tTerminee.Result);
                }
                else
                {
                    // Traitement du problème
                }

            });

      
            // je peux faire des choses en attendant la fin de t

            // Attente du résultat => blocant 
            //var r = t.Result;






        }
    }
}
