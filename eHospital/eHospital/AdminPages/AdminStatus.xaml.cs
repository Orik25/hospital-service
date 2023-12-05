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
using EF.context;
using EF.service.impl;
using eHospital.DoctorPages;
using eHospital.Forms;
using NLog;

namespace eHospital.AdminPages
{
    /// <summary>
    /// Interaction logic for AdminStatus.xaml
    /// </summary>
    public partial class AdminStatus : Page
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public AdminStatus()
        {
            InitializeComponent();
            var context = new NeondbContext();
            AppointmentServiceImpl appointmentServiceImpl = new AppointmentServiceImpl(context);
            UserServiceImpl userServiceImpl = new UserServiceImpl(context);
            NumberOfDoctors.Text = userServiceImpl.GetNumberOfDoctors().ToString();
            logger.Info("Успішно отримано загальну кількість лікарів");

            NumberOfPatients.Text = userServiceImpl.GetNumberOfPatients().ToString();
            logger.Info("Успішно отримано загальну кількість пацієнтів");

            NumberOfAppointments.Text = appointmentServiceImpl.GetNumberOfAppointments().ToString();
            logger.Info("Успішно отримано загальну кількість записів");

            logger.Info("Сторінка із статистикою успішно відобразилась");

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
            childWindow.Owner = parentWindow;
            childWindow.ShowDialog();
        }


        public void NavigateToAdminNotes_click(object sender, RoutedEventArgs e)
        {

            AdminNotes doctorNotesPage = new AdminNotes();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
            {
                mainFrame.Navigate(doctorNotesPage);
                logger.Info("Адміністратора успішно перенаправлено на сторінку із записами");

            }
        }

        public void NavigateToAdminPatients_click(object sender, RoutedEventArgs e)
        {

            AdminPatients doctorNotesPage = new AdminPatients();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
            {
                mainFrame.Navigate(doctorNotesPage);
                logger.Info("Адміністратора успішно перенаправлено на сторінку із пацієнтами");

            }
        }

        public void NavigateToAdminDoctors_click(object sender, RoutedEventArgs e)
        {

            AdminDoctors doctorNotesPage = new AdminDoctors();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
            {
                mainFrame.Navigate(doctorNotesPage);
                logger.Info("Адміністратора успішно перенаправлено на сторінку із лікарями");

            }
        }

    }
}
