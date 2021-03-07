using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiEmpleadosOAuth.Helpers
{
    public class HelperToken
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string SecretKey { get; set; }

        public HelperToken(IConfiguration configuration)
        {
            this.Issuer = configuration["ApiOAuth:Issuer"];
            this.Audience = configuration["ApiOAuth:Audience"];
            this.SecretKey = configuration["ApiOAuth:SecretKey"];
        }

        //NECESITAMOS UN METODO PARA GENERAR UNA CLAVE TOKEN A PARTIR DE NUESTRO SECRETKEY
        public SymmetricSecurityKey GetKeyToken()
        {
            byte[] data = Encoding.UTF8.GetBytes(this.SecretKey);
            return new SymmetricSecurityKey(data);
        }
        //METODO PARA LA CONFIGURACION DEL TOKEN DE SEGURIDAD QUE SE GENERARA

        public Action<JwtBearerOptions> GetJwtBearerOptions() {
            Action<JwtBearerOptions> jwtoptions = new Action<JwtBearerOptions>(options =>
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidateActor = true, ValidateAudience = true, ValidateLifetime = true, ValidateIssuerSigningKey = true,
                    ValidIssuer = this.Issuer, ValidAudience= this.Audience, IssuerSigningKey = this.GetKeyToken()
                }
            );
            return jwtoptions;
        }
        //METODO ACTION PARA LAS OPCIONES DE AUTHENTICATION
        public Action<AuthenticationOptions> GetAuthOptions()
        {
            Action<AuthenticationOptions> authoptions = new Action<AuthenticationOptions>(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            });
            return authoptions;
        }
    }
}
