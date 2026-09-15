using AutoMapper;
using CantineInterfaces;
using CantineServiceFromBDD.Models;
using HRDAL;
using Microsoft.EntityFrameworkCore;

namespace CantineServiceFromBDD
{
    public class CantineServiceBDD : ICantineService
    {
        private readonly CantineContext db;
        private readonly IMapper mapper;

        public CantineServiceBDD(CantineContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public Task ConsommerArticleAsync(string matricule, string referenceArticle)
        {
            var e = new Employe() { Nom=null};

          
            throw new NotImplementedException();
        }

        public Task IncrementerCreditEmployeAsync(string matricule, decimal montant)
        {
            throw new NotImplementedException();
        }

        public async Task<IEmploye> LireEmployeInfosAsync(string matricule)
        {
            var employeDAO= await db.Employes.FirstOrDefaultAsync(c=>c.PublicId== matricule);
            if (employeDAO == null)
            {
                throw new Exception("Matricule non trouvé");
            }

            // Les règles de conversion sont décrites dans CantineMapping
            return mapper.Map<Employe>(employeDAO);
           
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


            // AutoMapper travaille sur des objets déjà chargés
            // La requête est donc matérialisée avant le mapping
            var listeDesDAOs = await db.Articles.ToListAsync(); // SELECT * FROM TBL_Articles
            var listedesIarticles = mapper.Map<List<Article>>(listeDesDAOs);

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
