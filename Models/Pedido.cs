using System.ComponentModel.DataAnnotations;
using TiendavirtualArepasSafaera.Models;

public class Pedido
{
    public int Id { get; set; }
    public decimal Total { get; set; }
    public DateTime Fecha { get; set; }

    // Llave foránea
    public int UsuarioId { get; set; }
    // Propiedad de navegación
    public Usuario Usuario { get; set; }
}