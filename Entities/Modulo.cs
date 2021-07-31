using System.Collections.Generic;

namespace ApiKalumNotas.Entities
{
    public class Modulo
    {
        public string ModuloID {get;set;}
        
        public string CarreraID {get;set;}

        public string NombreModulo {get;set;}
        
        public int  NumeroSeminarios {get;set;}

        public virtual CarreraTecnica CarreraTecnica {get;set;}

        public virtual List<Seminario> Seminarios {get;set;}
    }
}