﻿CREATE TABLE dbo.Employee
(
  Id NVARCHAR(256) NOT NULL UNIQUE,
  UserId NVARCHAR(256) NOT NULL UNIQUE,
  CurrentlyEmployed BIT NOT NULL,
  PRIMARY KEY (Id),
  CONSTRAINT FK_Employee_SiteUser FOREIGN KEY (UserId) REFERENCES SiteUser(Id)
)