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
    public class Autor : IValidatableObject
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "El campo {0} es obligatorio.")]
        [StringLength(maximumLength:50, ErrorMessage = "El campo {0} no puede tener una longitud superior de {1} caracteres.")]
        [PrimeraLetraMayuscula]
        public string Nombre { get; set; }
        
        //[Range(18, 99)]
        //[NotMapped] // Propiedad Edad que no tiene vinculación con la tabla Autores.
        //public int Edad { get; set; }
        
        //[CreditCard] // Valida que sea un número de tarjeta de crédito/débito.
        //[NotMapped] // Propiedad Edad que no tiene vinculación con la tabla Autores.
        //public string TarjetaCredito { get; set; }
        
        //[Url]
        //[NotMapped] // Propiedad Edad que no tiene vinculación con la tabla Autores.
        //public string url { get; set; }
        
        public List<Libro> Libros { get; set; }

        // Regla de validación del propio modelo. Para que se ejecute esta validación, primero se tienen que pasar todas las reglas
        // de validación a nivel de atributo que hay en las propiedades.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!string.IsNullOrEmpty(this.Nombre))
            {
                var primeraLetra = this.Nombre[0].ToString();

                if (primeraLetra.ToUpper() != primeraLetra)
                {
                    // Yield permite ir añadiendo datos al IEnumerable que devuelve el método 'Validate'.
                    // Así se irían añadiendo los errores con otras validaciones.
                    yield return new ValidationResult("La primera letra debe ser mayúscula.", new string[] {nameof(this.Nombre)});
                }
            }
        }
    }
}
