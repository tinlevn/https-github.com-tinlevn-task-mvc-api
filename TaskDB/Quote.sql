CREATE TABLE [dbo].[Quote] (
    [QuoteID]   INT            NOT NULL IDENTITY,
    [QuoteType] NVARCHAR (20)  NOT NULL,
    [Contact]   NVARCHAR (25)  NULL,
    [Task]      NVARCHAR (MAX) NOT NULL,
    [DueDate]   DATE           NULL,
    [TaskType]  NVARCHAR (30)  NULL,
    PRIMARY KEY CLUSTERED ([QuoteID] ASC)
);

