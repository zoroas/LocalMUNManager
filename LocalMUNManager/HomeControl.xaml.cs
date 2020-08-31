using IMUNModel;
using LocalMUNManager.model;
using Newtonsoft.Json;
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
//using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LocalMUNManager
{
    /// <summary>
    /// Interaction logic for HomeControl.xaml
    /// </summary>
    public partial class HomeControl : BaseControl
    {
        public HomeControl(BaseWindow parent) : base(parent)
        {
            InitializeComponent();
            ApplicationSettings.LocalRoot = @"\\caislvs-005\\Mun data";
            parent.Title = "Home";
        }

        private void BtSchools_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String serverRoot = Properties.Settings.Default.ServerRootPath;
                if (!Directory.Exists(serverRoot))
                {
                    System.Windows.MessageBox.Show("Invalid server folder. Please change settings");
                }
                else
                {
                    this.SetContent(new SchoolsControl(this.BaseWindow));
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("" + ex.Message);
            }
        }

        private void BtParticipants_Click(object sender, RoutedEventArgs e)
        {
            String serverRoot = Properties.Settings.Default.ServerRootPath;
            if (!Directory.Exists(serverRoot))
            {
                MessageBox.Show("Invalid server folder. Please change settings");
            }
            else
            {
                this.SetContent(new ParticipantsControl(this.BaseWindow));
            }
        }

        private void BtExport_Click(object sender, RoutedEventArgs e)
        {
            Reports.CompleteHTMLReport.GenerateHTMLReport();
        }
        
        private void BtSettings_Click(object sender, RoutedEventArgs e)
        {
            this.SetContent(new SettingsControl(this.BaseWindow));
        }

        private void BtManageDelegations_Click(object sender, RoutedEventArgs e)
        {
            this.SetContent(new DelegationsControl(this.BaseWindow));
        }

        private void BtGenerateCards_Click(object sender, RoutedEventArgs e)
        {



            //Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            //{
            //    FileName = "Document", // Default file name
            //    DefaultExt = ".pptx", // Default file extension
            //    Filter = "PowerPoint file (.pptx)|*.pptx" // Filter files by extension
            //};

            //// Show save file dialog box
            //Nullable<bool> result = dlg.ShowDialog();

            //// Process save file dialog box results
            //if (result != true)
            //{
            //    return;
            //}
            //// Save document
            //string pptOutput = dlg.FileName;

            //this.Cursor = Cursors.Wait;
            //Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            //PowerPointCreator c = new PowerPointCreator(
            //    this.obsParticipants.ToList(),
            //    pptOutput);
            //if (c.HasError())
            //    MessageBox.Show(c.GetError());
            //this.Cursor = Cursors.Arrow;
            //Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        private void BtCards_Click(object sender, RoutedEventArgs e)
        {
            this.SetContent(new GenerateCardsControl(this.BaseWindow));
        }


    }
}
