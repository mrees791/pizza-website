CREATE TABLE [dbo].[menu_wings_sauce]
(
  [menu_wings_sauce_id] INT NOT NULL IDENTITY,
  [available_for_purchase] BIT NOT NULL,
  [name] VARCHAR(100) NOT NULL,
  [description] VARCHAR(512) NOT NULL,
  PRIMARY KEY ([menu_wings_sauce_id])
);