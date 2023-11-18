using EF;
using EF.service.impl;
using eHospital.LoginForms;
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
    /// Interaction logic for Profile.xaml
    /// </summary>
    public partial class Profile : Window
    {
        private readonly UserServiceImpl userService;
        public Profile()
        {
            InitializeComponent();
            this.userService = new UserServiceImpl(new EF.context.NeondbContext());
            User user = userService.FindById(App.UserId);
           
     
            profileFirstName.Text = user.FirstName.ToString(); 
            profileLastName.Text = user.LastName.ToString(); 
            profileEmail.Text = user.Email.ToString();
            profilePatronymic.Text = user?.Patronymic.ToString();
            profilePhone.Text = user?.Phone.ToString();
            profileType.Text = user?.Type.ToString();

        }
        public void Logout_click(object sender, RoutedEventArgs e)
        {
            Login homePage = new Login();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
            {
                this.Close();
                mainFrame.Navigate(homePage);
            }
        }
        public void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
