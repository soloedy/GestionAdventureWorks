using System;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Enterprise.App.Models
{
    public class EmployeeFullReadDto
    {
        public int IntBusinessEntityID { get; set; }
        public string StrPersonType { get; set; } = string.Empty;
        public bool BlnNameStyle { get; set; }
        public string? StrTitle { get; set; }
        public string StrFirstName { get; set; } = string.Empty;
        public string? StrMiddleName { get; set; }
        public string StrLastName { get; set; } = string.Empty;
        public string? StrSuffix { get; set; }
        public int IntEmailPromotion { get; set; }
        public Guid PersonRowGuid { get; set; }
        public DateTime PersonModifiedDate { get; set; }
        public string StrNationalIDNumber { get; set; } = string.Empty;
        public string StrLoginID { get; set; } = string.Empty;
        public string? StrOrganizationNode { get; set; }
        public short? IntOrganizationLevel { get; set; }
        public string StrJobTitle { get; set; } = string.Empty;
        public DateTime DtmBirthDate { get; set; }
        public string StrMaritalStatus { get; set; } = string.Empty;
        public string StrGender { get; set; } = string.Empty;
        public DateTime DtmHireDate { get; set; }
        public bool BlnSalariedFlag { get; set; }
        public short IntVacationHours { get; set; }
        public short IntSickLeaveHours { get; set; }
        public bool BlnCurrentFlag { get; set; }
        public Guid EmployeeRowGuid { get; set; }
        public DateTime EmployeeModifiedDate { get; set; }

        public string StrFullName => $"{StrFirstName} {(!string.IsNullOrEmpty(StrMiddleName) ? StrMiddleName + " " : "")}{StrLastName}";
    }

    public class EmployeeUpdateDto
    {
        [Required, StringLength(15)]
        public string StrNationalIDNumber { get; set; } = string.Empty;

        [Required, StringLength(256)]
        public string StrLoginID { get; set; } = string.Empty;

        [Required, StringLength(50)]
        public string StrJobTitle { get; set; } = string.Empty;

        [Required]
        public DateOnly DtmBirthDate { get; set; }

        [Required, StringLength(1)]
        public string StrMaritalStatus { get; set; } = string.Empty;

        [Required, StringLength(1)]
        public string StrGender { get; set; } = string.Empty;

        [Required]
        public DateOnly DtmHireDate { get; set; }

        public bool BlnSalariedFlag { get; set; }

        public short IntVacationHours { get; set; }

        public short IntSickLeaveHours { get; set; }

        public bool BlnCurrentFlag { get; set; }
    }

    public class CreateEmployeeFullDto
    {
        // Person.Person
        [Required, StringLength(2)]
        public string StrPersonType { get; set; } = "EM";
        
        public bool BlnNameStyle { get; set; } = false;
        
        [StringLength(8)]
        public string? StrTitle { get; set; }
        
        [Required, StringLength(50)]
        public string StrFirstName { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? StrMiddleName { get; set; }
        
        [Required, StringLength(50)]
        public string StrLastName { get; set; } = string.Empty;
        
        [StringLength(10)]
        public string? StrSuffix { get; set; }
        
        public int IntEmailPromotion { get; set; } = 0;
        
        public string? StrAdditionalContactInfo { get; set; }
        
        public string? StrDemographics { get; set; }

        // Employee
        [Required, StringLength(15)]
        public string StrNationalIDNumber { get; set; } = string.Empty;
        
        [Required, StringLength(256)]
        public string StrLoginID { get; set; } = string.Empty;
        
        public string? StrOrganizationNode { get; set; }
        
        [Required, StringLength(50)]
        public string StrJobTitle { get; set; } = string.Empty;
        
        [Required]
        public DateOnly DtmBirthDate { get; set; }
        
        [Required, StringLength(1)]
        public string StrMaritalStatus { get; set; } = string.Empty;
        
        [Required, StringLength(1)]
        public string StrGender { get; set; } = string.Empty;
        
        [Required]
        public DateOnly DtmHireDate { get; set; }
        
        public bool BlnSalariedFlag { get; set; }
        
        public short IntVacationHours { get; set; }
        
        public short IntSickLeaveHours { get; set; }
        
        public bool BlnCurrentFlag { get; set; }
    }

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