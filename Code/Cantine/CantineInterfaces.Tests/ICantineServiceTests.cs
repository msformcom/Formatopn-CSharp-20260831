using Microsoft.Extensions.DependencyInjection;

namespace CantineInterfaces.Tests
{
    [TestClass]
    public sealed class ICantineServiceTests
    {
        [TestMethod]
        public async Task ListeArticlesAsyncTests()
        {
            // Arrange
            // Avoir l'instance de ICantineService à tester
            // Ici on ne sait pas quel est le type précis que l'on va tester
            // Le CantineContext est Scoped : un scope par test évite de le partager
            // entre les méthodes exécutées en parallèle
            using var scope = DI.Services.CreateScope();
            ICantineService instance = scope.ServiceProvider.GetService<ICantineService>();
            Assert.IsNotNull(instance);

            var resultat = (await instance.ListeArticlesAsync(null)).ToList();

        }

        [TestMethod]
        public async Task MyTestMethod()
        {
            using var scope = DI.Services.CreateScope();
            ICantineService instance = scope.ServiceProvider.GetRequiredService<ICantineService>();
            var resultat=await instance.LireEmployeInfosAsync("AA0001");

            Assert.IsNotNull(resultat);


        }
    }
}
