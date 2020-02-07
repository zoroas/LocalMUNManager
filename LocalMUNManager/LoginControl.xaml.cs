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

namespace LocalMUNManager
{
    /// <summary>
    /// Interaction logic for LoginControl.xaml
    /// </summary>
    public partial class LoginControl : BaseControl
    {
        public LoginControl(BaseWindow parent):base(parent)
        {
            InitializeComponent();
            parent.Title = "Login";
        }

        private void BtLogin_Click(object sender, RoutedEventArgs e)
        {
            if (this.TbUser.Text.ToLower().Equals("pandrews") &&
                this.TbPassword.Password.Equals("1945") 
                ||
                this.TbUser.Text.ToLower().Equals("angela.fidalgo") &&
                this.TbPassword.Password.Equals("1945")
                ||
                this.TbUser.Text.ToLower().Equals("mc") &&
                this.TbPassword.Password.Equals("1945")
                )
            {
                this.SetContent(new HomeControl(this.BaseWindow));
            }
        }
    }
}
