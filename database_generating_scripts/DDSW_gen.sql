CREATE DATABASE DDSW
GO
ALTER DATABASE DDSW COLLATE Hungarian_CI_AS
GO
USE DDSW
GO


CREATE TABLE DocumentCategories
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  IsDesigned BIT DEFAULT 0,
  DisplayName NVARCHAR(MAX)
);


CREATE TABLE Documents
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  DocumentCategoryId UNIQUEIDENTIFIER,
  Path NVARCHAR(MAX),
  FOREIGN KEY (DocumentCategoryId) REFERENCES DocumentCategories (Id)
);


CREATE TABLE DocumentMetadataDefinitions
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  ExtractedName NVARCHAR(MAX)
);


CREATE TABLE DocumentCategoryEntities
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  DocumentCategoryId UNIQUEIDENTIFIER,
  DocumentMetadataDefinitionId UNIQUEIDENTIFIER,
  FOREIGN KEY (DocumentCategoryId) REFERENCES DocumentCategories (Id),
  FOREIGN KEY (DocumentMetadataDefinitionId) REFERENCES DocumentMetadataDefinitions (Id)
);
GO


CREATE TABLE DocumentMetadata
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  DocumentId UNIQUEIDENTIFIER,
  DocumentMetadataDefinitionId UNIQUEIDENTIFIER,
  Value NVARCHAR(MAX),
  FOREIGN KEY (DocumentId) REFERENCES Documents (Id),
  FOREIGN KEY (DocumentMetadataDefinitionId) REFERENCES DocumentMetadataDefinitions (Id)
);


CREATE TABLE ApplicationProperties
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  PropertyKey NVARCHAR(MAX),
  PropertyValue NVARCHAR(MAX)
);


CREATE TABLE DocumentUsages
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  UsageName NVARCHAR(MAX),
)


CREATE TABLE UserEventLogs
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


--USE master
--GO
--DROP DATABASE DDSW
--go