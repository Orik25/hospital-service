using eHospital.Forms;
using eHospital.LoginForms;
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

namespace eHospital.DoctorPages
{
    /// <summary>
    /// Interaction logic for DoctorNotes.xaml
    /// </summary>
    public partial class DoctorNotes : Page
    {
        public string patientName { get; set; } = "Олег";
        public int currentPage = 1;
        public int itemsPerPage;
        public int totalPages;
        List<Member> members = new List<Member>();
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
        public DoctorNotes()
        {
            InitializeComponent();
            currentPage = 1;

            members.Add(new Member { Name = "Name1 Surname", Type = "Хірург", Number = "123123", PhoneNumber = "+38099999123", Email = "Активно", id = 23 });
            members.Add(new Member { Name = "Name2 Surname", Type = "Хірург", Number = "3453453", PhoneNumber = "38099999123", Email = "Активно", id = 23 });
            members.Add(new Member { Name = "Name3 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "Активно", id = 23 });
            members.Add(new Member { Name = "Name4 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name5 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name6 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name7 ", Type = "Хірург", Number = "123123", PhoneNumber = "+38099999123", Email = "asdasdau@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name8 Surname", Type = "Хірург", Number = "3453453", PhoneNumber = "38099999123", Email = "asjdiasd@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name9 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name10 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name11 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name12 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name13 Surname", Type = "Хірург", Number = "123123", PhoneNumber = "+38099999123", Email = "asdasdau@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name14 Surname", Type = "Хірург", Number = "3453453", PhoneNumber = "38099999123", Email = "asjdiasd@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name15 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name16 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name17 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name18 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name19 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name20 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name1 Surname", Type = "Хірург", Number = "123123", PhoneNumber = "+38099999123", Email = "asdasdau@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name2 Surname", Type = "Хірург", Number = "3453453", PhoneNumber = "38099999123", Email = "asjdiasd@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name3 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name4 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name5 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name6 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name7 ", Type = "Хірург", Number = "123123", PhoneNumber = "+38099999123", Email = "asdasdau@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name8 Surname", Type = "Хірург", Number = "3453453", PhoneNumber = "38099999123", Email = "asjdiasd@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name9 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name10 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name11 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name12 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name13 Surname", Type = "Хірург", Number = "123123", PhoneNumber = "+38099999123", Email = "asdasdau@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name14 Surname", Type = "Хірург", Number = "3453453", PhoneNumber = "38099999123", Email = "asjdiasd@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name15 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name16 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name17 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name18 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name19 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });
            members.Add(new Member { Name = "Name20 Surname", Type = "Хірург", Number = "567567", PhoneNumber = "38099999123", Email = "lkeofkwoek@gmail.com", id = 23 });

            /*membersDataGrid.ItemsSource = members;*/

            itemsPerPage = 4; // Задайте бажану кількість рядків на сторінці
            int totalItems = members.Count(); // Кількість всіх рядків, які ви відображаєте

            totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
            currentPageButton.Content = currentPage.ToString();
            currentPageButton.InvalidateVisual();
            lastPageButton.Content = totalPages.ToString();
            nextPageButton.Content = (currentPage + 1).ToString();
            lastPageButton.InvalidateVisual();
            membersDataGrid.ItemsSource = members.Take(itemsPerPage); // Перша сторінка

            // Обробник події для кнопки "Наступна сторінка"

        }

        public void ShowDoctorProfile_click(object sender, RoutedEventArgs e)
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
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public class Member
        {
            public string Name { get; set; }
            public string Type { get; set; }

            public string Number { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }

            public int id { get; set; }

        }

        private void membersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
