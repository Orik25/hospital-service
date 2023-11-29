using EF.service.impl;
using EF;
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

namespace eHospital.AdminPages
{
    /// <summary>
    /// Interaction logic for AdminEditAppointment.xaml
    /// </summary>
    public partial class AdminEditAppointment : Window
    {
        private Appointment appointmentFromDB;
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

        public AdminEditAppointment(long appointmentId)
        {
            InitializeComponent();
            this.appointmentFromDB = appointmentService.FindById(appointmentId);
            
            doctors = userService.GetDoctors();
            doctorComboBox.ItemsSource = doctors;
            SelectedDoctor = appointmentFromDB.DoctorRefNavigation;
            dateComboBox.SelectedItem = SelectedDoctor;
            doctorComboBox.Text = SelectedDoctor.FirstName + " " + SelectedDoctor.LastName + " " + SelectedDoctor.Patronymic + " (" + SelectedDoctor.Type + ")";

            patients = userService.GetPatients();
            patientComboBox.ItemsSource = patients;
            SelectedPatient = appointmentFromDB.PatientRefNavigation;
            patientComboBox.SelectedItem = SelectedPatient;
            patientComboBox.Text = SelectedPatient.FirstName + " " + SelectedPatient.LastName;


            dates = GetListOfDates();
            dateComboBox.ItemsSource = dates;
            SelectedDate = appointmentFromDB.DateAndTime;
            dateComboBox.SelectedItem = SelectedDate;
            dateComboBox.Text = SelectedDate.ToShortDateString();

            times = appointmentService.GetFreeHoursByDoctorId(SelectedDoctor.UserId, SelectedDate);
            timeComboBox.ItemsSource = times;
            SelectedTime = appointmentFromDB.DateAndTime;
            timeComboBox.SelectedItem = SelectedTime;
            timeComboBox.Text = SelectedDate.ToShortTimeString();
            this.KeyDown += Esc_KeyDown;

        }
        private void Esc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
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
        public void EditAppointment_click(object sender, RoutedEventArgs e)
        {
            if (SelectedDoctor == null || SelectedPatient == null || SelectedTime == new DateTime())
            {
                MessageBox.Show("Заповніть всі дані");
            }
            else
            {
                appointmentService.Update(new EF.DTO.Appointment.AppointmentDTO(appointmentFromDB.AppointmentId,SelectedTime, appointmentFromDB.Message, SelectedPatient.UserId, SelectedDoctor.UserId));
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
