IF OBJECT_ID('HumanResources.usp_CreateEmployee', 'P') IS NOT NULL
    DROP PROCEDURE HumanResources.usp_CreateEmployee;
GO
CREATE PROCEDURE HumanResources.usp_CreateEmployee
(
    @PersonType         nchar(2)       = N'EM',
    @NameStyle          bit            = 0,
    @Title              nvarchar(8)    = NULL,
    @FirstName          nvarchar(50),
    @MiddleName         nvarchar(50)   = NULL,
    @LastName           nvarchar(50),
    @Suffix             nvarchar(10)   = NULL,
    @EmailPromotion     int            = 0,
    @AdditionalContactInfo xml         = NULL,
    @Demographics         xml          = NULL,
    @NationalIDNumber   nvarchar(15),
    @LoginID            nvarchar(256),
    @OrganizationNode   hierarchyid    = NULL,
    @JobTitle           nvarchar(50),
    @BirthDate          date,
    @MaritalStatus      nchar(1),
    @Gender             nchar(1),
    @HireDate           date,
    @SalariedFlag       bit,
    @VacationHours      smallint,
    @SickLeaveHours     smallint,
    @CurrentFlag        bit
)
AS
BEGIN
    SET NOCOUNT ON;
    BEGIN TRY
        BEGIN TRAN;

        DECLARE @BusinessEntityID int;

        INSERT INTO Person.BusinessEntity DEFAULT VALUES;
        SET @BusinessEntityID = CAST(SCOPE_IDENTITY() AS int);

        INSERT INTO Person.Person
        (
            BusinessEntityID, PersonType, NameStyle, Title, FirstName, MiddleName, LastName, Suffix,
            EmailPromotion, AdditionalContactInfo, Demographics, rowguid, ModifiedDate
        )
        VALUES
        (
            @BusinessEntityID, @PersonType, @NameStyle, @Title, @FirstName, @MiddleName, @LastName, @Suffix,
            @EmailPromotion, @AdditionalContactInfo, @Demographics, NEWID(), SYSUTCDATETIME()
        );

        INSERT INTO HumanResources.Employee
        (
            BusinessEntityID, NationalIDNumber, LoginID, OrganizationNode, JobTitle, BirthDate,
            MaritalStatus, Gender, HireDate, SalariedFlag, VacationHours, SickLeaveHours,
            CurrentFlag, rowguid, ModifiedDate
        )
        VALUES
        (
            @BusinessEntityID, @NationalIDNumber, @LoginID, @OrganizationNode, @JobTitle, @BirthDate,
            @MaritalStatus, @Gender, @HireDate, @SalariedFlag, @VacationHours, @SickLeaveHours,
            @CurrentFlag, NEWID(), SYSUTCDATETIME()
        );

        COMMIT;

        -- Devuelve el registro completo
        SELECT
            be.BusinessEntityID,
            p.PersonType,
            p.NameStyle,
            p.Title,
            p.FirstName,
            p.MiddleName,
            p.LastName,
            p.Suffix,
            p.EmailPromotion,
            p.rowguid        AS PersonRowGuid,
            p.ModifiedDate   AS PersonModifiedDate,
            e.NationalIDNumber,
            e.LoginID,
            e.OrganizationNode,
            e.OrganizationNode.GetLevel() AS OrganizationLevel,
            e.JobTitle,
            e.BirthDate,
            e.MaritalStatus,
            e.Gender,
            e.HireDate,
            e.SalariedFlag,
            e.VacationHours,
            e.SickLeaveHours,
            e.CurrentFlag,
            e.rowguid        AS EmployeeRowGuid,
            e.ModifiedDate   AS EmployeeModifiedDate
        FROM Person.BusinessEntity       AS be
        JOIN Person.Person               AS p ON p.BusinessEntityID = be.BusinessEntityID
        JOIN HumanResources.Employee     AS e ON e.BusinessEntityID = be.BusinessEntityID
        WHERE be.BusinessEntityID = @BusinessEntityID;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0 ROLLBACK;
        THROW;
    END CATCH
END
GO