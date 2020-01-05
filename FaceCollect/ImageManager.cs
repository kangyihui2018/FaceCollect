using FaceCollect.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FaceCollect
{
    /// <summary>
    /// 图片管理
    /// </summary>
    public class ImageManager
    {
        public static Dictionary<string, ImageInfo> images = new Dictionary<string, ImageInfo>();
        public static void InitImage()
        {
            //TODO 可能有通讯字节大小的问题
            var imgs = RasAssist.CallRemoteService<IFaceCollect, ImageInfo[]>(ee => ee.GetFace(0, 1000, false));
            foreach (var item in imgs)
            {
                string key = item.FileName;
                if (string.IsNullOrEmpty(key)) continue;

                if (!images.ContainsKey(key)) images.Add(key, item);               
            }
        }

        public static BitmapSource GetImage(string filename)
        {
            if (images.ContainsKey(filename))
            {
                var obj = images[filename];
                return BitmapToBitmapSource(obj.ImageData);
            }

            return GetDefaultFace("defaultRect.png");
        }

        public static BitmapSource BitmapToBitmapSource(byte[] bitmap)
        {

            BitmapImage bitmapImage = new BitmapImage();
            if (bitmap == null) return bitmapImage;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                ms.Write(bitmap, 0, bitmap.Length);
                ms.Position = 0;
                ms.Flush();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = ms;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }

            return bitmapImage;
        }

        public static BitmapSource GetDefaultFace(string filename)
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,  filename);
            var buf = ReadBytesFromFile(path);
            return BitmapToBitmapSource(buf);
        }

        public static byte[] ReadBytesFromFile(string filePath)
        {

            FileStream fs = new FileStream(filePath, FileMode.Open);
            byte[] buffer = new byte[(int)fs.Length];
            int flag = 0;
            while (flag != fs.Length)
            {
                flag += fs.Read(buffer, flag, buffer.Length - flag);
            }
            fs.Close();
            return buffer;
        }
    }
}
