
CREATE TABLE [dbo].[PortfolioItems] (
	[Id] UNIQUEIDENTIFIER NOT NULL,
	CONSTRAINT [PK_PortfolioItems] PRIMARY KEY ([Id]),
	[PortfolioId] UNIQUEIDENTIFIER NOT NULL,
	CONSTRAINT [FK_PortfolioItems_Portfolios] FOREIGN KEY ([PortfolioId]) REFERENCES [dbo].[Portfolios] ([Id]),
	[SecurityId] UNIQUEIDENTIFIER NOT NULL,
	CONSTRAINT [FK_PortfolioItems_Securities] FOREIGN KEY ([SecurityId]) REFERENCES [dbo].[Securities] ([Id]),
	[Units] DECIMAL(38,19)
)

