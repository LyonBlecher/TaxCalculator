using System;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using TaxCalculator.Api.Middleware;
using TaxCalculator.Core;
using TaxCalculator.Core.Datalayer;
using TaxCalculator.Core.Factory;
using TaxCalculator.Core.Options;
using TaxCalculator.Core.Repository;

namespace TaxCalculator.Api
{
    public class Startup
    {
        private string _v;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;            

            _v = $"v{this.GetType().Assembly.GetName().Version}";

            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                //.AddJsonFile($"appsettings.Development.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<Settings>(Configuration.GetSection(nameof(Settings)));

            services.AddTransient<ITaxCalculatorDb, TaxCalculatorDb>();
            services.AddTransient<ITaxCalculatorRepository, TaxCalculatorRepository>();
            services.AddTransient<ICalculatorFactory, CalculatorFactory>();
            services.AddTransient<ITaxCalculator, Core.TaxCalculator>();

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc(_v,
                    new Info
                    {
                        Title = "Tax Calculator",
                        Contact = new Contact { Email = "lyonblecher@gmail.com", Name = "Lyon Blecher" },
                        Version = _v
                    });

                var filePath = Path.Combine(AppContext.BaseDirectory, $"{this.GetType().Assembly.GetName().Name}.xml");

                c.IncludeXmlComments(filePath);
            });
                
            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"/swagger/{_v}/swagger.json", $"Tax Calculator {_v}");
                c.RoutePrefix = string.Empty;
            });

            app.UseMvc();
        }
    }
}
