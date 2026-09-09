namespace MesTests;

[TestClass]
public class DelegateTests
{
    [TestMethod]
    public void GenericDelegate()
    {
        var a = Addition(1, 2);

        Func<int,int,int> soustraction=(a,b) => a - b;
        var multiplication=(int a,int b) => a * b;

        int c = multiplication(1, 2);
        multiplication = (a, b) => a / b;

        Action<string> log=(s)=> Console.WriteLine(s);



        // Fonction définié de manière déclarative => disponible dès la première ligne de code
        int Addition(int a, int b)
        {
            return a + b;
        }
    }

    [TestMethod]
    public void Delegate()
    {
        OperationSurEntiers addition = (a, b) => a+b;
    }
    delegate int OperationSurEntiers(int a, int b);
}
