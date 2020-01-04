using System.Collections.Generic;
using FaceCollect.Entity;

namespace FaceCollect.Server
{
    class PersonSevice : FaceCollect.Entity.IFaceCollect
    {
        public bool AddOrEditPersonInfo(Person person)
        {
            var ret = PersonStorage.AddOrEdite(person);
            return ret;
        }

        public List<string> GetDepartments()
        {
            return new List<string>()
            {
                "院领导",
                "检委会",
                "政治部",
                "监察部",
                "院办公室",
                "第一检察部",
                "第二检察部",
                "第三检察部",
                "第四检察部",
                "第五检察部",
                "综合业务部",
                "法警大队",
                "罕台检察室",
                "基层巡回检察室"
            };
        }

        public List<ImageInfo> GetFace(int pageNo, int pageSize)
        {
            var files = PersonStorage.DirFacePic.GetFiles("*.jpg");
            var start = pageNo * pageSize;
            var end = start + pageSize;
            end = end > files.Length ? files.Length : end;
            List<ImageInfo> ret = new List<ImageInfo>();
            for (int i = start; i < end; i++)
            {
                var fileInfo = files[i];
                var imageInfo=   PersonStorage.GetImageInfoByName(fileInfo.FullName);
                ret.Add(imageInfo);
            }
            return ret;
        }

        public int GetFacePageCount(int pageSize)
        {
            var files = PersonStorage.DirFacePic.GetFiles("*.jpg");
            if (pageSize <= 0) return files.Length;
            var temp = files.Length / pageSize;
            return files.Length % pageSize == 0 ? temp : pageSize + 1;
        }

        public List<string> GetJobs()
        {
            return new List<string>();
        }

        public Person GetPersonInfo(string certificateId,string phoneNum)
        {
            var temp= PersonStorage.GetPersons(e => e.CertificateId == certificateId || e.PhoneNum == phoneNum);
            if (temp.Count == 0) return null;
            return temp[0];
          
        }
    }
}
