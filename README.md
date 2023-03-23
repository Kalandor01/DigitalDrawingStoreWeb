# Introduction 
TODO: Give a short introduction of your project. Let this section explain the objectives or the motivation behind this project. 

# Getting Started
TODO: Guide users through getting your code up and running on their own system. In this section you can talk about:
1.	Installation process
2.	Software dependencies
3.	Latest releases
4.	API references

# Build and Test

## Full script to create a prod environment with test data

```
CREATE TABLE DocumentCategories
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  IsDesigned BIT DEFAULT 0,
  DisplayName VARCHAR(MAX)
);
CREATE TABLE Documents
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  DocumentCategoryId UNIQUEIDENTIFIER,
  Path VARCHAR(MAX),
  FOREIGN KEY (DocumentCategoryId) REFERENCES DocumentCategories (Id)
);
CREATE TABLE DocumentMetadataDefinitions
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  ExtractedName VARCHAR(MAX)
);
CREATE TABLE DocumentCategoryEntities
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  DocumentCategoryId UNIQUEIDENTIFIER,
  DocumentMetadataDefinitionId UNIQUEIDENTIFIER,
  FOREIGN KEY (DocumentCategoryId) REFERENCES DocumentCategories (Id),
  FOREIGN KEY (DocumentMetadataDefinitionId) REFERENCES DocumentMetadataDefinitions (Id)
);
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
  PropertyKey VARCHAR(MAX),
  PropertyValue VARCHAR(MAX)
);
CREATE TABLE ApplicationPropertiesDictionary
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  ApplicationPropertiesId UNIQUEIDENTIFIER,
  PropertyValue VARCHAR(MAX),
  FOREIGN KEY (ApplicationPropertiesId) REFERENCES ApplicationProperties (Id)
);
CREATE TABLE UserEventLogs
(
	Id UNIQUEIDENTIFIER NOT NULL,
	EventId VARCHAR(max) NOT NULL,
	LoggedAt DateTime2 NOT NULL,
	UserDomainName VARCHAR(max) NOT NULL,
	MachineNumber VARCHAR(max) NOT NULL,
	SourceIp VARCHAR(max) NOT NULL,
	DocumentPath VARCHAR(max) NULL,
	DocumentVersion VARCHAR(max) NULL, -- ???
	TargetOfUsage VARCHAR(max) NULL,
	DocumentDrawingNumber VARCHAR(max) NULL, -- ???
	DocumentRevId VARCHAR(max) NULL, -- ???
	DocumentTitle VARCHAR(max) NULL, -- ???
	DocumentTypeOfPRoductOnDrawing VARCHAR(max) NULL,
	DocumentPrefix VARCHAR(max) NULL,
	WatermarkText VARCHAR(max) NULL,
	WatermarkPosition VARCHAR(max) NULL,
	WatermarkFontSizeInPt VARCHAR(max) NULL,
	WatermarkRotationAngleInDegree VARCHAR(max) NULL,
	OldPropertyName VARCHAR(max) NULL,
	NewPropertyName VARCHAR(max) NULL,
	DocumentCreatedAt DateTime2 NULL,
	DocumentApprovedAt DateTime2 NULL

	PRIMARY KEY(Id)
);

-- METADATA DEFINITIONS
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

-- CATEGORY ENTITIES
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

INSERT INTO ApplicationProperties (Id, PropertyKey, PropertyValue) VALUES(@TargetOfUsagePropertyId, 'TargetOfUsage', NULL);

INSERT INTO ApplicationPropertiesDictionary (Id, ApplicationPropertiesId, PropertyValue) VALUES (NEWID(), @TargetOfUsagePropertyId, 'Első cél');
INSERT INTO ApplicationPropertiesDictionary (Id, ApplicationPropertiesId, PropertyValue) VALUES (NEWID(), @TargetOfUsagePropertyId, 'Második cél');
INSERT INTO ApplicationPropertiesDictionary (Id, ApplicationPropertiesId, PropertyValue) VALUES (NEWID(), @TargetOfUsagePropertyId, 'Harmadik cél');

SELECT * FROM DocumentMetadataDefinitions;
SELECT * FROM DocumentCategories;
SELECT * FROM DocumentCategoryEntities;
SELECT * FROM Documents;
SELECT * FROM DocumentMetadata;
SELECT * FROM ApplicationProperties;
SELECT * FROM ApplicationPropertiesDictionary;
```

## Prod database tables

```
CREATE TABLE DocumentCategories
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  IsDesigned BIT DEFAULT 0,
  DisplayName VARCHAR(MAX)
);
CREATE TABLE Documents
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  DocumentCategoryId UNIQUEIDENTIFIER,
  Path VARCHAR(MAX),
  FOREIGN KEY (DocumentCategoryId) REFERENCES DocumentCategories (Id)
);
CREATE TABLE DocumentMetadataDefinitions
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  ExtractedName VARCHAR(MAX)
);
CREATE TABLE DocumentCategoryEntities
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  DocumentCategoryId UNIQUEIDENTIFIER,
  DocumentMetadataDefinitionId UNIQUEIDENTIFIER,
  FOREIGN KEY (DocumentCategoryId) REFERENCES DocumentCategories (Id),
  FOREIGN KEY (DocumentMetadataDefinitionId) REFERENCES DocumentMetadataDefinitions (Id)
);
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
  PropertyKey VARCHAR(MAX),
  PropertyValue VARCHAR(MAX)
);
CREATE TABLE ApplicationPropertiesDictionary
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  ApplicationPropertiesId UNIQUEIDENTIFIER,
  PropertyValue VARCHAR(MAX),
  FOREIGN KEY (ApplicationPropertiesId) REFERENCES ApplicationProperties (Id)
);
CREATE TABLE UserEventLogs
(
	Id UNIQUEIDENTIFIER NOT NULL,
	EventId VARCHAR(max) NOT NULL,
	LoggedAt DateTime2 NOT NULL,
	UserDomainName VARCHAR(max) NOT NULL,
	MachineNumber VARCHAR(max) NOT NULL,
	SourceIp VARCHAR(max) NOT NULL,
	DocumentPath VARCHAR(max) NULL,
	DocumentVersion VARCHAR(max) NULL,
	TargetOfUsage VARCHAR(max) NULL,
	DocumentDrawingNumber VARCHAR(max) NULL,
	DocumentRevId VARCHAR(max) NULL,
	DocumentTitle VARCHAR(max) NULL,
	DocumentTypeOfPRoductOnDrawing VARCHAR(max) NULL,
	DocumentPrefix VARCHAR(max) NULL,
	WatermarkText VARCHAR(max) NULL,
	WatermarkPosition VARCHAR(max) NULL,
	WatermarkFontSizeInPt VARCHAR(max) NULL,
	WatermarkRotationAngleInDegree VARCHAR(max) NULL,
	OldPropertyName VARCHAR(max) NULL,
	NewPropertyName VARCHAR(max) NULL,
	DocumentCreatedAt DateTime2 NULL,
	DocumentApprovedAt DateTime2 NULL

	PRIMARY KEY(Id)
);
```

## Test
- Create the following database in your local machine: AutomatedTests
  - use the following collate: LATIN1_GENERAL_100_CI_AS_SC_UTF8
- Use the following script to create database tables:

```
CREATE TABLE DocumentCategoriesTest
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  IsDesigned BIT DEFAULT 0,
  DisplayName VARCHAR(MAX)
);
CREATE TABLE DocumentsTest
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  DocumentCategoryId UNIQUEIDENTIFIER,
  Path VARCHAR(MAX),
  FOREIGN KEY (DocumentCategoryId) REFERENCES DocumentCategoriesTest (Id)
);
CREATE TABLE DocumentMetadataDefinitionsTest
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  ExtractedName VARCHAR(MAX)
);
CREATE TABLE DocumentCategoryEntitiesTest
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  DocumentCategoryId UNIQUEIDENTIFIER,
  DocumentMetadataDefinitionId UNIQUEIDENTIFIER,
  FOREIGN KEY (DocumentCategoryId) REFERENCES DocumentCategoriesTest (Id),
  FOREIGN KEY (DocumentMetadataDefinitionId) REFERENCES DocumentMetadataDefinitionsTest (Id)
);
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
  PropertyKey VARCHAR(MAX),
  PropertyValue VARCHAR(MAX)
);
CREATE TABLE ApplicationPropertiesDictionaryTest
(
  Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
  ApplicationPropertiesId UNIQUEIDENTIFIER,
  PropertyValue VARCHAR(MAX),
  FOREIGN KEY (ApplicationPropertiesId) REFERENCES ApplicationPropertiesTest (Id)
);
CREATE TABLE UserEventLogsTest
(
	Id UNIQUEIDENTIFIER NOT NULL,
	EventId VARCHAR(max) NOT NULL,
	LoggedAt DateTime2 NOT NULL,
	UserDomainName VARCHAR(max) NOT NULL,
	MachineNumber VARCHAR(max) NOT NULL,
	SourceIp VARCHAR(max) NOT NULL,
	DocumentPath VARCHAR(max) NULL,
	DocumentVersion VARCHAR(max) NULL,
	TargetOfUsage VARCHAR(max) NULL,
	DocumentDrawingNumber VARCHAR(max) NULL,
	DocumentRevId VARCHAR(max) NULL,
	DocumentTitle VARCHAR(max) NULL,
	DocumentTypeOfPRoductOnDrawing VARCHAR(max) NULL,
	DocumentPrefix VARCHAR(max) NULL,
	WatermarkText VARCHAR(max) NULL,
	WatermarkPosition VARCHAR(max) NULL,
	WatermarkFontSizeInPt VARCHAR(max) NULL,
	WatermarkRotationAngleInDegree VARCHAR(max) NULL,
	OldPropertyName VARCHAR(max) NULL,
	NewPropertyName VARCHAR(max) NULL,
	DocumentCreatedAt DateTime2 NULL,
	DocumentApprovedAt DateTime2 NULL

	PRIMARY KEY(Id)
);
```

- To drop the database tables use the following script:

Test data:
```
DROP TABLE DocumentMetadataTest;
DROP TABLE DocumentCategoryEntitiesTest;
DROP TABLE DocumentMetadataDefinitionsTest;
DROP TABLE DocumentsTest;
DROP TABLE DocumentCategoriesTest;
DROP TABLE ApplicationPropertiesDictionaryTest;
DROP TABLE ApplicationPropertiesTest;
DROP TABLE UserEventLogsTest;
```

Prod:
```
DROP TABLE DocumentMetadata;
DROP TABLE DocumentCategoryEntities;
DROP TABLE DocumentMetadataDefinitions;
DROP TABLE Documents;
DROP TABLE DocumentCategories;
DROP TABLE ApplicationPropertiesDictionary;
DROP TABLE ApplicationProperties;
DROP TABLE UserEventLogs;
```

- To select all data from all table, use the following script:
Test data:
```
SELECT * FROM DocumentMetadataTest;
SELECT * FROM DocumentCategoryEntitiesTest;
SELECT * FROM DocumentMetadataDefinitionsTest;
SELECT * FROM DocumentsTest;
SELECT * FROM DocumentCategoriesTest;
SELECT * FROM ApplicationPropertiesDictionaryTest;
SELECT * FROM ApplicationPropertiesTest;
SELECT * FROM UserEventLogsTest;
```

Prod:
```
SELECT * FROM DocumentMetadataDefinitions;
SELECT * FROM DocumentMetadata;
SELECT * FROM DocumentCategories;
SELECT * FROM DocumentCategoryEntities;
SELECT * FROM Documents;
SELECT * FROM ApplicationPropertiesDictionary;
SELECT * FROM ApplicationProperties;
SELECT * FROM UserEventLogs;
```

## Test data
```
INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (NEWID(), 1, 'Változások');
INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (NEWID(), 1, 'Szabványok');
INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (NEWID(), 1, 'Érvényes műveleti utasítások');
INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (NEWID(), 1, 'Érvényes kezelési és karbantartási utasítások');
INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (NEWID(), 1, 'Érvényes ellenőrzési utasítások / Minőségellenőrző kártyák');
INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (NEWID(), 1, 'Érvényes WPS lapok');
INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (NEWID(), 1, 'Érvényes PFMEA');
INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (NEWID(), 1, 'Érvényes folyamat szabályozási terv');
INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (NEWID(), 1, 'Érvényes folyamatábra');

DECLARE @Category1 AS UNIQUEIDENTIFIER;
SET @Category1 = ( SELECT Id FROM (SELECT TOP 3 [Id], ROW_NUMBER() OVER(ORDER BY Id) AS RowNumber FROM DocumentCategories) As res WHERE RowNumber=3)

DECLARE @Category2 AS UNIQUEIDENTIFIER;
SET @Category2 = (SELECT Id FROM (SELECT TOP 2 [Id], ROW_NUMBER() OVER(ORDER BY Id) AS RowNumber FROM DocumentCategories) As res WHERE RowNumber=2)

INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (NEWID(), @Category1, 'C:\TestDocuments\Test.pdf');
INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (NEWID(), @Category1, 'C:\TestDocuments\Test2.pdf');
INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (NEWID(), @Category1, 'C:\TestDocuments\Test3.pdf');
INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (NEWID(), @Category1, 'C:\TestDocuments\Test4.pdf');
INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (NEWID(), @Category2, 'C:\TestDocuments\Test.pdf');
INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (NEWID(), @Category2, 'C:\TestDocuments\Test2.pdf');
INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (NEWID(), @Category2, 'C:\TestDocuments\Test3.pdf');
INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (NEWID(), @Category2, 'C:\TestDocuments\Test4.pdf');

-- ApplicationProperties - target of document usage

DECLARE @TargetOfUsagePropertyId AS UNIQUEIDENTIFIER;
SET @TargetOfUsagePropertyId = NEWID();

INSERT INTO ApplicationPropertiesTest (Id, PropertyKey, PropertyValue) VALUES(@TargetOfUsagePropertyId, 'TargetOfUsage', NULL);

INSERT INTO ApplicationPropertiesDictionaryTest (Id, ApplicationPropertiesId, PropertyValue) VALUES (NEWID(), @TargetOfUsagePropertyId, 'Első cél');
INSERT INTO ApplicationPropertiesDictionaryTest (Id, ApplicationPropertiesId, PropertyValue) VALUES (NEWID(), @TargetOfUsagePropertyId, 'Második cél');
INSERT INTO ApplicationPropertiesDictionaryTest (Id, ApplicationPropertiesId, PropertyValue) VALUES (NEWID(), @TargetOfUsagePropertyId, 'Harmadik cél');
```

# Contribute
TODO: Explain how other users and developers can contribute to make your code better. 

If you want to learn more about creating good readme files then refer the following [guidelines](https://docs.microsoft.com/en-us/azure/devops/repos/git/create-a-readme?view=azure-devops). You can also seek inspiration from the below readme files:
- [ASP.NET Core](https://github.com/aspnet/Home)
- [Visual Studio Code](https://github.com/Microsoft/vscode)
- [Chakra Core](https://github.com/Microsoft/ChakraCore)