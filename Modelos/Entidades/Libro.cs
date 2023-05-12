using Modelos.Validaciones;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [Required]
        [PrimeraLetraMayuscula]
        [StringLength(maximumLength:250)]
        public string Titulo { get; set; }
        public List<Comentario> Comentarios { get; set; }
        public List<AutorLibro> AutoresLibros { get; set; }

    }
}
