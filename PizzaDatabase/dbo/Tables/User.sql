CREATE TABLE [dbo].[User]
(
  [Id] INT NOT NULL IDENTITY,
  [UserName] VARCHAR(20) NOT NULL,
  [Email] VARCHAR(256) NOT NULL,
  [PasswordHash] NVARCHAR(MAX) NOT NULL,
  [CurrentCartId] INT NOT NULL,
  [ConfirmOrderCartId] INT NOT NULL,
  [IsBanned] BIT NOT NULL,
  [EmailConfirmed] BIT NOT NULL,
  [PhoneNumber] VARCHAR(10),
  [PhoneNumberConfirmed] BIT NOT NULL,
  [ZipCode] VARCHAR(5),
  PRIMARY KEY ([Id]),
  FOREIGN KEY ([CurrentCartId]) REFERENCES Cart(Id),
  FOREIGN KEY ([ConfirmOrderCartId]) REFERENCES Cart(Id)
);