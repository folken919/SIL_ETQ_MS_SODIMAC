using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using ola_automatica.worker.core.Interfaces;
using sodimac.cedis.core.Utils;
using Sodimac.Cedis.Application.Services;
using Sodimac.Cedis.Core.Interfaces.Repository;
using Sodimac.Cedis.Core.Interfaces.Services;
using Sodimac.Cedis.Infraestructure.Repository;
using System;
using System.Linq;

namespace Sodimac.Cedis.Api
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
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddControllers().AddNewtonsoftJson(opc =>
            {
                opc.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;
                opc.SerializerSettings.ContractResolver = new DefaultContractResolver();
            });
            //TODO: no olvidar insertar los servicios para inyeccion de dependiencia
            services.AddTransient<IImpresionEtiquetaService, ImpresionEtiquetaService>();
            services.AddTransient<IImpresionEtiquetaRepository, ImpresionEtiquetaRepository>();
            services.AddTransient<ICedisRepository, CedisRepository>();
            services.AddTransient<IEjecucionesRepository, EjecucionesRepository>();
            services.AddTransient<IConsultaRepository, ConsultaRepository>();
            services.AddTransient<ILogsRepository, LogsRepository>();
            services.AddTransient<IConsolaSodimac, ConsolaSodimac>();

            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.SetIsOriginAllowed(isOriginAllowed: _ => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials());
            });


            services.AddSwaggerGen(options =>
            {
                var groupName = "v1";

                options.SwaggerDoc(groupName, new OpenApiInfo
                {
                    Title = $"API Cedis {groupName}",
                    Description = "Impresión de etiquetas",
                    //TODO: Adjuntar la HU aqui
                    TermsOfService = new Uri("https://homecentercolombia.visualstudio.com/SIL%20-%20Megafunza/_workitems/edit/220809"),
                    Contact = new OpenApiContact
                    {
                        Name = "GSLP-CEDIS",
                        Email = "gsplcedis@homecenter.co",
                        Url = new Uri("https://sgl.homecenter.co/"),
                    }
                });
                options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();
            app.UseDeveloperExceptionPage();
            app.UseSwaggerUI(
                options =>
                {
                    options.SwaggerEndpoint("./swagger/v1/swagger.json", "Impresión de etiquetas API v1");
                    options.RoutePrefix = string.Empty;
                }
            );

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseCors("AllowOrigin");

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
