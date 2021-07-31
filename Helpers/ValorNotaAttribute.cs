using System.ComponentModel.DataAnnotations;
using Microsoft.VisualBasic;

namespace ApiKalumNotas.Helpers
{
    public class ValorNotaAttribute : ValidationAttribute
    
    {
         protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }
            if (!Information.IsNumeric(value.ToString())||int.Parse(value.ToString())>100||int.Parse(value.ToString())<0)  //funcion de visual basic
            {
                return new ValidationResult("El valorNota es invalido. El rango de valor aceptado es de 0 a 100");

            }
            return ValidationResult.Success;
        }
        
    }
}