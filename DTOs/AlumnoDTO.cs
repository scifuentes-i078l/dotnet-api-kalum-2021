using System.ComponentModel.DataAnnotations;
using ApiKalumNotas.Helpers;

namespace ApiKalumNotas.DTOs
{
    public class AlumnoDTO
    {
        [Required]
        [Carne]
        public string Carne {get;set;}
        [Required]
        public string NoExpediente {get;set;}
        [Required]
        public string Apellidos {get;set;}
        [Required]
        public string Nombres {get;set;}
        [Required]
        [EmailAddress]
        public string Email {get;set;}

    }
}