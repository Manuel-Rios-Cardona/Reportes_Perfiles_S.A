using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace PerfilesSA.Models
{
    public class Employee
    {
        public int EmployeeId { get; set; }

        [Required(ErrorMessage = "El nombre es requerido")]
        [StringLength(50, ErrorMessage = "El nombre no puede exceder los 50 caracteres")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "El apellido es requerido")]
        [StringLength(50, ErrorMessage = "El apellido no puede exceder los 50 caracteres")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "El DPI es requerido")]
        [StringLength(13, MinimumLength = 13, ErrorMessage = "El DPI debe tener exactamente 13 dígitos")]
        [RegularExpression(@"^\d{13}$", ErrorMessage = "El DPI debe contener exactamente 13 números, sin letras ni caracteres especiales")]
        [Display(Name = "DPI")]
        public string DPI { get; set; }

        [Required(ErrorMessage = "La fecha de nacimiento es requerida")]
        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "El género es requerido")]
        [StringLength(1)]
        public string Gender { get; set; }

        [Required(ErrorMessage = "La fecha de ingreso es requerida")]
        public DateTime HireDate { get; set; }

        [StringLength(200)]
        public string Address { get; set; }

        [StringLength(20)]
        [RegularExpression(@"^\d{-}?\d{-}?$", ErrorMessage = "Formato de NIT inválido")]
        public string NIT { get; set; }

        [Required(ErrorMessage = "El departamento es requerido")]
        public int DepartmentId { get; set; }

        // Propiedades calculadas
        public int Age => CalculateAge(BirthDate);
        public int YearsOfService => CalculateYearsOfService(HireDate);

        // Métodos privados para cálculos
        private int CalculateAge(DateTime birthDate)
        {
            var today = DateTime.Today;
            var age = today.Year - birthDate.Year;
            if (birthDate > today.AddYears(-age))
                age--;
            return age;
        }

        private int CalculateYearsOfService(DateTime hireDate)
        {
            var today = DateTime.Today;
            var years = today.Year - hireDate.Year;
            if (hireDate > today.AddYears(-years))
                years--;
            return years;
        }

        public virtual Department Department { get; set; }

        public string DepartmentName { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        // Método de validación personalizado para el DPI
        public bool IsValidDPI()
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