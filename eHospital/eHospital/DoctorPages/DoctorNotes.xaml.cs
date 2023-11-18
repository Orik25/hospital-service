﻿using EF;
using EF.service.impl;
using eHospital.Forms;
using eHospital.LoginForms;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace eHospital.DoctorPages
{
    /// <summary>
    /// Interaction logic for DoctorNotes.xaml
    /// </summary>
    public partial class DoctorNotes : Page
    {
        public string patientName { get; set; } = "Олег";
        public int currentPage = 1;
        public int itemsPerPage;
        public int totalPages;

        private readonly AppointmentServiceImpl appointmentService;
        List<Member> members;
        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {

            if (currentPage != totalPages - 2)
            {
                int skip = currentPage * itemsPerPage;
                currentPage++;
                membersDataGrid.ItemsSource = members.Skip(skip).Take(itemsPerPage);
                currentPageButton.Content = currentPage.ToString();
                nextPageButton.Content = (currentPage + 1).ToString();
                currentPageButton.InvalidateVisual();
            }


        }
        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 2)
            {
                currentPage--;
                int skip = currentPage * itemsPerPage;
                membersDataGrid.ItemsSource = members.Skip(skip).Take(itemsPerPage);
                currentPageButton.Content = currentPage.ToString();
                nextPageButton.Content = (currentPage + 1).ToString();
                currentPageButton.InvalidateVisual();
            }

        }
        public DoctorNotes()
        {
            InitializeComponent();
            this.appointmentService = new AppointmentServiceImpl(new EF.context.NeondbContext());
            currentPage = 1;
               
            List<Appointment> appointments = appointmentService.GetAppointmentsByUserId(App.UserId);
            appointments = GetSortedAppointments(appointments);
            this.members = MapToMemberList(appointments);

           

            itemsPerPage = 4; 
            int totalItems = this.members.Count(); 

            totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
            currentPageButton.Content = currentPage.ToString();
            currentPageButton.InvalidateVisual();
            lastPageButton.Content = totalPages.ToString();
            nextPageButton.Content = (currentPage + 1).ToString();
            lastPageButton.InvalidateVisual();
            membersDataGrid.ItemsSource = members.Take(itemsPerPage); 

            

        }

        private List<Appointment> GetSortedAppointments(List<Appointment> appointments)
        {
            return appointments = appointments
                .OrderByDescending(appointment => appointment.DateAndTime.Date)  
                .ThenByDescending(appointment => appointment.DateAndTime.TimeOfDay) 
                .ToList();
        }

        private List<Member> MapToMemberList(List<Appointment> appointments)
        {
            List <Member> members = new List<Member>();
            foreach (var appointment in appointments)
            {
                Member newMember = new Member();
                newMember.Name = appointment.PatientRefNavigation.FirstName +" "+ appointment.PatientRefNavigation.LastName;
                newMember.Status = appointment.Status;
                newMember.Comment = appointment.Message;
                newMember.Date = appointment.DateAndTime.ToShortDateString();
                newMember.Time = appointment.DateAndTime.ToShortTimeString()+ "-" + appointment.DateAndTime.AddHours(1).ToShortTimeString();
                members.Add(newMember);
            }
            return members;
        }

        public void ShowDoctorProfile_click(object sender, RoutedEventArgs e)
        {

            Profile childWindow = new Profile();
            Window parentWindow = Window.GetWindow((DependencyObject)sender);

            parentWindow.Opacity = 0.25;
            childWindow.Closed += (s, args) =>
            {
                parentWindow.Opacity = 1.0;
            };
            childWindow.Show();
        }
        private void txtSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        public class Member
        {
            public long id { get; set; }
            public string Name { get; set; } = null!;

            public string Date { get; set; } = null!;
            public string Time { get; set; } = null!;
            public string Comment { get; set; } = null!;
            public string Status { get; set; } = null!;

        }

      
    }
}
