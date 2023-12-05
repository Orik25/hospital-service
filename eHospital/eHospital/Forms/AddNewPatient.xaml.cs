using EF;
using EF.service.impl;
using eHospital.AdminPages;
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

namespace eHospital.Forms
{
    /// <summary>
    /// Interaction logic for AddNewPatient.xaml
    /// </summary>
    public partial class AddNewPatient : Window
    {
        private readonly UserServiceImpl userService;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public AddNewPatient()
        {
            InitializeComponent();
            this.userService = new UserServiceImpl(new EF.context.NeondbContext());
            this.KeyDown += Esc_KeyDown;
            logger.Info("Форма додавання нового пацієнта успішно відобразилась");

        }
        private void Input_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Ім'я" || textBox.Text == "Прізвище" || textBox.Text == "Пошта" || textBox.Text == "Телефон" || textBox.Text == "Пароль")
            {
                textBox.Text = string.Empty;
            }
        }
        public void AddNewPatient_click(object sender, RoutedEventArgs e)
        {
            

            HideValidationAlerts();
            bool validInputs = true;
            string firstName = addNewPatientFirstName.Text;
            validInputs &= ValidateFirstName(firstName);
            string lastName = addNewPatientLastName.Text;
            validInputs &= ValidateLastName(lastName);
            string password = addNewPatientPassword.Text;
            validInputs &= ValidatePassword(password);
            string phone = addNewPatientPhone.Text;
            validInputs &= ValidatePhone(phone);
            string email = addNewPatientEmail.Text;
            validInputs &= ValidateEmail(email);
            if (validInputs)
            {
                userService.RegisterPatient(new EF.DTO.User.UserDTO(email, firstName, lastName, phone, password));
                AdminPatients homePage = new AdminPatients();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                {
                    this.Close();
                    logger.Info("Форма додавання нового пацієнта успішно закрилась");

                    mainFrame.Navigate(homePage);
                    logger.Info("Адміністратор успішно перенаправлений на сторінку з пацієнтами");

                }
            }
            else
            {
                logger.Error("Адміністратор ввів не валідні дані при додаванні нового пацієнта");
            }

        }
        private void Esc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
                logger.Info("Форма додавання нового пацієнта успішно закрилась");

            }
        }
        private bool ValidatePhone(string phone)
        {
            string phonePattern = @"^\+?\d{1,4}?[-.\s]?\(?\d{1,}\)?[-.\s]?\d{1,}[-.\s]?\d{1,}$";

            Regex regex = new Regex(phonePattern);

            if (!regex.IsMatch(phone) || phone.Equals("Телефон"))
            {
                ValidationErrorPhone.Text = "Телефон не валідний";
                logger.Error($"Адміністратор ввів не валідний телефон при додаванні нового пацієнта");

                return false;
            }

            return true;
        }
        private bool ValidatePassword(string password)
        {
            if (password.Length < 8)
            {
                ValidationErrorPassword.Text = "Потрібно більше 8 символів";
                logger.Error($"Адміністратор ввів не валідний пароль при додаванні нового пацієнта");

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
                ValidationErrorEmail.Text = "Пошта не валідна";
                logger.Error($"Адміністратор ввів не валідну пошту при додаванні нового пацієнта");

                return false;
            }
            try
            {
                userService.FindByEmail(email);
            }
            catch (ApplicationException ex)
            {
                return true;
            }
            ValidationErrorEmail.Text = "Користувач з такою поштою вже інсує";
            logger.Error($"Адміністратор ввів пошту, яка вже використвується, при додаванні нового пацієнта");

            return false;

        }
        private bool ValidateLastName(string lastName)
        {
            if (lastName.Equals("") || lastName.Equals("Прізвище"))
            {
                ValidationErrorLastName.Text = "Прізвище є обов'язковим";
                logger.Error($"Адміністратор не ввів прізвище при додаванні нового пацієнта");

                return false;
            }
            return true;
        }
        private bool ValidateFirstName(string firstName)
        {
            if (firstName.Equals("") || firstName.Equals("Ім'я"))
            {
                ValidationErrorFirstName.Text = "Ім'я є обов'язковим";
                logger.Error($"Адміністратор не ввів ім'я при додаванні нового пацієнта");

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
        public void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
            logger.Info("Форма додавання пацієнта успішно закрилась");

        }

    }
}
