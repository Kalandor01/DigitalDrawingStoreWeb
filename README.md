# Digital drawing store

# Introduction 
DDSW

# Getting Started


# Build and Test

## Prod database

```
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
```

## Script to fill the prod environment with test data

```
USE DDSW


-- METADATAS
DECLARE @MetadataDefinitionId1 AS UNIQUEIDENTIFIER;
SET @MetadataDefinitionId1 = NEWID();

DECLARE @MetadataDefinitionId2 AS UNIQUEIDENTIFIER;
SET @MetadataDefinitionId2 = NEWID();

DECLARE @MetadataDefinitionId3 AS UNIQUEIDENTIFIER;
SET @MetadataDefinitionId3 = NEWID();

DECLARE @MetadataDefinitionId4 AS UNIQUEIDENTIFIER;
SET @MetadataDefinitionId4 = NEWID();

DECLARE @MetadataDefinitionId5 AS UNIQUEIDENTIFIER;
SET @MetadataDefinitionId5 = NEWID();

DECLARE @MetadataDefinitionId6 AS UNIQUEIDENTIFIER;
SET @MetadataDefinitionId6 = NEWID();

INSERT INTO DocumentMetadataDefinitions (Id, ExtractedName) VALUES (@MetadataDefinitionId1, 'Document number');
INSERT INTO DocumentMetadataDefinitions (Id, ExtractedName) VALUES (@MetadataDefinitionId2, 'Part number');
INSERT INTO DocumentMetadataDefinitions (Id, ExtractedName) VALUES (@MetadataDefinitionId3, 'CreatedBy');
INSERT INTO DocumentMetadataDefinitions (Id, ExtractedName) VALUES (@MetadataDefinitionId4, 'CreatedAt');
INSERT INTO DocumentMetadataDefinitions (Id, ExtractedName) VALUES (@MetadataDefinitionId5, 'ModifiedBy');
INSERT INTO DocumentMetadataDefinitions (Id, ExtractedName) VALUES (@MetadataDefinitionId6, 'ModifiedAt');
INSERT INTO DocumentMetadataDefinitions (Id, ExtractedName) VALUES (NEWID(), 'ModeOfUsage');
INSERT INTO DocumentMetadataDefinitions (Id, ExtractedName) VALUES (NEWID(), 'Freetext');


-- CATEGORIES
DECLARE @CategoryId1 AS UNIQUEIDENTIFIER;
SET @CategoryId1 = NEWID();

DECLARE @CategoryId2 AS UNIQUEIDENTIFIER;
SET @CategoryId2 = NEWID();

DECLARE @CategoryId3 AS UNIQUEIDENTIFIER;
SET @CategoryId3 = NEWID();

DECLARE @CategoryId4 AS UNIQUEIDENTIFIER;
SET @CategoryId4 = NEWID();

DECLARE @CategoryId5 AS UNIQUEIDENTIFIER;
SET @CategoryId5 = NEWID();

DECLARE @CategoryId6 AS UNIQUEIDENTIFIER;
SET @CategoryId6 = NEWID();

INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (@CategoryId1, 1, 'Változások');
INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (@CategoryId2, 1, 'Szabványok');
INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (@CategoryId3, 1, 'Érvényes műveleti utasítások');
INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (@CategoryId4, 1, 'Érvényes kezelési és karbantartási utasítások');
INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (@CategoryId5, 1, 'Érvényes ellenőrzési utasítások / Minőségellenőrző kártyák');
INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (@CategoryId6, 1, 'Érvényes WPS lapok');
INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (NEWID(), 1, 'Érvényes PFMEA');
INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (NEWID(), 1, 'Érvényes folyamat szabályozási terv');
INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (NEWID(), 1, 'Érvényes folyamatábra');


-- CATEGORY METADATEA PAIRS
INSERT INTO DocumentCategoryEntities (Id, DocumentCategoryId, DocumentMetadataDefinitionId) VALUES (NEWID(), @CategoryId1, @MetadataDefinitionId1);
INSERT INTO DocumentCategoryEntities (Id, DocumentCategoryId, DocumentMetadataDefinitionId) VALUES (NEWID(), @CategoryId1, @MetadataDefinitionId2);
INSERT INTO DocumentCategoryEntities (Id, DocumentCategoryId, DocumentMetadataDefinitionId) VALUES (NEWID(), @CategoryId1, @MetadataDefinitionId3);
INSERT INTO DocumentCategoryEntities (Id, DocumentCategoryId, DocumentMetadataDefinitionId) VALUES (NEWID(), @CategoryId2, @MetadataDefinitionId4);
INSERT INTO DocumentCategoryEntities (Id, DocumentCategoryId, DocumentMetadataDefinitionId) VALUES (NEWID(), @CategoryId2, @MetadataDefinitionId5);
INSERT INTO DocumentCategoryEntities (Id, DocumentCategoryId, DocumentMetadataDefinitionId) VALUES (NEWID(), @CategoryId2, @MetadataDefinitionId6);
INSERT INTO DocumentCategoryEntities (Id, DocumentCategoryId, DocumentMetadataDefinitionId) VALUES (NEWID(), @CategoryId3, @MetadataDefinitionId2);
INSERT INTO DocumentCategoryEntities (Id, DocumentCategoryId, DocumentMetadataDefinitionId) VALUES (NEWID(), @CategoryId3, @MetadataDefinitionId3);
INSERT INTO DocumentCategoryEntities (Id, DocumentCategoryId, DocumentMetadataDefinitionId) VALUES (NEWID(), @CategoryId3, @MetadataDefinitionId4);
INSERT INTO DocumentCategoryEntities (Id, DocumentCategoryId, DocumentMetadataDefinitionId) VALUES (NEWID(), @CategoryId4, @MetadataDefinitionId1);
INSERT INTO DocumentCategoryEntities (Id, DocumentCategoryId, DocumentMetadataDefinitionId) VALUES (NEWID(), @CategoryId4, @MetadataDefinitionId6);
INSERT INTO DocumentCategoryEntities (Id, DocumentCategoryId, DocumentMetadataDefinitionId) VALUES (NEWID(), @CategoryId5, @MetadataDefinitionId2);
INSERT INTO DocumentCategoryEntities (Id, DocumentCategoryId, DocumentMetadataDefinitionId) VALUES (NEWID(), @CategoryId5, @MetadataDefinitionId4);
INSERT INTO DocumentCategoryEntities (Id, DocumentCategoryId, DocumentMetadataDefinitionId) VALUES (NEWID(), @CategoryId5, @MetadataDefinitionId5);
INSERT INTO DocumentCategoryEntities (Id, DocumentCategoryId, DocumentMetadataDefinitionId) VALUES (NEWID(), @CategoryId6, @MetadataDefinitionId1);


-- DOCUMENTS
DECLARE @DodcumentId1 AS UNIQUEIDENTIFIER;
SET @DodcumentId1 = NEWID();

DECLARE @DodcumentId2 AS UNIQUEIDENTIFIER;
SET @DodcumentId2 = NEWID();

DECLARE @DodcumentId3 AS UNIQUEIDENTIFIER;
SET @DodcumentId3 = NEWID();

INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (@DodcumentId1, @CategoryId1, 'C:\TestDocuments\Test.pdf');
INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (NEWID(), @CategoryId1, 'C:\TestDocuments\Test2.pdf');
INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (NEWID(), @CategoryId1, 'C:\TestDocuments\Test3.pdf');
INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (@DodcumentId2, @CategoryId1, 'C:\TestDocuments\Test4.pdf');
INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (NEWID(), @CategoryId2, 'C:\TestDocuments\Test.pdf');
INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (NEWID(), @CategoryId2, 'C:\TestDocuments\Test2.pdf');
INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (@DodcumentId3, @CategoryId2, 'C:\TestDocuments\Test3.pdf');
INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (NEWID(), @CategoryId2, 'C:\TestDocuments\Test4.pdf');


-- METADATA
INSERT INTO DocumentMetadata (Id, DocumentId, DocumentMetadataDefinitionId, Value) VALUES (NEWID(), @DodcumentId1, @MetadataDefinitionId1, 'DocumentNumberA');
INSERT INTO DocumentMetadata (Id, DocumentId, DocumentMetadataDefinitionId, Value) VALUES (NEWID(), @DodcumentId1, @MetadataDefinitionId2, 'PartNumberA');
INSERT INTO DocumentMetadata (Id, DocumentId, DocumentMetadataDefinitionId, Value) VALUES (NEWID(), @DodcumentId1, @MetadataDefinitionId3, 'TestUserA');

INSERT INTO DocumentMetadata (Id, DocumentId, DocumentMetadataDefinitionId, Value) VALUES (NEWID(), @DodcumentId2, @MetadataDefinitionId1, 'DocumentNumberB');
INSERT INTO DocumentMetadata (Id, DocumentId, DocumentMetadataDefinitionId, Value) VALUES (NEWID(), @DodcumentId2, @MetadataDefinitionId3, 'TestUserB');

INSERT INTO DocumentMetadata (Id, DocumentId, DocumentMetadataDefinitionId, Value) VALUES (NEWID(), @DodcumentId3, @MetadataDefinitionId1, 'DocumentNumberC');
INSERT INTO DocumentMetadata (Id, DocumentId, DocumentMetadataDefinitionId, Value) VALUES (NEWID(), @DodcumentId3, @MetadataDefinitionId2, 'PartNumberC');
INSERT INTO DocumentMetadata (Id, DocumentId, DocumentMetadataDefinitionId, Value) VALUES (NEWID(), @DodcumentId3, @MetadataDefinitionId3, 'TestUserC');


-- ApplicationProperties
INSERT INTO ApplicationProperties (PropertyKey, PropertyValue) VALUES ('SenderEmail', '<sender-email>');
INSERT INTO ApplicationProperties (PropertyKey, PropertyValue) VALUES ('EmailRecipients', '<recipient-email>;<recipient-email>;<recipient-email>;');
INSERT INTO ApplicationProperties (PropertyKey, PropertyValue) VALUES ('SmtpHost', 'smtp.gmail.com');
INSERT INTO ApplicationProperties (PropertyKey, PropertyValue) VALUES ('SmtpPort', '587');
INSERT INTO ApplicationProperties (PropertyKey, PropertyValue) VALUES ('SmtpUsername', '<sender-email>');
INSERT INTO ApplicationProperties (PropertyKey, PropertyValue) VALUES ('SmtpPassword', '<sender-password>');
INSERT INTO ApplicationProperties (PropertyKey, PropertyValue) VALUES ('UseDefaultSmtpCredentials', 'false');
INSERT INTO ApplicationProperties (PropertyKey, PropertyValue) VALUES ('EnableSmtpSsl', 'true');


-- Target of document usage
DECLARE @TargetOfUsagePropertyId AS UNIQUEIDENTIFIER;
SET @TargetOfUsagePropertyId = NEWID();

INSERT INTO DocumentUsages(Id, UsageName) VALUES (NEWID(), 'Első cél');
INSERT INTO DocumentUsages(Id, UsageName) VALUES (NEWID(), 'Második cél');
INSERT INTO DocumentUsages(Id, UsageName) VALUES (NEWID(), 'Harmadik cél');
```

## Test
- Use the following script to create database:

```
CREATE DATABASE DDSWAutomatedTests
go
ALTER DATABASE DDSWAutomatedTests COLLATE Hungarian_CI_AS
GO
use DDSWAutomatedTests
go


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
```

- To drop the database tables use the following script:

Test data:
```
DROP TABLE DocumentMetadataTest
DROP TABLE DocumentCategoryEntitiesTest
DROP TABLE DocumentMetadataDefinitionsTest
DROP TABLE DocumentsTest
DROP TABLE DocumentCategoriesTest
DROP TABLE UserEventLogsTest
DROP TABLE ApplicationPropertiesTest
DROP TABLE DocumentUsagesTest
```

Prod:
```
DROP TABLE DocumentMetadata
DROP TABLE DocumentCategoryEntities
DROP TABLE DocumentMetadataDefinitions
DROP TABLE Documents
DROP TABLE DocumentCategories
DROP TABLE UserEventLogs
DROP TABLE ApplicationProperties
DROP TABLE DocumentUsages
```

- To select all data from all table, use the following script:
Test data:
```
select * from DocumentCategoriesTest
select * from DocumentsTest
select * from DocumentMetadataDefinitionsTest
select * from DocumentCategoryEntitiesTest
select * from DocumentMetadataTest
select * from UserEventLogsTest
select * from ApplicationPropertiesTest
select * from DocumentUsagesTest
```

Prod:
```
SELECT * FROM DocumentMetadataDefinitions;
SELECT * FROM DocumentCategories;
SELECT * FROM DocumentCategoryEntities;
SELECT * FROM Documents;
SELECT * FROM DocumentMetadata;
SELECT * FROM ApplicationProperties;
SELECT * FROM DocumentUsages;
```