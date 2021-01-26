CREATE TABLE [dbo].[menu_drink]
(
  [menu_drink_id] INT NOT NULL IDENTITY,
  [available_for_purchase] BIT NOT NULL,
  [name] VARCHAR(100) NOT NULL,
  [available_in 20_oz] BIT NOT NULL,
  [available_in_2_liter] BIT NOT NULL,
  [available_in_2_pack_12_oz] BIT NOT NULL,
  [available_in_6_pack_12_oz] BIT NOT NULL,
  [price_20_oz] DECIMAL(20,2) NOT NULL,
  [price_2_liter] DECIMAL(20,2) NOT NULL,
  [price_2_pack_12_oz] DECIMAL(20,2) NOT NULL,
  [price_6_pack_12_oz] DECIMAL(20,2) NOT NULL,
  [description] VARCHAR(512) NOT NULL,
  [has_menu_icon] BIT NOT NULL,
  [menu_icon_file] VARCHAR(50) NOT NULL,
  PRIMARY KEY ([menu_drink_id])
);