using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PerfilesSA.Modelos
{
    public class Empleado
    {
        public int IdEmpleado { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres")]
        public string Nombre { get; set; }

        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(50, ErrorMessage = "El apellido no puede exceder los 50 caracteres")]
        public string Apellido { get; set; }

        [Required(ErrorMessage = "El DPI es requerido")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El DPI debe tener exactamente 13 dígitos")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "El DPI debe contener exactamente 13 números, sin letras ni caracteres especiales")]
        [Display(Name = "DPI")]
        public string DPI { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        public DateTime FechaNacimiento { get; set; }

        [Required(ErrorMessage = "El género es requerido")]
        [StringLength(1)]
        public string Genero { get; set; }

        [Required(ErrorMessage = "La fecha de ingreso es requerida")]
        public DateTime FechaIngreso { get; set; }

        [StringLength(200)]
        public string Direccion { get; set; }

        [StringLength(20)]
        [RegularExpression(@"^\d{-}?\d{-}?$", ErrorMessage = "Formato de NIT inválido")]
        public string NIT { get; set; }

        [Required(ErrorMessage = "El departamento es requerido")]
        public int IdDepartamento { get; set; }

        // Propiedades calculadas
        public int Edad => CalcularEdad(FechaNacimiento);
        public int AniosServicio => CalcularAniosServicio(FechaIngreso);

        // Métodos privados para cálculos
        private int CalcularEdad(DateTime fechaNacimiento)
        {
            var hoy = DateTime.Today;
            var edad = hoy.Year - fechaNacimiento.Year;
            if (fechaNacimiento > hoy.AddYears(-edad))
                edad--;
            return edad;
        }

        private int CalcularAniosServicio(DateTime fechaIngreso)
        {
            var hoy = DateTime.Today;
            var anios = hoy.Year - fechaIngreso.Year;
            if (fechaIngreso > hoy.AddYears(-anios))
                anios--;
            return anios;
        }

        public virtual Departamento Departamento { get; set; }

        public string NombreDepartamento { get; set; }

        public bool EstaActivo { get; set; }

        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }

        // Método de validación personalizado para el DPI
        public bool EsDPIValido()
        {
            if (string.IsNullOrEmpty(DPI)) return false;
            if (DPI.Length != 13) return false;
            
            // Verificar que todos los caracteres sean dígitos
            foreach (char c in DPI)
            {
                if (!char.IsDigit(c)) return false;
            }
            return true;
        }
    }
} 