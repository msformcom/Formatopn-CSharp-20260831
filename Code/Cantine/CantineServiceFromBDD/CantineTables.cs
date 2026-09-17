using HRDAL.DAO;
using Microsoft.EntityFrameworkCore;

namespace CantineServiceFromBDD
{
    // Nommage des tables et des colonnes, propre a l'application Cantine
    // HRDAL reste generique : chaque application fournit son nommage
    // via le Action<ModelBuilder> enregistre sous la cle "Builder1"
    public static class CantineTables
    {
        public static void Configure(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ArticleDAO>(options =>
            {
                options.ToTable("TBL_Articles");
                options.Property(c => c.Id).HasColumnName("PK_Article");
                options.Property(c => c.Reference).IsRequired().HasMaxLength(5).IsFixedLength();
            });

            modelBuilder.Entity<AchatDAO>(options =>
            {
                options.ToTable("TBL_Achats");
                options.Property(c => c.Id).HasColumnName("PK_Achat");
                options.Property(c => c.IdEmploye).HasColumnName("FK_Employe");
                options.Property(c => c.IdArticle).HasColumnName("FK_Article");
            });

            modelBuilder.Entity<EmployeDAO>(options =>
            {
                options.ToTable("TBL_Employes");
                options.Property(c => c.Id).HasColumnName("PK_Employe");
                options.Property(c => c.PublicId).IsRequired().HasMaxLength(5).IsFixedLength();
            });
        }
    }
}
