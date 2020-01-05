using System;
using System.Runtime.Serialization;

namespace FaceCollect.Entity
{



    /// <summary>
    /// 照片信息
    /// </summary>
    [DataContract]
    [Serializable]
    public class ImageInfo
    {
        [DataMember]
        public string FileName { get; set; }

        [DataMember]
        public byte[] ImageData { get; set; }
    }
}
