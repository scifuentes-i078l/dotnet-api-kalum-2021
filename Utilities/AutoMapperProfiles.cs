using ApiKalumNotas.DTOs;
using ApiKalumNotas.Entities;
using AutoMapper;

namespace ApiKalumNotas.Utilities
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AsignacionAlumno, AsignacionAlumnoDTO>();
            CreateMap<AsignacionAlumno, AsignacionAlumnoDetalleDTO>();
            CreateMap<AsignacionAlumnoDTO,AsignacionAlumno>();
             CreateMap<Alumno, AlumnoAsignacionDTO>().ConstructUsing(a => new AlumnoAsignacionDTO{NombreCompleto = $"{a.Apellidos} {a.Nombres}"});
             CreateMap<Clase, ClaseAsignacionDTO>();
             CreateMap<DetalleNota, DetalleNotaDTO>();
             CreateMap<DetalleNotaDTO,DetalleNota>();
             CreateMap<Modulo,ModuloDTO>();
             CreateMap<ModuloDTO,Modulo>();
             CreateMap<Seminario,SeminarioDTO>();
             CreateMap<SeminarioDTO,Seminario>();
             CreateMap<DetalleActividad,DetalleActividadDTO>();
             CreateMap<DetalleActividadDTO,DetalleActividad>();
             CreateMap<Alumno,AlumnoDTO>();
             CreateMap<AlumnoDTO,Alumno>();
             CreateMap<DetalleNota, DetalleNotaDetalleDTO>();

        }
        
    }
}