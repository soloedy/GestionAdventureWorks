using AdventureWorks.Enterprise.App.Models;
using System.Text.Json;
using System.Text;

namespace AdventureWorks.Enterprise.App.Services
{
    public class ApiService
    {
        private readonly HttpClient _ObjHttpClient;
        private readonly ILogger<ApiService> _ObjLogger;
        private readonly JsonSerializerOptions _ObjJsonOptions;
            
        public ApiService(HttpClient ObjHttpClient, ILogger<ApiService> ObjLogger, IConfiguration configuration)
        {
            _ObjHttpClient = ObjHttpClient;
            _ObjLogger = ObjLogger;
            if (_ObjHttpClient.BaseAddress == null)
            {
                // Puedes cambiar la URL por la que corresponda a tu API
                _ObjHttpClient.BaseAddress = new Uri("https://localhost:7279");
            }
            _ObjJsonOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = null, // Cambiar a null para manejar PascalCase
                PropertyNameCaseInsensitive = true, // Permitir coincidencias insensibles a mayúsculas
                WriteIndented = true
            };

            // Fallback: algunos proyectos usan ApiSettings y otros ApiSetting
            var apiKey = configuration["ApiSettings:ApiKey"] ?? configuration["ApiSetting:ApiKey"];
            if (!string.IsNullOrWhiteSpace(apiKey))
            {
                if (!_ObjHttpClient.DefaultRequestHeaders.Contains("X-Api-Key"))
                    _ObjHttpClient.DefaultRequestHeaders.Add("X-Api-Key", apiKey);
            }
            else
            {
                _ObjLogger.LogWarning("API Key no configurada. Verifique appsettings (ApiSettings:ApiKey o ApiSetting:ApiKey).");
            }
        }

        /// <summary>
        /// Obtiene todos los empleados
        /// </summary>
        public async Task<ApiResponse<List<EmployeeFullReadDto>>> FncGetAllEmployeesAsync()
        {
            try
            {
                _ObjLogger.LogInformation("Iniciando llamada al API para obtener empleados");
                var ObjResponse = await _ObjHttpClient.GetAsync("api/Employee");
                var StrContent = await ObjResponse.Content.ReadAsStringAsync();
                _ObjLogger.LogInformation("Respuesta del API recibida. Status: {StatusCode}, Content Length: {Length}", ObjResponse.StatusCode, StrContent.Length);

                if (ObjResponse.IsSuccessStatusCode)
                {
                    try
                    {
                        var ObjApiResponse = JsonSerializer.Deserialize<ApiResponse<List<EmployeeFullReadDto>>>(StrContent, _ObjJsonOptions);
                        _ObjLogger.LogInformation("Respuesta deserializada exitosamente. Empleados encontrados: {Count}", ObjApiResponse?.Data?.Count ?? 0);
                        return ObjApiResponse ?? new ApiResponse<List<EmployeeFullReadDto>> { Status = false, Message = "Error al procesar respuesta", Data = new List<EmployeeFullReadDto>() };
                    }
                    catch (JsonException jsonEx)
                    {
                        _ObjLogger.LogError(jsonEx, "Error al deserializar la respuesta JSON");
                        return new ApiResponse<List<EmployeeFullReadDto>>
                        {
                            Status = false,
                            Message = "La respuesta del servidor no es un JSON válido.",
                            Observacion = StrContent,
                            Data = new List<EmployeeFullReadDto>()
                        };
                    }
                }
                else
                {
                    try
                    {
                        var ObjErrorResponse = JsonSerializer.Deserialize<ApiResponse<object>>(StrContent, _ObjJsonOptions);
                        return new ApiResponse<List<EmployeeFullReadDto>>
                        {
                            Status = false,
                            Message = ObjErrorResponse?.Message ?? "Error al obtener empleados",
                            Observacion = ObjErrorResponse?.Observacion,
                            Data = new List<EmployeeFullReadDto>()
                        };
                    }
                    catch (JsonException jsonEx)
                    {
                        _ObjLogger.LogError(jsonEx, "Error al deserializar el error JSON");
                        return new ApiResponse<List<EmployeeFullReadDto>>
                        {
                            Status = false,
                            Message = "La respuesta de error del servidor no es un JSON válido.",
                            Observacion = StrContent,
                            Data = new List<EmployeeFullReadDto>()
                        };
                    }
                }
            }
            catch (Exception Ex)
            {
                _ObjLogger.LogError(Ex, "Error al obtener empleados desde la API");
                return new ApiResponse<List<EmployeeFullReadDto>>
                {
                    Status = false,
                    Message = "Error de conexión con el servidor",
                    Observacion = Ex.Message,
                    Data = new List<EmployeeFullReadDto>()
                };
            }
        }

        // Métodos para Employee
        public async Task<ApiResponse<EmployeeFullReadDto>> FncGetEmployeeByIdAsync(int id)
        {
            try
            {
                var response = await _ObjHttpClient.GetAsync($"api/Employee/{id}");
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<EmployeeFullReadDto>>(content, _ObjJsonOptions) ?? new ApiResponse<EmployeeFullReadDto> { Status = false, Message = "Error al obtener empleado" };
            }
            catch (Exception ex)
            {
                _ObjLogger.LogError(ex, "Error al obtener empleado {Id}", id);
                return new ApiResponse<EmployeeFullReadDto> { Status = false, Message = "Error de conexión", Observacion = ex.Message };
            }
        }

        public async Task<ApiResponse<EmployeeFullReadDto>> FncCreateEmployeeFullAsync(CreateEmployeeFullDto dto)
        {
            try
            {
                var json = JsonSerializer.Serialize(dto, _ObjJsonOptions);
                // Endpoint correcto es api/Employee/full (el POST simple crea otro recurso distinto)
                var response = await _ObjHttpClient.PostAsync("api/Employee/full", new StringContent(json, Encoding.UTF8, "application/json"));
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<EmployeeFullReadDto>>(content, _ObjJsonOptions) ?? new ApiResponse<EmployeeFullReadDto> { Status = false, Message = "Error al crear empleado" };
            }
            catch (Exception ex)
            {
                _ObjLogger.LogError(ex, "Error al crear empleado");
                return new ApiResponse<EmployeeFullReadDto> { Status = false, Message = "Error de conexión", Observacion = ex.Message };
            }
        }

        // Métodos para Product
        public async Task<ApiResponse<List<ProductReadDto>>> FncGetAllProductsAsync()
        {
            try
            {
                var response = await _ObjHttpClient.GetAsync("api/Product");
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<List<ProductReadDto>>>(content, _ObjJsonOptions) ?? new ApiResponse<List<ProductReadDto>> { Status = false, Message = "Error al obtener productos" };
            }
            catch (Exception ex)
            {
                _ObjLogger.LogError(ex, "Error al obtener productos");
                return new ApiResponse<List<ProductReadDto>> { Status = false, Message = "Error de conexión", Observacion = ex.Message };
            }
        }

        public async Task<ApiResponse<ProductReadDto>> FncGetProductByIdAsync(int id)
        {
            try
            {
                var response = await _ObjHttpClient.GetAsync($"api/Product/{id}");
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<ProductReadDto>>(content, _ObjJsonOptions) ?? new ApiResponse<ProductReadDto> { Status = false, Message = "Error al obtener producto" };
            }
            catch (Exception ex)
            {
                _ObjLogger.LogError(ex, "Error al obtener producto {Id}", id);
                return new ApiResponse<ProductReadDto> { Status = false, Message = "Error de conexión", Observacion = ex.Message };
            }
        }

        public async Task<ApiResponse<ProductReadDto>> FncCreateProductAsync(ProductCreateDto dto)
        {
            try
            {
                var json = JsonSerializer.Serialize(dto, _ObjJsonOptions);
                var response = await _ObjHttpClient.PostAsync("api/Product", new StringContent(json, Encoding.UTF8, "application/json"));
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<ProductReadDto>>(content, _ObjJsonOptions) ?? new ApiResponse<ProductReadDto> { Status = false, Message = "Error al crear producto" };
            }
            catch (Exception ex)
            {
                _ObjLogger.LogError(ex, "Error al crear producto");
                return new ApiResponse<ProductReadDto> { Status = false, Message = "Error de conexión", Observacion = ex.Message };
            }
        }

        // Métodos para ProductInventory
        public async Task<ApiResponse<List<ProductInventoryReadDto>>> FncGetAllProductInventoriesAsync()
        {
            try
            {
                var response = await _ObjHttpClient.GetAsync("api/ProductInventory");
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<List<ProductInventoryReadDto>>>(content, _ObjJsonOptions) ?? new ApiResponse<List<ProductInventoryReadDto>> { Status = false, Message = "Error al obtener inventarios" };
            }
            catch (Exception ex)
            {
                _ObjLogger.LogError(ex, "Error al obtener inventarios");
                return new ApiResponse<List<ProductInventoryReadDto>> { Status = false, Message = "Error de conexión", Observacion = ex.Message };
            }
        }

        // Sobrecarga con un parámetro (ProductId)
        public async Task<ApiResponse<ProductInventoryReadDto>> FncGetProductInventoryByIdAsync(int productId)
        {
            try
            {
                var response = await _ObjHttpClient.GetAsync($"api/ProductInventory/{productId}");
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<ProductInventoryReadDto>>(content, _ObjJsonOptions) ?? new ApiResponse<ProductInventoryReadDto> { Status = false, Message = "Error al obtener inventario" };
            }
            catch (Exception ex)
            {
                _ObjLogger.LogError(ex, "Error al obtener inventario del producto {ProductId}", productId);
                return new ApiResponse<ProductInventoryReadDto> { Status = false, Message = "Error de conexión", Observacion = ex.Message };
            }
        }

        // Sobrecarga con dos parámetros (ProductId y LocationId)
        public async Task<ApiResponse<ProductInventoryReadDto>> FncGetProductInventoryByIdAsync(int productId, short locationId)
        {
            try
            {
                var response = await _ObjHttpClient.GetAsync($"api/ProductInventory/Product/{productId}/{locationId}");
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<ProductInventoryReadDto>>(content, _ObjJsonOptions) ?? new ApiResponse<ProductInventoryReadDto> { Status = false, Message = "Error al obtener inventario" };
            }
            catch (Exception ex)
            {
                _ObjLogger.LogError(ex, "Error al obtener inventario del producto {ProductId} en ubicación {LocationId}", productId, locationId);
                return new ApiResponse<ProductInventoryReadDto> { Status = false, Message = "Error de conexión", Observacion = ex.Message };
            }
        }

        public async Task<ApiResponse<ProductInventoryReadDto>> FncUpdateProductInventoryAsync(ProductInventoryUpdateDto dto)
        {
            try
            {
                var json = JsonSerializer.Serialize(dto, _ObjJsonOptions);
                var response = await _ObjHttpClient.PutAsync($"api/ProductInventory", new StringContent(json, Encoding.UTF8, "application/json"));
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<ProductInventoryReadDto>>(content, _ObjJsonOptions) ?? new ApiResponse<ProductInventoryReadDto> { Status = false, Message = "Error al actualizar inventario" };
            }
            catch (Exception ex)
            {
                _ObjLogger.LogError(ex, "Error al actualizar inventario del producto {ProductId}", dto.ProductID);
                return new ApiResponse<ProductInventoryReadDto> { Status = false, Message = "Error de conexión", Observacion = ex.Message };
            }
        }

        // Métodos para WorkOrder
        public async Task<ApiResponse<List<WorkOrderReadDto>>> FncGetAllWorkOrdersAsync()
        {
            try
            {
                var response = await _ObjHttpClient.GetAsync("api/WorkOrder");
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<List<WorkOrderReadDto>>>(content, _ObjJsonOptions) ?? new ApiResponse<List<WorkOrderReadDto>> { Status = false, Message = "Error al obtener órdenes" };
            }
            catch (Exception ex)
            {
                _ObjLogger.LogError(ex, "Error al obtener órdenes de trabajo");
                return new ApiResponse<List<WorkOrderReadDto>> { Status = false, Message = "Error de conexión", Observacion = ex.Message };
            }
        }

        public async Task<ApiResponse<WorkOrderReadDto>> FncCreateWorkOrderAsync(WorkOrderCreateDto dto)
        {
            try
            {
                var json = JsonSerializer.Serialize(dto, _ObjJsonOptions);
                var response = await _ObjHttpClient.PostAsync("api/WorkOrder", new StringContent(json, Encoding.UTF8, "application/json"));
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<WorkOrderReadDto>>(content, _ObjJsonOptions) ?? new ApiResponse<WorkOrderReadDto> { Status = false, Message = "Error al crear orden" };
            }
            catch (Exception ex)
            {
                _ObjLogger.LogError(ex, "Error al crear orden de trabajo");
                return new ApiResponse<WorkOrderReadDto> { Status = false, Message = "Error de conexión", Observacion = ex.Message };
            }
        }

        // Reporte RRHH: empleados con más tiempo en departamento
        public async Task<ApiResponse<List<EmployeeDepartmentDto>>> FncGetEmployeesByDepartmentTimeAsync(int cantidad)
        {
            try
            {
                var response = await _ObjHttpClient.GetAsync($"api/Employee/department/{cantidad}");
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<ApiResponse<List<EmployeeDepartmentDto>>>(content, _ObjJsonOptions) ?? new ApiResponse<List<EmployeeDepartmentDto>> { Status = false, Message = "Error al obtener reporte RRHH" };
            }
            catch (Exception ex)
            {
                _ObjLogger.LogError(ex, "Error al obtener reporte de empleados por departamento");
                return new ApiResponse<List<EmployeeDepartmentDto>> { Status = false, Message = "Error de conexión", Observacion = ex.Message };
            }
        }

        /// <summary>
        /// Obtiene productos con bajo inventario (actualizado con logging)
        /// </summary>
        public async Task<ApiResponse<List<ProductLowInventoryDto>>> GetLowInventoryAsync(int IntCantidad)
        {
            try
            {
                _ObjLogger.LogInformation("Llamando a low-inventory con IntCantidad={Cantidad}", IntCantidad);
                var ObjResponse = await _ObjHttpClient.GetAsync($"api/ProductInventory/low-inventory/{IntCantidad}");
                var StrContent = await ObjResponse.Content.ReadAsStringAsync();
                _ObjLogger.LogInformation("Respuesta low-inventory Status={StatusCode} BodyLength={Length}", (int)ObjResponse.StatusCode, StrContent.Length);
                if (ObjResponse.IsSuccessStatusCode)
                {
                    try
                    {
                        var ObjApiResponse = JsonSerializer.Deserialize<ApiResponse<List<ProductLowInventoryDto>>>(StrContent, _ObjJsonOptions);
                        return ObjApiResponse ?? new ApiResponse<List<ProductLowInventoryDto>> { Status = false, Message = "Error al procesar respuesta", Data = new List<ProductLowInventoryDto>(), Observacion = "Deserialización nula" };
                    }
                    catch (JsonException jex)
                    {
                        _ObjLogger.LogError(jex, "Error deserializando respuesta low-inventory");
                        return new ApiResponse<List<ProductLowInventoryDto>> { Status = false, Message = "JSON inválido en respuesta low-inventory", Observacion = StrContent, Data = new List<ProductLowInventoryDto>() };
                    }
                }
                else
                {
                    try
                    {
                        var ObjErrorResponse = JsonSerializer.Deserialize<ApiResponse<object>>(StrContent, _ObjJsonOptions);
                        return new ApiResponse<List<ProductLowInventoryDto>>
                        {
                            Status = false,
                            Message = ObjErrorResponse?.Message ?? $"Error HTTP {(int)ObjResponse.StatusCode}",
                            Observacion = ObjErrorResponse?.Observacion ?? StrContent,
                            Data = new List<ProductLowInventoryDto>()
                        };
                    }
                    catch (JsonException)
                    {
                        return new ApiResponse<List<ProductLowInventoryDto>>
                        {
                            Status = false,
                            Message = $"Error HTTP {(int)ObjResponse.StatusCode} al obtener bajo inventario",
                            Observacion = StrContent,
                            Data = new List<ProductLowInventoryDto>()
                        };
                    }
                }
            }
            catch (Exception Ex)
            {
                _ObjLogger.LogError(Ex, "Excepción en GetLowInventoryAsync");
                return new ApiResponse<List<ProductLowInventoryDto>>
                {
                    Status = false,
                    Message = "Error de conexión con el servidor",
                    Observacion = Ex.Message,
                    Data = new List<ProductLowInventoryDto>()
                };
            }
        }

        /// <summary>
        /// Obtiene el Top 10 de productos más vendidos
        /// </summary>
        public async Task<ApiResponse<List<TopProductSalesDto>>> FncObtenerTopProductosAsync()
        {
            try
            {
                _ObjLogger.LogInformation("Llamando a TopProducts endpoint");
                var ObjResponse = await _ObjHttpClient.GetAsync("api/SalesOrder/TopProducts");
                var StrContent = await ObjResponse.Content.ReadAsStringAsync();
                _ObjLogger.LogInformation("Respuesta TopProducts Status={StatusCode} BodyLength={Length}", (int)ObjResponse.StatusCode, StrContent.Length);
                if (ObjResponse.IsSuccessStatusCode)
                {
                    try
                    {
                        var ObjApiResponse = JsonSerializer.Deserialize<ApiResponse<List<TopProductSalesDto>>>(StrContent, _ObjJsonOptions);
                        return ObjApiResponse ?? new ApiResponse<List<TopProductSalesDto>> { Status = false, Message = "Error al procesar respuesta", Data = new List<TopProductSalesDto>(), Observacion = "Deserialización nula" };
                    }
                    catch (JsonException jex)
                    {
                        _ObjLogger.LogError(jex, "Error deserializando respuesta TopProducts");
                        return new ApiResponse<List<TopProductSalesDto>> { Status = false, Message = "JSON inválido en respuesta TopProducts", Observacion = StrContent, Data = new List<TopProductSalesDto>() };
                    }
                }
                else
                {
                    try
                    {
                        var ObjErrorResponse = JsonSerializer.Deserialize<ApiResponse<object>>(StrContent, _ObjJsonOptions);
                        return new ApiResponse<List<TopProductSalesDto>>
                        {
                            Status = false,
                            Message = ObjErrorResponse?.Message ?? $"Error HTTP {(int)ObjResponse.StatusCode}",
                            Observacion = ObjErrorResponse?.Observacion ?? StrContent,
                            Data = new List<TopProductSalesDto>()
                        };
                    }
                    catch (JsonException)
                    {
                        return new ApiResponse<List<TopProductSalesDto>>
                        {
                            Status = false,
                            Message = $"Error HTTP {(int)ObjResponse.StatusCode} al obtener TopProducts",
                            Observacion = StrContent,
                            Data = new List<TopProductSalesDto>()
                        };
                    }
                }
            }
            catch (Exception Ex)
            {
                _ObjLogger.LogError(Ex, "Excepción en FncObtenerTopProductosAsync");
                return new ApiResponse<List<TopProductSalesDto>>
                {
                    Status = false,
                    Message = "Error de conexión con el servidor",
                    Observacion = Ex.Message,
                    Data = new List<TopProductSalesDto>()
                };
            }
        }

        /// <summary>
        /// DTO para el Top 10 de productos más vendidos
        /// </summary>
        public class TopProductSalesDto
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public decimal UnitPrice { get; set; }
            public int TotalQuantitySold { get; set; }
            public decimal TotalSalesAmount { get; set; }
            public string CategoryName { get; set; } = string.Empty;
            public string SubCategoryName { get; set; } = string.Empty;
        }

        /// <summary>
        /// DTO para productos con bajo inventario
        /// </summary>
        public class ProductLowInventoryDto
        {
            public int ProductID { get; set; }
            public string ProductName { get; set; } = string.Empty;
            public string ProductNumber { get; set; } = string.Empty;
            public short Quantity { get; set; } // <-- Cambiado de int a short
            public string LocationName { get; set; } = string.Empty;
            public string CategoryName { get; set; } = string.Empty;
            public string SubcategoryName { get; set; } = string.Empty;
        }
    }
}