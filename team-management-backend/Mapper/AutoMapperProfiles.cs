using AutoMapper;
using team_management_backend.Models;
using team_management_backend.DTOs.Asignaciones;

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
