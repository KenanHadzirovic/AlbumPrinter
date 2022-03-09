-- Table dbo.ProductTypes
create table
	[dbo].[ProductTypes]
(
	[ProductTypeId] int identity(1,1) not null
	, [ProductTypeName] nvarchar(50) not null
	, [ProductTypeDescription] nvarchar(150) null
	, [BinWidth] DECIMAL(18, 2) not null
,
constraint [Pk_ProductTypes_ProductTypeId] primary key clustered
(
	[ProductTypeId] asc
)
);