using AutoMapper;
using Snowdrop.Data.Entites;
using Snowdrop.Infrastructure.Dto;

namespace Snowdrop.Infrastructure.MappringProfiles
{
    public sealed class ProjectMemberProfiles : Profile
    {
        public ProjectMemberProfiles()
        {
            CreateMap<ProjectMember, ProjectMemberDto>();
            CreateMap<ProjectMemberDto, ProjectMember>();
        }
    }
}