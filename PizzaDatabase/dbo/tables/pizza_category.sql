CREATE TABLE [dbo].[pizza_category]
(
  [pizza_category_id] INT NOT NULL IDENTITY,
  [pizza_id] INT NOT NULL,
  [pizza_category_name] VARCHAR(50) NOT NULL,
  [available_for_purchase] BIT NOT NULL,
  [pizza_name] VARCHAR(100) NOT NULL,
  [description] VARCHAR(512) NOT NULL,
  PRIMARY KEY ([pizza_category_id]),
  FOREIGN KEY ([pizza_id]) REFERENCES pizza(pizza_id)
);