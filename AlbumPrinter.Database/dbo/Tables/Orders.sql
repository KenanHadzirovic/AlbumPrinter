-- Table dbo.Orders
create table
	[dbo].[Orders]
(
	[OrderId] uniqueidentifier not null
	, [DateCreated] datetime2(7) not null
,
constraint [Pk_Orders_OrderId] primary key clustered
(
	[OrderId] asc
)
);