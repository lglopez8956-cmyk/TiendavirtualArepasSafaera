namespace TiendavirtualArepasSafaera.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }

        // ✅ El ? lo hace opcional - el formulario no necesita enviarlo
        public ICollection<Producto>? Productos { get; set; }
    }
}