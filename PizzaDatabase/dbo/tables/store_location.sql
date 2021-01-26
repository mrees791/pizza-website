CREATE TABLE [dbo].[store_location]
(
  [store_id] INT NOT NULL IDENTITY,
  [store_name] VARCHAR(50) NOT NULL,
  [street_address] VARCHAR(50) NOT NULL,
  [city] VARCHAR(50) NOT NULL,
  [state] VARCHAR(50) NOT NULL,
  [zip_code] VARCHAR(5) NOT NULL,
  [phone_number] VARCHAR(10) NOT NULL,
  [is_active_location] BIT NOT NULL,
  PRIMARY KEY ([store_id])
);