﻿CREATE TABLE dbo.SiteRole
(
  Id INT NOT NULL IDENTITY,
  Name NVARCHAR(256) NOT NULL UNIQUE,
  PRIMARY KEY (Id)
)