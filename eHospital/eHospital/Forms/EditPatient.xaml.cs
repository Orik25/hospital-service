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

namespace eHospital.Forms
{
    /// <summary>
    /// Interaction logic for EditPatient.xaml
    /// </summary>
    public partial class EditPatient : Window
    {
        private readonly UserServiceImpl userService = new UserServiceImpl(new EF.context.NeondbContext());
        private User patient;
        public EditPatient(long id)
        {
            InitializeComponent();
            this.patient = userService.FindById(id);
            editPatientFirstName.Text = patient.FirstName;
            editPatientLastName.Text = patient.LastName;
            editPatientPhone.Text = patient.Phone;
            editPatientEmail.Text = patient.Email;
            this.KeyDown += Esc_KeyDown;
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
                AdminPatients homePage = new AdminPatients();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                {
                    this.Close();
                    mainFrame.Navigate(homePage);
                }
            }
        }
        private void Esc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
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
            if (email.Equals(patient.Email))
            {
                return true;
            }
            ValidationErrorEmail.Text = "Користувач з такою поштою вже інсує";
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
        }
        public void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
