using Microsoft.Extensions.DependencyInjection;

namespace CantineInterfaces.Tests
{
    [TestClass]
    public sealed class ICantineServiceTests
    {
        [TestMethod]
        public void ListeArticlesAsyncTests()
        {
            // Arrange
            // Avoir l'instance de ICantineService à tester
            // Ici on ne sait pas quel est le type précis que l'on va tester
            ICantineService instance=DI.Services.GetService<ICantineService>();
            Assert.IsNotNull(instance); 


        }
    }
}
