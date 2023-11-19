using EF;
using EF.service.impl;
using eHospital.LoginForms;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for PatientNewAppointment.xaml
    /// </summary>
    public partial class PatientNewAppointment : Window
    {
        public User SelectedDoctor { get; set; }
        public DateTime SelectedDate { get; set; }
        public DateTime SelectedTime { get; set; }

        private readonly UserServiceImpl userService = new UserServiceImpl(new EF.context.NeondbContext());
        private readonly AppointmentServiceImpl appointmentService = new AppointmentServiceImpl(new EF.context.NeondbContext());
        private readonly List<User> doctors;
        private List<DateTime> dates;
        private List<DateTime> times;

        public PatientNewAppointment()
        {
            InitializeComponent();

            doctors = userService.GetDoctors(); 
            doctorComboBox.ItemsSource = doctors;

            dates = GetListOfDates();
            dateComboBox.ItemsSource = dates;


        }

        private List<DateTime> GetListOfDates()
        {
            List<DateTime> dates = new List<DateTime>();
            DateTime currentDate = DateTime.Now;
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
        private void DoctorComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if(doctorComboBox.SelectedItem as User != null)
            {
                SelectedDoctor = doctorComboBox.SelectedItem as User;
                doctorComboBox.Text = SelectedDoctor.FirstName + " "+ SelectedDoctor.LastName +" "+ SelectedDoctor.Patronymic + " (" + SelectedDoctor.Type + ")";
            }

        }

        private void DatesComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (dateComboBox.SelectedItem != null)
            {
                SelectedDate  = (DateTime)dateComboBox.SelectedItem;
                dateComboBox.Text = SelectedDate.ToShortDateString();
                if(SelectedDoctor == null)
                {
                    MessageBox.Show("Виберіть лікаря!");
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
            if (SelectedDoctor == null || SelectedTime == new DateTime())
            {
                MessageBox.Show("Заповніть всі дані");
            }
            else
            {
                appointmentService.AddNew(new EF.DTO.Appointment.AppointmentDTO(SelectedTime, "", App.UserId, SelectedDoctor.UserId));
                PatientNotes homePage = new PatientNotes();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                {
                    this.Close();
                    mainFrame.Navigate(homePage);
                }
            }
        }

        public void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
