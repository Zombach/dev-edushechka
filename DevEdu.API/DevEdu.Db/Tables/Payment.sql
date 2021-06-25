﻿CREATE TABLE [Payment] (
	Id int NOT NULL,
	Date datetime NOT NULL,
	Sum decimal(6,2) NOT NULL,
	UserId int NOT NULL,
	IsPaid bit NOT NULL DEFAULT '0',
	IsDeleted bit NOT NULL DEFAULT '0',
  CONSTRAINT [PK_PAYMENT] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
go

ALTER TABLE [Payment] WITH CHECK ADD CONSTRAINT [Payment_fk0] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Payment] CHECK CONSTRAINT [Payment_fk0]
GO