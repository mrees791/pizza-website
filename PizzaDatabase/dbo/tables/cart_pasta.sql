CREATE TABLE [dbo].[cart_pasta]
(
  [cart_pasta_id] INT NOT NULL IDENTITY,
  [cart_id] INT NOT NULL,
  [menu_pasta_id] INT NOT NULL,
  [price_per_item] DECIMAL(20,2) NOT NULL,
  [quantity] INT NOT NULL,
  PRIMARY KEY ([cart_pasta_id]),
  FOREIGN KEY ([cart_id]) REFERENCES cart(cart_id),
  FOREIGN KEY ([menu_pasta_id]) REFERENCES menu_pasta(menu_pasta_id)
);