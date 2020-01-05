using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceCollect
{
    /// <summary>
    /// 用于UI COMMAND绑定
    /// </summary>
    public class RelayCommand
        : CommandBase
    {
        private Action<object> execute;
        private Func<object, bool> canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            if (execute == null) {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null) {
                canExecute = (o) => true;
            }

            this.execute = execute;
            this.canExecute = canExecute;
        }


        public override bool CanExecute(object parameter)
        {
            return canExecute(parameter);
        }

        protected override void OnExecute(object parameter)
        {
            execute(parameter);
        }
    }
}
