﻿using IMUNModel;
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

namespace LocalMUNManager
{
    /// <summary>
    /// Interaction logic for SchoolsControl.xaml
    /// </summary>
    public partial class ParticipantsControl : BaseControl
    {
        ObservableCollection<School> obsSchool;
        ObservableCollection<Card> obsParticipants;

        public ParticipantsControl(BaseWindow window) : base(window)
        {
            InitializeComponent();
            window.Title = "Participants";
            this.obsSchool = new ObservableCollection<School>();
            this.obsParticipants = new ObservableCollection<Card>();
            this.CbSchools.ItemsSource = obsSchool;
            this.LvParticipants.ItemsSource = obsParticipants;
            String serverRoot = @"\\caislvs-005\MUN data\";
            ApplicationSettings.LocalRoot = serverRoot;
           // String serverRoot = Properties.Settings.Default.ServerRootPath;
            if (!Directory.Exists(serverRoot))
            {
                throw new Exception("Invalid server path");
            }
            this.RefreshSchools();
            this.RefreshParticipants();
        }

        private void RefreshSchools()
        {
            String serverRoot = @"\\caislvs-005\MUN data\";
            ApplicationSettings.LocalRoot = serverRoot;
            this.obsSchool.Clear();
            foreach (School s in School.GetAllSchools(ApplicationSettings.LocalRoot))
            {
                this.obsSchool.Add(s);
            }
        }

        private void RefreshParticipants()
        {
            String serverRoot = @"\\caislvs-005\MUN data\";
            ApplicationSettings.LocalRoot = serverRoot;
//            String serverRoot = Properties.Settings.Default.ServerRootPath;
            List<Card> cards = new List<Card>();
            foreach(School school in School.GetAllSchools(ApplicationSettings.LocalRoot))
            {
                cards.AddRange(school.GetCardsList(ApplicationSettings.LocalRoot));
            }

            this.obsParticipants.Clear();
            foreach (Card participant in cards)
            {
                this.obsParticipants.Add(participant);
            }

            this.TbNumberOfParticipants.Text = "Total Number of Participants: " + cards.Count;
        }

        private void SaveSchools()
        {
            String serverRoot = @"\\caislvs-005\MUN data\";
            ApplicationSettings.LocalRoot = serverRoot;
//            String serverRoot = Properties.Settings.Default.ServerRootPath;
            String usersFilePath = serverRoot + "/users.json";
            using (StreamWriter file = File.CreateText(usersFilePath))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, this.obsSchool.ToArray());
            }
        }

        private void BtCreate_Click(object sender, RoutedEventArgs e)
        {
            this.SetContent(new EditParticipantControl(this.BaseWindow));
        }
        
        private void BtRemove_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String serverRoot = @"\\caislvs-005\MUN data\";
                ApplicationSettings.LocalRoot = serverRoot;
                Card card = obsParticipants.FirstOrDefault(x => x.Fullname.Equals(this.TbName.Text));
                if (card == null)
                {
                    MessageBox.Show("Please provide a valid school name.");
                    return;
                }
                String message = "Are you sure you want to delete " + card.FirstName + " " + card.LastName + "?\r\n";
                bool? result = new MyDialog(message).ShowDialog();
                if (result == true)
                {
                    //String serverRoot = Properties.Settings.Default.ServerRootPath;
                    this.obsParticipants.Remove(card);
                    card.Delete(ApplicationSettings.LocalRoot);
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

        private void TbUser_TextChanged(object sender, TextChangedEventArgs e)
        {
            //User school = obsSchool.FirstOrDefault(x => x.Username.Equals(this.TbUser.Text));
            //if (school == null)
            //{
            //    this.BtCreate.Content = "Create";
            //    return;
            //}
            //else
            //{
            //    this.BtCreate.Content = "Update School";

            //}
        }

        private void BtSearchName_Click(object sender, RoutedEventArgs e)
        {
            SearchByName(this.TbName.Text);
            this.CbSchools.SelectedItem = null;
            this.ImgCard.Source = null;
        }

        private void SearchByName(String name)
        {
            String serverRoot = @"\\caislvs-005\MUN data\";
            ApplicationSettings.LocalRoot = serverRoot;
            List<Card> cards = Card.GetAllCards(ApplicationSettings.LocalRoot).Where(x => x.FirstName.ToLower().Contains(name.ToLower()) |
                                                          x.LastName.ToLower().Contains(name.ToLower())).ToList();
            this.obsParticipants.Clear();
            foreach (Card c in cards)
            {
                this.obsParticipants.Add(c);
            }
        }

        private void BtSearchSchool_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.CbSchools.SelectedItem == null)
                {
                    this.SearchByName("");
                }
                else
                {
                    School school = (School)(this.CbSchools.SelectedItem);
                    this.obsParticipants.Clear();
                    String serverRoot = @"\\caislvs-005\MUN data\";
                    ApplicationSettings.LocalRoot = serverRoot;
                    foreach (Card c in school.GetCardsList(ApplicationSettings.LocalRoot))
                    {
                        this.obsParticipants.Add(c);
                    }
                }
                this.ImgCard.Source = null;
            }
            catch (Exception) { }
        }

        private void BtSearchDelegations_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String delegation = this.TbDelegation.Text;
                if (String.IsNullOrEmpty(delegation))
                {
                    this.SearchByName("");
                }
                else
                {
                    String serverRoot = @"\\caislvs-005\MUN data\";
                    ApplicationSettings.LocalRoot = serverRoot;
                    List<Card> list = Card.GetAllCards(ApplicationSettings.LocalRoot).Where(x => x.Country.ToLower().Contains(delegation.ToLower())).ToList();
                    this.obsParticipants.Clear();
                    foreach (Card c in list)
                    {
                        this.obsParticipants.Add(c);
                    }
                }
                this.CbSchools.SelectedItem = null;
                this.ImgCard.Source = null;
            }
            catch (Exception) { }
        }

        private void BtSearchForum_Click(object sender, RoutedEventArgs e)
        {
            String serverRoot = @"\\caislvs-005\MUN data\";
            ApplicationSettings.LocalRoot = serverRoot;
            try
            {
                String forum = this.TbForum.Text;
                if (String.IsNullOrEmpty(forum))
                {
                    this.SearchByName("");
                }
                else
                {
                    List<Card> list = Card.GetAllCards(ApplicationSettings.LocalRoot).Where(x => x.Forum.ToLower().Contains(forum.ToLower())).ToList();
                    this.obsParticipants.Clear();
                    foreach (Card c in list)
                    {
                        this.obsParticipants.Add(c);
                    }
                }
                this.CbSchools.SelectedItem = null;
                this.ImgCard.Source = null;
            }
            catch (Exception) { }
        }

        private void LvParticipants_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.LvParticipants.SelectedItems == null ||
                this.LvParticipants.SelectedItems.Count != 1)
                return;

            Card card = (Card)(this.LvParticipants.SelectedItem);
            this.TbName.Text = card.FirstName + " " + card.LastName;
            this.TbForum.Text = card.Forum;
            this.TbDelegation.Text = card.Country;
            this.CbSchools.SelectedItem = this.obsSchool.FirstOrDefault(x=>x.Name.Equals(card.School));

            this.ImgCard.Source = new BitmapImage(
                new Uri(card.LocalPicturePath));
        }

        private void BtCreateCards(object sender, RoutedEventArgs e)
        {
           
        }

        private void BtEdit_Click(object sender, RoutedEventArgs e)
        {
            if (this.LvParticipants.SelectedItem == null)
                return;
            Card card = (Card)(this.LvParticipants.SelectedItem);
            this.SetContent(new EditParticipantControl(this.BaseWindow, card));
        }
    }
}
