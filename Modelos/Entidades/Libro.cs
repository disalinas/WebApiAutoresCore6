using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int AutorId { get; set; }
        public Autor? Autor { get; set; }

    }
}
