using System;
using System.Collections.Generic;
using System.IO;
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
    /// Interaction logic for SettingsControl.xaml
    /// </summary>
    public partial class SettingsControl : BaseControl
    {
        public SettingsControl(BaseWindow window):base(window)
        {
            InitializeComponent();
            window.Title = "Settings";
            this.TbServerPath.Text = Properties.Settings.Default.ServerRootPath;
            this.TbOutputPath.Text = Properties.Settings.Default.OutputPath == "" ?
                                            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) : 
                                            Properties.Settings.Default.OutputPath;

            String tempPath = Properties.Settings.Default.ServerRootPath + @"\schools\templates\";
            string[] lines = System.IO.File.ReadAllLines(tempPath + "emailtemplate1.txt");
            foreach (String line in lines) {
                this.TbMailText1.Text = this.TbMailText1.Text + line + "\r";
            }
            lines = System.IO.File.ReadAllLines(tempPath + "emailtemplate2.txt");
            foreach (String line in lines)
            {
                this.TbMailText2.Text = this.TbMailText2.Text + line + "\r";
            }
        }

        private void BtBack_Click(object sender, RoutedEventArgs e)
        {
            this.SetContent(new HomeControl(this.BaseWindow));
        }

        private void BtSave_Click(object sender, RoutedEventArgs e)
        {
            if (!Directory.Exists(TbOutputPath.Text.Trim()))
            {
                MessageBox.Show("Invalid output path. \r\nPlease type a valid folder path or go back without saving.");

                return;
            }

            Properties.Settings.Default.ServerRootPath = TbServerPath.Text.Trim();
            Properties.Settings.Default.OutputPath = this.TbOutputPath.Text.Trim();
            Properties.Settings.Default.Save();

            String tempPath = Properties.Settings.Default.ServerRootPath + @"\schools\templates\";

            String emailTemplate1 = this.TbMailText1.Text;
            String[] lines = emailTemplate1.Split('\n');

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(tempPath , "emailtemplate1.txt")))
            {
                foreach (string line in lines)
                    outputFile.WriteLine(line);
            }

            String emailTemplate2 = this.TbMailText2.Text;
            lines = emailTemplate2.Split('\n');

            // Write the string array to a new file named "WriteLines.txt".
            using (StreamWriter outputFile = new StreamWriter(System.IO.Path.Combine(tempPath, "emailtemplate2.txt")))
            {
                foreach (string line in lines)
                    outputFile.WriteLine(line);
            }
            
            this.SetContent(new HomeControl(this.BaseWindow));
        }
    }
}
