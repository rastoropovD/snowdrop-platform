using AutoMapper;
using Snowdrop.Data.Entites;
using Snowdrop.Infrastructure.Dto.Users;

namespace Snowdrop.Infrastructure.MappringProfiles
{
    public sealed class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<SignUpRequest, User>()
                .ForMember(u => u.PasswordHash,
                    opt => opt.Ignore())
                .ForMember(u => u.Email,
                    opt => 
                        opt.MapFrom(scr => scr.UserName.ToLowerInvariant()));
        }
    }
}