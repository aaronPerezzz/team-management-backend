/*
 * @author Aaron Pérez
 * @since 07/10/2024
 */

namespace team_management_backend.DTOs
{
    public class BaseDTO<T>
    {
        public bool EsCorrecto { get; set; } = true;
        public string Mensaje { get; set; }
        public T Respuesta { get; set; } = default;

        public BaseDTO(bool EsCorrecto, string mensaje, T respuesta = default)
        {
            this.EsCorrecto = EsCorrecto;
            Mensaje = mensaje;
            Respuesta = respuesta;
        }

    }
}
