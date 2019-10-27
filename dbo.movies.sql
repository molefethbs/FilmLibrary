CREATE TABLE [dbo].[movies] (
    [movieId]     INT           IDENTITY (1, 1) NOT NULL,
    [title]       VARCHAR (255) NULL,
    [genre]       VARCHAR (255) NULL,
    [rating]      VARCHAR (255) NULL,
    [releaseDate] VARCHAR (255) NULL,
    [lenghth]     VARCHAR (255) NULL,
    CONSTRAINT [PK_movies] PRIMARY KEY CLUSTERED ([movieId] ASC)
);

