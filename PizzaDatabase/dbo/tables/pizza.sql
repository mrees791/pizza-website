CREATE TABLE [dbo].[pizza]
(
  [pizza_id] INT NOT NULL IDENTITY,
  [size] VARCHAR(50) NOT NULL,
  [menu_pizza_crust_id] INT NOT NULL,
  [menu_pizza_sauce_id] INT NOT NULL,
  [sauce_amount] VARCHAR(50) NOT NULL,
  [menu_pizza_cheese_id] INT NOT NULL,
  [cheese_amount] VARCHAR(50) NOT NULL,
  [menu_pizza_crust_flavor_id] INT NOT NULL,
  PRIMARY KEY ([pizza_id]),
  FOREIGN KEY ([menu_pizza_crust_id]) REFERENCES menu_pizza_crust(menu_pizza_crust_id),
  FOREIGN KEY ([menu_pizza_sauce_id]) REFERENCES menu_pizza_sauce(menu_pizza_sauce_id),
  FOREIGN KEY ([menu_pizza_cheese_id]) REFERENCES menu_pizza_cheese(menu_pizza_cheese_id),
  FOREIGN KEY ([menu_pizza_crust_flavor_id]) REFERENCES menu_pizza_crust_flavor(menu_pizza_crust_flavor_id)
);