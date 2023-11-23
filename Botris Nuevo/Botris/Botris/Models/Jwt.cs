using Botris.Controllers;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Security.Claims;

namespace Botris.Models
{
    public class Jwt
    {

      
        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }

        public string Subject { get; set; }

        public static dynamic ValidarToken(ClaimsIdentity identity, AplicationDbContext context)
        {
            try
            {
                // Crear las opciones para el contexto
               
                if (identity.Claims.Count() == 0)
                {
                    return new
                    {
                        success = false,
                        messagge = "Verificar si esta logeado",
                        result = ""
                    };
                }
                var id= identity.Claims.FirstOrDefault(x=> x.Type=="id").Value;
                var ingreso = context.Usuarios.FirstOrDefault(x => x.id == Guid.Parse( id));
                return new
                {
                    success = true,
                    messagge = "exito",
                    result = ingreso
                };
            }
            catch (Exception e)
            {

                return new
                {
                    success =false,
                    messagge= "Catch: "+ e.Message,
                    result=""
                };
            }
        }
    }
    }
