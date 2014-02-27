USE [DisplayMonkey]
GO
/*******************************************************************
  2013-11-09 [DPA] - DisplayMonkey object
*******************************************************************/
alter procedure dbo.sp_GetLocationDetails
	@displayId int
as begin
	set nocount on;

	declare 
		@lat float
	,	@lng float
	,	@unit char
	,	@locId int
	,	@levelId int
	,	@displayLoc int
	,	@parentLoc int
	,	@dateFmt varchar(20)
	,	@timeFmt varchar(10)
	,	@name nvarchar(100)
	;
	
	select top 1 @locId = LocationId from Display where DisplayId=@displayId;
	
	set @displayLoc = @locId;
	
	while (@lat is null or @lng is null or @unit is null or @dateFmt is null or @timeFmt is null) begin
		select top 1 
			@lat = isnull(@lat,Latitude)
		,	@lng = isnull(@lng,Longitude)
		,	@unit = isnull(@unit,TemperatureUnit)
		,	@parentLoc = AreaId
		,	@dateFmt = isnull(@dateFmt, DateFormat)
		,	@timeFmt = isnull(@timeFmt, TimeFormat)
		,	@name = case when @displayLoc = @locId then Name else @name end
		from Location where LocationId=@locId
		;
		if (@@rowcount=0) break;
		set @locId = @parentLoc;
	end
	
	select 
		LocationId
	,	LevelId
	,	Name = isnull(@name,N'')
	,	Latitude = isnull(@lat,0)
	,	Longitude = isnull(@lng,0)
	,	TemperatureUnit = isnull(@unit,'C')
	,	DateFormat = isnull(@dateFmt,'LL')
	,	TimeFormat = isnull(@timeFmt,'LT')
	,	OffsetGMT
	from Location where LocationId=@displayLoc
	;

end
go