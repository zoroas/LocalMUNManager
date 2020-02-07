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
    public partial class DelegationsControl : BaseControl
    {
#pragma warning disable IDE0044 // Add readonly modifier
        private ObservableCollection<School> obsSchool;
#pragma warning restore IDE0044 // Add readonly modifier
        
        public DelegationsControl(BaseWindow window) : base(window)
        {
            InitializeComponent();
            window.Title = "Delegations";
            this.obsSchool = new ObservableCollection<School>();
            this.LvSchools.ItemsSource = this.obsSchool;
            String serverRoot = Properties.Settings.Default.ServerRootPath;
            if (!Directory.Exists(serverRoot))
            {
                throw new Exception("Invalid server path");
            }
            this.RefreshSchools();
            this.LvAvailableDelegations.ItemsSource = Delegation.GetAvailableObjDelegations(ApplicationSettings.LocalRoot);
        }

        private void RefreshSchools()
        {
            this.obsSchool.Clear();
            foreach (School s in School.GetAllSchools(ApplicationSettings.LocalRoot))
            {
                this.obsSchool.Add(s);
            }
        }

        private void BtBack_Click(object sender, RoutedEventArgs e)
        {
            this.SetContent(new HomeControl(this.BaseWindow));
        }

        private void LvSchools_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.LvSchools.SelectedItems == null ||
                this.LvSchools.SelectedItems.Count != 1)
                return;

            School school = (School)(this.LvSchools.SelectedItem);
            this.TbSchool.Text = school.Name;
//            this.TbICJJudgesRequests.Text = "" + school.NrICJJudgesRequested;
//            this.TbICJAdvocatesRequests.Text = "" + school.NrICJAdvocatesRequested;
            this.TbSCRequests.Text = "" + school.NrOfSecCouncilRequested;
            this.TbNumberOfStudentsGAAndSpecConf.Text = "" + school.NrOfStudentsGAAndSpecConf;
            this.LvAssignedDelegations.ItemsSource = school.GetDelegationsObj(ApplicationSettings.LocalRoot);
           
        }

        private void ButtonMoveRight_Click(object sender, RoutedEventArgs e)
        {
            if (this.LvSchools.SelectedItems == null ||
                this.LvSchools.SelectedItems.Count != 1)
                return;

            School school = (School)(this.LvSchools.SelectedItem);

            if (this.LvAvailableDelegations.SelectedItems == null ||
                this.LvAvailableDelegations.SelectedItems.Count == 0)
                return;

            foreach (Delegation delegation in this.LvAvailableDelegations.SelectedItems)
            {
                school.AddDelegation(delegation);
            }
            school.Save(ApplicationSettings.LocalRoot);

            this.LvAvailableDelegations.ItemsSource = null;
            this.LvAvailableDelegations.ItemsSource = Delegation.GetAvailableObjDelegations(ApplicationSettings.LocalRoot);
            this.LvAssignedDelegations.ItemsSource = null;
            this.LvAssignedDelegations.ItemsSource = school.GetDelegationsObj(ApplicationSettings.LocalRoot);
        }

        private void ButtonMoveLeft_Click(object sender, RoutedEventArgs e)
        {
            if (this.LvSchools.SelectedItems == null ||
                this.LvSchools.SelectedItems.Count != 1)
                return;

            School school = (School)(this.LvSchools.SelectedItem);

            if (this.LvAssignedDelegations.SelectedItems == null ||
                this.LvAssignedDelegations.SelectedItems.Count == 0)
                return;

            foreach (Delegation delegation in this.LvAssignedDelegations.SelectedItems)
            {
                school.RemoveDelegation(delegation);
            }

            school.Save(ApplicationSettings.LocalRoot);

            this.LvAvailableDelegations.ItemsSource = null;
            this.LvAvailableDelegations.ItemsSource = Delegation.GetAvailableObjDelegations(ApplicationSettings.LocalRoot);
            this.LvAssignedDelegations.ItemsSource = null;
            this.LvAssignedDelegations.ItemsSource = school.GetDelegationsObj(ApplicationSettings.LocalRoot);
        }
    }
}
