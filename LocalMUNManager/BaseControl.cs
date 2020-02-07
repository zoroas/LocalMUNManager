using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LocalMUNManager
{

    public enum DialogResult
    {  
       Undefined,  
       Yes,  
       No  
    }

    public class BaseControl: UserControl
    {
        protected BaseWindow BaseWindow;
        public BaseControl():base() {
        }

        public BaseControl(BaseWindow parent):this()
        {
            this.BaseWindow = parent;
            //this.Width = 1024 - 48;
            //this.Height = 800 - 64;
        }

        public void SetContent(BaseControl anotherControl)
        {
            this.BaseWindow.PanelPresenter.Content = anotherControl;
        }

        public void GoHome()
        {
            SetContent(new HomeControl(this.BaseWindow));
        }

    }
}
