using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceCollect
{
    /// <summary>
    /// 通用节点描述信息
    /// </summary>
    public class NodeItem : NotifyPropertyChanged
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Key { get; set; }
    }
}
