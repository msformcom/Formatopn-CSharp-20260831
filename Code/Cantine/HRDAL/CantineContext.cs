using System;
using System.Collections.Generic;
using System.Text;
using HRDAL.DAO;
using Microsoft.EntityFrameworkCore;

namespace HRDAL
{
    // Représente la partie de la base de données sur laquelle je travaille
    // Attention : Bonne pratique : ne pas créer un context couvrant toutes les tables
    public class CantineContext : DbContext
    {
        // DALCompta => EmployeComptaDAO => Id, Nom, Prenom, Salaire, Matricule +  Civilite => Migration ALTER TABLE Employes ADD ...
        // DALPetanque => EmployePetanqueDAO => Id, Nom, Prenom, RefInscriptionPretanque, NiveauPetanque, Civilite
        // DALCAntine => EmployeCantineDAO => Id, Nom, Prenom, Allergies => Ajout CreditRepas => Migration => ALTER TABLE Employes ADD CreditRepas Decimal
        // Table :  Id, Nom, Prenom, Salaire, Matricule,RefInscriptionPretanque, NiveauPetanque, Allergies


        // Ce context saura interroger la BDD pour la table des articles
        public DbSet<ArticleDAO>     Articles { get; set; }
    }
}
