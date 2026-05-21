using System.ComponentModel.DataAnnotations;

namespace TiendavirtualArepasSafaera.Models
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = null!;

        [Required(ErrorMessage = "El correo es obligatorio")]
        [EmailAddress(ErrorMessage = "Formato de correo inválido")]
        public string Correo { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Mínimo 4 caracteres")]
        public string Clave { get; set; } = null!;

        [Required(ErrorMessage = "El rol es obligatorio")]
        public string Rol { get; set; } = null!;

        [Required(ErrorMessage = "El teléfono celular es obligatorio")]
        [RegularExpression(@"^3\d{9}$", ErrorMessage = "El celular debe empezar con 3 y tener exactamente 10 dígitos")]
        public string Celular { get; set; } = null!;
    }
}