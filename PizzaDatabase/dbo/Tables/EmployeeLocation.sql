CREATE TABLE dbo.EmployeeLocation
(
  Id INT NOT NULL IDENTITY,
  EmployeeId NVARCHAR(256) NOT NULL,
  StoreId INT NOT NULL,
  PRIMARY KEY (Id),
  CONSTRAINT FK_EmployeeLocation_Employee FOREIGN KEY (EmployeeId) REFERENCES Employee(Id),
  CONSTRAINT FK_EmployeeLocation_StoreLocation FOREIGN KEY (StoreId) REFERENCES StoreLocation(Id)
)