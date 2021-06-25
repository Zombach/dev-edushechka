﻿CREATE TABLE [Lesson_Comment] (
	Id int NOT NULL Identity,
	LessonId int NOT NULL,
	CommentId int NOT NULL,
  CONSTRAINT [PK_LESSON_COMMENT] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF)

)
go

ALTER TABLE [Lesson_Comment] WITH CHECK ADD CONSTRAINT [Lesson_Comment_fk0] FOREIGN KEY ([LessonId]) REFERENCES [Lesson]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Lesson_Comment] CHECK CONSTRAINT [Lesson_Comment_fk0]
GO
ALTER TABLE [Lesson_Comment] WITH CHECK ADD CONSTRAINT [Lesson_Comment_fk1] FOREIGN KEY ([CommentId]) REFERENCES [Comment]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [Lesson_Comment] CHECK CONSTRAINT [Lesson_Comment_fk1]
GO