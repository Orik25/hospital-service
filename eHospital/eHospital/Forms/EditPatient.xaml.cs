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
        }
        public void EditPatient_click(object sender, RoutedEventArgs e)
        {
            string editFirstName = editPatientFirstName.Text;
            string editLastName = editPatientLastName.Text;
            string editPhone = editPatientPhone.Text;
            string editEmail = editPatientEmail.Text;
            editEmail = editEmail.Trim();

            if (editEmail.Equals(patient.Email))
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
            else
            {
                User temp = null;
                try
                {
                    temp = userService.FindByEmail(editEmail);
                }
                catch (ApplicationException ex)
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
                if (temp != null)
                {
                    MessageBox.Show("Дана пошта вже використовується");

                }
            }
        }
        public void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
