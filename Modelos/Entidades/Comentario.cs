using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Comentario
    {
        public int Id { get; set; }
        public string contenido { get; set; }
        public string IdLibro { get; set; }
        public Libro Libro { get; set; }
    }
}
