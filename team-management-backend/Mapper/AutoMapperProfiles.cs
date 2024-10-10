using AutoMapper;
using team_management_backend.Entities;
using team_management_backend.Model.Asignaciones;

namespace team_management_backend.Mapper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AsignacionCrearDTO, Asignacion>();

            CreateMap<AsignacionEditarDTO, Asignacion>()
                .ReverseMap();
        }
    }
}
