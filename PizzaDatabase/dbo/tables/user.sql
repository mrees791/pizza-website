CREATE TABLE [dbo].[user]
(
  [user_id] INT NOT NULL IDENTITY,
  [current_cart_id] INT NOT NULL,
  [email] VARCHAR(256) NOT NULL,
  [password_hash] NVARCHAR(MAX) NOT NULL,
  [is_banned] BIT NOT NULL,
  [email_confirmed] BIT NOT NULL,
  [phone_number] VARCHAR(10) NOT NULL,
  [phone_number_confirmed] BIT NOT NULL,
  [zip_code] VARCHAR(5) NOT NULL,
  PRIMARY KEY ([user_id])
);