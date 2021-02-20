CREATE TABLE dbo.CartPizzaTopping
(
  Id INT NOT NULL IDENTITY,
  CartItemId INT NOT NULL,
  ToppingHalf NVARCHAR(50) NOT NULL,
  ToppingAmount NVARCHAR(50) NOT NULL,
  MenuPizzaToppingTypeId INT NOT NULL,
  PRIMARY KEY (Id),
  CONSTRAINT FK_CartPizzaTopping_CartItem FOREIGN KEY (CartItemId) REFERENCES CartItem(Id) ON DELETE CASCADE,
  CONSTRAINT FK_CartPizzaTopping_MenuPizzaToppingType FOREIGN KEY (MenuPizzaToppingTypeId) REFERENCES MenuPizzaToppingType(Id)
)