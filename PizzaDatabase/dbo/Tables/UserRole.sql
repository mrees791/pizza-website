CREATE TABLE [dbo].[UserRole]
(
  [Id] INT NOT NULL IDENTITY,
  [UserId] INT NOT NULL,
  [Name] VARCHAR(50) NOT NULL,
  PRIMARY KEY ([Id]),
  FOREIGN KEY ([UserId]) REFERENCES [dbo].[User](Id)
);