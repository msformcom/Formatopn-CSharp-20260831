using HRDAL.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HRDAL
{
    // Représente la partie de la base de données sur laquelle je travaille
    // Attention : Bonne pratique : ne pas créer un context couvrant toutes les tables
    public class CantineContext : DbContext
    {
        private readonly Action<ModelBuilder>? continueModelBuilding;
        private readonly IConfiguration config;
        private readonly DbDataModel dbDataModel;

        // le constructeur de CantineContext reçoit les options de config du contexte
        // et les passe au constructeur de la classe de base
        public CantineContext(DbContextOptions<CantineContext> options,
            // Demande à l'injecteur de dépendance un accès à la configuration
                 IConfiguration config,
                 DbDataModel dbDataModel,
            // Je reçois de la part du DI la fonction qui finit la config du Model
            [FromKeyedServices("Builder1")] Action<ModelBuilder>? continueModelBuilding=null
       
            
            ) : base(options)
        {
            this.continueModelBuilding = continueModelBuilding;
            this.config = config;
            this.dbDataModel = dbDataModel;
        }
        // DALCompta => EmployeComptaDAO => Id, Nom, Prenom, Salaire, Matricule +  Civilite => Migration ALTER TABLE Employes ADD ...
        // DALPetanque => EmployePetanqueDAO => Id, Nom, Prenom, RefInscriptionPretanque, NiveauPetanque, Civilite
        // DALCAntine => EmployeCantineDAO => Id, Nom, Prenom, Allergies => Ajout CreditRepas => Migration => ALTER TABLE Employes ADD CreditRepas Decimal
        // Table :  Id, Nom, Prenom, Salaire, Matricule,RefInscriptionPretanque, NiveauPetanque, Allergies

        // Gestion des dates Xréation / Modification
        public override int SaveChanges()
        {
            RenseignerDatesGestion();
            return base.SaveChanges();
        }

        // Même traitement pour la voie asynchrone, sinon les dates de gestion restent vides
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            RenseignerDatesGestion();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void RenseignerDatesGestion()
        {
            foreach(var e in this.ChangeTracker.Entries())
            {
                if(e.State== EntityState.Modified && e.Entity is IGestionData g)
                {
                   // g.Property(c => c.DateModification).IsModified = false ;
                    g.DateModification=DateTime.Now;
                }
                if (e.State == EntityState.Added && e.Entity is IGestionData g2)
                {
                    g2.DateModification = DateTime.Now;
                    g2.DateCreation = DateTime.Now;
                }
            }
        }

        // Ce context saura interroger la BDD pour la table des articles
        public DbSet<ArticleDAO> Articles { get; set; }
        public DbSet<EmployeDAO> Employes { get; set; }

        public DbSet<AchatDAO> Achats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuration des spécifications pour la BDD (commun à toutes les BDD)
            // Configuration pour ArticleDAO
            modelBuilder.Entity<ArticleDAO>(options =>
            {
               
                options.HasKey(a => a.Id);
                options.HasIndex(a => a.Reference);
                options.Property(c => c.Stock).IsConcurrencyToken();
                // Utilisation de la configuration pour gérer le nombre de décimales 
                // du prix
                var nbDecimals = dbDataModel.NbDecimals;
                options.Property(c => c.Price).HasPrecision(18, nbDecimals);
                options.Property(c => c.Label).HasMaxLength(dbDataModel.LabelLength);
                options.Property(c => c.Reference).IsUnicode(false).HasMaxLength(5);


                var article1 = new ArticleDAO() { Label = "Purée", Reference = "P0001", Price = 12, Allergens="" };
                var article2 = new ArticleDAO() { Label = "Steak", Reference = "S0001", Price = 15, Allergens="Cianure, Gluten" };
                options.HasMany(c => c.Achats).WithOne(c => c.Article).HasForeignKey(c => c.IdArticle).OnDelete(DeleteBehavior.Restrict);
            
                options.HasData(article1, article2);
            });

            modelBuilder.Entity<EmployeDAO>(options =>
            {

                // En installant un provider ici, je pourrais donner le nom de la table

                options.HasKey(a => a.Id);//.ToTable("TBL_Employes");
                options.HasIndex(a => a.PublicId);
                options.Property(c => c.CreditRepas).IsConcurrencyToken();
                options.Property(c => c.Name).HasMaxLength(100);
                options.Property(c => c.Surname).HasMaxLength(100);
                options.Property(c => c.PublicId).IsUnicode(false).HasMaxLength(5);
                var employe1 = new EmployeDAO() { PublicId = "AA001", Name = "NAME0001", Surname = "SURNAME0001", BirthDate = new DateOnly(1996, 2, 12) };
                var employe2 = new EmployeDAO() { PublicId = "AA002", Name = "NAME0002", Surname = "SURNAME0002", BirthDate = new DateOnly(2002, 4, 7) };



                options.HasData(employe1, employe2);
            });

            modelBuilder.Entity<AchatDAO>(options => {
                options.HasKey(a => a.Id);
                options.HasOne(c => c.Employe).WithMany(c => c.Achats).HasForeignKey(c => c.IdEmploye).OnDelete(DeleteBehavior.Restrict);

            });

            if (continueModelBuilding != null)
            {
                continueModelBuilding(modelBuilder); // => {
                                                     //modelBuilder.ToTable()
                                                     //}
            }
        }
    }
}
