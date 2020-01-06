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
        /// 按索引获取图片
        /// </summary>
        /// <param name="startIndex">起始索引</param>
        /// <param name="endIndex">结束索引</param>
        /// <param name="onlyUnBindPersonPic">是否仅返回未绑定到人员的图片信息</param>
        /// <returns></returns>
        [OperationContract]
        ImageInfo[] GetFace(int startIndex, int endIndex, bool onlyUnBindPersonPic = false);

        /// <summary>
        /// 获取人脸图片数量
        /// </summary>
        /// <param name="onlyUnBindPersonPic">是否仅返回未绑定到人员的图片数量</param>
        /// <returns></returns>
        [OperationContract]
        int GetPicCount(bool onlyUnBindPersonPic = false);

       /// <summary>
       /// 增加或编辑人员信息
       /// </summary>
       /// <param name="person"></param>
       /// <returns></returns>
        [OperationContract]
        bool AddOrEditPersonInfo(Person  person);

        /// <summary>
        /// 输入证件ID或电话号码查询人员信息
        /// </summary>
        /// <param name="certificateId"></param>
        /// <param name="phoneNum"></param>
        /// <returns></returns>
        [OperationContract]
        Person GetPersonInfo(string certificateId, string phoneNum);

        [OperationContract]
        Person[] GetAllPerson();

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string[] GetDepartments();

        /// <summary>
        /// 获取职务列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        string[] GetJobs();

        [OperationContract]
        byte[] GetImageByFileName(string fileName);
    }
}
