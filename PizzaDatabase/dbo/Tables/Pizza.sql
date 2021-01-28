CREATE TABLE [dbo].[Pizza]
(
  [Id] INT NOT NULL IDENTITY,
  [Size] VARCHAR(50) NOT NULL,
  [MenuPizzaCrustId] INT NOT NULL,
  [MenuPizzaSauceId] INT NOT NULL,
  [SauceAmount] VARCHAR(50) NOT NULL,
  [MenuPizzaCheeseId] INT NOT NULL,
  [CheeseAmount] VARCHAR(50) NOT NULL,
  [MenuPizzaCrustFlavorId] INT NOT NULL,
  PRIMARY KEY ([Id])
);