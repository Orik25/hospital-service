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
    /// Interaction logic for EditDoctor.xaml
    /// </summary>
    public partial class EditDoctor : Window
    {
        private readonly UserServiceImpl userService = new UserServiceImpl(new EF.context.NeondbContext());
        private User doctor;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public EditDoctor(long doctorId)
        {
            InitializeComponent();
            this.KeyDown += Esc_KeyDown;
            try
            {
                this.doctor = userService.FindById(doctorId);

            }
            catch (Exception ex)
            {
                logger.Error($"Лікаря {doctorId} не знайдено при спробі редагування");
            }
            editDoctorFirstName.Text = doctor.FirstName;
            editDoctorLastName.Text = doctor.LastName;
            editDoctorPatronymic.Text = doctor.Patronymic;
            editDoctorType.Text = doctor.Type;
            editDoctorPhone.Text = doctor.Phone;
            editDoctorEmail.Text = doctor.Email;
            logger.Info("Форма редагування лікаря успішно відобразилась");

        }
        private void Esc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
                logger.Info("Форма редагування лікаря успішно закрилась");

            }
        }
        public void EditDoctor_click(object sender, RoutedEventArgs e)
        {
            HideValidationAlerts();
            bool validInputs = true;
            string editFirstName = editDoctorFirstName.Text;
            validInputs &= ValidateFirstName(editFirstName);
            string editLastName = editDoctorLastName.Text;
            validInputs &= ValidateLastName(editLastName);
            string editPatronymic = editDoctorPatronymic.Text;
            validInputs &= ValidatePatronymic(editPatronymic);
            string editType = editDoctorType.Text;
            validInputs &= ValidateType(editType);
            string editPhone = editDoctorPhone.Text;
            validInputs &= ValidatePhone(editPhone);
            string editEmail = editDoctorEmail.Text;
            validInputs &= ValidateEmail(editEmail);
            editEmail = editEmail.Trim();

            if (validInputs)
            {
                userService.EditUser(new EF.DTO.User.UpdateUserDTO(doctor.UserId, editEmail, editFirstName, editLastName, editPatronymic, editPhone, editType));
                logger.Info($"Лікаря {doctor.UserId} успішно відредаговано");

                AdminDoctors homePage = new AdminDoctors();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                {
                    this.Close();
                    logger.Info("Форма редагування лікаря успішно закрилась");

                    mainFrame.Navigate(homePage);
                    logger.Info("Адміністратор успішно перенаправлений на сторінку з лікарями");

                }
            }
            else
            {
                logger.Error("Адміністратор ввів не валідні дані при редагуванні лікаря");
            }
        }
        private bool ValidatePhone(string phone)
        {
            string phonePattern = @"^\+?\d{1,4}?[-.\s]?\(?\d{1,}\)?[-.\s]?\d{1,}[-.\s]?\d{1,}$";

            Regex regex = new Regex(phonePattern);

            if (!regex.IsMatch(phone) || phone.Equals("Телефон"))
            {
                ValidationErrorPhone.Text = "Телефон не валідний";
                logger.Error($"Адміністратор ввів не валідний телефон при редагуванні лікаря");

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
                logger.Error($"Адміністратор ввів не валідну пошту при редагуванні лікаря");

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
            if (email.Equals(doctor.Email))
            {
                return true;
            }
            ValidationErrorEmail.Text = "Пошта використовується";
            logger.Error($"Адміністратор ввів пошту, яка вже використвується, при редагуванні лікаря");

            return false;

        }
        private bool ValidateLastName(string lastName)
        {
            if (lastName.Equals("") || lastName.Equals("Прізвище"))
            {
                ValidationErrorLastName.Text = "Прізвище є обов'язковим";
                logger.Error($"Адміністратор не ввів прізвище при редагуванні лікаря");

                return false;
            }
            return true;
        }
        private bool ValidatePatronymic(string patronymic)
        {
            if (patronymic.Equals("") || patronymic.Equals("По батькові"))
            {
                ValidationErrorPatronymic.Text = "По батькові є обов'язковим";
                logger.Error($"Адміністратор не ввів по батькові при редагуванні лікаря");

                return false;
            }
            return true;
        }
        private bool ValidateType(string type)
        {
            if (type.Equals("") || type.Equals("Посада"))
            {
                ValidationErrorType.Text = "Посада є обов'язкова";
                logger.Error($"Адміністратор не ввів посаду при редагуванні лікаря");

                return false;
            }
            return true;
        }
        private bool ValidateFirstName(string firstName)
        {
            if (firstName.Equals("") || firstName.Equals("Ім'я"))
            {
                ValidationErrorFirstName.Text = "Ім'я є обов'язковим";
                logger.Error($"Адміністратор не ввів ім'я при редагуванні лікаря");

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
            ValidationErrorType.Text = string.Empty;
            ValidationErrorPatronymic.Text = string.Empty;
        }
        public void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
            logger.Info("Форма редагування лікаря успішно закрилась");

        }
    }
}
