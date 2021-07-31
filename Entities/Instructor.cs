using System.Collections.Generic;


namespace ApiKalumNotas.Entities
{

   
        
    public class Instructor
    {

    public string InstructorId {get; set;}
    public string  Nombres {get; set;}
    public string Apellidos {get; set;}
    public string  Comentario {get; set;}
    public string  Direccion {get; set;}
    public string  Estatus {get; set;}
    public string  Foto {get; set;}
    public string Telefono {get; set;}

    public virtual List<Clase> Clases {get;set;}

    public Instructor(string InstructorId, string  Nombres, string Apellidos,string  Comentario,string  Direccion, string  Estatus,string  Foto,string Telefono)
    {
        this.InstructorId=InstructorId;
        this.Nombres=Nombres;
        this.Apellidos=Apellidos;
        this.Comentario=Comentario;
        this.Direccion=Direccion;
        this.Estatus=Estatus;
        this.Foto=Foto;
        this.Telefono=Telefono;
    }

    public Instructor()
    {
        
    }

        public override string ToString()
        {
            return $"{this.Apellidos} {this.Nombres}";
        }

        
    }
}