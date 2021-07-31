using System;
using System.ComponentModel.DataAnnotations;
using ApiKalumNotas.Helpers;

namespace ApiKalumNotas.DTOs
{
    public class AsignacionAlumnoDTO
    {
         public string AsignacionId {get;set;}
        [Required(ErrorMessage = "El campo carne es requerido")]
        [Carne]
        public string Carne {get;set;}
        [Required(ErrorMessage = "El campo ClaseId es requerido")]
        public string ClaseId {get;set;}
        [Required(ErrorMessage = "El campo Fecha es requerido")]
        public DateTime FechaAsignacion {get;set;}
    }
}