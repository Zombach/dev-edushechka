﻿CREATE TABLE [dbo].[StudentRating]
(
	[Id] INT NOT NULL IDENTITY(1,1), 
    [UserId] INT NOT NULL, 
    [GroupId] INT NOT NULL, 
    [RatingTypeId] INT NOT NULL, 
    [Rating] INT NOT NULL,
    [ReportingPeriodNumber] INT NOT NULL,
    CONSTRAINT [PK_StudentRating] PRIMARY KEY CLUSTERED
  (
  [Id] ASC
  ) WITH (IGNORE_DUP_KEY = OFF) 
     
)
GO

ALTER TABLE [dbo].[StudentRating] WITH CHECK ADD CONSTRAINT [StudentRating_fk0] FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [dbo].[StudentRating] CHECK CONSTRAINT [StudentRating_fk0]
GO
ALTER TABLE [dbo].[StudentRating] WITH CHECK ADD CONSTRAINT [StudentRating_fk1] FOREIGN KEY ([RatingTypeId]) REFERENCES [RatingType]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [dbo].[StudentRating] CHECK CONSTRAINT [StudentRating_fk1]
GO
ALTER TABLE [dbo].[StudentRating] WITH CHECK ADD CONSTRAINT [StudentRating_fk2] FOREIGN KEY ([GroupId]) REFERENCES [Group]([Id])
ON UPDATE NO ACTION
GO
ALTER TABLE [dbo].[StudentRating] CHECK CONSTRAINT [StudentRating_fk2]
GO
ALTER TABLE [dbo].[StudentRating] ADD CONSTRAINT rating_check CHECK(Rating >= 1 and Rating <= 100)
GO
ALTER TABLE [dbo].[StudentRating]
ADD CONSTRAINT UC_UserId_GroupId_RatingTypeId_ReportingPeriodNumber  UNIQUE(UserId, GroupId, RatingTypeId, ReportingPeriodNumber)
GO
