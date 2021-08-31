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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Mail;
using System.Net;
using IMUNModel;

namespace LocalMUNManager
{
    /// <summary>
    /// Interaction logic for SchoolsControl.xaml
    /// </summary>
    public partial class SchoolsControl : BaseControl
    {
        private readonly ObservableCollection<School> obsSchool;
        private readonly ObservableCollection<Card> obsCards;
        
        public SchoolsControl(BaseWindow window) : base(window)
        {
            InitializeComponent();
            window.Title = "Schools";
            this.obsSchool = new ObservableCollection<School>();
            this.obsCards = new ObservableCollection<Card>();
            this.LvSchools.ItemsSource = this.obsSchool;
            this.LvParticipants.ItemsSource = this.obsCards;
            String serverRoot = ApplicationSettings.LocalRoot;
            if (!Directory.Exists(serverRoot))
            {
                throw new Exception("Invalid server path");
            }

            this.TbSecurityCouncil.ItemsSource = new int[] { 0, 1, 2 };
            this.TbSecurityCouncil.SelectedIndex = 0;
            this.TbICJAdvocatesRequests.Text = "0";
            this.TbICJJudgesRequests.Text = "0";
            this.TbTotal.Text = "0";
            this.TbNumberOfPress.Text = "0";
            this.TbNumberOfStudentsGAAndSpecConf.Text = "0";
            this.ComboStatus.ItemsSource = new SchoolStatus[]
            {
                SchoolStatus.SUBMITTED, SchoolStatus.ACCEPTED, SchoolStatus.REJECTED, SchoolStatus.ON_STANDBY
            };
            this.RefreshSchools();
        }

        private void RefreshSchools()
        {
            this.obsSchool.Clear();
            School[] schools = School.GetAllSchools(ApplicationSettings.LocalRoot);
            foreach (School s in schools)
            {
                this.obsSchool.Add(s);
            }
            this.TbSchoolCount.Text = "Schools: " + schools.Length;
        }

        private void RefreshParticipants()
        {
            School school = School.GetSchool(ApplicationSettings.LocalRoot, this.LvSchools.Text.Trim());
            this.TbPassword.Text = school == null ? "" : school.Password;
            this.TbAddress.Text = school == null ? "" : school.Address;
            this.TbEmail.Text = school == null ? "" : school.Email;
            this.TbICJJudgesRequests.Text = school == null ? "" : "" + school.NrICJJudgesRequested;
            this.TbICJAdvocatesRequests.Text = school == null ? "" : "" + school.NrICJAdvocatesRequested;
            this.TbNumberOfStudentsGAAndSpecConf.Text = school == null ? "" : "" + school.NrOfStudentsGAAndSpecConf;
            this.TbSecurityCouncil.Text = school == null ? "" : "" + school.NrOfSecCouncilRequested;
            this.TbPhone.Text = school == null ? "" : school.DirectorPhone;
            this.TbMUNDirector.Text = school == null ? "" : school.MUNDirector;
            this.TbDirectors.Text = school == null ? "" : "" + school.NrDirectorsRequested;
            this.CbIsICJRegistrar.IsChecked = school == null ? false : school.IsICJRegistrar;
            this.CbIsICJPresident.IsChecked = school == null ? false : school.IsICJPresident;
            this.TbNumberOfPress.Text = school == null ? "" : "" + school.NrPressRequests;
            this.TbTotal.Text = school == null ? "" : "" + school.NrTotalRequests;
            this.ComboStatus.SelectedItem = school == null ? SchoolStatus.SUBMITTED :  school.Status;
            this.obsCards.Clear();
            if (school != null)
            {
                this.obsCards.Clear();
                Card[] cards = school.GetCardsList(ApplicationSettings.LocalRoot);
                foreach (Card c in cards)
                    this.obsCards.Add(c);
                this.TbParticipants.Text = "Participants (" + cards.Length + ")";
                //this.LvSchools.SelectedItem = school;
            }
        }

        private void BtCreate_Click(object sender, RoutedEventArgs e)
        {
            String schoolName = this.LvSchools.Text.Trim();
            if (String.IsNullOrEmpty(schoolName)) return;

            String password;
            String address;
            String email;
            int nrICJJudgesRequested;
            int nrICJAdvocatesRequested;
            int nrNumberOfStudentsGAAndSpecConf;
            int nrSCRequested;
            int nrDirectors;
            int nrPress;
            int total;
            String phone;
            String mUNDirector;
            SchoolStatus status;

            try
            {
                password = this.TbPassword.Text.Trim();
                address = this.TbAddress.Text.Trim();
                email = this.TbEmail.Text.Trim();
                nrICJJudgesRequested = int.Parse(this.TbICJJudgesRequests.Text.Trim());
                nrICJAdvocatesRequested = int.Parse(this.TbICJAdvocatesRequests.Text.Trim());
                nrNumberOfStudentsGAAndSpecConf = int.Parse(this.TbNumberOfStudentsGAAndSpecConf.Text.Trim());
                nrSCRequested = int.Parse(this.TbSecurityCouncil.Text.Trim());
                nrPress = int.Parse(this.TbNumberOfPress.Text.Trim());
                phone = this.TbPhone.Text.Trim();
                mUNDirector = this.TbMUNDirector.Text.Trim();
                nrDirectors = int.Parse(this.TbDirectors.Text);
                total = int.Parse(this.TbTotal.Text);
                status = (SchoolStatus)(this.ComboStatus.SelectedItem);
            }
            catch (Exception)
            {
                MessageBox.Show("The values are not valid.");
                return;
            }

            School school = obsSchool.FirstOrDefault(x => x.Name.ToLower().Equals(schoolName.ToLower()));
            if (school != null)
            {
                String message = "There is already a school with this name.\r\nDo you want to update it?";
                bool? result = new MyDialog(message).ShowDialog();
                if (result == false)
                    return;
            }
            else
            {
                school = new School(schoolName, ApplicationSettings.LocalRoot);
                this.obsSchool.Add(school);
            }

            school.Name = schoolName;
            school.Password = password;
            school.Address = address;
            school.Email = email;
            school.NrICJJudgesRequested = nrICJJudgesRequested;
            school.NrICJAdvocatesRequested = nrICJAdvocatesRequested;
            school.NrOfStudentsGAAndSpecConf = nrNumberOfStudentsGAAndSpecConf;
            school.NrOfSecCouncilRequested = nrSCRequested;
            school.NrPressRequests = nrPress;
            school.DirectorPhone = phone;
            school.MUNDirector = mUNDirector;
            school.NrDirectorsRequested = nrDirectors;
            school.IsICJPresident = (bool)(this.CbIsICJPresident.IsChecked);
            school.IsICJRegistrar = (bool)(this.CbIsICJRegistrar.IsChecked);
            school.NrTotalRequests = total;
            school.Status = status;

            //String serverRoot = Properties.Settings.Default.ServerRootPath;
            //DirectoryInfo dir = Directory.CreateDirectory(serverRoot + @"/" + school.Name);
            school.Save(ApplicationSettings.LocalRoot);
            RefreshSchools();
        }
        
        private void BtRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String schoolName = this.LvSchools.Text.Trim();

                School school = obsSchool.FirstOrDefault(x => x.Name.ToLower().Equals(schoolName.ToLower()));
                if (school == null)
                {
                    MessageBox.Show("Please provide a valid school name.");
                    return;
                }
                String message = "Are you sure you want to delete " + school.Name + "?\r\nAll participants of this school will also be deleted.";
                bool? result = new MyDialog(message).ShowDialog();
                if (result == true)
                {
                    this.obsCards.Clear();
                    this.obsSchool.Remove(school);
                    school.Delete(ApplicationSettings.LocalRoot);
                }
                else if (result == false)
                {
                    // Doesn't do anything
                }
            }
            catch(Exception ex) {
                Console.WriteLine(ex.Message);
            }
        }

        private void BtBack_Click(object sender, RoutedEventArgs e)
        {
            this.SetContent(new HomeControl(this.BaseWindow));
        }

        private void BtSendEmail1_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(this.LvSchools.Text) || String.IsNullOrWhiteSpace(this.LvSchools.Text))
            {
                MessageBox.Show("You must select a school when sending an email.");
                return;
            }

            try
            {
                School school = (School)(this.LvSchools.SelectedItem);
                MyDialog d = new MyDialog("Do you want to send an email to " + school.MUNDirector + "?");
                d.ShowDialog();
                if (d.DialogResult != true)
                    return;

                String tempPath = Properties.Settings.Default.ServerRootPath + @"\schools\templates\";
                string[] lines = System.IO.File.ReadAllLines(tempPath + "emailtemplate1.txt");
                String emailTemplate = "";
                foreach (String line in lines)
                {
                    emailTemplate = emailTemplate + line + "\r\n";
                }

                String login = school.Name;
                String email = school.Email;
                String password = school.Password;
                String director = school.MUNDirector;

                emailTemplate = emailTemplate.Replace("{login}", login);
                emailTemplate = emailTemplate.Replace("{password}", password);
                emailTemplate = emailTemplate.Replace("{name}", director);

                SmtpClient SmtpServer = new SmtpClient("192.168.20.250");
                var mail = new MailMessage
                {
                    From = new MailAddress("imun@caislisbon.org"),
                    Subject = "IMUN",
                    IsBodyHtml = false,
                    Body = emailTemplate
                };
                mail.To.Add(email);
                SmtpServer.Port = 25;
                SmtpServer.UseDefaultCredentials = true;

                SmtpServer.EnableSsl = false;
                SmtpServer.Send(mail);
                MessageBox.Show("An email was sent to " + director);
            }
            catch (SmtpException x)
            {
                MessageBox.Show("Message could not be sent. Check email destination.\r" + x.Message);
                return;
            }
            catch (Exception x)
            {
                MessageBox.Show("An error has ocurred.\r" + x.Message);
            }
        }

        private void BtSendEmail2_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(this.LvSchools.Text) || String.IsNullOrWhiteSpace(this.LvSchools.Text))
            {
                MessageBox.Show("You must select a school when sending an email.");
                return;
            }

            try
            {
                School school = (School)(this.LvSchools.SelectedItem);
                MyDialog d = new MyDialog("Do you want to send an email to " + school.MUNDirector + "?");
                d.ShowDialog();
                if (d.DialogResult != true)
                    return;

                String tempPath = Properties.Settings.Default.ServerRootPath + @"\schools\templates\";
                string[] lines = System.IO.File.ReadAllLines(tempPath + "emailtemplate2.txt");
                String emailTemplate = "";
                foreach (String line in lines)
                {
                    emailTemplate = emailTemplate + line + "\r\n";
                }

                String login = school.Name;
                String email = school.Email;
                String password = school.Password;
                String director = school.MUNDirector;

                emailTemplate = emailTemplate.Replace("{login}", login);
                emailTemplate = emailTemplate.Replace("{password}", password);
                emailTemplate = emailTemplate.Replace("{name}", director);

                SmtpClient SmtpServer = new SmtpClient("192.168.20.250");
                var mail = new MailMessage
                {
                    From = new MailAddress("imun@caislisbon.org"),
                    Subject = "IMUN",
                    IsBodyHtml = false,
                    Body = emailTemplate
                };
                mail.To.Add(email);
                SmtpServer.Port = 25;
                SmtpServer.UseDefaultCredentials = true;
                SmtpServer.EnableSsl = false;
                SmtpServer.Send(mail);
                MessageBox.Show("An email was sent to " + director);
            }
            catch (SmtpException x)
            {
                MessageBox.Show("Message could not be sent. Check email destination.\r" + x.Message);
                return;
            }
            catch (Exception x)
            {
                MessageBox.Show("An error has ocurred.\r" + x.Message);
            }
        }

        private void LvSchools_KeyUp(object sender, KeyEventArgs e)
        {
            if (String.IsNullOrEmpty(this.LvSchools.Text) || String.IsNullOrWhiteSpace(this.LvSchools.Text))
                return;

            String schoolName = this.LvSchools.Text.Trim();
            School school = obsSchool.FirstOrDefault(x => x.Name.ToLower().Equals(schoolName.ToLower()));
            if (school == null)
            {
                this.BtCreate.Content = "Create";
                return;
            }
            else
            {
                this.BtCreate.Content = "Update School";
            }


            this.RefreshParticipants();
        }

        private void LvSchools_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems == null || e.AddedItems.Count == 0)
                    return;
                School s = (School)(e.AddedItems[0]);
                this.LvSchools.Text = s.Name;
            }
            catch (Exception)
            {

            }
            this.RefreshParticipants();
        }
    }
}
