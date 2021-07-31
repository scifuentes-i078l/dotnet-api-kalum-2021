using System.ComponentModel.DataAnnotations;
using ApiKalumNotas.Helpers;

namespace ApiKalumNotas.DTOs
{
    public class DetalleNotaDTO
    {
        
        public string  DetalleNotaId {get;set;}
         [Required(ErrorMessage = "El campo {0} es requerido")]
        public string DetalleActividadId {get;set;}    
        [Required(ErrorMessage = "El campo {0} es requerido")]
        [Carne]
        public string Carne {get;set;} 
        [Required(ErrorMessage = "El campo {0} es requerido")]
        //[ValorNota]       
        [Range(0, 100, ErrorMessage = "El valor para {0} debe estar entre  {1}  y {2}.")]
        public int ValorNota {get;set;}

    }
}