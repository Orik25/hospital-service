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
            string editFirstName = editDoctorFirstName.Text;
            string editLastName = editDoctorLastName.Text;
            string editPatronymic = editDoctorPatronymic.Text;
            string editType = editDoctorType.Text;
            string editPhone = editDoctorPhone.Text;
            string editEmail = editDoctorEmail.Text;
            editEmail = editEmail.Trim();

            if (editEmail.Equals(doctor.Email))
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
            else
            {
                User temp = null;
                try
                {
                   temp = userService.FindByEmail(editEmail);
                }
                catch (ApplicationException ex) 
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
