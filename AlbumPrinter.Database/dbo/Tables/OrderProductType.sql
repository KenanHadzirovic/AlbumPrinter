-- Model New Model
-- Updated 17-Feb-12 7:00:00 AM
-- DDL Generated 09-Mar-22 9:46:42 AM

--**********************************************************************
--	Tables
--**********************************************************************

-- Table dbo.OrderProductType
create table
	[dbo].[OrderProductType]
(
	[OrderProductId] uniqueidentifier not null
	, [Quantity] int not null
	, [OrderId] uniqueidentifier not null
	, [ProductTypeId] int not null
,
constraint [Pk_OrderProductType_OrderProductId] primary key clustered
(
	[OrderProductId] asc
)
);
GO
--**********************************************************************
--	Data
--**********************************************************************
--**********************************************************************
--	Relationships
--**********************************************************************

-- Relationship Fk_Orders_OrderProductType_OrderId
alter table [dbo].[OrderProductType]
add constraint [Fk_Orders_OrderProductType_OrderId] foreign key ([OrderId]) references [dbo].[Orders] ([OrderId]);
GO
-- Relationship Fk_ProductTypes_OrderProductType_ProductTypeId
alter table [dbo].[OrderProductType]
add constraint [Fk_ProductTypes_OrderProductType_ProductTypeId] foreign key ([ProductTypeId]) references [dbo].[ProductTypes] ([ProductTypeId]);