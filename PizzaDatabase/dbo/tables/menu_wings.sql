CREATE TABLE [dbo].[menu_wings]
(
  [menu_wings_id] INT NOT NULL IDENTITY,
  [available_for_purchase] BIT NOT NULL,
  [name] VARCHAR(100) NOT NULL,
  [price_6_piece] DECIMAL(20,2) NOT NULL,
  [price_12_piece] DECIMAL(20,2) NOT NULL,
  [price_18_piece] DECIMAL(20,2) NOT NULL,
  [description] VARCHAR(512) NOT NULL,
  [has_menu_icon] BIT NOT NULL,
  [menu_icon_file] VARCHAR(50) NOT NULL,
  PRIMARY KEY ([menu_wings_id])
);