using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ApiKalumNotas.Helpers;

namespace ApiKalumNotas.Entities
{
    public class AsignacionAlumno //: IValidatableObject
    {
        public string AsignacionId {get;set;}
        [Required(ErrorMessage = "El campo carne es requerido")]
        [Carne]
        public string Carne {get;set;}
        [Required(ErrorMessage = "El campo ClaseId es requerido")]
        public string ClaseId {get;set;}
        [Required(ErrorMessage = "El campo Fecha es requerido")]
        public DateTime FechaAsignacion {get;set;}
        public virtual Alumno Alumno {get;set;}
        public virtual Clase Clase {get;set;}

      /*  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            string fechaAuxiliar  = FechaAsignacion.ToShortDateString();
            if (!string.IsNullOrEmpty(fechaAuxiliar))
            {
                DateTime fechaSalida;
                if (DateTime.TryParse(fechaAuxiliar, out fechaSalida))
                {
                    yield return new ValidationResult("La fecha de asignacion es invalida", new string[]{nameof(FechaAsignacion)});

                }

                

            }
        }*/
    }
}