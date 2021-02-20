CREATE TABLE dbo.MenuPizza
(
  Id INT NOT NULL IDENTITY,
  SortOrder INT NOT NULL,
  CategoryName NVARCHAR(50) NOT NULL,
  AvailableForPurchase BIT NOT NULL,
  PizzaName NVARCHAR(100) NOT NULL,
  Description NVARCHAR(512) NOT NULL,
  Size NVARCHAR(512) NOT NULL,
  MenuPizzaCrustId INT NOT NULL,
  MenuPizzaSauceId INT NOT NULL,
  SauceAmount NVARCHAR(50) NOT NULL,
  MenuPizzaCheeseId INT NOT NULL,
  CheeseAmount NVARCHAR(50) NOT NULL,
  MenuPizzaCrustFlavorId INT NOT NULL,
  PRIMARY KEY (Id),
  CONSTRAINT FK_MenuPizza_MenuPizzaCrust FOREIGN KEY (MenuPizzaCrustId) REFERENCES MenuPizzaCrust(Id),
  CONSTRAINT FK_MenuPizza_MenuPizzaSauce FOREIGN KEY (MenuPizzaSauceId) REFERENCES MenuPizzaSauce(Id),
  CONSTRAINT FK_MenuPizza_MenuPizzaCheese FOREIGN KEY (MenuPizzaCheeseId) REFERENCES MenuPizzaCheese(Id),
  CONSTRAINT FK_MenuPizza_MenuPizzaCrustFlavor FOREIGN KEY (MenuPizzaCrustFlavorId) REFERENCES MenuPizzaCrustFlavor(Id)
)