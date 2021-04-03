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

            //Middleware k�sm�na �yelik sistemini ekledik ve bu sistemin JWT ile korunaca��n� belirttik
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(opt =>
            {
                opt.RequireHttpsMetadata = false; //HTTPS zorunlulu�unu kald�rd�k, asl�nda �nerilmez
                opt.TokenValidationParameters = new TokenValidationParameters //Tokenin temel �zelliklerini belirledik
                {
                    ValidIssuer = "http://localhost", //Kim taraf�ndan olu�turuldu
                    ValidAudience = "http://localhost", //Kim kullanacak
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("eraybak�rberkaybak�r")), //Token do�rulamas� i�in bize �zel bir key belirledik
                    ValidateIssuerSigningKey=true,//Key kontrol�n�n yap�laca��n� belirtiyorum
                    ValidateLifetime=true, //Token i�in Expire Time kontrol� yap�lmas� gerekti�ini belirtiyoruz
                    ClockSkew=TimeSpan.Zero //Zaman farkl�l���n� yok sayd�k
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
