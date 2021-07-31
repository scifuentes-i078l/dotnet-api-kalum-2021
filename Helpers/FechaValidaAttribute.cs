using System;
using System.ComponentModel.DataAnnotations;

namespace ApiKalumNotas.Helpers
{
    public class FechaValidaAttribute : ValidationAttribute
    {
         protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return ValidationResult.Success;
            }

            DateTime fechaActual = DateTime.Now;
            if ((DateTime)value< fechaActual)  
            {
                return new ValidationResult($"La fecha que se recibe {(DateTime)value} no puede ser menor a la fecha actual {fechaActual}");

            }
            return ValidationResult.Success;
        } 
    }
}