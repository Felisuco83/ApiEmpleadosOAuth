using ApiEmpleadosOAuth.Helpers;
using ApiEmpleadosOAuth.Models;
using ApiEmpleadosOAuth.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiEmpleadosOAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        RepositoryEmpleados repo;
        HelperToken helpertoken;

        public AuthController(RepositoryEmpleados repo, HelperToken helpertoken)
        {
            this.repo = repo;
            this.helpertoken = helpertoken;
        }

        [HttpPost]
        [Route("[Action]")]
        public IActionResult Login (LoginModel model)
        {
            Empleado empleado = this.repo.ExisteEmpleado(model.UserName, int.Parse(model.Password));
            if(empleado == null)
            {
                return Unauthorized();
            } else
            {
                //UTILIZAREMOS CLAIMS PARA ALMACENAR DATOS POR KEY Y VALUE, VAMOS A ALMACENAR EL OBJETO EMPLEADO EN EL TOKEN
                string empleadojson = JsonConvert.SerializeObject(empleado);
                Claim[] claims = new[]
                {
                    new Claim("UserData", empleadojson)
                };
                JwtSecurityToken token = new JwtSecurityToken(
                    issuer: this.helpertoken.Issuer,
                    audience: this.helpertoken.Audience,
                    claims: claims,
                    expires: DateTime.UtcNow.AddMinutes(10),
                    notBefore: DateTime.UtcNow,
                    signingCredentials: new SigningCredentials(this.helpertoken.GetKeyToken(), SecurityAlgorithms.HmacSha256)
                    );
                return Ok(new { response = new JwtSecurityTokenHandler().WriteToken(token) });
            }
        }
    }
}
