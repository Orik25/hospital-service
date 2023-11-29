using EF.context;
using Microsoft.EntityFrameworkCore;
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
using EF.service.impl;
using EF;
using eHospital.AdminPages;
using eHospital.PatientPages;
using eHospital.DoctorPages;

namespace eHospital.LoginForms
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private readonly UserServiceImpl userService;
        public Login()
        {
            InitializeComponent();
            this.userService = new UserServiceImpl(new NeondbContext());

        }
        public void NavigateToRegistrationPage_Click(object sender, RoutedEventArgs e)
        {
            Registration registratioPage = new Registration();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
            {
                mainFrame.Navigate(registratioPage);
            }

        }

        public void Login_click(object sender, RoutedEventArgs e)
        {
            String username = loginUsername.Text;
            String password = loginPassword.Password;
            User user;
            try
            {
                user = userService.FindByEmail(username);
            }
            catch (ApplicationException ex) {
                ErrorTextBlock.Text = ex.Message;
                ErrorBorder.Visibility = Visibility.Visible;
                return;
            }
            string salt = BCrypt.Net.BCrypt.GenerateSalt(); 
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);
            bool passwordMatch = BCrypt.Net.BCrypt.Verify(password, user.Password);
            
            if (passwordMatch)
            {
                App.UserId = user.UserId;
                if (user.RoleRefNavigation.Name == "ROLE_ADMIN")
                {
                    AdminStatus adminStatusPage = new AdminStatus();
                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                    {
                        mainFrame.Navigate(adminStatusPage);
                       
                    }
                }
                else if (user.RoleRefNavigation.Name == "ROLE_PATIENT")
                {
                    PatientNotes patientNotesPage = new PatientNotes();
                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                    {
                        mainFrame.Navigate(patientNotesPage);
                    }
                }
                else if (user.RoleRefNavigation.Name == "ROLE_DOCTOR")
                {
                    DoctorNotes doctorNotesPage = new DoctorNotes();
                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                    {
                        mainFrame.Navigate(doctorNotesPage);
                    }

                }
                else
                {
                    ErrorTextBlock.Text = "У вас немає доступу до обліковго запису!";
                    ErrorBorder.Visibility = Visibility.Visible;
                }
            }
            else
            {
                ErrorTextBlock.Text = "Неправильний пароль";
                ErrorBorder.Visibility = Visibility.Visible;
            }

        }
        public void NavigateToForgotPasswordPage_Click(object sender, RoutedEventArgs e)
        {
            ForgotPassword forgotPasswordPage = new ForgotPassword();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
            {
                mainFrame.Navigate(forgotPasswordPage);
            }

        }
        private void LoginInput_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Пошта")
            {
                textBox.Text = string.Empty;
            }
        }
    }
}
