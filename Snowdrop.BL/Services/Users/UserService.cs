using System.Data;
using System.Security.Authentication;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Snowdrop.DAL.Repositories;
using Snowdrop.Data.Entites;
using Snowdrop.Infrastructure.Dto.Users;

namespace Snowdrop.BL.Tests.Unit.Services.Users
{
     public sealed class UserService : IUserService
    {
        private readonly IRepository<User> m_repository = default;
        private readonly IMapper m_mapper = default;

        private const string Salt = "94kJQ^rOZo^Jljn!qkjAHenG*Ggenia2%eN%zzg6y2";
        
        public UserService(
            IRepository<User> repository,
            IMapper mapper)
        {
            m_repository = repository;
            m_mapper = mapper;
        }
        
        public async Task<UserResponse> SignUp(SignUpRequest request)
        {
            User existingUser = await m_repository
                .GetSingle(u => u.Email == request.UserName.ToLowerInvariant());

            if (existingUser != null)
            {
                throw new DuplicateNameException($"User with username {request.UserName} already exists");
            }

            User user = m_mapper.Map<User>(request);
            user.PasswordHash = HashHelper.Create(request.Password, Salt);
            await m_repository.Insert(user);
            Claim[] claims = new[]
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email)
            };

            return new UserResponse(user.Email, claims);
        }

        public async Task<UserResponse> SignIn(SignInRequest request)
        {
            User user = await m_repository
                .GetSingle(u => u.Email == request.UserName.ToLowerInvariant());

            if (user == null)
            {
                throw new AuthenticationException($"User with username {request.UserName} not found");
            }

            if (!HashHelper.Validate(request.Password, Salt, user.PasswordHash))
            {
                throw new AuthenticationException($"Invalid credentials for {request.UserName}");
            }
            
            Claim[] claims = new[]
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email)
            };
            return new UserResponse(user.Email, claims);
        }
    }
}