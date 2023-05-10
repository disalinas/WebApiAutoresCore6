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
            CreateMap<AutorCreacion, Autor>();
        }
    }
}
