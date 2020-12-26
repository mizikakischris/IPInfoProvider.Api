using IPInfoProvider.Controllers;
using IPInfoProvider.Interfaces;
using IPInfoProvider.Services;
using IPInfoProvider.Types.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

namespace IPInfoProvider.Api
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
            RegisterApiServices(services);
            RegisterHttpClients(services);
            RegisterConfigurations(services);
            RegisterCache(services);
            RegisterControllers(services);
        }

        private void RegisterCache(IServiceCollection services)
        {
            services.AddMemoryCache();
        }

        private void RegisterControllers(IServiceCollection services)
        {
            services.AddControllers()
            .AddNewtonsoftJson(options =>
            {
                options.UseMemberCasing();
                options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                options.SerializerSettings.Converters = new List<JsonConverter>
                {
                    new StringEnumConverter
                    {
                        AllowIntegerValues = false
                    }
                };
            })
            .AddApplicationPart(typeof(IPInfoProviderController).Assembly);
        }
        private void RegisterApiServices(IServiceCollection services)
        {
            services.AddScoped<IIPInfoProviderService, IPInfoProviderService>();
        }
        private void RegisterHttpClients(IServiceCollection services)
        {
            services.AddHttpClient();
        }

        private void RegisterConfigurations(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
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

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
