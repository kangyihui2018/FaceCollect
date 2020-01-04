using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace FaceCollect.Entity
{


    [ServiceContract]
    public interface IFaceCollect
    {
        /// <summary>
        /// 按页获取人脸图片
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [OperationContract]
        List<ImageInfo> GetFace(int pageNo, int pageSize);

        /// <summary>
        /// 获取人脸图片分页信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        int GetFacePageCount(int pageSize);

       /// <summary>
       /// 增加或编辑人员信息
        /// </summary>
       /// <param name="person"></param>
       /// <returns></returns>
        [OperationContract]
        bool AddOrEditPersonInfo(Person  person);

        /// <summary>
        /// 查询人员信息
        /// </summary>
        /// <param name="department"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        /// <returns></returns>
        [OperationContract]
        Person GetPersonInfo(string department, string name, string phone);
    }
}
