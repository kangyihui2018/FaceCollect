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
using System.Windows.Shapes;

namespace FaceCollect
{
    /// <summary>
    /// WindowEdit.xaml 的交互逻辑
    /// </summary>
    public partial class PersonWindow  : BlankWindow
    {
        PersonWindowViewModel vm;
        public PersonWindow()
        {
            InitializeComponent();
            this.vm = new PersonWindowViewModel();
            this.vm.OnInitialize(this);
            this.DataContext = this.vm;
        }

        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Image ctl = sender as Image;
            this.vm.OnDoubleClick(ctl.Tag);
        }
    }
}
