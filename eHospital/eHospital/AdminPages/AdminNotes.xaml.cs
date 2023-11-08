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
        ObservableCollection<Appointment> appointments = new ObservableCollection<Appointment>();
        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {

            if (currentPage != totalPages - 2)
            {
                int skip = currentPage * itemsPerPage;
                currentPage++;
                membersDataGrid.ItemsSource = appointments.Skip(skip).Take(itemsPerPage);
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
                membersDataGrid.ItemsSource = appointments.Skip(skip).Take(itemsPerPage);
                currentPageButton.Content = currentPage.ToString();
                nextPageButton.Content = (currentPage + 1).ToString();
                currentPageButton.InvalidateVisual();
            }

        }
        public AdminNotes()
        {
            InitializeComponent();
            currentPage = 1;


            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });

            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });
            appointments.Add(new Appointment { ParientName = "Name1 Surname", DoctorName = "Doctor1 Name", PatientNumber = 1234, DoctorNumber = 4567, Date = "05.11.2023", DateTime = "13:00-13:30", Message = "Боюсь лікарів", id = 23 });


            itemsPerPage = 5; // Задайте бажану кількість рядків на сторінці
            int totalItems = appointments.Count(); // Кількість всіх рядків, які ви відображаєте

            totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
            currentPageButton.Content = currentPage.ToString();
            currentPageButton.InvalidateVisual();
            lastPageButton.Content = totalPages.ToString();
            nextPageButton.Content = (currentPage + 1).ToString();
            lastPageButton.InvalidateVisual();
            membersDataGrid.ItemsSource = appointments.Take(itemsPerPage); // Перша сторінка
        }
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        public class Appointment
        {
            public string ParientName { get; set; }
            public string DoctorName { get; set; }

            public int PatientNumber { get; set; }
            public int DoctorNumber { get; set; }
            public string Date { get; set; }
            public string DateTime { get; set; }
            public string Message { get; set; }

            public int id { get; set; }

        }
    }
}
