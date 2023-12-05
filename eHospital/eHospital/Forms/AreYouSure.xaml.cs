using EF.context;
using EF.service.impl;
using eHospital.AdminPages;
using Microsoft.EntityFrameworkCore.Storage;
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

namespace eHospital.Forms
{
    /// <summary>
    /// Interaction logic for AreYouSure.xaml
    /// </summary>
    public partial class AreYouSure : Window
    {
        private readonly UserServiceImpl userService;
        private readonly AppointmentServiceImpl appointmentService;
        private readonly string type;
        private readonly long id;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public AreYouSure(string type, long id)
        {
            InitializeComponent();
            this.type = type;
            this.id = id; 
            this.userService = new UserServiceImpl(new NeondbContext());
            this.appointmentService = new AppointmentServiceImpl(new NeondbContext());

            this.KeyDown += Esc_KeyDown;
            logger.Info($"Форма підтвердження видалення успішно відобразилась");


        }
        private void Esc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
                logger.Info($"Форма підтвердження видалення успішно закрилась");

            }
        }
        public void Confirm_click(object sender, RoutedEventArgs e)
        {
            if (type.Equals("Doctor"))
            {
                try
                {
                    userService.DeleteById(id);
                    logger.Info($"Форма підтвердження видалення успішно закрилась");

                }
                catch(Exception ex)
                {
                    logger.Error($"Лікаря {id} не знайдено при спробі видалення");

                }
                this.Close();
                AdminDoctors doctorNotesPage = new AdminDoctors();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                {
                    mainFrame.Navigate(doctorNotesPage);
                    logger.Info("Адміністратор успішно перенаправлений на сторінку з лікарями");

                }
                logger.Info($"Форма підтвердження видалення успішно закрилась");

            }
            else if (type.Equals("Patient"))
            {
                try
                {
                    userService.DeleteById(id);
                    logger.Info($"Форма підтвердження видалення успішно закрилась");

                }
                catch (Exception ex)
                {
                    logger.Error($"Пацієнта {id} не знайдено при спробі видалення");

                }
                AdminPatients doctorNotesPage = new AdminPatients();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                {
                    mainFrame.Navigate(doctorNotesPage);
                    logger.Info("Адміністратор успішно перенаправлений на сторінку з пацієнтами");

                }
                this.Close();
                logger.Info($"Форма підтвердження видалення успішно закрилась");

            }
            else if(type.Equals("Appointment"))
            {
                try
                {
                    userService.DeleteById(id);
                    logger.Info($"Форма підтвердження видалення успішно закрилась");

                }
                catch (Exception ex)
                {
                    logger.Error($"Запис {id} не знайдено при спробі видалення");

                }
                AdminNotes doctorNotesPage = new AdminNotes();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                {
                    mainFrame.Navigate(doctorNotesPage);
                    logger.Info("Адміністратор успішно перенаправлений на сторінку зі записами");

                }
                this.Close();
                logger.Info($"Форма підтвердження видалення успішно закрилась");

            }


        }
        public void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
            logger.Info($"Форма підтвердження видалення успішно закрилась");

        }
    }
}
