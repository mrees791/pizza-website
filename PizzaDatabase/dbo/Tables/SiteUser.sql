﻿CREATE TABLE dbo.SiteUser
(
  Id NVARCHAR(256) NOT NULL UNIQUE,
  CurrentCartId INT NOT NULL,
  ConfirmOrderCartId INT NOT NULL,
  OrderConfirmationId INT NOT NULL,
  IsBanned BIT NOT NULL,
  ZipCode NVARCHAR(5),
  Email NVARCHAR(256),
  EmailConfirmed BIT NOT NULL,
  PasswordHash NVARCHAR(MAX),
  SecurityStamp NVARCHAR(MAX),
  PhoneNumber NVARCHAR(MAX),
  PhoneNumberConfirmed BIT NOT NULL,
  TwoFactorEnabled BIT NOT NULL,
  LockoutEndDateUtc DATETIME,
  LockoutEnabled BIT NOT NULL,
  AccessFailedCount INT NOT NULL,
  PRIMARY KEY (Id),
  CONSTRAINT FK_SiteUser_Cart_Current FOREIGN KEY (CurrentCartId) REFERENCES Cart(Id),
  CONSTRAINT FK_SiteUser_Cart_Confirm_Order FOREIGN KEY (ConfirmOrderCartId) REFERENCES Cart(Id)
)