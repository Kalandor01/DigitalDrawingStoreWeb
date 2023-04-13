USE DDSW


-- METADATAS
DECLARE @MetadataDefinitionId1 AS UNIQUEIDENTIFIER = NEWID();

DECLARE @MetadataDefinitionId2 AS UNIQUEIDENTIFIER = NEWID();

DECLARE @MetadataDefinitionId3 AS UNIQUEIDENTIFIER = NEWID();

DECLARE @MetadataDefinitionId4 AS UNIQUEIDENTIFIER = NEWID();

DECLARE @MetadataDefinitionId5 AS UNIQUEIDENTIFIER = NEWID();

DECLARE @MetadataDefinitionId6 AS UNIQUEIDENTIFIER = NEWID();

INSERT INTO DocumentMetadataDefinitions (Id, ExtractedName) VALUES (@MetadataDefinitionId1, 'Dokumentum szám');
INSERT INTO DocumentMetadataDefinitions (Id, ExtractedName) VALUES (@MetadataDefinitionId2, 'Alkatrész szám');
INSERT INTO DocumentMetadataDefinitions (Id, ExtractedName) VALUES (@MetadataDefinitionId3, 'Készítette');
INSERT INTO DocumentMetadataDefinitions (Id, ExtractedName) VALUES (@MetadataDefinitionId4, 'Létrehozva');
INSERT INTO DocumentMetadataDefinitions (Id, ExtractedName) VALUES (@MetadataDefinitionId5, 'Módosította');
INSERT INTO DocumentMetadataDefinitions (Id, ExtractedName) VALUES (@MetadataDefinitionId6, 'Módosítva');
INSERT INTO DocumentMetadataDefinitions (Id, ExtractedName) VALUES (NEWID(), 'Használati mód');
INSERT INTO DocumentMetadataDefinitions (Id, ExtractedName) VALUES (NEWID(), 'Dokumentum szövege');


-- CATEGORIES
DECLARE @CategoryId1 AS UNIQUEIDENTIFIER = NEWID();

DECLARE @CategoryId2 AS UNIQUEIDENTIFIER = NEWID();

DECLARE @CategoryId3 AS UNIQUEIDENTIFIER = NEWID();

DECLARE @CategoryId4 AS UNIQUEIDENTIFIER = NEWID();

DECLARE @CategoryId5 AS UNIQUEIDENTIFIER = NEWID();

DECLARE @CategoryId6 AS UNIQUEIDENTIFIER = NEWID();

INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (@CategoryId1, 1, 'Változások');
INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (@CategoryId2, 1, 'Szabványok');
INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (@CategoryId3, 1, 'Érvényes mûveleti utasítások');
INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (@CategoryId4, 1, 'Érvényes kezelési és karbantartási utasítások');
INSERT INTO DocumentCategories (Id, IsDesigned, DisplayName) VALUES (@CategoryId5, 1, 'Érvényes ellenõrzési utasítások / Minõségellenõrzõ kártyák');
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
DECLARE @DodcumentId1 AS UNIQUEIDENTIFIER = NEWID();

DECLARE @DodcumentId2 AS UNIQUEIDENTIFIER = NEWID();

DECLARE @DodcumentId3 AS UNIQUEIDENTIFIER = NEWID();


DECLARE @TestDocumentsFolder AS NVARCHAR(MAX) = 'C:\TestDocuments\';

INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (@DodcumentId1, @CategoryId1, @TestDocumentsFolder + 'Test.pdf');
INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (NEWID(), @CategoryId1, @TestDocumentsFolder + 'Test2.pdf');
INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (NEWID(), @CategoryId1, @TestDocumentsFolder + 'Test3.pdf');
INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (@DodcumentId2, @CategoryId1, @TestDocumentsFolder + 'Test4.pdf');
INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (NEWID(), @CategoryId2, @TestDocumentsFolder + 'Test.pdf');
INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (NEWID(), @CategoryId2, @TestDocumentsFolder + 'Test2.pdf');
INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (@DodcumentId3, @CategoryId2, @TestDocumentsFolder + 'Test3.pdf');
INSERT INTO Documents (Id, DocumentCategoryId, Path) VALUES (NEWID(), @CategoryId2, @TestDocumentsFolder + 'Test4.pdf');


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
DECLARE @TargetOfUsagePropertyId AS UNIQUEIDENTIFIER = NEWID();

INSERT INTO DocumentUsages(Id, UsageName) VALUES (NEWID(), 'Elsõ cél');
INSERT INTO DocumentUsages(Id, UsageName) VALUES (NEWID(), 'Második cél');
INSERT INTO DocumentUsages(Id, UsageName) VALUES (NEWID(), 'Harmadik cél');

SELECT * FROM DocumentMetadataDefinitions;
SELECT * FROM DocumentCategories;
SELECT * FROM DocumentCategoryEntities;
SELECT * FROM Documents;
SELECT * FROM DocumentMetadata;
SELECT * FROM ApplicationProperties;
SELECT * FROM DocumentUsages;


-- Document data
SELECT dc.DisplayName as CategoryName, dc.IsDesigned as CaegoryDesigned, d.Path as DocumentPath, dmd.ExtractedName as Metadata, dm.Value MetadataValue FROM DocumentCategories dc
join Documents d on d.DocumentCategoryId = dc.Id
join DocumentMetadata dm on dm.DocumentId = d.Id
join DocumentMetadataDefinitions dmd on dm.DocumentMetadataDefinitionId = dmd.Id


-- witch metadatas are paired with witch category
SELECT DC.DisplayName, DC.IsDesigned, DMD.ExtractedName
FROM DocumentCategoryEntities DCE
JOIN DocumentCategories DC ON DCE.DocumentCategoryId = DC.Id
JOIN DocumentMetadataDefinitions DMD ON DCE.DocumentMetadataDefinitionId = DMD.Id
ORDER BY DC.DisplayName
GO


-- get all documents from a category
CREATE FUNCTION GetDocumentsFromCategory
(
	@CategoryId UNIQUEIDENTIFIER
)
RETURNS TABLE
AS
	RETURN SELECT *
	FROM Documents D
	WHERE D.DocumentCategoryId = @CategoryId
GO


--SELECT * FROM dbo.GetDocumentsFromCategory('9ABE765C-0201-4B3B-9622-D1A3451CC0E9')

--'BBAC363D-46D6-49BA-B975-AD2F1D8A6DF6'
--'695672D1-4B54-46F6-911A-35375AA03EFD'

--SELECT DBO.IsMetadataAllowed('4306E060-BB84-4B69-B7C3-57F413754647', '695672D1-4B54-46F6-911A-35375AA03EFD')
