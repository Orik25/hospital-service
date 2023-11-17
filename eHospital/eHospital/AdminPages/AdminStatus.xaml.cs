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
using EF.context;
using EF.service.impl;

namespace eHospital.AdminPages
{
    /// <summary>
    /// Interaction logic for AdminStatus.xaml
    /// </summary>
    public partial class AdminStatus : Page
    {
        public AdminStatus()
        {
            InitializeComponent();
            var context = new NeondbContext();
            AppointmentServiceImpl appointmentServiceImpl = new AppointmentServiceImpl(context);
            UserServiceImpl userServiceImpl = new UserServiceImpl(context);
            NumberOfDoctors.Text = userServiceImpl.GetNumberOfDoctors().ToString();
            NumberOfPatients.Text = userServiceImpl.GetNumberOfPatients().ToString();
            NumberOfAppointments.Text = appointmentServiceImpl.GetNumberOfAppointments().ToString();

        }
    }
}
