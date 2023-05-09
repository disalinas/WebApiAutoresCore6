using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Modelos.Validaciones;

namespace Modelos.Entidades
{
    public class Autor
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(maximumLength:50, ErrorMessage = "El campo {0} no puede tener una longitud superior de {1} caracteres.")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        
    }
}
