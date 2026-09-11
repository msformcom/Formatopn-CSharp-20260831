using System.Reflection.Emit;
using AlloCineDAL;
using AlloCineInterfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AlloCineServiceBDD
{
    public class AlloCineServiceFromDB : IAlloCineService
    {
        private IMapper mapper;
        private readonly AlloCineContext db;

        public AlloCineServiceFromDB(AlloCineContext db)
        {
            this.db = db;

            #region Configuration du mapper
            var builder = new MapperConfiguration(config =>
            {
                config.CreateMap<CategorieDAO, ICategorie>()
                .ConstructUsing(c => new Categorie())
                     .ForMember(c => c.Libelle, o => o.MapFrom(c => c.Label))
                    .ReverseMap();

                // 
                config.CreateMap<FilmDAO, IFilm>() // ; En s'arrêtant ici, les propriétés de même nom sont mappées
                                                   // IFilm.Code est mappé avec FilmDAO.Code
                    .ConstructUsing(c => new Film())
                    .ForMember(c => c.Titre, o => o.MapFrom(c => c.Title))
                    .ForMember(c => c.DateSortie, o => o.MapFrom(c => c.ReleaseDate))
                    .ForMember(c => c.Duree, o => o.MapFrom(c => c.Length * 60))
                    .ReverseMap()
                    .ForMember(c => c.Length, o => o.MapFrom(c => c.Duree / 60))
                    .ForMember(c => c.LastUpdate, o => o.MapFrom(c => DateTime.Now));

                config.CreateMap<CinemaDAO, ICinema>()
                .ConstructUsing(c => new Cinema())
                .ForMember(c => c.Nom, o => o.MapFrom(c => c.Name))
                .ForMember(c => c.NombreSalles, o => o.MapFrom(c => c.RoomCount))
                .ForMember(c => c.CodePostal, o => o.MapFrom(c => c.PostalCode))
                .ReverseMap()
                 .ForMember(c => c.OwnerName, o => o.MapFrom(c => "Inconnu"))
                 .ForMember(c => c.LastUpdate, o => o.MapFrom(c => DateTime.Now));
            }, new LoggerFactory());


            // Création du mapper
            this.mapper = builder.CreateMapper();
            #endregion


        }

        public async Task<IFilm> AddFilmAsync(IFilm film)
        {
            //var filmDAO = new FilmDAO()
            //{
            //    Code = film.Code,
            //    Length=film.Duree,
            //    Title=film.Titre.ToUpper(),
            //    ReleaseDate=film.DateSortie  };

            // Mappage du IFilm en FilmDAO
            var filmDAO = mapper.Map<FilmDAO>(film);
            // Repository => Dans le DbSet, le filmDAO est présent et marqué comme Inserted
            db.Films.Add(filmDAO);

            // Inspecte le DbSet Films à la recherche de données non synchronées 
            // => Inserted, Deleted, Modified
            // => Add => Inserted => INSERT
            // => Remove => Deleted => DELETE
            // => Modif dans une entite => Modified => UPDATE
            // Génère des requètes de synchro avec la BDD
            await db.SaveChangesAsync();
            return mapper.Map<IFilm>(filmDAO);
            //return new Film()
            //{
            //    Code=filmDAO.Code,
            //    Titre=filmDAO.Title,
            //    Duree=filmDAO.Length,
            //    DateSortie=filmDAO.ReleaseDate
            //};



        }

        public async Task<IEnumerable<ICinema>> GetCinemasAsync(ICinemaSearch search)
        {

            //// IQueryable + IEnumerable => Objet qui représente une requete dans la BDD
            //var r1 = db.Cinemas; // => SELECT * FROM TBL_Cinemas
            //var r2 = db.Cinemas.Where(c => c.PostalCode == "75000");// => SELECT * FROM TBL_Cinemas WHERE PostalCode="75000";
            //// .Where(c=>Math.Acosh(c.RoomCount)>0); // ACosH n'existe pas en SQL => erreur lors de la trabnscription en SQL
            //var r3 = r2.OrderBy(c => c.Name);// => SELECT * FROM TBL_Cinemas WHERE PostalCode="75000" ORDER BY Name ASC;

            //// var s1 = r3.ToList();// La requete est envoyee => resultats dans une liste
            //var s1 = r3.AsEnumerable(); // La constitution de la requete SQL s'arrète ici, le reste des operations se fera sur les résultats
            //// renvoyés par le serveur (quand l'enumeration commencera)
            //var s2 = s1.Where(c => Math.Acosh(c.RoomCount) > 0).Take(2); // Where sur IEnumerabe

            //var l1 = s2.ToList();

            // Requete ecrite avec Linq To Entities (sur IQueryable)
            IQueryable<CinemaDAO> requete = db.Cinemas;
            if (!string.IsNullOrWhiteSpace(search.CodePostal))
            {
                requete = requete.Where(c => c.PostalCode.ToUpper().StartsWith(search.CodePostal.Substring(0, 3).ToUpper()));
            }
            if (!string.IsNullOrWhiteSpace(search.NomPart))
            {
                requete = requete.Where(c => c.Name.ToUpper().Contains(search.NomPart.ToUpper()));
            }
            //return requete.Select(c => mapper.Map<Cinema>(c));//.Where(c=>c.Nom=="Paradiso");
            return requete.ProjectTo<Cinema>(mapper.ConfigurationProvider); // IQueryable
        }

        public Task<IEnumerable<ICinema>> GetCinemasByFilmAsync(string codeFilm)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IFilm>> GetFilmsByCinemaAsync(string codeCinema)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<IFilm>> SearchFilmsAsync(IFilmSearch search)
        {
            // SearchFilm (IFilmSearch..)
            // IFilmSearch : IEntitySearch
            // IEntitySearch => page + nbItemPerPage

            IQueryable<FilmDAO> requete = db.Films;
            if (search.DureeMax != null)
            {
                requete = requete.Where(c => c.Length <= search.DureeMax);
            }
            if (search.DureeMin != null)
            {
                requete = requete.Where(c => c.Length >= search.DureeMin);
            }
            if (!string.IsNullOrWhiteSpace(search.TitrePart))
            {
                requete = requete.Where(c => c.Title.Contains(search.TitrePart));
            }
            if (search.Page != 0 && search.NbItemsPerPage != 0)
            {
                requete = requete.Skip((search.Page - 1) * search.NbItemsPerPage).Take(search.NbItemsPerPage);

            }
            return requete.ProjectTo<Film>(mapper.ConfigurationProvider);
        }

        public async Task<ICinema> AddCinemaAsync(ICinema cinema)
        {
            var dao = mapper.Map<CinemaDAO>(cinema);
            db.Cinemas.Add(dao);
            await db.SaveChangesAsync();
            var cinemaInDb = db.Cinemas.Find(dao.Id);
            return mapper.Map<Cinema>(dao);


        }



        public async Task<ISeance> AddSeanceAsync(ISeance seance)
        {
            var seanceDAO = new SeanceDAO();
            var cinema = db.Cinemas.FirstOrDefault(c => c.Code == seance.Cinema.Code);
            if (cinema == null)
            {
                //throw new InvalidDataException("Le cinema n'existe pas");
                await AddCinemaAsync(seance.Cinema);
                cinema = db.Cinemas.FirstOrDefault(c => c.Code == seance.Cinema.Code);
            }
            var film = db.Films.FirstOrDefault(c => c.Code == seance.Film.Code);
            if (film == null)
            {
                await AddFilmAsync(seance.Film);
                film = db.Films.FirstOrDefault(c => c.Code == seance.Film.Code);
            }


            seanceDAO.Code = seance.Code;
            seanceDAO.IdCinema = cinema.Id;
            seanceDAO.IdFilm = film.Id;
            seanceDAO.Date = seance.Horaire;
            db.Add(seanceDAO);
            await db.SaveChangesAsync();
            return seance;
        }

        public async Task<ICategorie> GetCategoryByFilmAsync(string codeFilm)
        {
            var film = db.Films.FirstOrDefault(c => c.Code == codeFilm);
            if (film == null)
            {
                throw new Exception("Le film n'existe pas");
            }
            var categorie = db.Categories.FirstOrDefault(c => c.Id == film.IdCategorie);
            categorie = db.Films.Include(c => c.Categorie).FirstOrDefault(c => c.Code == codeFilm).Categorie;
           
            var resultat = mapper.Map<ICategorie>(categorie);
            return resultat as ICategorie;
        }
    }
}
