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
                Console.Read();
            }

            
        }
    }
}
