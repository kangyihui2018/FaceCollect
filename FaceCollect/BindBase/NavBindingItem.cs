using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace FaceCollect
{
    public class NavBindingItem : MenuBindingItem
    {
        private Geometry icon;
        public Geometry Icon { get { return icon; } }

        public NavBindingItem()
        {
            this.GroupName = "";
        }

        public NavBindingItem(string iconName) : this()
        {
            if (string.IsNullOrEmpty(iconName)) return;
            try
            {
                object resource = Application.Current.FindResource(iconName);
                this.icon = (Geometry)resource;
            }
            catch
            {
                this.icon = null;
            }
        }


    }
}
