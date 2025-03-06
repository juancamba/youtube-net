namespace validacion.example.Controllers
{
    public class RegisterRequest
    {
        public string? Nombre { get; set; }
        public string? Email { get; set; }
        public DateOnly FechaNacimiento { get; set; }
        public string? Password { get; set; }
        public string? ConfirmPassword { get; set; }
    }
}