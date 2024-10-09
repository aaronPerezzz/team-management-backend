using AutoMapper;
using team_management_backend.domain.Entities;
using team_management_backend.Web.Model;

namespace team_management_backend.Mapper
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<TipoEquipo, TipoEquipoModel>();
            CreateMap<Equipo, EquipoModel>().ReverseMap();
            CreateMap<Garantia, GarantiaModel>().ReverseMap();
            CreateMap<Poliza, PolizaModel>().ReverseMap(); 
            CreateMap<CaracteristicasTransporte, CaracteristicasTransporteModel>().ReverseMap();
        }
    }
}
