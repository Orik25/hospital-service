using EF;
using EF.service.impl;
using eHospital.LoginForms;
using NLog;
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

        private static Logger logger = LogManager.GetCurrentClassLogger();


        public PatientNewAppointment()
        {
            InitializeComponent();

            doctors = userService.GetDoctors(); 
            doctorComboBox.ItemsSource = doctors;
            logger.Info($"Список лікарів відобразився успішно");


            dates = GetListOfDates();
            dateComboBox.ItemsSource = dates;
            logger.Info($"Список дат відобразився успішно");


            this.KeyDown += Esc_KeyDown;
            logger.Info($"Форма додавання нового запису успішно відобразилась");

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
        private void DoctorComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if(doctorComboBox.SelectedItem as User != null)
            {
                SelectedDoctor = doctorComboBox.SelectedItem as User;
                doctorComboBox.Text = SelectedDoctor.FirstName + " "+ SelectedDoctor.LastName +" "+ SelectedDoctor.Patronymic + " (" + SelectedDoctor.Type + ")";
                logger.Info($"Пацієнт обрав лікаря {SelectedDoctor.FirstName} {SelectedDoctor.LastName} {SelectedDoctor.Patronymic} ({SelectedDoctor.Type})");
            }
            else
            {
                logger.Warn($"Пацієнт не обрав лікаря");
            }

        }

        private void DatesComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (dateComboBox.SelectedItem != null)
            {
                SelectedDate  = (DateTime)dateComboBox.SelectedItem;
                dateComboBox.Text = SelectedDate.ToShortDateString();
                logger.Info($"Пацієнт обрав дату");

                if (SelectedDoctor == null)
                {
                    ErrorTextBlock.Text = "Виберіть лікаря!";
                    logger.Warn($"Пацієнт не обрав лікаря");

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
                logger.Warn($"Пацієнт не обрав дати");
            }

        }
        private void TimesComboBox_DropDownClosed(object sender, EventArgs e)
        {
            if (timeComboBox.SelectedItem != null)
            {
                SelectedTime = (DateTime)timeComboBox.SelectedItem;
                timeComboBox.Text = SelectedTime.ToShortTimeString();
                logger.Info($"Пацієнт обрав {SelectedTime.ToShortTimeString()}");

            }
            else
            {
                logger.Warn($"Пацієнт не обрав час");
            }

        }
        public void AddNewAppointment_click(object sender, RoutedEventArgs e)
        {
            if (SelectedDoctor == null || SelectedTime == new DateTime())
            {
                ErrorTextBlock.Text = "Заповніть всі дані!";
                logger.Warn($"Пацієнт не обрав всі дані");

                ErrorBorder.Visibility = Visibility.Visible;
            }
            else
            {
                appointmentService.AddNew(new EF.DTO.Appointment.AppointmentDTO(SelectedTime, "", App.UserId, SelectedDoctor.UserId));
                logger.Info($"Пацієнт успішно створив новий запис");

                PatientNotes homePage = new PatientNotes();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                {
                    this.Close();
                    logger.Info($"Форма додавання запису успішно закрита");

                    mainFrame.Navigate(homePage);
                    logger.Info($"Пацієнт перенаправлений на сторінку записів");

                }
            }
        }

        public void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
            logger.Info($"Форма додавання запису успішно закрита");

        }
        private void Esc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
                logger.Info($"Форма додавання запису успішно закрита");

            }
        }
    }
}
