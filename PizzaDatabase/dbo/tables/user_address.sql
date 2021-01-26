CREATE TABLE [dbo].[user_address]
(
  [address_id] INT NOT NULL IDENTITY,
  [user_id] INT NOT NULL,
  [address_type] VARCHAR(50) NOT NULL,
  [address_name] VARCHAR(50) NOT NULL,
  [street_address] VARCHAR(50) NOT NULL,
  [city] VARCHAR(50) NOT NULL,
  [state] VARCHAR(50) NOT NULL,
  [zip_code] VARCHAR(5) NOT NULL,
  [phone_number] VARCHAR(10) NOT NULL,
  PRIMARY KEY ([address_id]),
  FOREIGN KEY ([user_id]) REFERENCES [dbo].[user](user_id)
);