using AlloCineDAL;
using AlloCineInterfaces;
using AutoMapper;
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
                 .ForMember(c => c.LastUpdate, o => o.MapFrom(c=>DateTime.Now));
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

        public async Task<IEnumerable<ICinema>> GetCinemasAsync(string codePostal)
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
            var requete = db.Cinemas.Where(c => c.PostalCode.ToUpper().StartsWith(codePostal.Substring(0, 3).ToUpper()));
            return requete.Select(c => new Cinema()
            {
                Nom = c.Name,
                Code = c.Code,
                CodePostal = c.PostalCode,
                NombreSalles = c.RoomCount
            });
        }

        public Task<IEnumerable<ICinema>> GetCinemasByFilmAsync(string codeFilm)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IFilm>> GetFilmsByCinemaAsync(string codeCinema)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IFilm>> SearchFilmsByTextAsync(string searchText)
        {
            var requete = db.Films.Where(c => c.Title.Contains(searchText));
            return Task.FromResult(requete.AsEnumerable().Select(c => mapper.Map<IFilm>(c)));
            //return Task.FromResult<IEnumerable<IFilm>>(requete.Select(c => new Film()
            //{
            //    Code = c.Code,
            //    DateSortie = c.ReleaseDate,
            //    Duree = c.Length,
            //    Titre = c.Title
            //}) );
        }

        public async Task<ICinema> AddCinemaAsync(ICinema cinema)
        {
            var dao = mapper.Map<CinemaDAO>(cinema);
            db.Cinemas.Add(dao);
            await db.SaveChangesAsync();
            var cinemaInDb = db.Cinemas.Find( dao.Id);
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
    }
}
