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
            throw new NotImplementedException();
        }

        public int GetFacePageCount(int pageSize)
        {
            throw new NotImplementedException();
        }

        public Person GetPersonInfo(string department, string name, string phone)
        {
            throw new NotImplementedException();
        }
    }
}
