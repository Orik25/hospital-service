using EF.context;
using EF.service.impl;
using eHospital.AdminPages;
using Microsoft.EntityFrameworkCore.Storage;
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
        public AreYouSure(string type, long id)
        {
            InitializeComponent();
            this.type = type;
            this.id = id; 
            this.userService = new UserServiceImpl(new NeondbContext());
            this.appointmentService = new AppointmentServiceImpl(new NeondbContext());

            this.KeyDown += Esc_KeyDown;

        }
        private void Esc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
            }
        }
        public void Confirm_click(object sender, RoutedEventArgs e)
        {
            if (type.Equals("Doctor"))
            {
                userService.DeleteById(id);
                this.Close();
                AdminDoctors doctorNotesPage = new AdminDoctors();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                {
                    mainFrame.Navigate(doctorNotesPage);
                }
            }
            else if (type.Equals("Patient"))
            {
                userService.DeleteById(id);
                AdminPatients doctorNotesPage = new AdminPatients();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                {
                    mainFrame.Navigate(doctorNotesPage);
                }
                this.Close();
            }
            else if(type.Equals("Appointment"))
            {
                appointmentService.DeleteById(id);
                AdminNotes doctorNotesPage = new AdminNotes();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                {
                    mainFrame.Navigate(doctorNotesPage);
                }
                this.Close();
            }
            

        }
        public void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
