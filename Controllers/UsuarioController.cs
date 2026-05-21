using Microsoft.AspNetCore.Mvc;
using TiendavirtualArepasSafaera.Data;
using TiendavirtualArepasSafaera.Models;
using TiendavirtualArepasSafaera.Helpers; 
using System.Linq;

namespace TiendavirtualArepasSafaera.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly TiendaContext _context;

        public UsuarioController(TiendaContext context)
        {
            _context = context;
        }

        // 1. LISTAR USUARIOS (Index)
        public IActionResult Index()
        {
            var usuarios = _context.Usuarios.ToList();
            return View(usuarios);
        }

        // 2. CREAR USUARIO (GET)
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Usuario usuario)
        {
            if (ModelState.IsValid)
            {
                _context.Usuarios.Add(usuario);
                _context.SaveChanges();

                // Si hay una sesión activa, el administrador está gestionando usuarios
                if (HttpContext.Session.GetString("Usuario") != null)
                {
                    return RedirectToAction("Index", "Usuario");
                }
                else
                {
                    // CORRECCIÓN: Apuntamos al controlador "Login" y la acción "Index"
                    // Esto evita el error 404 que tenías antes
                    return RedirectToAction("Index", "Login");
                }
            }
            return View(usuario);
        }

        // 3. EDITAR USUARIO (GET)
        public IActionResult Edit(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario == null)
            {
                return NotFound();
            }
            return View(usuario);
        }

        // EDITAR USUARIO (POST)
        [HttpPost]
        public IActionResult Edit(Usuario usuario)
        {
            // 1. Buscamos el usuario original en la base de datos
            var usuarioBD = _context.Usuarios.Find(usuario.Id);

            if (usuarioBD == null)
            {
                return NotFound();
            }

            // 2. Actualizamos solo los campos necesarios manualmente
            usuarioBD.Nombre = usuario.Nombre;
            usuarioBD.Correo = usuario.Correo;
            usuarioBD.Rol = usuario.Rol;
            usuarioBD.Celular = usuario.Celular;

            // 3. Guardamos los cambios
            try
            {
                _context.SaveChanges();
            }
            catch (Exception)
            {
                // Si hay un error, puedes imprimirlo o manejarlo
                ModelState.AddModelError("", "Error al guardar los cambios.");
                return View(usuario);
            }

            return RedirectToAction("Index"); // O a donde desees redirigir
        }

        // 4. ELIMINAR USUARIO
        public IActionResult Delete(int id)
        {
            var usuario = _context.Usuarios.Find(id);
            if (usuario != null)
            {
                _context.Usuarios.Remove(usuario);
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
    }
}