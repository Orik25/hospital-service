using EF.context;
using EF.DTO.User;
using EF.service.impl;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
        private static Logger logger = LogManager.GetCurrentClassLogger();
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
                logger.Info("Користувач повернувся на сторінку логування");
            }
        }
        public void Registration_Click(object sender, RoutedEventArgs e)
        {
            HideValidationAlerts();
            bool validInputs = true;
            String firstName = registrationFirstName.Text;
            validInputs &= ValidateFirstName(firstName);
            String lastName = registrationLastName.Text;
            validInputs &= ValidateLastName(lastName);
            String email = registrationEmail.Text;
            validInputs &= ValidateEmail(email);
            String password = registrationPassword.Text;
            validInputs &= ValidatePassword(password);
            String phone = registrationPhone.Text;
            validInputs &= ValidatePhone(phone);

            if (validInputs)
            {
                UserDTO newUser = new UserDTO(email, firstName, lastName, phone, password);
                userService.RegisterPatient(newUser);
                logger.Info($"Користувача {email} успішно зареєстровано");
                Login homePage = new Login();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                {
                    mainFrame.Navigate(homePage);
                }
            }
            else
            {
                logger.Error($"Користувач ввів не валідні данні при реєстрації");
            }
            
        }
        private bool ValidatePhone(string phone)
        {
            string phonePattern = @"^\+?\d{1,4}?[-.\s]?\(?\d{1,}\)?[-.\s]?\d{1,}[-.\s]?\d{1,}$";

            Regex regex = new Regex(phonePattern);

            if (!regex.IsMatch(phone) || phone.Equals("Телефон"))
            {
                logger.Error($"Користувач ввів не валідний телефон при реєстрації");
                ValidationErrorPhone.Text = "Телефон не валідний";
                return false;
            }
            
            return true;
        }
        private bool ValidatePassword(string password)
        {
            if (password.Length < 8)
            {
                logger.Error($"Користувач ввів не валідний телефон при реєстрації");
                ValidationErrorPassword.Text = "Потрібно більше 8 символів";
                return false;
            }
            return true;
        }
        private bool ValidateEmail(string email)
        {
            string emailPattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            Regex regex = new Regex(emailPattern);

            if (!regex.IsMatch(email) || email.Equals("Пошта"))
            {
                logger.Error($"Користувача ввів не валідну пошту при реєстрації");
                ValidationErrorEmail.Text = "Пошта не валідна";
                return false;
            }
            try
            {
                userService.FindByEmail(email);
            }
            catch(ApplicationException ex)
            {
                return true;
            }
            logger.Error($"Користувач ввів пошту яка вже існує при реєстрації");
            ValidationErrorEmail.Text = "Користувач з такою поштою вже інсує";
            return false;

        }
        private bool ValidateLastName(string lastName)
        {
            if (lastName.Equals("") || lastName.Equals("Прізвище"))
            {
                logger.Error($"Користувач не ввів прізвище при реєстрації");
                ValidationErrorLastName.Text = "Прізвище є обов'язковим";
                return false;
            }
            return true;
        }
        private bool ValidateFirstName(string firstName)
        {
            if (firstName.Equals("") || firstName.Equals("Ім'я"))
            {
                logger.Error($"Користувач не ввів ім'я при реєстрації");
                ValidationErrorFirstName.Text = "Ім'я є обов'язковим";
                return false;
            }
            return true;
        }
        private void HideValidationAlerts()
        {
            ValidationErrorFirstName.Text = string.Empty;
            ValidationErrorLastName.Text = string.Empty;
            ValidationErrorPassword.Text = string.Empty;
            ValidationErrorEmail.Text = string.Empty;
            ValidationErrorPhone.Text = string.Empty;   
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
