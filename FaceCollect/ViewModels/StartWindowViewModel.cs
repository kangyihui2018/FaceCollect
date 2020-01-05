using FaceCollect.Entity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FaceCollect.ViewModels
{
    public class StartWindowViewModel : NotifyPropertyChanged
    {
        public List<NavBindingItem> Navs { get; set; }

        public RelayCommand OnClickCommand { get; private set; }
        public RelayCommand AddCommand { get; private set; }
        public RelayCommand EditCommand { get; private set; }
        public RelayCommand DeleteCommand { get; private set; }


        public static List<string> Orgs = new List<string>();

        public ObservableCollection<ImageItem> Persons { get; set; } = new ObservableCollection<ImageItem>();

        private string orgName;
        public string OrgName
        {
            get { return this.orgName; }
            set
            {
                this.orgName = value;
                this.OnPropertyChanged(() => OrgName);
            }
        }

        private int count;
        public int Count
        {
            get { return this.count; }
            set
            {
                this.count = value;
                this.OnPropertyChanged(() => Count);
            }
        }

        private static List<string> jobs = new List<string>();

        /// <summary>
        /// 当前选择项
        /// </summary>
        public ImageItem SelectedPerson { get; set; }

        public StartWindowViewModel()
        {
            this.Init();
            GetJobs();
            this.OnClickCommand = new RelayCommand(OnNavChecked);
            this.AddCommand = new RelayCommand(this.OnAddPerson);
            this.EditCommand = new RelayCommand(this.OnEditPerson);
            this.DeleteCommand = new RelayCommand(this.OnDeletePerson);
        }

        public static List<string> GetJobs()
        {
            if (jobs.Count > 0) return jobs.ToList();

            var temp = RasAssist.CallRemoteService<IFaceCollect, string[]>(ee => ee.GetJobs());
            if (temp == null || temp.Length == 0) return new List<string>();

            jobs.AddRange(temp);
            return jobs.ToList();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="obj"></param>
        private void OnDeletePerson(object obj)
        {

        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="obj"></param>
        private void OnEditPerson(object obj)
        {
            if (this.SelectedPerson == null) return;

            ImageItem item = this.SelectedPerson;
            if (item == null || item.Person == null) return;

            PersonWindow win = new PersonWindow();
            PersonWindowViewModel dc = win.DataContext as PersonWindowViewModel;

            dc.CertificateId = item.Person.CertificateId;
            dc.Department = item.Person.Department;
            dc.FacePicFileName = item.Person.FacePicFileName;
            dc.Job = item.Person.Job;
            dc.Name = item.Person.Name;
            dc.PhoneNum = item.Person.PhoneNum;
            dc.FacePic = ImageManager.GetImage(item.Person.FacePicFileName);

            win.Owner = Application.Current.MainWindow;
            var ret = win.ShowDialog();
            if (ret == true)
            {
                Person person = new Person()
                {
                    CertificateId = dc.CertificateId,
                    Department = dc.Department,
                    FacePicFileName = dc.FacePicFileName,
                    Job = dc.Job,
                    Name = dc.Name,
                    PhoneNum = dc.PhoneNum,
                };

                var isSuccess = RasAssist.CallRemoteService<IFaceCollect, bool>(ee => ee.AddOrEditPersonInfo(person));
                if (!isSuccess)
                {
                    Plus.CSS.PromptHelper.Show("数据保存失败");
                    return;
                }

                var nav = this.Navs.ToList().Find(n => n.IsChecked == true);
                if (nav != null) this.OnNavChecked(nav);
            }
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="obj"></param>
        private void OnAddPerson(object obj)
        {
            PersonWindow win = new PersonWindow();
            win.Owner = Application.Current.MainWindow;
            var model = win.DataContext as PersonWindowViewModel;
            model.Department = this.OrgName;
            var ret = win.ShowDialog();
            if (ret == true)
            {
                PersonWindowViewModel dc = win.DataContext as PersonWindowViewModel;
                Person person = new Person()
                {
                    CertificateId = dc.CertificateId,
                    Department = dc.Department,
                    FacePicFileName = dc.FacePicFileName,
                    Job = dc.Job,
                    Name = dc.Name,
                    PhoneNum = dc.PhoneNum,
                };

                var isSuccess = RasAssist.CallRemoteService<IFaceCollect, bool>(ee => ee.AddOrEditPersonInfo(person));
                if (!isSuccess)
                {
                    Plus.CSS.PromptHelper.Show("数据保存失败");
                    return;
                }

                var nav = this.Navs.ToList().Find(n => n.IsChecked == true);
                if (nav != null) this.OnNavChecked(nav);
            }
        }

        private void OnNavChecked(object obj)
        {
            NavBindingItem item = obj as NavBindingItem;
            if (item == null) return;
            this.Persons.Clear();
            this.OrgName = item.Text;
            this.SelectedPerson = null;

            var persons = RasAssist.CallRemoteService<IFaceCollect, Person[]>(ee => ee.GetAllPerson());
            if (persons == null || persons.Length == 0) return;

            var ls = persons.ToList();
            var items = ls.FindAll(n => n.Department == item.Text);
            if (items == null || items.Count == 0) return;

            this.Persons.Clear();

            var tmps = new List<ImageItem>();
            foreach (var n in items)
            {
                var person = new ImageItem()
                {
                    FacePic = ImageManager.GetImage(n.FacePicFileName),
                    Person = n,
                    Name=n.Name,
                    Job=jobs.IndexOf(n.Job),
                };

                tmps.Add(person);
            }

            tmps.Sort(new RecorComparer());
            tmps.ForEach(n => this.Persons.Add(n));
            this.Count = this.Persons.Count;
        }

        private void Init()
        {
            this.Navs = new List<NavBindingItem>();

            var temp = RasAssist.CallRemoteService<IFaceCollect, string[]>(ee => ee.GetDepartments());
            if (temp == null || temp.Length == 0) return;

            foreach (var item in temp)
            {
                this.Navs.Add(new NavBindingItem("gi-nav-sy")
                {
                    Text = item,
                    GroupName = "mainnav",
                    IsChecked = false,
                });
            }

            if (Orgs.Count == 0)
            {
                Orgs.AddRange(temp);
            }

            this.Navs[0].IsChecked = true;
            this.OnNavChecked(this.Navs[0]);
        }
    }

    /// <summary>
    /// 升序排序
    /// </summary>
    class RecorComparer : IComparer<ImageItem>
    {
        public int Compare(ImageItem left, ImageItem right)
        {
            if (left.Job > right.Job)
                return 1;
            else if (left.Job == right.Job)
                return 0;
            else
                return -1;
        }
    }
}
