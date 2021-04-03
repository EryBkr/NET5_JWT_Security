using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JWT_Example
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddControllers();

            //Middleware kýsmýna üyelik sistemini ekledik ve bu sistemin JWT ile korunacaðýný belirttik
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false; //HTTPS zorunluluðunu kaldýrdýk, aslýnda önerilmez
                opt.TokenValidationParameters = new TokenValidationParameters //Tokenin temel özelliklerini belirledik
                {
                    ValidIssuer = "http://localhost", //Kim tarafýndan oluþturuldu
                    ValidAudience = "http://localhost", //Kim kullanacak
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("eraybakýrberkaybakýr")), //Token doðrulamasý için bize özel bir key belirledik
                    ValidateIssuerSigningKey=true,//Key kontrolünün yapýlacaðýný belirtiyorum
                    ValidateLifetime=true, //Token için Expire Time kontrolü yapýlmasý gerektiðini belirtiyoruz
                    ClockSkew=TimeSpan.Zero //Zaman farklýlýðýný yok saydýk
                };
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();
           

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
