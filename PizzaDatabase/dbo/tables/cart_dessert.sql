CREATE TABLE [dbo].[cart_dessert]
(
  [cart_dessert_id] INT NOT NULL IDENTITY,
  [cart_id] INT NOT NULL,
  [menu_dessert_id] INT NOT NULL,
  [price_per_item] DECIMAL(20,2) NOT NULL,
  [quantity] INT NOT NULL,
  PRIMARY KEY ([cart_dessert_id]),
  FOREIGN KEY ([cart_id]) REFERENCES cart(cart_id),
  FOREIGN KEY ([menu_dessert_id]) REFERENCES menu_dessert(menu_dessert_id)
);