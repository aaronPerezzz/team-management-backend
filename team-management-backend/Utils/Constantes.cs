/*
 * @author Aaron Pérez
 * @since 07/10/2024
 */

using System.Globalization;

namespace team_management_backend.Utils
{
    public class Constantes
    {

        public const string USUARIO = "Usuario";
        public const string ADMINISTRADOR = "Administrador";

        public const string ERROR_NOCONTROLADO_SSEG = "Error no controlado en Servicios de Seguridad: ";

        public const string ERROR_SEG01 = "Usuario no existe";
        public const string ERROR_SEG02 = "Error al añadir rol al usuario";
        public const string ERROR_SEG03 = "Error al crear el usuario";
        public const string ERROR_SEG04 = "Error al obtener los roles";
        public const string ERROR_SEG05 = "Error al realizar consulta";


        public const string ERROR_TIE01 = "Tipos de equipo vacía";


        //MENSAJES DE EXITO Seguridad
        public const string MSJ_SEG01 = "Inicio de sesión correctamente";
        public const string MSJ_SEG02 = "Usuario Editado con éxito";
        public const string MSJ_SEG03 = "Lista de Roles";
        public const string MSJ_SEG04 = "Lista de usuarios";
        public const string MSJ_SEG05 = "Usuario Editado con éxito";

        //MENSAJES DE EXITO
        public const string MSJ_TIE01 = "Tipos de equipo";

        public const int NUM0 = 0;
        public const int NUM1 = 1; 
        public const int NUM2 = 2;
        public const int NUM3 = 3;

        public const bool TRUE = true;
        public const bool FALSE = false;

        #region Constantes Asignaciones
        //MENSAJES DE ERROR ASIGNACIONES
        public const string ERROR_AS01 = "No se encontraron asignaciones.";
        public const string ERROR_AS02 = "Usuario no encontrado.";
        public const string ERROR_AS03 = "No se encontró la asignación.";
        public const string ERROR_AS04 = "El equipo ya está asignado a otro usuario. No puede reasignarse hasta que se libere";
        public const string ERROR_AS05 = "No se editó la asignación.";
        public const string ERROR_AS06 = "No se pudo eliminar la asignación.";
        public const string ERROR_AS07 = "Error no controlado en el servicio de Asignaciones-";
        public const string ERROR_AS08 = "No existe el tipo de equipo: ";
        public const string ERROR_AS09 = "No se encontraron asignaciones para el tipo de equipo proporcionado.";
        public const string ERROR_AS10 = "Error no controlado: ";
        public const string ERROR_AS11 = "Mal identificador id";

        //MENSAJES DE ÉXITO ASIGNACIONES
        public const string MSJ_AS01 = "Asignación editada con éxito.";
        public const string MSJ_AS02 = "Asignación eliminada.";
        public const string MSJ_AS03 = "Operación realizada con éxito.";

        #endregion
    }
}
