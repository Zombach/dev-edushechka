﻿CREATE PROCEDURE [dbo].[Task_Student_SelectById]
	@Id int
AS
BEGIN
	SELECT
		tstud.Id,
		tstud.TaskId,
		tstud.StudentId,
		tstud.Answer,
		tstud.CompletedDate,
		us.Id,
		us.Username,
		us.FirstName,
		us.LastName,
		us.Email,
		us.GitHubAccount,
		us.Photo,
		us.IsDeleted,
		t.Id,
		t.[Name],
		t.[Description],
		t.Links,
		t.IsRequired,
		t.IsDeleted,
		tstud.StatusId as Id
	FROM dbo.Task_Student tstud
		LEFT JOIN dbo.[User] us on us.Id = tstud.StatusId
		LEFT JOIN dbo.Task t on t.Id = tstud.TaskId
	WHERE tstud.Id =  @Id AND us.IsDeleted = 0
END