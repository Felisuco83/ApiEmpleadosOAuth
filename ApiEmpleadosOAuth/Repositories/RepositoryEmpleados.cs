using ApiEmpleadosOAuth.Data;
using ApiEmpleadosOAuth.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiEmpleadosOAuth.Repositories
{
    public class RepositoryEmpleados
    {
        private EmpleadosContext context;
        public RepositoryEmpleados(EmpleadosContext context)
        {
            this.context = context;
        }

        public List<Empleado> GetEmpleados(){
            return this.context.Empleados.ToList();
        }

        public Empleado GetEmpleado( int id )
        {
            return this.context.Empleados.SingleOrDefault(x => x.IdEmpleado == id);
        }

        public Empleado ExisteEmpleado (string apellido, int id)
        {
            return this.context.Empleados.SingleOrDefault(x => x.Apellido == apellido && x.IdEmpleado == id);
        }

        public List<Empleado> GetSubordinados (int id)
        {
            return this.context.Empleados.Where(x => x.Director == id).ToList();
        }
    }
}
