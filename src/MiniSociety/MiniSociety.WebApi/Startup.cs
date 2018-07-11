using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MiniSociety.Dominio.Crosscutting;
using MiniSociety.Dominio.Repositorios;
using MiniSociety.Dominio.Servicos;

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
            services.AddTransient<InscricaoRepositorio>();
            services.AddTransient<InscricaoServico>();

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
