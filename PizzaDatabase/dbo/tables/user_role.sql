CREATE TABLE [dbo].[user_role]
(
  [user_role_id] INT NOT NULL IDENTITY,
  [user_id] INT NOT NULL,
  [role_name] VARCHAR(50) NOT NULL,
  PRIMARY KEY ([user_role_id]),
  FOREIGN KEY ([user_id]) REFERENCES [dbo].[user](user_id)
);