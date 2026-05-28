using System.ComponentModel.DataAnnotations;
namespace TiendavirtualArepasSafaera.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public double Precio { get; set; }
        public int Stock { get; set; }
        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
        public string? ImagenUrl { get; set; }

        // ✅ AGREGADO - opcional
        public string? Descripcion { get; set; }

        public double CalcularValorInventario()
        {
            return Precio * Stock;
        }
    }
}