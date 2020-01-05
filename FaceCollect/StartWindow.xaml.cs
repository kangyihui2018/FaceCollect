using FaceCollect.ViewModels;
using Plus.CSS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FaceCollect
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class StartWindow : BlankWindow
    {
        StartWindowViewModel vm;
        public StartWindow()
        {
            InitializeComponent();
            this.vm = new StartWindowViewModel();
            this.DataContext = this.vm;
            this.OnLoadSystemButton();
        }
    }
}
