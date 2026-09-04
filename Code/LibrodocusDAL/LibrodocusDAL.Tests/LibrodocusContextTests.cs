using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LibrodocusDAL.Tests
{
    [TestClass]
    public sealed class LibrodocusContextTests
    {
        IServiceProvider Services=null;
        public LibrodocusContextTests()
        {
            var serviceCollection = new ServiceCollection();

           

            #region config
            // Je construis un objet de type IConfiguration
            ConfigurationBuilder builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
       
#if DEBUG
            builder.AddJsonFile("appsettings.dev.json");
#endif
            var config = builder.Build();
            serviceCollection.AddSingleton<IConfiguration>(config);

            #endregion

            #region logging
   
            // Ajout de la fenètre de debuggage aux sorties du looging

            serviceCollection.AddLogging(options =>
            {
                options.AddDebug();
            });


            #endregion


            #region Ajout du context
            serviceCollection.AddDbContext<LibrodocusContext>(
                options =>
                {
                    options.UseSqlServer("name=LibrodocusDB");
                }
            );
            #endregion

            Services=serviceCollection.BuildServiceProvider();

        }
        [TestMethod]
        public void InsertTest()
        {
            var db = Services.GetRequiredService<LibrodocusContext>();
            // Pour ce test, je m'assure de partir d'une base vide
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();


            // Insertion d'un genre
            var g = new GenreDAL() { Libelle = "Comédie" };
            db.Genres.Add(g);
            db.SaveChanges();


            // Insertion d'un livre
            db= Services.GetRequiredService<LibrodocusContext>();

            var l = new LivreDAL() { Genre=g, ISBN = "Toto", Resume = "C'est rigolo", Titre = "Y-at-il un pilote dans l'avion" };
            db.Livres.Add(l);
            db.Livres.Add(new LivreDAL() { Genre = g, ISBN = "Toto2", Resume = "C'est rigolo aussi", Titre = "Y-at-il un pilote dans l'avion 2" });

            db.SaveChanges();

            db = Services.GetRequiredService<LibrodocusContext>();
            var livresComique = db.Genres.FirstOrDefault(g => g.Libelle == "Comédie").Livres.ToList();


            db = Services.GetRequiredService<LibrodocusContext>();
            var genre = db.Genres.First();
            var livres = genre.Livres.ToList();


        }

        [TestMethod]
        public void SelectLivresDuGenre()
        {
           var db = Services.GetRequiredService<LibrodocusContext>();
            var genre = db.Genres.First();
            // var genre = db.Genres.Include(c=>c.Livres).First();

            // Je demande à charger les livres du GenreDAL genre
            db.Entry(genre).Collection(c => c.Livres).Load();
            var livres = genre.Livres;

        }


    }
}
