namespace AdventureWorks.Enterprise.Api.Entities
{
    public class Employee
    {
        public int BusinessEntityID { get; set; }
        public string NationalIDNumber { get; set; } = string.Empty;
        public string LoginID { get; set; } = string.Empty;
        public byte[]? OrganizationNode { get; set; } // hierarchyid as varbinary
        public int? OrganizationLevel { get; set; }   // computed column
        public string JobTitle { get; set; } = string.Empty;
        public DateOnly BirthDate { get; set; }
        public string MaritalStatus { get; set; } = string.Empty; // nchar(1)
        public string Gender { get; set; } = string.Empty;        // nchar(1)
        public DateOnly HireDate { get; set; }
        public bool SalariedFlag { get; set; }
        public short VacationHours { get; set; }
        public short SickLeaveHours { get; set; }
        public bool CurrentFlag { get; set; }
        public Guid RowGuid { get; set; }
        public DateTime ModifiedDate { get; set; }
    }
}
