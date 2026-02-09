using System;
using System.ComponentModel.DataAnnotations;

namespace Myper.Trabajadores.Web.Models
{
    public class Trabajador
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Nombres { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Apellidos { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Display(Name = "Tipo de documento")]
        public string TipoDocumento { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Display(Name = "Número de documento")]
        public string NumeroDocumento { get; set; } = string.Empty;

        [Required]
        public char Sexo { get; set; } // 'M' o 'F'

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime FechaNacimiento { get; set; }

        public string? Foto { get; set; }

        [StringLength(200)]
        public string? Direccion { get; set; }

        public DateTime FechaRegistro { get; set; }

        public bool Activo { get; set; } = true;
    }
}