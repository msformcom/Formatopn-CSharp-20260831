using System;
using System.Collections.Generic;
using System.Text;

namespace CodeTests
{

    [TestClass]
    public class ReflexionTests
    {
        [TestMethod]
        public void MyTestMethod()
        {
            var o = new Employe();
            var x = o.GetProperty<int>("X");
            Assert.AreEqual(0, x);
        }
    }

    public static class MesExtensions
    {
        public static T GetProperty<T>(this object o,string name)
        {
            var t = typeof(T);
            var property= t.GetProperty(name, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            var value = (T)property.GetValue(o);
            return value;
        }
    }

    public class Employe
    {
        private int X { get; set; }
    }
}
