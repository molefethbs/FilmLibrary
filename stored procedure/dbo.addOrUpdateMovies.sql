CREATE PROCEDURE dbo.addOrUpdateMovies
	@mode varchar(255),
	@movieId int,
	@title varchar(255),
	@genre varchar(255),
	@rating varchar(255),
	@releaseDate date,
	@length varchar(255)
AS
	IF @mode = 'Add'
	BEGIN
	INSERT INTO movies(
	title,
	genre,
	rating,
	releaseDate,
	lenghth)
	VALUES(
	@title,
	@genre,
	@rating,
	@releaseDate,
	@length)
	END
	ELSE IF @mode='Edit'
	BEGIN
	UPDATE movies
	SET
	title=@title,
	genre=@genre,
	rating=@rating,
	releaseDate=@releaseDate,
	lenghth=@length
	WHERE movieId=@movieId
	END