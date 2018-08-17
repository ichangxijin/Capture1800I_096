CREATE PROCEDURE [dbo].[SP_tbOrganContour_SELECT_PHASE]

	@Phase NVARCHAR(50),

	@StudyID NVARCHAR(50),

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(2000)

	SET @Sql ='USE '+@DBName 

    IF(@Phase !='0')

	BEGIN

		SET @Sql =@Sql +' SELECT tbOrganContour.* FROM tbOrganContour INNER JOIN tbSliceImage 

					ON tbOrganContour.SliceID = tbSliceImage.TPID AND tbSliceImage.Phase = '

				   + @Phase +' AND tbOrganContour.StudyID = '+@StudyID

	END

	ELSE

	BEGIN

		SET @Sql =@Sql +' SELECT tbOrganContour.* FROM tbOrganContour INNER JOIN tbSliceImage 

					ON tbOrganContour.SliceID = tbSliceImage.TPID 

					AND (tbSliceImage.Phase = 0 OR tbSliceImage.Phase IS NULL)'

					+' AND tbOrganContour.StudyID = '+@StudyID

print @Sql

	END

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: tbDVH_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:45:43

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:45:43	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbDVH_DELETE

	 @TPID	BIGINT

	,@DBName NVARCHAR(100),

	@UpdateTime TIMESTAMP,

	@ApplyRowVersion BIT

AS

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbDVH

		where TPID = @TPID AND UpdateTime=@UpdateTime'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT,

		@UpdateTime TIMESTAMP',	

		@DBName,

		@TPID,

		@UpdateTime

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbDVH

		where TPID = @TPID'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ',	

		@DBName,

		@TPID

	END

go
/******************************************************************

* ÂêçÁß∞: tbDose_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:45:18

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:45:18	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbDose_DELETE

	 @TPID	BIGINT

	,@DBName NVARCHAR(100),

	@UpdateTime TIMESTAMP,

	@ApplyRowVersion BIT

AS

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbDose

		where TPID = @TPID AND UpdateTime=@UpdateTime'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ,

		@UpdateTime TIMESTAMP',	

		@DBName,

		@TPID,

		@UpdateTime

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbDose

		where TPID = @TPID'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ',	

		@DBName,

		@TPID

	END

go

CREATE PROCEDURE [dbo].[SP_tbSliceImage_SELECT_PHASE]

    @InterpolateType NVARCHAR(5),

	@Phase NVARCHAR(50),

	@StudyID NVARCHAR(50),

	@DBName NVARCHAR(100)

AS
    
    DECLARE @Con NVARCHAR(1000)
    IF(@InterpolateType = 3)--ALL
	BEGIN
       SET @Con=''
    END
    ELSE IF(@InterpolateType=2)--InterpolateORIGINAL
	BEGIN
       SET @Con=' AND Interpolate = '+@InterpolateType
    END
    ELSE IF(@InterpolateType=0)--ORIGINAL
	BEGIN
       SET @Con=' AND (Interpolate = 0 OR Interpolate = 2 OR Interpolate IS NULL)'
    END
    ELSE IF(@InterpolateType=1)--InterpolateORIGINAL and Interpolate
	BEGIN
        SET @Con=' AND (Interpolate = 1 OR Interpolate = 2)'
    END

	DECLARE @Sql NVARCHAR(2000)

	SET @Sql ='USE '+@DBName 

    IF(@Phase !='0')

	BEGIN

		SET @Sql =@Sql +' SELECT * FROM tbSliceImage WHERE Phase = '

				   + @Phase +' AND StudyID = '+@StudyID + @Con

	END

	ELSE

	BEGIN

		SET @Sql =@Sql +' SELECT * FROM tbSliceImage 

					WHERE (tbSliceImage.Phase = 0 OR tbSliceImage.Phase IS NULL)'

					+' AND StudyID = '+@StudyID + @Con

	END

	EXEC(@Sql)

go
CREATE PROCEDURE dbo.getHospitalName 

	@HospitalID NVARCHAR(20)

AS	

    EXEC('SELECT HospitalID,Name FROM AutoManager.DBO.Hospital WHERE HospitalID LIKE '+''''+@HospitalID+'''')

	RETURN

go
--µº»Î“ª◊È4D ˝æ›

CREATE procedure [dbo].[updatePhaseTime]

@tpid     nvarchar(max),--–Õ»Á£∫1£¨2£¨3£¨4£¨5

@phase    nvarchar(max),--–Õ»Á£∫1£¨2£¨3£¨4£¨5

@time     nvarchar(max),--–Õ»Á£∫1£¨2£¨3£¨4£¨5

@dbName   varchar(100)  -- ˝æ›ø‚√˚≥∆

as

begin

declare @id VARCHAR(50)

declare @p VARCHAR(50)

declare @t VARCHAR(50)

declare @er  nvarchar(max)

set @er = ''

--∑÷¿Îµ•∏ˆ‘™¿¥∏¸–¬ ˝æ›ø‚÷–œ‡”¶µƒº«¬º

while(charindex(',',@tpid) > 0)

begin

set @id = substring(@tpid,1,charindex(',',@tpid)-1)

set @tpid = substring(@tpid,charindex(',',@tpid) +1,len(@tpid))

set @p = substring(@phase,1,charindex(',',@phase)-1)

set @phase = substring(@phase,charindex(',',@phase) +1,len(@phase))

set @t = substring(@time,1,charindex(',',@time)-1)

set @time = substring(@time,charindex(',',@time) +1,len(@time))

declare @sql varchar(1000)

set @sql = 'use ' + @dbName + ' '

set @sql = @sql + '

update tbSliceImage

set TimeMs = ' + @t +',

    Phase = ' + @p + '

where TPID = ' + @id 

--÷¥––

exec(@sql)

--º«¬º≥ˆ¥Ì

IF(@@ERROR = 0)

BEGIN

SET @ER = @ER + ',' + @ID

END

end

--◊Ó∫Û“ª‘™µƒ∏¸–¬

set @sql = 'use ' + @dbName + ' '

set @sql = @sql + '

update tbSliceImage

set TimeMs = ' + @time +',

    Phase = ' + @phase + '

where TPID = ' + @tpid 

exec(@sql)

IF(@@ERROR = 0)

BEGIN

SET @ER = @ER + ',' + @ID

END

end

go
/******************************************************************

* ÂêçÁß∞: SP_tbBeamOrient_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:44:04

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:44:04	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbBeamOrient_UPDATE

	@TPID	BIGINT,

	@OrientConstraintName	VARCHAR(30),

	@AngleFrom	FLOAT,

	@AngleTo	FLOAT,

	@Clockwise	BIT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@PlanID	BIGINT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)   ,

	@ApplyRowVersion BIT

 AS 

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' UPDATE tbBeamOrient SET

		TPID = @TPID,

		OrientConstraintName = @OrientConstraintName,

		AngleFrom = @AngleFrom,

		AngleTo = @AngleTo,

		Clockwise = @Clockwise,

		StudyID = @StudyID,

		Course = @Course,

		[PlanID] = @PlanID

		where TPID = @TPID AND UpdateTime=@UpdateTime

		SELECT @UpdateTime = UpdateTime FROM tbBeamOrient WHERE TPID=@TPID'

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' UPDATE tbBeamOrient SET

		TPID = @TPID,

		OrientConstraintName = @OrientConstraintName,

		AngleFrom = @AngleFrom,

		AngleTo = @AngleTo,

		Clockwise = @Clockwise,

		StudyID = @StudyID,

		Course = @Course,

		[PlanID] = @PlanID

		where TPID = @TPID

		SELECT @UpdateTime = UpdateTime FROM tbBeamOrient WHERE TPID=@TPID'

	END

	EXECUTE  sp_executesql @sql,N'

	@TPID	BIGINT ,

	@OrientConstraintName	VARCHAR(30),

	@AngleFrom	FLOAT,

	@AngleTo	FLOAT,

	@Clockwise	BIT,

	@StudyID	BIGINT ,

	@Course	VARCHAR(10),

	@PlanID	BIGINT ,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@TPID,

	@OrientConstraintName,

	@AngleFrom,

	@AngleTo,

	@Clockwise,

	@StudyID,

	@Course,

	@PlanID,

	@UpdateTime OUTPUT,

	@DBName   

go
CREATE PROCEDURE SP_updatePhaseTimeInStudy

	@TPID	INT,

	@PHASE   IMAGE,

	@ANP     IMAGE,

	@DBName NVARCHAR(100)   

 AS 

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' UPDATE tbStudy SET

	Phase = @PHASE,

	ANP = @ANP

	where TPID = @TPID'

	EXECUTE  sp_executesql @sql,N'

	@TPID INT,

	@PHASE IMAGE,

	@ANP IMAGE,

	@DBName NVARCHAR(100)',

	@TPID,

	@PHASE,

	@ANP,

	@DBName   

	SELECT @@ERROR
go

CREATE  PROCEDURE SP_tbDVH_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM tbDVH'

	EXEC(@Sql)

go
/******************************************************************

* √˚≥∆: SP_User_UPDATE

* ◊˜’ﬂ: Topslane

*  ±º‰: 2006-9-14 14:45:30

*

* -----------------------------------------------------------------

* ∞Ê±æ		 ±º‰			◊˜’ﬂ		±∏◊¢

*

* V2.00		2006-9-14 14:45:30	LZH		¥¥Ω®

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_User_UPDATE

    @ID INT,

	@HospitalID	INT,

	@Name	NVARCHAR(60),

	@Password	NVARCHAR(28),

	@Type	NVARCHAR(36),

	@RegisterDate	SMALLDATETIME,

	@DBName NVARCHAR(100)   

 AS 

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

		SET @Sql=@Sql+' UPDATE User SET

		HospitalID = @HospitalID,

		Name = @Name,

		Password = @Password,

		Type = @Type,

		RegisterDate = @RegisterDate WHERE ID=@ID'

	EXECUTE  sp_executesql @sql,N'

	@HospitalID	INT,

	@Name	NVARCHAR(60),

	@Password	NVARCHAR(28),

	@Type	NVARCHAR(36),

	@RegisterDate	SMALLDATETIME,

	@DBName NVARCHAR(100)',

	@HospitalID,

	@Name,

	@Password,

	@Type,

	@RegisterDate,

	@DBName   

go

CREATE  PROCEDURE SP_tbDose_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM tbDose'

	EXEC(@Sql)

go
CREATE  PROCEDURE [dbo].[SP_User_SELECT]

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM [User]'

	EXEC(@Sql)

go

CREATE  PROCEDURE SP_tbBEV_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM tbBEV'

	EXEC(@Sql)

go
/******************************************************************

* √˚≥∆: SP_User_INSERT

* ◊˜’ﬂ: Topslane

*  ±º‰: 2006-9-14 14:45:30

*

* -----------------------------------------------------------------

* ∞Ê±æ		 ±º‰			◊˜’ﬂ		±∏◊¢

*

* V2.00		2006-9-14 14:45:30	LZH		¥¥Ω®

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_User_INSERT

	@ID	INT OUTPUT,

	@HospitalID	INT,

	@Name	NVARCHAR(60),

	@Password	NVARCHAR(28),

	@Type	NVARCHAR(36),

	@RegisterDate	SMALLDATETIME,

	@DBName NVARCHAR(100)

 AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO User

	(

		HospitalID,

		Name,

		Password,

		Type,

		RegisterDate

	)

	VALUES

	(

		@HospitalID,

		@Name,

		@Password,

		@Type,

		@RegisterDate

	)

	SET @ID=SCOPE_IDENTITY()'

	EXECUTE  sp_executesql @sql,N'

	@ID	INT OUTPUT,

	@HospitalID	INT,

	@Name	NVARCHAR(60),

	@Password	NVARCHAR(28),

	@Type	NVARCHAR(36),

	@RegisterDate	SMALLDATETIME,

	@DBName NVARCHAR(100)',

	@ID OUTPUT,

	@HospitalID,

	@Name,

	@Password,

	@Type,

	@RegisterDate,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: tbBeamOrient_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:44:04

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:44:04	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbBeamOrient_DELETE

	 @TPID	BIGINT

	,@DBName NVARCHAR(100),

	@UpdateTime TIMESTAMP,

	@ApplyRowVersion BIT

AS

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbBeamOrient

		where TPID = @TPID AND UpdateTime=@UpdateTime'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ,

		@UpdateTime TIMESTAMP',	

		@DBName,

		@TPID,

		@UpdateTime

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbBeamOrient

		where TPID = @TPID'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ',	

		@DBName,

		@TPID

	END

go
/******************************************************************

* ÂêçÁß∞: SP_tbFrameCoord_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:46:01

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:46:01	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE [dbo].[SP_tbFrameCoord_INSERT]

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@Type	VARCHAR(30),

	@StudyID	BIGINT,

	@SliceID	BIGINT,

    @SlicePositionmm	FLOAT,

    @Points  IMAGE,

	@FixedNumber	SMALLINT,

	@ViewF	VARCHAR(980),

	@ViewU	VARCHAR(980),

	@ScreenF	VARCHAR(980),

	@ScreenU	VARCHAR(980),

	@StandardF	VARCHAR(980),

	@StandardU	VARCHAR(980),

	@AdjustedZF	VARCHAR(980),

	@AdjustedZU	VARCHAR(980),

	@DetectDate	DATETIME,

	@Note	VARCHAR(40),

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)

 AS

	IF(@TPID is Null)

	BEGIN

		SELECT @TPID=-1

	END

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO tbFrameCoord

	(

		TPID,

		Type,

		StudyID,

		SliceID,

		SlicePositionmm,

		Points,

		FixedNumber,

		ViewF,

		ViewU,

		ScreenF,

		ScreenU,

		StandardF,

		StandardU,

		AdjustedZF,

		AdjustedZU,

		DetectDate,

		Note

	)

	VALUES

	(

		@TPID,

		@Type,

		@StudyID,

		@SliceID,

	    @SlicePositionmm,

	    @Points ,

		@FixedNumber,

		@ViewF,

		@ViewU,

		@ScreenF,

		@ScreenU,

		@StandardF,

		@StandardU,

		@AdjustedZF,

		@AdjustedZU,

		@DetectDate,

		@Note

	)

	SET @AutoID=SCOPE_IDENTITY()

	SELECT @TPID= TPID,@UpdateTime = UpdateTime FROM tbFrameCoord WHERE AutoID=SCOPE_IDENTITY()

	IF (@TPID =-1)

	BEGIN

		SET @TPID=SCOPE_IDENTITY()

		UPDATE tbFrameCoord SET TPID=@TPID  WHERE AutoID=@TPID

	SELECT @UpdateTime = UpdateTime FROM tbFrameCoord WHERE AutoID=SCOPE_IDENTITY()

	END'

	EXECUTE  sp_executesql @sql,N'

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@Type	VARCHAR(30),

	@StudyID	BIGINT ,

	@SliceID	BIGINT ,

	@SlicePositionmm FLOAT,

	@Points  IMAGE,

	@FixedNumber	SMALLINT,

	@ViewF	VARCHAR(980),

	@ViewU	VARCHAR(980),

	@ScreenF	VARCHAR(980),

	@ScreenU	VARCHAR(980),

	@StandardF	VARCHAR(980),

	@StandardU	VARCHAR(980),

	@AdjustedZF	VARCHAR(980),

	@AdjustedZU	VARCHAR(980),

	@DetectDate	DATETIME,

	@Note	VARCHAR(40),

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@AutoID OUTPUT,

	@TPID OUTPUT,

	@Type,

	@StudyID,

	@SliceID,

	@SlicePositionmm,

	@Points,

	@FixedNumber,

	@ViewF,

	@ViewU,

	@ScreenF,

	@ScreenU,

	@StandardF,

	@StandardU,

	@AdjustedZF,

	@AdjustedZU,

	@DetectDate,

	@Note,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* √˚≥∆: User_DELETE

* ◊˜’ﬂ: Topslane

*  ±º‰: 2006-9-14 14:45:30

*

* -----------------------------------------------------------------

* ∞Ê±æ		 ±º‰			◊˜’ﬂ		±∏◊¢

*

* V2.00		2006-9-14 14:45:30	LZH		¥¥Ω®

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_User_DELETE

    @ID INT,

	@DBName NVARCHAR(100)

AS

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

		SET @Sql=@Sql+' DELETE FROM User WHERE ID =@ID'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@ID',	

		@DBName,

		@ID

go
/******************************************************************

* ÂêçÁß∞: SP_tbBlock_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:44:29

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:44:29	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbBlock_INSERT

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@PlanID	BIGINT,

	@BeamID	BIGINT,

	@Type	VARCHAR(9),

	@BlockName	VARCHAR(15),

	@BlockNameAlias	VARCHAR(20),

	@Margincm	FLOAT,

	@Transmission	FLOAT,

	@SBD	FLOAT,

	@PenumbraInsideField	FLOAT,

	@PenumbraOutsideField	FLOAT,

	@TrayFactor	FLOAT,

	@Points	IMAGE,

	@ApertureType	VARCHAR(10),

	@ConeOrMLCName	VARCHAR(30),

	@LeftMargin	FLOAT,

	@RightMargin	FLOAT,

	@TopMargin	FLOAT,

	@BottomMargin	FLOAT,

	@OrganMargin	FLOAT,

	@TargetMargin	FLOAT,

	@NormOrgan	VARCHAR(20),

	@MarginType	INT,

	@AperBlkProject	IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)

 AS

	IF(@TPID is Null)

	BEGIN

		SELECT @TPID=-1

	END

	DECLARE @Sql NVARCHAR(3000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO tbBlock

	(

		TPID,

		StudyID,

		Course,

		[PlanID],

		BeamID,

		Type,

		BlockName,

		BlockNameAlias,

		Margincm,

		Transmission,

		SBD,

		PenumbraInsideField,

		PenumbraOutsideField,

		TrayFactor,

		Points,

		ApertureType,

		ConeOrMLCName,

		LeftMargin,

		RightMargin,

		TopMargin,

		BottomMargin,

		OrganMargin,

		TargetMargin,

		NormOrgan,

		MarginType,

		AperBlkProject

	)

	VALUES

	(

		@TPID,

		@StudyID,

		@Course,

		@PlanID,

		@BeamID,

		@Type,

		@BlockName,

		@BlockNameAlias,

		@Margincm,

		@Transmission,

		@SBD,

		@PenumbraInsideField,

		@PenumbraOutsideField,

		@TrayFactor,

		@Points,

		@ApertureType,

		@ConeOrMLCName,

		@LeftMargin,

		@RightMargin,

		@TopMargin,

		@BottomMargin,

		@OrganMargin,

		@TargetMargin,

		@NormOrgan,

		@MarginType,

		@AperBlkProject

	)

	SET @AutoID=SCOPE_IDENTITY()

	SELECT @TPID= TPID,@UpdateTime = UpdateTime FROM tbBlock WHERE AutoID=SCOPE_IDENTITY()

	IF (@TPID =-1)

	BEGIN

		SET @TPID=SCOPE_IDENTITY()

		UPDATE tbBlock SET TPID=@TPID  WHERE AutoID=@TPID

	SELECT @UpdateTime = UpdateTime FROM tbBlock WHERE AutoID=SCOPE_IDENTITY()

	END'

	EXECUTE  sp_executesql @sql,N'

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@StudyID	BIGINT ,

	@Course	VARCHAR(10),

	@PlanID	BIGINT ,

	@BeamID	BIGINT ,

	@Type	VARCHAR(9),

	@BlockName	VARCHAR(15),

	@BlockNameAlias	VARCHAR(20),

	@Margincm	FLOAT,

	@Transmission	FLOAT,

	@SBD	FLOAT,

	@PenumbraInsideField	FLOAT,

	@PenumbraOutsideField	FLOAT,

	@TrayFactor	FLOAT,

	@Points	IMAGE,

	@ApertureType	VARCHAR(10),

	@ConeOrMLCName	VARCHAR(30),

	@LeftMargin	FLOAT,

	@RightMargin	FLOAT,

	@TopMargin	FLOAT,

	@BottomMargin	FLOAT,

	@OrganMargin	FLOAT,

	@TargetMargin	FLOAT,

	@NormOrgan	VARCHAR(20),

	@MarginType	INT,

	@AperBlkProject	IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@AutoID OUTPUT,

	@TPID OUTPUT,

	@StudyID,

	@Course,

	@PlanID,

	@BeamID,

	@Type,

	@BlockName,

	@BlockNameAlias,

	@Margincm,

	@Transmission,

	@SBD,

	@PenumbraInsideField,

	@PenumbraOutsideField,

	@TrayFactor,

	@Points,

	@ApertureType,

	@ConeOrMLCName,

	@LeftMargin,

	@RightMargin,

	@TopMargin,

	@BottomMargin,

	@OrganMargin,

	@TargetMargin,

	@NormOrgan,

	@MarginType,

	@AperBlkProject,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_tbFrameCoord_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:46:01

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:46:01	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbFrameCoord_UPDATE

	@TPID	BIGINT,

	@Type	VARCHAR(30),

	@StudyID	BIGINT,

	@SliceID	BIGINT,

	@SlicePositionmm	FLOAT,

	@Points  IMAGE,

	@FixedNumber	SMALLINT,

	@ViewF	VARCHAR(980),

	@ViewU	VARCHAR(980),

	@ScreenF	VARCHAR(980),

	@ScreenU	VARCHAR(980),

	@StandardF	VARCHAR(980),

	@StandardU	VARCHAR(980),

	@AdjustedZF	VARCHAR(980),

	@AdjustedZU	VARCHAR(980),

	@DetectDate	DATETIME,

	@Note	VARCHAR(40),

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)   ,

	@ApplyRowVersion BIT

 AS 

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' UPDATE tbFrameCoord SET

		TPID = @TPID,

		Type = @Type,

		StudyID = @StudyID,

		SliceID = @SliceID,

		SlicePositionmm = @SlicePositionmm,

		Points = @Points,

		FixedNumber = @FixedNumber,

		ViewF = @ViewF,

		ViewU = @ViewU,

		ScreenF = @ScreenF,

		ScreenU = @ScreenU,

		StandardF = @StandardF,

		StandardU = @StandardU,

		AdjustedZF = @AdjustedZF,

		AdjustedZU = @AdjustedZU,

		DetectDate = @DetectDate,

		Note = @Note

		where TPID = @TPID AND UpdateTime=@UpdateTime

		SELECT @UpdateTime = UpdateTime FROM tbFrameCoord WHERE TPID=@TPID'

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' UPDATE tbFrameCoord SET

		TPID = @TPID,

		Type = @Type,

		StudyID = @StudyID,

		SliceID = @SliceID,

		SlicePositionmm = @SlicePositionmm,

		Points = @Points,

		FixedNumber = @FixedNumber,

		ViewF = @ViewF,

		ViewU = @ViewU,

		ScreenF = @ScreenF,

		ScreenU = @ScreenU,

		StandardF = @StandardF,

		StandardU = @StandardU,

		AdjustedZF = @AdjustedZF,

		AdjustedZU = @AdjustedZU,

		DetectDate = @DetectDate,

		Note = @Note

		where TPID = @TPID

		SELECT @UpdateTime = UpdateTime FROM tbFrameCoord WHERE TPID=@TPID'

	END

	EXECUTE  sp_executesql @sql,N'

	@TPID	BIGINT ,

	@Type	VARCHAR(30),

	@StudyID	BIGINT ,

	@SliceID	BIGINT ,

	@SlicePositionmm	FLOAT,

    @Points IMAGE,

	@FixedNumber	SMALLINT,

	@ViewF	VARCHAR(980),

	@ViewU	VARCHAR(980),

	@ScreenF	VARCHAR(980),

	@ScreenU	VARCHAR(980),

	@StandardF	VARCHAR(980),

	@StandardU	VARCHAR(980),

	@AdjustedZF	VARCHAR(980),

	@AdjustedZU	VARCHAR(980),

	@DetectDate	DATETIME,

	@Note	VARCHAR(40),

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@TPID,

	@Type,

	@StudyID,

	@SliceID,

	@SlicePositionmm,

	@Points,

	@FixedNumber,

	@ViewF,

	@ViewU,

	@ScreenF,

	@ScreenU,

	@StandardF,

	@StandardU,

	@AdjustedZF,

	@AdjustedZU,

	@DetectDate,

	@Note,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_tbBlock_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:44:29

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:44:29	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbBlock_UPDATE

	@TPID	BIGINT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@PlanID	BIGINT,

	@BeamID	BIGINT,

	@Type	VARCHAR(9),

	@BlockName	VARCHAR(15),

	@BlockNameAlias	VARCHAR(20),

	@Margincm	FLOAT,

	@Transmission	FLOAT,

	@SBD	FLOAT,

	@PenumbraInsideField	FLOAT,

	@PenumbraOutsideField	FLOAT,

	@TrayFactor	FLOAT,

	@Points	IMAGE,

	@ApertureType	VARCHAR(10),

	@ConeOrMLCName	VARCHAR(30),

	@LeftMargin	FLOAT,

	@RightMargin	FLOAT,

	@TopMargin	FLOAT,

	@BottomMargin	FLOAT,

	@OrganMargin	FLOAT,

	@TargetMargin	FLOAT,

	@NormOrgan	VARCHAR(20),

	@MarginType	INT,

	@AperBlkProject	IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)   ,

	@ApplyRowVersion BIT

 AS 

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(3000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' UPDATE tbBlock SET

		TPID = @TPID,

		StudyID = @StudyID,

		Course = @Course,

		[PlanID] = @PlanID,

		BeamID = @BeamID,

		Type = @Type,

		BlockName = @BlockName,

		BlockNameAlias = @BlockNameAlias,

		Margincm = @Margincm,

		Transmission = @Transmission,

		SBD = @SBD,

		PenumbraInsideField = @PenumbraInsideField,

		PenumbraOutsideField = @PenumbraOutsideField,

		TrayFactor = @TrayFactor,

		Points = @Points,

		ApertureType = @ApertureType,

		ConeOrMLCName = @ConeOrMLCName,

		LeftMargin = @LeftMargin,

		RightMargin = @RightMargin,

		TopMargin = @TopMargin,

		BottomMargin = @BottomMargin,

		OrganMargin = @OrganMargin,

		TargetMargin = @TargetMargin,

		NormOrgan = @NormOrgan,

		MarginType = @MarginType,

		AperBlkProject = @AperBlkProject

		where TPID = @TPID AND UpdateTime=@UpdateTime

		SELECT @UpdateTime = UpdateTime FROM tbBlock WHERE TPID=@TPID'

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' UPDATE tbBlock SET

		TPID = @TPID,

		StudyID = @StudyID,

		Course = @Course,

		[PlanID] = @PlanID,

		BeamID = @BeamID,

		Type = @Type,

		BlockName = @BlockName,

		BlockNameAlias = @BlockNameAlias,

		Margincm = @Margincm,

		Transmission = @Transmission,

		SBD = @SBD,

		PenumbraInsideField = @PenumbraInsideField,

		PenumbraOutsideField = @PenumbraOutsideField,

		TrayFactor = @TrayFactor,

		Points = @Points,

		ApertureType = @ApertureType,

		ConeOrMLCName = @ConeOrMLCName,

		LeftMargin = @LeftMargin,

		RightMargin = @RightMargin,

		TopMargin = @TopMargin,

		BottomMargin = @BottomMargin,

		OrganMargin = @OrganMargin,

		TargetMargin = @TargetMargin,

		NormOrgan = @NormOrgan,

		MarginType = @MarginType,

		AperBlkProject = @AperBlkProject

		where TPID = @TPID

		SELECT @UpdateTime = UpdateTime FROM tbBlock WHERE TPID=@TPID'

	END

	EXECUTE  sp_executesql @sql,N'

	@TPID	BIGINT ,

	@StudyID	BIGINT ,

	@Course	VARCHAR(10),

	@PlanID	BIGINT ,

	@BeamID	BIGINT ,

	@Type	VARCHAR(9),

	@BlockName	VARCHAR(15),

	@BlockNameAlias	VARCHAR(20),

	@Margincm	FLOAT,

	@Transmission	FLOAT,

	@SBD	FLOAT,

	@PenumbraInsideField	FLOAT,

	@PenumbraOutsideField	FLOAT,

	@TrayFactor	FLOAT,

	@Points	IMAGE,

	@ApertureType	VARCHAR(10),

	@ConeOrMLCName	VARCHAR(30),

	@LeftMargin	FLOAT,

	@RightMargin	FLOAT,

	@TopMargin	FLOAT,

	@BottomMargin	FLOAT,

	@OrganMargin	FLOAT,

	@TargetMargin	FLOAT,

	@NormOrgan	VARCHAR(20),

	@MarginType	INT,

	@AperBlkProject	IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@TPID,

	@StudyID,

	@Course,

	@PlanID,

	@BeamID,

	@Type,

	@BlockName,

	@BlockNameAlias,

	@Margincm,

	@Transmission,

	@SBD,

	@PenumbraInsideField,

	@PenumbraOutsideField,

	@TrayFactor,

	@Points,

	@ApertureType,

	@ConeOrMLCName,

	@LeftMargin,

	@RightMargin,

	@TopMargin,

	@BottomMargin,

	@OrganMargin,

	@TargetMargin,

	@NormOrgan,

	@MarginType,

	@AperBlkProject,

	@UpdateTime OUTPUT,

	@DBName   

go
--Õ®π˝POSITIONªÒ»°SLICE ID

CREATE procedure dbo.SP_GetSliceImageID

@POSS     nvarchar(max),--–Õ»Á£∫1£¨2£¨3£¨4£¨5

@dbName   Nvarchar(100)  -- ˝æ›ø‚√˚≥∆

as

begin

DECLARE @POS NVARCHAR(50); -- µ•∏ˆPOS

DECLARE @RES NVARCHAR(MAX); -- Ω·π˚

SET @RES = '';

while(charindex(',',@POSS) > 0)

begin

set @POS = substring(@POSS,1,charindex(',',@POSS)-1)

set @POSS = substring(@POSS,charindex(',',@POSS) +1,len(@POSS))

DECLARE @SQL NVARCHAR(200);

SET @Sql=N'USE '+@DBName;

SET @Sql=@Sql+N' select @R = (select TPID FROM tbSliceImage WHERE SlicePositionmm = @POS)'; 

DECLARE @DEF nvarchar(200);

SET @DEF = N'@DBNAME  VARCHAR(50),@POS VARCHAR(50),@R VARCHAR(50) OUTPUT';

DECLARE @TMP VARCHAR(50);

EXECUTE sp_executesql @Sql,@DEF,@DBNAME,@POS ,@TMP OUTPUT;

IF(@TMP IS NULL)

SET @TMP = '-1';

IF(@RES != '')

SET @RES = @RES +','+ @TMP;

ELSE

SET @RES = @TMP;

END

--◊Ó∫Û“ª∏ˆ‘™Àÿ

SET @Sql=N'USE '+@DBName;

SET @Sql=@Sql+N' select @R = (select TPID FROM tbSliceImage WHERE SlicePositionmm = @POS)'; 

SET @DEF = N'@DBNAME  VARCHAR(50),@POS VARCHAR(50),@R VARCHAR(50) OUTPUT';

EXECUTE sp_executesql @Sql,@DEF,@DBNAME,@POSS ,@TMP OUTPUT;

IF(@TMP IS NULL)

SET @TMP = '-1';

SET @RES = @RES +','+ @TMP;

SELECT @RES

end

go

CREATE  PROCEDURE SP_tbBeamOrient_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM tbBeamOrient'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: tbFrameCoord_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:46:01

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:46:01	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbFrameCoord_DELETE

	 @TPID	BIGINT

	,@DBName NVARCHAR(100),

	@UpdateTime TIMESTAMP,

	@ApplyRowVersion BIT

AS

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbFrameCoord

		where TPID = @TPID AND UpdateTime=@UpdateTime'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ,

		@UpdateTime TIMESTAMP',	

		@DBName,

		@TPID,

		@UpdateTime

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbFrameCoord

		where TPID = @TPID'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ',	

		@DBName,

		@TPID

	END

go
/******************************************************************

* ÂêçÁß∞: SP_tbDose_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:45:18

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:45:18	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbDose_INSERT

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@BeamID	BIGINT,

	@SliceID	BIGINT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@PlanID	BIGINT,

	@SlicePositionmm	FLOAT,

	@RowCount	INT,

	@ColumnCount	INT,

	@SliceMaxDose	FLOAT,

	@TargetMaxDose	FLOAT,

	@TargetMinDose	FLOAT,

	@TargetMeanDose	FLOAT,

	@Data	IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)

 AS

    SET TRANSACTION ISOLATION LEVEL SERIALIZABLE

	IF(@TPID is Null)

	BEGIN

		SELECT @TPID=-1

	END

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' BEGIN TRANSACTION 

	 INSERT INTO tbDose

	(

		TPID,

		BeamID,

		SliceID,

		StudyID,

		Course,

		[PlanID],

		SlicePositionmm,

		[RowCount],

		ColumnCount,

		SliceMaxDose,

		TargetMaxDose,

		TargetMinDose,

		TargetMeanDose,

		Data

	)

	VALUES

	(

		@TPID,

		@BeamID,

		@SliceID,

		@StudyID,

		@Course,

		@PlanID,

		@SlicePositionmm,

		@RowCount,

		@ColumnCount,

		@SliceMaxDose,

		@TargetMaxDose,

		@TargetMinDose,

		@TargetMeanDose,

		@Data

	)

	SET @AutoID=SCOPE_IDENTITY()

	SELECT @TPID= TPID,@UpdateTime = UpdateTime FROM tbDose WHERE AutoID=@AutoID

	IF(@TPID=-1)

		BEGIN

			SET @TPID=@AutoID

			UPDATE tbDose SET TPID=@TPID  WHERE TPID=-1

			SELECT @UpdateTime = UpdateTime FROM tbDose WHERE TPID=@TPID

		END

    IF (@@error <> 0)

	BEGIN

		ROLLBACK TRANSACTION

	END	

	ELSE

	BEGIN

	COMMIT TRANSACTION

	END	'

	EXECUTE  sp_executesql @sql,N'

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@BeamID	BIGINT,

	@SliceID	BIGINT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@PlanID	BIGINT ,

	@SlicePositionmm	FLOAT,

	@RowCount	INT,

	@ColumnCount	INT,

	@SliceMaxDose	FLOAT,

	@TargetMaxDose	FLOAT,

	@TargetMinDose	FLOAT,

	@TargetMeanDose	FLOAT,

	@Data	IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@AutoID OUTPUT,

	@TPID OUTPUT,

	@BeamID,

	@SliceID,

	@StudyID,

	@Course,

	@PlanID,

	@SlicePositionmm,

	@RowCount,

	@ColumnCount,

	@SliceMaxDose,

	@TargetMaxDose,

	@TargetMinDose,

	@TargetMeanDose,

	@Data,

	@UpdateTime OUTPUT,

	@DBName   

go
CREATE PROCEDURE [dbo].[SP_RestoreOriginalSliceAndContour] 

    @TimeList NVARCHAR(MAX),

	@PhaseList NVARCHAR(MAX),

	@StudyID NVARCHAR(20),

    @numberOfPhase INT,

    @dbName NVARCHAR(50)

AS

BEGIN

	SET NOCOUNT ON;

	DECLARE @SQL NVARCHAR(MAX)

--	SET @SQL  = 'USE ' + @dbName + ' DELETE FROM tbSliceImage WHERE TimeMs NOT IN '+ @TimeList 

--				+' AND StudyID = '+@StudyID

	SET @SQL  = 'USE ' + @dbName + ' DELETE FROM tbSliceImage WHERE Interpolate = 1'

				+' AND StudyID = '+@StudyID

	EXEC (@SQL)

	SET @SQL  = 'USE ' + @dbName + ' DELETE FROM tbOrganContour WHERE SliceID NOT IN 

				(SELECT TPID FROM tbSliceImage where StudyID = '+@StudyID+') AND StudyID = '+@StudyID

	EXEC (@SQL)

	SET @TimeList = SUBSTRING(@TimeList,2,LEN(@TimeList)-2)+ ','

	DECLARE @IndexNO INT

	DECLARE @OriginalNOIndex INT

	DECLARE @IndexValue INT

	DECLARE @OriginalValueIndex INT

	SET @OriginalNOIndex = 1

	SET @IndexNO = CHARINDEX(',', @TimeList,1) 

	SET @OriginalValueIndex = 1

	SET @IndexValue = CHARINDEX(',', @PhaseList,1) 

	WHILE (@IndexNO!=0)

	BEGIN

	   DECLARE @Time NVARCHAR(10)

	   DECLARE @Phase NVARCHAR(10)

	   SET @Time = SUBSTRING(@TimeList,@OriginalNOIndex,@IndexNO-@OriginalNOIndex)

	   SET @OriginalNOIndex = @IndexNO+1;

	   SET @Phase = SUBSTRING(@PhaseList,@OriginalValueIndex,@IndexValue-@OriginalValueIndex)

	   SET @OriginalValueIndex = @IndexValue+1;

		DECLARE @m_tmp INT

		SET @m_tmp = 0

		DECLARE @m_OnePhase FLOAT  

		SET @m_OnePhase = 1.0 * 360 / @numberOfPhase

		DECLARE @m_time INT

		SET @m_time = Floor(@Phase / @m_OnePhase)

		DECLARE @m_last FLOAT

		SET @m_last = @Phase - @m_OnePhase * @m_time

		if (@m_last > @m_OnePhase / 2)

		BEGIN

			SET @m_tmp = Floor((@m_time + 1) * @m_OnePhase)

		END

		ELSE

		BEGIN 

			SET @m_tmp = Floor(@m_time * @m_OnePhase)

		END

		SET @Phase = @m_tmp % 360

	   SET @SQL = 'USE ' + @dbName + ' UPDATE tbSliceImage SET Phase = '+ @Phase + ' WHERE TimeMs = '+ @Time

				+ 'AND StudyID = '+ @StudyID

       EXEC (@SQL)

	   SET @IndexNO = CHARINDEX(',', @TimeList,@IndexNO+1)

	   SET @IndexValue = CHARINDEX(',', @PhaseList,@IndexValue+1) 

	END

END

go

CREATE  PROCEDURE SP_tbFrameCoord_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM tbFrameCoord'

	EXEC(@Sql)

go
CREATE PROCEDURE [dbo].[SP_GetBuild] 

AS

BEGIN

	SET NOCOUNT ON;

	SELECT 1002

END

go
/******************************************************************

* ÂêçÁß∞: tbBlock_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:44:29

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:44:29	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbBlock_DELETE

	 @TPID	BIGINT

	,@DBName NVARCHAR(100),

	@UpdateTime TIMESTAMP,

	@ApplyRowVersion BIT

AS

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbBlock

		where TPID = @TPID AND UpdateTime=@UpdateTime'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ,

		@UpdateTime TIMESTAMP',	

		@DBName,

		@TPID,

		@UpdateTime

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbBlock

		where TPID = @TPID'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ',	

		@DBName,

		@TPID

	END

go

CREATE PROCEDURE SP_GetStudySliceAndOrganNameNum 

     @dbName NVARCHAR(100)

AS

BEGIN
DECLARE @SQL NVARCHAR(MAX) 
SET @SQL = N'USE '+@dbName+' ' 
SET @SQL = @SQL + N' 
SELECT A.Study, B.Images, D.Organs, E.Targets 
FROM (SELECT TPID, Study 
FROM tbStudy) AS A LEFT OUTER JOIN 
(SELECT StudyID, COUNT(DISTINCT TPID) AS Images 
FROM tbSliceImage 
GROUP BY StudyID) AS B ON A.TPID = B.StudyID LEFT OUTER JOIN 
(SELECT Top(1) TPID, StudyID 
FROM tbStruct) As C ON A.TPID = C.StudyID LEFT OUTER JOIN 
(SELECT StructID, COUNT(DISTINCT TPID) AS Organs 
FROM tbOrgan 
GROUP BY StructID) AS D ON C.TPID = D.StructID LEFT OUTER JOIN 
(SELECT StructID, COUNT(DISTINCT TPID) AS Targets 
FROM tbOrgan 
WHERE ( OrganAttribute = ''target'') 
GROUP BY StructID) AS E ON C.TPID = E.StructID' 
EXEC(@SQL) 

END

go
/******************************************************************

* ÂêçÁß∞: SP_tbIMRTSegment_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:46:18

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:46:18	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbIMRTSegment_INSERT

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@PlanID	BIGINT,

	@BeamID	BIGINT,

	@SegmentNumber	INT,

	@LeafNumber	INT,

	@SegmentIndex	INT,

	@FractionIndex	FLOAT,

	@CarriageGroup	INT,

	@SegmentCollimator	FLOAT,

	@LeftLeaves	IMAGE,

	@RightLeaves	IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)

 AS

	IF(@TPID is Null)

	BEGIN

		SELECT @TPID=-1

	END

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO tbIMRTSegment

	(

		TPID,

		StudyID,

		Course,

		[PlanID],

		BeamID,

		SegmentNumber,

		LeafNumber,

		SegmentIndex,

		FractionIndex,

		CarriageGroup,

		SegmentCollimator,

		LeftLeaves,

		RightLeaves

	)

	VALUES

	(

		@TPID,

		@StudyID,

		@Course,

		@PlanID,

		@BeamID,

		@SegmentNumber,

		@LeafNumber,

		@SegmentIndex,

		@FractionIndex,

		@CarriageGroup,

		@SegmentCollimator,

		@LeftLeaves,

		@RightLeaves

	)

	SET @AutoID=SCOPE_IDENTITY()

	SELECT @TPID= TPID,@UpdateTime = UpdateTime FROM tbIMRTSegment WHERE AutoID=SCOPE_IDENTITY()

	IF (@TPID =-1)

	BEGIN

		SET @TPID=SCOPE_IDENTITY()

		UPDATE tbIMRTSegment SET TPID=@TPID  WHERE AutoID=@TPID

	SELECT @UpdateTime = UpdateTime FROM tbIMRTSegment WHERE AutoID=SCOPE_IDENTITY()

	END'

	EXECUTE  sp_executesql @sql,N'

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@PlanID	BIGINT,

	@BeamID	BIGINT ,

	@SegmentNumber	INT,

	@LeafNumber	INT,

	@SegmentIndex	INT,

	@FractionIndex	FLOAT,

	@CarriageGroup	INT,

	@SegmentCollimator	FLOAT,

	@LeftLeaves	IMAGE,

	@RightLeaves	IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@AutoID OUTPUT,

	@TPID OUTPUT,

	@StudyID,

	@Course,

	@PlanID,

	@BeamID,

	@SegmentNumber,

	@LeafNumber,

	@SegmentIndex,

	@FractionIndex,

	@CarriageGroup,

	@SegmentCollimator,

	@LeftLeaves,

	@RightLeaves,

	@UpdateTime OUTPUT,

	@DBName   

go

CREATE  PROCEDURE SP_tbBlock_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM tbBlock'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: SP_tbIMRTSegment_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:46:18

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:46:18	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbIMRTSegment_UPDATE

	@TPID	BIGINT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@PlanID	BIGINT,

	@BeamID	BIGINT,

	@SegmentNumber	INT,

	@LeafNumber	INT,

	@SegmentIndex	INT,

	@FractionIndex	FLOAT,

	@CarriageGroup	INT,

	@SegmentCollimator	FLOAT,

	@LeftLeaves	IMAGE,

	@RightLeaves	IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)   ,

	@ApplyRowVersion BIT

 AS 

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' UPDATE tbIMRTSegment SET

		TPID = @TPID,

		StudyID = @StudyID,

		Course = @Course,

		[PlanID] = @PlanID,

		BeamID = @BeamID,

		SegmentNumber = @SegmentNumber,

		LeafNumber = @LeafNumber,

		SegmentIndex = @SegmentIndex,

		FractionIndex = @FractionIndex,

		CarriageGroup = @CarriageGroup,

		SegmentCollimator = @SegmentCollimator,

		LeftLeaves = @LeftLeaves,

		RightLeaves = @RightLeaves

		where TPID = @TPID AND UpdateTime=@UpdateTime

		SELECT @UpdateTime = UpdateTime FROM tbIMRTSegment WHERE TPID=@TPID'

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' UPDATE tbIMRTSegment SET

		TPID = @TPID,

		StudyID = @StudyID,

		Course = @Course,

		[PlanID] = @PlanID,

		BeamID = @BeamID,

		SegmentNumber = @SegmentNumber,

		LeafNumber = @LeafNumber,

		SegmentIndex = @SegmentIndex,

		FractionIndex = @FractionIndex,

		CarriageGroup = @CarriageGroup,

		SegmentCollimator = @SegmentCollimator,

		LeftLeaves = @LeftLeaves,

		RightLeaves = @RightLeaves

		where TPID = @TPID

		SELECT @UpdateTime = UpdateTime FROM tbIMRTSegment WHERE TPID=@TPID'

	END

	EXECUTE  sp_executesql @sql,N'

	@TPID	BIGINT ,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@PlanID	BIGINT,

	@BeamID	BIGINT,

	@SegmentNumber	INT,

	@LeafNumber	INT,

	@SegmentIndex	INT,

	@FractionIndex	FLOAT,

	@CarriageGroup	INT,

	@SegmentCollimator	FLOAT,

	@LeftLeaves	IMAGE,

	@RightLeaves	IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@TPID,

	@StudyID,

	@Course,

	@PlanID,

	@BeamID,

	@SegmentNumber,

	@LeafNumber,

	@SegmentIndex,

	@FractionIndex,

	@CarriageGroup,

	@SegmentCollimator,

	@LeftLeaves,

	@RightLeaves,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_tbCombineOrgan_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:44:52

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:44:52	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbCombineOrgan_INSERT

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@StudyID	BIGINT,

	@Name	NVARCHAR(40),

	@Define	NVARCHAR(600),

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)

 AS

	IF(@TPID is Null)

	BEGIN

		SELECT @TPID=-1

	END

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO tbCombineOrgan

	(

		TPID,

		StudyID,

		Name,

		Define

	)

	VALUES

	(

		@TPID,

		@StudyID,

		@Name,

		@Define

	)

	SET @AutoID=SCOPE_IDENTITY()

	SELECT @TPID= TPID,@UpdateTime = UpdateTime FROM tbCombineOrgan WHERE AutoID=SCOPE_IDENTITY()

	IF (@TPID =-1)

	BEGIN

		SET @TPID=SCOPE_IDENTITY()

		UPDATE tbCombineOrgan SET TPID=@TPID  WHERE AutoID=@TPID

	SELECT @UpdateTime = UpdateTime FROM tbCombineOrgan WHERE AutoID=SCOPE_IDENTITY()

	END'

	EXECUTE  sp_executesql @sql,N'

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@StudyID	BIGINT ,

	@Name	NVARCHAR(40),

	@Define	NVARCHAR(600),

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@AutoID OUTPUT,

	@TPID OUTPUT,

	@StudyID,

	@Name,

	@Define,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: tbIMRTSegment_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:46:18

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:46:18	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbIMRTSegment_DELETE

	 @TPID	BIGINT

	,@DBName NVARCHAR(100),

	@UpdateTime TIMESTAMP,

	@ApplyRowVersion BIT

AS

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbIMRTSegment

		where TPID = @TPID AND UpdateTime=@UpdateTime'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ,

		@UpdateTime TIMESTAMP',	

		@DBName,

		@TPID,

		@UpdateTime

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbIMRTSegment

		where TPID = @TPID'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ',	

		@DBName,

		@TPID

	END

go
/******************************************************************

* ÂêçÁß∞: SP_tbCombineOrgan_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:44:52

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:44:52	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbCombineOrgan_UPDATE

	@TPID	BIGINT,

	@StudyID	BIGINT,

	@Name	NVARCHAR(40),

	@Define	NVARCHAR(600),

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)   ,

	@ApplyRowVersion BIT

 AS 

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' UPDATE tbCombineOrgan SET

		TPID = @TPID,

		StudyID = @StudyID,

		Name = @Name,

		Define = @Define

		where TPID = @TPID AND UpdateTime=@UpdateTime

		SELECT @UpdateTime = UpdateTime FROM tbCombineOrgan WHERE TPID=@TPID'

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' UPDATE tbCombineOrgan SET

		TPID = @TPID,

		StudyID = @StudyID,

		Name = @Name,

		Define = @Define

		where TPID = @TPID

		SELECT @UpdateTime = UpdateTime FROM tbCombineOrgan WHERE TPID=@TPID'

	END

	EXECUTE  sp_executesql @sql,N'

	@TPID	BIGINT ,

	@StudyID	BIGINT ,

	@Name	NVARCHAR(40),

	@Define	NVARCHAR(600),

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@TPID,

	@StudyID,

	@Name,

	@Define,

	@UpdateTime OUTPUT,

	@DBName   

go

CREATE  PROCEDURE SP_tbIMRTSegment_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM tbIMRTSegment'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: tbCombineOrgan_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:44:52

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:44:52	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbCombineOrgan_DELETE

	 @TPID	BIGINT

	,@DBName NVARCHAR(100),

	@UpdateTime TIMESTAMP,

	@ApplyRowVersion BIT

AS

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbCombineOrgan

		where TPID = @TPID AND UpdateTime=@UpdateTime'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ,

		@UpdateTime TIMESTAMP',	

		@DBName,

		@TPID,

		@UpdateTime

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbCombineOrgan

		where TPID = @TPID'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ',	

		@DBName,

		@TPID

	END

go
/******************************************************************

* ÂêçÁß∞: SP_tbOrganContour_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:46:29

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:46:29	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbOrganContour_INSERT

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@SliceID	BIGINT,

	@OrganName	VARCHAR(18),

	@OrganAttribute	VARCHAR(18),

	@SlicePoints	IMAGE,

	@Color	VARCHAR(12),

	@SlicePositionmm	FLOAT,

	@Density	FLOAT,

	@StudyID	BIGINT,

	@Valid	BIT,

	@Formal	BIT,

	@StructUIDforDicom	VARCHAR(64),

	@OrganNumber	INT,

	@OrganSubID	INT,

	@OrganAreaCMxCM	FLOAT,

	@XminPixel	INT,

	@XmaxPixel	INT,

	@YminPixel	INT,

	@YmaxPixel	INT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)

 AS

	IF(@TPID is Null)

	BEGIN

		SELECT @TPID=-1

	END

	DECLARE @Sql NVARCHAR(MAX) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO tbOrganContour

	(

		TPID,

		SliceID,

		OrganName,

		OrganAttribute,

		SlicePoints,

		Color,

		SlicePositionmm,

		Density,

		StudyID,

		Valid,

		Formal,

		StructUIDforDicom,

		OrganNumber,

		OrganSubID,

		OrganAreaCMxCM,

		XminPixel,

		XmaxPixel,

		YminPixel,

		YmaxPixel

	)

	VALUES

	(

		@TPID,

		@SliceID,

		@OrganName,

		@OrganAttribute,

		@SlicePoints,

		@Color,

		@SlicePositionmm,

		@Density,

		@StudyID,

		@Valid,

		@Formal,

		@StructUIDforDicom,

		@OrganNumber,

		@OrganSubID,

		@OrganAreaCMxCM,

		@XminPixel,

		@XmaxPixel,

		@YminPixel,

		@YmaxPixel

	)

	SET @AutoID=SCOPE_IDENTITY()

	SELECT @TPID= TPID,@UpdateTime = UpdateTime FROM tbOrganContour WHERE AutoID=SCOPE_IDENTITY()

	IF (@TPID =-1)

	BEGIN

		SET @TPID=SCOPE_IDENTITY()

		UPDATE tbOrganContour SET TPID=@TPID  WHERE AutoID=@TPID

	SELECT @UpdateTime = UpdateTime FROM tbOrganContour WHERE AutoID=SCOPE_IDENTITY()

	END'

	EXECUTE  sp_executesql @sql,N'

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@SliceID	BIGINT,

	@OrganName	VARCHAR(18),

	@OrganAttribute	VARCHAR(18),

	@SlicePoints	IMAGE,

	@Color	VARCHAR(12),

	@SlicePositionmm	FLOAT,

	@Density	FLOAT,

	@StudyID	BIGINT,

	@Valid	BIT,

	@Formal	BIT,

	@StructUIDforDicom	VARCHAR(64),

	@OrganNumber	INT,

	@OrganSubID	INT,

	@OrganAreaCMxCM	FLOAT,

	@XminPixel	INT,

	@XmaxPixel	INT,

	@YminPixel	INT,

	@YmaxPixel	INT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@AutoID OUTPUT,

	@TPID OUTPUT,

	@SliceID,

	@OrganName,

	@OrganAttribute,

	@SlicePoints,

	@Color,

	@SlicePositionmm,

	@Density,

	@StudyID,

	@Valid,

	@Formal,

	@StructUIDforDicom,

	@OrganNumber,

	@OrganSubID,

	@OrganAreaCMxCM,

	@XminPixel,

	@XmaxPixel,

	@YminPixel,

	@YmaxPixel,

	@UpdateTime OUTPUT,

	@DBName   

go

CREATE  PROCEDURE SP_tbCombineOrgan_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM tbCombineOrgan'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: SP_tbOrganContour_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:46:29

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:46:29	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbOrganContour_UPDATE

	@TPID	BIGINT,

	@SliceID	BIGINT,

	@OrganName	VARCHAR(18),

	@OrganAttribute	VARCHAR(18),

	@SlicePoints	IMAGE,

	@Color	VARCHAR(12),

	@SlicePositionmm	FLOAT,

	@Density	FLOAT,

	@StudyID	BIGINT,

	@Valid	BIT,

	@Formal	BIT,

	@StructUIDforDicom	VARCHAR(64),

	@OrganNumber	INT,

	@OrganSubID	INT,

	@OrganAreaCMxCM	FLOAT,

	@XminPixel	INT,

	@XmaxPixel	INT,

	@YminPixel	INT,

	@YmaxPixel	INT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)   ,

	@ApplyRowVersion BIT

 AS 

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(MAX) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' UPDATE tbOrganContour SET

		TPID = @TPID,

		SliceID = @SliceID,

		OrganName = @OrganName,

		OrganAttribute = @OrganAttribute,

		SlicePoints = @SlicePoints,

		Color = @Color,

		SlicePositionmm = @SlicePositionmm,

		Density = @Density,

		StudyID = @StudyID,

		Valid = @Valid,

		Formal = @Formal,

		StructUIDforDicom = @StructUIDforDicom,

		OrganNumber = @OrganNumber,

		OrganSubID = @OrganSubID,

		OrganAreaCMxCM = @OrganAreaCMxCM,

		XminPixel = @XminPixel,

		XmaxPixel = @XmaxPixel,

		YminPixel = @YminPixel,

		YmaxPixel = @YmaxPixel

		where TPID = @TPID AND UpdateTime=@UpdateTime

		SELECT @UpdateTime = UpdateTime FROM tbOrganContour WHERE TPID=@TPID'

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' UPDATE tbOrganContour SET

		TPID = @TPID,

		SliceID = @SliceID,

		OrganName = @OrganName,

		OrganAttribute = @OrganAttribute,

		SlicePoints = @SlicePoints,

		Color = @Color,

		SlicePositionmm = @SlicePositionmm,

		Density = @Density,

		StudyID = @StudyID,

		Valid = @Valid,

		Formal = @Formal,

		StructUIDforDicom = @StructUIDforDicom,

		OrganNumber = @OrganNumber,

		OrganSubID = @OrganSubID,

		OrganAreaCMxCM = @OrganAreaCMxCM,

		XminPixel = @XminPixel,

		XmaxPixel = @XmaxPixel,

		YminPixel = @YminPixel,

		YmaxPixel = @YmaxPixel

		where TPID = @TPID

		SELECT @UpdateTime = UpdateTime FROM tbOrganContour WHERE TPID=@TPID'

	END

	EXECUTE  sp_executesql @sql,N'

	@TPID	BIGINT ,

	@SliceID	BIGINT ,

	@OrganName	VARCHAR(18),

	@OrganAttribute	VARCHAR(18),

	@SlicePoints	IMAGE,

	@Color	VARCHAR(12),

	@SlicePositionmm	FLOAT,

	@Density	FLOAT,

	@StudyID	BIGINT ,

	@Valid	BIT,

	@Formal	BIT,

	@StructUIDforDicom	VARCHAR(64),

	@OrganNumber	INT,

	@OrganSubID	INT,

	@OrganAreaCMxCM	FLOAT,

	@XminPixel	INT,

	@XmaxPixel	INT,

	@YminPixel	INT,

	@YmaxPixel	INT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@TPID,

	@SliceID,

	@OrganName,

	@OrganAttribute,

	@SlicePoints,

	@Color,

	@SlicePositionmm,

	@Density,

	@StudyID,

	@Valid,

	@Formal,

	@StructUIDforDicom,

	@OrganNumber,

	@OrganSubID,

	@OrganAreaCMxCM,

	@XminPixel,

	@XmaxPixel,

	@YminPixel,

	@YmaxPixel,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_tbCoordinate_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:45:06

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:45:06	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbCoordinate_INSERT

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@StudyID	BIGINT,

	@Type	SMALLINT,

	@OffsetX	FLOAT,

	@OffsetY	FLOAT,

	@OffsetZ	FLOAT,

	@AngleX	FLOAT,

	@AngleY	FLOAT,

	@AngleZ	FLOAT,

	@SizeX	FLOAT,

	@SizeY	FLOAT,

	@SizeZ	FLOAT,

	@Note	VARCHAR(40),

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)

 AS

	IF(@TPID is Null)

	BEGIN

		SELECT @TPID=-1

	END

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO tbCoordinate

	(

		TPID,

		StudyID,

		Type,

		OffsetX,

		OffsetY,

		OffsetZ,

		AngleX,

		AngleY,

		AngleZ,

		SizeX,

		SizeY,

		SizeZ,

		Note

	)

	VALUES

	(

		@TPID,

		@StudyID,

		@Type,

		@OffsetX,

		@OffsetY,

		@OffsetZ,

		@AngleX,

		@AngleY,

		@AngleZ,

		@SizeX,

		@SizeY,

		@SizeZ,

		@Note

	)

	SET @AutoID=SCOPE_IDENTITY()

	SELECT @TPID= TPID,@UpdateTime = UpdateTime FROM tbCoordinate WHERE AutoID=SCOPE_IDENTITY()

	IF (@TPID =-1)

	BEGIN

		SET @TPID=SCOPE_IDENTITY()

		UPDATE tbCoordinate SET TPID=@TPID  WHERE AutoID=@TPID

	SELECT @UpdateTime = UpdateTime FROM tbCoordinate WHERE AutoID=SCOPE_IDENTITY()

	END'

	EXECUTE  sp_executesql @sql,N'

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@StudyID	BIGINT ,

	@Type	SMALLINT,

	@OffsetX	FLOAT,

	@OffsetY	FLOAT,

	@OffsetZ	FLOAT,

	@AngleX	FLOAT,

	@AngleY	FLOAT,

	@AngleZ	FLOAT,

	@SizeX	FLOAT,

	@SizeY	FLOAT,

	@SizeZ	FLOAT,

	@Note	VARCHAR(40),

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@AutoID OUTPUT,

	@TPID OUTPUT,

	@StudyID,

	@Type,

	@OffsetX,

	@OffsetY,

	@OffsetZ,

	@AngleX,

	@AngleY,

	@AngleZ,

	@SizeX,

	@SizeY,

	@SizeZ,

	@Note,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: tbOrganContour_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:46:29

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:46:29	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbOrganContour_DELETE

	 @TPID	BIGINT

	,@DBName NVARCHAR(100),

	@UpdateTime TIMESTAMP,

	@ApplyRowVersion BIT

AS

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbOrganContour

		where TPID = @TPID AND UpdateTime=@UpdateTime'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ,

		@UpdateTime TIMESTAMP',	

		@DBName,

		@TPID,

		@UpdateTime

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbOrganContour

		where TPID = @TPID'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ',	

		@DBName,

		@TPID

	END

go
/******************************************************************

* ÂêçÁß∞: SP_tbCoordinate_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:45:06

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:45:06	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbCoordinate_UPDATE

	@TPID	BIGINT,

	@StudyID	BIGINT,

	@Type	SMALLINT,

	@OffsetX	FLOAT,

	@OffsetY	FLOAT,

	@OffsetZ	FLOAT,

	@AngleX	FLOAT,

	@AngleY	FLOAT,

	@AngleZ	FLOAT,

	@SizeX	FLOAT,

	@SizeY	FLOAT,

	@SizeZ	FLOAT,

	@Note	VARCHAR(40),

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)   ,

	@ApplyRowVersion BIT

 AS 

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' UPDATE tbCoordinate SET

		TPID = @TPID,

		StudyID = @StudyID,

		Type = @Type,

		OffsetX = @OffsetX,

		OffsetY = @OffsetY,

		OffsetZ = @OffsetZ,

		AngleX = @AngleX,

		AngleY = @AngleY,

		AngleZ = @AngleZ,

		SizeX = @SizeX,

		SizeY = @SizeY,

		SizeZ = @SizeZ,

		Note = @Note

		where TPID = @TPID AND UpdateTime=@UpdateTime

		SELECT @UpdateTime = UpdateTime FROM tbCoordinate WHERE TPID=@TPID'

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' UPDATE tbCoordinate SET

		TPID = @TPID,

		StudyID = @StudyID,

		Type = @Type,

		OffsetX = @OffsetX,

		OffsetY = @OffsetY,

		OffsetZ = @OffsetZ,

		AngleX = @AngleX,

		AngleY = @AngleY,

		AngleZ = @AngleZ,

		SizeX = @SizeX,

		SizeY = @SizeY,

		SizeZ = @SizeZ,

		Note = @Note

		where TPID = @TPID

		SELECT @UpdateTime = UpdateTime FROM tbCoordinate WHERE TPID=@TPID'

	END

	EXECUTE  sp_executesql @sql,N'

	@TPID	BIGINT ,

	@StudyID	BIGINT ,

	@Type	SMALLINT,

	@OffsetX	FLOAT,

	@OffsetY	FLOAT,

	@OffsetZ	FLOAT,

	@AngleX	FLOAT,

	@AngleY	FLOAT,

	@AngleZ	FLOAT,

	@SizeX	FLOAT,

	@SizeY	FLOAT,

	@SizeZ	FLOAT,

	@Note	VARCHAR(40),

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@TPID,

	@StudyID,

	@Type,

	@OffsetX,

	@OffsetY,

	@OffsetZ,

	@AngleX,

	@AngleY,

	@AngleZ,

	@SizeX,

	@SizeY,

	@SizeZ,

	@Note,

	@UpdateTime OUTPUT,

	@DBName   

go

CREATE  PROCEDURE [dbo].[SP_tbOrganContour_SELECT]

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM tbOrganContour'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: tbCoordinate_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:45:06

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:45:06	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbCoordinate_DELETE

	 @TPID	BIGINT

	,@DBName NVARCHAR(100),

	@UpdateTime TIMESTAMP,

	@ApplyRowVersion BIT

AS

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbCoordinate

		where TPID = @TPID AND UpdateTime=@UpdateTime'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ,

		@UpdateTime TIMESTAMP',	

		@DBName,

		@TPID,

		@UpdateTime

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbCoordinate

		where TPID = @TPID'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ',	

		@DBName,

		@TPID

	END

go
/******************************************************************

* ÂêçÁß∞: SP_tbPlan_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:46:39

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:46:39	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbPlan_INSERT

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@CourseAlias	VARCHAR(20),

	@Plan	VARCHAR(10),

	@PlanAlias	VARCHAR(20),

	@HeterogeneityCorrect	BIT,

	@CalculateMethod	VARCHAR(4),

	@PencilSize	FLOAT,

	@NormMode	VARCHAR(20),

	@NormPointW	FLOAT,

	@NormPointL	FLOAT,

	@NormPointH	FLOAT,

	@NormPointDose	FLOAT,

	@DisplayMode	VARCHAR(20),

	@PrescriptionPtName	VARCHAR(20),

	@Prescribeddose	FLOAT,

	@DosePerFraction	FLOAT,

	@PrescribeLevel	FLOAT,

	@UserPrescribeLevel	FLOAT,

	@FractionNumber	INT,

	@PlanType	INT,

	@IMRTTargetName	VARCHAR(15),

	@IMRTMaxAbsorbDoseW	FLOAT,

	@IMRTMaxAbsorbDoseL	FLOAT,

	@IMRTMaxAbsorbDoseH	FLOAT,

	@IMRTMaxAbsorbDose	FLOAT,

	@IMRTDMLCType	VARCHAR(30),

	@IMRTDeliveryTech	INT,

	@IMRTDoseRate	INT,

	@IMRTLeafTolerance	FLOAT,

	@IMRTSandSLevelNum	INT,

	@IMRTDoseType	INT,

	@CalculateAreaW0	FLOAT,

	@CalculateAreaL0	FLOAT,

	@CalculateAreaH0	FLOAT,

	@CalculateAreaW	FLOAT,

	@CalculateAreaL	FLOAT,

	@CalculateAreaH	FLOAT,

	@CalculateAreaSpace	FLOAT,

	@PlanUIDForDicom	VARCHAR(64),

	@Note	VARCHAR(500),

	@PlannerName	VARCHAR(30),

	@PlanDate	DATETIME,

	@Locked	BIT,

	@Beams	IMAGE,

	@Approved	BIT,

	@ApprovedDateTime	DATETIME,

	@BolusList IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)

 AS

	IF(@TPID is Null)

	BEGIN

		SELECT @TPID=-1

	END

	DECLARE @Sql NVARCHAR(3000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO tbPlan

	(

		TPID,

		StudyID,

		Course,

		CourseAlias,

		[Plan],

		PlanAlias,

		HeterogeneityCorrect,

		CalculateMethod,

		PencilSize,

		NormMode,

		NormPointW,

		NormPointL,

		NormPointH,

		NormPointDose,

		DisplayMode,

		PrescriptionPtName,

		Prescribeddose,

		DosePerFraction,

		PrescribeLevel,

		UserPrescribeLevel,

		FractionNumber,

		PlanType,

		IMRTTargetName,

		IMRTMaxAbsorbDoseW,

		IMRTMaxAbsorbDoseL,

		IMRTMaxAbsorbDoseH,

		IMRTMaxAbsorbDose,

		IMRTDMLCType,

		IMRTDeliveryTech,

		IMRTDoseRate,

		IMRTLeafTolerance,

		IMRTSandSLevelNum,

		IMRTDoseType,

		CalculateAreaW0,

		CalculateAreaL0,

		CalculateAreaH0,

		CalculateAreaW,

		CalculateAreaL,

		CalculateAreaH,

		CalculateAreaSpace,

		PlanUIDForDicom,

		Note,

		PlannerName,

		PlanDate,

		Locked,

		Beams,

		Approved,

		ApprovedDateTime,

	    BolusList

	)

	VALUES

	(

		@TPID,

		@StudyID,

		@Course,

		@CourseAlias,

		@Plan,

		@PlanAlias,

		@HeterogeneityCorrect,

		@CalculateMethod,

		@PencilSize,

		@NormMode,

		@NormPointW,

		@NormPointL,

		@NormPointH,

		@NormPointDose,

		@DisplayMode,

		@PrescriptionPtName,

		@Prescribeddose,

		@DosePerFraction,

		@PrescribeLevel,

		@UserPrescribeLevel,

		@FractionNumber,

		@PlanType,

		@IMRTTargetName,

		@IMRTMaxAbsorbDoseW,

		@IMRTMaxAbsorbDoseL,

		@IMRTMaxAbsorbDoseH,

		@IMRTMaxAbsorbDose,

		@IMRTDMLCType,

		@IMRTDeliveryTech,

		@IMRTDoseRate,

		@IMRTLeafTolerance,

		@IMRTSandSLevelNum,

		@IMRTDoseType,

		@CalculateAreaW0,

		@CalculateAreaL0,

		@CalculateAreaH0,

		@CalculateAreaW,

		@CalculateAreaL,

		@CalculateAreaH,

		@CalculateAreaSpace,

		@PlanUIDForDicom,

		@Note,

		@PlannerName,

		@PlanDate,

		@Locked,

		@Beams,

		@Approved,

		@ApprovedDateTime,

		@BolusList

	)

	SET @AutoID=SCOPE_IDENTITY()

	SELECT @TPID= TPID,@UpdateTime = UpdateTime FROM tbPlan WHERE AutoID=SCOPE_IDENTITY()

	IF (@TPID =-1)

	BEGIN

		SET @TPID=SCOPE_IDENTITY()

		UPDATE tbPlan SET TPID=@TPID  WHERE AutoID=@TPID

	SELECT @UpdateTime = UpdateTime FROM tbPlan WHERE AutoID=SCOPE_IDENTITY()

	END'

	EXECUTE  sp_executesql @sql,N'

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@CourseAlias	VARCHAR(20),

	@Plan	VARCHAR(10),

	@PlanAlias	VARCHAR(20),

	@HeterogeneityCorrect	BIT,

	@CalculateMethod	VARCHAR(4),

	@PencilSize	FLOAT,

	@NormMode	VARCHAR(20),

	@NormPointW	FLOAT,

	@NormPointL	FLOAT,

	@NormPointH	FLOAT,

	@NormPointDose	FLOAT,

	@DisplayMode	VARCHAR(20),

	@PrescriptionPtName	VARCHAR(20),

	@Prescribeddose	FLOAT,

	@DosePerFraction	FLOAT,

	@PrescribeLevel	FLOAT,

	@UserPrescribeLevel	FLOAT,

	@FractionNumber	INT,

	@PlanType	INT,

	@IMRTTargetName	VARCHAR(15),

	@IMRTMaxAbsorbDoseW	FLOAT,

	@IMRTMaxAbsorbDoseL	FLOAT,

	@IMRTMaxAbsorbDoseH	FLOAT,

	@IMRTMaxAbsorbDose	FLOAT,

	@IMRTDMLCType	VARCHAR(30),

	@IMRTDeliveryTech	INT,

	@IMRTDoseRate	INT,

	@IMRTLeafTolerance	FLOAT,

	@IMRTSandSLevelNum	INT,

	@IMRTDoseType	INT,

	@CalculateAreaW0	FLOAT,

	@CalculateAreaL0	FLOAT,

	@CalculateAreaH0	FLOAT,

	@CalculateAreaW	FLOAT,

	@CalculateAreaL	FLOAT,

	@CalculateAreaH	FLOAT,

	@CalculateAreaSpace	FLOAT,

	@PlanUIDForDicom	VARCHAR(64),

	@Note	VARCHAR(500),

	@PlannerName	VARCHAR(30),

	@PlanDate	DATETIME,

	@Locked	BIT,

	@Beams	IMAGE,

	@Approved	BIT,

	@ApprovedDateTime	DATETIME,

	@BolusList IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@AutoID OUTPUT,

	@TPID OUTPUT,

	@StudyID,

	@Course,

	@CourseAlias,

	@Plan,

	@PlanAlias,

	@HeterogeneityCorrect,

	@CalculateMethod,

	@PencilSize,

	@NormMode,

	@NormPointW,

	@NormPointL,

	@NormPointH,

	@NormPointDose,

	@DisplayMode,

	@PrescriptionPtName,

	@Prescribeddose,

	@DosePerFraction,

	@PrescribeLevel,

	@UserPrescribeLevel,

	@FractionNumber,

	@PlanType,

	@IMRTTargetName,

	@IMRTMaxAbsorbDoseW,

	@IMRTMaxAbsorbDoseL,

	@IMRTMaxAbsorbDoseH,

	@IMRTMaxAbsorbDose,

	@IMRTDMLCType,

	@IMRTDeliveryTech,

	@IMRTDoseRate,

	@IMRTLeafTolerance,

	@IMRTSandSLevelNum,

	@IMRTDoseType,

	@CalculateAreaW0,

	@CalculateAreaL0,

	@CalculateAreaH0,

	@CalculateAreaW,

	@CalculateAreaL,

	@CalculateAreaH,

	@CalculateAreaSpace,

	@PlanUIDForDicom,

	@Note,

	@PlannerName,

	@PlanDate,

	@Locked,

	@Beams,

	@Approved,

	@ApprovedDateTime,

	@BolusList,

	@UpdateTime OUTPUT,

	@DBName   

go

CREATE  PROCEDURE SP_tbCoordinate_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM tbCoordinate'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: SP_tbPlan_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:46:39

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:46:39	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbPlan_UPDATE

	@TPID	BIGINT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@CourseAlias	VARCHAR(20),

	@Plan	VARCHAR(10),

	@PlanAlias	VARCHAR(20),

	@HeterogeneityCorrect	BIT,

	@CalculateMethod	VARCHAR(4),

	@PencilSize	FLOAT,

	@NormMode	VARCHAR(20),

	@NormPointW	FLOAT,

	@NormPointL	FLOAT,

	@NormPointH	FLOAT,

	@NormPointDose	FLOAT,

	@DisplayMode	VARCHAR(20),

	@PrescriptionPtName	VARCHAR(20),

	@Prescribeddose	FLOAT,

	@DosePerFraction	FLOAT,

	@PrescribeLevel	FLOAT,

	@UserPrescribeLevel	FLOAT,

	@FractionNumber	INT,

	@PlanType	INT,

	@IMRTTargetName	VARCHAR(15),

	@IMRTMaxAbsorbDoseW	FLOAT,

	@IMRTMaxAbsorbDoseL	FLOAT,

	@IMRTMaxAbsorbDoseH	FLOAT,

	@IMRTMaxAbsorbDose	FLOAT,

	@IMRTDMLCType	VARCHAR(30),

	@IMRTDeliveryTech	INT,

	@IMRTDoseRate	INT,

	@IMRTLeafTolerance	FLOAT,

	@IMRTSandSLevelNum	INT,

	@IMRTDoseType	INT,

	@CalculateAreaW0	FLOAT,

	@CalculateAreaL0	FLOAT,

	@CalculateAreaH0	FLOAT,

	@CalculateAreaW	FLOAT,

	@CalculateAreaL	FLOAT,

	@CalculateAreaH	FLOAT,

	@CalculateAreaSpace	FLOAT,

	@PlanUIDForDicom	VARCHAR(64),

	@Note	VARCHAR(500),

	@PlannerName	VARCHAR(30),

	@PlanDate	DATETIME,

	@Locked	BIT,

	@Beams	IMAGE,

	@Approved	BIT,

	@ApprovedDateTime	DATETIME,

	@BolusList IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)   ,

	@ApplyRowVersion BIT

 AS 

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(3000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' UPDATE tbPlan SET

		TPID = @TPID,

		StudyID = @StudyID,

		Course = @Course,

		CourseAlias = @CourseAlias,

		[Plan] = @Plan,

		PlanAlias = @PlanAlias,

		HeterogeneityCorrect = @HeterogeneityCorrect,

		CalculateMethod = @CalculateMethod,

		PencilSize = @PencilSize,

		NormMode = @NormMode,

		NormPointW = @NormPointW,

		NormPointL = @NormPointL,

		NormPointH = @NormPointH,

		NormPointDose = @NormPointDose,

		DisplayMode = @DisplayMode,

		PrescriptionPtName = @PrescriptionPtName,

		Prescribeddose = @Prescribeddose,

		DosePerFraction = @DosePerFraction,

		PrescribeLevel = @PrescribeLevel,

		UserPrescribeLevel = @UserPrescribeLevel,

		FractionNumber = @FractionNumber,

		PlanType = @PlanType,

		IMRTTargetName = @IMRTTargetName,

		IMRTMaxAbsorbDoseW = @IMRTMaxAbsorbDoseW,

		IMRTMaxAbsorbDoseL = @IMRTMaxAbsorbDoseL,

		IMRTMaxAbsorbDoseH = @IMRTMaxAbsorbDoseH,

		IMRTMaxAbsorbDose = @IMRTMaxAbsorbDose,

		IMRTDMLCType = @IMRTDMLCType,

		IMRTDeliveryTech = @IMRTDeliveryTech,

		IMRTDoseRate = @IMRTDoseRate,

		IMRTLeafTolerance = @IMRTLeafTolerance,

		IMRTSandSLevelNum = @IMRTSandSLevelNum,

		IMRTDoseType = @IMRTDoseType,

		CalculateAreaW0 = @CalculateAreaW0,

		CalculateAreaL0 = @CalculateAreaL0,

		CalculateAreaH0 = @CalculateAreaH0,

		CalculateAreaW = @CalculateAreaW,

		CalculateAreaL = @CalculateAreaL,

		CalculateAreaH = @CalculateAreaH,

		CalculateAreaSpace = @CalculateAreaSpace,

		PlanUIDForDicom = @PlanUIDForDicom,

		Note = @Note,

		PlannerName = @PlannerName,

		PlanDate = @PlanDate,

		Locked = @Locked,

		Beams = @Beams,

		Approved = @Approved,

		ApprovedDateTime = @ApprovedDateTime,

		BolusList = @BolusList

		where TPID = @TPID AND UpdateTime=@UpdateTime

		SELECT @UpdateTime = UpdateTime FROM tbPlan WHERE TPID=@TPID'

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' UPDATE tbPlan SET


		TPID = @TPID,

		StudyID = @StudyID,

		Course = @Course,

		CourseAlias = @CourseAlias,

		[Plan] = @Plan,

		PlanAlias = @PlanAlias,

		HeterogeneityCorrect = @HeterogeneityCorrect,

		CalculateMethod = @CalculateMethod,

		PencilSize = @PencilSize,

		NormMode = @NormMode,

		NormPointW = @NormPointW,

		NormPointL = @NormPointL,

		NormPointH = @NormPointH,

		NormPointDose = @NormPointDose,

		DisplayMode = @DisplayMode,

		PrescriptionPtName = @PrescriptionPtName,

		Prescribeddose = @Prescribeddose,

		DosePerFraction = @DosePerFraction,

		PrescribeLevel = @PrescribeLevel,

		UserPrescribeLevel = @UserPrescribeLevel,

		FractionNumber = @FractionNumber,

		PlanType = @PlanType,

		IMRTTargetName = @IMRTTargetName,

		IMRTMaxAbsorbDoseW = @IMRTMaxAbsorbDoseW,

		IMRTMaxAbsorbDoseL = @IMRTMaxAbsorbDoseL,

		IMRTMaxAbsorbDoseH = @IMRTMaxAbsorbDoseH,

		IMRTMaxAbsorbDose = @IMRTMaxAbsorbDose,

		IMRTDMLCType = @IMRTDMLCType,

		IMRTDeliveryTech = @IMRTDeliveryTech,

		IMRTDoseRate = @IMRTDoseRate,

		IMRTLeafTolerance = @IMRTLeafTolerance,

		IMRTSandSLevelNum = @IMRTSandSLevelNum,

		IMRTDoseType = @IMRTDoseType,

		CalculateAreaW0 = @CalculateAreaW0,

		CalculateAreaL0 = @CalculateAreaL0,

		CalculateAreaH0 = @CalculateAreaH0,

		CalculateAreaW = @CalculateAreaW,

		CalculateAreaL = @CalculateAreaL,

		CalculateAreaH = @CalculateAreaH,

		CalculateAreaSpace = @CalculateAreaSpace,

		PlanUIDForDicom = @PlanUIDForDicom,

		Note = @Note,

		PlannerName = @PlannerName,

		PlanDate = @PlanDate,

		Locked = @Locked,

		Beams = @Beams,

		Approved = @Approved,

		ApprovedDateTime = @ApprovedDateTime,

		BolusList = @BolusList

		where TPID = @TPID

		SELECT @UpdateTime = UpdateTime FROM tbPlan WHERE TPID=@TPID'

	END

	EXECUTE  sp_executesql @sql,N'

	@TPID	BIGINT , 

	@StudyID	BIGINT ,

	@Course	VARCHAR(10),

	@CourseAlias	VARCHAR(20),

	@Plan	VARCHAR(10),

	@PlanAlias	VARCHAR(20),

	@HeterogeneityCorrect	BIT,

	@CalculateMethod	VARCHAR(4),

	@PencilSize	FLOAT,

	@NormMode	VARCHAR(20),

	@NormPointW	FLOAT,

	@NormPointL	FLOAT,

	@NormPointH	FLOAT,

	@NormPointDose	FLOAT,

	@DisplayMode	VARCHAR(20),

	@PrescriptionPtName	VARCHAR(20),

	@Prescribeddose	FLOAT,

	@DosePerFraction	FLOAT,

	@PrescribeLevel	FLOAT,

	@UserPrescribeLevel	FLOAT,

	@FractionNumber	INT,

	@PlanType	INT,

	@IMRTTargetName	VARCHAR(15),

	@IMRTMaxAbsorbDoseW	FLOAT,

	@IMRTMaxAbsorbDoseL	FLOAT,

	@IMRTMaxAbsorbDoseH	FLOAT,

	@IMRTMaxAbsorbDose	FLOAT,

	@IMRTDMLCType	VARCHAR(30),

	@IMRTDeliveryTech	INT,

	@IMRTDoseRate	INT,

	@IMRTLeafTolerance	FLOAT,

	@IMRTSandSLevelNum	INT,

	@IMRTDoseType	INT,

	@CalculateAreaW0	FLOAT,

	@CalculateAreaL0	FLOAT,

	@CalculateAreaH0	FLOAT,

	@CalculateAreaW	FLOAT,

	@CalculateAreaL	FLOAT,

	@CalculateAreaH	FLOAT,

	@CalculateAreaSpace	FLOAT,

	@PlanUIDForDicom	VARCHAR(64),

	@Note	VARCHAR(500),

	@PlannerName	VARCHAR(30),

	@PlanDate	DATETIME,

	@Locked	BIT,

	@Beams	IMAGE,

	@Approved	BIT,

	@ApprovedDateTime	DATETIME,

    @BolusList IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@TPID,

	@StudyID,

	@Course,

	@CourseAlias,

	@Plan,

	@PlanAlias,

	@HeterogeneityCorrect,

	@CalculateMethod,

	@PencilSize,

	@NormMode,

	@NormPointW,

	@NormPointL,

	@NormPointH,

	@NormPointDose,

	@DisplayMode,

	@PrescriptionPtName,

	@Prescribeddose,

	@DosePerFraction,

	@PrescribeLevel,

	@UserPrescribeLevel,

	@FractionNumber,

	@PlanType,

	@IMRTTargetName,

	@IMRTMaxAbsorbDoseW,

	@IMRTMaxAbsorbDoseL,

	@IMRTMaxAbsorbDoseH,

	@IMRTMaxAbsorbDose,

	@IMRTDMLCType,

	@IMRTDeliveryTech,

	@IMRTDoseRate,

	@IMRTLeafTolerance,

	@IMRTSandSLevelNum,

	@IMRTDoseType,

	@CalculateAreaW0,

	@CalculateAreaL0,

	@CalculateAreaH0,

	@CalculateAreaW,

	@CalculateAreaL,

	@CalculateAreaH,

	@CalculateAreaSpace,

	@PlanUIDForDicom,

	@Note,

	@PlannerName,

	@PlanDate,

	@Locked,

	@Beams,

	@Approved,

	@ApprovedDateTime,

	@BolusList,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_tbDose_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:45:18

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:45:18	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbDose_UPDATE

	@TPID	BIGINT,

	@BeamID	BIGINT,

	@SliceID	BIGINT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@PlanID	BIGINT,

	@SlicePositionmm	FLOAT,

	@RowCount	INT,

	@ColumnCount	INT,

	@SliceMaxDose	FLOAT,

	@TargetMaxDose	FLOAT,

	@TargetMinDose	FLOAT,

	@TargetMeanDose	FLOAT,

	@Data	IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)   ,

	@ApplyRowVersion BIT

 AS 

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(MAX) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' UPDATE tbDose SET

		TPID = @TPID,

		BeamID = @BeamID,

		SliceID = @SliceID,

		StudyID = @StudyID,

		Course = @Course,

		[PlanID] = @PlanID,

		SlicePositionmm = @SlicePositionmm,

		[RowCount] = @RowCount,

		ColumnCount = @ColumnCount,

		SliceMaxDose = @SliceMaxDose,

		TargetMaxDose = @TargetMaxDose,

		TargetMinDose = @TargetMinDose,

		TargetMeanDose = @TargetMeanDose,

		Data = @Data

		where TPID = @TPID AND UpdateTime=@UpdateTime

		SELECT @UpdateTime = UpdateTime FROM tbDose WHERE TPID=@TPID'

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' UPDATE tbDose SET

		TPID = @TPID,

		BeamID = @BeamID,

		SliceID = @SliceID,

		StudyID = @StudyID,

		Course = @Course,

		[PlanID] = @PlanID,

		SlicePositionmm = @SlicePositionmm,

		[RowCount] = @RowCount,

		ColumnCount = @ColumnCount,

		SliceMaxDose = @SliceMaxDose,

		TargetMaxDose = @TargetMaxDose,

		TargetMinDose = @TargetMinDose,

		TargetMeanDose = @TargetMeanDose,

		Data = @Data

		where TPID = @TPID

		SELECT @UpdateTime = UpdateTime FROM tbDose WHERE TPID=@TPID'

	END

	EXECUTE  sp_executesql @sql,N'

	@TPID	BIGINT ,

	@BeamID	BIGINT ,

	@SliceID	BIGINT ,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@PlanID	BIGINT ,

	@SlicePositionmm	FLOAT,

	@RowCount	INT,

	@ColumnCount	INT,

	@SliceMaxDose	FLOAT,

	@TargetMaxDose	FLOAT,

	@TargetMinDose	FLOAT,

	@TargetMeanDose	FLOAT,

	@Data	IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@TPID,

	@BeamID,

	@SliceID,

	@StudyID,

	@Course,

	@PlanID,

	@SlicePositionmm,

	@RowCount,

	@ColumnCount,

	@SliceMaxDose,

	@TargetMaxDose,

	@TargetMinDose,

	@TargetMeanDose,

	@Data,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: tbPlan_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:46:39

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:46:39	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbPlan_DELETE

	 @TPID	BIGINT

	,@DBName NVARCHAR(100),

	@UpdateTime TIMESTAMP,

	@ApplyRowVersion BIT

AS

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbPlan

		where TPID = @TPID AND UpdateTime=@UpdateTime'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT,

		@UpdateTime TIMESTAMP',	

		@DBName,

		@TPID,

		@UpdateTime

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbPlan

		where TPID = @TPID'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ',	

		@DBName,

		@TPID

	END

go
CREATE PROCEDURE SP_Conditional_SELECT

@dbName  VARCHAR(100),

@FieldNames  VARCHAR(1000),

@Filter  VARCHAR(1000),

@TableName VARCHAR(100)

 AS

IF @Filter=''

BEGIN

	exec('USE '+ @dbName + ' SELECT  '+ @FieldNames + ' FROM '+ @TableName)

END

ELSE if ((UPPER (SUBSTRING(@Filter,1,8))='ORDER BY') 
OR (UPPER (SUBSTRING(@Filter,1,8))='GROUP BY') OR (UPPER (SUBSTRING(@Filter,1,10))='INNER JOIN'))

BEGIN

	exec('USE '+ @dbName + ' SELECT  '+ @FieldNames + ' FROM '+ @TableName + ' ' + @Filter)

END

ELSE

BEGIN

	exec('USE '+ @dbName + ' SELECT  '+ @FieldNames + ' FROM '+ @TableName+' where '+ @Filter)

END

go

CREATE  PROCEDURE SP_tbPlan_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM tbPlan'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: SP_tbPOI_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:46:50

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:46:50	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbPOI_INSERT

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@Name	VARCHAR(10),

	@X	FLOAT,

	@Y	FLOAT,

	@Z	FLOAT,

	@RelativeDose	FLOAT,

	@AbsoluteDosecGy	FLOAT,

	@PlanID	BIGINT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@BeamID	BIGINT,

	@IRFSSD	FLOAT,

	@IRFBeamDose	FLOAT,

	@IRFTotalDose	FLOAT,

	@WhereDefine	VARCHAR(20),

	@DoseOfBeamsList	IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)

 AS

	IF(@TPID is Null)

	BEGIN

		SELECT @TPID=-1

	END

	DECLARE @Sql NVARCHAR(3000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO tbPOI

	(

		TPID,

		Name,

		X,

		Y,

		Z,

		RelativeDose,

		AbsoluteDosecGy,

		[PlanID],

		StudyID,

		Course,

		BeamID,

		IRFSSD,

		IRFBeamDose,

		IRFTotalDose,

		WhereDefine,

		DoseOfBeamsList

	)

	VALUES

	(

		@TPID,

		@Name,

		@X,

		@Y,

		@Z,

		@RelativeDose,

		@AbsoluteDosecGy,

		@PlanID,

		@StudyID,

		@Course,

		@BeamID,

		@IRFSSD,

		@IRFBeamDose,

		@IRFTotalDose,

		@WhereDefine,

		@DoseOfBeamsList

	)

	SET @AutoID=SCOPE_IDENTITY()

	SELECT @TPID= TPID,@UpdateTime = UpdateTime FROM tbPOI WHERE AutoID=SCOPE_IDENTITY()

	IF (@TPID =-1)

	BEGIN

		SET @TPID=SCOPE_IDENTITY()

		UPDATE tbPOI SET TPID=@TPID  WHERE AutoID=@TPID

	SELECT @UpdateTime = UpdateTime FROM tbPOI WHERE AutoID=SCOPE_IDENTITY()

	END'

	EXECUTE  sp_executesql @sql,N'

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@Name	VARCHAR(10),

	@X	FLOAT,

	@Y	FLOAT,

	@Z	FLOAT,

	@RelativeDose	FLOAT,

	@AbsoluteDosecGy	FLOAT,

	@PlanID	BIGINT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@BeamID	BIGINT,

	@IRFSSD	FLOAT,

	@IRFBeamDose	FLOAT,

	@IRFTotalDose	FLOAT,

	@WhereDefine	VARCHAR(20),

	@DoseOfBeamsList	IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@AutoID OUTPUT,

	@TPID OUTPUT,

	@Name,

	@X,

	@Y,

	@Z,

	@RelativeDose,

	@AbsoluteDosecGy,

	@PlanID,

	@StudyID,

	@Course,

	@BeamID,

	@IRFSSD,

	@IRFBeamDose,

	@IRFTotalDose,

	@WhereDefine,

	@DoseOfBeamsList,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_tbPOI_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:46:50

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:46:50	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbPOI_UPDATE

	@TPID	BIGINT,

	@Name	VARCHAR(10),

	@X	FLOAT,

	@Y	FLOAT,

	@Z	FLOAT,

	@RelativeDose	FLOAT,

	@AbsoluteDosecGy	FLOAT,

	@PlanID	BIGINT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@BeamID	BIGINT,

	@IRFSSD	FLOAT,

	@IRFBeamDose	FLOAT,

	@IRFTotalDose	FLOAT,

	@WhereDefine	VARCHAR(20),

	@DoseOfBeamsList	IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)   ,

	@ApplyRowVersion BIT

 AS 

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(3000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' UPDATE tbPOI SET

		TPID = @TPID,

		Name = @Name,

		X = @X,

		Y = @Y,

		Z = @Z,

		RelativeDose = @RelativeDose,

		AbsoluteDosecGy = @AbsoluteDosecGy,

		[PlanID] = @PlanID,

		StudyID = @StudyID,

		Course = @Course,

		BeamID = @BeamID,

		IRFSSD = @IRFSSD,

		IRFBeamDose = @IRFBeamDose,

		IRFTotalDose = @IRFTotalDose,

		WhereDefine = @WhereDefine,

		DoseOfBeamsList = @DoseOfBeamsList

		where TPID = @TPID AND UpdateTime=@UpdateTime

		SELECT @UpdateTime = UpdateTime FROM tbPOI WHERE TPID=@TPID'

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' UPDATE tbPOI SET

		TPID = @TPID,

		Name = @Name,

		X = @X,

		Y = @Y,

		Z = @Z,

		RelativeDose = @RelativeDose,

		AbsoluteDosecGy = @AbsoluteDosecGy,

		[PlanID] = @PlanID,

		StudyID = @StudyID,

		Course = @Course,

		BeamID = @BeamID,

		IRFSSD = @IRFSSD,

		IRFBeamDose = @IRFBeamDose,

		IRFTotalDose = @IRFTotalDose,

		WhereDefine = @WhereDefine,

		DoseOfBeamsList = @DoseOfBeamsList

		where TPID = @TPID

		SELECT @UpdateTime = UpdateTime FROM tbPOI WHERE TPID=@TPID'

	END

	EXECUTE  sp_executesql @sql,N'

	@TPID	BIGINT ,

	@Name	VARCHAR(10),

	@X	FLOAT,

	@Y	FLOAT,

	@Z	FLOAT,

	@RelativeDose	FLOAT,

	@AbsoluteDosecGy	FLOAT,

	@PlanID	BIGINT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@BeamID	BIGINT,

	@IRFSSD	FLOAT,

	@IRFBeamDose	FLOAT,

	@IRFTotalDose	FLOAT,

	@WhereDefine	VARCHAR(20),

	@DoseOfBeamsList	IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@TPID,

	@Name,

	@X,

	@Y,

	@Z,

	@RelativeDose,

	@AbsoluteDosecGy,

	@PlanID,

	@StudyID,

	@Course,

	@BeamID,

	@IRFSSD,

	@IRFBeamDose,

	@IRFTotalDose,

	@WhereDefine,

	@DoseOfBeamsList,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: tbPOI_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:46:50

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:46:50	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbPOI_DELETE

	 @TPID	BIGINT

	,@DBName NVARCHAR(100),

	@UpdateTime TIMESTAMP,

	@ApplyRowVersion BIT

AS

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbPOI

		where TPID = @TPID AND UpdateTime=@UpdateTime'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ,

		@UpdateTime TIMESTAMP',	

		@DBName,

		@TPID,

		@UpdateTime

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbPOI

		where TPID = @TPID'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ',	

		@DBName,

		@TPID

	END

go

CREATE  PROCEDURE SP_tbPOI_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM tbPOI'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: SP_tbPTV_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:47:00

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:47:00	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbPTV_INSERT

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@OrganName	VARCHAR(18),

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@PlanID	BIGINT,

	@SoftOrHard	VARCHAR(4),

	@WindowType	VARCHAR(6),

	@ConstraintType	VARCHAR(5),

	@MinDose	FLOAT,

	@MaxDose	FLOAT,

	@MinVolume	FLOAT,

	@MaxVolume	FLOAT,

	@Penalty	FLOAT,

	@GridSize	FLOAT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)

 AS

	IF(@TPID is Null)

	BEGIN

		SELECT @TPID=-1

	END

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO tbPTV

	(

		TPID,

		OrganName,

		StudyID,

		Course,

		[PlanID],

		SoftOrHard,

		WindowType,

		ConstraintType,

		MinDose,

		MaxDose,

		MinVolume,

		MaxVolume,

		Penalty,

		GridSize

	)

	VALUES

	(

		@TPID,

		@OrganName,

		@StudyID,

		@Course,

		@PlanID,

		@SoftOrHard,

		@WindowType,

		@ConstraintType,

		@MinDose,

		@MaxDose,

		@MinVolume,

		@MaxVolume,

		@Penalty,

		@GridSize

	)

	SET @AutoID=SCOPE_IDENTITY()

	SELECT @TPID= TPID,@UpdateTime = UpdateTime FROM tbPTV WHERE AutoID=SCOPE_IDENTITY()

	IF (@TPID =-1)

	BEGIN

		SET @TPID=SCOPE_IDENTITY()

		UPDATE tbPTV SET TPID=@TPID  WHERE AutoID=@TPID

	SELECT @UpdateTime = UpdateTime FROM tbPTV WHERE AutoID=SCOPE_IDENTITY()

	END'

	EXECUTE  sp_executesql @sql,N'

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@OrganName	VARCHAR(18),

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@PlanID	BIGINT,

	@SoftOrHard	VARCHAR(4),

	@WindowType	VARCHAR(6),

	@ConstraintType	VARCHAR(5),

	@MinDose	FLOAT,

	@MaxDose	FLOAT,

	@MinVolume	FLOAT,

	@MaxVolume	FLOAT,

	@Penalty	FLOAT,

	@GridSize	FLOAT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@AutoID OUTPUT,

	@TPID OUTPUT,

	@OrganName,

	@StudyID,

	@Course,

	@PlanID,

	@SoftOrHard,

	@WindowType,

	@ConstraintType,

	@MinDose,

	@MaxDose,

	@MinVolume,

	@MaxVolume,

	@Penalty,

	@GridSize,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_tbPTV_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:47:00

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:47:00	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbPTV_UPDATE

	@TPID	BIGINT,

	@OrganName	VARCHAR(18),

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@PlanID	BIGINT,

	@SoftOrHard	VARCHAR(4),

	@WindowType	VARCHAR(6),

	@ConstraintType	VARCHAR(5),

	@MinDose	FLOAT,

	@MaxDose	FLOAT,

	@MinVolume	FLOAT,

	@MaxVolume	FLOAT,

	@Penalty	FLOAT,

	@GridSize	FLOAT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)   ,

	@ApplyRowVersion BIT

 AS 

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' UPDATE tbPTV SET

		TPID = @TPID,

		OrganName = @OrganName,

		StudyID = @StudyID,

		Course = @Course,

		[PlanID] = @PlanID,

		SoftOrHard = @SoftOrHard,

		WindowType = @WindowType,

		ConstraintType = @ConstraintType,

		MinDose = @MinDose,

		MaxDose = @MaxDose,

		MinVolume = @MinVolume,

		MaxVolume = @MaxVolume,

		Penalty = @Penalty,

		GridSize = @GridSize

		where TPID = @TPID AND UpdateTime=@UpdateTime

		SELECT @UpdateTime = UpdateTime FROM tbPTV WHERE TPID=@TPID'

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' UPDATE tbPTV SET

		TPID = @TPID,

		OrganName = @OrganName,

		StudyID = @StudyID,

		Course = @Course,

		[PlanID] = @PlanID,

		SoftOrHard = @SoftOrHard,

		WindowType = @WindowType,

		ConstraintType = @ConstraintType,

		MinDose = @MinDose,

		MaxDose = @MaxDose,

		MinVolume = @MinVolume,

		MaxVolume = @MaxVolume,

		Penalty = @Penalty,

		GridSize = @GridSize

		where TPID = @TPID

		SELECT @UpdateTime = UpdateTime FROM tbPTV WHERE TPID=@TPID'

	END

	EXECUTE  sp_executesql @sql,N'

	@TPID	BIGINT ,

	@OrganName	VARCHAR(18),

	@StudyID	BIGINT ,

	@Course	VARCHAR(10) ,

	@PlanID	BIGINT ,

	@SoftOrHard	VARCHAR(4),

	@WindowType	VARCHAR(6),

	@ConstraintType	VARCHAR(5),

	@MinDose	FLOAT,

	@MaxDose	FLOAT,

	@MinVolume	FLOAT,

	@MaxVolume	FLOAT,

	@Penalty	FLOAT,

	@GridSize	FLOAT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@TPID,

	@OrganName,

	@StudyID,

	@Course,

	@PlanID,

	@SoftOrHard,

	@WindowType,

	@ConstraintType,

	@MinDose,

	@MaxDose,

	@MinVolume,

	@MaxVolume,

	@Penalty,

	@GridSize,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: tbPTV_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:47:00

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:47:00	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbPTV_DELETE

	 @TPID	BIGINT

	,@DBName NVARCHAR(100),

	@UpdateTime TIMESTAMP,

	@ApplyRowVersion BIT

AS

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbPTV

		where TPID = @TPID AND UpdateTime=@UpdateTime'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT,

		@UpdateTime TIMESTAMP',	

		@DBName,

		@TPID,

		@UpdateTime

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbPTV

		where TPID = @TPID'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT',	

		@DBName,

		@TPID

	END

go

CREATE  PROCEDURE SP_tbPTV_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM tbPTV'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: SP_tbSliceImage_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:47:11

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:47:11	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE [dbo].[SP_tbSliceImage_INSERT]

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@CTPointL	FLOAT,

	@CTPointW0	FLOAT,

	@CTPointH0	FLOAT,

	@CoordPoint	IMAGE,

	@RegisterDate	DATETIME,

	@SlicePositionmm	FLOAT,

	@SpacingInY	FLOAT,

	@RealCoordX	FLOAT,

	@RealCoordY	FLOAT,

	@RealCoordZ	FLOAT,

	@StudyID	BIGINT,

	@SliceUIDforDicom	VARCHAR(64),

	@OrganNumber	INT,

	@SliceImage	IMAGE,

	@CTValue	IMAGE,

	@TimeMs INT,

	@Phase INT,

	@Interpolate INT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)

 AS

	IF(@TPID is Null)

	BEGIN

		SELECT @TPID=-1

	END

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO tbSliceImage

	(

		TPID,

		CTPointL,

		CTPointW0,

		CTPointH0,

		CoordPoint,

		RegisterDate,

		SlicePositionmm,

		SpacingInY,

		RealCoordX,

		RealCoordY,

		RealCoordZ,

		StudyID,

		SliceUIDforDicom,

		OrganNumber,

		SliceImage,

		CTValue,

		TimeMs,

		Phase,

		Interpolate

	)

	VALUES

	(

		@TPID,

		@CTPointL,

		@CTPointW0,

		@CTPointH0,

		@CoordPoint,

		@RegisterDate,

		@SlicePositionmm,

		@SpacingInY,

		@RealCoordX,

		@RealCoordY,

		@RealCoordZ,

		@StudyID,

		@SliceUIDforDicom,

		@OrganNumber,

		@SliceImage,

		@CTValue,

		@TimeMs,

		@Phase,

		@Interpolate

	)

	SET @AutoID=SCOPE_IDENTITY()

	SELECT @TPID= TPID,@UpdateTime = UpdateTime FROM tbSliceImage WHERE AutoID=SCOPE_IDENTITY()

	IF (@TPID =-1)

	BEGIN

		SET @TPID=SCOPE_IDENTITY()

		UPDATE tbSliceImage SET TPID=@TPID  WHERE AutoID=@TPID

	SELECT @UpdateTime = UpdateTime FROM tbSliceImage WHERE AutoID=SCOPE_IDENTITY()

	END'

	EXECUTE  sp_executesql @sql,N'

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@CTPointL	FLOAT,

	@CTPointW0	FLOAT,

	@CTPointH0	FLOAT,

	@CoordPoint	IMAGE,

	@RegisterDate	DATETIME,

	@SlicePositionmm	FLOAT,

	@SpacingInY	FLOAT,

	@RealCoordX	FLOAT,

	@RealCoordY	FLOAT,

	@RealCoordZ	FLOAT,

	@StudyID	BIGINT,

	@SliceUIDforDicom	VARCHAR(64),

	@OrganNumber	INT,

	@SliceImage	IMAGE,

	@CTValue	IMAGE,

	@TimeMs INT,

	@Phase INT,

	@Interpolate INT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@AutoID OUTPUT,

	@TPID OUTPUT,

	@CTPointL,

	@CTPointW0,

	@CTPointH0,

	@CoordPoint,

	@RegisterDate,

	@SlicePositionmm,

	@SpacingInY,

	@RealCoordX,

	@RealCoordY,

	@RealCoordZ,

	@StudyID,

	@SliceUIDforDicom,

	@OrganNumber,

	@SliceImage,

	@CTValue,

	@TimeMs,

	@Phase,

	@Interpolate,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_tbSliceImage_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:47:11

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:47:11	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE [dbo].[SP_tbSliceImage_UPDATE]

	@TPID	BIGINT,

	@CTPointL	FLOAT,

	@CTPointW0	FLOAT,

	@CTPointH0	FLOAT,

	@CoordPoint	IMAGE,

	@RegisterDate	DATETIME,

	@SlicePositionmm	FLOAT,

	@SpacingInY	FLOAT,

	@RealCoordX	FLOAT,

	@RealCoordY	FLOAT,

	@RealCoordZ	FLOAT,

	@StudyID	BIGINT,

	@SliceUIDforDicom	VARCHAR(64),

	@OrganNumber	INT,

	@SliceImage	IMAGE,

	@CTValue	IMAGE,

	@TimeMs INT,

	@Phase INT,

	@Interpolate INT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)   ,

	@ApplyRowVersion BIT

 AS 

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' UPDATE tbSliceImage SET

		TPID = @TPID,

		CTPointL = @CTPointL,

		CTPointW0 = @CTPointW0,

		CTPointH0 = @CTPointH0,

		CoordPoint = @CoordPoint,

		RegisterDate = @RegisterDate,

		SlicePositionmm = @SlicePositionmm,

		SpacingInY = @SpacingInY,

		RealCoordX = @RealCoordX,

		RealCoordY = @RealCoordY,

		RealCoordZ = @RealCoordZ,

		StudyID = @StudyID,

		SliceUIDforDicom = @SliceUIDforDicom,

		OrganNumber = @OrganNumber,

		SliceImage = @SliceImage,

		CTValue = @CTValue,

		TimeMs = @TimeMs,

		Phase = @Phase,

		Interpolate = @Interpolate

		where TPID = @TPID AND UpdateTime=@UpdateTime

		SELECT @UpdateTime = UpdateTime FROM tbSliceImage WHERE TPID=@TPID'

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' UPDATE tbSliceImage SET

		TPID = @TPID,

		CTPointL = @CTPointL,

		CTPointW0 = @CTPointW0,

		CTPointH0 = @CTPointH0,

		CoordPoint = @CoordPoint,

		RegisterDate = @RegisterDate,

		SlicePositionmm = @SlicePositionmm,

		SpacingInY = @SpacingInY,

		RealCoordX = @RealCoordX,

		RealCoordY = @RealCoordY,

		RealCoordZ = @RealCoordZ,

		StudyID = @StudyID,

		SliceUIDforDicom = @SliceUIDforDicom,

		OrganNumber = @OrganNumber,

	    SliceImage = @SliceImage,

		CTValue = @CTValue,

		TimeMs = @TimeMs,

		Phase = @Phase,

		Interpolate = @Interpolate

		where TPID = @TPID

		SELECT @UpdateTime = UpdateTime FROM tbSliceImage WHERE TPID=@TPID'

	END

	EXECUTE  sp_executesql @sql,N'

	@TPID	BIGINT ,

	@CTPointL	FLOAT,

	@CTPointW0	FLOAT,

	@CTPointH0	FLOAT,

	@CoordPoint	IMAGE,

	@RegisterDate	DATETIME,

	@SlicePositionmm	FLOAT,

	@SpacingInY	FLOAT,

	@RealCoordX	FLOAT,

	@RealCoordY	FLOAT,

	@RealCoordZ	FLOAT,

	@StudyID	BIGINT ,

	@SliceUIDforDicom	VARCHAR(64),

	@OrganNumber	INT,

	@SliceImage	IMAGE,

	@CTValue	IMAGE,

	@TimeMs INT,

	@Phase INT,

    @Interpolate INT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@TPID,

	@CTPointL,

	@CTPointW0,

	@CTPointH0,

	@CoordPoint,

	@RegisterDate,

	@SlicePositionmm,

	@SpacingInY,

	@RealCoordX,

	@RealCoordY,

	@RealCoordZ,

	@StudyID,

	@SliceUIDforDicom,

	@OrganNumber,

	@SliceImage,

	@CTValue,

	@TimeMs,

	@Phase,

	@Interpolate,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: tbSliceImage_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:47:11

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:47:11	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbSliceImage_DELETE

	 @TPID	BIGINT

	,@DBName NVARCHAR(100),

	@UpdateTime TIMESTAMP,

	@ApplyRowVersion BIT

AS

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbSliceImage

		where TPID = @TPID AND UpdateTime=@UpdateTime'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT,

		@UpdateTime TIMESTAMP',	

		@DBName,

		@TPID,

		@UpdateTime

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbSliceImage

		where TPID = @TPID'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT',	

		@DBName,

		@TPID

	END

go

CREATE  PROCEDURE SP_tbSliceImage_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM tbSliceImage'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: SP_tbStudy_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:47:33

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:47:33	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE [dbo].[SP_tbStudy_INSERT]

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@Study	VARCHAR(20),

	@StudyAlias	VARCHAR(20),

	@MasterStudyID	BIGINT,

	@SlaveStudyID	BIGINT,

	@ImageType	VARCHAR(50),

	@FixedType	VARCHAR(20),

	@EstabDate	DATETIME,

	@Consistent	BIT,

	@HeadInside	BIT,

	@Supine	BIT,

	@Note	VARCHAR(30),

	@CTOriginX	FLOAT,

	@CTOriginY	FLOAT,

	@CTOriginZ	FLOAT,

	@FrameOriginX	FLOAT,

	@FrameOriginY	FLOAT,

	@FrameOriginZ	FLOAT,

	@FrameDirectionX  BIT,

	@FrameDirectionY  BIT,

	@FrameDirectionZ  BIT,

	@CoronalImage   IMAGE,

	@SagittalImage   IMAGE,

	@CTUpdateTime NVARCHAR(100),

	@CTCompressed INT,

	@CTSize	INT,

	@CTRange	INT,

	@CTLevel	INT,

	@CTWindow	INT,

	@StudyUIDforDicom	VARCHAR(64),

	@SeriesUIDforDicom	VARCHAR(64),

	@FusionPoints	IMAGE,

	@MasterPatientID	INT,

	@SlavePatientID	INT,

	@Checked	BIT,

	@DoseCalculated	BIT,

	@Locked	BIT,

	@DownloadRecord	IMAGE,

	@PixelSize	FLOAT,

	@DoctorName	VARCHAR(30),

	@MinCTSpacemm	FLOAT,

	@CTMachineType	VARCHAR(30),

	@CTCalibrationType	VARCHAR(30),

	@EnhancePoint	IMAGE,

	@Converse	BIT,

	@ModifyCT	IMAGE,

	@ANP	IMAGE,

	@Phase	IMAGE,

	@ImportType INT,

	@ImageStatus INT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)

 AS

	IF(@TPID is Null)

	BEGIN

		SELECT @TPID=-1

	END

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO tbStudy

	(

		TPID,

		Study,

		StudyAlias,

		MasterStudyID,

		SlaveStudyID,

		ImageType,

		FixedType,

		EstabDate,

		Consistent,

		HeadInside,

		Supine,

		Note,

		CTOriginX,

		CTOriginY,

		CTOriginZ,

		FrameOriginX,

		FrameOriginY,

		FrameOriginZ,

		FrameDirectionX,

		FrameDirectionY,

		FrameDirectionZ,

		CoronalImage,

		SagittalImage,

		CTUpdateTime,

		CTCompressed,

		CTSize,

		CTRange,

		CTLevel,

		CTWindow,

		StudyUIDforDicom,

		SeriesUIDforDicom,

		FusionPoints,

		MasterPatientID,

		SlavePatientID,

		Checked,

		DoseCalculated,

		Locked,

		DownloadRecord,

		PixelSize,

		DoctorName,

		MinCTSpacemm,

		CTMachineType,

		CTCalibrationType,

		EnhancePoint,

		Converse,

		ModifyCT,

		ANP,

		Phase,

		ImportType,

		ImageStatus

	)

	VALUES

	(

		@TPID,

		@Study,

		@StudyAlias,

		@MasterStudyID,

		@SlaveStudyID,

		@ImageType,

		@FixedType,

		@EstabDate,

		@Consistent,

		@HeadInside,

		@Supine,

		@Note,

		@CTOriginX,

		@CTOriginY,

		@CTOriginZ,

		@FrameOriginX,

		@FrameOriginY,

		@FrameOriginZ,

		@FrameDirectionX,

		@FrameDirectionY,

		@FrameDirectionZ,

		@CoronalImage,

		@SagittalImage,

		@CTUpdateTime,

		@CTCompressed,

		@CTSize,

		@CTRange,

		@CTLevel,

		@CTWindow,

		@StudyUIDforDicom,

		@SeriesUIDforDicom,

		@FusionPoints,

		@MasterPatientID,

		@SlavePatientID,

		@Checked,

		@DoseCalculated,

		@Locked,

		@DownloadRecord,

		@PixelSize,

		@DoctorName,

		@MinCTSpacemm,

		@CTMachineType,

		@CTCalibrationType,

		@EnhancePoint,

		@Converse,

		@ModifyCT,

		@ANP,

		@Phase,

		@ImportType,

		@ImageStatus

	)

	SET @AutoID=SCOPE_IDENTITY()

	SELECT @TPID= TPID,@UpdateTime = UpdateTime FROM tbStudy WHERE AutoID=SCOPE_IDENTITY()

	IF (@TPID =-1)

	BEGIN

		SET @TPID=SCOPE_IDENTITY()

		UPDATE tbStudy SET TPID=@TPID WHERE AutoID=@TPID

	SELECT @UpdateTime = UpdateTime FROM tbStudy WHERE AutoID=SCOPE_IDENTITY()

	END'

	EXECUTE  sp_executesql @sql,N'

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@Study	VARCHAR(20),

	@StudyAlias	VARCHAR(20),

	@MasterStudyID	BIGINT  ,

	@SlaveStudyID	BIGINT ,

	@ImageType	VARCHAR(50),

	@FixedType	VARCHAR(20),

	@EstabDate	DATETIME,

	@Consistent	BIT,

	@HeadInside	BIT,

	@Supine	BIT,

	@Note	VARCHAR(30),

	@CTOriginX	FLOAT,

	@CTOriginY	FLOAT,

	@CTOriginZ	FLOAT,

	@FrameOriginX	FLOAT,

	@FrameOriginY	FLOAT,

	@FrameOriginZ	FLOAT,

	@FrameDirectionX  BIT,

	@FrameDirectionY  BIT,

	@FrameDirectionZ  BIT,

	@CoronalImage   IMAGE,

	@SagittalImage   IMAGE,

	@CTUpdateTime NVARCHAR(100),

	@CTCompressed INT,

	@CTSize	INT,

	@CTRange	INT,

	@CTLevel	INT,

	@CTWindow	INT,

	@StudyUIDforDicom	VARCHAR(64),

	@SeriesUIDforDicom	VARCHAR(64),

	@FusionPoints	IMAGE,

	@MasterPatientID	INT,

	@SlavePatientID	INT,

	@Checked	BIT,

	@DoseCalculated	BIT,

	@Locked	BIT,

	@DownloadRecord	IMAGE,

	@PixelSize	FLOAT,

	@DoctorName	VARCHAR(30),

	@MinCTSpacemm	FLOAT,

	@CTMachineType	VARCHAR(30),

	@CTCalibrationType	VARCHAR(30),

	@EnhancePoint	IMAGE,

	@Converse	BIT,

	@ModifyCT	IMAGE,

	@ANP	IMAGE,

	@Phase	IMAGE,

	@ImportType INT,

	@ImageStatus INT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@AutoID OUTPUT,

	@TPID OUTPUT,

	@Study,

	@StudyAlias,

	@MasterStudyID,

	@SlaveStudyID,

	@ImageType,

	@FixedType,

	@EstabDate,

	@Consistent,

	@HeadInside,

	@Supine,

	@Note,

	@CTOriginX,

	@CTOriginY,

	@CTOriginZ,

	@FrameOriginX,

	@FrameOriginY,

	@FrameOriginZ,

	@FrameDirectionX  ,

	@FrameDirectionY  ,

	@FrameDirectionZ  ,

	@CoronalImage,

	@SagittalImage,

	@CTUpdateTime,

	@CTCompressed,

	@CTSize,

	@CTRange,

	@CTLevel,

	@CTWindow,

	@StudyUIDforDicom,

	@SeriesUIDforDicom,

	@FusionPoints,

	@MasterPatientID,

	@SlavePatientID,

	@Checked,

	@DoseCalculated,

	@Locked,

	@DownloadRecord,

	@PixelSize,

	@DoctorName,

	@MinCTSpacemm,

	@CTMachineType,

	@CTCalibrationType,

	@EnhancePoint,

	@Converse,

	@ModifyCT,

	@ANP,

	@Phase,

	@ImportType,

	@ImageStatus,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_tbStudy_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:47:33

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:47:33	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE [dbo].[SP_tbStudy_UPDATE]

	@TPID	BIGINT,

	@Study	VARCHAR(20),

	@StudyAlias	VARCHAR(20),

	@MasterStudyID	BIGINT,

	@SlaveStudyID	BIGINT,

	@ImageType	VARCHAR(50),

	@FixedType	VARCHAR(20),

	@EstabDate	DATETIME,

	@Consistent	BIT,

	@HeadInside	BIT,

	@Supine	BIT,

	@Note	VARCHAR(30),

	@CTOriginX	FLOAT,

	@CTOriginY	FLOAT,

	@CTOriginZ	FLOAT,

	@FrameOriginX	FLOAT,

	@FrameOriginY	FLOAT,

	@FrameOriginZ	FLOAT,

	@FrameDirectionX  BIT,

	@FrameDirectionY  BIT,

	@FrameDirectionZ  BIT,

	@CoronalImage   IMAGE,

	@SagittalImage   IMAGE,

	@CTUpdateTime NVARCHAR(100),

	@CTCompressed INT,

	@CTSize	INT,

	@CTRange	INT,

	@CTLevel	INT,

	@CTWindow	INT,

	@StudyUIDforDicom	VARCHAR(64),

	@SeriesUIDforDicom	VARCHAR(64),

	@FusionPoints	IMAGE,

	@MasterPatientID	INT,

	@SlavePatientID	INT,

	@Checked	BIT,

	@DoseCalculated	BIT,

	@Locked	BIT,

	@DownloadRecord	IMAGE,

	@PixelSize	FLOAT,

	@DoctorName	VARCHAR(30),

	@MinCTSpacemm	FLOAT,

	@CTMachineType	VARCHAR(30),

	@CTCalibrationType	VARCHAR(30),

	@EnhancePoint	IMAGE,

	@Converse	BIT,

	@ModifyCT	IMAGE,

	@ANP	IMAGE,

	@Phase	IMAGE,

	@ImportType INT,

	@ImageStatus INT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)   ,

	@ApplyRowVersion BIT

 AS 

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' UPDATE tbStudy SET

		Study = @Study,

		StudyAlias = @StudyAlias,

		MasterStudyID = @MasterStudyID,

		SlaveStudyID = @SlaveStudyID,

		ImageType = @ImageType,

		FixedType = @FixedType,

		EstabDate = @EstabDate,

		Consistent = @Consistent,

		HeadInside = @HeadInside,

		Supine = @Supine,

		Note = @Note,

		CTOriginX = @CTOriginX,

		CTOriginY = @CTOriginY,

		CTOriginZ = @CTOriginZ,

		FrameOriginX = @FrameOriginX,

		FrameOriginY = @FrameOriginY,

		FrameOriginZ = @FrameOriginZ,

		FrameDirectionX = @FrameDirectionX,

		FrameDirectionY = @FrameDirectionY,

		FrameDirectionZ = @FrameDirectionZ,

		CoronalImage = @CoronalImage,

		SagittalImage = @SagittalImage,

		CTUpdateTime = @CTUpdateTime,

		CTCompressed = @CTCompressed,

		CTSize = @CTSize,

		CTRange = @CTRange,

		CTLevel = @CTLevel,

		CTWindow = @CTWindow,

		StudyUIDforDicom = @StudyUIDforDicom,

		SeriesUIDforDicom = @SeriesUIDforDicom,

		FusionPoints = @FusionPoints,

		MasterPatientID = @MasterPatientID,

		SlavePatientID = @SlavePatientID,

		Checked = @Checked,

		DoseCalculated = @DoseCalculated,

		Locked = @Locked,

		DownloadRecord = @DownloadRecord,

		PixelSize = @PixelSize,

		DoctorName = @DoctorName,

		MinCTSpacemm = @MinCTSpacemm,

		CTMachineType = @CTMachineType,

		CTCalibrationType = @CTCalibrationType,

		EnhancePoint = @EnhancePoint,

		Converse = @Converse,

		ModifyCT = @ModifyCT,

		ANP = @ANP,

		Phase = @Phase,

		ImportType = @ImportType,

		ImageStatus = @ImageStatus

		where TPID = @TPID AND UpdateTime=@UpdateTime

		SELECT @UpdateTime = UpdateTime FROM tbStudy WHERE TPID=@TPID'

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' UPDATE tbStudy SET

		Study = @Study,

		StudyAlias = @StudyAlias,

		MasterStudyID = @MasterStudyID,

		SlaveStudyID = @SlaveStudyID,

		ImageType = @ImageType,

		FixedType = @FixedType,

		EstabDate = @EstabDate,

		Consistent = @Consistent,

		HeadInside = @HeadInside,

		Supine = @Supine,

		Note = @Note,

		CTOriginX = @CTOriginX,

		CTOriginY = @CTOriginY,

		CTOriginZ = @CTOriginZ,

	    FrameOriginX = @FrameOriginX,

		FrameOriginY = @FrameOriginY,

		FrameOriginZ = @FrameOriginZ,

		FrameDirectionX = @FrameDirectionX,

		FrameDirectionY = @FrameDirectionY,

		FrameDirectionZ = @FrameDirectionZ,

		CoronalImage = @CoronalImage,

		SagittalImage = @SagittalImage,

		CTUpdateTime = @CTUpdateTime,

		CTCompressed = @CTCompressed,

		CTSize = @CTSize,

		CTRange = @CTRange,

		CTLevel = @CTLevel,

		CTWindow = @CTWindow,

		StudyUIDforDicom = @StudyUIDforDicom,

		SeriesUIDforDicom = @SeriesUIDforDicom,

		FusionPoints = @FusionPoints,

		MasterPatientID = @MasterPatientID,

		SlavePatientID = @SlavePatientID,

		Checked = @Checked,

		DoseCalculated = @DoseCalculated,

		Locked = @Locked,

		DownloadRecord = @DownloadRecord,

		PixelSize = @PixelSize,

		DoctorName = @DoctorName,

		MinCTSpacemm = @MinCTSpacemm,

		CTMachineType = @CTMachineType,

		CTCalibrationType = @CTCalibrationType,

		EnhancePoint = @EnhancePoint,

		Converse = @Converse,

		ModifyCT = @ModifyCT,

		ANP = @ANP,

		Phase = @Phase,

		ImportType =@ImportType,

		ImageStatus = @ImageStatus

		where TPID = @TPID

		SELECT @UpdateTime = UpdateTime FROM tbStudy WHERE TPID=@TPID'

	END

	EXECUTE  sp_executesql @sql,N'

	@TPID	BIGINT,

	@Study	VARCHAR(20),

	@StudyAlias	VARCHAR(20),

	@MasterStudyID	BIGINT ,

	@SlaveStudyID	BIGINT ,

	@ImageType	VARCHAR(50),

	@FixedType	VARCHAR(20),

	@EstabDate	DATETIME,

	@Consistent	BIT,

	@HeadInside	BIT,

	@Supine	BIT,

	@Note	VARCHAR(30),

	@CTOriginX	FLOAT,

	@CTOriginY	FLOAT,

	@CTOriginZ	FLOAT,

	@FrameOriginX	FLOAT,

	@FrameOriginY	FLOAT,

	@FrameOriginZ	FLOAT,

	@FrameDirectionX  BIT,

	@FrameDirectionY  BIT,

	@FrameDirectionZ  BIT,

	@CoronalImage   IMAGE,

	@SagittalImage   IMAGE,

	@CTUpdateTime NVARCHAR(100),

	@CTCompressed INT,

	@CTSize	INT,

	@CTRange	INT,

	@CTLevel	INT,

	@CTWindow	INT,

	@StudyUIDforDicom	VARCHAR(64),

	@SeriesUIDforDicom	VARCHAR(64),

	@FusionPoints	IMAGE,

	@MasterPatientID	INT,

	@SlavePatientID	INT,

	@Checked	BIT,

	@DoseCalculated	BIT,

	@Locked	BIT,

	@DownloadRecord	IMAGE,

	@PixelSize	FLOAT,

	@DoctorName	VARCHAR(30),

	@MinCTSpacemm	FLOAT,

	@CTMachineType	VARCHAR(30),

	@CTCalibrationType	VARCHAR(30),

	@EnhancePoint	IMAGE,

	@Converse	BIT,

	@ModifyCT	IMAGE,

	@ANP	IMAGE,

	@Phase	IMAGE,

	@ImportType INT,

	@ImageStatus INT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@TPID,

	@Study,

	@StudyAlias,

	@MasterStudyID,

	@SlaveStudyID,

	@ImageType,

	@FixedType,

	@EstabDate,

	@Consistent,

	@HeadInside,

	@Supine,

	@Note,

	@CTOriginX,

	@CTOriginY,

	@CTOriginZ,

	@FrameOriginX,

	@FrameOriginY,

	@FrameOriginZ,

	@FrameDirectionX,

	@FrameDirectionY,

	@FrameDirectionZ,

	@CoronalImage,

	@SagittalImage,

	@CTUpdateTime,

	@CTCompressed,

	@CTSize,

	@CTRange,

	@CTLevel,

	@CTWindow,

	@StudyUIDforDicom,

	@SeriesUIDforDicom,

	@FusionPoints,

	@MasterPatientID,

	@SlavePatientID,

	@Checked,

	@DoseCalculated,

	@Locked,

	@DownloadRecord,

	@PixelSize,

	@DoctorName,

	@MinCTSpacemm,

	@CTMachineType,

	@CTCalibrationType,

	@EnhancePoint,

	@Converse,

	@ModifyCT,

	@ANP,

	@Phase,

	@ImportType,

	@ImageStatus,

	@UpdateTime OUTPUT,

	@DBName

go
/******************************************************************

* ÂêçÁß∞: tbStudy_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:47:33

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:47:33	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbStudy_DELETE

	 @TPID	BIGINT

	,@DBName NVARCHAR(100),

	@UpdateTime TIMESTAMP,

	@ApplyRowVersion BIT

AS

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbStudy

		where TPID = @TPID AND UpdateTime=@UpdateTime'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT,

		@UpdateTime TIMESTAMP',	

		@DBName,

		@TPID,

		@UpdateTime

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbStudy

		where TPID = @TPID'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT',	

		@DBName,

		@TPID

	END

go

CREATE  PROCEDURE SP_tbStudy_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM tbStudy'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: SP_tbTissue_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:47:42

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:47:42	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbTissue_INSERT

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@SliceID	BIGINT,

	@OrganName	VARCHAR(18),

	@OrganAttribute	VARCHAR(18),

	@SlicePoints	IMAGE,

	@Color	VARCHAR(12),

	@SlicePositionmm	FLOAT,

	@Density	FLOAT,

	@StudyID	BIGINT,

	@Valid	BIT,

	@Formal	BIT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)

 AS

	IF(@TPID is Null)

	BEGIN

		SELECT @TPID=-1

	END

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO tbTissue

	(

		TPID,

		SliceID,

		OrganName,

		OrganAttribute,

		SlicePoints,

		Color,

		SlicePositionmm,

		Density,

		StudyID,

		Valid,

		Formal

	)

	VALUES

	(

		@TPID,

		@SliceID,

		@OrganName,

		@OrganAttribute,

		@SlicePoints,

		@Color,

		@SlicePositionmm,

		@Density,

		@StudyID,

		@Valid,

		@Formal

	)

	SET @AutoID=SCOPE_IDENTITY()

	SELECT @TPID= TPID,@UpdateTime = UpdateTime FROM tbTissue WHERE AutoID=SCOPE_IDENTITY()

	IF (@TPID =-1)

	BEGIN

		SET @TPID=SCOPE_IDENTITY()

		UPDATE tbTissue SET TPID=@TPID  WHERE AutoID=@TPID

	SELECT @UpdateTime = UpdateTime FROM tbTissue WHERE AutoID=SCOPE_IDENTITY()

	END'

	EXECUTE  sp_executesql @sql,N'

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@SliceID	BIGINT,

	@OrganName	VARCHAR(18),

	@OrganAttribute	VARCHAR(18),

	@SlicePoints	IMAGE,

	@Color	VARCHAR(12),

	@SlicePositionmm	FLOAT,

	@Density	FLOAT,

	@StudyID	BIGINT,

	@Valid	BIT,

	@Formal	BIT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@AutoID OUTPUT,

	@TPID OUTPUT,

	@SliceID,

	@OrganName,

	@OrganAttribute,

	@SlicePoints,

	@Color,

	@SlicePositionmm,

	@Density,

	@StudyID,

	@Valid,

	@Formal,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_tbTissue_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:47:42

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:47:42	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbTissue_UPDATE

	@TPID	BIGINT,

	@SliceID	BIGINT,

	@OrganName	VARCHAR(18),

	@OrganAttribute	VARCHAR(18),

	@SlicePoints	IMAGE,

	@Color	VARCHAR(12),

	@SlicePositionmm	FLOAT,

	@Density	FLOAT,

	@StudyID	BIGINT,

	@Valid	BIT,

	@Formal	BIT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)   ,

	@ApplyRowVersion BIT

 AS 

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' UPDATE tbTissue SET

		TPID = @TPID,

		SliceID = @SliceID,

		OrganName = @OrganName,

		OrganAttribute = @OrganAttribute,

		SlicePoints = @SlicePoints,

		Color = @Color,

		SlicePositionmm = @SlicePositionmm,

		Density = @Density,

		StudyID = @StudyID,

		Valid = @Valid,

		Formal = @Formal

		where TPID = @TPID AND UpdateTime=@UpdateTime

		SELECT @UpdateTime = UpdateTime FROM tbTissue WHERE TPID=@TPID'

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' UPDATE tbTissue SET

		TPID = @TPID,

		SliceID = @SliceID,

		OrganName = @OrganName,

		OrganAttribute = @OrganAttribute,

		SlicePoints = @SlicePoints,

		Color = @Color,

		SlicePositionmm = @SlicePositionmm,

		Density = @Density,

		StudyID = @StudyID,

		Valid = @Valid,

		Formal = @Formal

		where TPID = @TPID

		SELECT @UpdateTime = UpdateTime FROM tbTissue WHERE TPID=@TPID'

	END

	EXECUTE  sp_executesql @sql,N'

	@TPID	BIGINT,

	@SliceID	BIGINT,

	@OrganName	VARCHAR(18),

	@OrganAttribute	VARCHAR(18),

	@SlicePoints	IMAGE,

	@Color	VARCHAR(12),

	@SlicePositionmm	FLOAT,

	@Density	FLOAT,

	@StudyID	BIGINT,

	@Valid	BIT,

	@Formal	BIT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@TPID,

	@SliceID,

	@OrganName,

	@OrganAttribute,

	@SlicePoints,

	@Color,

	@SlicePositionmm,

	@Density,

	@StudyID,

	@Valid,

	@Formal,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: tbTissue_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:47:42

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:47:42	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbTissue_DELETE

	 @TPID	BIGINT

	,@DBName NVARCHAR(100),

	@UpdateTime TIMESTAMP,

	@ApplyRowVersion BIT

AS

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbTissue

		where TPID = @TPID AND UpdateTime=@UpdateTime'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT,

		@UpdateTime TIMESTAMP',	

		@DBName,

		@TPID,

		@UpdateTime

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbTissue

		where TPID = @TPID'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT',	

		@DBName,

		@TPID

	END

go

CREATE  PROCEDURE SP_tbTissue_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM tbTissue'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: SP_tbSource_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:51:12

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:51:12	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbSource_INSERT

	@ID	INT OUTPUT,

	@Study	VARCHAR(10),

	@Course	VARCHAR(10),

	@Plan	VARCHAR(10),

	@PlanID	INT,

	@Type	VARCHAR(8),

	@SourceID	INT,

	@SourceName	VARCHAR(20),

	@SourceAlias	VARCHAR(20),

	@Duration	VARCHAR(16),

	@AX	FLOAT,

	@AY	FLOAT,

	@AZ	FLOAT,

	@BX	FLOAT,

	@BY	FLOAT,

	@BZ	FLOAT,

	@Activity	FLOAT,

	@AdjustedActivity	VARCHAR(50),

	@On	BIT,

	@DBName NVARCHAR(100)

 AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO tbSource

	(

		Study,

		Course,

		[Plan],

		PlanID,

		Type,

		SourceID,

		SourceName,

		SourceAlias,

		Duration,

		AX,

		AY,

		AZ,

		BX,

		[BY],

		BZ,

		Activity,

		AdjustedActivity,

		[On]

	)

	VALUES

	(

		@Study,

		@Course,

		@Plan,

		@PlanID,

		@Type,

		@SourceID,

		@SourceName,

		@SourceAlias,

		@Duration,

		@AX,

		@AY,

		@AZ,

		@BX,

		@BY,

		@BZ,

		@Activity,

		@AdjustedActivity,

		@On

	)

	SET @ID=SCOPE_IDENTITY()

	'

	EXECUTE  sp_executesql @sql,N'

	@ID	INT OUTPUT,

	@Study	VARCHAR(10),

	@Course	VARCHAR(10),

	@Plan	VARCHAR(10),

	@PlanID	INT,

	@Type	VARCHAR(8),

	@SourceID	INT,

	@SourceName	VARCHAR(20),

	@SourceAlias	VARCHAR(20),

	@Duration	VARCHAR(16),

	@AX	FLOAT,

	@AY	FLOAT,

	@AZ	FLOAT,

	@BX	FLOAT,

	@BY	FLOAT,

	@BZ	FLOAT,

	@Activity	FLOAT,

	@AdjustedActivity	VARCHAR(50),

	@On	BIT,

	@DBName NVARCHAR(100)',

	@ID OUTPUT,

	@Study,

	@Course,

	@Plan,

	@PlanID,

	@Type,

	@SourceID,

	@SourceName,

	@SourceAlias,

	@Duration,

	@AX,

	@AY,

	@AZ,

	@BX,

	@BY,

	@BZ,

	@Activity,

	@AdjustedActivity,

	@On,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_tbSource_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:51:12

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:51:12	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbSource_UPDATE

	@ID	INT,

	@Study	VARCHAR(10),

	@Course	VARCHAR(10),

	@Plan	VARCHAR(10),

	@PlanID	INT,

	@Type	VARCHAR(8),

	@SourceID	INT,

	@SourceName	VARCHAR(20),

	@SourceAlias	VARCHAR(20),

	@Duration	VARCHAR(16),

	@AX	FLOAT,

	@AY	FLOAT,

	@AZ	FLOAT,

	@BX	FLOAT,

	@BY	FLOAT,

	@BZ	FLOAT,

	@Activity	FLOAT,

	@AdjustedActivity	VARCHAR(50),

	@On	BIT,

	@DBName NVARCHAR(100)   

 AS 

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' UPDATE tbSource SET

		Study = @Study,

		Course = @Course,

		[Plan] = @Plan,

		PlanID = @PlanID,

		Type = @Type,

		SourceID = @SourceID,

		SourceName = @SourceName,

		SourceAlias = @SourceAlias,

		Duration = @Duration,

		AX = @AX,

		AY = @AY,

		AZ = @AZ,

		BX = @BX,

		[BY] = @BY,

		BZ = @BZ,

		Activity = @Activity,

		AdjustedActivity = @AdjustedActivity,

		[On] = @On

	where ID = @ID'

	EXECUTE  sp_executesql @sql,N'

	@ID	INT,

	@Study	VARCHAR(10),

	@Course	VARCHAR(10),

	@Plan	VARCHAR(10),

	@PlanID	INT,

	@Type	VARCHAR(8),

	@SourceID	INT,

	@SourceName	VARCHAR(20),

	@SourceAlias	VARCHAR(20),

	@Duration	VARCHAR(16),

	@AX	FLOAT,

	@AY	FLOAT,

	@AZ	FLOAT,

	@BX	FLOAT,

	@BY	FLOAT,

	@BZ	FLOAT,

	@Activity	FLOAT,

	@AdjustedActivity	VARCHAR(50),

	@On	BIT,

	@DBName NVARCHAR(100)',

	@ID,

	@Study,

	@Course,

	@Plan,

	@PlanID,

	@Type,

	@SourceID,

	@SourceName,

	@SourceAlias,

	@Duration,

	@AX,

	@AY,

	@AZ,

	@BX,

	@BY,

	@BZ,

	@Activity,

	@AdjustedActivity,

	@On,

	@DBName   

go
CREATE PROCEDURE SP_XrhGetDoseValueInfo 

 @dbName NVARCHAR(100),

 @Plan NVARCHAR(50),

 @SliceID INT,

 @Beam NVARCHAR(50),

 @Study NVARCHAR(50),

 @Description NVARCHAR(100)

AS

DECLARE @TableName NVARCHAR(100)

SET @TableName=SUBSTRING(@dbName,1,9)+'.dbo.ISOLevel'

EXEC('SELECT A.Prescribeddose,A.UserPrescribeLevel,A.PrescribeLevel,A.NormPointDose,A.CalculateAreaSpace,

A.CalculateAreaW0,A.CalculateAreaH0 FROM '+@dbName+'.dbo.tbPlan AS A WHERE A.[Plan]='+''''+@Plan+'''')

EXEC('SELECT A.ColumnCount,A.[RowCount] FROM '+@dbName+'.dbo.tbDose AS A 

WHERE A.SliceID='+@SliceID+' AND A.Beam ='+''''+@Beam+'''')

EXEC('SELECT A.Data FROM '+@dbName+'.dbo.tbDoseValue AS A INNER JOIN '+@dbName+'.dbo.tbDose AS B ON A.ID=B.ID 

WHERE B.SliceID='+@SliceID+' AND B.Beam ='+''''+@Beam+'''')

EXEC('SELECT A.PixelSize FROM '+@dbName+'.dbo.tbStudy AS A WHERE A.Study='+''''+@Study+'''')

EXEC('SELECT A.* FROM '+@TableName+' AS A WHERE A.Description='+''''+@Description+'''')

go
/******************************************************************

* ÂêçÁß∞: tbSource_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:51:12

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:51:12	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbSource_DELETE

	 @ID	int

	,@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' DELETE FROM tbSource

	where ID = @ID'

	EXECUTE  sp_executesql @sql,N'

	@DBName NVARCHAR(100),

	@ID	int',	

	@DBName,

	@ID

go

CREATE  PROCEDURE SP_tbSource_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM tbSource'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: SP_tbFilm_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:51:59

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:51:59	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbFilm_INSERT

	@FilmID	INT OUTPUT,

	@Study	CHAR(10),

	@GantryAngle	FLOAT,

	@SAD	FLOAT,

	@SFD	FLOAT,

	@OriginX	INT,

	@OriginY	INT,

	@ImageSize	INT,

	@PixelSizemm	FLOAT,

	@FilmImage	IMAGE,

	@DBName NVARCHAR(100)

 AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO tbFilm

	(

		Study,

		GantryAngle,

		SAD,

		SFD,

		OriginX,

		OriginY,

		ImageSize,

		PixelSizemm,

		FilmImage

	)

	VALUES

	(

		@Study,

		@GantryAngle,

		@SAD,

		@SFD,

		@OriginX,

		@OriginY,

		@ImageSize,

		@PixelSizemm,

		@FilmImage

	)

	SET @FilmID=SCOPE_IDENTITY()

	'

	EXECUTE  sp_executesql @sql,N'

	@FilmID	INT OUTPUT,

	@Study	CHAR(10),

	@GantryAngle	FLOAT,

	@SAD	FLOAT,

	@SFD	FLOAT,

	@OriginX	INT,

	@OriginY	INT,

	@ImageSize	INT,

	@PixelSizemm	FLOAT,

	@FilmImage	IMAGE,

	@DBName NVARCHAR(100)',

	@FilmID OUTPUT,

	@Study,

	@GantryAngle,

	@SAD,

	@SFD,

	@OriginX,

	@OriginY,

	@ImageSize,

	@PixelSizemm,

	@FilmImage,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_tbFilm_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:51:59

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:51:59	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbFilm_UPDATE

	@FilmID	INT,

	@Study	CHAR(10),

	@GantryAngle	FLOAT,

	@SAD	FLOAT,

	@SFD	FLOAT,

	@OriginX	INT,

	@OriginY	INT,

	@ImageSize	INT,

	@PixelSizemm	FLOAT,

	@FilmImage	IMAGE,

	@DBName NVARCHAR(100)   

 AS 

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' UPDATE tbFilm SET

		Study = @Study,

		GantryAngle = @GantryAngle,

		SAD = @SAD,

		SFD = @SFD,

		OriginX = @OriginX,

		OriginY = @OriginY,

		ImageSize = @ImageSize,

		PixelSizemm = @PixelSizemm,

		FilmImage = @FilmImage

	where FilmID = @FilmID'

	EXECUTE  sp_executesql @sql,N'

	@FilmID	INT,

	@Study	CHAR(10),

	@GantryAngle	FLOAT,

	@SAD	FLOAT,

	@SFD	FLOAT,

	@OriginX	INT,

	@OriginY	INT,

	@ImageSize	INT,

	@PixelSizemm	FLOAT,

	@FilmImage	IMAGE,

	@DBName NVARCHAR(100)',

	@FilmID,

	@Study,

	@GantryAngle,

	@SAD,

	@SFD,

	@OriginX,

	@OriginY,

	@ImageSize,

	@PixelSizemm,

	@FilmImage,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: tbFilm_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:51:59

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:51:59	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbFilm_DELETE

	 @FilmID	int

	,@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' DELETE FROM tbFilm

	where FilmID = @FilmID'

	EXECUTE  sp_executesql @sql,N'

	@DBName NVARCHAR(100),

	@FilmID	int',	

	@DBName,

	@FilmID

go

CREATE  PROCEDURE SP_tbFilm_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM tbFilm'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: SP_tbDwellPoint_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:52:22

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:52:22	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbDwellPoint_INSERT

	@ID	INT OUTPUT,

	@Study	VARCHAR(10),

	@Course	VARCHAR(10),

	@Plan	VARCHAR(10),

	@PlanID	INT,

	@ChannelID	INT,

	@Distance	FLOAT,

	@Weight	FLOAT,

	@Activity	FLOAT,

	@PX	FLOAT,

	@PY	FLOAT,

	@PZ	FLOAT,

	@Time	FLOAT,

	@DBName NVARCHAR(100)

 AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO tbDwellPoint

	(

		Study,

		Course,

		[Plan],

		PlanID,

		ChannelID,

		Distance,

		Weight,

		Activity,

		PX,

		PY,

		PZ,

		[Time]

	)

	VALUES

	(

		@Study,

		@Course,

		@Plan,

		@PlanID,

		@ChannelID,

		@Distance,

		@Weight,

		@Activity,

		@PX,

		@PY,

		@PZ,

		@Time

	)

	SET @ID=SCOPE_IDENTITY()

	'

	EXECUTE  sp_executesql @sql,N'

	@ID	INT OUTPUT,

	@Study	VARCHAR(10),

	@Course	VARCHAR(10),

	@Plan	VARCHAR(10),

	@PlanID	INT,

	@ChannelID	INT,

	@Distance	FLOAT,

	@Weight	FLOAT,

	@Activity	FLOAT,

	@PX	FLOAT,

	@PY	FLOAT,

	@PZ	FLOAT,

	@Time	FLOAT,

	@DBName NVARCHAR(100)',

	@ID OUTPUT,

	@Study,

	@Course,

	@Plan,

	@PlanID,

	@ChannelID,

	@Distance,

	@Weight,

	@Activity,

	@PX,

	@PY,

	@PZ,

	@Time,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_tbDwellPoint_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:52:22

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:52:22	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbDwellPoint_UPDATE

	@ID	INT,

	@Study	VARCHAR(10),

	@Course	VARCHAR(10),

	@Plan	VARCHAR(10),

	@PlanID	INT,

	@ChannelID	INT,

	@Distance	FLOAT,

	@Weight	FLOAT,

	@Activity	FLOAT,

	@PX	FLOAT,

	@PY	FLOAT,

	@PZ	FLOAT,

	@Time	FLOAT,

	@DBName NVARCHAR(100)   

 AS 

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' UPDATE tbDwellPoint SET

		Study = @Study,

		Course = @Course,

		[Plan] = @Plan,

		PlanID = @PlanID,

		ChannelID = @ChannelID,

		Distance = @Distance,

		Weight = @Weight,

		Activity = @Activity,

		PX = @PX,

		PY = @PY,

		PZ = @PZ,

		[Time] = @Time

	where ID = @ID'

	EXECUTE  sp_executesql @sql,N'

	@ID	INT,

	@Study	VARCHAR(10),

	@Course	VARCHAR(10),

	@Plan	VARCHAR(10),

	@PlanID	INT,

	@ChannelID	INT,

	@Distance	FLOAT,

	@Weight	FLOAT,

	@Activity	FLOAT,

	@PX	FLOAT,

	@PY	FLOAT,

	@PZ	FLOAT,

	@Time	FLOAT,

	@DBName NVARCHAR(100)',

	@ID,

	@Study,

	@Course,

	@Plan,

	@PlanID,

	@ChannelID,

	@Distance,

	@Weight,

	@Activity,

	@PX,

	@PY,

	@PZ,

	@Time,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: tbDwellPoint_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:52:22

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:52:22	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbDwellPoint_DELETE

	 @ID	int

	,@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' DELETE FROM tbDwellPoint

	where ID = @ID'

	EXECUTE  sp_executesql @sql,N'

	@DBName NVARCHAR(100),

	@ID	int',	

	@DBName,

	@ID

go

CREATE  PROCEDURE SP_tbDwellPoint_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM tbDwellPoint'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: SP_tbHDRChannel_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:52:46

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:52:46	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbHDRChannel_INSERT

	@ID	INT OUTPUT,

	@RegID	INT,

	@Study	VARCHAR(10),

	@Course	VARCHAR(10),

	@Plan	VARCHAR(10),

	@PlanID	INT,

	@ApplicatorID	INT,

	@ApplicatorModelNumber	VARCHAR(50),

	@SourceID	INT,

	@SourceName	VARCHAR(20),

	@Length	FLOAT,

	@Vertex	IMAGE,

	@On	BIT,

	@DBName NVARCHAR(100)

 AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO tbHDRChannel

	(

		RegID,

		Study,

		Course,

		[Plan],

		PlanID,

		ApplicatorID,

		ApplicatorModelNumber,

		SourceID,

		SourceName,

		Length,

		Vertex,

		[On]

	)

	VALUES

	(

		@RegID,

		@Study,

		@Course,

		@Plan,

		@PlanID,

		@ApplicatorID,

		@ApplicatorModelNumber,

		@SourceID,

		@SourceName,

		@Length,

		@Vertex,

		@On

	)

	SET @ID=SCOPE_IDENTITY()

	'

	EXECUTE  sp_executesql @sql,N'

	@ID	INT OUTPUT,

	@RegID	INT,

	@Study	VARCHAR(10),

	@Course	VARCHAR(10),

	@Plan	VARCHAR(10),

	@PlanID	INT,

	@ApplicatorID	INT,

	@ApplicatorModelNumber	VARCHAR(50),

	@SourceID	INT,

	@SourceName	VARCHAR(20),

	@Length	FLOAT,

	@Vertex	IMAGE,

	@On	BIT,

	@DBName NVARCHAR(100)',

	@ID OUTPUT,

	@RegID,

	@Study,

	@Course,

	@Plan,

	@PlanID,

	@ApplicatorID,

	@ApplicatorModelNumber,

	@SourceID,

	@SourceName,

	@Length,

	@Vertex,

	@On,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_tbHDRChannel_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:52:46

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:52:46	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbHDRChannel_UPDATE

	@ID	INT,

	@RegID	INT,

	@Study	VARCHAR(10),

	@Course	VARCHAR(10),

	@Plan	VARCHAR(10),

	@PlanID	INT,

	@ApplicatorID	INT,

	@ApplicatorModelNumber	VARCHAR(50),

	@SourceID	INT,

	@SourceName	VARCHAR(20),

	@Length	FLOAT,

	@Vertex	IMAGE,

	@On	BIT,

	@DBName NVARCHAR(100)   

 AS 

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' UPDATE tbHDRChannel SET

		RegID = @RegID,

		Study = @Study,

		Course = @Course,

		[Plan] = @Plan,

		PlanID = @PlanID,

		ApplicatorID = @ApplicatorID,

		ApplicatorModelNumber = @ApplicatorModelNumber,

		SourceID = @SourceID,

		SourceName = @SourceName,

		Length = @Length,

		Vertex = @Vertex,

		[On] = @On

	where ID = @ID'

	EXECUTE  sp_executesql @sql,N'

	@ID	INT,

	@RegID	INT,

	@Study	VARCHAR(10),

	@Course	VARCHAR(10),

	@Plan	VARCHAR(10),

	@PlanID	INT,

	@ApplicatorID	INT,

	@ApplicatorModelNumber	VARCHAR(50),

	@SourceID	INT,

	@SourceName	VARCHAR(20),

	@Length	FLOAT,

	@Vertex	IMAGE,

	@On	BIT,

	@DBName NVARCHAR(100)',

	@ID,

	@RegID,

	@Study,

	@Course,

	@Plan,

	@PlanID,

	@ApplicatorID,

	@ApplicatorModelNumber,

	@SourceID,

	@SourceName,

	@Length,

	@Vertex,

	@On,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: tbHDRChannel_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:52:46

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:52:46	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbHDRChannel_DELETE

	 @ID	int

	,@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' DELETE FROM tbHDRChannel

	where ID = @ID'

	EXECUTE  sp_executesql @sql,N'

	@DBName NVARCHAR(100),

	@ID	int',	

	@DBName,

	@ID

go

CREATE  PROCEDURE SP_tbHDRChannel_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM tbHDRChannel'

	EXEC(@Sql)

go
CREATE  PROCEDURE SP_tbDoseCalculateFactor_SELECT

@dbName  VARCHAR(100)

AS

declare @TableName VARCHAR(100)

set @TableName='.dbo.tbDoseCalculateFactor'

exec('SELECT * FROM ' + @dbName + ''+@TableName+'')

go
/******************************************************************

* ÂêçÁß∞: SP_Compensator_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:56:31

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:56:31	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_Compensator_INSERT

	@ID	INT OUTPUT,

	@MachineID	INT,

	@Name	NVARCHAR(60),

	@CompensatorMaterial	NVARCHAR(60),

	@RadiationType	INT,

	@NominalEnergy	NVARCHAR(20),

	@Attenuation	FLOAT,

	@CalibrationFactor	FLOAT,

	@DBName NVARCHAR(100)

 AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO Compensator

	(

		MachineID,

		Name,

		CompensatorMaterial,

		RadiationType,

		NominalEnergy,

		Attenuation,

		CalibrationFactor

	)

	VALUES

	(

		@MachineID,

		@Name,

		@CompensatorMaterial,

		@RadiationType,

		@NominalEnergy,

		@Attenuation,

		@CalibrationFactor

	)

	SET @ID=SCOPE_IDENTITY()

	'

	EXECUTE  sp_executesql @sql,N'

	@ID	INT OUTPUT,

	@MachineID	INT,

	@Name	NVARCHAR(60),

	@CompensatorMaterial	NVARCHAR(60),

	@RadiationType	INT,

	@NominalEnergy	NVARCHAR(20),

	@Attenuation	FLOAT,

	@CalibrationFactor	FLOAT,

	@DBName NVARCHAR(100)',

	@ID OUTPUT,

	@MachineID,

	@Name,

	@CompensatorMaterial,

	@RadiationType,

	@NominalEnergy,

	@Attenuation,

	@CalibrationFactor,

	@DBName   

go
CREATE  PROCEDURE SP_tbDoseTable_SELECT

@dbName  VARCHAR(100)

AS

declare @TableName VARCHAR(100)

set @TableName='.dbo.tbDoseTable'

exec('SELECT * FROM ' + @dbName + ''+@TableName+'')

go
/******************************************************************

* ÂêçÁß∞: SP_Compensator_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:56:31

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:56:31	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_Compensator_UPDATE

	@ID	INT,

	@MachineID	INT,

	@Name	NVARCHAR(60),

	@CompensatorMaterial	NVARCHAR(60),

	@RadiationType	INT,

	@NominalEnergy	NVARCHAR(20),

	@Attenuation	FLOAT,

	@CalibrationFactor	FLOAT,

	@DBName NVARCHAR(100)   

 AS 

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' UPDATE Compensator SET

		MachineID = @MachineID,

		Name = @Name,

		CompensatorMaterial = @CompensatorMaterial,

		RadiationType = @RadiationType,

		NominalEnergy = @NominalEnergy,

		Attenuation = @Attenuation,

		CalibrationFactor = @CalibrationFactor

	where ID = @ID'

	EXECUTE  sp_executesql @sql,N'

	@ID	INT,

	@MachineID	INT,

	@Name	NVARCHAR(60),

	@CompensatorMaterial	NVARCHAR(60),

	@RadiationType	INT,

	@NominalEnergy	NVARCHAR(20),

	@Attenuation	FLOAT,

	@CalibrationFactor	FLOAT,

	@DBName NVARCHAR(100)',

	@ID,

	@MachineID,

	@Name,

	@CompensatorMaterial,

	@RadiationType,

	@NominalEnergy,

	@Attenuation,

	@CalibrationFactor,

	@DBName   

go
CREATE  PROCEDURE SP_tbFluenceMatrix_SELECT

@dbName  VARCHAR(100)

AS

declare @TableName VARCHAR(100)

set @TableName='.dbo.tbFluenceMatrix'

exec('SELECT * FROM ' + @dbName + ''+@TableName+'')

go
/******************************************************************

* ÂêçÁß∞: Compensator_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:56:31

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:56:31	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_Compensator_DELETE

	 @ID	int

	,@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' DELETE FROM Compensator

	where ID = @ID'

	EXECUTE  sp_executesql @sql,N'

	@DBName NVARCHAR(100),

	@ID	int',	

	@DBName,

	@ID

go
CREATE  PROCEDURE SP_tbPrimaryDepthDose_SELECT

@dbName  VARCHAR(100)

AS

declare @TableName VARCHAR(100)

set @TableName='.dbo.tbPrimaryDepthDose'

exec('SELECT * FROM ' + @dbName + ''+@TableName+'')

go

CREATE  PROCEDURE SP_Compensator_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM Compensator'

	EXEC(@Sql)

go
CREATE  PROCEDURE SP_tbPenumbraParameter_SELECT

@dbName  VARCHAR(100)

AS

declare @TableName VARCHAR(100)

set @TableName='.dbo.tbPenumbraParameter'

exec('SELECT * FROM ' + @dbName + ''+@TableName+'')

go
/******************************************************************

* ÂêçÁß∞: SP_Cone_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:56:42

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:56:42	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_Cone_INSERT

	@ID	INT OUTPUT,

	@MachineID	INT,

	@Name	NVARCHAR(60),

	@Diameter	FLOAT,

	@CirclePoints	IMAGE,

	@ConeOutput	FLOAT,

	@Height	FLOAT,

	@Sc	FLOAT,

	@DBName NVARCHAR(100)

 AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO Cone

	(

		MachineID,

		Name,

		Diameter,

		CirclePoints,

		ConeOutput,

		Height,

		Sc

	)

	VALUES

	(

		@MachineID,

		@Name,

		@Diameter,

		@CirclePoints,

		@ConeOutput,

		@Height,

		@Sc

	)

	SET @ID=SCOPE_IDENTITY()

	'

	EXECUTE  sp_executesql @sql,N'

	@ID	INT OUTPUT,

	@MachineID	INT,

	@Name	NVARCHAR(60),

	@Diameter	FLOAT,

	@CirclePoints	IMAGE,

	@ConeOutput	FLOAT,

	@Height	FLOAT,

	@Sc	FLOAT,

	@DBName NVARCHAR(100)',

	@ID OUTPUT,

	@MachineID,

	@Name,

	@Diameter,

	@CirclePoints,

	@ConeOutput,

	@Height,

	@Sc,

	@DBName   

go
CREATE  PROCEDURE SP_tbProfile_SELECT

@dbName  VARCHAR(100)

AS

declare @TableName VARCHAR(100)

set @TableName='.dbo.tbProfile'

exec('SELECT * FROM ' + @dbName + ''+@TableName+'')

go
/******************************************************************

* ÂêçÁß∞: SP_Cone_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:56:42

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:56:42	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_Cone_UPDATE

	@ID	INT,

	@MachineID	INT,

	@Name	NVARCHAR(60),

	@Diameter	FLOAT,

	@CirclePoints	IMAGE,

	@ConeOutput	FLOAT,

	@Height	FLOAT,

	@Sc	FLOAT,

	@DBName NVARCHAR(100)   

 AS 

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' UPDATE Cone SET

		MachineID = @MachineID,

		Name = @Name,

		Diameter = @Diameter,

		CirclePoints = @CirclePoints,

		ConeOutput = @ConeOutput,

		Height = @Height,

		Sc = @Sc

	where ID = @ID'

	EXECUTE  sp_executesql @sql,N'

	@ID	INT,

	@MachineID	INT,

	@Name	NVARCHAR(60),

	@Diameter	FLOAT,

	@CirclePoints	IMAGE,

	@ConeOutput	FLOAT,

	@Height	FLOAT,

	@Sc	FLOAT,

	@DBName NVARCHAR(100)',

	@ID,

	@MachineID,

	@Name,

	@Diameter,

	@CirclePoints,

	@ConeOutput,

	@Height,

	@Sc,

	@DBName   

go
CREATE  PROCEDURE SP_tbTriangleToSquare_SELECT

@dbName  VARCHAR(100)

AS

declare @TableName VARCHAR(100)

set @TableName='.dbo.tbTriangleToSquare'

exec('SELECT * FROM ' + @dbName + ''+@TableName+'')

go
/******************************************************************

* ÂêçÁß∞: Cone_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:56:42

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:56:42	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_Cone_DELETE

	 @ID	int

	,@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' DELETE FROM Cone

	where ID = @ID'

	EXECUTE  sp_executesql @sql,N'

	@DBName NVARCHAR(100),

	@ID	int',	

	@DBName,

	@ID

go
CREATE  PROCEDURE SP_tbMotorizedWedge_SELECT

@dbName  VARCHAR(100)

AS

declare @TableName VARCHAR(100)

set @TableName='.dbo.tbMotorizedWedge'

exec('SELECT * FROM ' + @dbName + ''+@TableName+'')

go

CREATE  PROCEDURE SP_Cone_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM Cone'

	EXEC(@Sql)

go
CREATE  PROCEDURE SP_tbDynamicWedge_SELECT

@dbName  VARCHAR(100)

AS

declare @TableName VARCHAR(100)

set @TableName='.dbo.tbDynamicWedge'

exec('SELECT * FROM ' + @dbName + ''+@TableName+'')

go
/******************************************************************

* ÂêçÁß∞: SP_FrameOffset_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:57:01

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:57:01	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_FrameOffset_INSERT

	@MachineID	INT,

	@FrameType	NVARCHAR(100),

	@OffsetW	FLOAT,

	@OffsetL	FLOAT,

	@OffsetH	FLOAT,

	@OffsetVerifyCRC	FLOAT,

	@DBName NVARCHAR(100)

 AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO FrameOffset

	(

		MachineID,

		FrameType,

		OffsetW,

		OffsetL,

		OffsetH,

		OffsetVerifyCRC

	)

	VALUES

	(

		@MachineID,

		@FrameType,

		@OffsetW,

		@OffsetL,

		@OffsetH,

		@OffsetVerifyCRC

	)

	'

	EXECUTE  sp_executesql @sql,N'

	@MachineID	INT,

	@FrameType	NVARCHAR(100),

	@OffsetW	FLOAT,

	@OffsetL	FLOAT,

	@OffsetH	FLOAT,

	@OffsetVerifyCRC	FLOAT,

	@DBName NVARCHAR(100)',

	@MachineID,

	@FrameType,

	@OffsetW,

	@OffsetL,

	@OffsetH,

	@OffsetVerifyCRC,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_FrameOffset_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:57:01

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:57:01	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_FrameOffset_UPDATE

	@MachineID	INT,

	@FrameType	NVARCHAR(100),

	@OffsetW	FLOAT,

	@OffsetL	FLOAT,

	@OffsetH	FLOAT,

	@OffsetVerifyCRC	FLOAT,

	@DBName NVARCHAR(100)   

 AS 

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' UPDATE FrameOffset SET

		MachineID = @MachineID,

		FrameType = @FrameType,

		OffsetW = @OffsetW,

		OffsetL = @OffsetL,

		OffsetH = @OffsetH,

		OffsetVerifyCRC = @OffsetVerifyCRC

	where MachineID = @MachineID AND FrameType=@FrameType'

	EXECUTE  sp_executesql @sql,N'

	@MachineID	INT,

	@FrameType	NVARCHAR(100),

	@OffsetW	FLOAT,

	@OffsetL	FLOAT,

	@OffsetH	FLOAT,

	@OffsetVerifyCRC	FLOAT,

	@DBName NVARCHAR(100)',

	@MachineID,

	@FrameType,

	@OffsetW,

	@OffsetL,

	@OffsetH,

	@OffsetVerifyCRC,

	@DBName

go
/******************************************************************

* ÂêçÁß∞: FrameOffset_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:57:01

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:57:01	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_FrameOffset_DELETE

	 @MachineID	int,

	 @FrameType NVARCHAR(50)

	,@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' DELETE FROM FrameOffset

	where MachineID = @MachineID AND FrameType=@FrameType'

	EXECUTE  sp_executesql @sql,N'

	@DBName NVARCHAR(100),

	@MachineID	int,

	@FrameType NVARCHAR(50)',	

	@DBName,

	@MachineID,

	@FrameType

go

CREATE  PROCEDURE SP_FrameOffset_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM FrameOffset'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: SP_Machine_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:57:13

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:57:13	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_Machine_INSERT

	@MachineID	INT OUTPUT,

	@HospitalID	INT,

	@MachineName	NVARCHAR(60),

	@AccessoryNumber	INT,

	@MachineType	NVARCHAR(60),

	@WedgeType	INT,

	@RegisterDate	SMALLDATETIME,

	@Comments	NVARCHAR(120),

	@DBName NVARCHAR(100)

 AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO Machine

	(

		HospitalID,

		MachineName,

		AccessoryNumber,

		MachineType,

		WedgeType,

		RegisterDate,

		Comments

	)

	VALUES

	(

		@HospitalID,

		@MachineName,

		@AccessoryNumber,

		@MachineType,

		@WedgeType,

		@RegisterDate,

		@Comments

	)

	SET @MachineID=SCOPE_IDENTITY()

	'

	EXECUTE  sp_executesql @sql,N'

	@MachineID	INT OUTPUT,

	@HospitalID	INT,

	@MachineName	NVARCHAR(60),

	@AccessoryNumber	INT,

	@MachineType	NVARCHAR(60),

	@WedgeType	INT,

	@RegisterDate	SMALLDATETIME,

	@Comments	NVARCHAR(120),

	@DBName NVARCHAR(100)',

	@MachineID OUTPUT,

	@HospitalID,

	@MachineName,

	@AccessoryNumber,

	@MachineType,

	@WedgeType,

	@RegisterDate,

	@Comments,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_Machine_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:57:13

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:57:13	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_Machine_UPDATE

	@MachineID	INT,

	@HospitalID	INT,

	@MachineName	NVARCHAR(60),

	@AccessoryNumber	INT,

	@MachineType	NVARCHAR(60),

	@WedgeType	INT,

	@RegisterDate	SMALLDATETIME,

	@Comments	NVARCHAR(120),

	@DBName NVARCHAR(100)   

 AS 

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' UPDATE Machine SET

		HospitalID = @HospitalID,

		MachineName = @MachineName,

		AccessoryNumber = @AccessoryNumber,

		MachineType = @MachineType,

		WedgeType = @WedgeType,

		RegisterDate = @RegisterDate,

		Comments = @Comments

	where MachineID = @MachineID'

	EXECUTE  sp_executesql @sql,N'

	@MachineID	INT,

	@HospitalID	INT,

	@MachineName	NVARCHAR(60),

	@AccessoryNumber	INT,

	@MachineType	NVARCHAR(60),

	@WedgeType	INT,

	@RegisterDate	SMALLDATETIME,

	@Comments	NVARCHAR(120),

	@DBName NVARCHAR(100)',

	@MachineID,

	@HospitalID,

	@MachineName,

	@AccessoryNumber,

	@MachineType,

	@WedgeType,

	@RegisterDate,

	@Comments,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: Machine_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:57:13

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:57:13	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_Machine_DELETE

	 @MachineID	int

	,@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' DELETE FROM Machine

	where MachineID = @MachineID'

	EXECUTE  sp_executesql @sql,N'

	@DBName NVARCHAR(100),

	@MachineID	int',	

	@DBName,

	@MachineID

go

CREATE  PROCEDURE SP_Machine_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM Machine'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: SP_MachineDimension_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:57:26

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:57:26	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_MachineDimension_INSERT

	@MachineID	INT,

	@GantryLength	FLOAT,

	@GantryWidth	FLOAT,

	@GantryHeight	FLOAT,

	@GantryAngelMin	FLOAT,

	@GantryAngelMax	FLOAT,

	@GantryArmLength	FLOAT,

	@GantryArmWidth	FLOAT,

	@GantryArmHeight	FLOAT,

	@BracketLength	FLOAT,

	@BracketWidth	FLOAT,

	@BracketHeight	FLOAT,

	@IsocenterGround	FLOAT,

	@CouchLength	FLOAT,

	@CouchWidth	FLOAT,

	@CouchDepth	FLOAT,

	@IsocenterCouchcenter	FLOAT,

	@CouchLateralMin	FLOAT,

	@CouchLateralMax	FLOAT,

	@CouchLongitudinalMin	FLOAT,

	@CouchLongitudinalMax	FLOAT,

	@CouchHeightMin	FLOAT,

	@CouchHeightMax	FLOAT,

	@PedestalLength	FLOAT,

	@PedestalWidth	FLOAT,

	@PedestalHeight	FLOAT,

	@IsocenterPedestalcenter	FLOAT,

	@DBName NVARCHAR(100)

 AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO MachineDimension

	(

		MachineID,

		GantryLength,

		GantryWidth,

		GantryHeight,

		GantryAngelMin,

		GantryAngelMax,

		GantryArmLength,

		GantryArmWidth,

		GantryArmHeight,

		BracketLength,

		BracketWidth,

		BracketHeight,

		IsocenterGround,

		CouchLength,

		CouchWidth,

		CouchDepth,

		IsocenterCouchcenter,

		CouchLateralMin,

		CouchLateralMax,

		CouchLongitudinalMin,

		CouchLongitudinalMax,

		CouchHeightMin,

		CouchHeightMax,

		PedestalLength,

		PedestalWidth,

		PedestalHeight,

		IsocenterPedestalcenter

	)

	VALUES

	(

		@MachineID,

		@GantryLength,

		@GantryWidth,

		@GantryHeight,

		@GantryAngelMin,

		@GantryAngelMax,

		@GantryArmLength,

		@GantryArmWidth,

		@GantryArmHeight,

		@BracketLength,

		@BracketWidth,

		@BracketHeight,

		@IsocenterGround,

		@CouchLength,

		@CouchWidth,

		@CouchDepth,

		@IsocenterCouchcenter,

		@CouchLateralMin,

		@CouchLateralMax,

		@CouchLongitudinalMin,

		@CouchLongitudinalMax,

		@CouchHeightMin,

		@CouchHeightMax,

		@PedestalLength,

		@PedestalWidth,

		@PedestalHeight,

		@IsocenterPedestalcenter

	)

	'

	EXECUTE  sp_executesql @sql,N'

	@MachineID	INT,

	@GantryLength	FLOAT,

	@GantryWidth	FLOAT,

	@GantryHeight	FLOAT,

	@GantryAngelMin	FLOAT,

	@GantryAngelMax	FLOAT,

	@GantryArmLength	FLOAT,

	@GantryArmWidth	FLOAT,

	@GantryArmHeight	FLOAT,

	@BracketLength	FLOAT,

	@BracketWidth	FLOAT,

	@BracketHeight	FLOAT,

	@IsocenterGround	FLOAT,

	@CouchLength	FLOAT,

	@CouchWidth	FLOAT,

	@CouchDepth	FLOAT,

	@IsocenterCouchcenter	FLOAT,

	@CouchLateralMin	FLOAT,

	@CouchLateralMax	FLOAT,

	@CouchLongitudinalMin	FLOAT,

	@CouchLongitudinalMax	FLOAT,

	@CouchHeightMin	FLOAT,

	@CouchHeightMax	FLOAT,

	@PedestalLength	FLOAT,

	@PedestalWidth	FLOAT,

	@PedestalHeight	FLOAT,

	@IsocenterPedestalcenter	FLOAT,

	@DBName NVARCHAR(100)',

	@MachineID,

	@GantryLength,

	@GantryWidth,

	@GantryHeight,

	@GantryAngelMin,

	@GantryAngelMax,

	@GantryArmLength,

	@GantryArmWidth,

	@GantryArmHeight,

	@BracketLength,

	@BracketWidth,

	@BracketHeight,

	@IsocenterGround,

	@CouchLength,

	@CouchWidth,

	@CouchDepth,

	@IsocenterCouchcenter,

	@CouchLateralMin,

	@CouchLateralMax,

	@CouchLongitudinalMin,

	@CouchLongitudinalMax,

	@CouchHeightMin,

	@CouchHeightMax,

	@PedestalLength,

	@PedestalWidth,

	@PedestalHeight,

	@IsocenterPedestalcenter,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_MachineDimension_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:57:26

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:57:26	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_MachineDimension_UPDATE

	@MachineID	INT,

	@GantryLength	FLOAT,

	@GantryWidth	FLOAT,

	@GantryHeight	FLOAT,

	@GantryAngelMin	FLOAT,

	@GantryAngelMax	FLOAT,

	@GantryArmLength	FLOAT,

	@GantryArmWidth	FLOAT,

	@GantryArmHeight	FLOAT,

	@BracketLength	FLOAT,

	@BracketWidth	FLOAT,

	@BracketHeight	FLOAT,

	@IsocenterGround	FLOAT,

	@CouchLength	FLOAT,

	@CouchWidth	FLOAT,

	@CouchDepth	FLOAT,

	@IsocenterCouchcenter	FLOAT,

	@CouchLateralMin	FLOAT,

	@CouchLateralMax	FLOAT,

	@CouchLongitudinalMin	FLOAT,

	@CouchLongitudinalMax	FLOAT,

	@CouchHeightMin	FLOAT,

	@CouchHeightMax	FLOAT,

	@PedestalLength	FLOAT,

	@PedestalWidth	FLOAT,

	@PedestalHeight	FLOAT,

	@IsocenterPedestalcenter	FLOAT,

	@DBName NVARCHAR(100)   

 AS 

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' UPDATE MachineDimension SET

		MachineID = @MachineID,

		GantryLength = @GantryLength,

		GantryWidth = @GantryWidth,

		GantryHeight = @GantryHeight,

		GantryAngelMin = @GantryAngelMin,

		GantryAngelMax = @GantryAngelMax,

		GantryArmLength = @GantryArmLength,

		GantryArmWidth = @GantryArmWidth,

		GantryArmHeight = @GantryArmHeight,

		BracketLength = @BracketLength,

		BracketWidth = @BracketWidth,

		BracketHeight = @BracketHeight,

		IsocenterGround = @IsocenterGround,

		CouchLength = @CouchLength,

		CouchWidth = @CouchWidth,

		CouchDepth = @CouchDepth,

		IsocenterCouchcenter = @IsocenterCouchcenter,

		CouchLateralMin = @CouchLateralMin,

		CouchLateralMax = @CouchLateralMax,

		CouchLongitudinalMin = @CouchLongitudinalMin,

		CouchLongitudinalMax = @CouchLongitudinalMax,

		CouchHeightMin = @CouchHeightMin,

		CouchHeightMax = @CouchHeightMax,

		PedestalLength = @PedestalLength,

		PedestalWidth = @PedestalWidth,

		PedestalHeight = @PedestalHeight,

		IsocenterPedestalcenter = @IsocenterPedestalcenter

	where MachineID = @MachineID'

	EXECUTE  sp_executesql @sql,N'

	@MachineID	INT,

	@GantryLength	FLOAT,

	@GantryWidth	FLOAT,

	@GantryHeight	FLOAT,

	@GantryAngelMin	FLOAT,

	@GantryAngelMax	FLOAT,

	@GantryArmLength	FLOAT,

	@GantryArmWidth	FLOAT,

	@GantryArmHeight	FLOAT,

	@BracketLength	FLOAT,

	@BracketWidth	FLOAT,

	@BracketHeight	FLOAT,

	@IsocenterGround	FLOAT,

	@CouchLength	FLOAT,

	@CouchWidth	FLOAT,

	@CouchDepth	FLOAT,

	@IsocenterCouchcenter	FLOAT,

	@CouchLateralMin	FLOAT,

	@CouchLateralMax	FLOAT,

	@CouchLongitudinalMin	FLOAT,

	@CouchLongitudinalMax	FLOAT,

	@CouchHeightMin	FLOAT,

	@CouchHeightMax	FLOAT,

	@PedestalLength	FLOAT,

	@PedestalWidth	FLOAT,

	@PedestalHeight	FLOAT,

	@IsocenterPedestalcenter	FLOAT,

	@DBName NVARCHAR(100)',

	@MachineID,

	@GantryLength,

	@GantryWidth,

	@GantryHeight,

	@GantryAngelMin,

	@GantryAngelMax,

	@GantryArmLength,

	@GantryArmWidth,

	@GantryArmHeight,

	@BracketLength,

	@BracketWidth,

	@BracketHeight,

	@IsocenterGround,

	@CouchLength,

	@CouchWidth,

	@CouchDepth,

	@IsocenterCouchcenter,

	@CouchLateralMin,

	@CouchLateralMax,

	@CouchLongitudinalMin,

	@CouchLongitudinalMax,

	@CouchHeightMin,

	@CouchHeightMax,

	@PedestalLength,

	@PedestalWidth,

	@PedestalHeight,

	@IsocenterPedestalcenter,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: MachineDimension_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:57:26

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:57:26	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_MachineDimension_DELETE

	 @MachineID	int

	,@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' DELETE FROM MachineDimension

	where MachineID = @MachineID'

	EXECUTE  sp_executesql @sql,N'

	@DBName NVARCHAR(100),

	@MachineID	int',	

	@DBName,

	@MachineID

go

CREATE  PROCEDURE SP_MachineDimension_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM MachineDimension'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: SP_MachineEnergy_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:57:36

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:57:36	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_MachineEnergy_INSERT

	@EnergyID	INT OUTPUT,

	@MachineID	INT,

	@Energy	FLOAT,

	@RadiationType	INT,

	@GantryAngle	FLOAT,

	@CollimatorAngle	FLOAT,

	@CalibrationDoseOutput	FLOAT,

	@CalibrationDate	SMALLDATETIME,

	@Setup	INT,

	@DMax	FLOAT,

	@DetectorType	INT,

	@TransmissionFactor	FLOAT,

	@PenumbraInsideBlock	FLOAT,

	@PenumbraOutsideBlock	FLOAT,

	@TrayFactor	FLOAT,

	@FocusPencilPlaneDis	FLOAT,

	@PencilBeamSigma	FLOAT,

	@PracticalRange	FLOAT,

	@PencilBeamFMCS	FLOAT,

	@SigmaPrimary	FLOAT,

	@SourceDisplacement	FLOAT,

	@Approved	BIT,

	@ApprovedDateTime	SMALLDATETIME,

	@SourceSurfaceDistance	FLOAT,

	@SourceFilmDistance	FLOAT,

	@SourceCalibrationDis	FLOAT,

	@SourceDiameter	FLOAT,

	@DBName NVARCHAR(100)

 AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO MachineEnergy

	(

		MachineID,

		Energy,

		RadiationType,

		GantryAngle,

		CollimatorAngle,

		CalibrationDoseOutput,

		CalibrationDate,

		Setup,

		DMax,

		DetectorType,

		TransmissionFactor,

		PenumbraInsideBlock,

		PenumbraOutsideBlock,

		TrayFactor,

		FocusPencilPlaneDis,

		PencilBeamSigma,

		PracticalRange,

		PencilBeamFMCS,

		SigmaPrimary,

		SourceDisplacement,

		Approved,

		ApprovedDateTime,

		SourceSurfaceDistance,

		SourceFilmDistance,

		SourceCalibrationDis,

		SourceDiameter

	)

	VALUES

	(

		@MachineID,

		@Energy,

		@RadiationType,

		@GantryAngle,

		@CollimatorAngle,

		@CalibrationDoseOutput,

		@CalibrationDate,

		@Setup,

		@DMax,

		@DetectorType,

		@TransmissionFactor,

		@PenumbraInsideBlock,

		@PenumbraOutsideBlock,

		@TrayFactor,

		@FocusPencilPlaneDis,

		@PencilBeamSigma,

		@PracticalRange,

		@PencilBeamFMCS,

		@SigmaPrimary,

		@SourceDisplacement,

		@Approved,

		@ApprovedDateTime,

		@SourceSurfaceDistance,

		@SourceFilmDistance,

		@SourceCalibrationDis,

		@SourceDiameter

	)

	SET @EnergyID=SCOPE_IDENTITY()

	'

	EXECUTE  sp_executesql @sql,N'

	@EnergyID	INT OUTPUT,

	@MachineID	INT,

	@Energy	FLOAT,

	@RadiationType	INT,

	@GantryAngle	FLOAT,

	@CollimatorAngle	FLOAT,

	@CalibrationDoseOutput	FLOAT,

	@CalibrationDate	SMALLDATETIME,

	@Setup	INT,

	@DMax	FLOAT,

	@DetectorType	INT,

	@TransmissionFactor	FLOAT,

	@PenumbraInsideBlock	FLOAT,

	@PenumbraOutsideBlock	FLOAT,

	@TrayFactor	FLOAT,

	@FocusPencilPlaneDis	FLOAT,

	@PencilBeamSigma	FLOAT,

	@PracticalRange	FLOAT,

	@PencilBeamFMCS	FLOAT,

	@SigmaPrimary	FLOAT,

	@SourceDisplacement	FLOAT,

	@Approved	BIT,

	@ApprovedDateTime	SMALLDATETIME,

	@SourceSurfaceDistance	FLOAT,

	@SourceFilmDistance	FLOAT,

	@SourceCalibrationDis	FLOAT,

	@SourceDiameter	FLOAT,

	@DBName NVARCHAR(100)',

	@EnergyID OUTPUT,

	@MachineID,

	@Energy,

	@RadiationType,

	@GantryAngle,

	@CollimatorAngle,

	@CalibrationDoseOutput,

	@CalibrationDate,

	@Setup,

	@DMax,

	@DetectorType,

	@TransmissionFactor,

	@PenumbraInsideBlock,

	@PenumbraOutsideBlock,

	@TrayFactor,

	@FocusPencilPlaneDis,

	@PencilBeamSigma,

	@PracticalRange,

	@PencilBeamFMCS,

	@SigmaPrimary,

	@SourceDisplacement,

	@Approved,

	@ApprovedDateTime,

	@SourceSurfaceDistance,

	@SourceFilmDistance,

	@SourceCalibrationDis,

	@SourceDiameter,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_MachineEnergy_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:57:36

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:57:36	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_MachineEnergy_UPDATE

	@EnergyID	INT,

	@MachineID	INT,

	@Energy	FLOAT,

	@RadiationType	INT,

	@GantryAngle	FLOAT,

	@CollimatorAngle	FLOAT,

	@CalibrationDoseOutput	FLOAT,

	@CalibrationDate	SMALLDATETIME,

	@Setup	INT,

	@DMax	FLOAT,

	@DetectorType	INT,

	@TransmissionFactor	FLOAT,

	@PenumbraInsideBlock	FLOAT,

	@PenumbraOutsideBlock	FLOAT,

	@TrayFactor	FLOAT,

	@FocusPencilPlaneDis	FLOAT,

	@PencilBeamSigma	FLOAT,

	@PracticalRange	FLOAT,

	@PencilBeamFMCS	FLOAT,

	@SigmaPrimary	FLOAT,

	@SourceDisplacement	FLOAT,

	@Approved	BIT,

	@ApprovedDateTime	SMALLDATETIME,

	@SourceSurfaceDistance	FLOAT,

	@SourceFilmDistance	FLOAT,

	@SourceCalibrationDis	FLOAT,

	@SourceDiameter	FLOAT,

	@DBName NVARCHAR(100)   

 AS 

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' UPDATE MachineEnergy SET

		MachineID = @MachineID,

		Energy = @Energy,

		RadiationType = @RadiationType,

		GantryAngle = @GantryAngle,

		CollimatorAngle = @CollimatorAngle,

		CalibrationDoseOutput = @CalibrationDoseOutput,

		CalibrationDate = @CalibrationDate,

		Setup = @Setup,

		DMax = @DMax,

		DetectorType = @DetectorType,

		TransmissionFactor = @TransmissionFactor,

		PenumbraInsideBlock = @PenumbraInsideBlock,

		PenumbraOutsideBlock = @PenumbraOutsideBlock,

		TrayFactor = @TrayFactor,

		FocusPencilPlaneDis = @FocusPencilPlaneDis,

		PencilBeamSigma = @PencilBeamSigma,

		PracticalRange = @PracticalRange,

		PencilBeamFMCS = @PencilBeamFMCS,

		SigmaPrimary = @SigmaPrimary,

		SourceDisplacement = @SourceDisplacement,

		Approved = @Approved,

		ApprovedDateTime = @ApprovedDateTime,

		SourceSurfaceDistance = @SourceSurfaceDistance,

		SourceFilmDistance = @SourceFilmDistance,

		SourceCalibrationDis = @SourceCalibrationDis,

		SourceDiameter = @SourceDiameter

	where EnergyID = @EnergyID'

	EXECUTE  sp_executesql @sql,N'

	@EnergyID	INT,

	@MachineID	INT,

	@Energy	FLOAT,

	@RadiationType	INT,

	@GantryAngle	FLOAT,

	@CollimatorAngle	FLOAT,

	@CalibrationDoseOutput	FLOAT,

	@CalibrationDate	SMALLDATETIME,

	@Setup	INT,

	@DMax	FLOAT,

	@DetectorType	INT,

	@TransmissionFactor	FLOAT,

	@PenumbraInsideBlock	FLOAT,

	@PenumbraOutsideBlock	FLOAT,

	@TrayFactor	FLOAT,

	@FocusPencilPlaneDis	FLOAT,

	@PencilBeamSigma	FLOAT,

	@PracticalRange	FLOAT,

	@PencilBeamFMCS	FLOAT,

	@SigmaPrimary	FLOAT,

	@SourceDisplacement	FLOAT,

	@Approved	BIT,

	@ApprovedDateTime	SMALLDATETIME,

	@SourceSurfaceDistance	FLOAT,

	@SourceFilmDistance	FLOAT,

	@SourceCalibrationDis	FLOAT,

	@SourceDiameter	FLOAT,

	@DBName NVARCHAR(100)',

	@EnergyID,

	@MachineID,

	@Energy,

	@RadiationType,

	@GantryAngle,

	@CollimatorAngle,

	@CalibrationDoseOutput,

	@CalibrationDate,

	@Setup,

	@DMax,

	@DetectorType,

	@TransmissionFactor,

	@PenumbraInsideBlock,

	@PenumbraOutsideBlock,

	@TrayFactor,

	@FocusPencilPlaneDis,

	@PencilBeamSigma,

	@PracticalRange,

	@PencilBeamFMCS,

	@SigmaPrimary,

	@SourceDisplacement,

	@Approved,

	@ApprovedDateTime,

	@SourceSurfaceDistance,

	@SourceFilmDistance,

	@SourceCalibrationDis,

	@SourceDiameter,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: MachineEnergy_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:57:36

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:57:36	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_MachineEnergy_DELETE

	 @EnergyID	int

	,@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' DELETE FROM MachineEnergy

	where EnergyID = @EnergyID'

	EXECUTE  sp_executesql @sql,N'

	@DBName NVARCHAR(100),

	@EnergyID	int',	

	@DBName,

	@EnergyID

go

CREATE  PROCEDURE SP_MachineEnergy_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM MachineEnergy'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: SP_MachineParam_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:57:47

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:57:47	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_MachineParam_INSERT

	@MachineID	INT,

	@GantryRotationOrigin	FLOAT,

	@GantryRotationSign	BIT,

	@GantryRotationLow	FLOAT,

	@GantryRotationHigh	FLOAT,

	@CollimRotationOrigin	FLOAT,

	@CollimRotationSign	BIT,

	@CollimRotationLow	FLOAT,

	@CollimRotationHigh	FLOAT,

	@JawConvention	INT,

	@JawConfiguration	INT,

	@TurntRotationOrigin	FLOAT,

	@TurntRotationSign	BIT,

	@TurntRotationLow	FLOAT,

	@TurntRotationHigh	FLOAT,

	@PedRotationOrigin	FLOAT,

	@PedRotationSign	BIT,

	@PedRotationLow	FLOAT,

	@PedRotationHigh	FLOAT,

	@OffofPedandTurntAng	FLOAT,

	@SourceAxisDistance	FLOAT,

	@SourceBlockDistance	FLOAT,

	@SourceCollimatorXDis	FLOAT,

	@SourceCollimatorYDis	FLOAT,

	@DBName NVARCHAR(100)

 AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO MachineParam

	(

		MachineID,

		GantryRotationOrigin,

		GantryRotationSign,

		GantryRotationLow,

		GantryRotationHigh,

		CollimRotationOrigin,

		CollimRotationSign,

		CollimRotationLow,

		CollimRotationHigh,

		JawConvention,

		JawConfiguration,

		TurntRotationOrigin,

		TurntRotationSign,

		TurntRotationLow,

		TurntRotationHigh,

		PedRotationOrigin,

		PedRotationSign,

		PedRotationLow,

		PedRotationHigh,

		OffofPedandTurntAng,

		SourceAxisDistance,

		SourceBlockDistance,

		SourceCollimatorXDis,

		SourceCollimatorYDis

	)

	VALUES

	(

		@MachineID,

		@GantryRotationOrigin,

		@GantryRotationSign,

		@GantryRotationLow,

		@GantryRotationHigh,

		@CollimRotationOrigin,

		@CollimRotationSign,

		@CollimRotationLow,

		@CollimRotationHigh,

		@JawConvention,

		@JawConfiguration,

		@TurntRotationOrigin,

		@TurntRotationSign,

		@TurntRotationLow,

		@TurntRotationHigh,

		@PedRotationOrigin,

		@PedRotationSign,

		@PedRotationLow,

		@PedRotationHigh,

		@OffofPedandTurntAng,

		@SourceAxisDistance,

		@SourceBlockDistance,

		@SourceCollimatorXDis,

		@SourceCollimatorYDis

	)

	'

	EXECUTE  sp_executesql @sql,N'

	@MachineID	INT,

	@GantryRotationOrigin	FLOAT,

	@GantryRotationSign	BIT,

	@GantryRotationLow	FLOAT,

	@GantryRotationHigh	FLOAT,

	@CollimRotationOrigin	FLOAT,

	@CollimRotationSign	BIT,

	@CollimRotationLow	FLOAT,

	@CollimRotationHigh	FLOAT,

	@JawConvention	INT,

	@JawConfiguration	INT,

	@TurntRotationOrigin	FLOAT,

	@TurntRotationSign	BIT,

	@TurntRotationLow	FLOAT,

	@TurntRotationHigh	FLOAT,

	@PedRotationOrigin	FLOAT,

	@PedRotationSign	BIT,

	@PedRotationLow	FLOAT,

	@PedRotationHigh	FLOAT,

	@OffofPedandTurntAng	FLOAT,

	@SourceAxisDistance	FLOAT,

	@SourceBlockDistance	FLOAT,

	@SourceCollimatorXDis	FLOAT,

	@SourceCollimatorYDis	FLOAT,

	@DBName NVARCHAR(100)',

	@MachineID,

	@GantryRotationOrigin,

	@GantryRotationSign,

	@GantryRotationLow,

	@GantryRotationHigh,

	@CollimRotationOrigin,

	@CollimRotationSign,

	@CollimRotationLow,

	@CollimRotationHigh,

	@JawConvention,

	@JawConfiguration,

	@TurntRotationOrigin,

	@TurntRotationSign,

	@TurntRotationLow,

	@TurntRotationHigh,

	@PedRotationOrigin,

	@PedRotationSign,

	@PedRotationLow,

	@PedRotationHigh,

	@OffofPedandTurntAng,

	@SourceAxisDistance,

	@SourceBlockDistance,

	@SourceCollimatorXDis,

	@SourceCollimatorYDis,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_MachineParam_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:57:47

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:57:47	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_MachineParam_UPDATE

	@MachineID	INT,

	@GantryRotationOrigin	FLOAT,

	@GantryRotationSign	BIT,

	@GantryRotationLow	FLOAT,

	@GantryRotationHigh	FLOAT,

	@CollimRotationOrigin	FLOAT,

	@CollimRotationSign	BIT,

	@CollimRotationLow	FLOAT,

	@CollimRotationHigh	FLOAT,

	@JawConvention	INT,

	@JawConfiguration	INT,

	@TurntRotationOrigin	FLOAT,

	@TurntRotationSign	BIT,

	@TurntRotationLow	FLOAT,

	@TurntRotationHigh	FLOAT,

	@PedRotationOrigin	FLOAT,

	@PedRotationSign	BIT,

	@PedRotationLow	FLOAT,

	@PedRotationHigh	FLOAT,

	@OffofPedandTurntAng	FLOAT,

	@SourceAxisDistance	FLOAT,

	@SourceBlockDistance	FLOAT,

	@SourceCollimatorXDis	FLOAT,

	@SourceCollimatorYDis	FLOAT,

	@DBName NVARCHAR(100)   

 AS 

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' UPDATE MachineParam SET

		MachineID = @MachineID,

		GantryRotationOrigin = @GantryRotationOrigin,

		GantryRotationSign = @GantryRotationSign,

		GantryRotationLow = @GantryRotationLow,

		GantryRotationHigh = @GantryRotationHigh,

		CollimRotationOrigin = @CollimRotationOrigin,

		CollimRotationSign = @CollimRotationSign,

		CollimRotationLow = @CollimRotationLow,

		CollimRotationHigh = @CollimRotationHigh,

		JawConvention = @JawConvention,

		JawConfiguration = @JawConfiguration,

		TurntRotationOrigin = @TurntRotationOrigin,

		TurntRotationSign = @TurntRotationSign,

		TurntRotationLow = @TurntRotationLow,

		TurntRotationHigh = @TurntRotationHigh,

		PedRotationOrigin = @PedRotationOrigin,

		PedRotationSign = @PedRotationSign,

		PedRotationLow = @PedRotationLow,

		PedRotationHigh = @PedRotationHigh,

		OffofPedandTurntAng = @OffofPedandTurntAng,

		SourceAxisDistance = @SourceAxisDistance,

		SourceBlockDistance = @SourceBlockDistance,

		SourceCollimatorXDis = @SourceCollimatorXDis,

		SourceCollimatorYDis = @SourceCollimatorYDis

	where MachineID = @MachineID'

	EXECUTE  sp_executesql @sql,N'

	@MachineID	INT,

	@GantryRotationOrigin	FLOAT,

	@GantryRotationSign	BIT,

	@GantryRotationLow	FLOAT,

	@GantryRotationHigh	FLOAT,

	@CollimRotationOrigin	FLOAT,

	@CollimRotationSign	BIT,

	@CollimRotationLow	FLOAT,

	@CollimRotationHigh	FLOAT,

	@JawConvention	INT,

	@JawConfiguration	INT,

	@TurntRotationOrigin	FLOAT,

	@TurntRotationSign	BIT,

	@TurntRotationLow	FLOAT,

	@TurntRotationHigh	FLOAT,

	@PedRotationOrigin	FLOAT,

	@PedRotationSign	BIT,

	@PedRotationLow	FLOAT,

	@PedRotationHigh	FLOAT,

	@OffofPedandTurntAng	FLOAT,

	@SourceAxisDistance	FLOAT,

	@SourceBlockDistance	FLOAT,

	@SourceCollimatorXDis	FLOAT,

	@SourceCollimatorYDis	FLOAT,

	@DBName NVARCHAR(100)',

	@MachineID,

	@GantryRotationOrigin,

	@GantryRotationSign,

	@GantryRotationLow,

	@GantryRotationHigh,

	@CollimRotationOrigin,

	@CollimRotationSign,

	@CollimRotationLow,

	@CollimRotationHigh,

	@JawConvention,

	@JawConfiguration,

	@TurntRotationOrigin,

	@TurntRotationSign,

	@TurntRotationLow,

	@TurntRotationHigh,

	@PedRotationOrigin,

	@PedRotationSign,

	@PedRotationLow,

	@PedRotationHigh,

	@OffofPedandTurntAng,

	@SourceAxisDistance,

	@SourceBlockDistance,

	@SourceCollimatorXDis,

	@SourceCollimatorYDis,

	@DBName   

go
CREATE PROCEDURE SP_OrganContour_GetMaxVolumnID

@dbName  VARCHAR(100)

AS

declare @TableName VARCHAR(100)

set @TableName='.dbo.tbOrganContour'

exec('SELECT max('+@dbName+@TableName+'.VolumnID) FROM ' + @dbName + ''+@TableName+'')

go
/******************************************************************

* ÂêçÁß∞: MachineParam_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:57:47

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:57:47	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_MachineParam_DELETE

	 @MachineID	int

	,@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' DELETE FROM MachineParam

	where MachineID = @MachineID'

	EXECUTE  sp_executesql @sql,N'

	@DBName NVARCHAR(100),

	@MachineID	int',	

	@DBName,

	@MachineID

go
CREATE PROCEDURE SP_Study_GetMaxStudyName

@DBName varchar(100)

 AS

exec( 'use '+@DBName+'  SELECT '

+''''+'Study_'+''''+'+CAST(MAX'+'('+'CAST(SUBSTRING'+'('+'Study,7,LEN'

+'('+'Study'+')'+'-6'+') AS INT)) AS NVARCHAR(50)) '+'FROM tbStudy')

go

CREATE  PROCEDURE SP_MachineParam_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM MachineParam'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: SP_MLC_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:57:59

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:57:59	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_MLC_INSERT

	@ID	INT OUTPUT,

	@MachineID	INT,

	@Name	NVARCHAR(60),

	@Manufacturer	NVARCHAR(60),

	@Model	NVARCHAR(60),

	@NumberofLeafPairs	INT,

	@NumberofLevels	INT,

	@LeafHeight	FLOAT,

	@BottomLeafHeight	FLOAT,

	@LeafThicknessArray	IMAGE,

	@MaxLengthofFieldSize	FLOAT,

	@MaxWidthofFieldSize	FLOAT,

	@MaxLeafSpeed	FLOAT,

	@MaxOvertravel	FLOAT,

	@BeamPenumbra	FLOAT,

	@MaxLeafLeakage	FLOAT,

	@MaxLeafTransmission	FLOAT,

	@IsIsocenterSize	BIT,

	@ClearancetoIsocenter	FLOAT,

	@LinacInterfaceHeight	FLOAT,

	@AlignmentRingThickness	FLOAT,

	@UpperLeafSpace	FLOAT,

	@InnerLeafSpace	FLOAT,

	@MLCLength	FLOAT,

	@MLCWidth	FLOAT,

	@MLCHeight	FLOAT,

	@LeafGap	FLOAT,

	@LeafTolerence	FLOAT,

	@MAXDVAFields	INT,

	@InsidePenumbraFactor	FLOAT,

	@OutsidePenumbraFactor	FLOAT,

	@SoursetoMLCTopEdge	FLOAT,

	@MinLeafSpeed	FLOAT,

	@MaxLeafAcceleration	FLOAT,

	@MaxExtendCarriage	FLOAT,

	@LeafSepCorrectioncm	FLOAT,

	@TungeGrooveWidthcm	FLOAT,

	@MLCTrayFactor	FLOAT,

	@DBName NVARCHAR(100)

 AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO MLC

	(

		MachineID,

		Name,

		Manufacturer,

		Model,

		NumberofLeafPairs,

		NumberofLevels,

		LeafHeight,

		BottomLeafHeight,

		LeafThicknessArray,

		MaxLengthofFieldSize,

		MaxWidthofFieldSize,

		MaxLeafSpeed,

		MaxOvertravel,

		BeamPenumbra,

		MaxLeafLeakage,

		MaxLeafTransmission,

		IsIsocenterSize,

		ClearancetoIsocenter,

		LinacInterfaceHeight,

		AlignmentRingThickness,

		UpperLeafSpace,

		InnerLeafSpace,

		MLCLength,

		MLCWidth,

		MLCHeight,

		LeafGap,

		LeafTolerence,

		MAXDVAFields,

		InsidePenumbraFactor,

		OutsidePenumbraFactor,

		SoursetoMLCTopEdge,

		MinLeafSpeed,

		MaxLeafAcceleration,

		MaxExtendCarriage,

		LeafSepCorrectioncm,

		TungeGrooveWidthcm,

		MLCTrayFactor

	)

	VALUES

	(

		@MachineID,

		@Name,

		@Manufacturer,

		@Model,

		@NumberofLeafPairs,

		@NumberofLevels,

		@LeafHeight,

		@BottomLeafHeight,

		@LeafThicknessArray,

		@MaxLengthofFieldSize,

		@MaxWidthofFieldSize,

		@MaxLeafSpeed,

		@MaxOvertravel,

		@BeamPenumbra,

		@MaxLeafLeakage,

		@MaxLeafTransmission,

		@IsIsocenterSize,

		@ClearancetoIsocenter,

		@LinacInterfaceHeight,

		@AlignmentRingThickness,

		@UpperLeafSpace,

		@InnerLeafSpace,

		@MLCLength,

		@MLCWidth,

		@MLCHeight,

		@LeafGap,

		@LeafTolerence,

		@MAXDVAFields,

		@InsidePenumbraFactor,

		@OutsidePenumbraFactor,

		@SoursetoMLCTopEdge,

		@MinLeafSpeed,

		@MaxLeafAcceleration,

		@MaxExtendCarriage,

		@LeafSepCorrectioncm,

		@TungeGrooveWidthcm,

		@MLCTrayFactor

	)

	SET @ID=SCOPE_IDENTITY()

	'

	EXECUTE  sp_executesql @sql,N'

	@ID	INT OUTPUT,

	@MachineID	INT,

	@Name	NVARCHAR(60),

	@Manufacturer	NVARCHAR(60),

	@Model	NVARCHAR(60),

	@NumberofLeafPairs	INT,

	@NumberofLevels	INT,

	@LeafHeight	FLOAT,

	@BottomLeafHeight	FLOAT,

	@LeafThicknessArray	IMAGE,

	@MaxLengthofFieldSize	FLOAT,

	@MaxWidthofFieldSize	FLOAT,

	@MaxLeafSpeed	FLOAT,

	@MaxOvertravel	FLOAT,

	@BeamPenumbra	FLOAT,

	@MaxLeafLeakage	FLOAT,

	@MaxLeafTransmission	FLOAT,

	@IsIsocenterSize	BIT,

	@ClearancetoIsocenter	FLOAT,

	@LinacInterfaceHeight	FLOAT,

	@AlignmentRingThickness	FLOAT,

	@UpperLeafSpace	FLOAT,

	@InnerLeafSpace	FLOAT,

	@MLCLength	FLOAT,

	@MLCWidth	FLOAT,

	@MLCHeight	FLOAT,

	@LeafGap	FLOAT,

	@LeafTolerence	FLOAT,

	@MAXDVAFields	INT,

	@InsidePenumbraFactor	FLOAT,

	@OutsidePenumbraFactor	FLOAT,

	@SoursetoMLCTopEdge	FLOAT,

	@MinLeafSpeed	FLOAT,

	@MaxLeafAcceleration	FLOAT,

	@MaxExtendCarriage	FLOAT,

	@LeafSepCorrectioncm	FLOAT,

	@TungeGrooveWidthcm	FLOAT,

	@MLCTrayFactor	FLOAT,

	@DBName NVARCHAR(100)',

	@ID OUTPUT,

	@MachineID,

	@Name,

	@Manufacturer,

	@Model,

	@NumberofLeafPairs,

	@NumberofLevels,

	@LeafHeight,

	@BottomLeafHeight,

	@LeafThicknessArray,

	@MaxLengthofFieldSize,

	@MaxWidthofFieldSize,

	@MaxLeafSpeed,

	@MaxOvertravel,

	@BeamPenumbra,

	@MaxLeafLeakage,

	@MaxLeafTransmission,

	@IsIsocenterSize,

	@ClearancetoIsocenter,

	@LinacInterfaceHeight,

	@AlignmentRingThickness,

	@UpperLeafSpace,

	@InnerLeafSpace,

	@MLCLength,

	@MLCWidth,

	@MLCHeight,

	@LeafGap,

	@LeafTolerence,

	@MAXDVAFields,

	@InsidePenumbraFactor,

	@OutsidePenumbraFactor,

	@SoursetoMLCTopEdge,

	@MinLeafSpeed,

	@MaxLeafAcceleration,

	@MaxExtendCarriage,

	@LeafSepCorrectioncm,

	@TungeGrooveWidthcm,

	@MLCTrayFactor,

	@DBName   

go
CREATE PROCEDURE SP_PartBeam_UPDATE

 @DBName NVARCHAR(100),

 @Beam NVARCHAR(8),

 @BEVIsoCenterXcm FLOAT,

 @BEVIsoCenterYcm FLOAT,

 @BEVPixelSizecm FLOAT,

 @BEVImageSize INT,

 @BEVImage IMAGE,

 @DrrValue IMAGE

AS

declare @sql NVARCHAR(500)

set @sql='USE '+@DBName

set @sql=@sql+' Update tbBeam set BEVIsoCenterXcm=@BEVIsoCenterXcm,BEVIsoCenterYcm=

@BEVIsoCenterYcm,BEVPixelSizecm=@BEVPixelSizecm,BEVImageSize=@BEVImageSize

,BEVImage=@BEVImage,DrrValue=@DrrValue WHERE Beam=@Beam'

EXECUTE  sp_executesql @sql,N'@dbName VARCHAR(100),@Beam  NVARCHAR(8),@BEVIsoCenterXcm FLOAT,

@BEVIsoCenterYcm FLOAT,@BEVPixelSizecm FLOAT,

@BEVImageSize INT,@BEVImage IMAGE,@DrrValue IMAGE',

@DBName,@Beam ,@BEVIsoCenterXcm ,

@BEVIsoCenterYcm ,@BEVPixelSizecm ,

@BEVImageSize ,@BEVImage ,@DrrValue

go
/******************************************************************

* ÂêçÁß∞: SP_MLC_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:57:59

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:57:59	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_MLC_UPDATE

	@ID	INT,

	@MachineID	INT,

	@Name	NVARCHAR(60),

	@Manufacturer	NVARCHAR(60),

	@Model	NVARCHAR(60),

	@NumberofLeafPairs	INT,

	@NumberofLevels	INT,

	@LeafHeight	FLOAT,

	@BottomLeafHeight	FLOAT,

	@LeafThicknessArray	IMAGE,

	@MaxLengthofFieldSize	FLOAT,

	@MaxWidthofFieldSize	FLOAT,

	@MaxLeafSpeed	FLOAT,

	@MaxOvertravel	FLOAT,

	@BeamPenumbra	FLOAT,

	@MaxLeafLeakage	FLOAT,

	@MaxLeafTransmission	FLOAT,

	@IsIsocenterSize	BIT,

	@ClearancetoIsocenter	FLOAT,

	@LinacInterfaceHeight	FLOAT,

	@AlignmentRingThickness	FLOAT,

	@UpperLeafSpace	FLOAT,

	@InnerLeafSpace	FLOAT,

	@MLCLength	FLOAT,

	@MLCWidth	FLOAT,

	@MLCHeight	FLOAT,

	@LeafGap	FLOAT,

	@LeafTolerence	FLOAT,

	@MAXDVAFields	INT,

	@InsidePenumbraFactor	FLOAT,

	@OutsidePenumbraFactor	FLOAT,

	@SoursetoMLCTopEdge	FLOAT,

	@MinLeafSpeed	FLOAT,

	@MaxLeafAcceleration	FLOAT,

	@MaxExtendCarriage	FLOAT,

	@LeafSepCorrectioncm	FLOAT,

	@TungeGrooveWidthcm	FLOAT,

	@MLCTrayFactor	FLOAT,

	@DBName NVARCHAR(100)   

 AS 

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' UPDATE MLC SET

		MachineID = @MachineID,

		Name = @Name,

		Manufacturer = @Manufacturer,

		Model = @Model,

		NumberofLeafPairs = @NumberofLeafPairs,

		NumberofLevels = @NumberofLevels,

		LeafHeight = @LeafHeight,

		BottomLeafHeight = @BottomLeafHeight,

		LeafThicknessArray = @LeafThicknessArray,

		MaxLengthofFieldSize = @MaxLengthofFieldSize,

		MaxWidthofFieldSize = @MaxWidthofFieldSize,

		MaxLeafSpeed = @MaxLeafSpeed,

		MaxOvertravel = @MaxOvertravel,

		BeamPenumbra = @BeamPenumbra,

		MaxLeafLeakage = @MaxLeafLeakage,

		MaxLeafTransmission = @MaxLeafTransmission,

		IsIsocenterSize = @IsIsocenterSize,

		ClearancetoIsocenter = @ClearancetoIsocenter,

		LinacInterfaceHeight = @LinacInterfaceHeight,

		AlignmentRingThickness = @AlignmentRingThickness,

		UpperLeafSpace = @UpperLeafSpace,

		InnerLeafSpace = @InnerLeafSpace,

		MLCLength = @MLCLength,

		MLCWidth = @MLCWidth,

		MLCHeight = @MLCHeight,

		LeafGap = @LeafGap,

		LeafTolerence = @LeafTolerence,

		MAXDVAFields = @MAXDVAFields,

		InsidePenumbraFactor = @InsidePenumbraFactor,

		OutsidePenumbraFactor = @OutsidePenumbraFactor,

		SoursetoMLCTopEdge = @SoursetoMLCTopEdge,

		MinLeafSpeed = @MinLeafSpeed,

		MaxLeafAcceleration = @MaxLeafAcceleration,

		MaxExtendCarriage = @MaxExtendCarriage,

		LeafSepCorrectioncm = @LeafSepCorrectioncm,

		TungeGrooveWidthcm = @TungeGrooveWidthcm,

		MLCTrayFactor = @MLCTrayFactor

	where ID = @ID'

	EXECUTE  sp_executesql @sql,N'

	@ID	INT,

	@MachineID	INT,

	@Name	NVARCHAR(60),

	@Manufacturer	NVARCHAR(60),

	@Model	NVARCHAR(60),

	@NumberofLeafPairs	INT,

	@NumberofLevels	INT,

	@LeafHeight	FLOAT,

	@BottomLeafHeight	FLOAT,

	@LeafThicknessArray	IMAGE,

	@MaxLengthofFieldSize	FLOAT,

	@MaxWidthofFieldSize	FLOAT,

	@MaxLeafSpeed	FLOAT,

	@MaxOvertravel	FLOAT,

	@BeamPenumbra	FLOAT,

	@MaxLeafLeakage	FLOAT,

	@MaxLeafTransmission	FLOAT,

	@IsIsocenterSize	BIT,

	@ClearancetoIsocenter	FLOAT,

	@LinacInterfaceHeight	FLOAT,

	@AlignmentRingThickness	FLOAT,

	@UpperLeafSpace	FLOAT,

	@InnerLeafSpace	FLOAT,

	@MLCLength	FLOAT,

	@MLCWidth	FLOAT,

	@MLCHeight	FLOAT,

	@LeafGap	FLOAT,

	@LeafTolerence	FLOAT,

	@MAXDVAFields	INT,

	@InsidePenumbraFactor	FLOAT,

	@OutsidePenumbraFactor	FLOAT,

	@SoursetoMLCTopEdge	FLOAT,

	@MinLeafSpeed	FLOAT,

	@MaxLeafAcceleration	FLOAT,

	@MaxExtendCarriage	FLOAT,

	@LeafSepCorrectioncm	FLOAT,

	@TungeGrooveWidthcm	FLOAT,

	@MLCTrayFactor	FLOAT,

	@DBName NVARCHAR(100)',

	@ID,

	@MachineID,

	@Name,

	@Manufacturer,

	@Model,

	@NumberofLeafPairs,

	@NumberofLevels,

	@LeafHeight,

	@BottomLeafHeight,

	@LeafThicknessArray,

	@MaxLengthofFieldSize,

	@MaxWidthofFieldSize,

	@MaxLeafSpeed,

	@MaxOvertravel,

	@BeamPenumbra,

	@MaxLeafLeakage,

	@MaxLeafTransmission,

	@IsIsocenterSize,

	@ClearancetoIsocenter,

	@LinacInterfaceHeight,

	@AlignmentRingThickness,

	@UpperLeafSpace,

	@InnerLeafSpace,

	@MLCLength,

	@MLCWidth,

	@MLCHeight,

	@LeafGap,

	@LeafTolerence,

	@MAXDVAFields,

	@InsidePenumbraFactor,

	@OutsidePenumbraFactor,

	@SoursetoMLCTopEdge,

	@MinLeafSpeed,

	@MaxLeafAcceleration,

	@MaxExtendCarriage,

	@LeafSepCorrectioncm,

	@TungeGrooveWidthcm,

	@MLCTrayFactor,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: MLC_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:57:59

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:57:59	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_MLC_DELETE

	 @ID	int

	,@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' DELETE FROM MLC

	where ID = @ID'

	EXECUTE  sp_executesql @sql,N'

	@DBName NVARCHAR(100),

	@ID	int',	

	@DBName,

	@ID

go

CREATE  PROCEDURE SP_MLC_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM MLC'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: SP_OrganList_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:58:34

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:58:34	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_OrganList_INSERT

	@AutoID	INT OUTPUT,

	@TPID	INT OUTPUT,

	@Type1	NVARCHAR(26),

	@Type2	NVARCHAR(32),

	@Name	NVARCHAR(30),

	@Color	NVARCHAR(24),

	@Paired	BIT,

	@Density	FLOAT,

	@Priority	INT,

	@CTLow	INT,

	@CTHigh	INT,

	@DisplayStyle	NVARCHAR(22),

	@DetectMethod	NVARCHAR(22),

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)

 AS

	IF(@TPID is Null)

	BEGIN

		SELECT @TPID=-1

	END

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO OrganList

	(

		TPID,

		Type1,

		Type2,

		Name,

		Color,

		Paired,

		Density,

		Priority,

		CTLow,

		CTHigh,

		DisplayStyle,

		DetectMethod

	)

	VALUES

	(

		@TPID,

		@Type1,

		@Type2,

		@Name,

		@Color,

		@Paired,

		@Density,

		@Priority,

		@CTLow,

		@CTHigh,

		@DisplayStyle,

		@DetectMethod

	)

	SET @AutoID=SCOPE_IDENTITY()

	SELECT @TPID= TPID,@UpdateTime = UpdateTime FROM OrganList WHERE AutoID=SCOPE_IDENTITY()

	IF (@TPID =-1)

	BEGIN

		SET @TPID=SCOPE_IDENTITY()

		UPDATE OrganList SET TPID=@TPID  WHERE AutoID=@TPID

	SELECT @UpdateTime = UpdateTime FROM OrganList WHERE AutoID=SCOPE_IDENTITY()

	END'

	EXECUTE  sp_executesql @sql,N'

	@AutoID	INT OUTPUT,

	@TPID	INT OUTPUT,

	@Type1	NVARCHAR(26),

	@Type2	NVARCHAR(32),

	@Name	NVARCHAR(30),

	@Color	NVARCHAR(24),

	@Paired	BIT,

	@Density	FLOAT,

	@Priority	INT,

	@CTLow	INT,

	@CTHigh	INT,

	@DisplayStyle	NVARCHAR(22),

	@DetectMethod	NVARCHAR(22),

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@AutoID OUTPUT,

	@TPID OUTPUT,

	@Type1,

	@Type2,

	@Name,

	@Color,

	@Paired,

	@Density,

	@Priority,

	@CTLow,

	@CTHigh,

	@DisplayStyle,

	@DetectMethod,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_OrganList_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:58:34

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:58:34	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_OrganList_UPDATE

	@TPID	INT,

	@Type1	NVARCHAR(26),

	@Type2	NVARCHAR(32),

	@Name	NVARCHAR(30),

	@Color	NVARCHAR(24),

	@Paired	BIT,

	@Density	FLOAT,

	@Priority	INT,

	@CTLow	INT,

	@CTHigh	INT,

	@DisplayStyle	NVARCHAR(22),

	@DetectMethod	NVARCHAR(22),

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)   ,

	@ApplyRowVersion BIT

 AS 

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' UPDATE OrganList SET

		TPID = @TPID,

		Type1 = @Type1,

		Type2 = @Type2,

		Name = @Name,

		Color = @Color,

		Paired = @Paired,

		Density = @Density,

		Priority = @Priority,

		CTLow = @CTLow,

		CTHigh = @CTHigh,

		DisplayStyle = @DisplayStyle,

		DetectMethod = @DetectMethod

		where TPID = @TPID AND UpdateTime=@UpdateTime

		SELECT @UpdateTime = UpdateTime FROM OrganList WHERE TPID=@TPID'

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' UPDATE OrganList SET

		TPID = @TPID,

		Type1 = @Type1,

		Type2 = @Type2,

		Name = @Name,

		Color = @Color,

		Paired = @Paired,

		Density = @Density,

		Priority = @Priority,

		CTLow = @CTLow,

		CTHigh = @CTHigh,

		DisplayStyle = @DisplayStyle,

		DetectMethod = @DetectMethod

		where TPID = @TPID

		SELECT @UpdateTime = UpdateTime FROM OrganList WHERE TPID=@TPID'

	END

	EXECUTE  sp_executesql @sql,N'

	@TPID	INT,

	@Type1	NVARCHAR(26),

	@Type2	NVARCHAR(32),

	@Name	NVARCHAR(30),

	@Color	NVARCHAR(24),

	@Paired	BIT,

	@Density	FLOAT,

	@Priority	INT,

	@CTLow	INT,

	@CTHigh	INT,

	@DisplayStyle	NVARCHAR(22),

	@DetectMethod	NVARCHAR(22),

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@TPID,

	@Type1,

	@Type2,

	@Name,

	@Color,

	@Paired,

	@Density,

	@Priority,

	@CTLow,

	@CTHigh,

	@DisplayStyle,

	@DetectMethod,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: OrganList_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:58:34

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:58:34	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_OrganList_DELETE

	 @TPID	int

	,@DBName NVARCHAR(100),

	@UpdateTime TIMESTAMP,

	@ApplyRowVersion BIT

AS

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' DELETE FROM OrganList

		where TPID = @TPID AND UpdateTime=@UpdateTime'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	int,

		@UpdateTime TIMESTAMP',	

		@DBName,

		@TPID,

		@UpdateTime

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' DELETE FROM OrganList

		where TPID = @TPID'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	int',	

		@DBName,

		@TPID

	END

go
/******************************************************************

* ÂêçÁß∞: SP_tbBeam_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:43:45

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:43:45	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbBeam_INSERT

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@PlanID	BIGINT,

	@Beam	VARCHAR(8),

	@BeamAlias	VARCHAR(20),

	@MachineRegisterID	INT,

	@Color	VARCHAR(16),

	@Weight	FLOAT,

	@BeamOn	BIT,

	@BeamModality	INT,

	@SSD	FLOAT,

	@CollimatorJawX1	FLOAT,

	@CollimatorJawX2	FLOAT,

	@CollimatorJawY1	FLOAT,

	@CollimatorJawY2	FLOAT,

	@BeamType	VARCHAR(11),

	@GantryStartAngle	FLOAT,

	@GantryEndAngle	FLOAT,

	@GantryAngleStep	FLOAT,

	@ARCDirection	VARCHAR(20),

	@CollimatorAngle	FLOAT,

	@IsocenterX	FLOAT,

	@IsocenterY	FLOAT,

	@IsocenterZ	FLOAT,

	@PresetPosition	IMAGE,

	@CouchHeight	FLOAT,

	@CouchLateral	FLOAT,

	@CouchLongitudinal	FLOAT,

	@TurntableAngle	FLOAT,

	@CouchAngle	FLOAT,

	@PedestalAngle	FLOAT,

	@WedgeorConeName	VARCHAR(20),

	@WedgeAngle	VARCHAR(10),

	@WedgeOrientation	VARCHAR(10),

	@WeightMode	VARCHAR(20),

	@WeightPointW	FLOAT,

	@WeightPointL	FLOAT,

	@WeightPointH	FLOAT,

	@WeightPointDose	FLOAT,

	@BeamTargetName	VARCHAR(20),

	@ShareIsoFlag	VARCHAR(12),

	@FrameIsoW	FLOAT,

	@FrameIsoL	FLOAT,

	@FrameIsoH	FLOAT,

	@IMRTBeamIntensity	IMAGE,

	@BeamIntensityny	INT,

	@BeamIntensitynx	INT,

	@NormIntensityValue	FLOAT,

	@MuTime	FLOAT,

	@MuTimeUnits	VARCHAR(4),

	@BEVIsoCenterXcm	FLOAT,

	@BEVIsoCenterYcm	FLOAT,

	@BEVPixelSizecm	FLOAT,

	@BEVImageSize	INT,

	@BEVImage	IMAGE,

	@BolusName	VARCHAR(15),

	@BolusThicknesscm	FLOAT,

	@IMRTDeliverableNx	INT,

	@IMRTDeliverableNy	INT,

	@IMRTDeliverableDx	FLOAT,

	@IMRTDeliverableDy	FLOAT,

	@IMRTDeliverableMap	IMAGE,

	@BeamNormPointDose	FLOAT,

	@Collision	VARCHAR(15),

	@DrrValue	IMAGE,

	@BeamIntensityDx	IMAGE,

	@BeamIntensityDy	IMAGE,

	@BeamProjection	IMAGE,

	@WedgeDoseRatio	FLOAT,

	@WedgeFactor	FLOAT,

	@CalcPtSSD	FLOAT,

	@CalcPtPhyDepth	FLOAT,

	@CalcPtEffDepth	FLOAT,

	@OpenFldEQSQ	FLOAT,

	@BlkedFldEQSQ	FLOAT,

	@CalcPtTMR	FLOAT,

	@BlkedFldSp	FLOAT,

	@OpenFldSc	FLOAT,

	@CalcPtOAR	FLOAT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)

 AS

	SET TRANSACTION ISOLATION LEVEL SERIALIZABLE

	IF(@TPID is Null)

	BEGIN

		SELECT @TPID=-1

	END

	DECLARE @Sql NVARCHAR(MAX) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' BEGIN TRANSACTION  

	INSERT INTO tbBeam

	(

		TPID,

		StudyID,

		Course,

		PlanID,

		Beam,

		BeamAlias,

		MachineRegisterID,

		Color,

		Weight,

		BeamOn,

		BeamModality,

		SSD,

		CollimatorJawX1,

		CollimatorJawX2,

		CollimatorJawY1,

		CollimatorJawY2,

		BeamType,

		GantryStartAngle,

		GantryEndAngle,

		GantryAngleStep,

		ARCDirection,

		CollimatorAngle,

		IsocenterX,

		IsocenterY,

		IsocenterZ,

		PresetPosition,

		CouchHeight,

		CouchLateral,

		CouchLongitudinal,

		TurntableAngle,

		CouchAngle,

		PedestalAngle,

		WedgeorConeName,

		WedgeAngle,

		WedgeOrientation,

		WeightMode,

		WeightPointW,

		WeightPointL,

		WeightPointH,

		WeightPointDose,

		BeamTargetName,

		ShareIsoFlag,

		FrameIsoW,

		FrameIsoL,

		FrameIsoH,

		IMRTBeamIntensity,

		BeamIntensityny,

		BeamIntensitynx,

		NormIntensityValue,

		MuTime,

		MuTimeUnits,

		BEVIsoCenterXcm,

		BEVIsoCenterYcm,

		BEVPixelSizecm,

		BEVImageSize,

		BEVImage,

		BolusName,

		BolusThicknesscm,

		IMRTDeliverableNx,

		IMRTDeliverableNy,

		IMRTDeliverableDx,

		IMRTDeliverableDy,

		IMRTDeliverableMap,

		BeamNormPointDose,

		Collision,

		DrrValue,

		BeamIntensityDx,

		BeamIntensityDy,

		BeamProjection,

		WedgeDoseRatio,

		WedgeFactor,

		CalcPtSSD,

		CalcPtPhyDepth,

		CalcPtEffDepth,

		OpenFldEQSQ,

		BlkedFldEQSQ,

		CalcPtTMR,

		BlkedFldSp,

		OpenFldSc,

		CalcPtOAR

	)

	VALUES

	(

		@TPID,

		@StudyID,

		@Course,

		@PlanID,

		@Beam,

		@BeamAlias,

		@MachineRegisterID,

		@Color,

		@Weight,

		@BeamOn,

		@BeamModality,

		@SSD,

		@CollimatorJawX1,

		@CollimatorJawX2,

		@CollimatorJawY1,

		@CollimatorJawY2,

		@BeamType,

		@GantryStartAngle,

		@GantryEndAngle,

		@GantryAngleStep,

		@ARCDirection,

		@CollimatorAngle,

		@IsocenterX,

		@IsocenterY,

		@IsocenterZ,

		@PresetPosition,

		@CouchHeight,

		@CouchLateral,

		@CouchLongitudinal,

		@TurntableAngle,

		@CouchAngle,

		@PedestalAngle,

		@WedgeorConeName,

		@WedgeAngle,

		@WedgeOrientation,

		@WeightMode,

		@WeightPointW,

		@WeightPointL,

		@WeightPointH,

		@WeightPointDose,

		@BeamTargetName,

		@ShareIsoFlag,

		@FrameIsoW,

		@FrameIsoL,

		@FrameIsoH,

		@IMRTBeamIntensity,

		@BeamIntensityny,

		@BeamIntensitynx,

		@NormIntensityValue,

		@MuTime,

		@MuTimeUnits,

		@BEVIsoCenterXcm,

		@BEVIsoCenterYcm,

		@BEVPixelSizecm,

		@BEVImageSize,

		@BEVImage,

		@BolusName,

		@BolusThicknesscm,

		@IMRTDeliverableNx,

		@IMRTDeliverableNy,

		@IMRTDeliverableDx,

		@IMRTDeliverableDy,

		@IMRTDeliverableMap,

		@BeamNormPointDose,

		@Collision,

		@DrrValue,

		@BeamIntensityDx,

		@BeamIntensityDy,

		@BeamProjection,

		@WedgeDoseRatio,

		@WedgeFactor,

		@CalcPtSSD,

		@CalcPtPhyDepth,

		@CalcPtEffDepth,

		@OpenFldEQSQ,

		@BlkedFldEQSQ,

		@CalcPtTMR,

		@BlkedFldSp,

		@OpenFldSc,

		@CalcPtOAR

	)

	SET @AutoID=SCOPE_IDENTITY()

	SELECT @TPID= TPID,@UpdateTime = UpdateTime FROM tbBeam WHERE AutoID=SCOPE_IDENTITY()

	IF (@TPID =-1)

	BEGIN

		SET @TPID=SCOPE_IDENTITY()

		UPDATE tbBeam SET TPID=@TPID  WHERE AutoID=@TPID

	SELECT @UpdateTime = UpdateTime FROM tbBeam WHERE AutoID=SCOPE_IDENTITY()

	END 

	IF (@@error <> 0)

	BEGIN

		ROLLBACK TRANSACTION

	END	

	ELSE

	BEGIN

	COMMIT TRANSACTION

	END'

	EXECUTE  sp_executesql @sql,N'

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@StudyID	BIGINT ,

	@Course	VARCHAR(10),

	@PlanID	BIGINT ,

	@Beam	VARCHAR(8),

	@BeamAlias	VARCHAR(20),

	@MachineRegisterID	INT,

	@Color	VARCHAR(16),

	@Weight	FLOAT,

	@BeamOn	BIT,

	@BeamModality	INT,

	@SSD	FLOAT,

	@CollimatorJawX1	FLOAT,

	@CollimatorJawX2	FLOAT,

	@CollimatorJawY1	FLOAT,

	@CollimatorJawY2	FLOAT,

	@BeamType	VARCHAR(11),

	@GantryStartAngle	FLOAT,

	@GantryEndAngle	FLOAT,

	@GantryAngleStep	FLOAT,

	@ARCDirection	VARCHAR(20),

	@CollimatorAngle	FLOAT,

	@IsocenterX	FLOAT,

	@IsocenterY	FLOAT,

	@IsocenterZ	FLOAT,

	@PresetPosition	IMAGE,

	@CouchHeight	FLOAT,

	@CouchLateral	FLOAT,

	@CouchLongitudinal	FLOAT,

	@TurntableAngle	FLOAT,

	@CouchAngle	FLOAT,

	@PedestalAngle	FLOAT,

	@WedgeorConeName	VARCHAR(20),

	@WedgeAngle	VARCHAR(10),

	@WedgeOrientation	VARCHAR(10),

	@WeightMode	VARCHAR(20),

	@WeightPointW	FLOAT,

	@WeightPointL	FLOAT,

	@WeightPointH	FLOAT,

	@WeightPointDose	FLOAT,

	@BeamTargetName	VARCHAR(20),

	@ShareIsoFlag	VARCHAR(12),

	@FrameIsoW	FLOAT,

	@FrameIsoL	FLOAT,

	@FrameIsoH	FLOAT,

	@IMRTBeamIntensity	IMAGE,

	@BeamIntensityny	INT,

	@BeamIntensitynx	INT,

	@NormIntensityValue	FLOAT,

	@MuTime	FLOAT,

	@MuTimeUnits	VARCHAR(4),

	@BEVIsoCenterXcm	FLOAT,

	@BEVIsoCenterYcm	FLOAT,

	@BEVPixelSizecm	FLOAT,

	@BEVImageSize	INT,

	@BEVImage	IMAGE,

	@BolusName	VARCHAR(15),

	@BolusThicknesscm	FLOAT,

	@IMRTDeliverableNx	INT,

	@IMRTDeliverableNy	INT,

	@IMRTDeliverableDx	FLOAT,

	@IMRTDeliverableDy	FLOAT,

	@IMRTDeliverableMap	IMAGE,

	@BeamNormPointDose	FLOAT,

	@Collision	VARCHAR(15),

	@DrrValue	IMAGE,

	@BeamIntensityDx	IMAGE,

	@BeamIntensityDy	IMAGE,

	@BeamProjection	IMAGE,

	@WedgeDoseRatio	FLOAT,

	@WedgeFactor	FLOAT,

	@CalcPtSSD	FLOAT,

	@CalcPtPhyDepth	FLOAT,

	@CalcPtEffDepth	FLOAT,

	@OpenFldEQSQ	FLOAT,

	@BlkedFldEQSQ	FLOAT,

	@CalcPtTMR	FLOAT,

	@BlkedFldSp	FLOAT,

	@OpenFldSc	FLOAT,

	@CalcPtOAR	FLOAT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@AutoID OUTPUT,

	@TPID OUTPUT,

	@StudyID,

	@Course,

	@PlanID,

	@Beam,

	@BeamAlias,

	@MachineRegisterID,

	@Color,

	@Weight,

	@BeamOn,

	@BeamModality,

	@SSD,

	@CollimatorJawX1,

	@CollimatorJawX2,

	@CollimatorJawY1,

	@CollimatorJawY2,

	@BeamType,

	@GantryStartAngle,

	@GantryEndAngle,

	@GantryAngleStep,

	@ARCDirection,

	@CollimatorAngle,

	@IsocenterX,

	@IsocenterY,

	@IsocenterZ,

	@PresetPosition,

	@CouchHeight,

	@CouchLateral,

	@CouchLongitudinal,

	@TurntableAngle,

	@CouchAngle,

	@PedestalAngle,

	@WedgeorConeName,

	@WedgeAngle,

	@WedgeOrientation,

	@WeightMode,

	@WeightPointW,

	@WeightPointL,

	@WeightPointH,

	@WeightPointDose,

	@BeamTargetName,

	@ShareIsoFlag,

	@FrameIsoW,

	@FrameIsoL,

	@FrameIsoH,

	@IMRTBeamIntensity,

	@BeamIntensityny,

	@BeamIntensitynx,

	@NormIntensityValue,

	@MuTime,

	@MuTimeUnits,

	@BEVIsoCenterXcm,

	@BEVIsoCenterYcm,

	@BEVPixelSizecm,

	@BEVImageSize,

	@BEVImage,

	@BolusName,

	@BolusThicknesscm,

	@IMRTDeliverableNx,

	@IMRTDeliverableNy,

	@IMRTDeliverableDx,

	@IMRTDeliverableDy,

	@IMRTDeliverableMap,

	@BeamNormPointDose,

	@Collision,

	@DrrValue,

	@BeamIntensityDx,

	@BeamIntensityDy,

	@BeamProjection,

	@WedgeDoseRatio,

	@WedgeFactor,

	@CalcPtSSD,

	@CalcPtPhyDepth,

	@CalcPtEffDepth,

	@OpenFldEQSQ,

	@BlkedFldEQSQ,

	@CalcPtTMR,

	@BlkedFldSp,

	@OpenFldSc,

	@CalcPtOAR,

	@UpdateTime OUTPUT,

	@DBName   

go

CREATE  PROCEDURE SP_OrganList_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM OrganList'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: SP_tbBeam_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:43:45

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:43:45	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbBeam_UPDATE

	@TPID	BIGINT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@PlanID	BIGINT,

	@Beam	VARCHAR(8),

	@BeamAlias	VARCHAR(20),

	@MachineRegisterID	INT,

	@Color	VARCHAR(16),

	@Weight	FLOAT,

	@BeamOn	BIT,

	@BeamModality	INT,

	@SSD	FLOAT,

	@CollimatorJawX1	FLOAT,

	@CollimatorJawX2	FLOAT,

	@CollimatorJawY1	FLOAT,

	@CollimatorJawY2	FLOAT,

	@BeamType	VARCHAR(11),

	@GantryStartAngle	FLOAT,

	@GantryEndAngle	FLOAT,

	@GantryAngleStep	FLOAT,

	@ARCDirection	VARCHAR(20),

	@CollimatorAngle	FLOAT,

	@IsocenterX	FLOAT,

	@IsocenterY	FLOAT,

	@IsocenterZ	FLOAT,

	@PresetPosition	IMAGE,

	@CouchHeight	FLOAT,

	@CouchLateral	FLOAT,

	@CouchLongitudinal	FLOAT,

	@TurntableAngle	FLOAT,

	@CouchAngle	FLOAT,

	@PedestalAngle	FLOAT,

	@WedgeorConeName	VARCHAR(20),

	@WedgeAngle	VARCHAR(10),

	@WedgeOrientation	VARCHAR(10),

	@WeightMode	VARCHAR(20),

	@WeightPointW	FLOAT,

	@WeightPointL	FLOAT,

	@WeightPointH	FLOAT,

	@WeightPointDose	FLOAT,

	@BeamTargetName	VARCHAR(20),

	@ShareIsoFlag	VARCHAR(12),

	@FrameIsoW	FLOAT,

	@FrameIsoL	FLOAT,

	@FrameIsoH	FLOAT,

	@IMRTBeamIntensity	IMAGE,

	@BeamIntensityny	INT,

	@BeamIntensitynx	INT,

	@NormIntensityValue	FLOAT,

	@MuTime	FLOAT,

	@MuTimeUnits	VARCHAR(4),

	@BEVIsoCenterXcm	FLOAT,

	@BEVIsoCenterYcm	FLOAT,

	@BEVPixelSizecm	FLOAT,

	@BEVImageSize	INT,

	@BEVImage	IMAGE,

	@BolusName	VARCHAR(15),

	@BolusThicknesscm	FLOAT,

	@IMRTDeliverableNx	INT,

	@IMRTDeliverableNy	INT,

	@IMRTDeliverableDx	FLOAT,

	@IMRTDeliverableDy	FLOAT,

	@IMRTDeliverableMap	IMAGE,

	@BeamNormPointDose	FLOAT,

	@Collision	VARCHAR(15),

	@DrrValue	IMAGE,

	@BeamIntensityDx	IMAGE,

	@BeamIntensityDy	IMAGE,

	@BeamProjection	IMAGE,

	@WedgeDoseRatio	FLOAT,

	@WedgeFactor	FLOAT,

	@CalcPtSSD	FLOAT,

	@CalcPtPhyDepth	FLOAT,

	@CalcPtEffDepth	FLOAT,

	@OpenFldEQSQ	FLOAT,

	@BlkedFldEQSQ	FLOAT,

	@CalcPtTMR	FLOAT,

	@BlkedFldSp	FLOAT,

	@OpenFldSc	FLOAT,

	@CalcPtOAR	FLOAT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)   ,

	@ApplyRowVersion BIT

 AS 

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(MAX) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' UPDATE tbBeam SET

		TPID = @TPID,

		StudyID = @StudyID,

		Course = @Course,

		PlanID = @PlanID,

		Beam = @Beam,

		BeamAlias = @BeamAlias,

		MachineRegisterID = @MachineRegisterID,

		Color = @Color,

		Weight = @Weight,

		BeamOn = @BeamOn,

		BeamModality = @BeamModality,

		SSD = @SSD,

		CollimatorJawX1 = @CollimatorJawX1,

		CollimatorJawX2 = @CollimatorJawX2,

		CollimatorJawY1 = @CollimatorJawY1,

		CollimatorJawY2 = @CollimatorJawY2,

		BeamType = @BeamType,

		GantryStartAngle = @GantryStartAngle,

		GantryEndAngle = @GantryEndAngle,

		GantryAngleStep = @GantryAngleStep,

		ARCDirection = @ARCDirection,

		CollimatorAngle = @CollimatorAngle,

		IsocenterX = @IsocenterX,

		IsocenterY = @IsocenterY,

		IsocenterZ = @IsocenterZ,

		PresetPosition = @PresetPosition,

		CouchHeight = @CouchHeight,

		CouchLateral = @CouchLateral,

		CouchLongitudinal = @CouchLongitudinal,

		TurntableAngle = @TurntableAngle,

		CouchAngle = @CouchAngle,

		PedestalAngle = @PedestalAngle,

		WedgeorConeName = @WedgeorConeName,

		WedgeAngle = @WedgeAngle,

		WedgeOrientation = @WedgeOrientation,

		WeightMode = @WeightMode,

		WeightPointW = @WeightPointW,

		WeightPointL = @WeightPointL,

		WeightPointH = @WeightPointH,

		WeightPointDose = @WeightPointDose,

		BeamTargetName = @BeamTargetName,

		ShareIsoFlag = @ShareIsoFlag,

		FrameIsoW = @FrameIsoW,

		FrameIsoL = @FrameIsoL,

		FrameIsoH = @FrameIsoH,

		IMRTBeamIntensity = @IMRTBeamIntensity,

		BeamIntensityny = @BeamIntensityny,

		BeamIntensitynx = @BeamIntensitynx,

		NormIntensityValue = @NormIntensityValue,

		MuTime = @MuTime,

		MuTimeUnits = @MuTimeUnits,

		BEVIsoCenterXcm = @BEVIsoCenterXcm,

		BEVIsoCenterYcm = @BEVIsoCenterYcm,

		BEVPixelSizecm = @BEVPixelSizecm,

		BEVImageSize = @BEVImageSize,

		BEVImage = @BEVImage,

		BolusName = @BolusName,

		BolusThicknesscm = @BolusThicknesscm,

		IMRTDeliverableNx = @IMRTDeliverableNx,

		IMRTDeliverableNy = @IMRTDeliverableNy,

		IMRTDeliverableDx = @IMRTDeliverableDx,

		IMRTDeliverableDy = @IMRTDeliverableDy,

		IMRTDeliverableMap = @IMRTDeliverableMap,

		BeamNormPointDose = @BeamNormPointDose,

		Collision = @Collision,

		DrrValue = @DrrValue,

		BeamIntensityDx = @BeamIntensityDx,

		BeamIntensityDy = @BeamIntensityDy,

		BeamProjection = @BeamProjection,

		WedgeDoseRatio = @WedgeDoseRatio,

		WedgeFactor = @WedgeFactor,

		CalcPtSSD = @CalcPtSSD,

		CalcPtPhyDepth = @CalcPtPhyDepth,

		CalcPtEffDepth = @CalcPtEffDepth,

		OpenFldEQSQ = @OpenFldEQSQ,

		BlkedFldEQSQ = @BlkedFldEQSQ,

		CalcPtTMR = @CalcPtTMR,

		BlkedFldSp = @BlkedFldSp,

		OpenFldSc = @OpenFldSc,

		CalcPtOAR = @CalcPtOAR

		where TPID = @TPID AND UpdateTime=@UpdateTime

		SELECT @UpdateTime = UpdateTime FROM tbBeam WHERE TPID=@TPID'

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' UPDATE tbBeam SET

		TPID = @TPID,

		StudyID = @StudyID,

		Course = @Course,

		PlanID = @PlanID,

		Beam = @Beam,

		BeamAlias = @BeamAlias,

		MachineRegisterID = @MachineRegisterID,

		Color = @Color,

		Weight = @Weight,

		BeamOn = @BeamOn,

		BeamModality = @BeamModality,

		SSD = @SSD,

		CollimatorJawX1 = @CollimatorJawX1,

		CollimatorJawX2 = @CollimatorJawX2,

		CollimatorJawY1 = @CollimatorJawY1,

		CollimatorJawY2 = @CollimatorJawY2,

		BeamType = @BeamType,

		GantryStartAngle = @GantryStartAngle,

		GantryEndAngle = @GantryEndAngle,

		GantryAngleStep = @GantryAngleStep,

		ARCDirection = @ARCDirection,

		CollimatorAngle = @CollimatorAngle,

		IsocenterX = @IsocenterX,

		IsocenterY = @IsocenterY,

		IsocenterZ = @IsocenterZ,

		PresetPosition = @PresetPosition,

		CouchHeight = @CouchHeight,

		CouchLateral = @CouchLateral,

		CouchLongitudinal = @CouchLongitudinal,

		TurntableAngle = @TurntableAngle,

		CouchAngle = @CouchAngle,

		PedestalAngle = @PedestalAngle,

		WedgeorConeName = @WedgeorConeName,

		WedgeAngle = @WedgeAngle,

		WedgeOrientation = @WedgeOrientation,

		WeightMode = @WeightMode,

		WeightPointW = @WeightPointW,

		WeightPointL = @WeightPointL,

		WeightPointH = @WeightPointH,

		WeightPointDose = @WeightPointDose,

		BeamTargetName = @BeamTargetName,

		ShareIsoFlag = @ShareIsoFlag,

		FrameIsoW = @FrameIsoW,

		FrameIsoL = @FrameIsoL,

		FrameIsoH = @FrameIsoH,

		IMRTBeamIntensity = @IMRTBeamIntensity,

		BeamIntensityny = @BeamIntensityny,

		BeamIntensitynx = @BeamIntensitynx,

		NormIntensityValue = @NormIntensityValue,

		MuTime = @MuTime,

		MuTimeUnits = @MuTimeUnits,

		BEVIsoCenterXcm = @BEVIsoCenterXcm,

		BEVIsoCenterYcm = @BEVIsoCenterYcm,

		BEVPixelSizecm = @BEVPixelSizecm,

		BEVImageSize = @BEVImageSize,

		BEVImage = @BEVImage,

		BolusName = @BolusName,

		BolusThicknesscm = @BolusThicknesscm,

		IMRTDeliverableNx = @IMRTDeliverableNx,

		IMRTDeliverableNy = @IMRTDeliverableNy,

		IMRTDeliverableDx = @IMRTDeliverableDx,

		IMRTDeliverableDy = @IMRTDeliverableDy,

		IMRTDeliverableMap = @IMRTDeliverableMap,

		BeamNormPointDose = @BeamNormPointDose,

		Collision = @Collision,

		DrrValue = @DrrValue,

		BeamIntensityDx = @BeamIntensityDx,

		BeamIntensityDy = @BeamIntensityDy,

		BeamProjection = @BeamProjection,

		WedgeDoseRatio = @WedgeDoseRatio,

		WedgeFactor = @WedgeFactor,

		CalcPtSSD = @CalcPtSSD,

		CalcPtPhyDepth = @CalcPtPhyDepth,

		CalcPtEffDepth = @CalcPtEffDepth,

		OpenFldEQSQ = @OpenFldEQSQ,

		BlkedFldEQSQ = @BlkedFldEQSQ,

		CalcPtTMR = @CalcPtTMR,

		BlkedFldSp = @BlkedFldSp,

		OpenFldSc = @OpenFldSc,

		CalcPtOAR = @CalcPtOAR

		where TPID = @TPID

		SELECT @UpdateTime = UpdateTime FROM tbBeam WHERE TPID=@TPID'

	END

	EXECUTE  sp_executesql @sql,N'

	@TPID	BIGINT ,

	@StudyID	BIGINT ,

	@Course	VARCHAR(10),

	@PlanID	BIGINT ,

	@Beam	VARCHAR(8),

	@BeamAlias	VARCHAR(20),

	@MachineRegisterID	INT,

	@Color	VARCHAR(16),

	@Weight	FLOAT,

	@BeamOn	BIT,

	@BeamModality	INT,

	@SSD	FLOAT,

	@CollimatorJawX1	FLOAT,

	@CollimatorJawX2	FLOAT,

	@CollimatorJawY1	FLOAT,

	@CollimatorJawY2	FLOAT,

	@BeamType	VARCHAR(11),

	@GantryStartAngle	FLOAT,

	@GantryEndAngle	FLOAT,

	@GantryAngleStep	FLOAT,

	@ARCDirection	VARCHAR(20),

	@CollimatorAngle	FLOAT,

	@IsocenterX	FLOAT,

	@IsocenterY	FLOAT,

	@IsocenterZ	FLOAT,

	@PresetPosition	IMAGE,

	@CouchHeight	FLOAT,

	@CouchLateral	FLOAT,

	@CouchLongitudinal	FLOAT,

	@TurntableAngle	FLOAT,

	@CouchAngle	FLOAT,

	@PedestalAngle	FLOAT,

	@WedgeorConeName	VARCHAR(20),

	@WedgeAngle	VARCHAR(10),

	@WedgeOrientation	VARCHAR(10),

	@WeightMode	VARCHAR(20),

	@WeightPointW	FLOAT,

	@WeightPointL	FLOAT,

	@WeightPointH	FLOAT,

	@WeightPointDose	FLOAT,

	@BeamTargetName	VARCHAR(20),

	@ShareIsoFlag	VARCHAR(12),

	@FrameIsoW	FLOAT,

	@FrameIsoL	FLOAT,

	@FrameIsoH	FLOAT,

	@IMRTBeamIntensity	IMAGE,

	@BeamIntensityny	INT,

	@BeamIntensitynx	INT,

	@NormIntensityValue	FLOAT,

	@MuTime	FLOAT,

	@MuTimeUnits	VARCHAR(4),

	@BEVIsoCenterXcm	FLOAT,

	@BEVIsoCenterYcm	FLOAT,

	@BEVPixelSizecm	FLOAT,

	@BEVImageSize	INT,

	@BEVImage	IMAGE,

	@BolusName	VARCHAR(15),

	@BolusThicknesscm	FLOAT,

	@IMRTDeliverableNx	INT,

	@IMRTDeliverableNy	INT,

	@IMRTDeliverableDx	FLOAT,

	@IMRTDeliverableDy	FLOAT,

	@IMRTDeliverableMap	IMAGE,

	@BeamNormPointDose	FLOAT,

	@Collision	VARCHAR(15),

	@DrrValue	IMAGE,

	@BeamIntensityDx	IMAGE,

	@BeamIntensityDy	IMAGE,

	@BeamProjection	IMAGE,

	@WedgeDoseRatio	FLOAT,

	@WedgeFactor	FLOAT,

	@CalcPtSSD	FLOAT,

	@CalcPtPhyDepth	FLOAT,

	@CalcPtEffDepth	FLOAT,

	@OpenFldEQSQ	FLOAT,

	@BlkedFldEQSQ	FLOAT,

	@CalcPtTMR	FLOAT,

	@BlkedFldSp	FLOAT,

	@OpenFldSc	FLOAT,

	@CalcPtOAR	FLOAT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@TPID,

	@StudyID,

	@Course,

	@PlanID,

	@Beam,

	@BeamAlias,

	@MachineRegisterID,

	@Color,

	@Weight,

	@BeamOn,

	@BeamModality,

	@SSD,

	@CollimatorJawX1,

	@CollimatorJawX2,

	@CollimatorJawY1,

	@CollimatorJawY2,

	@BeamType,

	@GantryStartAngle,

	@GantryEndAngle,

	@GantryAngleStep,

	@ARCDirection,

	@CollimatorAngle,

	@IsocenterX,

	@IsocenterY,

	@IsocenterZ,

	@PresetPosition,

	@CouchHeight,

	@CouchLateral,

	@CouchLongitudinal,

	@TurntableAngle,

	@CouchAngle,

	@PedestalAngle,

	@WedgeorConeName,

	@WedgeAngle,

	@WedgeOrientation,

	@WeightMode,

	@WeightPointW,

	@WeightPointL,

	@WeightPointH,

	@WeightPointDose,

	@BeamTargetName,

	@ShareIsoFlag,

	@FrameIsoW,

	@FrameIsoL,

	@FrameIsoH,

	@IMRTBeamIntensity,

	@BeamIntensityny,

	@BeamIntensitynx,

	@NormIntensityValue,

	@MuTime,

	@MuTimeUnits,

	@BEVIsoCenterXcm,

	@BEVIsoCenterYcm,

	@BEVPixelSizecm,

	@BEVImageSize,

	@BEVImage,

	@BolusName,

	@BolusThicknesscm,

	@IMRTDeliverableNx,

	@IMRTDeliverableNy,

	@IMRTDeliverableDx,

	@IMRTDeliverableDy,

	@IMRTDeliverableMap,

	@BeamNormPointDose,

	@Collision,

	@DrrValue,

	@BeamIntensityDx,

	@BeamIntensityDy,

	@BeamProjection,

	@WedgeDoseRatio,

	@WedgeFactor,

	@CalcPtSSD,

	@CalcPtPhyDepth,

	@CalcPtEffDepth,

	@OpenFldEQSQ,

	@BlkedFldEQSQ,

	@CalcPtTMR,

	@BlkedFldSp,

	@OpenFldSc,

	@CalcPtOAR,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_Patient_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:58:41

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:58:41	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE [dbo].[SP_Patient_INSERT]

	@AutoID	INT OUTPUT,

	@TPID	INT OUTPUT,

	@PatientID	VARCHAR(20),

	@Name	VARCHAR(22),

	@FirstName	VARCHAR(22),

	@MiddleName	VARCHAR(22),

	@Sex	VARCHAR(6),

	@Age	INT,

	@Room	VARCHAR(10),

	@Bed	INT,

	@RegisterDate	DATETIME,

	@DoctorName	VARCHAR(22),

	@BirthDate	DATETIME,

	@Note	VARCHAR(40),

	@PDMLock	BIT,

	@PLNLock	BIT,

	@PatientPhoto	IMAGE,

	@Completed	BIT,

	@DownLoad	INT,

	@DoctorForDownLoad	VARCHAR(20),

	@Deleted	BIT,

	@LogRecord	IMAGE,

	@NetPassword	VARCHAR(20),

	@IsWorkNormal	BIT,

	@Compressed	BIT,

	@UserLocked VARCHAR(50),

	@Type NVARCHAR(50),

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)

 AS

	IF(@TPID is Null)

	BEGIN

		SELECT @TPID=-1

	END

	DECLARE @Sql NVARCHAR(2000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO Patient

	(

		TPID,

		PatientID,

		Name,

		FirstName,

		MiddleName,

		Sex,

		Age,

		Room,

		Bed,

		RegisterDate,

		DoctorName,

		BirthDate,

		Note,

		PDMLock,

		PLNLock,

		PatientPhoto,

		Completed,

		DownLoad,

		DoctorForDownLoad,

		Deleted,

		LogRecord,

		NetPassword,

		IsWorkNormal,

		Compressed,

		UserLocked,

		Type

	)

	VALUES

	(

		@TPID,

		@PatientID,

		@Name,

		@FirstName,

		@MiddleName,

		@Sex,

		@Age,

		@Room,

		@Bed,

		@RegisterDate,

		@DoctorName,

		@BirthDate,

		@Note,

		@PDMLock,

		@PLNLock,

		@PatientPhoto,

		@Completed,

		@DownLoad,

		@DoctorForDownLoad,

		@Deleted,

		@LogRecord,

		@NetPassword,

		@IsWorkNormal,

		@Compressed,

		@UserLocked,

		@Type

	)

	SET @AutoID=SCOPE_IDENTITY()

	SELECT @TPID= TPID,@UpdateTime = UpdateTime FROM Patient WHERE AutoID=SCOPE_IDENTITY()

	IF (@TPID =-1)

	BEGIN

		SET @TPID=SCOPE_IDENTITY()

		UPDATE Patient SET TPID=@TPID  WHERE AutoID=@TPID

	SELECT @UpdateTime = UpdateTime FROM Patient WHERE AutoID=SCOPE_IDENTITY()

	END'

	EXECUTE  sp_executesql @sql,N'

	@AutoID	INT OUTPUT,

	@TPID	INT OUTPUT,

	@PatientID	VARCHAR(20),

	@Name	VARCHAR(22),

	@FirstName	VARCHAR(22),

	@MiddleName	VARCHAR(22),

	@Sex	VARCHAR(6),

	@Age	INT,

	@Room	VARCHAR(10),

	@Bed	INT,

	@RegisterDate	DATETIME,

	@DoctorName	VARCHAR(22),

	@BirthDate	DATETIME,

	@Note	VARCHAR(40),

	@PDMLock	BIT,

	@PLNLock	BIT,

	@PatientPhoto	IMAGE,

	@Completed	BIT,

	@DownLoad	INT,

	@DoctorForDownLoad	VARCHAR(20),

	@Deleted	BIT,

	@LogRecord	IMAGE,

	@NetPassword	VARCHAR(20),

	@IsWorkNormal	BIT,

	@Compressed	BIT,

	@UserLocked VARCHAR(50),

	@Type NVARCHAR(50),

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@AutoID OUTPUT,

	@TPID OUTPUT,

	@PatientID,

	@Name,

	@FirstName,

	@MiddleName,

	@Sex,

	@Age,

	@Room,

	@Bed,

	@RegisterDate,

	@DoctorName,

	@BirthDate,

	@Note,

	@PDMLock,

	@PLNLock,

	@PatientPhoto,

	@Completed,

	@DownLoad,

	@DoctorForDownLoad,

	@Deleted,

	@LogRecord,

	@NetPassword,

	@IsWorkNormal,

	@Compressed,

	@UserLocked,

	@Type,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: tbBeam_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:43:45

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:43:45	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbBeam_DELETE

	 @TPID	BIGINT

	,@DBName NVARCHAR(100),

	@UpdateTime TIMESTAMP,

	@ApplyRowVersion BIT

AS

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbBeam

		where TPID = @TPID AND UpdateTime=@UpdateTime'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ,

		@UpdateTime TIMESTAMP',	

		@DBName,

		@TPID,

		@UpdateTime

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbBeam

		where TPID = @TPID'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ',	

		@DBName,

		@TPID

	END

go
/******************************************************************

* ÂêçÁß∞: SP_tbBEV_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:44:17

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:44:17	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbBEV_INSERT

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@PlanID	BIGINT,

	@BeamID	BIGINT,

	@OrganName	VARCHAR(18),

	@OrganBEVOutline	IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)

 AS

	IF(@TPID is Null)

	BEGIN

		SELECT @TPID=-1

	END

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO tbBEV

	(

		TPID,

		StudyID,

		Course,

		[PlanID],

		BeamID,

		OrganName,

		OrganBEVOutline

	)

	VALUES

	(

		@TPID,

		@StudyID,

		@Course,

		@PlanID,

		@BeamID,

		@OrganName,

		@OrganBEVOutline

	)

	SET @AutoID=SCOPE_IDENTITY()

	SELECT @TPID= TPID,@UpdateTime = UpdateTime FROM tbBEV WHERE AutoID=SCOPE_IDENTITY()

	IF (@TPID =-1)

	BEGIN

		SET @TPID=SCOPE_IDENTITY()

		UPDATE tbBEV SET TPID=@TPID  WHERE AutoID=@TPID

	SELECT @UpdateTime = UpdateTime FROM tbBEV WHERE AutoID=SCOPE_IDENTITY()

	END'

	EXECUTE  sp_executesql @sql,N'

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@StudyID	BIGINT ,

	@Course	VARCHAR(10),

	@PlanID	BIGINT ,

	@BeamID	BIGINT ,

	@OrganName	VARCHAR(18),

	@OrganBEVOutline	IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@AutoID OUTPUT,

	@TPID OUTPUT,

	@StudyID,

	@Course,

	@PlanID,

	@BeamID,

	@OrganName,

	@OrganBEVOutline,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: Patient_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:58:41

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:58:41	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_Patient_DELETE

	 @TPID	int

	,@DBName NVARCHAR(100),

	@UpdateTime TIMESTAMP,

	@ApplyRowVersion BIT

AS

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' DELETE FROM Patient

		where TPID = @TPID AND UpdateTime=@UpdateTime'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	int,

		@UpdateTime TIMESTAMP',	

		@DBName,

		@TPID,

		@UpdateTime

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' DELETE FROM Patient

		where TPID = @TPID'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	int',	

		@DBName,

		@TPID

	END

go
/******************************************************************

* ÂêçÁß∞: SP_Patient_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:58:41

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:58:41	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE [dbo].[SP_Patient_UPDATE]

	@TPID	INT,

	@PatientID	VARCHAR(20),

	@Name	VARCHAR(22),

	@FirstName	VARCHAR(22),

	@MiddleName	VARCHAR(22),

	@Sex	VARCHAR(6),

	@Age	INT,

	@Room	VARCHAR(10),

	@Bed	INT,

	@RegisterDate	DATETIME,

	@DoctorName	VARCHAR(22),

	@BirthDate	DATETIME,

	@Note	VARCHAR(40),

	@PDMLock	BIT,

	@PLNLock	BIT,

	@PatientPhoto	IMAGE,

	@Completed	BIT,

	@DownLoad	INT,

	@DoctorForDownLoad	VARCHAR(20),

	@Deleted	BIT,

	@LogRecord	IMAGE,

	@NetPassword	VARCHAR(20),

	@IsWorkNormal	BIT,

	@Compressed	BIT,

	@UserLocked VARCHAR(50),

	@Type NVARCHAR(50),

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)   ,

	@ApplyRowVersion BIT

 AS 

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' UPDATE Patient SET

		TPID = @TPID,

		PatientID = @PatientID,

		Name = @Name,

		FirstName = @FirstName,

		MiddleName = @MiddleName,

		Sex = @Sex,

		Age = @Age,

		Room = @Room,

		Bed = @Bed,

		RegisterDate = @RegisterDate,

		DoctorName = @DoctorName,

		BirthDate = @BirthDate,

		Note = @Note,

		PDMLock = @PDMLock,

		PLNLock = @PLNLock,

		PatientPhoto = @PatientPhoto,

		Completed = @Completed,

		DownLoad = @DownLoad,

		DoctorForDownLoad = @DoctorForDownLoad,

		Deleted = @Deleted,

		LogRecord = @LogRecord,

		NetPassword = @NetPassword,

		IsWorkNormal = @IsWorkNormal,

		Compressed = @Compressed,

		UserLocked = @UserLocked,

		Type  = @Type

		where TPID = @TPID AND UpdateTime=@UpdateTime

		SELECT @UpdateTime = UpdateTime FROM Patient WHERE TPID=@TPID'

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' UPDATE Patient SET

		TPID = @TPID,

		PatientID = @PatientID,

		Name = @Name,

		FirstName = @FirstName,

		MiddleName = @MiddleName,

		Sex = @Sex,

		Age = @Age,

		Room = @Room,

		Bed = @Bed,

		RegisterDate = @RegisterDate,

		DoctorName = @DoctorName,

		BirthDate = @BirthDate,

		Note = @Note,

		PDMLock = @PDMLock,

		PLNLock = @PLNLock,

		PatientPhoto = @PatientPhoto,

		Completed = @Completed,

		DownLoad = @DownLoad,

		DoctorForDownLoad = @DoctorForDownLoad,

		Deleted = @Deleted,

		LogRecord = @LogRecord,

		NetPassword = @NetPassword,

		IsWorkNormal = @IsWorkNormal,

		Compressed = @Compressed,

		UserLocked = @UserLocked,

		Type  = @Type

		where TPID = @TPID

		SELECT @UpdateTime = UpdateTime FROM Patient WHERE TPID=@TPID'

	END

	EXECUTE  sp_executesql @sql,N'

	@TPID	INT,

	@PatientID	VARCHAR(20),

	@Name	VARCHAR(22),

	@FirstName	VARCHAR(22),

	@MiddleName	VARCHAR(22),

	@Sex	VARCHAR(6),

	@Age	INT,

	@Room	VARCHAR(10),

	@Bed	INT,

	@RegisterDate	DATETIME,

	@DoctorName	VARCHAR(22),

	@BirthDate	DATETIME,

	@Note	VARCHAR(40),

	@PDMLock	BIT,

	@PLNLock	BIT,

	@PatientPhoto	IMAGE,

	@Completed	BIT,

	@DownLoad	INT,

	@DoctorForDownLoad	VARCHAR(20),

	@Deleted	BIT,

	@LogRecord	IMAGE,

	@NetPassword	VARCHAR(20),

	@IsWorkNormal	BIT,

	@Compressed	BIT,

	@UserLocked VARCHAR(50),

	@Type NVARCHAR(50),

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@TPID,

	@PatientID,

	@Name,

	@FirstName,

	@MiddleName,

	@Sex,

	@Age,

	@Room,

	@Bed,

	@RegisterDate,

	@DoctorName,

	@BirthDate,

	@Note,

	@PDMLock,

	@PLNLock,

	@PatientPhoto,

	@Completed,

	@DownLoad,


	@DoctorForDownLoad,

	@Deleted,

	@LogRecord,

	@NetPassword,

	@IsWorkNormal,

	@Compressed,

	@UserLocked,

	@Type,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_tbDVH_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:45:43

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:45:43	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbDVH_UPDATE

	@TPID	BIGINT,

	@PlanID	BIGINT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@OrganName	VARCHAR(20),

	@OrganColor	VARCHAR(16),

	@DDVHMaxVolume	FLOAT,

	@DifferentialDVH	IMAGE,

	@CDVHMaxVolume	FLOAT,

	@CumulativeDVH	IMAGE,

	@MaxDose	FLOAT,

	@MinDose	FLOAT,

	@AverageDose	FLOAT,

	@MinSurfaceDose	FLOAT,

	@MaxSurfaceDose	FLOAT,

	@AverageSurfaceDose	CHAR(10),

	@TCP	FLOAT,

	@NTCP	FLOAT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)   ,

	@ApplyRowVersion BIT

 AS 

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' UPDATE tbDVH SET

		TPID = @TPID,

		[PlanID] = @PlanID,

		StudyID = @StudyID,

		Course = @Course,

		OrganName = @OrganName,

		OrganColor = @OrganColor,

		DDVHMaxVolume = @DDVHMaxVolume,

		DifferentialDVH = @DifferentialDVH,

		CDVHMaxVolume = @CDVHMaxVolume,

		CumulativeDVH = @CumulativeDVH,

		MaxDose = @MaxDose,

		MinDose = @MinDose,

		AverageDose = @AverageDose,

		MinSurfaceDose = @MinSurfaceDose,

		MaxSurfaceDose = @MaxSurfaceDose,

		AverageSurfaceDose = @AverageSurfaceDose,

		TCP = @TCP,

		NTCP = @NTCP

		where TPID = @TPID AND UpdateTime=@UpdateTime

		SELECT @UpdateTime = UpdateTime FROM tbDVH WHERE TPID=@TPID'

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' UPDATE tbDVH SET

		TPID = @TPID,

		[PlanID] = @PlanID,

		StudyID = @StudyID,

		Course = @Course,

		OrganName = @OrganName,

		OrganColor = @OrganColor,

		DDVHMaxVolume = @DDVHMaxVolume,

		DifferentialDVH = @DifferentialDVH,

		CDVHMaxVolume = @CDVHMaxVolume,

		CumulativeDVH = @CumulativeDVH,

		MaxDose = @MaxDose,

		MinDose = @MinDose,

		AverageDose = @AverageDose,

		MinSurfaceDose = @MinSurfaceDose,

		MaxSurfaceDose = @MaxSurfaceDose,

		AverageSurfaceDose = @AverageSurfaceDose,

		TCP = @TCP,

		NTCP = @NTCP

		where TPID = @TPID

		SELECT @UpdateTime = UpdateTime FROM tbDVH WHERE TPID=@TPID'

	END

	EXECUTE  sp_executesql @sql,N'

	@TPID	BIGINT ,

	@PlanID	BIGINT ,

	@StudyID	BIGINT ,

	@Course	VARCHAR(10),

	@OrganName	VARCHAR(20),

	@OrganColor	VARCHAR(16),

	@DDVHMaxVolume	FLOAT,

	@DifferentialDVH	IMAGE,

	@CDVHMaxVolume	FLOAT,

	@CumulativeDVH	IMAGE,

	@MaxDose	FLOAT,

	@MinDose	FLOAT,

	@AverageDose	FLOAT,

	@MinSurfaceDose	FLOAT,

	@MaxSurfaceDose	FLOAT,

	@AverageSurfaceDose	CHAR(10),

	@TCP	FLOAT,

	@NTCP	FLOAT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@TPID,

	@PlanID,

	@StudyID,

	@Course,

	@OrganName,

	@OrganColor,

	@DDVHMaxVolume,

	@DifferentialDVH,

	@CDVHMaxVolume,

	@CumulativeDVH,

	@MaxDose,

	@MinDose,

	@AverageDose,

	@MinSurfaceDose,

	@MaxSurfaceDose,

	@AverageSurfaceDose,

	@TCP,

	@NTCP,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_tbDVH_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:45:43

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:45:43	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbDVH_INSERT

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@PlanID	BIGINT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@OrganName	VARCHAR(20),

	@OrganColor	VARCHAR(16),

	@DDVHMaxVolume	FLOAT,

	@DifferentialDVH	IMAGE,

	@CDVHMaxVolume	FLOAT,

	@CumulativeDVH	IMAGE,

	@MaxDose	FLOAT,

	@MinDose	FLOAT,

	@AverageDose	FLOAT,

	@MinSurfaceDose	FLOAT,

	@MaxSurfaceDose	FLOAT,

	@AverageSurfaceDose	CHAR(10),

	@TCP	FLOAT,

	@NTCP	FLOAT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)

 AS

	IF(@TPID is Null)

	BEGIN

		SELECT @TPID=-1

	END

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO tbDVH

	(

		TPID,

		[PlanID],

		StudyID,

		Course,

		OrganName,

		OrganColor,

		DDVHMaxVolume,

		DifferentialDVH,

		CDVHMaxVolume,

		CumulativeDVH,

		MaxDose,

		MinDose,

		AverageDose,

		MinSurfaceDose,

		MaxSurfaceDose,

		AverageSurfaceDose,

		TCP,

		NTCP

	)

	VALUES

	(

		@TPID,

		@PlanID,

		@StudyID,

		@Course,

		@OrganName,

		@OrganColor,

		@DDVHMaxVolume,

		@DifferentialDVH,

		@CDVHMaxVolume,

		@CumulativeDVH,

		@MaxDose,

		@MinDose,

		@AverageDose,

		@MinSurfaceDose,

		@MaxSurfaceDose,

		@AverageSurfaceDose,

		@TCP,

		@NTCP

	)

	SET @AutoID=SCOPE_IDENTITY()

	SELECT @TPID= TPID,@UpdateTime = UpdateTime FROM tbDVH WHERE AutoID=SCOPE_IDENTITY()

	IF (@TPID =-1)

	BEGIN

		SET @TPID=SCOPE_IDENTITY()

		UPDATE tbDVH SET TPID=@TPID  WHERE AutoID=@TPID

	SELECT @UpdateTime = UpdateTime FROM tbDVH WHERE AutoID=SCOPE_IDENTITY()

	END'

	EXECUTE  sp_executesql @sql,N'

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@PlanID	BIGINT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@OrganName	VARCHAR(20),

	@OrganColor	VARCHAR(16),

	@DDVHMaxVolume	FLOAT,

	@DifferentialDVH	IMAGE,

	@CDVHMaxVolume	FLOAT,

	@CumulativeDVH	IMAGE,

	@MaxDose	FLOAT,

	@MinDose	FLOAT,

	@AverageDose	FLOAT,

	@MinSurfaceDose	FLOAT,

	@MaxSurfaceDose	FLOAT,

	@AverageSurfaceDose	CHAR(10),

	@TCP	FLOAT,

	@NTCP	FLOAT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@AutoID OUTPUT,

	@TPID OUTPUT,

	@PlanID,

	@StudyID,

	@Course,

	@OrganName,

	@OrganColor,

	@DDVHMaxVolume,

	@DifferentialDVH,

	@CDVHMaxVolume,

	@CumulativeDVH,

	@MaxDose,

	@MinDose,

	@AverageDose,

	@MinSurfaceDose,

	@MaxSurfaceDose,

	@AverageSurfaceDose,

	@TCP,

	@NTCP,

	@UpdateTime OUTPUT,

	@DBName   

go
/******************************************************************

* ÂêçÁß∞: SP_tbBeamOrient_INSERT

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:44:04

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:44:04	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbBeamOrient_INSERT

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@OrientConstraintName	VARCHAR(30),

	@AngleFrom	FLOAT,

	@AngleTo	FLOAT,

	@Clockwise	BIT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@PlanID	BIGINT,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)

 AS

	IF(@TPID is Null)

	BEGIN

		SELECT @TPID=-1

	END

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	SET @Sql=@Sql+' INSERT INTO tbBeamOrient

	(

		TPID,

		OrientConstraintName,

		AngleFrom,

		AngleTo,

		Clockwise,

		StudyID,

		Course,

		[PlanID]

	)

	VALUES

	(

		@TPID,

		@OrientConstraintName,

		@AngleFrom,

		@AngleTo,

		@Clockwise,

		@StudyID,

		@Course,

		@PlanID

	)

	SET @AutoID=SCOPE_IDENTITY()

	SELECT @TPID= TPID,@UpdateTime = UpdateTime FROM tbBeamOrient WHERE AutoID=SCOPE_IDENTITY()

	IF (@TPID =-1)

	BEGIN

		SET @TPID=SCOPE_IDENTITY()

		UPDATE tbBeamOrient SET TPID=@TPID  WHERE AutoID=@TPID

	SELECT @UpdateTime = UpdateTime FROM tbBeamOrient WHERE AutoID=SCOPE_IDENTITY()

	END'

	EXECUTE  sp_executesql @sql,N'

	@AutoID	INT OUTPUT,

	@TPID	BIGINT OUTPUT,

	@OrientConstraintName	VARCHAR(30),

	@AngleFrom	FLOAT,

	@AngleTo	FLOAT,

	@Clockwise	BIT,

	@StudyID	BIGINT ,

	@Course	VARCHAR(10),

	@PlanID	BIGINT ,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@AutoID OUTPUT,

	@TPID OUTPUT,

	@OrientConstraintName,

	@AngleFrom,

	@AngleTo,

	@Clockwise,

	@StudyID,

	@Course,

	@PlanID,

	@UpdateTime OUTPUT,

	@DBName   

go

CREATE  PROCEDURE SP_tbBeam_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM tbBeam'

	EXEC(@Sql)

go
/******************************************************************

* ÂêçÁß∞: tbBEV_DELETE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:44:17

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:44:17	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbBEV_DELETE

	 @TPID	BIGINT

	,@DBName NVARCHAR(100),

	@UpdateTime TIMESTAMP,

	@ApplyRowVersion BIT

AS

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbBEV

		where TPID = @TPID AND UpdateTime=@UpdateTime'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ,

		@UpdateTime TIMESTAMP',	

		@DBName,

		@TPID,

		@UpdateTime

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' DELETE FROM tbBEV

		where TPID = @TPID'

		EXECUTE  sp_executesql @sql,N'

		@DBName NVARCHAR(100),

		@TPID	BIGINT ',	

		@DBName,

		@TPID

	END

go
/******************************************************************

* ÂêçÁß∞: SP_tbBEV_UPDATE

* ‰ΩúËÄ? Topslane

* Êó∂Èó¥: 2006-4-20 11:44:17

*

* -----------------------------------------------------------------

* ÁâàÊú¨		Êó∂Èó¥			‰ΩúËÄ?	Â§áÊ≥®

*

* V2.00		2006-4-20 11:44:17	LZH		ÂàõÂª∫

* -----------------------------------------------------------------

******************************************************************/

CREATE  PROCEDURE SP_tbBEV_UPDATE

	@TPID	BIGINT,

	@StudyID	BIGINT,

	@Course	VARCHAR(10),

	@PlanID	BIGINT,

	@BeamID	BIGINT,

	@OrganName	VARCHAR(18),

	@OrganBEVOutline	IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)   ,

	@ApplyRowVersion BIT

 AS 

	SET NOCOUNT ON

	SET XACT_ABORT ON

	DECLARE @Sql NVARCHAR(4000) 

 	SET @Sql='USE '+@DBName

	IF(@ApplyRowVersion=1)

	BEGIN

		SET @Sql=@Sql+' UPDATE tbBEV SET

		TPID = @TPID,

		StudyID = @StudyID,

		Course = @Course,

		[PlanID] = @PlanID,

		BeamID = @BeamID,

		OrganName = @OrganName,

		OrganBEVOutline = @OrganBEVOutline

		where TPID = @TPID AND UpdateTime=@UpdateTime

		SELECT @UpdateTime = UpdateTime FROM tbBEV WHERE TPID=@TPID'

	END

	ELSE

	BEGIN

		SET @Sql=@Sql+' UPDATE tbBEV SET

		TPID = @TPID,

		StudyID = @StudyID,

		Course = @Course,

		[PlanID] = @PlanID,

		BeamID = @BeamID,

		OrganName = @OrganName,

		OrganBEVOutline = @OrganBEVOutline

		where TPID = @TPID

		SELECT @UpdateTime = UpdateTime FROM tbBEV WHERE TPID=@TPID'

	END

	EXECUTE  sp_executesql @sql,N'

	@TPID	BIGINT ,

	@StudyID	BIGINT ,

	@Course	VARCHAR(10),

	@PlanID	BIGINT ,

	@BeamID	BIGINT ,

	@OrganName	VARCHAR(18),

	@OrganBEVOutline	IMAGE,

	@UpdateTime	TIMESTAMP OUTPUT,

	@DBName NVARCHAR(100)',

	@TPID,

	@StudyID,

	@Course,

	@PlanID,

	@BeamID,

	@OrganName,

	@OrganBEVOutline,

	@UpdateTime OUTPUT,

	@DBName   

go

CREATE  PROCEDURE SP_Patient_SELECT

	@DBName NVARCHAR(100)

AS

	DECLARE @Sql NVARCHAR(200)

	SET @Sql ='USE '+@DBName 

	SET @Sql =@Sql +' SELECT * FROM Patient'

	EXEC(@Sql)

go

/******************************************************************
* √˚≥∆: SP_tbDoseCalculateFactor_INSERT
* ◊˜’ﬂ: Topslane
*  ±º‰: 2006-11-3 9:27:04
*
* -----------------------------------------------------------------
* ∞Ê±æ		 ±º‰			◊˜’ﬂ		±∏◊¢
*
* V2.00		2006-11-3 9:27:04	LZH		¥¥Ω®
* -----------------------------------------------------------------
******************************************************************/
CREATE  PROCEDURE SP_tbDoseCalculateFactor_INSERT
	@TotalScatter0	FLOAT,
	@TotalScatter1	FLOAT,
	@TotalScatter2	FLOAT,
	@TotalScatter3	FLOAT,
	@PhantomScatter0	FLOAT,
	@PhantomScatter1	FLOAT,
	@PhantomScatter2	FLOAT,
	@PhantomScatter3	FLOAT,
	@CalibrationFactor	FLOAT,
	@DBName NVARCHAR(100)
 AS

	DECLARE @Sql NVARCHAR(4000) 
 	SET @Sql='USE '+@DBName
	SET @Sql=@Sql+' INSERT INTO tbDoseCalculateFactor
	(
		TotalScatter0,
		TotalScatter1,
		TotalScatter2,
		TotalScatter3,
		PhantomScatter0,
		PhantomScatter1,
		PhantomScatter2,
		PhantomScatter3,
		CalibrationFactor
	)
	VALUES
	(
		@TotalScatter0,
		@TotalScatter1,
		@TotalScatter2,
		@TotalScatter3,
		@PhantomScatter0,
		@PhantomScatter1,
		@PhantomScatter2,
		@PhantomScatter3,
		@CalibrationFactor
	)
	'
	EXECUTE  sp_executesql @sql,N'
	@TotalScatter0	FLOAT,
	@TotalScatter1	FLOAT,
	@TotalScatter2	FLOAT,
	@TotalScatter3	FLOAT,
	@PhantomScatter0	FLOAT,
	@PhantomScatter1	FLOAT,
	@PhantomScatter2	FLOAT,
	@PhantomScatter3	FLOAT,
	@CalibrationFactor	FLOAT,
	@DBName NVARCHAR(100)',

	@TotalScatter0,
	@TotalScatter1,
	@TotalScatter2,
	@TotalScatter3,
	@PhantomScatter0,
	@PhantomScatter1,
	@PhantomScatter2,
	@PhantomScatter3,
	@CalibrationFactor,
	@DBName   
	
	
go

/******************************************************************
* √˚≥∆: SP_tbDoseTable_INSERT
* ◊˜’ﬂ: Topslane
*  ±º‰: 2006-11-3 9:27:55
*
* -----------------------------------------------------------------
* ∞Ê±æ		 ±º‰			◊˜’ﬂ		±∏◊¢
*
* V2.00		2006-11-3 9:27:55	LZH		¥¥Ω®
* -----------------------------------------------------------------
******************************************************************/
CREATE  PROCEDURE SP_tbDoseTable_INSERT
	@WedgeorCone	BIT,
	@WedgeAngle	FLOAT,
	@WedgeorConeIdentifier	NVARCHAR(40),
	@TypeofTable	INT,
	@FocusSkinDistance	FLOAT,
	@NormalizationDepth	FLOAT,
	@NormalizationSize	FLOAT,
	@DataPoints	IMAGE,
	@FieldSize	FLOAT,
	@ZDrift	FLOAT,
	@OutputFactor	FLOAT,
	@DBName NVARCHAR(100)
 AS

	DECLARE @Sql NVARCHAR(4000) 
 	SET @Sql='USE '+@DBName
	SET @Sql=@Sql+' INSERT INTO tbDoseTable
	(
		WedgeorCone,
		WedgeAngle,
		WedgeorConeIdentifier,
		TypeofTable,
		FocusSkinDistance,
		NormalizationDepth,
		NormalizationSize,
		DataPoints,
		FieldSize,
		ZDrift,
		OutputFactor
	)
	VALUES
	(
		@WedgeorCone,
		@WedgeAngle,
		@WedgeorConeIdentifier,
		@TypeofTable,
		@FocusSkinDistance,
		@NormalizationDepth,
		@NormalizationSize,
		@DataPoints,
		@FieldSize,
		@ZDrift,
		@OutputFactor
	)
	'
	EXECUTE  sp_executesql @sql,N'
	@WedgeorCone	BIT,
	@WedgeAngle	FLOAT,
	@WedgeorConeIdentifier	NVARCHAR(40),
	@TypeofTable	INT,
	@FocusSkinDistance	FLOAT,
	@NormalizationDepth	FLOAT,
	@NormalizationSize	FLOAT,
	@DataPoints	IMAGE,
	@FieldSize	FLOAT,
	@ZDrift	FLOAT,
	@OutputFactor	FLOAT,
	@DBName NVARCHAR(100)',

	@WedgeorCone,
	@WedgeAngle,
	@WedgeorConeIdentifier,
	@TypeofTable,
	@FocusSkinDistance,
	@NormalizationDepth,
	@NormalizationSize,
	@DataPoints,
	@FieldSize,
	@ZDrift,
	@OutputFactor,
	@DBName   
	
	
go

/******************************************************************
* √˚≥∆: SP_tbDynamicWedge_INSERT
* ◊˜’ﬂ: Topslane
*  ±º‰: 2006-11-3 9:28:19
*
* -----------------------------------------------------------------
* ∞Ê±æ		 ±º‰			◊˜’ﬂ		±∏◊¢
*
* V2.00		2006-11-3 9:28:19	LZH		¥¥Ω®
* -----------------------------------------------------------------
******************************************************************/
CREATE  PROCEDURE SP_tbDynamicWedge_INSERT
	@WedgeAngle	FLOAT,
	@OpenRatio	FLOAT,
	@StepSpacecm	FLOAT,
	@DBName NVARCHAR(100)
 AS

	DECLARE @Sql NVARCHAR(4000) 
 	SET @Sql='USE '+@DBName
	SET @Sql=@Sql+' INSERT INTO tbDynamicWedge
	(
		WedgeAngle,
		OpenRatio,
		StepSpacecm
	)
	VALUES
	(
		@WedgeAngle,
		@OpenRatio,
		@StepSpacecm
	)
	
	SET @ID=SCOPE_IDENTITY()
	'
	EXECUTE  sp_executesql @sql,N'
	@WedgeAngle	FLOAT,
	@OpenRatio	FLOAT,
	@StepSpacecm	FLOAT,
	@DBName NVARCHAR(100)',

	@WedgeAngle,
	@OpenRatio,
	@StepSpacecm,
	@DBName   
	
	
go


/******************************************************************
* √˚≥∆: SP_tbFluenceMatrix_INSERT
* ◊˜’ﬂ: Topslane
*  ±º‰: 2006-11-3 9:28:50
*
* -----------------------------------------------------------------
* ∞Ê±æ		 ±º‰			◊˜’ﬂ		±∏◊¢
*
* V2.00		2006-11-3 9:28:50	LZH		¥¥Ω®
* -----------------------------------------------------------------
******************************************************************/
CREATE  PROCEDURE SP_tbFluenceMatrix_INSERT
	@WedgeorCone	BIT,
	@WedgeAngle	FLOAT,
	@WedgeorConeIdentifier	NVARCHAR(40),
	@Dx	FLOAT,
	@Dy	FLOAT,
	@Modality	INT,
	@IsMeasured	BIT,
	@Width	FLOAT,
	@Length	FLOAT,
	@InPlaneCoordination	FLOAT,
	@DataPoints	IMAGE,
	@DBName NVARCHAR(100)
 AS

	DECLARE @Sql NVARCHAR(4000) 
 	SET @Sql='USE '+@DBName
	SET @Sql=@Sql+' INSERT INTO tbFluenceMatrix
	(
		WedgeorCone,
		WedgeAngle,
		WedgeorConeIdentifier,
		Dx,
		Dy,
		Modality,
		IsMeasured,
		Width,
		Length,
		InPlaneCoordination,
		DataPoints
	)
	VALUES
	(
		@WedgeorCone,
		@WedgeAngle,
		@WedgeorConeIdentifier,
		@Dx,
		@Dy,
		@Modality,
		@IsMeasured,
		@Width,
		@Length,
		@InPlaneCoordination,
		@DataPoints
	)
	'
	EXECUTE  sp_executesql @sql,N'
	@WedgeorCone	BIT,
	@WedgeAngle	FLOAT,
	@WedgeorConeIdentifier	NVARCHAR(40),
	@Dx	FLOAT,
	@Dy	FLOAT,
	@Modality	INT,
	@IsMeasured	BIT,
	@Width	FLOAT,
	@Length	FLOAT,
	@InPlaneCoordination	FLOAT,
	@DataPoints	IMAGE,
	@DBName NVARCHAR(100)',

	@WedgeorCone,
	@WedgeAngle,
	@WedgeorConeIdentifier,
	@Dx,
	@Dy,
	@Modality,
	@IsMeasured,
	@Width,
	@Length,
	@InPlaneCoordination,
	@DataPoints,
	@DBName   
	
	
go

/******************************************************************
* √˚≥∆: SP_tbMotorizedWedge_INSERT
* ◊˜’ﬂ: Topslane
*  ±º‰: 2006-11-3 9:29:03
*
* -----------------------------------------------------------------
* ∞Ê±æ		 ±º‰			◊˜’ﬂ		±∏◊¢
*
* V2.00		2006-11-3 9:29:03	LZH		¥¥Ω®
* -----------------------------------------------------------------
******************************************************************/
CREATE  PROCEDURE SP_tbMotorizedWedge_INSERT
	@ID	INT OUTPUT,
	@FieldSize	FLOAT,
	@WedgeAngle	FLOAT,
	@WedgeFactor	FLOAT,
	@WedgeDoseRatio	FLOAT,
	@DBName NVARCHAR(100)
 AS

	DECLARE @Sql NVARCHAR(4000) 
 	SET @Sql='USE '+@DBName
	SET @Sql=@Sql+' INSERT INTO tbMotorizedWedge
	(
		FieldSize,
		WedgeAngle,
		WedgeFactor,
		WedgeDoseRatio
	)
	VALUES
	(
		@FieldSize,
		@WedgeAngle,
		@WedgeFactor,
		@WedgeDoseRatio
	)
	
	SET @ID=SCOPE_IDENTITY()
	'
	EXECUTE  sp_executesql @sql,N'
	@ID	INT OUTPUT,
	@FieldSize	FLOAT,
	@WedgeAngle	FLOAT,
	@WedgeFactor	FLOAT,
	@WedgeDoseRatio	FLOAT,
	@DBName NVARCHAR(100)',
	@ID	OUTPUT,
	@FieldSize,
	@WedgeAngle,
	@WedgeFactor,
	@WedgeDoseRatio,
	@DBName   
	
	
go

/******************************************************************
* √˚≥∆: SP_tbPenumbraParameter_INSERT
* ◊˜’ﬂ: Topslane
*  ±º‰: 2006-11-3 9:29:14
*
* -----------------------------------------------------------------
* ∞Ê±æ		 ±º‰			◊˜’ﬂ		±∏◊¢
*
* V2.00		2006-11-3 9:29:14	LZH		¥¥Ω®
* -----------------------------------------------------------------
******************************************************************/
CREATE  PROCEDURE SP_tbPenumbraParameter_INSERT
	@WedgeAngle	FLOAT,
	@WedgeFactor	FLOAT,
	@AlphainJawAB	FLOAT,
	@AlphaoutJawAB	FLOAT,
	@AlphainJawCD	FLOAT,
	@AlphaoutJawCD	FLOAT,
	@PenumbraAB20to50	FLOAT,
	@PenumbraAB50to80	FLOAT,
	@PenumbraCD20to50	FLOAT,
	@PenumbraCD50to80	FLOAT,
	@CollimatorTransmission	FLOAT,
	@Type	INT,
	@Orientation	INT,
	@Rotation	FLOAT,
	@Fraction	FLOAT,
	@DBName NVARCHAR(100)
 AS

	DECLARE @Sql NVARCHAR(4000) 
 	SET @Sql='USE '+@DBName
	SET @Sql=@Sql+' INSERT INTO tbPenumbraParameter
	(
		WedgeAngle,
		WedgeFactor,
		AlphainJawAB,
		AlphaoutJawAB,
		AlphainJawCD,
		AlphaoutJawCD,
		PenumbraAB20to50,
		PenumbraAB50to80,
		PenumbraCD20to50,
		PenumbraCD50to80,
		CollimatorTransmission,
		Type,
		Orientation,
		Rotation,
		Fraction
	)
	VALUES
	(
		@WedgeAngle,
		@WedgeFactor,
		@AlphainJawAB,
		@AlphaoutJawAB,
		@AlphainJawCD,
		@AlphaoutJawCD,
		@PenumbraAB20to50,
		@PenumbraAB50to80,
		@PenumbraCD20to50,
		@PenumbraCD50to80,
		@CollimatorTransmission,
		@Type,
		@Orientation,
		@Rotation,
		@Fraction
	)
	'
	EXECUTE  sp_executesql @sql,N'
	@WedgeAngle	FLOAT,
	@WedgeFactor	FLOAT,
	@AlphainJawAB	FLOAT,
	@AlphaoutJawAB	FLOAT,
	@AlphainJawCD	FLOAT,
	@AlphaoutJawCD	FLOAT,
	@PenumbraAB20to50	FLOAT,
	@PenumbraAB50to80	FLOAT,
	@PenumbraCD20to50	FLOAT,
	@PenumbraCD50to80	FLOAT,
	@CollimatorTransmission	FLOAT,
	@Type	INT,
	@Orientation	INT,
	@Rotation	FLOAT,
	@Fraction	FLOAT,
	@DBName NVARCHAR(100)',

	@WedgeAngle,
	@WedgeFactor,
	@AlphainJawAB,
	@AlphaoutJawAB,
	@AlphainJawCD,
	@AlphaoutJawCD,
	@PenumbraAB20to50,
	@PenumbraAB50to80,
	@PenumbraCD20to50,
	@PenumbraCD50to80,
	@CollimatorTransmission,
	@Type,
	@Orientation,
	@Rotation,
	@Fraction,
	@DBName   
	
	
go


/******************************************************************
* √˚≥∆: SP_tbPrimaryDepthDose_INSERT
* ◊˜’ﬂ: Topslane
*  ±º‰: 2006-11-3 9:29:28
*
* -----------------------------------------------------------------
* ∞Ê±æ		 ±º‰			◊˜’ﬂ		±∏◊¢
*
* V2.00		2006-11-3 9:29:28	LZH		¥¥Ω®
* -----------------------------------------------------------------
******************************************************************/
CREATE  PROCEDURE SP_tbPrimaryDepthDose_INSERT
	@WedgeAngle	FLOAT,
	@Attenuation	FLOAT,
	@Dmax	FLOAT,
	@Tmr0	FLOAT,
	@DataPoints	IMAGE,
	@DBName NVARCHAR(100)
 AS

	DECLARE @Sql NVARCHAR(4000) 
 	SET @Sql='USE '+@DBName
	SET @Sql=@Sql+' INSERT INTO tbPrimaryDepthDose
	(
		WedgeAngle,
		Attenuation,
		Dmax,
		Tmr0,
		DataPoints
	)
	VALUES
	(
		@WedgeAngle,
		@Attenuation,
		@Dmax,
		@Tmr0,
		@DataPoints
	)
	'
	EXECUTE  sp_executesql @sql,N'
	@WedgeAngle	FLOAT,
	@Attenuation	FLOAT,
	@Dmax	FLOAT,
	@Tmr0	FLOAT,
	@DataPoints	IMAGE,
	@DBName NVARCHAR(100)',

	@WedgeAngle,
	@Attenuation,
	@Dmax,
	@Tmr0,
	@DataPoints,
	@DBName   
	
	
go

/******************************************************************
* √˚≥∆: SP_tbProfile_INSERT
* ◊˜’ﬂ: Topslane
*  ±º‰: 2006-11-3 9:29:41
*
* -----------------------------------------------------------------
* ∞Ê±æ		 ±º‰			◊˜’ﬂ		±∏◊¢
*
* V2.00		2006-11-3 9:29:41	LZH		¥¥Ω®
* -----------------------------------------------------------------
******************************************************************/
CREATE  PROCEDURE SP_tbProfile_INSERT
	@ID	INT OUTPUT,
	@TypeofScan	INT,
	@ScanDirection	INT,
	@FieldWidth	FLOAT,
	@FieldLength	FLOAT,
	@ShapeofField	INT,
	@SourcetoCalibDistance	FLOAT,
	@DepthofProfileScan	FLOAT,
	@OffAxisDistance	FLOAT,
	@WedgeAngle	FLOAT,
	@WedgeorConeIdentifier	NVARCHAR(40),
	@WedgeFactor	FLOAT,
	@ConeType	FLOAT,
	@ConeWidth	FLOAT,
	@ConeLength	FLOAT,
	@ConeJawMarginAB	FLOAT,
	@ConeJawMarginCD	FLOAT,
	@ECFED	FLOAT,
	@EquivalentFieldSize	FLOAT,
	@TotalScatter	FLOAT,
	@CollimatorScatter	FLOAT,
	@CurveTitle	NVARCHAR(160),
	@MeasurementType	INT,
	@NormalizationDose	FLOAT,
	@NormalizationDepth	FLOAT,
	@MeasureData	IMAGE,
	@CalculateData	IMAGE,
	@Note	NTEXT,
	@DBName NVARCHAR(100)
 AS

	DECLARE @Sql NVARCHAR(4000) 
 	SET @Sql='USE '+@DBName
	SET @Sql=@Sql+' INSERT INTO tbProfile
	(
		TypeofScan,
		ScanDirection,
		FieldWidth,
		FieldLength,
		ShapeofField,
		SourcetoCalibDistance,
		DepthofProfileScan,
		OffAxisDistance,
		WedgeAngle,
		WedgeorConeIdentifier,
		WedgeFactor,
		ConeType,
		ConeWidth,
		ConeLength,
		ConeJawMarginAB,
		ConeJawMarginCD,
		ECFED,
		EquivalentFieldSize,
		TotalScatter,
		CollimatorScatter,
		CurveTitle,
		MeasurementType,
		NormalizationDose,
		NormalizationDepth,
		MeasureData,
		CalculateData,
		Note
	)
	VALUES
	(
		@TypeofScan,
		@ScanDirection,
		@FieldWidth,
		@FieldLength,
		@ShapeofField,
		@SourcetoCalibDistance,
		@DepthofProfileScan,
		@OffAxisDistance,
		@WedgeAngle,
		@WedgeorConeIdentifier,
		@WedgeFactor,
		@ConeType,
		@ConeWidth,
		@ConeLength,
		@ConeJawMarginAB,
		@ConeJawMarginCD,
		@ECFED,
		@EquivalentFieldSize,
		@TotalScatter,
		@CollimatorScatter,
		@CurveTitle,
		@MeasurementType,
		@NormalizationDose,
		@NormalizationDepth,
		@MeasureData,
		@CalculateData,
		@Note
	)
	
	SET @ID=SCOPE_IDENTITY()
	'
	EXECUTE  sp_executesql @sql,N'
	@ID	INT OUTPUT,
	@TypeofScan	INT,
	@ScanDirection	INT,
	@FieldWidth	FLOAT,
	@FieldLength	FLOAT,
	@ShapeofField	INT,
	@SourcetoCalibDistance	FLOAT,
	@DepthofProfileScan	FLOAT,
	@OffAxisDistance	FLOAT,
	@WedgeAngle	FLOAT,
	@WedgeorConeIdentifier	NVARCHAR(40),
	@WedgeFactor	FLOAT,
	@ConeType	FLOAT,
	@ConeWidth	FLOAT,
	@ConeLength	FLOAT,
	@ConeJawMarginAB	FLOAT,
	@ConeJawMarginCD	FLOAT,
	@ECFED	FLOAT,
	@EquivalentFieldSize	FLOAT,
	@TotalScatter	FLOAT,
	@CollimatorScatter	FLOAT,
	@CurveTitle	NVARCHAR(160),
	@MeasurementType	INT,
	@NormalizationDose	FLOAT,
	@NormalizationDepth	FLOAT,
	@MeasureData	IMAGE,
	@CalculateData	IMAGE,
	@Note	NTEXT,
	@DBName NVARCHAR(100)',
	@ID	OUTPUT,
	@TypeofScan,
	@ScanDirection,
	@FieldWidth,
	@FieldLength,
	@ShapeofField,
	@SourcetoCalibDistance,
	@DepthofProfileScan,
	@OffAxisDistance,
	@WedgeAngle,
	@WedgeorConeIdentifier,
	@WedgeFactor,
	@ConeType,
	@ConeWidth,
	@ConeLength,
	@ConeJawMarginAB,
	@ConeJawMarginCD,
	@ECFED,
	@EquivalentFieldSize,
	@TotalScatter,
	@CollimatorScatter,
	@CurveTitle,
	@MeasurementType,
	@NormalizationDose,
	@NormalizationDepth,
	@MeasureData,
	@CalculateData,
	@Note,
	@DBName   
	
	
go


/******************************************************************
* √˚≥∆: SP_tbTriangleToSquare_INSERT
* ◊˜’ﬂ: Topslane
*  ±º‰: 2006-11-3 9:30:00
*
* -----------------------------------------------------------------
* ∞Ê±æ		 ±º‰			◊˜’ﬂ		±∏◊¢
*
* V2.00		2006-11-3 9:30:00	LZH		¥¥Ω®
* -----------------------------------------------------------------
******************************************************************/
CREATE  PROCEDURE SP_tbTriangleToSquare_INSERT
	@Name	NVARCHAR(40),
	@Base	FLOAT,
	@DataPoints	IMAGE,
	@DBName NVARCHAR(100)
 AS

	DECLARE @Sql NVARCHAR(4000) 
 	SET @Sql='USE '+@DBName
	SET @Sql=@Sql+' INSERT INTO tbTriangleToSquare
	(
		Name,
		Base,
		DataPoints
	)
	VALUES
	(
		@Name,
		@Base,
		@DataPoints
	)
	'
	EXECUTE  sp_executesql @sql,N'
	@Name	NVARCHAR(40),
	@Base	FLOAT,
	@DataPoints	IMAGE,
	@DBName NVARCHAR(100)',

	@Name,
	@Base,
	@DataPoints,
	@DBName   
	
	
go


/******************************************************************
* √˚≥∆: SP_OrganTrain_INSERT
* ◊˜’ﬂ: Topslane
*  ±º‰: 2006-11-15 9:21:27
*
* -----------------------------------------------------------------
* ∞Ê±æ		 ±º‰			◊˜’ﬂ		±∏◊¢
*
* V2.00		2006-11-15 9:21:27	LZH		¥¥Ω®
* -----------------------------------------------------------------
******************************************************************/
CREATE  PROCEDURE SP_OrganTrain_INSERT
	@AutoID	INT OUTPUT,
	@OrganSpharm	IMAGE,
	@OrganMeshVertex	IMAGE,
	@OrganMeshNormal	IMAGE,
	@OrganMeshFace	IMAGE,
	@OrganSpharmUnit	NVARCHAR(100),
	@OrganMeshUnit	NVARCHAR(100),
	@HospitalID	INT,
	@PatientID	NVARCHAR(100),
	@StudyID	INT,
	@UpdateTime	TIMESTAMP OUTPUT,
	@DBName NVARCHAR(100)
 AS

	DECLARE @Sql NVARCHAR(1000) 
 	SET @Sql='USE '+@DBName
	SET @Sql=@Sql+' INSERT INTO OrganTrain
	(
		OrganSpharm,
		OrganMeshVertex,
		OrganMeshNormal,
		OrganMeshFace,
		OrganSpharmUnit,
		OrganMeshUnit,
		HospitalID,
		PatientID,
		StudyID
	)
	VALUES
	(
		@OrganSpharm,
		@OrganMeshVertex,
		@OrganMeshNormal,
		@OrganMeshFace,
		@OrganSpharmUnit,
		@OrganMeshUnit,
		@HospitalID,
		@PatientID,
		@StudyID
	)
	
	SET @AutoID=SCOPE_IDENTITY()
	
	SELECT @UpdateTime = UpdateTime FROM OrganTrain WHERE AutoID=SCOPE_IDENTITY()
	'

	EXECUTE  sp_executesql @sql,N'
	@AutoID	INT OUTPUT,
	@OrganSpharm	IMAGE,
	@OrganMeshVertex	IMAGE,
	@OrganMeshNormal	IMAGE,
	@OrganMeshFace	IMAGE,
	@OrganSpharmUnit	NVARCHAR(100),
	@OrganMeshUnit	NVARCHAR(100),
	@HospitalID	INT,
	@PatientID	NVARCHAR(100),
	@StudyID	INT,
	@UpdateTime	TIMESTAMP OUTPUT,
	@DBName NVARCHAR(100)',

	@AutoID OUTPUT,
	@OrganSpharm,
	@OrganMeshVertex,
	@OrganMeshNormal,
	@OrganMeshFace,
	@OrganSpharmUnit,
	@OrganMeshUnit,
	@HospitalID,
	@PatientID,
	@StudyID,
	@UpdateTime OUTPUT,
	@DBName   
	
	
go
/******************************************************************
* √˚≥∆: SP_OrganTrain_UPDATE
* ◊˜’ﬂ: Topslane
*  ±º‰: 2006-11-15 9:21:27
*
* -----------------------------------------------------------------
* ∞Ê±æ		 ±º‰			◊˜’ﬂ		±∏◊¢
*
* V2.00		2006-11-15 9:21:27	LZH		¥¥Ω®
* -----------------------------------------------------------------
******************************************************************/
CREATE  PROCEDURE SP_OrganTrain_UPDATE
    @AutoID INT,
	@OrganSpharm	IMAGE,
	@OrganMeshVertex	IMAGE,
	@OrganMeshNormal	IMAGE,
	@OrganMeshFace	IMAGE,
	@OrganSpharmUnit	NVARCHAR(100),
	@OrganMeshUnit	NVARCHAR(100),
	@HospitalID	INT,
	@PatientID	NVARCHAR(100),
	@StudyID	INT,
	@UpdateTime TIMESTAMP OUTPUT,
	@DBName NVARCHAR(100)   ,
	@ApplyRowVersion BIT
 AS 
	SET NOCOUNT ON
	SET XACT_ABORT ON
	DECLARE @Sql NVARCHAR(1000) 
 	SET @Sql='USE '+@DBName
	IF(@ApplyRowVersion=1)
	BEGIN
		SET @Sql=@Sql+' UPDATE OrganTrain SET
		OrganSpharm = @OrganSpharm,
		OrganMeshVertex = @OrganMeshVertex,
		OrganMeshNormal = @OrganMeshNormal,
		OrganMeshFace = @OrganMeshFace,
		OrganSpharmUnit = @OrganSpharmUnit,
		OrganMeshUnit = @OrganMeshUnit,
		HospitalID = @HospitalID,
		PatientID = @PatientID,
		StudyID = @StudyID
		where AutoID = @AutoID AND UpdateTime=@UpdateTime
		SELECT @UpdateTime = UpdateTime FROM OrganTrain WHERE AutoID=@AutoID'
	END
	ELSE
	BEGIN
		SET @Sql=@Sql+' UPDATE OrganTrain SET
		OrganSpharm = @OrganSpharm,
		OrganMeshVertex = @OrganMeshVertex,
		OrganMeshNormal = @OrganMeshNormal,
		OrganMeshFace = @OrganMeshFace,
		OrganSpharmUnit = @OrganSpharmUnit,
		OrganMeshUnit = @OrganMeshUnit,
		HospitalID = @HospitalID,
		PatientID = @PatientID,
		StudyID = @StudyID
		where AutoID = @AutoID
		SELECT @UpdateTime = UpdateTime FROM OrganTrain WHERE AutoID=@AutoID'
	END

	EXECUTE  sp_executesql @sql,N'
	@AutoID INT,
	@OrganSpharm	IMAGE,
	@OrganMeshVertex	IMAGE,
	@OrganMeshNormal	IMAGE,
	@OrganMeshFace	IMAGE,
	@OrganSpharmUnit	NVARCHAR(100),
	@OrganMeshUnit	NVARCHAR(100),
	@HospitalID	INT,
	@PatientID	NVARCHAR(100),
	@StudyID	INT,
	@UpdateTime	TIMESTAMP OUTPUT,
	@DBName NVARCHAR(100)',
	@AutoID,
	@OrganSpharm,
	@OrganMeshVertex,
	@OrganMeshNormal,
	@OrganMeshFace,
	@OrganSpharmUnit,
	@OrganMeshUnit,
	@HospitalID,
	@PatientID,
	@StudyID,
	@UpdateTime OUTPUT,
	@DBName   

	
go
/******************************************************************
* √˚≥∆: OrganTrain_DELETE
* ◊˜’ﬂ: Topslane
*  ±º‰: 2006-11-15 9:21:27
*
* -----------------------------------------------------------------
* ∞Ê±æ		 ±º‰			◊˜’ﬂ		±∏◊¢
*
* V2.00		2006-11-15 9:21:27	LZH		¥¥Ω®
* -----------------------------------------------------------------
******************************************************************/
CREATE  PROCEDURE SP_OrganTrain_DELETE
	 @AutoID	int
	,@DBName NVARCHAR(100),
	@UpdateTime TIMESTAMP,
	@ApplyRowVersion BIT
AS
	SET NOCOUNT ON
	SET XACT_ABORT ON
	DECLARE @Sql NVARCHAR(4000) 
 	SET @Sql='USE '+@DBName
	IF(@ApplyRowVersion=1)
	BEGIN
		SET @Sql=@Sql+' DELETE FROM OrganTrain
		where AutoID = @AutoID AND UpdateTime=@UpdateTime'

		EXECUTE  sp_executesql @sql,N'
		@DBName NVARCHAR(100),
		@AutoID	int,
		@UpdateTime TIMESTAMP',	
		@DBName,
		@AutoID,
		@UpdateTime
	END
	ELSE
	BEGIN
		SET @Sql=@Sql+' DELETE FROM OrganTrain
		where AutoID = @AutoID'

		EXECUTE  sp_executesql @sql,N'
		@DBName NVARCHAR(100),
		@AutoID	int',	
		@DBName,
		@AutoID
	END
	
go

CREATE  PROCEDURE SP_OrganTrain_SELECT
	@DBName NVARCHAR(100)
AS
	DECLARE @Sql NVARCHAR(200)
	SET @Sql ='USE '+@DBName 
	SET @Sql =@Sql +' SELECT * FROM OrganTrain'
	EXEC(@Sql)
go
/******************************************************************
* √˚≥∆: SP_tbImportData_INSERT
* ◊˜’ﬂ: Topslane
*  ±º‰: 2006-11-23 16:07:30
*
* -----------------------------------------------------------------
* ∞Ê±æ		 ±º‰			◊˜’ﬂ		±∏◊¢
*
* V2.00		2006-11-23 16:07:30	LZH		¥¥Ω®
* -----------------------------------------------------------------
******************************************************************/
CREATE  PROCEDURE SP_tbImportData_INSERT
	@AutoID	INT OUTPUT,
	@TPID	BIGINT OUTPUT,
	@SOPUid	NVARCHAR(64),
	@Modality	NVARCHAR(8),
	@SeriesUid	NVARCHAR(64),
	@Data	IMAGE,
	@UpdateTime	TIMESTAMP OUTPUT,
	@DBName NVARCHAR(100)
 AS

	IF(@TPID is Null)
	BEGIN
		SELECT @TPID=-1
	END
	DECLARE @Sql NVARCHAR(1000) 
 	SET @Sql='USE '+@DBName
	SET @Sql=@Sql+' INSERT INTO tbImportData
	(
		TPID,
		SOPUid,
		Modality,
		SeriesUid,
		Data
	)
	VALUES
	(
		@TPID,
		@SOPUid,
		@Modality,
		@SeriesUid,
		@Data
	)
	
	SET @AutoID=SCOPE_IDENTITY()
	
	SELECT @TPID= TPID,@UpdateTime = UpdateTime FROM tbImportData WHERE AutoID=SCOPE_IDENTITY()
	IF (@TPID =-1)
	BEGIN
		SET @TPID=SCOPE_IDENTITY()
		UPDATE tbImportData SET TPID=@TPID  WHERE AutoID=@TPID
	SELECT @UpdateTime = UpdateTime FROM tbImportData WHERE AutoID=SCOPE_IDENTITY()
	END'

	EXECUTE  sp_executesql @sql,N'
	@AutoID	INT OUTPUT,
	@TPID	BIGINT OUTPUT,
	@SOPUid	NVARCHAR(128),
	@Modality	NVARCHAR(16),
	@SeriesUid	NVARCHAR(128),
	@Data	IMAGE,
	@UpdateTime	TIMESTAMP OUTPUT,
	@DBName NVARCHAR(100)',

	@AutoID OUTPUT,
	@TPID OUTPUT,
	@SOPUid,
	@Modality,
	@SeriesUid,
	@Data,
	@UpdateTime OUTPUT,
	@DBName   
	
	
go
/******************************************************************
* √˚≥∆: SP_tbImportData_UPDATE
* ◊˜’ﬂ: Topslane
*  ±º‰: 2006-11-23 16:07:30
*
* -----------------------------------------------------------------
* ∞Ê±æ		 ±º‰			◊˜’ﬂ		±∏◊¢
*
* V2.00		2006-11-23 16:07:30	LZH		¥¥Ω®
* -----------------------------------------------------------------
******************************************************************/
CREATE  PROCEDURE SP_tbImportData_UPDATE
	@TPID	BIGINT,
	@SOPUid	NVARCHAR(64),
	@Modality	NVARCHAR(8),
	@SeriesUid	NVARCHAR(64),
	@Data	IMAGE,
	@UpdateTime	TIMESTAMP OUTPUT,
	@DBName NVARCHAR(100)   ,
	@ApplyRowVersion BIT
 AS 
	SET NOCOUNT ON
	SET XACT_ABORT ON
	DECLARE @Sql NVARCHAR(1000) 
 	SET @Sql='USE '+@DBName
	IF(@ApplyRowVersion=1)
	BEGIN
		SET @Sql=@Sql+' UPDATE tbImportData SET
		TPID = @TPID,
		SOPUid = @SOPUid,
		Modality = @Modality,
		SeriesUid = @SeriesUid,
		Data = @Data
		where TPID = @TPID AND UpdateTime=@UpdateTime
		SELECT @UpdateTime = UpdateTime FROM tbImportData WHERE TPID=@TPID'
	END
	ELSE
	BEGIN
		SET @Sql=@Sql+' UPDATE tbImportData SET
		TPID = @TPID,
		SOPUid = @SOPUid,
		Modality = @Modality,
		SeriesUid = @SeriesUid,
		Data = @Data
		where TPID = @TPID
		SELECT @UpdateTime = UpdateTime FROM tbImportData WHERE TPID=@TPID'
	END

	EXECUTE  sp_executesql @sql,N'
	@TPID	BIGINT,
	@SOPUid	NVARCHAR(128),
	@Modality	NVARCHAR(16),
	@SeriesUid	NVARCHAR(128),
	@Data	IMAGE,
	@UpdateTime	TIMESTAMP OUTPUT,
	@DBName NVARCHAR(100)',

	@TPID,
	@SOPUid,
	@Modality,
	@SeriesUid,
	@Data,
	@UpdateTime OUTPUT,
	@DBName   

	
go
/******************************************************************
* √˚≥∆: tbImportData_DELETE
* ◊˜’ﬂ: Topslane
*  ±º‰: 2006-11-23 16:07:30
*
* -----------------------------------------------------------------
* ∞Ê±æ		 ±º‰			◊˜’ﬂ		±∏◊¢
*
* V2.00		2006-11-23 16:07:30	LZH		¥¥Ω®
* -----------------------------------------------------------------
******************************************************************/
CREATE  PROCEDURE SP_tbImportData_DELETE
	 @TPID	bigint
	,@DBName NVARCHAR(100),
	@UpdateTime TIMESTAMP,
	@ApplyRowVersion BIT
AS
	SET NOCOUNT ON
	SET XACT_ABORT ON
	DECLARE @Sql NVARCHAR(1000) 
 	SET @Sql='USE '+@DBName
	IF(@ApplyRowVersion=1)
	BEGIN
		SET @Sql=@Sql+' DELETE FROM tbImportData
		where TPID = @TPID AND UpdateTime=@UpdateTime'

		EXECUTE  sp_executesql @sql,N'
		@DBName NVARCHAR(100),
		@TPID	bigint,
		@UpdateTime TIMESTAMP',	
		@DBName,
		@TPID,
		@UpdateTime
	END
	ELSE
	BEGIN
		SET @Sql=@Sql+' DELETE FROM tbImportData
		where TPID = @TPID'

		EXECUTE  sp_executesql @sql,N'
		@DBName NVARCHAR(100),
		@TPID	bigint',	
		@DBName,
		@TPID
	END
	
go

CREATE  PROCEDURE SP_tbImportData_SELECT
	@DBName NVARCHAR(100)
AS
	DECLARE @Sql NVARCHAR(200)
	SET @Sql ='USE '+@DBName 
	SET @Sql =@Sql +' SELECT * FROM tbImportData'
	EXEC(@Sql)
go
 
CREATE PROCEDURE [dbo].[SP_GetFirstAndLastPos] 
	@StudyID NVARCHAR(20),
    @dbName NVARCHAR(50)
AS
BEGIN    
    DECLARE @SQL NVARCHAR(MAX)
    CREATE TABLE ##PhaseTable --NEW FOR DELETE NeedLESS CT
	(
        Phase NVARCHAR(10)
	)
	SET @SQL ='USE '+@dbName+' INSERT INTO ##PhaseTable(Phase) SELECT DISTINCT Phase FROM tbSliceImage'
    EXEC(@SQL)
    DECLARE @OnePhase NVARCHAR(10) --NEW FOR DELETE NeedLESS CT
	DECLARE @OnePhaseMaxPos FLOAT
	DECLARE @OnePhaseMinPos FLOAT
	DECLARE @TailPos FLOAT
	DECLARE @HeadPos FLOAT
    SET @TailPos = 9999
	SET @HeadPos = -9999
	DECLARE Phase_Cursor CURSOR FOR
	SELECT Phase FROM ##PhaseTable
	OPEN Phase_Cursor
	FETCH NEXT FROM Phase_Cursor INTO @OnePhase
	WHILE @@FETCH_STATUS = 0
	BEGIN
		SET @SQL = 'USE '+@dbName+' SELECT TOP 1 @OnePhaseMinPos = SlicePositionmm FROM tbSliceImage WHERE Phase = @OnePhase ORDER BY SlicePositionmm ASC'
		EXECUTE  sp_executesql @SQL,N'@dbName NVARCHAR(50),@OnePhaseMinPos FLOAT OUTPUT,@OnePhase NVARCHAR(10)',@dbName,@OnePhaseMinPos OUTPUT,@OnePhase
		SET @SQL = 'USE '+@dbName+' SELECT TOP 1 @OnePhaseMaxPos = SlicePositionmm FROM tbSliceImage WHERE Phase = @OnePhase ORDER BY SlicePositionmm DESC'
		EXECUTE  sp_executesql @SQL,N'@dbName NVARCHAR(50),@OnePhaseMaxPos FLOAT OUTPUT,@OnePhase NVARCHAR(10)',@dbName,@OnePhaseMaxPos OUTPUT,@OnePhase
		IF((@HeadPos<@OnePhaseMinPos) AND ((@OnePhaseMinPos-@HeadPos)>0.01))
        BEGIN
			SET @HeadPos = @OnePhaseMinPos
        END
		IF((@TailPos>@OnePhaseMaxPos) AND ((@TailPos-@OnePhaseMaxPos)>0.01))
        BEGIN
			SET @TailPos = @OnePhaseMaxPos
        END
		FETCH NEXT FROM Phase_Cursor INTO @OnePhase
	END
	CLOSE Phase_Cursor;
	DEALLOCATE Phase_Cursor

    DROP TABLE ##PhaseTable

    SELECT @HeadPos,@TailPos

--    SET @HeadPos = @HeadPos - 0.01;
--    SET @TailPos = @TailPos + 0.01;
--
--    SET  @SQL = 'USE @dbName UPDATE tbSliceImage SET Interpolate = (CASE WHEN (Interpolate IS NULL OR Interpolate = 0) THEN 2 ELSE 3 END) WHERE SlicePositionmm < @HeadPos OR SlicePositionmm > @TailPos'
--    EXECUTE  sp_executesql @SQL,N'@dbName NVARCHAR(50),@HeadPos FLOAT,@TailPos FLOAT',@dbName,@HeadPos,@TailPos
END

go

CREATE PROCEDURE [dbo].[SP_GetPatientInfo] 
     @dbName NVARCHAR(100)
AS
BEGIN
	DECLARE @SQL NVARCHAR(MAX)
    SET @SQL = N'USE '+@dbName+' '
    SET @SQL = @SQL + N'
    SELECT TPID, PatientID, Name, Sex, Age, Room, Bed, 
           RegisterDate, DoctorName, Note, PatientPhoto
    FROM Patient'

    EXEC(@SQL)
END

go