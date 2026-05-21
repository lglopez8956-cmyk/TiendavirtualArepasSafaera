using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // Necesario para usar las sesiones
using TiendavirtualArepasSafaera.Data;
using TiendavirtualArepasSafaera.Helpers;
using TiendavirtualArepasSafaera.Models;
using System.Linq;

namespace TiendavirtualArepasSafaera.Controllers
{
    public class LoginController : Controller
    {
        private readonly TiendaContext _context;

        public LoginController(TiendaContext context)
        {
            _context = context;
        }

        // 1. MOSTRAR EL FORMULARIO DE LOGIN (GET)
        public IActionResult Index()
        {
            return View();
        }

        // 2. PROCESAR EL INGRESO DE CREDENCIALES (POST)
        [HttpPost]
        public IActionResult Index(string correo, string clave)
        {
            // 1️⃣ Obtenemos el hash de la clave digitada
            string claveHash = HashHelper.ObtenerHash(clave);

            // 2️⃣ Buscamos al usuario solo por correo primero
            var usuario = _context.Usuarios.FirstOrDefault(u => u.Correo == correo);

            // 3️⃣ Verificamos si existe el usuario Y si la clave coincide (ya sea por Hash o Texto Plano)
            bool esValido = false;
            if (usuario != null)
            {
                // Verifica si la clave coincide con el hash O si coincide con la clave en texto plano
                if (usuario.Clave == claveHash || usuario.Clave == clave)
                {
                    esValido = true;
                }
            }

            if (esValido && usuario != null)
            {
                HttpContext.Session.SetString("Usuario", usuario.Nombre);
                HttpContext.Session.SetString("Rol", usuario.Rol);

                // Guardamos el ID del usuario
                HttpContext.Session.SetString("UsuarioId", usuario.Id.ToString());

                return RedirectToAction("Index", "Home");
            }

            // 4️⃣ Si falla, enviamos el mensaje de error a la vista
            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }

        // 3. CERRAR SESIÓN (LOGOUT)
        public IActionResult Logout()
        {
            // Solo limpia la sesión para cerrar el usuario
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }
    }
}