﻿CREATE TABLE [dbo].[Employee]
(
  [Id] INT NOT NULL IDENTITY,
  [UserId] INT NOT NULL,
  [CurrentlyEmployed] BIT NOT NULL,
  PRIMARY KEY ([Id]),
  FOREIGN KEY ([UserId]) REFERENCES [dbo].[User](Id)
);