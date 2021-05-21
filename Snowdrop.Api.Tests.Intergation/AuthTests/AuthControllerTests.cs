using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Docker.DotNet.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Snowdrop.Api.Tests.Intergation.Base;
using Snowdrop.Api.Tests.Intergation.Helpers;
using Snowdrop.Auth.Models;
using Snowdrop.DAL.Repositories;
using Snowdrop.Data.Entites;
using Snowdrop.Infrastructure.Dto.Users;
using Xunit;
using Xunit.Extensions.Ordering;

namespace Snowdrop.Api.Tests.Intergation.AuthTests
{
    public sealed class AuthControllerTests : IClassFixture<EnvironmentFixture<Startup>>
    {
        private readonly HttpClient m_client = default;
        private readonly IRepository<User> m_repository = default;

        public AuthControllerTests(EnvironmentFixture<Startup> fixture)
        {
            m_client = fixture.Client;
            m_repository = fixture.Services.GetService<IRepository<User>>();
        }

        [Fact, Order(1)]
        public async Task SingUp_ValidUsed_SignUpSuccessResponse()
        {
            //Arrange
            string email = "some.test@email.com";
            string password = "123456";
            string route = "auth/signup";
            SignUpRequest request = new SignUpRequest(email, password);

            //Act
            HttpResponseMessage response = await m_client
                .PostAsync(route, TestProjectHelper.GetStringContent(request));

            string content = await response.Content.ReadAsStringAsync();
            JwtAuthResult authResult = JsonConvert.DeserializeObject<JwtAuthResult>(content);
            User user = await m_repository.GetSingle(1);
            
            //Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(content);
            Assert.NotNull(user);
        }
        
        [Fact, Order(2)]
        public async Task SingIn_ValidUsed_SignInSuccessResponse()
        {
            //Arrange
            string email = "some.test@email.com";
            string password = "123456";
            string route = "auth/signin";
            SignInRequest request = new SignInRequest(email, password);

            //Act
            HttpResponseMessage response = await m_client
                .PostAsync(route, TestProjectHelper.GetStringContent(request));

            string content = await response.Content.ReadAsStringAsync();
            JwtAuthResult authResult = JsonConvert.DeserializeObject<JwtAuthResult>(content);
            
            //Assert
            response.EnsureSuccessStatusCode();
            Assert.NotNull(authResult);
        }
    }
}