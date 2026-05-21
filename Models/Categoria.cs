namespace TiendavirtualArepasSafaera.Models
{
    public class Categoria
    {
        public int Id { get; set; }
        public string Nombre { get; set; }

        // AGREGA ESTA LÍNEA PARA SOLUCIONAR EL ERROR:
        public string Descripcion { get; set; }

        public ICollection<Producto> Productos { get; set; }
    }
}