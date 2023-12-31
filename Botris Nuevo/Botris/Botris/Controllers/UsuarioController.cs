﻿using Botris.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using NuGet.Protocol.Plugins;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Botris.Controllers
{
    [ApiController]
    [Route("usuario")]
    public class UsuarioController:ControllerBase
    {
        private readonly AplicationDbContext _context;
        private IConfiguration _configuration;
        public UsuarioController(AplicationDbContext context, IConfiguration configuration )
        {
            _context = context;
            _configuration = configuration;
        }
        //[HttpPost]
        //[Route("Login")]
        //public dynamic IniciarSesion([FromBody] Object optData)
        //{
        //    var data =JsonConvert.DeserializeObject<dynamic>(optData.ToString());
        //    string usuario=data.user.ToString();
        //    string clave = data.password.ToString();

        //    var ingreso = _context.Usuarios.Where(x => x.User == usuario && x.Password == clave).FirstOrDefault();

        //    if (ingreso ==null)
        //    {
        //        return new
        //        {
        //            success = false,
        //            message = "Credenciales Incorrectas",
        //            result = ""
        //        };
        //    }
        //    var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
        //    var clains = new[]
        //    {
        //        new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
        //        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
        //        new Claim("Guid", ingreso.Guid.ToString()),
        //        new Claim("User",ingreso.User)
        //    };
        //    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
        //    var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        //    var tokken = new JwtSecurityToken(
        //         jwt.Issuer,
        //         jwt.Audience,
        //         clains,
        //         expires: DateTime.Now.AddMinutes(4),//borramos si no queremos que se cierre
        //         signingCredentials: singIn

        //        );
        //    return new
        //    {
        //        succss = true,
        //        message = "exito",
        //        result = new JwtSecurityTokenHandler().WriteToken(tokken)
        //    };
        //}
        [HttpPost]
        [EnableCors("AllowWebapp")]
        [Route("Login")]
        public async Task<IActionResult> IniciarSesion([FromBody] Usuario userObj)
        {
            if (userObj == null) return BadRequest();//ERROR 404
            var user= await _context.Usuarios.FirstOrDefaultAsync(x=> x.User==userObj.User && x.Password==userObj.Password);
            if(user == null) return NotFound(new {success=false, message="Usuario Incorrecto", result=""});
            return Ok(new {success=true, message="Ingreso Exitoso" });
        }
        [HttpPost]
        [EnableCors("AllowWebapp")]
        [Route("Registro")]
        public async Task<IActionResult> Registrar([FromBody] Usuario userObj)
        {
            if(userObj== null) return BadRequest(new {success=false, message="Ingrese Todos los campos" });
            await _context.Usuarios.AddAsync(userObj);
            await _context.SaveChangesAsync();
            return Ok(new { success = true, message = "Ingreso Exitoso" });

        }

    }
}
  