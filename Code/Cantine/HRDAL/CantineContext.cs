using System;
using System.Collections.Generic;
using System.Text;
using HRDAL.DAO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace HRDAL
{
    // Représente la partie de la base de données sur laquelle je travaille
    // Attention : Bonne pratique : ne pas créer un context couvrant toutes les tables
    public class CantineContext : DbContext
    {
        private readonly Action<ModelBuilder>? continueModelBuilding;

        // le constructeur de CantineContext reçoit les options de config du contexte
        // et les passe au constructeur de la classe de base
        public CantineContext(DbContextOptions<CantineContext> options,
            // Je reçois de la part du DI la fonction qui finit la config du Model
            [FromKeyedServices("Builder1")] Action<ModelBuilder>? continueModelBuilding=null
            
            ) : base(options)
        {
            this.continueModelBuilding = continueModelBuilding;
        }
        // DALCompta => EmployeComptaDAO => Id, Nom, Prenom, Salaire, Matricule +  Civilite => Migration ALTER TABLE Employes ADD ...
        // DALPetanque => EmployePetanqueDAO => Id, Nom, Prenom, RefInscriptionPretanque, NiveauPetanque, Civilite
        // DALCAntine => EmployeCantineDAO => Id, Nom, Prenom, Allergies => Ajout CreditRepas => Migration => ALTER TABLE Employes ADD CreditRepas Decimal
        // Table :  Id, Nom, Prenom, Salaire, Matricule,RefInscriptionPretanque, NiveauPetanque, Allergies


        // Ce context saura interroger la BDD pour la table des articles
        public DbSet<ArticleDAO> Articles { get; set; }
        public DbSet<EmployeDAO> Employes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuration des spécifications pour la BDD (commun à toutes les BDD)
            // Configuration pour ArticleDAO
            modelBuilder.Entity<ArticleDAO>(options =>
            {
                options.HasKey(a => a.Id);
                options.HasIndex(a => a.Reference);
                options.Property(c => c.Price).HasPrecision(18, 2);
                options.Property(c => c.Label).HasMaxLength(100);
                options.Property(c => c.Reference).IsUnicode(false).HasMaxLength(5);


                var article1 = new ArticleDAO() { Label = "Purée", Reference = "P01", Price = 12 };
                var article2 = new ArticleDAO() { Label = "Steak", Reference = "S01", Price = 15 };

                options.HasData(article1, article2);
            });

            modelBuilder.Entity<EmployeDAO>(options =>
            {

                // En installant un provider ici, je pourrais donner le nom de la table

                options.HasKey(a => a.Id);//.ToTable("TBL_Employes");
                options.HasIndex(a => a.PublicId);
                options.Property(c => c.Name).HasMaxLength(100);
                options.Property(c => c.Surname).HasMaxLength(100);
                options.Property(c => c.PublicId).IsUnicode(false).HasMaxLength(6);
                var employe1 = new EmployeDAO() { PublicId = "AA0001", Name = "NAME0001", Surname = "SURNAME0001", BirthDate = new DateOnly(1996, 2, 12) };
                var employe2 = new EmployeDAO() { PublicId = "AA0002", Name = "NAME0002", Surname = "SURNAME0002", BirthDate = new DateOnly(2002, 4, 7) };



                options.HasData(employe1, employe2);
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
