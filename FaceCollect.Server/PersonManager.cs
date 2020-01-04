using System;
using System.Collections.Generic;
using FaceCollect.Entity;

namespace FaceCollect.Server
{
    class PersonManager : FaceCollect.Entity.IFaceCollect
    {
        public bool AddOrEditPersonInfo(Person person)
        {
            throw new NotImplementedException();
        }

        public List<ImageInfo> GetFace(int pageNo, int pageSize)
        {
            var files = FacePicHelper.DirFacePic.GetFiles("*.jpg");
            var start = pageNo * pageSize;
            var end = start + pageSize;
            end = end > files.Length ? files.Length : end;
            List<ImageInfo> ret = new List<ImageInfo>();
            for (int i = start; i < end; i++)
            {
                var fileInfo = files[i];
                var imageInfo=   FacePicHelper.GetImageInfoByName(fileInfo.FullName);
                ret.Add(imageInfo);
            }
            return ret;
        }

        public int GetFacePageCount(int pageSize)
        {
            var files = FacePicHelper.DirFacePic.GetFiles("*.jpg");
            if (pageSize <= 0) return files.Length;
            var temp = files.Length / pageSize;
            return files.Length % pageSize == 0 ? temp : pageSize + 1;
        }

        public Person GetPersonInfo(string department, string name, string phone)
        {
            return null;
           // throw new NotImplementedException();
        }
    }
}
