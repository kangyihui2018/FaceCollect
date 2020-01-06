using FaceCollect.Entity;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceCollect.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            //var oldPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FacePic2");
            //var newPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "FacePic");
            //var files = Directory.CreateDirectory(oldPath).GetFiles("*.jpg");
            //for (int i = 0; i < files.Length; i++)
            //{
            //    var bmp = Image.FromFile(files[i].FullName) as Bitmap;
            //    var bp2 = KiResizeImage(bmp, 604, 900);
            //    if (bp2 == null)
            //    {
            //        SystemTools.PrintErrorInfo(files[i].Name);
            //    }
            //    var fileName = (i + 1).ToString().PadLeft(3, '0') + ".jpg";
            //    bp2.Save(Path.Combine(newPath, fileName));
            //    bmp.Dispose();
            //    bp2.Dispose();
            //}

            PersonStorage.Init();
            RasAssist.OpenHost<IFaceCollect, PersonSevice>();

            while (true)
            {
                Console.WriteLine("人员信息采集程序运行中……，请勿关闭。");
                Console.Read();
            }
        }


        public static Bitmap KiResizeImage(Bitmap bmp, int newW, int newH)
        {
            try
            {
                Bitmap b = new Bitmap(newW, newH);
                Graphics g = Graphics.FromImage(b);
                // 插值算法的质量 
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.DrawImage(bmp,
                    new Rectangle(0, 0, newW, newH),
                    new Rectangle(0, 0, bmp.Width, bmp.Height),
                    GraphicsUnit.Pixel);
                g.Dispose();
                return b;
            }
            catch(Exception ex)
            {
                return null;
            }

        }
    }
    }
