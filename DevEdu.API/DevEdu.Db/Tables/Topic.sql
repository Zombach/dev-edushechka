﻿CREATE TABLE [Topic] (
	Id int NOT NULL IDENTITY(1,1),
	Name nvarchar(255) NOT NULL,
	Duration int NOT NULL,
	IsDeleted bit NOT NULL DEFAULT '0',
  CONSTRAINT [PK_TOPIC] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)