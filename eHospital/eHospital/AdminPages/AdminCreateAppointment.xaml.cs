using EF;
using EF.service.impl;
using eHospital.PatientPages;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace eHospital.AdminPages
{
    public partial class AdminCreateAppointment : Window
    {
        public User SelectedDoctor { get; set; }
        public User SelectedPatient { get; set; }
        public DateTime SelectedDate { get; set; }
        public DateTime SelectedTime { get; set; }
        
        private readonly UserServiceImpl userService = new UserServiceImpl(new EF.context.NeondbContext());
        private readonly AppointmentServiceImpl appointmentService = new AppointmentServiceImpl(new EF.context.NeondbContext());
        
        private readonly List<User> doctors;
        private readonly List<User> patients;
        private List<DateTime> dates;
        private List<DateTime> times;

        public AdminCreateAppointment()
        {
            InitializeComponent();
            doctors = userService.GetDoctors();
            doctorComboBox.ItemsSource = doctors;

            patients = userService.GetPatients();
            patientComboBox.ItemsSource = patients;

            dates = GetListOfDates();
            dateComboBox.ItemsSource = dates;

        }
        private void PatientComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (patientComboBox.SelectedItem as User != null)
            {
                SelectedPatient = patientComboBox.SelectedItem as User;
                patientComboBox.Text = SelectedPatient.FirstName + " " + SelectedPatient.LastName;
            }

        }
        private void DoctorComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (doctorComboBox.SelectedItem as User != null)
            {
                SelectedDoctor = doctorComboBox.SelectedItem as User;
                doctorComboBox.Text = SelectedDoctor.FirstName + " " + SelectedDoctor.LastName + " " + SelectedDoctor.Patronymic + " (" + SelectedDoctor.Type + ")";
            }

        }
        private void DatesComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (dateComboBox.SelectedItem != null)
            {
                SelectedDate = (DateTime)dateComboBox.SelectedItem;
                dateComboBox.Text = SelectedDate.ToShortDateString();
                if (SelectedDoctor == null)
                {
                    ErrorTextBlock.Text = "Виберіть лікаря!";
                    ErrorBorder.Visibility = Visibility.Visible;
                }
                else
                {
                    times = appointmentService.GetFreeHoursByDoctorId(SelectedDoctor.UserId, SelectedDate);
                    timeComboBox.ItemsSource = times;
                }

            }

        }
        private void TimesComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (timeComboBox.SelectedItem != null)
            {
                SelectedTime = (DateTime)timeComboBox.SelectedItem;
                timeComboBox.Text = SelectedTime.ToShortTimeString();
            }

        }
        public void AddNewAppointment_click(object sender, RoutedEventArgs e)
        {
            if (SelectedDoctor == null || SelectedPatient == null || SelectedTime == new DateTime())
            {
                ErrorTextBlock.Text = "Заповніть всі дані!";
                ErrorBorder.Visibility = Visibility.Visible;
            }
            else
            {
                appointmentService.AddNew(new EF.DTO.Appointment.AppointmentDTO(SelectedTime, "", SelectedPatient.UserId, SelectedDoctor.UserId));
                AdminNotes homePage = new AdminNotes();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                {
                    this.Close();
                    mainFrame.Navigate(homePage);
                }
            }
        }
        private List<DateTime> GetListOfDates()
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime currentDate = DateTime.Now;
            if (currentDate.Hour > 17)
            {
                currentDate = currentDate.AddDays(1);
            }
            for (int i = 0; i < 7; i++)
            {
                if (currentDate.DayOfWeek != DayOfWeek.Saturday && currentDate.DayOfWeek != DayOfWeek.Sunday)
                {
                    dates.Add(currentDate);
                }
                currentDate = currentDate.AddDays(1);
            }
            return dates;
        }

        public void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
