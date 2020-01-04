using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using FaceCollect.Entity;

namespace FaceCollect.Server
{

    class FacePicHelper
    {
        private static string DirPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FacePic");
        public static DirectoryInfo DirFacePic = Directory.CreateDirectory(DirPath);

        public static ImageInfo GetImageInfoByName(string fullFilePath)
        {
            if (!File.Exists(fullFilePath)) return null;
            ImageInfo info = new ImageInfo();
            info.FileName = Path.GetFileName(fullFilePath);
            info.ImageData = File.ReadAllBytes(fullFilePath);
            return info;
        }
    }
}
