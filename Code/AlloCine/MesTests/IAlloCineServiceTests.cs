using AlloCineInterfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using AlloCineServiceBDD;
using AlloCineDAL;

namespace MesTests;

[TestClass]
public class IAlloCineServiceTests
{

    public IAlloCineServiceTests()
    {
        var context = DI.GetInjector().GetRequiredService<AlloCineContext>();
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();
    }

    IServiceProvider services = DI.GetInjector();
    [TestMethod]
    public async Task GetCinemasAsyncTest()
    {
        // Je demande une instance de IAlloCineService à l'injector
        // La configuration de l'injection détermine quelle classe est fournie
        IAlloCineService service = services.GetRequiredService<IAlloCineService>();
        Assert.IsNotNull(service);


        // J'obtiens un enumerable => Rien est encore matérialisé
        var cinemasParisiensQuery = await service.GetCinemasAsync("75000");

        var resultats = cinemasParisiensQuery.ToList();
        Assert.AreEqual(1, resultats.Count);
        var cinema = resultats.First();
        Assert.AreEqual("Paradiso", cinema.Nom);


    }

    [TestMethod]
    public async Task SearchFilmsByTextAsyncTest()
    {
        // Je demande une instance de IAlloCineService à l'injector
        // La configuration de l'injection détermine quelle classe est fournie
        IAlloCineService service = services.GetRequiredService<IAlloCineService>();
        Assert.IsNotNull(service);


        // J'obtiens un enumerable => Rien est encore matérialisé
        var filmsSearch = await service.SearchFilmsByTextAsync("soupe");

        var resultats = filmsSearch.ToList();
        Assert.HasCount(1, resultats);
        var film = resultats.First();
        Assert.AreEqual("La soupe aux choux", film.Titre);

    }
    [TestMethod]
    public async Task AjoutAddFilmAsyncTest()
    {
        var film = new Film()
        {
            Code = "AH345",
            DateSortie = new DateOnly(),
            Duree = 100,
            Titre = "L'Odysee"
        };
        IAlloCineService service = services.GetRequiredService<IAlloCineService>();
        Assert.IsNotNull(service);

        var filmInsere = await service.AddFilmAsync(film);

        var filmsInBDD = await service.SearchFilmsByTextAsync("Odysee");
        Assert.HasCount(1, filmsInBDD);
    }

    [TestMethod]
    public async Task AddCineFilmSeanceTest()
    {

        ;


        // le scope est IDisposable
        // Tote resource demandée à l'injecteur par l'intermédaire du scope
        // qui est IDisposable sera fermée en même temps que le scope
        using (var scope = DI.GetInjector().CreateScope()!)
        {
            var service = scope.ServiceProvider.GetRequiredService<IAlloCineService>();
            var cinema = await service.AddCinemaAsync(new Cinema()
            {
                Code = "Cinema1",
                CodePostal = "75000",
                NombreSalles = 2,
                Nom = "Cinema de la Mairie"
            });
        }

        //}
        //using (var scope = DI.GetInjector().CreateScope()!)
        //{
        //    var service = scope.ServiceProvider.GetRequiredService<IAlloCineService>();
        //    var films = await service.AddFilmAsync(new Film()
        //    {
        //        Code = "Film1",
        //        DateSortie=new DateOnly(),
        //        Duree=10000,
        //        Titre = "Tous mes amis sont morts"
        //    });

        //}

        using (var scope = DI.GetInjector().CreateScope()!)
        {
            var service = scope.ServiceProvider.GetRequiredService<IAlloCineService>();
            var seance = await service.AddSeanceAsync(new Seance()
            {
                Code = "Seance1",
                Cinema = new Cinema()
                {
                    Code = "Cinema1",
                    //         CodePostal="75000",
                    //         NombreSalles=2,
                    //         Nom="Cinema de la Mairie"
                    //    }

                },
                Film = new Film()
                {
                    Code = "Film1",
                    DateSortie = new DateOnly(),
                    Duree = 10000,
                    Titre = "Tous mes amis sont morts"
                }
            });

        }


    }

}
