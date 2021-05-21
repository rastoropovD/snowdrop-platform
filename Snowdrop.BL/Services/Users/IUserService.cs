using System.Threading.Tasks;
using Snowdrop.Infrastructure.Dto.Users;

namespace Snowdrop.BL.Tests.Unit.Services.Users
{
    public interface IUserService
    {
        Task<UserResponse> SignUp(SignUpRequest request);
        Task<UserResponse> SignIn(SignInRequest request);
    }
}