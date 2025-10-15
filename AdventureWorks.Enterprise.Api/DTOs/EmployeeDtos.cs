using System;
using System.ComponentModel.DataAnnotations;

namespace AdventureWorks.Enterprise.Api.DTOs
{
    public class EmployeeCreateDto
    {
        [Required]
        public int IntBusinessEntityID { get; set; }

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

    public class EmployeeReadDto
    {
        public int IntBusinessEntityID { get; set; }
        public string StrNationalIDNumber { get; set; } = string.Empty;
        public string StrLoginID { get; set; } = string.Empty;
        public string StrJobTitle { get; set; } = string.Empty;
        public DateOnly DtmBirthDate { get; set; }
        public string StrMaritalStatus { get; set; } = string.Empty;
        public string StrGender { get; set; } = string.Empty;
        public DateOnly DtmHireDate { get; set; }
        public bool BlnSalariedFlag { get; set; }
        public short IntVacationHours { get; set; }
        public short IntSickLeaveHours { get; set; }
        public bool BlnCurrentFlag { get; set; }
        public Guid RowGuid { get; set; }
        public DateTime DtmModifiedDate { get; set; }
    }

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
    }
}
