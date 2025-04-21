using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PerfilesSA.Modelos
{
    public class Departamento
    {
        public int IdDepartamento { get; set; }

        [Required(ErrorMessage = "El nombre del departamento es requerido")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres")]
        public string Nombre { get; set; }

        [StringLength(200)]
        public string Descripcion { get; set; }

        public bool EstaActivo { get; set; } = true;

        public DateTime FechaCreacion { get; set; }

        public DateTime? FechaModificacion { get; set; }

        public virtual ICollection<Empleado> Empleados { get; set; }

        public Departamento()
        {
            Empleados = new HashSet<Empleado>();
            FechaCreacion = DateTime.Now;
            EstaActivo = true;
        }
    }
} 