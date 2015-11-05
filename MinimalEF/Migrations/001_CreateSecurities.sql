﻿

CREATE TABLE [dbo].[Securities] (
	[Id] UNIQUEIDENTIFIER NOT NULL,
	CONSTRAINT [PK_Securities] PRIMARY KEY ([Id]),
	[Code] NVARCHAR(MAX) NOT NULL,
	[CurrentPrice] DECIMAL(38,19) NOT NULL
)
