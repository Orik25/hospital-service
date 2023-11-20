using EF;
using EF.service.impl;
using eHospital.AdminPages;
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

namespace eHospital.Forms
{
    /// <summary>
    /// Interaction logic for AddNewPatient.xaml
    /// </summary>
    public partial class AddNewPatient : Window
    {
        private readonly UserServiceImpl userService;
        public AddNewPatient()
        {
            InitializeComponent();
            this.userService = new UserServiceImpl(new EF.context.NeondbContext());

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
            string firstName = addNewPatientFirstName.Text;
            string lastName = addNewPatientLastName.Text;
            string password = addNewPatientPassword.Text;
            string phone = addNewPatientPhone.Text;
            string email = addNewPatientEmail.Text;
            User findedUser = null;
            try
            {
                findedUser = userService.FindByEmail(email);
            }
            catch (ApplicationException ex)
            {
                userService.RegisterPatient(new EF.DTO.User.UserDTO(email, firstName, lastName, phone, password));
                AdminDoctors homePage = new AdminDoctors();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                {
                    this.Close();
                    mainFrame.Navigate(homePage);
                }

            }
            if (findedUser != null)
            {
                MessageBox.Show("Дана пошта вже використовується");
            }

        }
        public void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
