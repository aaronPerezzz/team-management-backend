namespace team_management_backend.Utils.Pagination
{
    public static class HttpContextExtensions
    {
        public static Task InsertarParametrosPaginacionEnRespuesta(
            this HttpContext context, int totalRegistro, int cantidadRegistrosMostral)
        {
            if (context == null) { throw new ArgumentNullException(nameof(context)); }
            double conteo = totalRegistro;
            double totalPaginas = Math.Ceiling(conteo / cantidadRegistrosMostral);
            context.Response.Headers.Append("totalRegistros", conteo.ToString());
            context.Response.Headers.Append("totalPaginas", totalPaginas.ToString());
            return Task.FromResult(0);
        }
    }
}
