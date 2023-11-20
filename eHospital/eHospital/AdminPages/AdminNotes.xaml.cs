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
    /// Interaction logic for AdminNotes.xaml
    /// </summary>
    public partial class AdminNotes : Page
    {
        public int currentPage = 1;
        public int itemsPerPage;
        public int totalPages;
        private readonly AppointmentServiceImpl appointmentService = new AppointmentServiceImpl(new EF.context.NeondbContext());
        List<Member> members;
        List<Appointment> appointments;
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
        public AdminNotes()
        {
            InitializeComponent();
            currentPage = 1;
            
            this.appointments = appointmentService.GetAppointments();
            this.members = MapAppointmentsToMembers(this.appointments);

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
        private List<Member> MapAppointmentsToMembers(List<Appointment> appointments)
        {
            List<Member> returnMembers = new List<Member>();
            foreach (var appointment in appointments)
            {
                Member newMember = new Member();
                newMember.Id = appointment.AppointmentId;
                newMember.ParientName = appointment.PatientRefNavigation.FirstName+ " " + appointment.PatientRefNavigation.LastName;
                newMember.DoctorName = appointment.DoctorRefNavigation.FirstName+ " " + appointment.DoctorRefNavigation.LastName + " " + appointment.DoctorRefNavigation.Patronymic;
                newMember.DoctorType = appointment.DoctorRefNavigation.Type;
                newMember.Date = appointment.DateAndTime.ToShortDateString();
                newMember.Time = appointment.DateAndTime.ToShortTimeString() + "-" + appointment.DateAndTime.AddHours(1).ToShortTimeString();
                newMember.Message = appointment.Message;
                newMember.Status = appointment.Status;
                returnMembers.Add(newMember);
            }
            return returnMembers;
        }
        public class Member
        {
            public long Id { get; set; }
            public string ParientName { get; set; }
            public string DoctorName { get; set; }
            public string DoctorType { get; set; }
            public string Date { get; set; }
            public string Time { get; set; }
            public string Message { get; set; }
            public string Status { get; set; }

        }
        public void NavigateToAdminPatients_click(object sender, RoutedEventArgs e)
        {

            AdminPatients doctorNotesPage = new AdminPatients();
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
        public void ShowAdminProfile_click(object sender, RoutedEventArgs e)
        {

            Profile childWindow = new Profile();
            Window parentWindow = Window.GetWindow((DependencyObject)sender);

            parentWindow.Opacity = 0.25;
            childWindow.Closed += (s, args) =>
            {
                parentWindow.Opacity = 1.0;
            };
            childWindow.Show();
        }
        private void ShowEditAppointment_click(object sender, RoutedEventArgs e)
        {
            Button editButton = (Button)sender;
            long memberIdToEdit = (long)editButton.Tag;

            AdminEditAppointment childWindow = new AdminEditAppointment(memberIdToEdit);
            Window parentWindow = Window.GetWindow((DependencyObject)sender);

            parentWindow.Opacity = 0.25;
            childWindow.Closed += (s, args) =>
            {
                parentWindow.Opacity = 1.0;
            };
            childWindow.Show();
        }
        public void ShowAddAppointment_click(object sender, RoutedEventArgs e)
        {

            AdminCreateAppointment childWindow = new AdminCreateAppointment();
            Window parentWindow = Window.GetWindow((DependencyObject)sender);

            parentWindow.Opacity = 0.25;
            childWindow.Closed += (s, args) =>
            {
                parentWindow.Opacity = 1.0;
            };
            childWindow.Show();
        }
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            FilterMembers();
        }
        private void FilterMembers()
        {
            string searchText = txtSearch.Text.ToLower();

            List<Member> filteredMembers = members
                .Where(member => member.ParientName.ToLower().Contains(searchText) || member.DoctorName.ToLower().Contains(searchText))
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
