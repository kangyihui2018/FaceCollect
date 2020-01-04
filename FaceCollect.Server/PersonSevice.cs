using System.Collections.Generic;
using System.IO;
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

        public Person[] GetAllPerson()
        {
           return PersonStorage.GetPersons(e=> { return true; }).ToArray();
        }

        public string[] GetDepartments()
        {
            var temp= File.ReadAllText("departments.txt");
            var str = System.Environment.NewLine;
            return temp.Split(new string[] { str }, System.StringSplitOptions.RemoveEmptyEntries);
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

        public string[] GetJobs()
        {
            var temp = File.ReadAllText("jobs.txt");
            var str = System.Environment.NewLine;
            return temp.Split(new string[] { str }, System.StringSplitOptions.RemoveEmptyEntries);
        }

        public Person GetPersonInfo(string certificateId,string phoneNum)
        {
            var temp= PersonStorage.GetPersons(e => e.CertificateId == certificateId || e.PhoneNum == phoneNum);
            if (temp.Count == 0) return null;
            return temp[0];
          
        }
    }
}
