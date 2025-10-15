using System;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Enterprise.Api.DTOs
{
    /// <summary>
    /// DTO para los empleados con más tiempo en su departamento actual
    /// </summary>
    public class EmployeeDepartmentDto
    {
        public int IntBusinessEntityID { get; set; }
        public string StrFullName { get; set; } = string.Empty;
        public string StrFirstName { get; set; } = string.Empty;
        public string? StrMiddleName { get; set; }
        public string StrLastName { get; set; } = string.Empty;
        public string StrDepartmentName { get; set; } = string.Empty;
        public short IntDepartmentID { get; set; }
        public DateTime DtmStartDate { get; set; }
        public int IntDaysInDepartment { get; set; }
        public int IntYearsInDepartment { get; set; }
        public string StrJobTitle { get; set; } = string.Empty;
        public DateTime DtmHireDate { get; set; }
    }
}