using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniSociety.Dominio.Aplicacao;
using MiniSociety.Dominio.Crosscutting;
using MiniSociety.Dominio.Entitidades;
using MiniSociety.Dominio.Repositorios;
using System.Reflection;

namespace MiniSociety.WebApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(new AppSettingsHelper(Configuration));
            services.AddTransient<AlunosRepositorio>();
            services.AddTransient<TurmasRepositorio>();
            services.AddTransient<RealizarInscricaoHandler>();

            services.AddMediatR(typeof(Startup).GetTypeInfo().Assembly, typeof(Aluno).GetTypeInfo().Assembly);

            services.AddMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            app.UseMvc();
        }
    }
}
