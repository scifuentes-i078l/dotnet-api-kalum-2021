using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;

namespace ApiKalumNotas.Helpers
{
    public class NumeroSeminariosAttribute: ValidationAttribute
    {
           protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            if (!Information.IsNumeric(value.ToString())||int.Parse(value.ToString())<0)  
            {
                return new ValidationResult("El NumeroSeminarios es invalido. El valor no puede ser negativo");

            }
            return ValidationResult.Success;
        }
    }
}