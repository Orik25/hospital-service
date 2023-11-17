using EF.context;
using EF.DTO.User;
using EF.service.impl;
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

namespace eHospital.LoginForms
{
    /// <summary>
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        private readonly UserServiceImpl userService;
        public Registration()
        {
            InitializeComponent();
            this.userService = new UserServiceImpl(new NeondbContext());
        }
        
        public void NavigateToLoginPage_click(object sender, RoutedEventArgs e)
        {
            Login homePage = new Login();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
            {
                mainFrame.Navigate(homePage);
            }
        }
        public void Registration_Click(object sender, RoutedEventArgs e)
        {
            String firstName = registrationFirstName.Text;
            String lastName = registrationLastName.Text;
            String email = registrationEmail.Text;
            String password = registrationPassword.Text;
            String phone = registrationPhone.Text;

            try
            {
                userService.FindByEmail(email);
                MessageBox.Show("Користувач з поштою: "+email+" вже інсує(");
            }
            catch (ApplicationException ex) {
                UserDTO newUser = new UserDTO(email, firstName, lastName, phone, password);
                userService.RegisterPatient(newUser);
                MessageBox.Show(firstName + ", Ви успішно зареєструвались");
                Login homePage = new Login();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                {
                    mainFrame.Navigate(homePage);
                }
            }
            
        }

        private void FirstNameInput_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Ім'я")
            {
                textBox.Text = string.Empty;
            }
        }

        private void LastNameInput_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Прізвище")
            {
                textBox.Text = string.Empty;
            }
        }

        private void EmailInput_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Пошта")
            {
                textBox.Text = string.Empty;
            }
        }

        private void PasswordInput_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Пароль")
            {
                textBox.Text = string.Empty;
            }
        }

        private void PhoneInput_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Телефон")
            {
                textBox.Text = string.Empty;
            }
        }
    }
}
