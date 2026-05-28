using System.Security.Cryptography; // 🚨 Obligatorio para usar SHA256
using System.Text;                  // 🚨 Obligatorio para el Encoding

namespace TiendavirtualArepasSafaera.Helpers
{
    public class HashHelper 
    {
        
        public static string ObtenerHash(string texto) 
        {
            using (SHA256 sha256 = SHA256.Create()) // Inicializa el algoritmo criptográfico
            {
                // Convierte el texto de la contraseña en una matriz de bytes
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(texto)); //

                StringBuilder builder = new StringBuilder(); //

                // Recorre cada byte y lo convierte a formato hexadecimal
                foreach (var b in bytes) //
                {
                    builder.Append(b.ToString("x2")); // "x2" garantiza que tenga dos caracteres por byte
                }

                return builder.ToString(); // Devuelve la cadena encriptada final
            }
        }
    }
}