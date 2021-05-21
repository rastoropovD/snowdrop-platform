using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Snowdrop.Auth.Extensions;
using Snowdrop.BL.Tests.Unit.Extensions;
using Snowdrop.DAL.Extensions;
using Snowdrop.Infrastructure.Extensions;

namespace Snowdrop.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Environment = env;
        }

        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddSwaggerGen(c => { c.SwaggerDoc("v1", new OpenApiInfo {Title = "Snowdrop.Api", Version = "v1"}); });
            
            if (!Environment.IsEnvironment("Tests"))
            {
                services.AddSnowdropJwt(Configuration);
                services.AddRedisSessionManager(Configuration);
                services.AddSnowdropContext(Configuration.GetConnectionString("SnowdropContext"));
            }

            services.AddSnowdropMapper();
            services.AddSnowdropServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Snowdrop.Api v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            
            app.AddAuth();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}