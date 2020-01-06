using FaceCollect.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;

namespace FaceCollect.ViewModels
{
    public class PersonWindowViewModel : ViewModelBase
    {
        public RelayCommand SaveCommand { get; private set; }

        public RelayCommand PreviousPageCommand { get; private set; }

        public RelayCommand NextPageCommand { get; private set; }

        public RelayCommand CancelCommand { get; private set; }

        public List<String> Jobs { get { return StartWindowViewModel.GetJobs(); } }

        public List<String> Orgs { get { return StartWindowViewModel.Orgs.ToList(); } }

        public ObservableCollection<ImageItem> Images { get; set; } = new ObservableCollection<ImageItem>();

        public List<int> Nums { get; set; } = new List<int>();

        #region MyRegion
        private string department;
        public string Department
        {
            get { return this.department; }
            set
            {
                this.department = value;
                this.OnPropertyChanged(() => Department);
            }
        }
        private string job;
        public string Job
        {
            get { return this.job; }
            set
            {
                this.job = value;
                this.OnPropertyChanged(() => Job);
            }
        }
        private string name;
        public string Name
        {
            get { return this.name; }
            set
            {
                this.name = value;
                this.OnPropertyChanged(() => Name);
            }
        }
        private string phoneNum;
        public string PhoneNum
        {
            get { return this.phoneNum; }
            set
            {
                this.phoneNum = value;
                this.OnPropertyChanged(() => PhoneNum);
            }
        }
        private string filename;
        public string FacePicFileName
        {
            get { return this.filename; }
            set
            {
                this.filename = value;
                this.OnPropertyChanged(() => FacePicFileName);
            }
        }
        private string certificatedId;
        public string CertificateId
        {
            get { return this.certificatedId; }
            set
            {
                this.certificatedId = value;
                this.OnPropertyChanged(() => CertificateId);
            }
        }

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

        private int index;
        public int Index
        {
            get { return this.index; }
            set
            {
                this.index = value;
                this.OnPropertyChanged(()=>Index);
                this.OnLoad();
            }
        }
        #endregion

        const int PAGECOUNT = 12;

        public RelayCommand ViewDoubleClickCommand { get; set; }

        public PersonWindowViewModel()
        {
            this.SaveCommand = new RelayCommand(this.OnSave);
            this.OnLoad();
            this.GetImages();
            this.PreviousPageCommand = new RelayCommand(this.OnPreviousPage,(e)=>this.Index!=0);
            this.NextPageCommand = new RelayCommand(this.OnNextPage, e => this.Index != this.Nums.Last()-1);
            this.CancelCommand = new RelayCommand((ee) => (this.mainScope as Window).Close());
            this.filename = "";
            this.facePic = ImageManager.GetDefaultFace("defaultRect.png");
            this.ViewDoubleClickCommand = new RelayCommand(this.OnDoubleClick);
        }

        private void OnPreviousPage(object obj)
        {
            this.Index--;
        }

        private void OnNextPage(object obj)
        {
            this.Index++;
        }

        public void OnDoubleClick(object obj)
        {
            if (obj == null) return;
            ImageItem item = obj as ImageItem;
            if (item == null) return;
            this.FacePicFileName = item.FileName;
            this.FacePic = item.FacePic;
        }

        private void OnSave(object obj)
        {
            if (string.IsNullOrEmpty(this.name))
            {
                Plus.CSS.PromptHelper.Show("姓名必须填写。");
                return;
            }

            if (string.IsNullOrEmpty(this.department))
            {
                Plus.CSS.PromptHelper.Show("请选择部门。");
                return;
            }

            if (string.IsNullOrEmpty(this.job))
            {
                Plus.CSS.PromptHelper.Show("请选择职务。");
                return;
            }

            if (string.IsNullOrEmpty(this.certificatedId))
            {
                Plus.CSS.PromptHelper.Show("请填写证件号。");
                return;
            }

            if (string.IsNullOrEmpty(this.phoneNum))
            {
                Plus.CSS.PromptHelper.Show("请填写手机号。");
                return;
            }

            if (string.IsNullOrEmpty(this.filename))
            {
                Plus.CSS.PromptHelper.Show("请选择照片。");
                return;
            }

            Window owner = mainScope as Window;
            if (owner == null) return;
            owner.DialogResult = true;
        }

        private void OnLoad()
        {
            this.GetImages();
            Nums.Clear();
            var count = RasAssist.CallRemoteService<IFaceCollect, int>(ee => ee.GetPicCount(true));
            int num = count / PAGECOUNT;
            int ys = count % PAGECOUNT;
            if (count == 0) return;
            if (ys > 0) num++;
            for (int i = 1; i <= num; i++)
            {
                Nums.Add(i);
            }
        }

        private void GetImages()
        {
            var temp = this.Images.ToArray();
            this.Images.Clear();
           
            int startIndex = this.index * PAGECOUNT;
            int endIndex = (this.index + 1) * PAGECOUNT;
            int maxCount = 6;
            int tempEndIndex = endIndex - startIndex > maxCount ? startIndex + maxCount : endIndex;
            var imgs = RasAssist.CallRemoteService<IFaceCollect, ImageInfo[]>(ee => ee.GetFace(startIndex, tempEndIndex, true));
           
            while (imgs != null)
            {
                if (imgs.Length == 0) break;
                foreach (var item in imgs)
                {
                    ImageItem img = new ImageItem();
                    img.FileName = item.FileName;
                    img.FacePic = ImageManager.BitmapToBitmapSource(item.ImageData);
                    this.Images.Add(img);
                }
                startIndex += maxCount;
                tempEndIndex = endIndex - startIndex > maxCount ? startIndex + maxCount : endIndex;
                if (startIndex >= endIndex) break;
                imgs = RasAssist.CallRemoteService<IFaceCollect, ImageInfo[]>(ee => ee.GetFace(startIndex, tempEndIndex, true));
            }
        }
    }
}
