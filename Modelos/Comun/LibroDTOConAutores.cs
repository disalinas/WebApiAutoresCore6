using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Comun
{
    public class LibroDTOConAutores : LibroDTO
    {
        public List<AutorDTO> Autores { get; set; }
    }
}
