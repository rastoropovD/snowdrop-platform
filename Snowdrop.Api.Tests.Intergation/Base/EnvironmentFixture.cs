using System;
using System.Threading.Tasks;
using TestEnvironment.Docker;
using TestEnvironment.Docker.Containers.Mssql;
using Xunit;

namespace Snowdrop.Api.Tests.Intergation.Base
{
    public sealed class EnvironmentFixture<TStartup> : BaseTest<TStartup>, IAsyncLifetime
    {
        private string m_connectionString = default;
        protected override string DatabaseConnectionString => m_connectionString;
        protected override string EnvironmentName => "Tests";
        protected override string ConfigurationFileName => "appsettings.Tests.json";

        private const string SqlContainerName = "mssql-tests";
        private DockerEnvironment m_environment = default;
        private MssqlContainer m_mssqlContainer = default;
        
        public async Task InitializeAsync()
        {
            DockerEnvironmentBuilder builder = new DockerEnvironmentBuilder();
            m_environment = builder
                            .AddMssqlContainer(SqlContainerName, Guid.NewGuid().ToString())
                            .Build();

            await m_environment.Up();

            m_mssqlContainer = m_environment.GetContainer<MssqlContainer>(SqlContainerName);
            m_connectionString = m_mssqlContainer.GetConnectionString();

            Initialize();
        }

        public async Task DisposeAsync()
        {
            await m_environment.DisposeAsync();
            Dispose();
        }


    }
}