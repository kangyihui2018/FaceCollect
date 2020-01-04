using System.Runtime.Serialization;

namespace FaceCollect.Entity
{
    [DataContract]
    /// <summary>
    /// 人员信息
    /// </summary>
    public class Person
    {
        public string Department { get; set; }

        public string Job { get; set; }

        public string Name { get; set; }

        public string PhoneNum { get; set; }

        public string FacePicFileName { get; set; }

        public string CertificateId { get; set; }

    }
}
