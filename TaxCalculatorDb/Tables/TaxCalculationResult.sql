CREATE TABLE [dbo].[TaxCalculationResult]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [PostalCode] NVARCHAR(10) NOT NULL, 
	[TaxCalculationType] tinyint NOT NULL,
    [Salary] DECIMAL(19, 5) NOT NULL, 
    [DateCalculated] DATETIME NOT NULL DEFAULT getdate(), 
    [TaxAmount] DECIMAL(19, 5) NOT NULL
)
