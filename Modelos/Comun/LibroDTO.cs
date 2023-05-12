using Modelos.Entidades;
using Modelos.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Comun
{
    public class LibroDTO
    {
        public int Id { get; set; }
        public string Titulo { get; set; }
        public List<ComentarioDTO> Comentarios { get; set; } = new List<ComentarioDTO>();
    }
}
