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
using System.IO;
using MimeKit;

namespace eHospital.LoginForms
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        private readonly string filePath = "rememberMeSettings.dat";
        private bool rememberMe = false;
        private readonly UserServiceImpl userService;
        public Login()
        {
            InitializeComponent();

            this.userService = new UserServiceImpl(new NeondbContext());
            if (File.Exists(filePath))
            {
                string fileContent = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(fileContent))
                {
                   
                    using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                    {

                        string username = reader.ReadString();
                        string password = reader.ReadString();
                        loginUsername.Focus();
                        loginUsername.Text= username;
                        loginPassword.Focus();
                        loginPassword.Password = password;
                        passwordPlaceHolder.Visibility = Visibility.Collapsed;
                        
                    }

                }
            }
            
            
        }
        private void loginPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            passwordPlaceHolder.Visibility = Visibility.Collapsed;
        }

        private void loginPassword_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(loginPassword.Password))
            {
                passwordPlaceHolder.Visibility = Visibility.Visible;
            }
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

        public void LoginLogic(string username, string password, bool rememberMe)
        {
            User user;
            try
            {
                user = userService.FindByEmail(username);
            }
            catch (ApplicationException ex)
            {
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

                if (rememberMe)
                {
                    using (BinaryWriter writer = new BinaryWriter(File.Open(filePath, FileMode.Create)))
                    {

                        writer.Write(username);
                        writer.Write(password);
                    }
                }
                else
                {
                    if (File.Exists(filePath))
                    {
                        string fileContent = File.ReadAllText(filePath);
                        if (!string.IsNullOrEmpty(fileContent))
                        {
                            string username2;
                            string password2;
                            using (BinaryReader reader = new BinaryReader(File.Open(filePath, FileMode.Open)))
                            {
                                username2 = reader.ReadString();
                                password2 = reader.ReadString();

                           

                            }
                            if (!loginPassword.Equals(password2) && !loginUsername.Equals(username2))
                            {
                                File.WriteAllBytes(filePath, new byte[0]);
                            }

                        }

                    }
                }

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

        public void Login_click(object sender, RoutedEventArgs e)
        {
            String username = loginUsername.Text;
            String password = loginPassword.Password;
         
            LoginLogic(username, password, rememberMe);
            
        }
        
        private void RememberMe_Checked(object sender, RoutedEventArgs e)
        {
            rememberMe = true;
        }

        private void RememberMe_Unchecked(object sender, RoutedEventArgs e)
        {
            rememberMe = false;
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
