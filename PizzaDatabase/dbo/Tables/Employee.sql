CREATE TABLE dbo.Employee
(
  Id NVARCHAR(256) NOT NULL UNIQUE,
  UserId INT NOT NULL,
  CurrentlyEmployed BIT NOT NULL,
  PRIMARY KEY (EmployeeId),
  CONSTRAINT FK_Employee_SiteUser FOREIGN KEY (UserId) REFERENCES SiteUser(Id)
)