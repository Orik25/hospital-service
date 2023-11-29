using EF;
using EF.service.impl;
using eHospital.AdminPages;
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
        public EditDoctor(long doctorId)
        {
            InitializeComponent();
            
            this.doctor = userService.FindById(doctorId);
            editDoctorFirstName.Text = doctor.FirstName;
            editDoctorLastName.Text = doctor.LastName;
            editDoctorPatronymic.Text = doctor.Patronymic;
            editDoctorType.Text = doctor.Type;
            editDoctorPhone.Text = doctor.Phone;
            editDoctorEmail.Text = doctor.Email;
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
                AdminDoctors homePage = new AdminDoctors();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                {
                    this.Close();
                    mainFrame.Navigate(homePage);
                }
            }
        }
        private bool ValidatePhone(string phone)
        {
            string phonePattern = @"^\+?\d{1,4}?[-.\s]?\(?\d{1,}\)?[-.\s]?\d{1,}[-.\s]?\d{1,}$";

            Regex regex = new Regex(phonePattern);

            if (!regex.IsMatch(phone) || phone.Equals("Телефон"))
            {
                ValidationErrorPhone.Text = "Телефон не валідний";
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
            return false;

        }
        private bool ValidateLastName(string lastName)
        {
            if (lastName.Equals("") || lastName.Equals("Прізвище"))
            {
                ValidationErrorLastName.Text = "Прізвище є обов'язковим";
                return false;
            }
            return true;
        }
        private bool ValidatePatronymic(string patronymic)
        {
            if (patronymic.Equals("") || patronymic.Equals("По батькові"))
            {
                ValidationErrorPatronymic.Text = "По батькові є обов'язковим";
                return false;
            }
            return true;
        }
        private bool ValidateType(string type)
        {
            if (type.Equals("") || type.Equals("Посада"))
            {
                ValidationErrorType.Text = "Посада є обов'язкова";
                return false;
            }
            return true;
        }
        private bool ValidateFirstName(string firstName)
        {
            if (firstName.Equals("") || firstName.Equals("Ім'я"))
            {
                ValidationErrorFirstName.Text = "Ім'я є обов'язковим";
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
        }
    }
}
