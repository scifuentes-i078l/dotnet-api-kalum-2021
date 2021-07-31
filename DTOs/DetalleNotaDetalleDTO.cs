using System.ComponentModel.DataAnnotations;
using ApiKalumNotas.Helpers;

namespace ApiKalumNotas.DTOs
{
    public class DetalleNotaDetalleDTO
    {
        
        public string  DetalleNotaId {get;set;}
         [Required(ErrorMessage = "El campo {0} es requerido")]
      
        [Range(0, 100, ErrorMessage = "El valor para {0} debe estar entre  {1}  y {2}.")]
        public int ValorNota {get;set;}

          
        public DetalleActividadDTO DetalleActividad {get;set;}

        public AlumnoDTO Alumno {get;set;}

    }
}