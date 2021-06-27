﻿CREATE TABLE [Course_Topic] (
	Id int NOT NULL IDENTITY(1,1),
	CourseId int NOT NULL,
	TopicId int NOT NULL,
	Position int NOT NULL,
  CONSTRAINT [PK_COURSE_TOPIC] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
go

ALTER TABLE [Course_Topic] WITH CHECK ADD CONSTRAINT [Course_Topic_fk0] FOREIGN KEY ([CourseId]) REFERENCES [Course]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Course_Topic] CHECK CONSTRAINT [Course_Topic_fk0]
GO
ALTER TABLE [Course_Topic] WITH CHECK ADD CONSTRAINT [Course_Topic_fk1] FOREIGN KEY ([TopicId]) REFERENCES [Topic]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Course_Topic] CHECK CONSTRAINT [Course_Topic_fk1]
GO