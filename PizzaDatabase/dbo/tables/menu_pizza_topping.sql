CREATE TABLE [dbo].[menu_pizza_topping]
(
  [menu_pizza_topping_id] INT NOT NULL IDENTITY,
  [available_for_purchase] BIT NOT NULL,
  [name] VARCHAR(100) NOT NULL,
  [price_light] DECIMAL(20,2) NOT NULL,
  [price_regular] DECIMAL(20,2) NOT NULL,
  [price_extra] DECIMAL(20,2) NOT NULL,
  [pizza_topping_type] VARCHAR(50) NOT NULL,
  [description] VARCHAR(512) NOT NULL,
  [has_menu_icon] BIT NOT NULL,
  [menu_icon_file] VARCHAR(50) NOT NULL,
  [has_pizza_builder_image] BIT NOT NULL,
  [pizza_builder_image_file] VARCHAR(50) NOT NULL,
  PRIMARY KEY ([menu_pizza_topping_id])
);