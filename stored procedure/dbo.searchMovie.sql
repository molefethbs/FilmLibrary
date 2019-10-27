CREATE PROCEDURE dbo.searchMovie
	@title varchar(255)
AS
	SELECT * from movies WHERE title LIKE @title+'%'