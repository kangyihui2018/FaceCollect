using System.Collections.Generic;
using System.IO;
using FaceCollect.Entity;
using System.Linq;

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

        public List<ImageInfo> GetFace(int startIndex,int endIndex, bool onlyUnBindPersonPic = false)
        {
            FileInfo[] files = null;
            if (!onlyUnBindPersonPic)
            {
                files = PersonStorage.DirFacePic.GetFiles("*.jpg");
            }
            else
            {
                files = GetUnBindPersonPic();
            }
            if (endIndex > files.Length)
                endIndex = files.Length;
            List<ImageInfo> ret = new List<ImageInfo>();
            for (int i = startIndex; i < endIndex; i++)
            {
                var fileInfo = files[i];
                var imageInfo=   PersonStorage.GetImageInfoByName(fileInfo.FullName);
                ret.Add(imageInfo);
            }
            return ret;
        }

        private FileInfo[] GetUnBindPersonPic()
        {
            var files = PersonStorage.DirFacePic.GetFiles("*.jpg");
            List<FileInfo> infos = new List<FileInfo>();
           var temp= this.GetAllPerson();
            foreach (var item in files)
            {
                if (!temp.Any(e => item.Name == e.FacePicFileName))
                {
                    infos.Add(item);
                }
            }
            return infos.ToArray();
        }

        public int GetPicCount(bool onlyUnBindPersonPic = false)
        {
            if (onlyUnBindPersonPic)
            {
                return GetUnBindPersonPic().Length;
            }
            else
            {
                var files = PersonStorage.DirFacePic.GetFiles("*.jpg");
                return files.Length;
            }
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
