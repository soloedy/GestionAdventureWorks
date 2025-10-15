using AdventureWorks.Enterprise.Api.Data;
using AdventureWorks.Enterprise.Api.DTOs;
using AdventureWorks.Enterprise.Api.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Data;

namespace AdventureWorks.Enterprise.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly AdventureWorksDbContext _ObjDbContext;
        private readonly ILogger<EmployeeController> _ObjLogger;
        private readonly IConfiguration _ObjConfiguration;

        public EmployeeController(AdventureWorksDbContext ObjDbContext, ILogger<EmployeeController> ObjLogger, IConfiguration ObjConfiguration)
        {
            _ObjDbContext = ObjDbContext;
            _ObjLogger = ObjLogger;
            _ObjConfiguration = ObjConfiguration;
        }

        private IActionResult FncJsonResponse(bool BlnStatus, object? ObjData, string StrMessage, string? StrObservacion = null, int IntStatusCode = 200)
        {
            return StatusCode(IntStatusCode, new { Status = BlnStatus, Data = ObjData, Message = StrMessage, Observacion = StrObservacion });
        }

        [HttpGet]
        public async Task<IActionResult> FncGetAll()
        {
            try
            {
                var StrSql = "SELECT be.BusinessEntityID AS IntBusinessEntityID, " +
                           "p.PersonType AS StrPersonType, " +
                           "p.NameStyle AS BlnNameStyle, " +
                           "p.Title AS StrTitle, " +
                           "p.FirstName AS StrFirstName, " +
                           "p.MiddleName AS StrMiddleName, " +
                           "p.LastName AS StrLastName, " +
                           "p.Suffix AS StrSuffix, " +
                           "p.EmailPromotion AS IntEmailPromotion, " +
                           "p.rowguid AS PersonRowGuid, " +
                           "p.ModifiedDate AS PersonModifiedDate, " +
                           "e.NationalIDNumber AS StrNationalIDNumber, " +
                           "e.LoginID AS StrLoginID, " +
                           "CAST(e.OrganizationNode AS nvarchar(4000)) AS StrOrganizationNode, " +
                           "e.OrganizationNode.GetLevel() AS IntOrganizationLevel, " +
                           "e.JobTitle AS StrJobTitle, " +
                           "e.BirthDate AS DtmBirthDate, " +
                           "e.MaritalStatus AS StrMaritalStatus, " +
                           "e.Gender AS StrGender, " +
                           "e.HireDate AS DtmHireDate, " +
                           "e.SalariedFlag AS BlnSalariedFlag, " +
                           "e.VacationHours AS IntVacationHours, " +
                           "e.SickLeaveHours AS IntSickLeaveHours, " +
                           "e.CurrentFlag AS BlnCurrentFlag, " +
                           "e.rowguid AS EmployeeRowGuid, " +
                           "e.ModifiedDate AS EmployeeModifiedDate " +
                           "FROM Person.BusinessEntity be " +
                           "JOIN Person.Person p ON p.BusinessEntityID = be.BusinessEntityID " +
                           "JOIN HumanResources.Employee e ON e.BusinessEntityID = be.BusinessEntityID";
                
                var LstEmployees = await _ObjDbContext.Set<EmployeeFullReadDto>().FromSqlRaw(StrSql).ToListAsync();
                return FncJsonResponse(true, LstEmployees, "Empleados obtenidos correctamente.");
            }
            catch (Exception Ex)
            {
                _ObjLogger.LogError(Ex, "Error al obtener empleados");
                return FncJsonResponse(false, null, "Error interno al obtener empleados.", Ex.Message, 500);
            }
        }

        [HttpGet("{IntId:int}")]
        public async Task<IActionResult> FncGetById(int IntId)
        {
            try
            {
                var StrSql = "SELECT be.BusinessEntityID AS IntBusinessEntityID, " +
                           "p.PersonType AS StrPersonType, " +
                           "p.NameStyle AS BlnNameStyle, " +
                           "p.Title AS StrTitle, " +
                           "p.FirstName AS StrFirstName, " +
                           "p.MiddleName AS StrMiddleName, " +
                           "p.LastName AS StrLastName, " +
                           "p.Suffix AS StrSuffix, " +
                           "p.EmailPromotion AS IntEmailPromotion, " +
                           "p.rowguid AS PersonRowGuid, " +
                           "p.ModifiedDate AS PersonModifiedDate, " +
                           "e.NationalIDNumber AS StrNationalIDNumber, " +
                           "e.LoginID AS StrLoginID, " +
                           "CAST(e.OrganizationNode AS nvarchar(4000)) AS StrOrganizationNode, " +
                           "e.OrganizationNode.GetLevel() AS IntOrganizationLevel, " +
                           "e.JobTitle AS StrJobTitle, " +
                           "e.BirthDate AS DtmBirthDate, " +
                           "e.MaritalStatus AS StrMaritalStatus, " +
                           "e.Gender AS StrGender, " +
                           "e.HireDate AS DtmHireDate, " +
                           "e.SalariedFlag AS BlnSalariedFlag, " +
                           "e.VacationHours AS IntVacationHours, " +
                           "e.SickLeaveHours AS IntSickLeaveHours, " +
                           "e.CurrentFlag AS BlnCurrentFlag, " +
                           "e.rowguid AS EmployeeRowGuid, " +
                           "e.ModifiedDate AS EmployeeModifiedDate " +
                           "FROM Person.BusinessEntity be " +
                           "JOIN Person.Person p ON p.BusinessEntityID = be.BusinessEntityID " +
                           "JOIN HumanResources.Employee e ON e.BusinessEntityID = be.BusinessEntityID " +
                           "WHERE be.BusinessEntityID = @IntId";
                
                var Param = new SqlParameter("@IntId", IntId);
                var LstEmployees = await _ObjDbContext.Set<EmployeeFullReadDto>().FromSqlRaw(StrSql, Param).ToListAsync();
                var ObjDto = LstEmployees.FirstOrDefault();
                if (ObjDto == null) return FncJsonResponse(false, null, $"No existe empleado con ID: {IntId}", null, 404);
                return FncJsonResponse(true, ObjDto, "Empleado obtenido correctamente.");
            }
            catch (Exception Ex)
            {
                _ObjLogger.LogError(Ex, $"Error al obtener empleado ID: {IntId}");
                return FncJsonResponse(false, null, "Error interno al obtener empleado.", Ex.Message, 500);
            }
        }

        [HttpPut("{IntId:int}")]
        public async Task<IActionResult> FncUpdate(int IntId, [FromBody] EmployeeUpdateDto ObjDto)
        {
            if (!ModelState.IsValid) return FncJsonResponse(false, null, "Datos de entrada inválidos.", "Errores de validación en el modelo.", 400);
            try
            {
                var ObjEntity = await _ObjDbContext.Employees.FirstOrDefaultAsync(Emp => Emp.BusinessEntityID == IntId);
                if (ObjEntity == null) return FncJsonResponse(false, null, $"No existe empleado con ID: {IntId}", null, 404);
                ObjEntity.NationalIDNumber = ObjDto.StrNationalIDNumber;
                ObjEntity.LoginID = ObjDto.StrLoginID;
                ObjEntity.JobTitle = ObjDto.StrJobTitle;
                ObjEntity.BirthDate = ObjDto.DtmBirthDate;
                ObjEntity.MaritalStatus = ObjDto.StrMaritalStatus;
                ObjEntity.Gender = ObjDto.StrGender;
                ObjEntity.HireDate = ObjDto.DtmHireDate;
                ObjEntity.SalariedFlag = ObjDto.BlnSalariedFlag;
                ObjEntity.VacationHours = ObjDto.IntVacationHours;
                ObjEntity.SickLeaveHours = ObjDto.IntSickLeaveHours;
                ObjEntity.CurrentFlag = ObjDto.BlnCurrentFlag;
                ObjEntity.ModifiedDate = DateTime.UtcNow;
                try { await _ObjDbContext.SaveChangesAsync(); }
                catch (DbUpdateException DbEx)
                {
                    string StrDetalle = DbEx.InnerException?.Message ?? DbEx.Message;
                    bool BlnDuplicado = StrDetalle.Contains("AK_Employee_LoginID") || StrDetalle.Contains("AK_Employee_NationalIDNumber");
                    string StrMsg = BlnDuplicado ? "El LoginID o NationalIDNumber ya existe." : "Error al actualizar empleado. Verifique los datos.";
                    return FncJsonResponse(false, null, StrMsg, StrDetalle, 500);
                }
                return FncJsonResponse(true, null, "Empleado actualizado correctamente.");
            }
            catch (Exception Ex)
            {
                _ObjLogger.LogError(Ex, $"Error al actualizar empleado ID: {IntId}");
                return FncJsonResponse(false, null, "Error interno al actualizar empleado.", Ex.Message, 500);
            }
        }

        [HttpDelete("{IntId:int}")]
        public async Task<IActionResult> FncDelete(int IntId)
        {
            try
            {
                var ObjEntity = await _ObjDbContext.Employees.FirstOrDefaultAsync(Emp => Emp.BusinessEntityID == IntId);
                if (ObjEntity == null) return FncJsonResponse(false, null, $"No existe empleado con ID: {IntId}", null, 404);
                _ObjDbContext.Employees.Remove(ObjEntity);
                try { await _ObjDbContext.SaveChangesAsync(); }
                catch (DbUpdateException DbEx)
                {
                    string StrDetalle = DbEx.InnerException?.Message ?? DbEx.Message;
                    return FncJsonResponse(false, null, "Error al eliminar empleado. Puede tener registros relacionados.", StrDetalle, 500);
                }
                return FncJsonResponse(true, null, "Empleado eliminado correctamente.");
            }
            catch (Exception Ex)
            {
                _ObjLogger.LogError(Ex, $"Error al eliminar empleado ID: {IntId}");
                return FncJsonResponse(false, null, "Error interno al eliminar empleado.", Ex.Message, 500);
            }
        }

        [HttpPost("full")]
        public async Task<IActionResult> FncCreateEmployeeFull([FromBody] CreateEmployeeFullDto ObjDto)
        {
            if (!ModelState.IsValid) return FncJsonResponse(false, null, "Datos de entrada inválidos.", "Errores de validación en el modelo.", 400);
            try
            {
                var StrConn = _ObjConfiguration.GetConnectionString("DefaultConnection");
                using var ObjConn = new SqlConnection(StrConn);
                using var ObjCmd = new SqlCommand("HumanResources.usp_CreateEmployee", ObjConn) { CommandType = CommandType.StoredProcedure, CommandTimeout = 30 };
                ObjCmd.Parameters.AddWithValue("@PersonType", ObjDto.StrPersonType);
                ObjCmd.Parameters.AddWithValue("@NameStyle", ObjDto.BlnNameStyle);
                ObjCmd.Parameters.AddWithValue("@Title", (object?)ObjDto.StrTitle ?? DBNull.Value);
                ObjCmd.Parameters.AddWithValue("@FirstName", ObjDto.StrFirstName);
                ObjCmd.Parameters.AddWithValue("@MiddleName", (object?)ObjDto.StrMiddleName ?? DBNull.Value);
                ObjCmd.Parameters.AddWithValue("@LastName", ObjDto.StrLastName);
                ObjCmd.Parameters.AddWithValue("@Suffix", (object?)ObjDto.StrSuffix ?? DBNull.Value);
                ObjCmd.Parameters.AddWithValue("@EmailPromotion", ObjDto.IntEmailPromotion);
                ObjCmd.Parameters.AddWithValue("@AdditionalContactInfo", (object?)ObjDto.StrAdditionalContactInfo ?? DBNull.Value);
                ObjCmd.Parameters.AddWithValue("@Demographics", (object?)ObjDto.StrDemographics ?? DBNull.Value);
                ObjCmd.Parameters.AddWithValue("@NationalIDNumber", ObjDto.StrNationalIDNumber);
                ObjCmd.Parameters.AddWithValue("@LoginID", ObjDto.StrLoginID);
                ObjCmd.Parameters.AddWithValue("@OrganizationNode", (object?)ObjDto.StrOrganizationNode ?? DBNull.Value);
                ObjCmd.Parameters.AddWithValue("@JobTitle", ObjDto.StrJobTitle);
                ObjCmd.Parameters.AddWithValue("@BirthDate", ObjDto.DtmBirthDate);
                ObjCmd.Parameters.AddWithValue("@MaritalStatus", ObjDto.StrMaritalStatus);
                ObjCmd.Parameters.AddWithValue("@Gender", ObjDto.StrGender);
                ObjCmd.Parameters.AddWithValue("@HireDate", ObjDto.DtmHireDate);
                ObjCmd.Parameters.AddWithValue("@SalariedFlag", ObjDto.BlnSalariedFlag);
                ObjCmd.Parameters.AddWithValue("@VacationHours", ObjDto.IntVacationHours);
                ObjCmd.Parameters.AddWithValue("@SickLeaveHours", ObjDto.IntSickLeaveHours);
                ObjCmd.Parameters.AddWithValue("@CurrentFlag", ObjDto.BlnCurrentFlag);
                try { await ObjConn.OpenAsync(); }
                catch (SqlException SqlEx) { return FncJsonResponse(false, null, "Error de conexión con la base de datos.", SqlEx.Message, 500); }
                using var ObjReader = await ObjCmd.ExecuteReaderAsync();
                if (!ObjReader.HasRows) return FncJsonResponse(false, null, "Error al crear empleado. Verifique los datos.", null, 500);
                var DctResult = new Dictionary<string, object?>();
                if (await ObjReader.ReadAsync())
                    for (int IntIdx = 0; IntIdx < ObjReader.FieldCount; IntIdx++) DctResult[ObjReader.GetName(IntIdx)] = ObjReader.GetValue(IntIdx);
                return FncJsonResponse(true, DctResult, "Empleado y persona creados correctamente.", null, 201);
            }
            catch (SqlException SqlEx)
            {
                string StrDetalle = SqlEx.Message;
                bool BlnDuplicado = StrDetalle.Contains("AK_Employee_LoginID") || StrDetalle.Contains("AK_Employee_NationalIDNumber");
                string StrMsg = BlnDuplicado ? "El LoginID o NationalIDNumber ya existe." : "Error en base de datos. Verifique los datos.";
                return FncJsonResponse(false, null, StrMsg, StrDetalle, 500);
            }
            catch (Exception Ex)
            {
                return FncJsonResponse(false, null, "Error interno al crear empleado/persona.", Ex.Message, 500);
            }
        }

        /// <summary>
        /// Obtiene los empleados con más tiempo en su departamento actual
        /// </summary>
        /// <param name="IntCantidad">Número de empleados a devolver</param>
        /// <returns>Lista de empleados ordenados por tiempo en departamento (descendente)</param>
        [HttpGet("department/{IntCantidad:int}")]
        public async Task<IActionResult> FncGetEmployeesByDepartmentTime(int IntCantidad)
        {
            try
            {
                // Validar parámetro de entrada
                if (IntCantidad <= 0)
                {
                    return FncJsonResponse(false, null, "El parámetro cantidad debe ser mayor que 0.", null, 400);
                }

                var StrConn = _ObjConfiguration.GetConnectionString("DefaultConnection");
                using var ObjConn = new SqlConnection(StrConn);
                using var ObjCmd = new SqlCommand("HumanResources.sp_DepartamentoEmpleado", ObjConn) 
                { 
                    CommandType = CommandType.StoredProcedure, 
                    CommandTimeout = 30 
                };

                // Agregar parámetro
                ObjCmd.Parameters.AddWithValue("@IntCantidad", IntCantidad);

                try
                {
                    await ObjConn.OpenAsync();
                }
                catch (SqlException SqlEx)
                {
                    _ObjLogger.LogError(SqlEx, "Error de conexión con la base de datos");
                    return FncJsonResponse(false, null, "Error de conexión con la base de datos.", SqlEx.Message, 500);
                }

                using var ObjReader = await ObjCmd.ExecuteReaderAsync();
                var LstEmployees = new List<EmployeeDepartmentDto>();

                while (await ObjReader.ReadAsync())
                {
                    var ObjEmployee = new EmployeeDepartmentDto
                    {
                        IntBusinessEntityID = ObjReader.GetInt32("IntBusinessEntityID"),
                        StrFullName = ObjReader.GetString("StrFullName"),
                        StrFirstName = ObjReader.GetString("StrFirstName"),
                        StrMiddleName = ObjReader.IsDBNull("StrMiddleName") ? null : ObjReader.GetString("StrMiddleName"),
                        StrLastName = ObjReader.GetString("StrLastName"),
                        StrDepartmentName = ObjReader.GetString("StrDepartmentName"),
                        IntDepartmentID = (short)ObjReader["IntDepartmentID"],
                        DtmStartDate = ObjReader.GetDateTime("DtmStartDate"),
                        IntDaysInDepartment = ObjReader.GetInt32("IntDaysInDepartment"),
                        IntYearsInDepartment = ObjReader.GetInt32("IntYearsInDepartment"),
                        StrJobTitle = ObjReader.GetString("StrJobTitle"),
                        DtmHireDate = ObjReader.GetDateTime("DtmHireDate")
                    };
                    LstEmployees.Add(ObjEmployee);
                }

                _ObjLogger.LogInformation($"Se obtuvieron {LstEmployees.Count} empleados con más tiempo en departamento");
                return FncJsonResponse(true, LstEmployees, "Empleados con más tiempo en departamento obtenidos correctamente.");
            }
            catch (SqlException SqlEx)
            {
                _ObjLogger.LogError(SqlEx, "Error SQL al obtener empleados por tiempo en departamento");
                return FncJsonResponse(false, null, "Error en base de datos.", SqlEx.Message, 500);
            }
            catch (Exception Ex)
            {
                _ObjLogger.LogError(Ex, "Error al obtener empleados por tiempo en departamento");
                return FncJsonResponse(false, null, "Error interno al obtener empleados por tiempo en departamento.", Ex.Message, 500);
            }
        }
    }
}