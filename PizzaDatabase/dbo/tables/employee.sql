CREATE TABLE [dbo].[employee]
(
  [employee_id] INT NOT NULL IDENTITY,
  [user_id] INT NOT NULL,
  [currently_employed] BIT NOT NULL,
  PRIMARY KEY ([employee_id]),
  FOREIGN KEY ([user_id]) REFERENCES [dbo].[user](user_id)
);