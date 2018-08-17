IF EXISTS (SELECT name FROM master.dbo.sysdatabases WHERE name = N'AutoAccount')
	BEGIN
	ALTER DATABASE [AutoAccount] set OffLine with rollback after 0
	ALTER DATABASE [AutoAccount] set OnLine
	DROP DATABASE [AutoAccount] 
	END

CREATE DATABASE [AutoAccount] 
 COLLATE Chinese_PRC_CI_AS

exec sp_dboption N'AutoAccount', N'autoclose', N'false'

exec sp_dboption N'AutoAccount', N'bulkcopy', N'false'

exec sp_dboption N'AutoAccount', N'trunc. log', N'false'

exec sp_dboption N'AutoAccount', N'torn page detection', N'true'

exec sp_dboption N'AutoAccount', N'read only', N'false'

exec sp_dboption N'AutoAccount', N'dbo use', N'false'

exec sp_dboption N'AutoAccount', N'single', N'false'

exec sp_dboption N'AutoAccount', N'autoshrink', N'false'

exec sp_dboption N'AutoAccount', N'ANSI null default', N'false'

exec sp_dboption N'AutoAccount', N'recursive triggers', N'false'

exec sp_dboption N'AutoAccount', N'ANSI nulls', N'false'

exec sp_dboption N'AutoAccount', N'concat null yields null', N'false'

exec sp_dboption N'AutoAccount', N'cursor close on commit', N'false'

exec sp_dboption N'AutoAccount', N'default to local cursor', N'false'

exec sp_dboption N'AutoAccount', N'quoted identifier', N'false'

exec sp_dboption N'AutoAccount', N'ANSI warnings', N'false'

exec sp_dboption N'AutoAccount', N'auto create statistics', N'true'

exec sp_dboption N'AutoAccount', N'auto update statistics', N'true'

