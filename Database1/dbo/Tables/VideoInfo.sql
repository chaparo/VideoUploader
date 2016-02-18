CREATE TABLE [dbo].[VideoInfo] (
    [id]          UNIQUEIDENTIFIER NOT NULL,
    [videoPath]   NVARCHAR (500)   NULL,
    [description] NVARCHAR (MAX)   NOT NULL,
    [title]       NVARCHAR (100)   NULL,
    [addDate]     DATETIME         NULL,
    [duration]    TIME (7)         NULL,
    [imagePath]   NVARCHAR (500)   NULL
);

