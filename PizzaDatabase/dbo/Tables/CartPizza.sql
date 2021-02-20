CREATE TABLE dbo.CartPizza
(
  CartItemId INT NOT NULL,
  Size NVARCHAR(50) NOT NULL,
  MenuPizzaCrustId INT NOT NULL,
  MenuPizzaSauceId INT NOT NULL,
  SauceAmount NVARCHAR(50) NOT NULL,
  MenuPizzaCheeseId INT NOT NULL,
  CheeseAmount NVARCHAR(50) NOT NULL,
  MenuPizzaCrustFlavorId INT NOT NULL,
  CONSTRAINT FK_CartPizza_CartItem FOREIGN KEY (CartItemId) REFERENCES CartItem(Id) ON DELETE CASCADE,
  CONSTRAINT FK_CartPizza_MenuPizzaCrust FOREIGN KEY (MenuPizzaCrustId) REFERENCES MenuPizzaCrust(Id),
  CONSTRAINT FK_CartPizza_MenuPizzaSauce FOREIGN KEY (MenuPizzaSauceId) REFERENCES MenuPizzaSauce(Id),
  CONSTRAINT FK_CartPizza_MenuPizzaCheese FOREIGN KEY (MenuPizzaCheeseId) REFERENCES MenuPizzaCheese(Id),
  CONSTRAINT FK_CartPizza_MenuPizzaCrustFlavor FOREIGN KEY (MenuPizzaCrustFlavorId) REFERENCES MenuPizzaCrustFlavor(Id)
)
GO

CREATE UNIQUE CLUSTERED INDEX IX_CartPizza_Index ON dbo.CartPizza (CartItemId ASC)