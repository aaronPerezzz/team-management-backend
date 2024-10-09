using AutoMapper;
using team_management_backend.domain.Entities;
using team_management_backend.Web.Model.Asignaciones;

namespace team_management_backend.Mapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AsignacionCrearDTO, Asignacion>();
        }
    }
}
