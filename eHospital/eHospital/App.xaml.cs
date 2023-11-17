using Microsoft.Extensions.DependencyInjection;
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
    }
}
