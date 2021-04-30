using AutoMapper;
using Snowdrop.Data;
using Snowdrop.Infrastructure.Dto;

namespace Snowdrop.Infrastructure.MappringProfiles
{
    public sealed class ProjectProfiles : Profile
    {
        public ProjectProfiles()
        {
            CreateMap<Project, ProjectDto>();
            CreateMap<ProjectDto, Project>();
        }
    }
}