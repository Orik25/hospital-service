using EF;
using EF.service.impl;
using eHospital.PatientPages;
using NLog;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace eHospital.AdminPages
{
    public partial class AdminCreateAppointment : Window
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
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
            logger.Info($"Список лікарів відобразився успішно");
            patients = userService.GetPatients();
            patientComboBox.ItemsSource = patients;
            logger.Info($"Список пацієнтів відобразився успішно");
            dates = GetListOfDates();
            dateComboBox.ItemsSource = dates;
            logger.Info($"Список дат відобразився успішно");
            this.KeyDown += Esc_KeyDown;
            logger.Info("Форма додавання нового лікаря успішно відобразилась");


        }
        private void Esc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
                logger.Info($"Форма додавання запису успішно закрита");
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
                doctorComboBox.Text = SelectedDoctor.FirstName + " " + SelectedDoctor.LastName + " " + SelectedDoctor.Patronymic + " (" + SelectedDoctor.Type + ")";
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
                    ErrorTextBlock.Text = "Виберіть лікаря!";
                    ErrorBorder.Visibility = Visibility.Visible;
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
        public void AddNewAppointment_click(object sender, RoutedEventArgs e)
        {
            if (SelectedDoctor == null || SelectedPatient == null || SelectedTime == new DateTime())
            {
                logger.Warn($"Адміністратор не обрав всі дані");
                ErrorTextBlock.Text = "Заповніть всі дані!";
                ErrorBorder.Visibility = Visibility.Visible;
            }
            else
            {
                appointmentService.AddNew(new EF.DTO.Appointment.AppointmentDTO(SelectedTime, "", SelectedPatient.UserId, SelectedDoctor.UserId));
                logger.Info($"Адміністратор успішно створив новий запис");
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
            logger.Info($"Список дат успішно згенеровано");
            return dates;
        }

        public void Cancel_click(object sender, RoutedEventArgs e)
        {
            logger.Info($"Форма додавання запису успішно закрита");
            this.Close();
        }
    }
}
