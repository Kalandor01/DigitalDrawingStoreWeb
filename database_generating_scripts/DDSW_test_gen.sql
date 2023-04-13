CREATE DATABASE DDSWAutomatedTests
GO
ALTER DATABASE DDSWAutomatedTests COLLATE Hungarian_CI_AS
GO
use DDSWAutomatedTests
GO


CREATE TABLE DocumentCategoriesTest
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  IsDesigned BIT DEFAULT 0,
  DisplayName NVARCHAR(MAX)
);


CREATE TABLE DocumentsTest
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  DocumentCategoryId UNIQUEIDENTIFIER,
  Path NVARCHAR(MAX),
  FOREIGN KEY (DocumentCategoryId) REFERENCES DocumentCategoriesTest (Id)
);


CREATE TABLE DocumentMetadataDefinitionsTest
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  ExtractedName NVARCHAR(MAX)
);


CREATE TABLE DocumentCategoryEntitiesTest
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  DocumentCategoryId UNIQUEIDENTIFIER,
  DocumentMetadataDefinitionId UNIQUEIDENTIFIER,
  FOREIGN KEY (DocumentCategoryId) REFERENCES DocumentCategoriesTest (Id),
  FOREIGN KEY (DocumentMetadataDefinitionId) REFERENCES DocumentMetadataDefinitionsTest (Id)
);
GO


CREATE TABLE DocumentMetadataTest
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  DocumentId UNIQUEIDENTIFIER,
  DocumentMetadataDefinitionId UNIQUEIDENTIFIER,
  Value NVARCHAR(MAX),
  FOREIGN KEY (DocumentId) REFERENCES DocumentsTest (Id),
  FOREIGN KEY (DocumentMetadataDefinitionId) REFERENCES DocumentMetadataDefinitionsTest (Id)
);


CREATE TABLE ApplicationPropertiesTest
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  PropertyKey NVARCHAR(MAX),
  PropertyValue NVARCHAR(MAX)
);


CREATE TABLE DocumentUsagesTest
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  UsageName NVARCHAR(MAX),
)


CREATE TABLE UserEventLogsTest
(
	Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
	EventName NVARCHAR(MAX) NOT NULL,
	LoggedAt DateTime2 NOT NULL,
	UserDomainName NVARCHAR(MAX) NOT NULL,
	MachineNumber NVARCHAR(MAX) NOT NULL,
	SourceIp NVARCHAR(MAX) NOT NULL,
	DocumentPath NVARCHAR(MAX) NULL,
	DocumentVersion NVARCHAR(MAX) NULL,
	TargetOfUsage NVARCHAR(MAX) NULL,
	DocumentDrawingNumber NVARCHAR(MAX) NULL,
	DocumentRevId NVARCHAR(MAX) NULL,
	DocumentTitle NVARCHAR(MAX) NULL,
	DocumentTypeOfPRoductOnDrawing NVARCHAR(MAX) NULL,
	DocumentPrefix NVARCHAR(MAX) NULL,
	WatermarkText NVARCHAR(MAX) NULL,
	WatermarkPosition NVARCHAR(MAX) NULL,
	WatermarkFontSizeInPt NVARCHAR(MAX) NULL,
	WatermarkRotationAngleInDegree NVARCHAR(MAX) NULL,
	WatermarkTransparencyInPercentage NVARCHAR(MAX) NULL,
	OldPropertyName NVARCHAR(MAX) NULL,
	NewPropertyName NVARCHAR(MAX) NULL,
	DocumentCreatedAt DateTime2 NULL,
	DocumentApprovedAt DateTime2 NULL
);



select * from DocumentCategoriesTest
select * from DocumentsTest
select * from DocumentMetadataDefinitionsTest
select * from DocumentCategoryEntitiesTest
select * from DocumentMetadataTest
select * from UserEventLogsTest
select * from ApplicationPropertiesTest
select * from DocumentUsagesTest

--DROP TABLE DocumentMetadataTest
--DROP TABLE DocumentCategoryEntitiesTest
--DROP TABLE DocumentMetadataDefinitionsTest
--DROP TABLE DocumentsTest
--DROP TABLE DocumentCategoriesTest
--DROP TABLE UserEventLogsTest
--DROP TABLE ApplicationPropertiesTest
--DROP TABLE DocumentUsagesTest

--USE master
--GO
--DROP DATABASE DDSWAutomatedTests
--go

