﻿using EF;
using EF.service.impl;
using eHospital.LoginForms;
using NLog;
using System;
using System.Collections.Generic;
using System.IO;
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
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public Profile()
        {
            InitializeComponent();
            this.userService = new UserServiceImpl(new EF.context.NeondbContext());
            User user = null;
            try
            {
                user = userService.FindById(App.UserId);
            }
            catch (Exception ex)
            {
                logger.Error($"Юзера {App.UserId} не знайдено при спробі відображення форми профілю користувача");
            }
     
            profileFirstName.Text = user.FirstName.ToString(); 
            profileLastName.Text = user.LastName.ToString(); 
            profileEmail.Text = user.Email.ToString();
            if(user.Patronymic != null)
            {
                profilePatronymic.Text = user.Patronymic.ToString();
            }
            else
            {
                profilePatronymic.Text = "";
            }
           
            profilePhone.Text = user?.Phone.ToString();
            if (user.Type != null) 
            {
                profileType.Text = user.Type.ToString();
            }
            else
            {
                profileType.Text = "";
            }
            this.KeyDown += Profile_KeyDown;
            logger.Info("Форма профілю користувача успішно відобразилась");

        }
        public void Logout_click(object sender, RoutedEventArgs e)
        {
            string filePath = "rememberMeSettings.dat";
            if (File.Exists(filePath))
            {
                string fileContent = File.ReadAllText(filePath);
                if (!string.IsNullOrEmpty(fileContent))
                {
                   
                   File.WriteAllBytes(filePath, new byte[0]);
                   logger.Info("Файл із запам'ятованою інформацією про користувача очищений");

                }

            }
            else
            {
                logger.Warn("Файл із запам'ятованою інформацією про користувача не знайдений.");
            }
            Login homePage = new Login();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
            {
                this.Close();
                logger.Info("Форма профілю користувача успішно закрилась");

                mainFrame.Navigate(homePage);
                logger.Info("Користувача успішно перенаправлено на сторінку логування");

            }
        }
        public void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
            logger.Info("Форма профілю користувача успішно закрилась");

        }
        private void Profile_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.Close();
                logger.Info("Форма профілю користувача успішно закрилась");

            }
        }
    }
}
