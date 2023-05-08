using System.ComponentModel.DataAnnotations;

namespace Modelos.Validaciones
{
    public class PrimeraLetraMayusculaAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (null == value || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var primeraLetra = value.ToString()[0].ToString();

            if (primeraLetra.ToUpper() != primeraLetra)
            {
                return new ValidationResult("La primera letra debe ser mayúscula.");
            }

            return ValidationResult.Success;
        }
    }
}
