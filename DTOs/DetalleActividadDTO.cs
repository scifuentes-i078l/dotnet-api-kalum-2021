using System;
using System.ComponentModel.DataAnnotations;
using ApiKalumNotas.Helpers;

namespace ApiKalumNotas.DTOs
{
    public class DetalleActividadDTO
    {
         public string DetalleActividadId {get;set;}

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string SeminarioId {get;set;}

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string NombreActividad {get;set;}

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public int NotaActividad {get;set;}

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [FechaValida]
        public DateTime  FechaCreacion {get;set;}

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [FechaValida]
        public DateTime  FechaEntrega {get;set;}

        [Required(ErrorMessage = "El campo {0} es requerido")]
        [FechaValida]
        public DateTime  FechaPostergacion {get;set;}

        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string Estado {get;set;}

       
    }
}