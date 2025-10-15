namespace AdventureWorks.Enterprise.Api.DTOs
{
    /// <summary>
    /// Clase para estandarizar las respuestas de la API
    /// </summary>
    /// <typeparam name="T">Tipo de datos a devolver</typeparam>
    public class ApiResponse<T>
    {
        /// <summary>
        /// Indica si la operación fue exitosa (true) o si hubo un error (false)
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Datos devueltos por la operación (solo si Status es true)
        /// </summary>
        public T? Data { get; set; }

        /// <summary>
        /// Mensaje claro sobre el resultado (éxito o error)
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Detalle técnico del error (si aplica, solo si hubo error, puede ser null en éxito)
        /// </summary>
        public string? Observacion { get; set; }

        /// <summary>
        /// Constructor para una respuesta exitosa
        /// </summary>
        /// <param name="data">Datos a devolver</param>
        /// <param name="message">Mensaje de éxito</param>
        public static ApiResponse<T> Success(T data, string message = "Operación completada con éxito")
        {
            return new ApiResponse<T>
            {
                Status = true,
                Data = data,
                Message = message,
                Observacion = null
            };
        }

        /// <summary>
        /// Constructor para una respuesta de error
        /// </summary>
        /// <param name="message">Mensaje de error amigable</param>
        /// <param name="observacion">Detalles técnicos del error</param>
        public static ApiResponse<T> Error(string message, string? observacion = null)
        {
            return new ApiResponse<T>
            {
                Status = false,
                Data = default,
                Message = message,
                Observacion = observacion
            };
        }
    }
}