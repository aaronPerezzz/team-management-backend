using AutoMapper;
using team_management_backend.domain.Entities;
using team_management_backend.DTOs;
using team_management_backend.Web.Model;
using team_management_backend.Web.Model.Asignaciones;

namespace team_management_backend.Mapper
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AsignacionCrearDTO, Asignacion>();
            CreateMap<TipoEquipo, TipoEquipoDTO>().ReverseMap();
            CreateMap<Equipo, EquipoDTO>().ReverseMap();
            CreateMap<Garantia, GarantiaDTO>().ReverseMap();
            CreateMap<Poliza, PolizaDTO>().ReverseMap();
            CreateMap<Software, SoftwareDTO>().ReverseMap();
            CreateMap<Hardware, HardwareDTO>().ReverseMap();
            CreateMap<CaracteristicasTransporte, CaracteristicasTransporteDTO>().ReverseMap();

            CreateMap<TipoEquipo, PorTipoEquipoDTO>();
        }
    }
}
