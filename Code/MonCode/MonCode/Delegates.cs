namespace MonCode;

[TestClass]
public class Delegates
{
    [TestMethod]
    public void DelegateTests()
    {
        var a = Addition(1, 2);

        Action<int, int> ac = (a, b) =>
        {
            Console.WriteLine($"a={a} b={b}");
        };
        Func<int,double,DateTime,string> f = (a, b, c) => $"{a} {b} {c:dd/MM/yyyy hh:mm}";

        Func<int,int,int> operation = Addition;
        a=operation(1, 2);
        operation = Soustraction;
        a = operation(1, 2);
        object o = Addition;
        o = Incrementation;
        ((Func<int,int>)o)(1);

        operation=(a,b)=> a * b;
        var op=(int a,int b) => a * b;
        operation=(int a,int b) =>
        {
            return a/b;
        };


    }


    // Création de fonction en déclaratif
    int Addition(int a, int b)
    {
        return a + b;
    }
    int Soustraction(int a, int b)
    {
        return a - b;
    }
    int Incrementation(int a)
    {
        return a + 1;
    }
}
