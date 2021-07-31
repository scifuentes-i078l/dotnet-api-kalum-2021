using ApiKalumNotas.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiKalumNotas.DbContexts
{

    
    public class KalumNotasDBContext : DbContext 

    {

        public DbSet<AsignacionAlumno> AsignacionesAlumnos {get;set;}
        public DbSet<Alumno> Alumnos {get;set;}
        public DbSet<Clase> Clases {get;set;}

        public DbSet<Modulo> Modulos {get;set;}
        public DbSet<CarreraTecnica> Carreras {get;set;}

        public DbSet<Seminario> Seminarios {get;set;}

        public DbSet<DetalleActividad> DetalleActividades {get;set;}

        public DbSet<DetalleNota> DetalleNotas {get;set;}


        public KalumNotasDBContext(DbContextOptions<KalumNotasDBContext> options) : base (options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

             modelBuilder.Entity<Alumno>()
            .ToTable(name: "Alumnos")
            .HasKey(c => new {c.Carne});
            modelBuilder.Entity<Instructor>()
            .ToTable(name: "Instructores")
            .HasKey(f => new {f.InstructorId});
            modelBuilder.Entity<Clase>()
            .ToTable(name: "Clases")
            .HasKey(g => new {g.ClaseId});

            modelBuilder.Entity<Horario>()
            .ToTable(name: "Horarios")
            .HasKey(h => new {h.HorarioId});
            modelBuilder.Entity<Salon>()
            .ToTable(name: "Salones")
            .HasKey(b => new {b.SalonId});
            
            modelBuilder.Entity<CarreraTecnica>()
            .ToTable(name: "CarrerasTecnicas")
            .HasKey(e => new {e.CarreraId});

    
            modelBuilder.Entity<Clase>()
            .HasOne<CarreraTecnica>(c => c.CarreraTecnica)
            .WithMany(ct => ct.Clases)
            .HasForeignKey(c => c.CarreraId);

            modelBuilder.Entity<AsignacionAlumno>()
            .ToTable("AsignacionesAlumnos")
            .HasKey(aa => aa.AsignacionId);

            modelBuilder.Entity<AsignacionAlumno>()
            .HasOne<Alumno> (aa => aa.Alumno)
            .WithMany(a => a.Asignaciones)
            .HasForeignKey(aa => aa.Carne);        


            modelBuilder.Entity<Modulo>()
            .ToTable("Modulos")
            .HasKey(m=> m.ModuloID);

            modelBuilder.Entity<Modulo>()
            .HasOne<CarreraTecnica>( m => m.CarreraTecnica)
            .WithMany(c=> c.Modulos)
            .HasForeignKey(m => m.CarreraID);


            modelBuilder.Entity<Seminario>()
            .ToTable(name: "Seminarios")
            .HasKey(s => new {s.SeminarioId});

            modelBuilder.Entity<Seminario>()
            .HasOne<Modulo>(s => s.Modulo)
            .WithMany(m => m.Seminarios)
            .HasForeignKey( s => s.ModuloId);

            modelBuilder.Entity<DetalleActividad>()
            .ToTable(name: "DetalleActividades")
            .HasKey(da => new {da.DetalleActividadId});

            modelBuilder.Entity<DetalleActividad>()
            .HasOne<Seminario>(da => da.Seminario)
            .WithMany(s => s.DetalleActividades)
            .HasForeignKey(da => da.SeminarioId);

            modelBuilder.Entity<DetalleNota>()
            .ToTable(name: "DetalleNotas")
            .HasKey(dn => new {dn.DetalleNotaId});

            modelBuilder.Entity<DetalleNota>()
            .HasOne<DetalleActividad>(dn => dn.DetalleActividad)
            .WithMany(da => da.DetalleNotas)
            .HasForeignKey(dn => dn.DetalleActividadId);

            modelBuilder.Entity<DetalleNota>()
            .HasOne<Alumno>(dn => dn.Alumno)
            .WithMany(da => da.DetalleNotas)
            .HasForeignKey(dn => dn.Carne);



            
        }
    }
}