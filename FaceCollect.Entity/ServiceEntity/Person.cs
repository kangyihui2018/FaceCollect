using System.Runtime.Serialization;
using System;
namespace FaceCollect.Entity
{

    /// <summary>
    /// 人员信息
    /// </summary>
    [DataContract]
    [Serializable]
    public class Person
    {
        [DataMember]
        public string Department { get; set; }
        [DataMember]
        public string Job { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string PhoneNum { get; set; }
        [DataMember]
        public string FacePicFileName { get; set; }
        [DataMember]
        public string CertificateId { get; set; }

    }
}
