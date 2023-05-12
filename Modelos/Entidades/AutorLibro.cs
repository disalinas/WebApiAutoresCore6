using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class AutorLibro
    {
        public int LibroId { get; set; }
        public int AutorId { get; set; }
        public int Orden { get; set; }
        public Libro Libro { get; set; }
        public Autor Autor { get; set; }
    }
}
