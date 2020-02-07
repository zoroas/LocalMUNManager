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
    public partial class GenerateCardsControl : BaseControl
    {
        public GenerateCardsControl(BaseWindow parent) : base(parent)
        {
            InitializeComponent();
            parent.Title = "Home";
            this.CbSchool.Visibility = Visibility.Hidden;
        }
        
        private School GetCurrentSchool()
        {
            if (this.RBGenerateAllSchools.IsChecked == true)
                return null;
            else
            {
                if (this.CbSchool.SelectedItem != null)
                    return (School)(this.CbSchool.SelectedItem);
                else
                    return null;
            }
        }

        private void GenerateCards(Card[] cards, string template)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "Document", // Default file name
                DefaultExt = ".pptx", // Default file extension
                Filter = "PowerPoint file (.pptx)|*.pptx" // Filter files by extension
            };

            //// Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            //// Process save file dialog box results
            if (result != true)
            {
                return;
            }
            // Save document
            string pptOutput = dlg.FileName;
            this.Cursor = Cursors.Wait;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            PowerPointCreator.CreateBadges(
                cards,
                template,
                pptOutput             
                );
            if (PowerPointCreator.HasError())
                MessageBox.Show(PowerPointCreator.GetError());  
            this.Cursor = Cursors.Arrow;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        private void GenerateCertificates(Card[] cards, string template)
        {
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog
            {
                FileName = "Document", // Default file name
                DefaultExt = ".pptx", // Default file extension
                Filter = "PowerPoint file (.pptx)|*.pptx" // Filter files by extension
            };

            //// Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            //// Process save file dialog box results
            if (result != true)
            {
                return;
            }
            // Save document
            string pptOutput = dlg.FileName;
            this.Cursor = Cursors.Wait;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Wait;

            PowerPointCreator.CreateCertificates(
                cards,
                template,
                pptOutput
                );
            if (PowerPointCreator.HasError())
                MessageBox.Show(PowerPointCreator.GetError());
            this.Cursor = Cursors.Arrow;
            Mouse.OverrideCursor = System.Windows.Input.Cursors.Arrow;
        }

        private void BtDelegates_Click(object sender, RoutedEventArgs e)
        {
            Card[] cards = Card.GetAllCards(ApplicationSettings.LocalRoot).Where(x =>
                x.Forum.Equals("General Assembly Delegate") ||
                x.Forum.Equals("Special Conference Delegate") ||
                x.Forum.Equals("Security Council Delegate") ||
                x.Forum.Equals("ICJ Advocate") ||
                x.Forum.Equals("ICJ Judge")
                ).ToArray();
            School current = GetCurrentSchool();
            if (current != null)
                cards = cards.Where(x => x.School.Equals(current.Name)).ToArray();
            GenerateCards(cards, "card_delegate.pptx");
        }

        private void BtPress_Click(object sender, RoutedEventArgs e)
        {
            Card[] cards = Card.GetAllCards(ApplicationSettings.LocalRoot).Where(x =>
                x.IsPress).ToArray();
            School current = GetCurrentSchool();
            if (current != null)
                cards = cards.Where(x => x.School.Equals(current.Name)).ToArray();
            GenerateCards(cards, "card_press.pptx");
        }

        private void BtAdmin_Click(object sender, RoutedEventArgs e)
        {
            Card[] cards = Card.GetAllCards(ApplicationSettings.LocalRoot).Where(x =>
                x.IsAdmin).ToArray();
            School current = GetCurrentSchool();
            if (current != null)
                cards = cards.Where(x => x.School.Equals(current.Name)).ToArray();
            GenerateCards(cards, "card_admin.pptx");
        }

        private void BtOfficer_Click(object sender, RoutedEventArgs e)
        {
            Card[] cards = Card.GetAllCards(ApplicationSettings.LocalRoot).Where(x =>
                           x.IsOfficer).ToArray();
            School current = GetCurrentSchool();
            if (current != null)
                cards = cards.Where(x => x.School.Equals(current.Name)).ToArray();
            GenerateCards(cards, "card_officer.pptx");
        }

        private void BtDirector_Click(object sender, RoutedEventArgs e)
        {
            Card[] cards = Card.GetAllCards(ApplicationSettings.LocalRoot).Where(x =>
       //         x.IsChaperone ||
                x.IsDirector
                ).ToArray();
            School current = GetCurrentSchool();
            if (current != null)
                cards = cards.Where(x => x.School.Equals(current.Name)).ToArray();
            GenerateCards(cards, "card_director.pptx");
        }

        private void BtBack_Click(object sender, RoutedEventArgs e)
        {
            this.SetContent(new HomeControl(this.BaseWindow));
        }

        private void RBGenerateAllSchools_Click(object sender, RoutedEventArgs e)
        {
            this.CbSchool.Visibility = Visibility.Collapsed;
        }

        private void RBGenerateOneSchool_Click(object sender, RoutedEventArgs e)
        {
            this.CbSchool.Visibility = Visibility.Visible;
            this.CbSchool.ItemsSource = School.GetAllSchools(ApplicationSettings.LocalRoot);
        }

        private void BtCertificates_Click(object sender, RoutedEventArgs e)
        {
            Card[] cards = Card.GetAllCards(ApplicationSettings.LocalRoot).Where(x=> 
                                    x.IsDirector == false &&
                                    x.IsOfficer == false).ToArray();
            School current = GetCurrentSchool();
            if (current != null)
                cards = cards.Where(x => x.School.Equals(current.Name)).ToArray();
            GenerateCertificates(cards, "imun_certificate.pptx");
        }
    }
}
