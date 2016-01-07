using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using SongApplicationService.util.DataAccess;
using System.Data;
using System.Configuration;
using System.Web.Caching;
using System.Runtime.Caching;


namespace SongApplicationService
{
    [ServiceBehavior(ConcurrencyMode=ConcurrencyMode.Multiple, InstanceContextMode=InstanceContextMode.Single)]
    public class SongService : ISongService
    {
        [WebGet]
        public List<AlbumInfo_ReturnType> GetAlbumInformation(string albumName)
        {
            AlbumInfo_ReturnType albumRC = new AlbumInfo_ReturnType();
            List<AlbumInfo_ReturnType> list = new List<AlbumInfo_ReturnType>();

            try
            {
                ObjectCache cache = MemoryCache.Default;

                if (!string.IsNullOrEmpty(albumName))
                {
                    albumName = albumName.Trim();
                    albumRC.AlbumName = albumName;
                }
                else
                    throw new Exception(@"Missing AlbumName value.");
                if (cache[albumName] != null && cache.Contains(albumName))
                {
                    list = (List<AlbumInfo_ReturnType>)cache.Get(albumName);
                }
                else
                {
                    list = manageAlbumInfo(albumName, "album");
                    if (list.Count > 0)
                    {
                        CacheItemPolicy cachePolicy = new CacheItemPolicy();
                        cachePolicy.AbsoluteExpiration = DateTime.Now.AddHours(1);
                        cache.Add(albumName, list, cachePolicy);
                    }
                }
            }
            catch (Exception e)
            {
                albumRC.Status = -1;
                albumRC.StatusDescription = @"Excetion::" + e.TargetSite + " - " + e.Message;
            }
            return list;
        }

        public SongApplication_ReturnType AddSongToAlbum(string albumName, string albumId, string songName, string songLength, string trackNumber, string genre, ref string songId)
        {
            SongApplication_ReturnType resultRC = new SongApplication_ReturnType();
            string tagName = @"song";
            string excMsg = string.Empty;

            try
            {
                if (!string.IsNullOrEmpty(albumName))
                    albumName = albumName.Trim();
                if (!string.IsNullOrEmpty(songName))
                    songName = songName.Trim();
                if (!string.IsNullOrEmpty(genre))
                    genre = genre.Trim();

                if (string.IsNullOrEmpty(albumName))
                    excMsg = @"Missing Album Name value.";
                else if (string.IsNullOrEmpty(songName))
                    excMsg = @"Missing Song's Title value.";
                else if (string.IsNullOrEmpty(genre))
                    excMsg = @"Missing Genre value.";

                if (!string.IsNullOrEmpty(excMsg))
                    throw new Exception(excMsg);

                resultRC = manageSongRecord(albumName, songName, songLength, trackNumber, genre, tagName, @"insert", ref songId, albumId);
            }
            catch (Exception e)
            {
                resultRC.Status = -1;
                resultRC.StatusDescription = @"Excetion::" + e.TargetSite + " - " + e.Message;
            }
            return resultRC;
        }

        public SongApplication_ReturnType UpdateSongInfo(string songId, string albumId, string songName, string albumName, string songLength, string trackNumber, string genre)
        {
            SongApplication_ReturnType resultRC = new SongApplication_ReturnType();
            string excMsg = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(songId.Trim()))
                    excMsg = @"Missing SongId value.";
                else if (string.IsNullOrEmpty(albumId.Trim()))
                    excMsg = @"Missing albumId vlaue.";
                if (!string.IsNullOrEmpty(excMsg))
                    throw new Exception(excMsg);

                resultRC = manageSongRecord(albumName, songName, songLength, trackNumber, genre, @"song", @"update", ref songId, albumId);

            }
            catch (Exception e)
            {
                resultRC.Status = -1;
                resultRC.StatusDescription = @"Excetion::" + e.TargetSite + " - " + e.Message;
            }
            return resultRC;
        }

        public SongApplication_ReturnType RemoveSongInfo(string albumId, string songId, string songName)
        {
            SongApplication_ReturnType resultRC = new SongApplication_ReturnType();
            string excMsg = string.Empty;

            try
            {
                if (string.IsNullOrEmpty(songId.Trim()))
                    excMsg = @"Missing SongId value.";
                else if (string.IsNullOrEmpty(albumId.Trim()))
                    excMsg = @"Missing albumId vlaue.";
                if (!string.IsNullOrEmpty(excMsg))
                    throw new Exception(excMsg);

                SongApplicationDataAccess.DeleteXMLSongInfo(albumId, songId, songName);
                resultRC.Status = 0;
                resultRC.StatusDescription = @"";
            }
            catch (Exception e)
            {
                resultRC.Status = -1;
                resultRC.StatusDescription = @"Excetion::" + e.TargetSite + " - " + e.Message;
            }
            return resultRC;
        }

        private static List<AlbumInfo_ReturnType> manageAlbumInfo(string albumName, string tagName)
        {
            AlbumInfo_ReturnType dataRC = null;
            DataTable data = new DataTable();
            DataSet dataSet = new DataSet();
            string artist = string.Empty;
            string connectDB = string.Empty;
            List<AlbumInfo_ReturnType> list = new List<AlbumInfo_ReturnType>();

            try
            {
                if ((Convert.ToDateTime(ConfigurationManager.AppSettings[@"recentXMLDataSourceModify"].ToString()) - (Convert.ToDateTime(ConfigurationManager.AppSettings[@"recentSQLDataSourceModify"].ToString()))).TotalMinutes < 0)
                {
                    //sql
                    if (ConfigurationManager.AppSettings[@"connectionString"] != null)
                        connectDB = Convert.ToString(ConfigurationManager.AppSettings[@"connectionString"]);
                    dataSet = SongApplicationDataAccess.GetDataSet(0, connectDB, albumName);
                    if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows != null && dataSet.Tables[0].Rows.Count > 0)
                    {
                        data = dataSet.Tables[0];
                    }
                }
                else
                {
                    data = SongApplicationDataAccess.GetXMLAlbumData(albumName, tagName, ref artist);
                }
                
                if (data != null && data.Rows.Count > 0)
                {
                    for (int rec = 0; rec < data.Rows.Count; rec++)
                    {
                        dataRC = new AlbumInfo_ReturnType();
                        dataRC.AlbumId = (!Convert.IsDBNull(data.Rows[rec][@"albumId"])) ? Convert.ToInt32(data.Rows[rec][@"albumId"]) : 0;
                        dataRC.SongId = (!Convert.IsDBNull(data.Rows[rec][@"songId"])) ? Convert.ToInt32(data.Rows[rec][@"songId"]) : 0;
                        dataRC.AlbumName = (!Convert.IsDBNull(data.Rows[rec][@"albumName"])) ? Convert.ToString(data.Rows[rec][@"albumName"]) : string.Empty;
                        dataRC.Artist = (!Convert.IsDBNull(data.Rows[rec][@"artistName"])) ? Convert.ToString(data.Rows[rec][@"artistName"]) : string.Empty;
                        dataRC.SongName = (!Convert.IsDBNull(data.Rows[rec][@"songName"])) ? Convert.ToString(data.Rows[rec].ItemArray[data.Columns[@"songName"].Ordinal]) : string.Empty;
                        dataRC.Length = (!Convert.IsDBNull(data.Rows[rec][@"length"])) ? Convert.ToString(data.Rows[rec][@"length"]) : string.Empty;
                        dataRC.TrackNumber = (!Convert.IsDBNull(data.Rows[rec][@"trackNumber"])) ? Convert.ToString(data.Rows[rec][@"trackNumber"]) : string.Empty;
                        dataRC.Genre = (!Convert.IsDBNull(data.Rows[rec][@"genre"])) ? Convert.ToString(data.Rows[rec][@"genre"]) : string.Empty;
                        if(dataRC != null)
                            list.Add(dataRC);
                    }
                    dataRC.Status = 0;
                }
                else
                {
                    dataRC.Status = 1;
                    dataRC.StatusDescription = @"No Records where found for album named - " + albumName;
                }
            }
            catch (Exception e)
            {
                dataRC.Status = -1;
                dataRC.StatusDescription = @"Excetion::" + e.TargetSite + " - " + e.Message;
                list.Clear();
                list.Add(dataRC);
            }
            return list;
        }
        private static SongApplication_ReturnType manageSongRecord(string albumName, string songName, string songLength, string trackNumber, string genre, string tagName, string action, ref string songId, string albumId)
        {
            SongApplication_ReturnType dataRC = new SongApplication_ReturnType();
            List<AlbumInfo_ReturnType> updateData = new List<AlbumInfo_ReturnType>();
            
            AlbumInfo_ReturnType record = new AlbumInfo_ReturnType();
            DataSet dataSet = new DataSet();
            string connectDB = string.Empty;
            Random rand = new Random(); //0 = SQL, 1 = XML

            try
            {
                if (ConfigurationManager.AppSettings[@"connectionString"] != null)
                    connectDB = Convert.ToString(ConfigurationManager.AppSettings[@"connectionString"]);
                ObjectCache updateCache = MemoryCache.Default;

                if (action.Equals(@"update"))
                {
                    if (rand.Next(2) == 0)
                    {
                        //sql
                        dataSet = SongApplicationDataAccess.GetDataSet(2, connectDB, albumId, songId, songName, songLength, trackNumber, genre);
                        dataRC.StatusDescription = "Update Song was successful.";
                        dataRC.Status = 0;
                    }
                    else
                    {
                        //xml
                        SongApplicationDataAccess.UpdateXMLSongInfo(songId, albumId, songName, songLength, trackNumber, genre, tagName);
                        dataRC.Status = 0;
                        dataRC.StatusDescription = "Update request was successful";
                    }
                    if (updateCache.Contains(albumName))
                    {
                        updateData = (List<AlbumInfo_ReturnType>)updateCache.Get(albumName);
                        if (updateData != null && updateData.Count > 0)
                        {
                            for (int i = 0; i < updateData.Count; i++)
                            {
                                if (updateData[i].SongId.ToString().Equals(songId) && updateData[i].AlbumId.ToString().Equals(albumId))
                                {
                                    updateData[i].SongName = songName;
                                    updateData[i].Length = songLength;
                                    updateData[i].TrackNumber = trackNumber;
                                    updateData[i].Genre = genre;
                                }
                            }
                        }
                        CacheItemPolicy cachePolicy = new CacheItemPolicy();
                        cachePolicy.AbsoluteExpiration = DateTime.Now.AddHours(1);
                        updateCache.Set(albumName, updateData, cachePolicy);
                    }
                }
                else if (action.Equals(@"insert"))
                {
                    if (rand.Next(2) == 0)
                    {
                        //sql
                        dataSet = SongApplicationDataAccess.GetDataSet(1, connectDB, albumId, albumName, songName, songLength, trackNumber, genre);
                        if (dataSet != null && dataSet.Tables != null && dataSet.Tables.Count > 0 && dataSet.Tables[0].Rows != null && dataSet.Tables[0].Rows.Count > 0)
                        {
                            if (Convert.IsDBNull(dataSet.Tables[0].Rows[0][@"SongId"]) != null)
                                songId = Convert.IsDBNull(dataSet.Tables[0].Rows[0][@"SongId"]).ToString();
                        }
                    }
                    else
                    {
                        //xml
                        SongApplicationDataAccess.insertXMLSongInfo(albumName, albumId, songName, songLength, trackNumber, genre, tagName, ref songId);
                        if (string.IsNullOrEmpty(songId))
                        {
                            dataRC.StatusDescription = "Failed to insert record in XML Data source. This can be due to albumn record does not exsits.";
                            dataRC.Status = -1;
                        }
                    }
                    if (string.IsNullOrEmpty(songId))
                    {
                        dataRC.StatusDescription = "Failed to insert song.";
                        dataRC.Status = -1;
                    }
                    else
                    {
                        dataRC.StatusDescription = "Tnsert song request was succesful.";
                        dataRC.Status = 0;
                        if (updateCache.Contains(albumName))
                        {
                            updateData = (List<AlbumInfo_ReturnType>)updateCache.Get(albumName);
                            if (updateCache != null)
                            {
                                record.AlbumId = Convert.ToInt32(albumId);
                                record.AlbumName = albumName;
                                record.SongId = Convert.ToInt32(songId);
                                record.SongName = songName;
                                record.Length = songLength;
                                record.TrackNumber = trackNumber;
                                record.Genre = genre;
                                updateData.Add(record);
                            }
                            CacheItemPolicy cachePolicy = new CacheItemPolicy();
                            cachePolicy.AbsoluteExpiration = DateTime.Now.AddHours(1);
                            updateCache.Set(albumName, updateData, cachePolicy);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                dataRC.Status = -1;
                dataRC.StatusDescription = @"Excetion::" + e.TargetSite + " - " + e.Message;
            }
            return dataRC;
        }
    }
}
