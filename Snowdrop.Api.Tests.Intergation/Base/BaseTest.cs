using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Snowdrop.Api.Tests.Intergation.Helpers;
using Snowdrop.Auth.Extensions;
using Snowdrop.DAL.Extensions;

namespace Snowdrop.Api.Tests.Intergation.Base
{
    public abstract class BaseTest<TStartup> : IDisposable
    {
        protected abstract string DatabaseConnectionString { get; }
        protected abstract string EnvironmentName { get; }
        protected abstract string ConfigurationFileName { get; }

        public HttpClient Client { get; private set; }
        public IServiceProvider Services { get; private set; }

        private TestServer m_server = default;
        
        
        protected void Initialize()
        {
            Assembly startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;
            string relativePath = Path.Combine("");

            string contentRoot = TestProjectHelper.GetProjectPath(relativePath, startupAssembly);

            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder()
                                                         .SetBasePath(contentRoot)
                                                         .AddJsonFile(ConfigurationFileName);

            IConfiguration configuration = configurationBuilder.Build();

            IWebHostBuilder webHostBuilder = new WebHostBuilder()
                                             .UseContentRoot(contentRoot)
                                             .ConfigureServices(services => InitializeServices(services, configuration))
                                             .UseConfiguration(configuration)
                                             .UseEnvironment(EnvironmentName)
                                             .UseStartup(typeof(TStartup));

            m_server = new TestServer(webHostBuilder);

            Client = m_server.CreateClient();
            Client.BaseAddress = new Uri("https://localhost:5001");
            Client.DefaultRequestHeaders.Accept.Clear();
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        private void InitializeServices(IServiceCollection services, IConfiguration configuration)
        {
            Assembly startupAssembly = typeof(TStartup).GetTypeInfo().Assembly;

            ApplicationPartManager manager = new ApplicationPartManager
            {
                ApplicationParts =
                {
                    new AssemblyPart(startupAssembly)
                },
                FeatureProviders =
                {
                    new ControllerFeatureProvider()
                }
            };

            services.AddSnowdropContext(DatabaseConnectionString);
            services.AddSnowdropJwt(configuration);
            services.AddMemorySessionManager();
            services.AddSingleton(manager);

            Services = services.BuildServiceProvider();
        }
        
        public void Dispose()
        {
            m_server?.Dispose();
            Client?.Dispose();
        }
    }
}