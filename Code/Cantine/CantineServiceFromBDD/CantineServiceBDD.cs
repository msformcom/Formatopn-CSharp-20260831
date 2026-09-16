using System.Transactions;
using AutoMapper;
using AutoMapper.QueryableExtensions;
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





        public async Task ConsommerArticleAsync(string matricule, string referenceArticle, int quantite = 1)
        {
            //using(var T = await db.Database.BeginTransactionAsync(IsolationLevel.RepeatableRead))
            //{

            var article = await db.Articles.Where(c => c.Reference == referenceArticle).FirstOrDefaultAsync();

            if (article == null)
            {
                throw new ArgumentException("Pas d'article avec cette référence");
            }
            var employe = await db.Employes.Where(c => c.PublicId == matricule).FirstOrDefaultAsync();
            if (employe == null)
            {
                throw new ArgumentException("Pas d'employé avec ce matricule");
            }
            if (article.Stock < quantite)
            {
                throw new ArgumentException("Stock insuffisant");
            }
            if (employe.CreditRepas < article.Price * quantite)
            {
                throw new ArgumentException("Crédit insuffisant");
            }

            var achat = new AchatDAO()
            {
                IdArticle = article.Id,
                IdEmploye = employe.Id,

                Quantite = quantite
            };
            // Ajout de l'achat à la BDD
            db.Achats.Add(achat); // achat est marquée comme Added dans le changetracker
            employe.CreditRepas = employe.CreditRepas - article.Price * quantite; // Modified
            article.Stock -= quantite; // Modified
            await db.SaveChangesAsync(); // Exécute les changements dans une transaction
                                         // T.Commit();
                                         //}
                                         // 1 créer un transaction scope
                                         // 2 associer plusieurs contexts au  transaction scope
                                         // 3 faire els op
                                         // 4 commitre le scope
        }

        public async Task IncrementerCreditEmployeAsync(string matricule, decimal montant)
        {
            var db = services.GetRequiredService<CantineContext>();

           
     

            // Selectionner puis mettre à jour
            var employe = db.Employes.FirstOrDefault(c => c.PublicId == matricule);
            if (employe == null)
            {
                throw new Exception("Matricule non trouvé");
            }
            employe.CreditRepas += montant;
            employe.Name = "Toto";

            
            await db.SaveChangesAsync();

            // UPDATE TBL_Employes SET CreditRepas=CreditRepas+1 WHERE CreditRepas<1000
            db.Employes.Where(c=>c.CreditRepas<1000)
                .ExecuteUpdate(u=>u.SetProperty(    // Nom de la propriété à modifier
                                                    c=>c.CreditRepas, 
                                                    // Nouvelle valeur (e représente l'enregistrement en cour)
                                                    e=>e.CreditRepas+1)
                                    //.SetProperty(    // Nom de la propriété à modifier
                                    //                c => c.CreditRepas,
                                    //                // Nouvelle valeur (e représente l'enregistrement en cour)
                                    //                e => e.CreditRepas + 1)


               );
            // DELETE FROM TBL_EMPLOYES WHERE 1=2
            db.Employes.Where(c=>false).ExecuteDelete();

         

        }

        public async Task<IEmploye> LireEmployeInfosAsync(string matricule)
        {

            // Journalisation
            var logger = services.GetRequiredService<ILogger<CantineServiceBDD>>();
            logger.LogInformation("Selection d'un employé par matricule");


            var employeDAO = await db.Employes.FirstOrDefaultAsync(c => c.PublicId == matricule);
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


            IQueryable<ArticleDAO> listedesIarticlesDAO = db.Articles; // SELECT * FROM TBL_Articles
                                                                       //.Where(c => c.Price > 1000)  SELECT * FROM TBL_Articles WHERE Price> 1000
                                                                       //
            if (search != null)
            {
                if (!string.IsNullOrWhiteSpace(search.SearchText))
                {
                    // J'ajoute la condition si SearchText n'est pas vide
                    listedesIarticlesDAO = listedesIarticlesDAO.Where(c => c.Label.Contains(search.SearchText));
                }
                if (search.PrixMax.HasValue)
                {
                    // J'ajoute la condition si PrixMax est non null
                    listedesIarticlesDAO = listedesIarticlesDAO.Where(c => c.Price <= search.PrixMax);
                }
            }


            var listedesIarticles = listedesIarticlesDAO
                                    // Project to permet de faire passer les méthodes
                                    // de IQueryable dans la requete SQL
                                    .Select(dao => new Article()
                                    {
                                        Libelle = dao.Label
                                    });



            //var testValues = listedesIarticles.ToList();


            // Ce que je renvois IEnumerable mais aussi IQueryble
            return listedesIarticles;

        }

        public Task SupprimerArticleAsync(string referenceArticle)
        {
            throw new Exception();
        }
    }
}
