IF EXISTS (SELECT 1 FROM SYS.TABLES where name ='tempProductTypes')
BEGIN 
DROP TABLE tempProductTypes
END
 
 
CREATE TABLE tempProductTypes(
	 [ProductTypeId] int not null
	, [ProductTypeName] nvarchar(50) not null
	, [ProductTypeDescription] nvarchar(150) null
	, [BinWidth] DECIMAL(18, 2) not null)
GO

INSERT INTO tempProductTypes(
							 [ProductTypeId]
							,[ProductTypeName]
							,[ProductTypeDescription]
							,[BinWidth])
     VALUES
         (1, 'photoBook', 'photoBook', 19)
         ,(2, 'calendar', 'calendar', 10)
         ,(3, 'canvas', 'canvas', 16)
         ,(4, 'cards', 'cards', 4.7)
         ,(5, 'mug', 'mug', 94)

GO
 
BEGIN TRAN
	SET IDENTITY_INSERT dbo.ProductTypes ON

	MERGE dbo.ProductTypes Tgt
	USING tempProductTypes Src ON Tgt.ProductTypeId = Src.ProductTypeId
	WHEN NOT MATCHED BY TARGET 
	THEN 
		INSERT (
				 [ProductTypeId]
				,[ProductTypeName]
				,[ProductTypeDescription]
				,[BinWidth])
		VALUES (
				 Src.[ProductTypeId]
				,Src.[ProductTypeName]
				,Src.[ProductTypeDescription]
				,Src.[BinWidth]
				);

	SET IDENTITY_INSERT dbo.ProductTypes OFF
				
 COMMIT TRAN

 DROP TABLE tempProductTypes
 GO