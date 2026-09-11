using AlloCineDAL;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MesTests
{
    [TestClass]
    public sealed class DbContextTests
    {
        // Services me permet de construire des instances de class à partir des interface

        IServiceProvider Services = null;

        AlloCineContext db = null;

        public DbContextTests()
        {
            this.Services = DI.GetInjector();
            this.db=this.Services.GetRequiredService<AlloCineContext>();

        }

        


        [TestMethod]
        public void CreateDatabaseTest()
        {
            // Je demande à l'injecteur de dépendance de construire une instance de AlloCineContext
            // il va se charger de construire les objets nécessaires (dans le construteur)
            var db =this.Services.GetService<AlloCineContext>();

            if(db.Database.EnsureCreated())
            {
                var logger = this.Services.GetService<ILogger<DbContextTests>>();
                logger.LogInformation("La BDD vient d'être créée");
            }
        }





        [TestMethod]
        public void Insert()
        {
            // ChangeTracker 
            var changes = db.ChangeTracker.Entries().ToList(); // 0 Changements

            var cat = new CategorieDAO() { Code = "Cat3", Label = "Thriller" };
            db.Categories.Add(cat);

            var film = new FilmDAO() { Code = "Cat3", Title = "Thriller", Categorie=cat };
            db.Films.Add(film);

            changes = db.ChangeTracker.Entries().ToList(); // 2 entrées state Modified
            db.SaveChanges();
            changes = db.ChangeTracker.Entries().ToList(); // 2 entrées state UnChanged

            //using (var t1 = db.Database.BeginTransaction())
            //{
            //    using (var t2 = db2.Database.BeginTransaction())
            //    {

            //        db.SaveChanges(); // Toutes les modifs sont effectuées dans une transaction
            //                          // BEGIN TRAN
            //                          // INSERT INTO TBL_Categories....
            //                          // INSERT INTO TBL_Films... => ERREUR SQL => ROLLBACK
            //                          // COMMIT
            //        db2.SaveChanges();
            //        t1.Commit();
            //        t2.Commit();
            //    }
            //}
            db.AddRange(
            db.Films.Where(c => true).Select(c => new FilmDAO { Title = c.Title + "*", Code = c.Code + "*" }));
        }

        [TestMethod]
        public void Update()
        {
            var changes = db.ChangeTracker.Entries().ToList();
            var film = db.Films.First();

            // Attention à la concurrence => IsConcurrencyToken dans OnModelCreating
            film.Title += "*";
            changes = db.ChangeTracker.Entries().ToList();

            db.SaveChanges();
            changes = db.ChangeTracker.Entries().ToList();
        }
        [TestMethod]
        public void UpdateGlobal()
        {

            db.Films.Where(c => true).ExecuteUpdate(updates => updates
                                                                .SetProperty(e => e.Code, e => e.Title.Substring(0, 3))
                                                                .SetProperty(e => e.Title, e => e.Title + "*")
                                                                );
        }
        [TestMethod]
        public void Delete()
        {
            // en selectionnant le film en premier
            var film = db.Films.First(); // SELECT....
            db.Films.Remove(film);
            db.SaveChanges(); // DELETE FROM TBL_Films WHERE Id=...

            var film2 = new FilmDAO() { Id = Guid.Parse("Le guid") };
            db.Entry(film2).State = EntityState.Deleted; // Inserer dans le changeTracker et marquer le film comme supprimer
            db.SaveChanges();

            db.Films.Where(c => c.Code == "CodeFilm").ExecuteDelete();

        }

            [TestMethod]
        public void Select()
        {
            // Tous les films qui contiennent le mot Peur dans le titre
            var films = db.Films.Where(c => c.Title.Contains("Peur"));

            // Moyenne de la durée des films 
            var moyenneDuree = db.Films.Average(c => c.Length);

            var troisFilmsLesPlusLongs = db.Films.OrderByDescending(c => c.Length).Take(3);

            // Films categorie Horreur
            var idCat1 = db.Categories.FirstOrDefault(c => c.Label == "Horreur").Id;
            var filmsCat1 = db.Films.Where(c => c.IdCategorie == idCat1).ToArray();

            filmsCat1 = db.Categories.Include(c => c.Films).FirstOrDefault(c => c.Label == "Horreur").Films.ToArray();

          
            var cat1 = db.Categories.FirstOrDefault(c => c.Label == "Horreur");

            // J'obtiens la catégorie mais cat1.Films vide
            // Chargement des films associés à la catégorie 
            // Ici cat1.Films est vide
            db.Entry(cat1).Collection(c => c.Films).Load();
            // Ici cat1.Films contient les films

            var filmLePlusLong = db.Films.OrderByDescending(c => c.Length).First();
            // filmLePlusLong.Categorie=null
            // Chargement de la categorie du film
            db.Entry(filmLePlusLong).Reference(c => c.Categorie).Load();
            var catFilmLePlusLong = filmLePlusLong.Categorie;

            // Un select enregistre les entités selectionnées dans le change tracker
            // et il les surveille => Prend des resources
            // sauf si AsNoTracking => Economies => Pas de MAJ sur SaveChanges
            var selectionSansModifsPrevues = db.Films.AsNoTracking().Where(c => c.Title == "Odyssee");

        }
    }
}
