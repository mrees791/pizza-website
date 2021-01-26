CREATE TABLE [dbo].[pizza_topping]
(
  [pizza_topping_id] INT NOT NULL IDENTITY,
  [pizza_id] INT NOT NULL,
  [topping_half] VARCHAR(50) NOT NULL,
  [menu_pizza_topping_id] INT NOT NULL,
  PRIMARY KEY ([pizza_topping_id]),
  FOREIGN KEY ([pizza_id]) REFERENCES pizza(pizza_id),
  FOREIGN KEY ([menu_pizza_topping_id]) REFERENCES menu_pizza_topping(menu_pizza_topping_id)
);