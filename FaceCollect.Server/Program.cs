using FaceCollect.Entity;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceCollect.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            PersonStorage.Init();
            RasAssist.OpenHost<IFaceCollect, PersonSevice>();     

            while (true)
            {
                Console.WriteLine("人员信息采集程序运行中……，请勿关闭。");
                Console.Read();
            }

            
        }
    }
}
