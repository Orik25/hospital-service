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
        Brush activeBrush;
        Brush defaultBrush;



        private void CheckAndShowEllipsis()
        {
            if (totalPages > 3)
            {
                if (currentPage > 2 && currentPage < totalPages - 1)
                {
                    ellipsisPanel1.Visibility = Visibility.Visible;
                    ellipsisPanel2.Visibility = Visibility.Visible;
                }
                else
                {
                    if (currentPage < 3)
                    {
                        ellipsisPanel1.Visibility = Visibility.Collapsed;
                        ellipsisPanel2.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        ellipsisPanel1.Visibility = Visibility.Visible; // Ховаємо стек панель із "..."
                        ellipsisPanel2.Visibility = Visibility.Collapsed; // Ховаємо стек панель із "..."
                    }
                }
            }

        }
        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {

            if (currentPage < totalPages-1)
            {
                currentPage++;
                int skip = (currentPage-1) * itemsPerPage;
                currentPageButton.Background = activeBrush;
                firstPageButton.Background = defaultBrush;
                membersDataGrid.ItemsSource = members.Skip(skip).Take(itemsPerPage);
                currentPageButton.Content = currentPage.ToString();
                currentPageButton.InvalidateVisual();
                CheckAndShowEllipsis();
            }
            else
            {
                if (currentPage == totalPages - 1)
                {
                    currentPage++;
                    int skip = (currentPage - 1) * itemsPerPage;
                    currentPageButton.Background = defaultBrush;
                    lastPageButton.Background = activeBrush;

                    membersDataGrid.ItemsSource = members.Skip(skip).Take(itemsPerPage);
                    //currentPageButton.Content = currentPage.ToString();
                    currentPageButton.InvalidateVisual();
                    CheckAndShowEllipsis();
                }
            }
            


        }
        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage == totalPages)
            {
                currentPage--;
                int skip = (currentPage-1) * itemsPerPage;
                membersDataGrid.ItemsSource = members.Skip(skip).Take(itemsPerPage);
                currentPageButton.Content = currentPage.ToString();
                currentPageButton.Background = activeBrush;
                lastPageButton.Background = defaultBrush;
                //nextPageButton.Content = (currentPage + 1).ToString();
                currentPageButton.InvalidateVisual();
                CheckAndShowEllipsis();
            }
            else
            {
                if (currentPage > 2)
                {
                    currentPage--;
                    int skip = (currentPage-1) * itemsPerPage;
                    membersDataGrid.ItemsSource = members.Skip(skip).Take(itemsPerPage);
                    currentPageButton.Content = currentPage.ToString();
                    //nextPageButton.Content = (currentPage + 1).ToString();
                    currentPageButton.InvalidateVisual();
                    CheckAndShowEllipsis();
                }
                else
                {
                    if (currentPage == 2)
                    {
                        currentPage--;
                        int skip = currentPage * itemsPerPage;
                        membersDataGrid.ItemsSource = members.Take(itemsPerPage);
                        firstPageButton.Background = activeBrush;
                        currentPageButton.Background = defaultBrush;
                        currentPageButton.InvalidateVisual();
                        CheckAndShowEllipsis();
                    }
                }
                
            }
            
            

        }
        public AdminNotes()
        {
            InitializeComponent();
            currentPage = 1;

            this.appointments = appointmentService.GetAppointments();
            this.members = MapAppointmentsToMembers(this.appointments);
            ColorConverter converter = new ColorConverter();
            Color color = (Color)ColorConverter.ConvertFromString("#9E9E9E");
            Color color2 = (Color)ColorConverter.ConvertFromString("#AD99DA");

            // Створення кисті з вказаним кольором
            activeBrush = new SolidColorBrush(color);
            defaultBrush = new SolidColorBrush(color2);
            itemsPerPage = 5;
            int totalItems = members.Count();

            totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
            if (totalPages > 3)
            {
                ellipsisPanel1.Visibility = Visibility.Collapsed;
                ellipsisPanel2.Visibility = Visibility.Visible;
            }
            else
            {
                ellipsisPanel1.Visibility = Visibility.Collapsed;
                ellipsisPanel2.Visibility = Visibility.Collapsed;
            }

            firstPageButton.Content = currentPage.ToString();
            firstPageButton.Background = activeBrush;
            currentPageButton.Content = (currentPage + 1).ToString();
            firstPageButton.InvalidateVisual();
            currentPageButton.InvalidateVisual();
            lastPageButton.Content = totalPages.ToString();

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
                newMember.ParientName = appointment.PatientRefNavigation.FirstName + " " + appointment.PatientRefNavigation.LastName;
                newMember.DoctorName = appointment.DoctorRefNavigation.FirstName + " " + appointment.DoctorRefNavigation.LastName + " " + appointment.DoctorRefNavigation.Patronymic;
                newMember.DoctorType = appointment.DoctorRefNavigation.Type;
                newMember.Date = appointment.DateAndTime.ToShortDateString();
                newMember.Time = appointment.DateAndTime.ToShortTimeString() + "-" + appointment.DateAndTime.AddHours(1).ToShortTimeString();
                newMember.Message = appointment.Message;
                newMember.Status = appointment.Status;
                returnMembers.Add(newMember);
            }
            List<Member> sortedMembers = returnMembers.OrderByDescending(member => DateTime.Parse(member.Date)).ToList();
            return sortedMembers;
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
        private void ShowConfirm_click(object sender, RoutedEventArgs e)
        {
            Button editButton = (Button)sender;
            long memberId = (long)editButton.Tag;

            AreYouSure childWindow = new AreYouSure("Appointment", memberId);
            Window parentWindow = Window.GetWindow((DependencyObject)sender);

            parentWindow.Opacity = 0.25;
            childWindow.Closed += (s, args) =>
            {
                parentWindow.Opacity = 1.0;
            };
            childWindow.Show();
        }

        private void ArchiveAppointment_click(object sender, RoutedEventArgs e)
        {
            Button editButton = (Button)sender;
            long memberId = (long)editButton.Tag;

            appointmentService.ArchiveById(memberId);
            AdminNotes doctorNotesPage = new AdminNotes();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
            {
                mainFrame.Navigate(doctorNotesPage);
            }
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
            //nextPageButton.Content = (currentPage + 1).ToString();

            int skip = (currentPage - 1) * itemsPerPage;
            membersDataGrid.ItemsSource = filteredMembers.Skip(skip).Take(itemsPerPage);
        }

        private void membersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void membersDataGrid_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}