using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace SongApplicationService
{
    [ServiceContract]
    public interface ISongService
    {

        [OperationContract]
        List<AlbumInfo_ReturnType> GetAlbumInformation(string albumName);

        [OperationContract]
        SongApplication_ReturnType AddSongToAlbum(string albumName, string albumId, string songName, string songLength, string trackNumber, string genre, ref string songId);

        [OperationContract]
        SongApplication_ReturnType UpdateSongInfo(string songId, string albumId, string songName, string albumName, string songLength, string trackNumber, string genre);

        [OperationContract]
        SongApplication_ReturnType RemoveSongInfo(string albumId, string songId, string songName);

        // TODO: Add your service operations here
    }


    // Use a data contract as illustrated in the sample below to add composite types to service operations.
    [DataContract]
    public class SongApplication_ReturnType
    {
        [DataMember]
        public int Status;
        [DataMember]
        public string StatusDescription;
    }

    [DataContract]
    public class AlbumInfo_ReturnType : SongApplication_ReturnType
    {
        [DataMember]
        public string Artist { get; set; }
        [DataMember]
        public string AlbumName { get; set; }
        [DataMember]
        public string SongName { get; set; }
        [DataMember]
        public string Length { get; set; }
        [DataMember]
        public string TrackNumber { get; set; }
        [DataMember]
        public string Genre { get; set; }
        [DataMember]
        public int SongId { get; set; }
        [DataMember]
        public int AlbumId { get; set; }
    }
    [DataContract]
    public partial class SongInfoRec
    {
        [DataMember]
        public string SongName { get; set; }
        [DataMember]
        public string Length { get; set; }
        [DataMember]
        public string TrackNumber { get; set; }
        [DataMember]
        public string Genre { get; set; }
        [DataMember]
        public int SongId { get; set; }
        [DataMember]
        public int AlbumId { get; set; }
    }
}
