CREATE TABLE [dbo].[menu_dessert]
(
  [menu_dessert_id] INT NOT NULL IDENTITY,
  [available_for_purchase] BIT NOT NULL,
  [name] VARCHAR(100) NOT NULL,
  [price] DECIMAL(20,2) NOT NULL,
  [description] VARCHAR(512) NOT NULL,
  [item_details] VARCHAR(512) NOT NULL,
  [has_menu_icon] BIT NOT NULL,
  [menu_icon_file] VARCHAR(50) NOT NULL,
  PRIMARY KEY ([menu_dessert_id])
);