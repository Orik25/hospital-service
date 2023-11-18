using EF;
using EF.service.impl;
using eHospital.AdminPages;
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

namespace eHospital.PatientPages
{
    /// <summary>
    /// Interaction logic for PatientNotes.xaml
    /// </summary>
    public partial class PatientNotes : Page
    {
        public string patientName { get; set; } = "Олег";
        public int currentPage = 1;
        public int itemsPerPage;
        public int totalPages;

        private readonly AppointmentServiceImpl appointmentService;
        List<Member> members;
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
        public PatientNotes()
        {
            InitializeComponent();
            this.appointmentService = new AppointmentServiceImpl(new EF.context.NeondbContext());
            currentPage = 1;

            List<Appointment> appointments = appointmentService.GetAppointmentsByUserId(App.UserId);
            appointments = GetSortedAppointments(appointments);
            members = MapToMemberList(appointments);

            /*membersDataGrid.ItemsSource = members;*/

            itemsPerPage = 3; // Задайте бажану кількість рядків на сторінці
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
        private List<Appointment> GetSortedAppointments(List<Appointment> appointments)
        {
            return appointments = appointments
                .OrderByDescending(appointment => appointment.DateAndTime.Date)
                .ThenByDescending(appointment => appointment.DateAndTime.TimeOfDay)
                .ToList();
        }
        private List<Member> MapToMemberList(List<Appointment> appointments)
        {
            List<Member> members = new List<Member>();
            foreach (var appointment in appointments)
            {
                Member newMember = new Member();
                newMember.Id = appointment.AppointmentId;
                newMember.Name = appointment.DoctorRefNavigation.FirstName + " " + appointment.DoctorRefNavigation.LastName;
                newMember.Status = appointment.Status;
                newMember.Type = appointment.DoctorRefNavigation.Type;
                if(appointment.Message != null)
                {
                    newMember.Comment = appointment.Message;
                }
                else
                {
                    newMember.Comment = " ";
                }
               
                newMember.Date = appointment.DateAndTime.ToShortDateString();
                newMember.Time = appointment.DateAndTime.ToShortTimeString() + "-" + appointment.DateAndTime.AddHours(1).ToShortTimeString();
                members.Add(newMember);
            }
            return members;
        }
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
        public void ShowPatientProfile_click(object sender, RoutedEventArgs e)
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

        public class Member
        {
            public long Id { get; set; }
            public string Name { get; set; } = null!;
            public string Type { get; set; } = null!;
            public string Date { get; set; } = null!;
            public string Time { get; set; } = null!;
            public string Comment { get; set; } = null!;
            public string Status { get; set; } = null!;

        }
    }
}
