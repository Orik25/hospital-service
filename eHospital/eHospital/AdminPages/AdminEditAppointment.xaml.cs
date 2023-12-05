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
using NLog;

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

        private static Logger logger = LogManager.GetCurrentClassLogger();


        public AdminEditAppointment(long appointmentId)
        {
            InitializeComponent();
            try
            {
                this.appointmentFromDB = appointmentService.FindById(appointmentId);
                logger.Info($"Запис {appointmentId} успішно знайдено");


            }
            catch (ApplicationException ex)
            {
                logger.Info($"Запис {appointmentId} не знайдено");

            }

            doctors = userService.GetDoctors();
            logger.Info("Успішно отримано список лікарів");

            doctorComboBox.ItemsSource = doctors;
            SelectedDoctor = appointmentFromDB.DoctorRefNavigation;
            dateComboBox.SelectedItem = SelectedDoctor;
            doctorComboBox.Text = SelectedDoctor.FirstName + " " + SelectedDoctor.LastName + " " + SelectedDoctor.Patronymic + " (" + SelectedDoctor.Type + ")";

            patients = userService.GetPatients();
            logger.Info("Успішно отримано список пацієнтів");

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
            logger.Info("Успішно отримано список вільних годин");

            timeComboBox.ItemsSource = times;
            SelectedTime = appointmentFromDB.DateAndTime;
            timeComboBox.SelectedItem = SelectedTime;
            timeComboBox.Text = SelectedDate.ToShortTimeString();
            this.KeyDown += Esc_KeyDown;

            logger.Info("Форма редагування запису успішно відобразилась");


        }
        private void Esc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
                logger.Info("Форма редагування запису успішно закрилась");

            }
        }
        private void PatientComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (patientComboBox.SelectedItem as User != null)
            {
                SelectedPatient = patientComboBox.SelectedItem as User;
                patientComboBox.Text = SelectedPatient.FirstName + " " + SelectedPatient.LastName;
                logger.Info($"Адміністратор обрав пацієнта {SelectedPatient.FirstName} {SelectedPatient.LastName}");
            }
            else
            {
                logger.Warn($"Адміністратор не обрав пацієнта");
            }

        }
        private void DoctorComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (doctorComboBox.SelectedItem as User != null)
            {
                SelectedDoctor = doctorComboBox.SelectedItem as User;
                logger.Info($"Адміністратор обрав лікаря {SelectedDoctor.FirstName} {SelectedDoctor.LastName} {SelectedDoctor.Patronymic} ({SelectedDoctor.Type})");
            }
            else
            {
                logger.Warn($"Адміністратор не обрав лікаря");
            }

        }
        private void DatesComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (dateComboBox.SelectedItem != null)
            {
                SelectedDate = (DateTime)dateComboBox.SelectedItem;
                dateComboBox.Text = SelectedDate.ToShortDateString();
                logger.Info($"Адміністратор обрав дату");

                if (SelectedDoctor == null)
                {
                    logger.Warn($"Адміністратор не обрав лікаря");
                    MessageBox.Show("Виберіть лікаря!");
                }
                else
                {
                    times = appointmentService.GetFreeHoursByDoctorId(SelectedDoctor.UserId, SelectedDate);
                    timeComboBox.ItemsSource = times;
                    logger.Info($"Відобразився список вільних годин");

                }

            }
            else
            {
                logger.Warn($"Адміністратор не обрав дати");
            }

        }
        private void TimesComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (timeComboBox.SelectedItem != null)
            {
                SelectedTime = (DateTime)timeComboBox.SelectedItem;
                timeComboBox.Text = SelectedTime.ToShortTimeString(); 
                logger.Info($"Адміністратор обрав {SelectedTime.ToShortTimeString()}");
            }
            else
            {
                logger.Warn($"Адміністратор не обрав час");
            }
        }
        public void EditAppointment_click(object sender, RoutedEventArgs e)
        {
            if (SelectedDoctor == null || SelectedPatient == null || SelectedTime == new DateTime())
            {
                MessageBox.Show("Заповніть всі дані");
                logger.Warn($"Адміністратор не обрав всі дані");

            }
            else
            {
                appointmentService.Update(new EF.DTO.Appointment.AppointmentDTO(appointmentFromDB.AppointmentId,SelectedTime, appointmentFromDB.Message, SelectedPatient.UserId, SelectedDoctor.UserId));
                logger.Info($"Адміністратор успішно відредагував запис");

                AdminNotes homePage = new AdminNotes();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                {
                    this.Close();
                    logger.Info($"Форма редагування запису успішно закрилась");

                    mainFrame.Navigate(homePage);
                    logger.Info($"Адміністратор перенаправлений на сторінку записів");

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
            logger.Info("Успішно отримано список дат");

            return dates;
        }

        public void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
            logger.Info($"Форма редагування запису успішно закрилась");

        }
    }
}
