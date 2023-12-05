using EF.service.impl;
using EF;
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
using System.Numerics;
using eHospital.AdminPages;
using System.Text.RegularExpressions;
using NLog;

namespace eHospital.Forms
{
    /// <summary>
    /// Interaction logic for EditPatient.xaml
    /// </summary>
    public partial class EditPatient : Window
    {
        private readonly UserServiceImpl userService = new UserServiceImpl(new EF.context.NeondbContext());
        private User patient;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public EditPatient(long id)
        {
            InitializeComponent();
            try
            {
                this.patient = userService.FindById(id);

            }
            catch (Exception ex)
            {
                logger.Error($"Пацієнт {id} не знайдено при спробі редагування");
            }
            editPatientFirstName.Text = patient.FirstName;
            editPatientLastName.Text = patient.LastName;
            editPatientPhone.Text = patient.Phone;
            editPatientEmail.Text = patient.Email;
            this.KeyDown += Esc_KeyDown;
            logger.Info("Форма редагування пацієнта успішно відобразилась");

        }
        public void EditPatient_click(object sender, RoutedEventArgs e)
        {
            

            HideValidationAlerts();
            bool validInputs = true;
            string editFirstName = editPatientFirstName.Text;
            validInputs&=ValidateFirstName(editFirstName);
            string editLastName = editPatientLastName.Text;
            validInputs &=ValidateLastName(editLastName);
            string editPhone = editPatientPhone.Text;
            validInputs &=ValidatePhone(editPhone);
            string editEmail = editPatientEmail.Text;
            editEmail = editEmail.Trim();
            validInputs&=ValidateEmail(editEmail);

            if (validInputs)
            {
                userService.EditUser(new EF.DTO.User.UpdateUserDTO(patient.UserId, editEmail, editFirstName, editLastName, editPhone));
                logger.Info($"Пацієнта {patient.UserId} успішно відредаговано");

                AdminPatients homePage = new AdminPatients();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                {
                    this.Close();
                    logger.Info("Форма редагування пацієнта успішно закрилась");

                    mainFrame.Navigate(homePage);
                    logger.Info("Адміністратор успішно перенаправлений на сторінку з пацієнтами");

                }
            }
            else
            {
                logger.Error("Адміністратор ввів не валідні дані при редагуванні пацієнта");
            }
        }
        private void Esc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
                logger.Info("Форма редагування пацієнта успішно закрилась");

            }
        }
        private bool ValidatePhone(string phone)
        {
            string phonePattern = @"^\+?\d{1,4}?[-.\s]?\(?\d{1,}\)?[-.\s]?\d{1,}[-.\s]?\d{1,}$";

            Regex regex = new Regex(phonePattern);

            if (!regex.IsMatch(phone) || phone.Equals("Телефон"))
            {
                ValidationErrorPhone.Text = "Телефон не валідний";
                logger.Error($"Адміністратор ввів не валідний телефон при редагуванні пацієнта");

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
                logger.Error($"Адміністратор ввів не валідну пошту при редагуванні пацієнта");

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
            if (email.Equals(patient.Email))
            {
                return true;
            }
            ValidationErrorEmail.Text = "Користувач з такою поштою вже інсує";
            logger.Error($"Адміністратор ввів пошту, яка вже використвується, при редагуванні пацієнта");

            return false;

        }
        private bool ValidateLastName(string lastName)
        {
            if (lastName.Equals("") || lastName.Equals("Прізвище"))
            {
                ValidationErrorLastName.Text = "Прізвище є обов'язковим";
                logger.Error($"Адміністратор не ввів прізвище при редагуванні пацієнта");

                return false;
            }
            return true;
        }
        private bool ValidateFirstName(string firstName)
        {
            if (firstName.Equals("") || firstName.Equals("Ім'я"))
            {
                ValidationErrorFirstName.Text = "Ім'я є обов'язковим";
                logger.Error($"Адміністратор не ввів ім'я при редагуванні пацієнта");

                return false;
            }
            return true;
        }
        private void HideValidationAlerts()
        {
            ValidationErrorFirstName.Text = string.Empty;
            ValidationErrorLastName.Text = string.Empty;
            ValidationErrorEmail.Text = string.Empty;
            ValidationErrorPhone.Text = string.Empty;
        }
        public void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
            logger.Info("Форма редагування пацієнта успішно закрилась");

        }
    }
}
