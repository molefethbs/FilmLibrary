CREATE PROCEDURE dbo.deleteMovie
	@movieId int
AS
	DELETE movies
	WHERE movieId=@movieId