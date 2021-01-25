CREATE TABLE [dbo].[cart_dip]
(
  [cart_dip_id] INT NOT NULL IDENTITY,
  [cart_id] INT NOT NULL,
  [menu_dip_id] INT NOT NULL,
  [price_per_item] DECIMAL(20,2) NOT NULL,
  [quantity] INT NOT NULL,
  PRIMARY KEY ([cart_dip_id]),
  FOREIGN KEY ([cart_id]) REFERENCES cart(cart_id),
  FOREIGN KEY ([menu_dip_id]) REFERENCES menu_dip(menu_dip_id)
);