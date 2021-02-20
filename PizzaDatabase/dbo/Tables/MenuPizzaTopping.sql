CREATE TABLE dbo.MenuPizzaTopping
(
  Id INT NOT NULL IDENTITY,
  MenuPizzaId INT NOT NULL,
  ToppingHalf NVARCHAR(50) NOT NULL,
  ToppingAmount NVARCHAR(50) NOT NULL,
  MenuPizzaToppingTypeId INT NOT NULL,
  PRIMARY KEY (Id),
  CONSTRAINT FK_MenuPizzaTopping_MenuPizza FOREIGN KEY (MenuPizzaId) REFERENCES MenuPizza(Id),
  CONSTRAINT FK_MenuPizzaTopping_MenuPizzaToppingType FOREIGN KEY (MenuPizzaToppingTypeId) REFERENCES MenuPizzaToppingType(Id)
)