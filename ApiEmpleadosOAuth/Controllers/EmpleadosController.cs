using ApiEmpleadosOAuth.Models;
using ApiEmpleadosOAuth.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ApiEmpleadosOAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmpleadosController : ControllerBase
    {
        private RepositoryEmpleados repo;
        public EmpleadosController (RepositoryEmpleados repo)
        {
            this.repo = repo;
        }
        [Authorize]
        [HttpGet]
        public ActionResult<List<Empleado>> GetEmpleados()
        {
            return this.repo.GetEmpleados();
        }
        [Authorize]
        [HttpGet("{id}")]
        public ActionResult<Empleado> GetDoctor(int id)
        {
            return this.repo.GetEmpleado(id);
        }

        [HttpGet]
        [Route("[action]")]
        [Authorize]
        public ActionResult<Empleado> PerfilEmpleado()
        {
            // UNA VEZ QUE NOS HEMOS VALIDADO CON EL TOKEN ESTAMOS AQUI Y EN NUESTRO API Y TAMBIEN ESTAMOS VALIDADOS
            //DEBEMOS RECUPERAR EL CLAIM DE USERDATA DE LOS CLAIMS DEL USUARIO DE LA APP
            List<Claim> claims = HttpContext.User.Claims.ToList();
            //BUSCAMOS EL JSON DEL EMPLEADO GUARDADO CON LA KEY USERDATA
            string jsonempleado = claims.SingleOrDefault(x => x.Type == "UserData").Value;
            Empleado emp = JsonConvert.DeserializeObject<Empleado>(jsonempleado);
            return emp;
        }

    }
}
