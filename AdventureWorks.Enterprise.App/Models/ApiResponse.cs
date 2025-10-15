namespace AdventureWorks.Enterprise.App.Models
{
    /// <summary>
    /// Clase para estandarizar las respuestas de la API
    /// </summary>
    /// <typeparam name="T">Tipo de datos a devolver</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indica si la operaci�n fue exitosa (true) o si hubo un error (false)
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Datos devueltos por la operaci�n (solo si Status es true)
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Mensaje claro sobre el resultado (�xito o error)
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Detalle t�cnico del error (si aplica, solo si hubo error, puede ser null en �xito)
        /// </summary>
        public string? Observacion { get; set; }
    }

    /// <summary>
    /// Modelo para paginaci�n
    /// </summary>
    public class PagedResult<T>
    {
        public List<T> Data { get; set; } = new();
        public int IntTotalRecords { get; set; }
        public int IntPage { get; set; }
        public int IntPageSize { get; set; }
        public int IntTotalPages => (int)Math.Ceiling((double)IntTotalRecords / IntPageSize);
        public bool BlnHasNextPage => IntPage < IntTotalPages;
        public bool BlnHasPreviousPage => IntPage > 1;
    }
}