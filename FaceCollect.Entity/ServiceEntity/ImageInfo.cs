using System.Runtime.Serialization;

namespace FaceCollect.Entity
{


    [DataContract]
    /// <summary>
    /// 照片信息
    /// </summary>
    public class ImageInfo
    {
        public string FileName { get; set; }

        public byte[] ImageData { get; set; }
    }
}
