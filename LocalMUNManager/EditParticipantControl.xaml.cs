using IMUNModel;
using LocalMUNManager.model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LocalMUNManager
{
    public partial class EditParticipantControl : BaseControl
    {
        private readonly ObservableCollection<School> obsSchool;
        FileInfo photoPath = null;
       // String SERVER_BASE_FOLDER = @"\\caislvs-005\MUN data\";
        Card currentCard = null;

        private static readonly string ICJ_ADVOCATE = "ICJ Advocate";
        private static readonly string ICJ_JUDGE = "ICJ Judge";
        private static readonly string PRESS_APPLICANT = "Press";
//        private static readonly string CHAPERONE = "Chaperone";
        private static readonly string DIRECTOR = "Director";

        public EditParticipantControl(BaseWindow window) : base(window)
        {
            InitializeComponent();
            window.Title = "Edit Participant";
            this.obsSchool = new ObservableCollection<School>();
            this.CbSchools.ItemsSource = obsSchool;
            this.RefreshSchools();

            String[] venues = Positions.GetAll();
            Console.WriteLine("ok");
            this.TbForum.ItemsSource = venues;
            this.TbDelegation.IsEnabled = false;
            this.CheckBoxHasOpeningSpeach.IsChecked = false;
            this.CheckBoxIsChairman.IsChecked = false;
        }

        public EditParticipantControl(BaseWindow window, Card card) : this(window)
        {
            currentCard = card;
            this.TbFirstName.Text = card.FirstName;
            this.TbLastName.Text = card.LastName;
            this.TbDelegation.IsEnabled = true;

            String serverRoot = @"\\caislvs-005\MUN data\";
            ApplicationSettings.LocalRoot = serverRoot;

            String root = ApplicationSettings.LocalRoot;
            this.TbDelegation.ItemsSource = School.GetSchool(root, card.School).GetAvailableDelegationsForCard(root, card);

            this.TbForum.SelectedItem = card.Forum;
            this.TbFirstName.IsReadOnly = true;
            this.TbLastName.IsReadOnly = true;

            foreach (School s in this.CbSchools.ItemsSource)
            {
                if (s.Name.Equals(card.School))
                    this.CbSchools.SelectedItem = s;
            }

            if (!Positions.IsSpecialPosition(card.Forum))
            {
                if ((Delegation[])(this.TbDelegation.ItemsSource) != null)
                {
                    try
                    {
                        Delegation delegation = ((Delegation[])(this.TbDelegation.ItemsSource)).FirstOrDefault(x => x.Name.Equals(card.Country));
                        this.TbDelegation.SelectedItem = delegation;
                    }
                    catch(Exception)
                    { }
                }
            }
            else
            {
                // do nothing
            }
 
            this.ImgCard.Source = new BitmapImage(
                  new Uri(card.LocalPicturePath));
            this.photoPath = new FileInfo(card.LocalPicturePath);
            this.CheckBoxIsChairman.IsChecked = card.IsChairman;
            this.CheckBoxHasOpeningSpeach.IsChecked = card.HasOpeningSpeach;
        }

        private void RefreshSchools()
        {
            this.obsSchool.Clear();

            String serverRoot = @"\\caislvs-005\MUN data\";
            ApplicationSettings.LocalRoot = serverRoot;

            foreach (School s in School.GetAllSchools(ApplicationSettings.LocalRoot))
            {
                this.obsSchool.Add(s);
            }
        }
     
        private void BtCreate_Click(object sender, RoutedEventArgs e)
        {
            School school = (School)(this.CbSchools.SelectedItem);
            if (school == null)
            {
                String message = "There is no school with this name.\r\nIf the problem persists please contact the application manager.";
                System.Windows.MessageBox.Show(message);
                return;
            }

            if (currentCard == null)
            {
                if (photoPath == null)
                {
                    System.Windows.MessageBox.Show("Please provide a picture.");
                    return;
                }

                String position = this.TbForum.Text;
                String country = position.Equals("Press") ? "Press" : this.TbDelegation.Text;
                country = country.Equals("Director") ? "Director" : country;
 //               country = country.Equals("Chaperone") ? "Chaperone" : country;

                currentCard = new Card(school, this.TbFirstName.Text, this.TbLastName.Text)
                {
                    Country = country,
                    Filename = photoPath.Name,
                    Forum = position,
                    PictureUserName = photoPath.Name,
                    IsChairman = (bool)(this.CheckBoxIsChairman.IsChecked)
                    //ServerBaseFolder = SERVER_BASE_FOLDER + school.Name
                };
                String serverRoot = ApplicationSettings.LocalRoot;
                // Retrieve the name of the file that is posted.
                //string strFileName = photoPath.Name;
                //strFileName = System.IO.Path.GetFileName(strFileName + @"\schools\" + school.Name + @"\");
                currentCard.Save(ApplicationSettings.LocalRoot);
                File.Copy(photoPath.FullName, serverRoot + @"\schools\" + school.Name + @"\" + currentCard.PictureInternalName);
            }
            else
            {
                String position = this.TbForum.Text;


                String country = this.TbDelegation.Text;
                country = (position.Equals(ICJ_ADVOCATE) || position.Equals(ICJ_JUDGE)) ? "" : country;
                country = position.Equals(PRESS_APPLICANT) ? "" : country;
  //              country = position.Equals(CHAPERONE) ? "" : country;
                country = position.Equals(DIRECTOR) ? "" : country;

                currentCard.Country = country;
                currentCard.Filename = photoPath.Name;
                currentCard.Forum = position;
                currentCard.PictureUserName = photoPath.Name;
                currentCard.IsChairman = (bool)(this.CheckBoxIsChairman.IsChecked);
                currentCard.HasOpeningSpeach = (bool)(this.CheckBoxHasOpeningSpeach.IsChecked);
                //currentCard.ServerBaseFolder = SERVER_BASE_FOLDER + school.Name;


                String serverRoot = @"\\caislvs-005\MUN data\";
                ApplicationSettings.LocalRoot = serverRoot;

                // String serverRoot = Properties.Settings.Default.ServerRootPath;
                // Retrieve the name of the file that is posted.
                //string strFileName = photoPath.Name;
                //strFileName = System.IO.Path.GetFileName(strFileName);
                currentCard.Save(ApplicationSettings.LocalRoot + @"\schools\" + school.Name + @"\");
                if (!File.Exists(serverRoot + @"\schools\" + school.Name + @"\" + currentCard.PictureInternalName))
                    File.Copy(photoPath.FullName, serverRoot + @"\schools\" + school.Name + @"\" + currentCard.PictureInternalName);
            }
            this.SetContent(new ParticipantsControl(this.BaseWindow));
        }

        private void BtBack_Click(object sender, RoutedEventArgs e)
        {
            this.SetContent(new ParticipantsControl(this.BaseWindow));
        }
       
        private void BtUploadPicture_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                this.ImgCard.Source = new BitmapImage(
                    new Uri(dialog.FileName));
                this.photoPath = new FileInfo(dialog.FileName);
            }
        }

        private void CbSchools_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.CbSchools.SelectedItem != null)
            {
                this.TbDelegation.IsEnabled = true;
                School school = (School)(this.CbSchools.SelectedItem);

                String serverRoot = @"\\caislvs-005\MUN data\";
                ApplicationSettings.LocalRoot = serverRoot;

                //string dir = Properties.Settings.Default.ServerRootPath;

                List<Delegation> delegations = school.GetAvailableDelegationsObj(ApplicationSettings.LocalRoot).ToList();
                Card card = Card.GetCard(serverRoot + @"\schools\" + school.Name + @"\" , this.TbFirstName.Text, this.TbLastName.Text);
                if (card != null)
                {
                    delegations.Add(Delegation.GetDelegation(ApplicationSettings.LocalRoot, card.Country));
                }
                this.TbDelegation.ItemsSource = delegations.ToArray();
            }
        }
    }
}
