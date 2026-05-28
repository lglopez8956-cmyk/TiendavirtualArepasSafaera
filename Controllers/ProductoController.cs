using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json;
using TiendavirtualArepasSafaera.Data;
using TiendavirtualArepasSafaera.Models;

namespace TiendavirtualArepasSafaera.Controllers
{
    public class ProductoController : Controller
    {
        private readonly TiendaContext _context;

        public ProductoController(TiendaContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Usuario") == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var productos = _context.Productos
                .Include(p => p.Categoria)
                .ToList();

            return View(productos);
        }

        // CREATE Producto (GET)
        public IActionResult Create()
        {
            var rol = HttpContext.Session.GetString("Rol")?.Trim();
            if (!string.Equals(rol, "Administrador", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index", "Producto");
            }

            // ✅ ESTO ES LO QUE FALTA - carga las categorías para el dropdown
            ViewBag.Categorias = _context.Categorias.ToList();

            return View();
        }
     
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto, IFormFile imagen)
        {
            var rol = HttpContext.Session.GetString("Rol")?.Trim();
            if (!string.Equals(rol, "Administrador", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index");
            }

            // Manejo de imagen
            if (imagen != null)
            {
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
                var ruta = Path.Combine(carpeta, imagen.FileName);
                using (var stream = new FileStream(ruta, FileMode.Create))
                {
                    await imagen.CopyToAsync(stream);
                }
                producto.ImagenUrl = "/images/" + imagen.FileName;
            }

            _context.Productos.Add(producto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Edit(int id)
        {
            if (HttpContext.Session.GetString("Rol") != "Administrador")
            {
                return RedirectToAction("Index", "Login");
            }

            var producto = _context.Productos.Find(id);
            if (producto == null) return NotFound();

            ViewBag.Categorias = _context.Categorias.ToList();
            return View(producto);
        }

        [HttpPost]
        public IActionResult Edit(Producto producto, IFormFile imagen)
        {
            var productoBD = _context.Productos.Find(producto.Id);
            if (productoBD == null) return NotFound();

            productoBD.Nombre = producto.Nombre;
            productoBD.Precio = producto.Precio;
            productoBD.Stock = producto.Stock;
            productoBD.CategoriaId = producto.CategoriaId;

            if (imagen != null)
            {
                var carpeta = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
                var ruta = Path.Combine(carpeta, imagen.FileName);
                using (var stream = new FileStream(ruta, FileMode.Create)) imagen.CopyTo(stream);
                productoBD.ImagenUrl = "/images/" + imagen.FileName;
            }

            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public IActionResult Carrito()
        {
            var carritoJson = HttpContext.Session.GetString("Carrito");
            List<CarritoItem> carrito = carritoJson == null ? new List<CarritoItem>() : JsonSerializer.Deserialize<List<CarritoItem>>(carritoJson);

            var productos = new List<(Producto producto, int cantidad)>();
            foreach (var item in carrito)
            {
                var producto = _context.Productos.Find(item.ProductoId);
                if (producto != null) productos.Add((producto, item.Cantidad));
            }
            return View(productos);
        }

        [HttpPost]
        public IActionResult Comprar()
        {
            var carritoJson = HttpContext.Session.GetString("Carrito");
            // Si está vacío, devolvemos un error para que JS lo maneje
            if (string.IsNullOrEmpty(carritoJson)) return BadRequest("Carrito vacío");

            var carrito = System.Text.Json.JsonSerializer.Deserialize<List<CarritoItem>>(carritoJson);
            var usuarioIdString = HttpContext.Session.GetString("UsuarioId");
            int usuarioId = string.IsNullOrEmpty(usuarioIdString) ? 0 : int.Parse(usuarioIdString);

            var nuevoPedido = new Pedido
            {
                UsuarioId = usuarioId,
                Fecha = DateTime.Now,
                Total = carrito.Sum(x => x.Precio * x.Cantidad)
            };

            _context.Pedidos.Add(nuevoPedido);
            _context.SaveChanges();

            HttpContext.Session.Remove("Carrito");

            
            return Ok();
        }

        [HttpPost]
        public IActionResult AgregarCarrito(int id, int cantidad)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null || producto.Stock < cantidad)
            {
                TempData["Error"] = "Stock insuficiente";
                return RedirectToAction("Index");
            }

            // 1️⃣ RESTAMOS EL STOCK EN LA BASE DE DATOS
            producto.Stock -= cantidad;
            _context.SaveChanges();

            var carritoJson = HttpContext.Session.GetString("Carrito");
            List<CarritoItem> carrito = carritoJson == null ? new List<CarritoItem>() : JsonSerializer.Deserialize<List<CarritoItem>>(carritoJson);

            var item = carrito.FirstOrDefault(p => p.ProductoId == id);

            if (item != null)
            {
                item.Cantidad += cantidad;
            }
            else
            {
                carrito.Add(new CarritoItem
                {
                    ProductoId = id,
                    Cantidad = cantidad,
                    Precio = (decimal)producto.Precio
                });
            }

            HttpContext.Session.SetString("Carrito", JsonSerializer.Serialize(carrito));
            TempData["Mensaje"] = "Producto agregado al carrito";
            return RedirectToAction("Index");
        }

        public IActionResult HistorialCompras()
        {
            if (HttpContext.Session.GetString("Rol") != "Administrador")
                return RedirectToAction("Index", "Home");

            var historial = _context.Pedidos.Include(p => p.Usuario).ToList();
            return View(historial);
        }

        public IActionResult Delete(int id)
        {
            if (HttpContext.Session.GetString("Rol") != "Administrador") return RedirectToAction("Index", "Login");

            var producto = _context.Productos.Find(id);
            if (producto != null)
            {
                _context.Productos.Remove(producto);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}