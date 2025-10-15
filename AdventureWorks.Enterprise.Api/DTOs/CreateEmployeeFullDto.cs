using System;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Enterprise.Api.DTOs
{
    /// <summary>
    /// DTO para la creación completa de un empleado y persona asociada usando el SP HumanResources.usp_CreateEmployee.
    /// </summary>
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
}
