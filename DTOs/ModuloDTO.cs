using System.ComponentModel.DataAnnotations;
using ApiKalumNotas.Helpers;

namespace ApiKalumNotas.DTOs
{
    public class ModuloDTO
    {
        public string ModuloID {get;set;}
        
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string CarreraID {get;set;}
        
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string NombreModulo {get;set;}
        
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [NumeroSeminarios]
        public int  NumeroSeminarios {get;set;}

    }
}