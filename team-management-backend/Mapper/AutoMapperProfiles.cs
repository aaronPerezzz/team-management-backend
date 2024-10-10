using AutoMapper;
using team_management_backend.Models;
using team_management_backend.DTOs.Asignaciones;
using team_management_backend.DTOs;

namespace team_management_backend.Mapper
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AsignacionCrearDTO, Asignacion>();
            CreateMap<AsignacionEditarDTO, Asignacion>()
                .ReverseMap();
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
