using System;
using System.ComponentModel.DataAnnotations;
using ApiKalumNotas.Helpers;

namespace ApiKalumNotas.DTOs
{
    public class SeminarioDTO
    {
        public string SeminarioId {get;set;}
       
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string ModuloId {get;set;}
       
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string NombreSeminario {get;set;}
       
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [FechaValida]
        public DateTime  FechaInicio {get;set;}
       
        [Required(ErrorMessage = "El campo {0} es requerido")]

        [FechaValida]                      
        public DateTime  FechaFinal {get;set;}
    }
}