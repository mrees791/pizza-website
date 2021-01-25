CREATE TABLE [dbo].[cart_wings]
(
  [cart_wings_id] INT NOT NULL IDENTITY,
  [cart_id] INT NOT NULL,
  [menu_wings_id] INT NOT NULL,
  [menu_wings_sauce_id] INT NOT NULL,
  [piece_amount] INT NOT NULL,
  [price_per_item] DECIMAL(20,2) NOT NULL,
  [quantity] INT NOT NULL,
  PRIMARY KEY ([cart_wings_id]),
  FOREIGN KEY ([cart_id]) REFERENCES cart(cart_id),
  FOREIGN KEY ([menu_wings_id]) REFERENCES menu_wings(menu_wings_id),
  FOREIGN KEY ([menu_wings_sauce_id]) REFERENCES menu_wings_sauce(menu_wings_sauce_id)
);