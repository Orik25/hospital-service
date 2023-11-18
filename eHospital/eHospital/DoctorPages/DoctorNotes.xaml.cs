using EF;
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
        Appointment nearestAppointment;
        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {

            if (currentPage != totalPages - 2)
            {
                int skip = currentPage * itemsPerPage;
                ++currentPage;
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
                --currentPage;
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
            members = MapToMemberList(appointments);



            this.nearestAppointment = GetNearestAppointment(appointments);
            SetNearestAppointment(nearestAppointment);
            
  
            
            itemsPerPage = 4; 
            int totalItems = members.Count(); 

            totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
            currentPageButton.Content = currentPage.ToString();
            currentPageButton.InvalidateVisual();
            lastPageButton.Content = totalPages.ToString();
            nextPageButton.Content = (currentPage + 1).ToString();
            lastPageButton.InvalidateVisual();
            membersDataGrid.ItemsSource = members.Take(itemsPerPage);

            txtSearch.TextChanged += txtSearch_TextChanged;

        }

        private Appointment GetNearestAppointment(List<Appointment> appointments)
        {
            DateTime now = DateTime.Now;

            var nearestAppointment = appointments
                .Where(appointment => appointment.Status == "active")
                .OrderBy(appointment => Math.Abs((appointment.DateAndTime - now).TotalMinutes))
                .FirstOrDefault();

            return nearestAppointment;
        }

        private List<Appointment> GetSortedAppointments(List<Appointment> appointments)
        {
            return appointments = appointments
                .OrderByDescending(appointment => appointment.DateAndTime.Date)  
                .ThenByDescending(appointment => appointment.DateAndTime.TimeOfDay) 
                .ToList();
        }

        private void SetNearestAppointment(Appointment appointment)
        {
            if (appointment != null)
            {
                NearestAppointmentPatient.Text = appointment.PatientRefNavigation.FirstName + " " + appointment.PatientRefNavigation.LastName;
                NearestAppointmentDate.Text = appointment.DateAndTime.ToShortDateString();
                NearestAppointmentTime.Text = appointment.DateAndTime.ToShortTimeString() + "-" + appointment.DateAndTime.AddHours(1).ToShortTimeString();
                NearestAppointmentComment.Text = appointment.Message;
            }
            else
            {
                NearestAppointmentPatient.Text = "Найблжчий запис відсутній!";
                NearestAppointmentDate.Text = "";
                NearestAppointmentTime.Text = "";
                NearestAppointmentComment.Text = "";
            }
            
        }

        private List<Member> MapToMemberList(List<Appointment> appointments)
        {
            List <Member> members = new List<Member>();
            foreach (var appointment in appointments)
            {
                Member newMember = new Member();
                newMember.Id = appointment.AppointmentId;
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
            FilterMembers();
        }
        private void FilterMembers()
        {
            string searchText = txtSearch.Text.ToLower();

            List<Member> filteredMembers = members
                .Where(member => member.Name.ToLower().Contains(searchText))
                .ToList();

            RefreshDataGrid(filteredMembers);
        }
        private void RefreshDataGrid(List<Member> filteredMembers)
        {
            currentPage = 1;
            totalPages = (int)Math.Ceiling((double)filteredMembers.Count() / itemsPerPage);

            currentPageButton.Content = currentPage.ToString();
            lastPageButton.Content = totalPages.ToString();
            nextPageButton.Content = (currentPage + 1).ToString();

            int skip = (currentPage - 1) * itemsPerPage;
            membersDataGrid.ItemsSource = filteredMembers.Skip(skip).Take(itemsPerPage);
        }


        public class Member
        {
            public long Id { get; set; }
            public string Name { get; set; } = null!;

            public string Date { get; set; } = null!;
            public string Time { get; set; } = null!;
            public string Comment { get; set; } = null!;
            public string Status { get; set; } = null!;

        }

      
    }
}
