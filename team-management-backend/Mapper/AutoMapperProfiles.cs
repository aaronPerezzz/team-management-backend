using AutoMapper;
using team_management_backend.domain.Entities;
using team_management_backend.Web.Model;
using team_management_backend.Web.Model.Asignaciones;

namespace team_management_backend.Mapper
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<AsignacionCrearDTO, Asignacion>();
            CreateMap<TipoEquipo, TipoEquipoModel>();
            CreateMap<Equipo, EquipoModel>().ReverseMap();
            CreateMap<Garantia, GarantiaModel>().ReverseMap();
            CreateMap<Poliza, PolizaModel>().ReverseMap(); 
            CreateMap<CaracteristicasTransporte, CaracteristicasTransporteModel>().ReverseMap();
        }
    }
}
