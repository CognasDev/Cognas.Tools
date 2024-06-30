CREATE PROCEDURE [dbo].[AllMusic_LabelsStats_Pivot]

AS

SET NOCOUNT ON;

--Select album count statistic for given labels (AfterGlo, Dedicated, EBV and Moving Shadow)
SELECT
	'Album Count' AS [LabelStatistic],
	CAST([AfterGlo] AS VARCHAR(250))		AS [AfterGlo],
	CAST([Dedicated] AS VARCHAR(250))		AS [Dedicated],
	CAST([EBV] AS VARCHAR(250))				AS [EBV],
	CAST([EBV] AS VARCHAR(250))				AS [Moving Shadow]
FROM 
(
	SELECT [dbo].[Labels].[Name], [AlbumId] FROM [dbo].[Albums]
	INNER JOIN [dbo].[Labels] ON [dbo].[Labels].[LabelId] = [dbo].[Albums].[LabelId]
) As Temp
PIVOT
(
	COUNT ([AlbumId])
	FOR [Name] IN
	(
		[AfterGlo],
		[Dedicated],
		[EBV],
		[Moving Shadow]
	)
) As [AlbumCountPivot]
UNION
--Select last release date for given labels (AfterGlo, Dedicated, EBV and Moving Shadow)
SELECT
	'Last Release',
	CONVERT(CHAR(10),[AfterGlo],126),
	CONVERT(CHAR(10),[Dedicated],126),
	CONVERT(CHAR(10),[EBV],126),
	CONVERT(CHAR(10),[Moving Shadow],126)
FROM 
(
	SELECT [dbo].[Labels].[Name], [ReleaseDate] FROM [dbo].[Albums]
	INNER JOIN [dbo].[Labels] ON [dbo].[Labels].[LabelId] = [dbo].[Albums].[LabelId]
) As Temp
PIVOT
(
	MAX ([ReleaseDate])
	FOR [Name] IN
	(
		[AfterGlo],
		[Dedicated],
		[EBV],
		[Moving Shadow]
	)
) As [ReleaseDatePivot];