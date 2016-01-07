using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using SongInventoryApplication_Client.SongServiceReference;
using System.Web.Script.Serialization;
using System.Web.Services;
using System.Data;

namespace SongInventoryApplication_Client.Pages
{
    public partial class main : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }
        [WebMethod]
        public static List<AlbumInfo_ReturnType> GetAlbumData(string albumName)
        {
            SongServiceClient song = new SongServiceClient();
            //AlbumInfo_ReturnType albumRC = new AlbumInfo_ReturnType();
            List<AlbumInfo_ReturnType> dataSet = new List<AlbumInfo_ReturnType>();

            dataSet = song.GetAlbumInformation(albumName);
            //string json = new JavaScriptSerializer().Serialize(dataSet);
            return dataSet;
        }
        [WebMethod]
        public static string UpdateSongData(string albumId, string songId, string albumName, string songName, string length, string trackNumber, string genre)
        {
            SongServiceClient song = new SongServiceClient();
            SongApplication_ReturnType songRC = new SongApplication_ReturnType();

            songRC = song.UpdateSongInfo(songId, albumId, songName, albumName, length, trackNumber, genre);
            return songRC.Status.ToString();
        }
        [WebMethod]
        public static List<string> AddSongRecord(string albumId, string albumName, string songName, string length, string trackNumber, string genre)
        {
            SongServiceClient song = new SongServiceClient();
            SongApplication_ReturnType songRC = new SongApplication_ReturnType();
            List<string> id = new List<string>();
            string songId = string.Empty;

            songRC = song.AddSongToAlbum(albumName, albumId, songName, length, trackNumber, genre, ref songId);
            id.Add(songId);

            return id;
        }
    }
}