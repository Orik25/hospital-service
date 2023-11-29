using EF.context;
using EF.service.impl;
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

namespace eHospital.LoginForms
{
    /// <summary>
    /// Interaction logic for ForgotPassword.xaml
    /// </summary>
    public partial class ForgotPassword : Page
    {
        private readonly UserServiceImpl userService;
        public ForgotPassword()
        {
            InitializeComponent();
            this.userService = new UserServiceImpl(new NeondbContext());
        }
        public void NavigateToLoginPage_click(object sender, RoutedEventArgs e)
        {
            Login homePage = new Login();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
            {
                mainFrame.Navigate(homePage);
            }
        }

        public void SendNewPassword_click(object sender, RoutedEventArgs e)
        {

            String email = forgotPasswordEmail.Text;
            try
            {
                userService.ChangePasswordByEmail(email);
                Login homePage = new Login();
                var mainWindow = Application.Current.MainWindow as MainWindow;
                if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
                {
                    mainFrame.Navigate(homePage);
                }

            }
            catch (ApplicationException ex) {
                ErrorTextBlock.Text = "Юзера з поштою: " + email + " не знайдено(";
                ErrorBorder.Visibility = Visibility.Visible;
            }
            catch(Exception ex)
            {
                ErrorTextBlock.Text = "Виникли технічні проблеми!\nCпробуйте пізніше)";
                ErrorBorder.Visibility = Visibility.Visible;
            }
        }
    }

}
