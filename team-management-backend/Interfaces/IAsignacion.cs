using team_management_backend.Model.Asignaciones;
using team_management_backend.Utils.Pagination;

namespace team_management_backend.Interfaces
{
    public interface IAsignacion
    {
        Task<List<AsignacionRegistroDTO>> GetAllAssignments(PaginationDTO pag);
        Task<List<AsignacionRegistroDTO>> GetAssignmentsByType(string nombreTipoEquipo, PaginationDTO pag);
        Task<bool> ThereIsEquipment(string nombreTipoEquipo);
        Task<(int id, string msj)> CreateAssignment(AsignacionCrearDTO asignacion);
        Task<(List<AsignacionRegistroDTO> lstAsignaciones, string msj)> UserAssignment(string correo);
        Task<(int id, string msj)> UpdateAssignment(AsignacionEditarDTO asignacion);
        Task<string> DeleteAssignment(int id);
    }
}
