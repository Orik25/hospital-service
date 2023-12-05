using EF;
using EF.service.impl;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace eHospital.AdminPages
{
    public partial class AdminDoctorHistory : Window
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public List<Record> Records { get; set; } = new List<Record>();
        private List<Appointment> appointmentsHistory;
        private readonly AppointmentServiceImpl appointmentService = new AppointmentServiceImpl(new EF.context.NeondbContext());

        public AdminDoctorHistory(long oatientId)
        {
            InitializeComponent();
            this.appointmentsHistory = appointmentService.GetArchiveAppointmentsByUserId(oatientId);
            logger.Info($"Отримано список історії записів пацієнта {oatientId}");
            this.Records = MapAppointmentsHistoryToRecords(appointmentsHistory);
            
            membersDataGrid.ItemsSource = Records;
            this.KeyDown += Esc_KeyDown;

        }
        private void Esc_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                logger.Info($"Форма історії пацієнта успішно закрита");
                this.Close();
            }
        }
        private List<Record> MapAppointmentsHistoryToRecords(List<Appointment> appointments)
        {
            List<Record> returnRecords = new List<Record>();
            foreach (Appointment appointment in appointments)
            {
                Record newRecord = new Record();
                newRecord.Name = appointment.DoctorRefNavigation.FirstName + " " + appointment.DoctorRefNavigation.LastName + " " + appointment.DoctorRefNavigation.Patronymic;
                newRecord.Date = appointment.DateAndTime.ToShortDateString();
                newRecord.Type = appointment.DoctorRefNavigation.Type;
                newRecord.Time = appointment.DateAndTime.ToShortTimeString() + "-" + appointment.DateAndTime.AddHours(1).ToShortTimeString();
                returnRecords.Add(newRecord);
            }
            logger.Info($"Успішно отримано список записів для форми");
            return returnRecords;

        }
        public void Cancel_click(object sender, RoutedEventArgs e)
        {
            logger.Info($"Форма історії пацієнта успішно закрита");
            this.Close();
        }

        private void membersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
        }
    }
    public class Record
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Date { get; set; }
        public string Time { get; set; }
    }
}