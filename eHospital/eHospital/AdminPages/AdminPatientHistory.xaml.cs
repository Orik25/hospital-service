using EF;
using EF.service.impl;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace eHospital.AdminPages
{
    public partial class AdminPatientHistory : Window
    {
        public List<Record> Records { get; set; } = new List<Record>();
        private List<Appointment> appointmentsHistory;
        private readonly AppointmentServiceImpl appointmentService = new AppointmentServiceImpl(new EF.context.NeondbContext());

      
        public AdminPatientHistory(long doctorId)
        {
            InitializeComponent();
            this.appointmentsHistory = appointmentService.GetArchiveAppointmentsByUserId(doctorId);
            this.Records = MapAppointmentsHistoryToRecords(appointmentsHistory);
            membersDataGrid.ItemsSource = Records;
        }
        private List<Record> MapAppointmentsHistoryToRecords(List<Appointment> appointments)
        {
            List<Record> returnRecords = new List<Record>();
            foreach (Appointment appointment in appointments)
            {
                Record newRecord = new Record();
                newRecord.Name = appointment.PatientRefNavigation.FirstName + " " + appointment.PatientRefNavigation.LastName;
                newRecord.Date = appointment.DateAndTime.ToShortDateString();
                newRecord.Time = appointment.DateAndTime.ToShortTimeString() + "-" + appointment.DateAndTime.AddHours(1).ToShortTimeString();
                returnRecords.Add(newRecord);
            }
            return returnRecords;

        }
        public void Cancel_click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void membersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
        }
        public class Record
        {
            public string Name { get; set; }
            public string Date { get; set; }
            public string Time { get; set; }
        }
    }
}