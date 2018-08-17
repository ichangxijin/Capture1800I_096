IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'AutoFunction')
	BEGIN
	ALTER DATABASE [AutoFunction] set OffLine with rollback after 0
	ALTER DATABASE [AutoFunction] set OnLine
	DROP DATABASE [AutoFunction]
	END

CREATE DATABASE [AutoFunction]
 COLLATE Chinese_PRC_CI_AS

exec sp_dboption N'AutoFunction', N'autoclose', N'false'

exec sp_dboption N'AutoFunction', N'bulkcopy', N'false'

exec sp_dboption N'AutoFunction', N'trunc. log', N'false'

exec sp_dboption N'AutoFunction', N'torn page detection', N'true'

exec sp_dboption N'AutoFunction', N'read only', N'false'

exec sp_dboption N'AutoFunction', N'dbo use', N'false'

exec sp_dboption N'AutoFunction', N'single', N'false'

exec sp_dboption N'AutoFunction', N'autoshrink', N'false'

exec sp_dboption N'AutoFunction', N'ANSI null default', N'false'

exec sp_dboption N'AutoFunction', N'recursive triggers', N'false'

exec sp_dboption N'AutoFunction', N'ANSI nulls', N'false'

exec sp_dboption N'AutoFunction', N'concat null yields null', N'false'

exec sp_dboption N'AutoFunction', N'cursor close on commit', N'false'

exec sp_dboption N'AutoFunction', N'default to local cursor', N'false'

exec sp_dboption N'AutoFunction', N'quoted identifier', N'false'

exec sp_dboption N'AutoFunction', N'ANSI warnings', N'false'

exec sp_dboption N'AutoFunction', N'auto create statistics', N'true'

exec sp_dboption N'AutoFunction', N'auto update statistics', N'true'

