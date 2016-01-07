using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;
using System.Web.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

namespace SongApplicationService.util.DataAccess
{
    public class SongApplicationDataAccess
    {
        public static Configuration webConfigUpdate = WebConfigurationManager.OpenWebConfiguration("~");
        public static string[] sqlProcs = {  "EXEC dbo.getAlbumdata {0} ---",    //0
                                              "Exec dbo.insertSongData {0}, {1}, {2}, {3}, {4}, {5} ---",  //1
                                              "EXEC dbo.updateSongData {0}, {1}, {2}, {3}, {4}, {5} ---",   //2
                                              "Exec dbo.deleteSongRecord {0}, {1}, {2} ---" //3
                                          };

        #region XMLpublic
        public static DataTable GetXMLAlbumData(string albumName, string tagName, ref string artist)
        {
            XmlDocument doc = new XmlDocument();
            string name = string.Empty;
            DataTable albumInfo = new DataTable(@"AblumInfo");
            XmlNodeList childNodeList;
            string albumId = string.Empty;
            DataTable albumTblt = new DataTable();

            try
            {
                doc.Load(HttpContext.Current.Server.MapPath("~/DataSource/SongInventory.xml"));
                XmlNodeList nodeList1 = doc.SelectNodes(@"//" + tagName + @"[@Title = '" + albumName + "']");
                if (nodeList1 != null && nodeList1.Count > 0)
                {
                    artist = nodeList1[0].ParentNode.Attributes[@"name"].Value;
                    albumName = nodeList1[0].Attributes[@"Title"].Value;
                    albumId = nodeList1[0].Attributes[@"Id"].Value;
                    childNodeList = nodeList1[0].ChildNodes;

                    albumInfo = createTable(albumInfo, @"songId|albumId|albumName|artistName|songName|length|trackNumber|genre".Split('|'));
                    if (childNodeList != null && childNodeList.Count > 0)
                    {
                        for (int i = 0; i < childNodeList.Count; i++)
                        {
                            albumInfo.Rows.Add(createSongDataRow(albumInfo, childNodeList[i].Attributes[@"SongId"].Value, childNodeList[i].Attributes[@"Title"].Value, childNodeList[i].Attributes[@"Length"].Value, childNodeList[i].Attributes[@"TrackNumber"].Value, childNodeList[i].Attributes[@"Genre"].Value, albumId, albumName, artist));
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
            return albumInfo;
        }
        public static void insertXMLSongInfo(string albumName, string albumId, string songName, string songLength, string trackNumber, string genre, string tagName, ref string songId)
        {
            XmlDocument doc = new XmlDocument();
            XmlNodeList node = null;
            XmlNode insertNode = doc.CreateDocumentFragment();
            int newSongID = 0;
            //int newAlbumId = 0;

            try
            {
                doc.Load(HttpContext.Current.Server.MapPath("~/DataSource/SongInventory.xml"));
                if (doc != null)
                {
                    if (tagName.Equals(@"song"))
                        node = doc.SelectNodes(@"//album" + @"[@Id = '" + albumId + "']");

                    newSongID = ConfigurationManager.AppSettings[@"songLastId"] != null ? Convert.ToInt32(ConfigurationManager.AppSettings[@"songLastId"]) : 0;
                    if (node.Count > 0)
                    {
                        insertNode.InnerXml = @"<" + tagName + " Title = \"" + songName + "\" Length = \"" + songLength + "\" SongId = \"" + newSongID + "\" TrackNumber = \"" + trackNumber + "\" Genre = \"" + genre + "\"/>";
                        node[0].AppendChild(insertNode);
                        songId = newSongID.ToString();
                        webConfigUpdate.AppSettings.Settings[@"songLastId"].Value = Convert.ToString(newSongID+1);
                        webConfigUpdate.AppSettings.Settings["recentXMLDataSourceModify"].Value = DateTime.Now.ToString();
                        webConfigUpdate.Save();
                    }
                    else
                    {
                        songId = "-1";
                    }
                    doc.Save(HttpContext.Current.Server.MapPath("~/DataSource/SongInventory.xml"));
                }
                else
                {
                    throw new Exception("No album exist in the Source File.");
                }
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public static void UpdateXMLSongInfo(string songId, string albumId, string songName, string length, string trackNumber, string genre, string tagName)
        {
            XmlDocument doc = new XmlDocument();
            XmlNodeList node = null;

            try
            {
                doc.Load(HttpContext.Current.Server.MapPath("~/DataSource/SongInventory.xml"));
                if(!string.IsNullOrEmpty(songId))
                    node = doc.SelectNodes(@"//" + tagName + @"[@SongId = '" + songId + "']");
                else if (!string.IsNullOrEmpty(songName))
                    node = doc.SelectNodes(@"//" + tagName + @"[@Title = '" + songName + "']");

                for (int i = 0; i < node.Count; i++)
                {
                    if (node[i].ParentNode.Attributes[@"Id"].Value.Equals(albumId))
                    {
                        node[i].Attributes[@"Title"].Value = string.IsNullOrEmpty(songName) ? node[i].Attributes[@"Title"].Value : songName;
                        node[i].Attributes[@"Genre"].Value = string.IsNullOrEmpty(genre) ? node[i].Attributes[@"Genre"].Value : genre;
                        node[i].Attributes[@"TrackNumber"].Value = string.IsNullOrEmpty(trackNumber) ? node[i].Attributes[@"TrackNumber"].Value : trackNumber;
                        node[i].Attributes[@"Length"].Value = string.IsNullOrEmpty(length) ? node[i].Attributes[@"Length"].Value : length;
                    }
                }
                webConfigUpdate.AppSettings.Settings["recentXMLDataSourceModify"].Value = DateTime.Now.ToString();
                webConfigUpdate.Save();
                doc.Save(HttpContext.Current.Server.MapPath("~/DataSource/SongInventory.xml"));
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        public static void DeleteXMLSongInfo(string albumId, string songId, string songName)
        {
            XmlDocument doc = new XmlDocument();
            XmlNodeList node = null;

            try
            {
                doc.Load(HttpContext.Current.Server.MapPath("~/DataSource/SongInventory.xml"));
                if (!string.IsNullOrEmpty(songId))
                    node = doc.SelectNodes(@"//song" + @"[@SongId = '" + songId + "']");
                else if (!string.IsNullOrEmpty(songName))
                    node = doc.SelectNodes(@"//song" + @"[@Title = '" + songName + "']");

                for (int i = 0; i < node.Count; i++)
                {
                    if (node[i].ParentNode.Attributes[@"Id"].Value.Equals(albumId))
                    {
                        node[i].RemoveAll();
                    }
                }
                webConfigUpdate.AppSettings.Settings["recentXMLDataSourceModify"].Value = DateTime.Now.ToString();
                webConfigUpdate.Save();
                doc.Save(HttpContext.Current.Server.MapPath("~/DataSource/SongInventory.xml"));
            }
            catch (Exception e)
            {
                throw e.InnerException;
            }
        }
        #endregion
        #region SQL-public
        public static DataSet GetDataSet(int index, string strConn, params object[] args)
        {
            SqlConnection dbConn = new SqlConnection(strConn);
            DataSet dataSet = new DataSet();
            string sqlcmd = string.Empty;

            try
            {
                if (index > -1)
                {
                    if (args != null)
                        sqlcmd = string.Format(sqlProcs[index], args);
                    else
                        sqlcmd = sqlProcs[index];
                    
                        dbConn.Open();
                        SqlDataAdapter adapter = new SqlDataAdapter(sqlcmd, dbConn);
                        adapter.Fill(dataSet);
                        if (index != 0)
                        {
                            webConfigUpdate.AppSettings.Settings["recentSQLDataSourceModify"].Value = DateTime.Now.ToString();
                            webConfigUpdate.Save();
                        }
                }
            }
            catch (SqlException e)
            {
                Exception ex = e.InnerException;
                if (ex == null)
                    throw;
                else
                    throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbConn.Close();
            }
            return dataSet;
        }
        #endregion
        #region private
        private static DataRow createAlbumDataRow(DataTable table, string title, string albumId, string artist)
        {
            DataRow dataRow = table.NewRow();
            dataRow[@"albumName"] = title;
            dataRow[@"albumId"] = albumId;
            dataRow[@"artistName"] = artist;

            return dataRow;
        }
        private static DataRow createSongDataRow(DataTable table, string songId, string songName, string length, string trackNumber, string genre, string albumId, string albumName, string artist)
        {
            DataRow dataRow = table.NewRow();

            dataRow[@"songId"] = songId;
            dataRow[@"albumId"] = albumId;
            dataRow[@"songName"] = songName;
            dataRow[@"Length"] = length;
            dataRow[@"TrackNumber"] = trackNumber;
            dataRow[@"Genre"] = genre;
            dataRow[@"albumName"] = albumName;
            dataRow[@"artistName"] = artist;
            
            return dataRow;
        }
        private static DataTable createTable(DataTable table, params object[] columnList)
        {
            try
            {
                if (columnList.Count() > 0)
                {
                    for (int col = 0; col < columnList.Count(); col++)
                    {
                        table.Columns.Add(columnList[col].ToString());
                    }
                }
                else
                {
                    table = new DataTable();
                }
            }
            catch (Exception e)
            {
            }
            return table;
        }
        private static Type typeOfColumn(string type)
        {
            switch(type){
                case @"int":
                    return typeof(int);
                    
                case @"string":
                    return typeof(string);
                    
                case @"float":
                    return typeof(float);
                    
                case @"boolean":
                    return typeof(bool);
                    
                case @"decimal":
                    return typeof(Decimal);

                default:
                    return typeof(string);
            }
        }
        #endregion
    }
}