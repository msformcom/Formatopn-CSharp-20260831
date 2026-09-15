using AutoMapper;
using CantineInterfaces;
using CantineServiceFromBDD.Mappings;
using CantineServiceFromBDD.Models;
using HRDAL;
using HRDAL.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CantineServiceFromBDD
{
    public class CantineServiceBDD : ICantineService
    {


        protected static IMapper mapper = null;
        


        static CantineServiceBDD()
        {
            // Configuration du mapping DAO <=> Interfaces
            var configMapping = new MapperConfiguration(config =>
            {
                // Configuration de l'objet mapper pour mapper de IArticle vers ArticleDAO
                config.AddArticleMapping();
                config.AddEmployeMapping();
            }, LoggerFactory.Create(o => { }));
            mapper = configMapping.CreateMapper();    
        }

        private readonly CantineContext db;
        private readonly IServiceProvider services;
  

        public CantineServiceBDD(CantineContext db, 
                // Référence vers l'injection de dépendance,
                //ILogger<CantineServiceBDD> logger,
                IServiceProvider services)
        {
            // logger typé pour besoins de suivi
            
            this.db = db;
            this.services = services;
       

        }

        public Task ConsommerArticleAsync(string matricule, string referenceArticle)
        {
            var e = new Employe() { Nom=null};

          
            throw new NotImplementedException();
        }

        public Task IncrementerCreditEmployeAsync(string matricule, decimal montant)
        {
            var db=services.GetRequiredService<CantineContext>();
            throw new NotImplementedException();
        }

        public async Task<IEmploye> LireEmployeInfosAsync(string matricule)
        {
         
            // Journalisation
            var logger=services.GetRequiredService<ILogger<CantineServiceBDD>>();
            logger.LogInformation("Selection d'un employé par matricule");


            var employeDAO= await db.Employes.FirstOrDefaultAsync(c=>c.PublicId== matricule);
            if (employeDAO == null)
            {
                throw new Exception("Matricule non trouvé");
            }

            return mapper.Map<IEmploye>(employeDAO);

            //return new Employe()
            //{
            //    DateNaissance = employeDAO.BirthDate,
            //    Matricule = employeDAO.PublicId,
            //    Nom = employeDAO.Name,
            //    Prenom = employeDAO.Surname
            //};
        }

        public async Task<IEnumerable<IArticle>> ListeArticlesAsync(IArticleSearch search)
        {
            // Accéder à la BDD via le CantineContext pour obtenir la liste des éléments à renvoyer
            // Le CantineContext est fourni par DI lors de la construction

            // var listeDesDAOs = db.Articles; //.Where(c=>c.Price<10);

            // db.Vecteurs; // Vecteur => X, Y
            // SELECT * FROM TBL_Vecteurs
            //db.Vecteurs.Where(v=>v.x>0) // Linq to Entities => IQueryable => Modifie la requete qui est à gauche
            // SELECT * FROM TBL_Vecteurs WHERE X>0 // L'instruction Linq est traduite en SQL (Store expression)

            //db.Vecteurs.Where(v => Math.Acosh(x) > 0);
            // SELECT * FROM TBL_Vecteurs WHERE (Impossible à traduire)


            //db.Vecteurs.ToList().Where(v => Math.Acosh(x) > 0)
            // SELECT X,Y FROM TBL_Vecteurs => Tous les enregistrements dans une liste => Le filtre s'effectue sur la liste

            // DbSet => Methodes IQueryable (Where) => AsEnumerable => Methodes IEnumerable => List()

            //var requete = db.Vecteurs.AsEnumerable().Where(v => Math.Acosh(x) > 0);
            //var elements=requete.Take(10).ToList();
            // SELECT X,Y FROM TBL_Vecteurs => 100 premiers enregistre => Filtre appliqué 1 par 1 => Take => 10 => Arreter la lecture


            var listedesIarticlesDAO = db.Articles; // SELECT * FROM TBL_Articles
                                                 //.Where(c => c.Price > 1000)  SELECT * FROM TBL_Articles WHERE Price> 1000
                                                 //
              var listedesIarticles=listedesIarticlesDAO.ToList()
                                    .Select(dao => 
                                    mapper.Map<IArticle>(dao)
                                    );
                                    // mapper.map<ArticleDAO>(art)
                                    //.Select(dao => new Article()
            //{
            //    Reference = dao.Reference,
            //    Libelle = dao.Label,
            //    Photo = dao.Photo,
            //    Prix = dao.Price,
            //    Allergenes = dao.Allergens.Split(',').ToList()
            //}); // SELECT Reference, Label, Photo,Price, Allergens FROM TBL_Articles
            // Puis new Article()

            //SELECT Reference, Label, Photo, Price, Allergens FROM TBL_Articles

            // Exemple plus complexe non transcriptible en SQL
            //{
            //    var article = new Article()
            //    {
            //        Reference = dao.Reference,
            //        Libelle = dao.Label,
            //        Photo = dao.Photo,
            //        Prix = dao.Price
            //    };
            //    // Allergenes doit être une ICollection (IArticle)
            //    // C'est une chaine simple séparée par des , dans la bdd
            //    article.Allergenes = dao.Allergens.Split(',').ToList();
            //    return article;
            //});



            return listedesIarticles;
            
        }

        public Task SupprimerArticleAsync(string referenceArticle)
        {
            throw new NotImplementedException();
        }
    }
}
