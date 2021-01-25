CREATE TABLE [dbo].[employee_location]
(
  [employee_location_id] INT NOT NULL IDENTITY,
  [employee_id] INT NOT NULL,
  [store_id] INT NOT NULL,
  PRIMARY KEY ([employee_location_id]),
  FOREIGN KEY ([employee_id]) REFERENCES employee(employee_id),
  FOREIGN KEY ([store_id]) REFERENCES store_location(store_id)
);