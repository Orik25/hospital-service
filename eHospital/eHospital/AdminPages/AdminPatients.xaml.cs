using EF.service.impl;
using EF;
using eHospital.Forms;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace eHospital.AdminPages
{
    /// <summary>
    /// Interaction logic for AdminPatients.xaml
    /// </summary>
    public partial class AdminPatients : Page
    {
        public int currentPage = 1;
        public int itemsPerPage;
        public int totalPages;
        List<Member> members;
        List<User> patients;
        private readonly UserServiceImpl userService;
        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {

            if (currentPage != totalPages - 2)
            {
                int skip = currentPage * itemsPerPage;
                currentPage++;
                membersDataGrid.ItemsSource = members.Skip(skip).Take(itemsPerPage);
                currentPageButton.Content = currentPage.ToString();
                nextPageButton.Content = (currentPage + 1).ToString();
                currentPageButton.InvalidateVisual();
            }


        }
        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 2)
            {
                currentPage--;
                int skip = currentPage * itemsPerPage;
                membersDataGrid.ItemsSource = members.Skip(skip).Take(itemsPerPage);
                currentPageButton.Content = currentPage.ToString();
                nextPageButton.Content = (currentPage + 1).ToString();
                currentPageButton.InvalidateVisual();
            }

        }
        public AdminPatients()
        {
            InitializeComponent();

            currentPage = 1;

            this.userService = new UserServiceImpl(new EF.context.NeondbContext());

            this.patients = userService.GetPatients();
            this.members = MapDoctorsToMembers(this.patients);


            itemsPerPage = 5;
            int totalItems = members.Count(); 

            totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
            currentPageButton.Content = currentPage.ToString();
            currentPageButton.InvalidateVisual();
            lastPageButton.Content = totalPages.ToString();
            nextPageButton.Content = (currentPage + 1).ToString();
            lastPageButton.InvalidateVisual();
            membersDataGrid.ItemsSource = members.Take(itemsPerPage);

            txtSearch.TextChanged += txtSearch_TextChanged;
        }
        private List<Member> MapDoctorsToMembers(List<User> patients)
        {
            List<Member> returnMembers = new List<Member>();
            foreach (var patient in patients)
            {
                Member newMember = new Member();
                newMember.Id = patient.UserId;
                newMember.Name = patient.FirstName + " " + patient.LastName;
                newMember.Email = patient.Email;
                newMember.PhoneNumber = patient.Phone;
                returnMembers.Add(newMember);
            }
            List<Member> sortedMembersByName = returnMembers.OrderBy(member => member.Name).ToList();
            return sortedMembersByName;
        }

        private void membersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        
        public class Member
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }



        }
        public void ShowAdminProfile_click(object sender, RoutedEventArgs e)
        {

            Profile childWindow = new Profile();
            Window parentWindow = Window.GetWindow((DependencyObject)sender);

            parentWindow.Opacity = 0.25;
            childWindow.Closed += (s, args) =>
            {
                parentWindow.Opacity = 1.0;
            };
            childWindow.Owner = parentWindow;
            childWindow.ShowDialog();
        }
        public void ShowAddNewPatient_click(object sender, RoutedEventArgs e)
        {

            AddNewPatient childWindow = new AddNewPatient();
            Window parentWindow = Window.GetWindow((DependencyObject)sender);

            parentWindow.Opacity = 0.25;
            childWindow.Closed += (s, args) =>
            {
                parentWindow.Opacity = 1.0;
            };
            childWindow.Owner = parentWindow;
            childWindow.ShowDialog();
        }
        private void ShowPatientHistory_click(object sender, RoutedEventArgs e)
        {
            Button editButton = (Button)sender;
            long memberId = (long)editButton.Tag;

            AdminDoctorHistory childWindow = new AdminDoctorHistory(memberId);
            Window parentWindow = Window.GetWindow((DependencyObject)sender);

            parentWindow.Opacity = 0.25;
            childWindow.Closed += (s, args) =>
            {
                parentWindow.Opacity = 1.0;
            };
            childWindow.Owner = parentWindow;
            childWindow.ShowDialog();
        }
        private void ShowConfirm_click(object sender, RoutedEventArgs e)
        {
            Button editButton = (Button)sender;
            long memberId = (long)editButton.Tag;

            AreYouSure childWindow = new AreYouSure("Patient", memberId);
            Window parentWindow = Window.GetWindow((DependencyObject)sender);

            parentWindow.Opacity = 0.25;
            childWindow.Closed += (s, args) =>
            {
                parentWindow.Opacity = 1.0;
            };
            childWindow.Owner = parentWindow;
            childWindow.ShowDialog();
        }
        private void ShowEditPatient_click(object sender, RoutedEventArgs e)
        {
            Button editButton = (Button)sender;
            long memberIdToEdit = (long)editButton.Tag;

            EditPatient childWindow = new EditPatient(memberIdToEdit);
            Window parentWindow = Window.GetWindow((DependencyObject)sender);

            parentWindow.Opacity = 0.25;
            childWindow.Closed += (s, args) =>
            {
                parentWindow.Opacity = 1.0;
            };
            childWindow.Owner = parentWindow;
            childWindow.ShowDialog();
        }
        public void NavigateToAdminNotes_click(object sender, RoutedEventArgs e)
        {

            AdminNotes doctorNotesPage = new AdminNotes();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
            {
                mainFrame.Navigate(doctorNotesPage);
            }
        }
        public void NavigateToAdminDoctors_click(object sender, RoutedEventArgs e)
        {

            AdminDoctors doctorNotesPage = new AdminDoctors();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
            {
                mainFrame.Navigate(doctorNotesPage);
            }
        }
        public void NavigateToAdminStatus_click(object sender, RoutedEventArgs e)
        {

            AdminStatus doctorNotesPage = new AdminStatus();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
            {
                mainFrame.Navigate(doctorNotesPage);
            }
        }
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterMembers();
        }
        private void FilterMembers()
        {
            string searchText = txtSearch.Text.ToLower();

            List<Member> filteredMembers = members
                .Where(member => member.Name.ToLower().Contains(searchText))
                .ToList();

            RefreshDataGrid(filteredMembers);
        }
        private void RefreshDataGrid(List<Member> filteredMembers)
        {
            currentPage = 1;
            totalPages = (int)Math.Ceiling((double)filteredMembers.Count() / itemsPerPage);

            currentPageButton.Content = currentPage.ToString();
            lastPageButton.Content = totalPages.ToString();
            nextPageButton.Content = (currentPage + 1).ToString();

            int skip = (currentPage - 1) * itemsPerPage;
            membersDataGrid.ItemsSource = filteredMembers.Skip(skip).Take(itemsPerPage);
        }
    }
}
