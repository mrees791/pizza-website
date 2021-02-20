CREATE TABLE dbo.DeliveryInfo
(
  Id INT NOT NULL IDENTITY,
  DateOfDelivery DATETIME NOT NULL,
  DeliveryAddressType NVARCHAR(50) NOT NULL,
  DeliveryAddressName NVARCHAR(50) NOT NULL,
  DeliveryStreetAddress NVARCHAR(50) NOT NULL,
  DeliveryCity NVARCHAR(50) NOT NULL,
  DeliveryState NVARCHAR(2) NOT NULL,
  DeliveryZipCode NVARCHAR(5) NOT NULL,
  DeliveryPhoneNumber NVARCHAR(MAX) NOT NULL,
  PRIMARY KEY (Id)
)