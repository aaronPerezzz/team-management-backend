using team_management_backend.Web.Model.Asignaciones;

namespace team_management_backend.Domain.Interfaces.Repository
{
    public interface IAsignacion
    {
        Task<List<AsignacionRegistroDTO>> GetAllAssignments();
        Task<List<AsignacionRegistroDTO>> GetAssignmentsByType(string nombreTipoEquipo);
        Task<bool> ExisteTipoEquipo(string nombreTipoEquipo);
        Task<(int id, string msj)> CreateAssignment(AsignacionCrearDTO asignacion);
        Task<(List<AsignacionRegistroDTO> lstAsignaciones, string msj)> UserAssignment(string correo);
    }
}
