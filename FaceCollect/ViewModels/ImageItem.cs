using FaceCollect.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace FaceCollect.ViewModels
{
    public class ImageItem : NotifyPropertyChanged
    {
        public string FileName { get; set; }

        public byte[] ImageData { get; set; }

        private BitmapSource facePic = null;
        public BitmapSource FacePic
        {
            get { return this.facePic; }
            set
            {
                this.facePic = value;
                this.OnPropertyChanged(() => FacePic);
            }
        }

        public string Name { get; set; }

        public int Job { get; set; }

        public Person Person { get; set; }
    }
}
