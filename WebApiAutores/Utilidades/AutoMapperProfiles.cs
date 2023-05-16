using AutoMapper;
using Modelos.Comun;
using Modelos.Entidades;

namespace WebApiAutores.Utilidades
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            // Configurar el mapeo automático del modelo DTO AutorCreacion a la entity Autor.
            CreateMap<AutorCreacionDTO, Autor>();
            CreateMap<Autor, AutorDTO>();
            CreateMap<Autor, AutorDTOConLibros>()
                .ForMember(autorDTOConLibros => autorDTOConLibros.Libros, opciones => opciones.MapFrom(MapLibros));
            CreateMap<LibroCreacionDTO, Libro>()
                .ForMember(libro => libro.AutoresLibros, opciones => opciones.MapFrom(MapAutoresLibros));
            CreateMap<Libro, LibroDTO>();
            CreateMap<Libro, LibroDTOConAutores>()
                .ForMember(LibroDTOConAutores => LibroDTOConAutores.Autores, opciones => opciones.MapFrom(MapLibroDTOAutores));
            CreateMap<ComentarioCreacionDTO, Comentario>();
            CreateMap<Comentario, ComentarioDTO>();
            CreateMap<LibroPatchDTO, Libro>().ReverseMap();

        }

        private List<AutorLibro> MapAutoresLibros(LibroCreacionDTO libroCreacionDTO, Libro libro)
        {
            var resultado = new List<AutorLibro>();

            if (null != libroCreacionDTO.AutoresIds)
            {
                foreach (var autorId in libroCreacionDTO.AutoresIds)
                {
                    resultado.Add(new AutorLibro()
                    {
                        AutorId = autorId
                    });
                }
            }

            return resultado;
        }

        private List<AutorDTO> MapLibroDTOAutores(Libro libro, LibroDTOConAutores libroDTO)
        {
            var resultado = new List<AutorDTO>();

            if (null != libro.AutoresLibros)
            {
                foreach(var autorLibro in libro.AutoresLibros)
                {
                    resultado.Add(new AutorDTO()
                    {
                        Id = autorLibro.AutorId,
                        Nombre = autorLibro.Autor.Nombre
                    });
                }
            }

            return resultado;
        }

        private List<LibroDTO> MapLibros(Autor autor, AutorDTOConLibros autorDTOConLibros)
        {
            var resultado = new List<LibroDTO>();

            if (null != autor.AutoresLibros)
            {
                foreach (var autorLibro in autor.AutoresLibros)
                {
                    resultado.Add(new LibroDTO()
                    {
                        Id = autorLibro.LibroId,
                        Titulo = autorLibro.Libro.Titulo
                    });
                }
            }

            return resultado;
        }
    }
}
