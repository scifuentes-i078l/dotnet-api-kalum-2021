using System;
using System.ComponentModel.DataAnnotations;
using ApiKalumNotas.Helpers;

namespace ApiKalumNotas.DTOs
{
    public class AsignacionAlumnoDetalleDTO
    {
        public string AsignacionId {get;set;}        
        public DateTime FechaAsignacion {get;set;}
        public AlumnoAsignacionDTO alumno; //{get;set;}
        public ClaseAsignacionDTO clase;// {get;set;}

    }
}