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
        ObservableCollection<Patient> patients = new ObservableCollection<Patient>();
        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {

            if (currentPage != totalPages - 2)
            {
                int skip = currentPage * itemsPerPage;
                currentPage++;
                membersDataGrid.ItemsSource = patients.Skip(skip).Take(itemsPerPage);
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
                membersDataGrid.ItemsSource = patients.Skip(skip).Take(itemsPerPage);
                currentPageButton.Content = currentPage.ToString();
                nextPageButton.Content = (currentPage + 1).ToString();
                currentPageButton.InvalidateVisual();
            }

        }
        public AdminPatients()
        {
            InitializeComponent();

            currentPage = 1;

            patients.Add(new Patient { Name = "Patient Name1", Number = "123123", PhoneNumber = "+38099999123", Email = "asdasdau@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name2", Number = "3453453", PhoneNumber = "38099999123", Email = "asjdiasd@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name3", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name4", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name5", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name6", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name7", Number = "123123", PhoneNumber = "+38099999123", Email = "asdasdau@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name8", Number = "3453453", PhoneNumber = "38099999123", Email = "asjdiasd@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name9", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name1", Number = "123123", PhoneNumber = "+38099999123", Email = "asdasdau@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name2", Number = "3453453", PhoneNumber = "38099999123", Email = "asjdiasd@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name3", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name4", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name5", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name6", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name7", Number = "123123", PhoneNumber = "+38099999123", Email = "asdasdau@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name8", Number = "3453453", PhoneNumber = "38099999123", Email = "asjdiasd@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name9", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });

            patients.Add(new Patient { Name = "Patient Name1", Number = "123123", PhoneNumber = "+38099999123", Email = "asdasdau@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name2", Number = "3453453", PhoneNumber = "38099999123", Email = "asjdiasd@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name3", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name4", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name5", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name6", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name7", Number = "123123", PhoneNumber = "+38099999123", Email = "asdasdau@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name8", Number = "3453453", PhoneNumber = "38099999123", Email = "asjdiasd@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name9", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });

            patients.Add(new Patient { Name = "Patient Name1", Number = "123123", PhoneNumber = "+38099999123", Email = "asdasdau@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name2", Number = "3453453", PhoneNumber = "38099999123", Email = "asjdiasd@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name3", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name4", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name5", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name6", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name7", Number = "123123", PhoneNumber = "+38099999123", Email = "asdasdau@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name8", Number = "3453453", PhoneNumber = "38099999123", Email = "asjdiasd@gmail.com", id = 23 });
            patients.Add(new Patient { Name = "Patient Name9", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });


            /*membersDataGrid.ItemsSource = members;*/

            itemsPerPage = 5; // Задайте бажану кількість рядків на сторінці
            int totalItems = patients.Count(); // Кількість всіх рядків, які ви відображаєте

            totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
            currentPageButton.Content = currentPage.ToString();
            currentPageButton.InvalidateVisual();
            lastPageButton.Content = totalPages.ToString();
            nextPageButton.Content = (currentPage + 1).ToString();
            lastPageButton.InvalidateVisual();
            membersDataGrid.ItemsSource = patients.Take(itemsPerPage);

            // Обробник події для кнопки "Наступна сторінка"
        }

        private void membersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        public class Patient
        {
            public string Name { get; set; }
            public string Number { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }

            public int id { get; set; }

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
    }
}
