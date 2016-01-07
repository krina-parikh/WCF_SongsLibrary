README

Time taken fordevleopment:
1. WCF Service:
	DEvelopement - 3hrs
	Testing - 1hr approx.
2. Front UI design:
	Design - 2-3hrs
	DataTable integration with WCF service methods - 2hrs
	
Assumptions:
1. The XML and SQL data-source for song will be synced every 15mins. Hence both the data-source will be having same Data Records.
2. The last update / Insert timestamps for each data-source is maintained in Web.config file (recentXMLDataSourceModify, recentSQLDataSourceModify)
3. For GetAlbumInformation() - which datasource to use is decided by comparing xml and sql timestamp from above point #2.
4. For updateSongInfo() and AddSongToAlbum() - the datasource selection is done by random numer generation
	a. 0 - SQL Server to use.
	b. 1 - XML data to use.
5. For Caching output of WCF service used - ObjectCache / MemoryCache
