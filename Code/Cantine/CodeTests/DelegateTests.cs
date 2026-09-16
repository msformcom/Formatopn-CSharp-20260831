using System;
using System.Collections.Generic;
using System.Text;

namespace CodeTests
{
    [TestClass]
    public class DelegateTests
    {
        [TestMethod]
        public void VariablesDeTypeFonctionTests()
        {
            var b = 2;
            var a = Addition(1, b);

            Action<int, int> action1 = (a, b) => { 
            };
            Func<int, int, string> op = (a, b) => (a + b).ToString();
            Func<int, int, int> operation = (int a, int b) => { return a + b; };
            operation = (int a, int b) => a + b;
            operation = (a, b) => a-b;

            var operation2=(int a, int b) => a + b;

            a = operation(1, 2);
            operation = Addition;

            // Func<int, int, int> est un délégué Function avec deux pramas int qui retourne un int
            // le dernier type => type de retour de la Function
            // Action<int,int> Méthode retour void avec deux pramas int

            //OperationDelegate opNommee = (a, b) => (a + b).ToString();


            // Méthode déclarative
            int Addition(int a, int b)
            {
                return a + b;
            }
        }

        // Equivalent nommé de Func<int,int,string>
        delegate string OperationDelegate (int a, int b);

    }
}
