CREATE TABLE [dbo].[EmployeeLocation]
(
  [Id] INT NOT NULL IDENTITY,
  [EmployeeId] INT NOT NULL,
  [StoreId] INT NOT NULL,
  PRIMARY KEY ([Id]),
  FOREIGN KEY ([EmployeeId]) REFERENCES Employee(Id),
  FOREIGN KEY ([StoreId]) REFERENCES StoreLocation(Id),
);