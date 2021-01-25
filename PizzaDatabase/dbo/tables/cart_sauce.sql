CREATE TABLE [dbo].[cart_sauce]
(
  [cart_sauce_id] INT NOT NULL IDENTITY,
  [cart_id] INT NOT NULL,
  [menu_sauce_id] INT NOT NULL,
  [price_per_item] DECIMAL(20,2) NOT NULL,
  [quantity] INT NOT NULL,
  PRIMARY KEY ([cart_sauce_id]),
  FOREIGN KEY ([cart_id]) REFERENCES cart(cart_id),
  FOREIGN KEY ([menu_sauce_id]) REFERENCES menu_sauce(menu_sauce_id)
);