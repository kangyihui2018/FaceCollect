using FaceCollect.Entity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;


namespace FaceCollect
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {

        protected override void OnStartup(StartupEventArgs e)
        {
           var temp= RasAssist.CallRemoteService<IFaceCollect, string[]>(ee=>ee.GetDepartments());
            base.OnStartup(e);
        }
    }
}
