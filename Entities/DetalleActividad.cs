using System;
using System.Collections.Generic;

namespace ApiKalumNotas.Entities
{
    public class DetalleActividad
    {
        public string DetalleActividadId {get;set;}
        
        public string SeminarioId {get;set;}

        public string NombreActividad {get;set;}

        public int NotaActividad {get;set;}
        
        public DateTime  FechaCreacion {get;set;}
        public DateTime  FechaEntrega {get;set;}

        public DateTime  FechaPostergacion {get;set;}

        public string Estado {get;set;}

        public virtual Seminario Seminario {get;set;}

        public virtual List<DetalleNota> DetalleNotas {get;set;}
           
    }
}