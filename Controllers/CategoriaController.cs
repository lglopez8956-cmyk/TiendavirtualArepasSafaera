using Microsoft.AspNetCore.Http; // 🚨 OBLIGATORIO para que funcione HttpContext.Session
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TiendavirtualArepasSafaera.Data;
using TiendavirtualArepasSafaera.Models;

namespace TiendavirtualArepasSafaera.Controllers
{
    public class CategoriaController : Controller
    {
        private readonly TiendaContext _context;

        public CategoriaController(TiendaContext context)
        {
            _context = context;
        }

        // 1. LISTAR CATEGORÍAS
        public IActionResult Index()
        {
            
            var categorias = _context.Categorias.Include(c => c.Productos).ToList();

            return View(categorias);
        }

        // 2. CREAR CATEGORÍA (GET)
        public IActionResult Create()
        {
            // Protección al inicio del método
            if (HttpContext.Session.GetString("Rol") != "Administrador")
            {
                return RedirectToAction("Index", "Categoria");
            }

            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            var rol = HttpContext.Session.GetString("Rol")?.Trim();

            
            if (!string.Equals(rol, "Administrador", StringComparison.OrdinalIgnoreCase))
            {
                return RedirectToAction("Index");
            }

            if (categoria != null)
            {
                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(categoria);
        }

        // 3. EDITAR CATEGORÍA (GET)
        public IActionResult Edit(int id)
        {
            // Protección al inicio del método
            if (HttpContext.Session.GetString("Rol") != "Administrador")
            {
                TempData["Error"] = "No tienes permisos para realizar esta acción.";
                return RedirectToAction("Index");
            }

            var categoria = _context.Categorias.Find(id);
            if (categoria == null) return NotFound();
            return View(categoria);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Categoria categoria)
        {
            if (HttpContext.Session.GetString("Rol") != "Administrador")
            {
                return RedirectToAction("Index", "Login");
            }

          
            if (!ModelState.IsValid)
            {
                var errores = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .Select(x => $"{x.Key}: {string.Join(", ", x.Value.Errors.Select(e => e.ErrorMessage))}");

                TempData["Error"] = string.Join(" | ", errores);
                return View(categoria);
            }

            _context.Categorias.Update(categoria);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        // 4. ELIMINAR CATEGORÍA
        public IActionResult Delete(int id)
        {
            // Protección al inicio del método
            if (HttpContext.Session.GetString("Rol") != "Administrador")
            {
                return RedirectToAction("Index", "Login"); 
            }

            var categoria = _context.Categorias.Find(id);
            if (categoria != null)
            {
                _context.Categorias.Remove(categoria);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}