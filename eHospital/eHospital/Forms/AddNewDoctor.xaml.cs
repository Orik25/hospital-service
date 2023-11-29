﻿using EF;
using EF.service.impl;
using eHospital.AdminPages;
using eHospital.PatientPages;
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
    /// Interaction logic for AddNewDoctor.xaml
    /// </summary>
    public partial class AddNewDoctor : Window
    {
        private readonly UserServiceImpl userService;
        public AddNewDoctor()
        {
            InitializeComponent();
            this.userService = new UserServiceImpl(new EF.context.NeondbContext());
            this.KeyDown += Esc_KeyDown;
        }

        public void AddNewDoctor_click(object sender, RoutedEventArgs e)
        {
            

            HideValidationAlerts();
            bool validInputs = true;
            string firstName = addNewDoctorFirstName.Text;
            validInputs&=ValidateFirstName(firstName);
            string lastName = addNewDoctorLastName.Text;
            validInputs &=ValidateLastName(lastName);
            string patronymic = addNewDoctorPatronymic.Text;
            validInputs &= ValidatePatronymic(patronymic);
            string type = addNewDoctorType.Text;
            validInputs&=ValidateType(type);
            string password = addNewDoctorPassword.Text;
            validInputs&=ValidatePassword(password);
            string phone = addNewDoctorPhone.Text;
            validInputs&= ValidatePhone(phone);
            string email = addNewDoctorEmail.Text;
            validInputs&=ValidateEmail(email);
            if (validInputs)
            {
                userService.RegisterDoctor(new EF.DTO.User.UserDTO(email, firstName, lastName, patronymic, phone, password, type));
                AdminDoctors homePage = new AdminDoctors();
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
        private bool ValidatePassword(string password)
        {
            if (password.Length < 8)
            {
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
            ValidationErrorPassword.Text = string.Empty;
            ValidationErrorEmail.Text = string.Empty;
            ValidationErrorPhone.Text = string.Empty;
            ValidationErrorType.Text = string.Empty;
            ValidationErrorPatronymic.Text = string.Empty;
        }

        private void Input_GotFocus(object sender, RoutedEventArgs e)
        {
            TextBox textBox = (TextBox)sender;
            if (textBox.Text == "Ім'я" || textBox.Text == "Прізвище" || textBox.Text == "По батькові" || textBox.Text == "Посада" || textBox.Text == "Пошта" || textBox.Text == "Телефон" || textBox.Text == "Пароль")
            {
                textBox.Text = string.Empty;
            }
        }
        public void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
