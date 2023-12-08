using Microsoft.Extensions.DependencyInjection;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace eHospital
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public static long UserId
        {
            get
            {
                if (Application.Current.Properties.Contains("UserId"))
                {
                    return (long)Application.Current.Properties["UserId"];
                }
                return 0; // Значення за замовчуванням, якщо властивість ще не була встановлена
            }
            set
            {
                Application.Current.Properties["UserId"] = value;
            }
        }
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            
            Current.DispatcherUnhandledException += AppDispatcherUnhandledException;
        }

        private void AppDispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            
            logger.Error($"Виникла неперехоплена помилка: {e.Exception.Message}", "Помилка", MessageBoxButton.OK, MessageBoxImage.Error);

            e.Handled = true;
        }
    }
}
