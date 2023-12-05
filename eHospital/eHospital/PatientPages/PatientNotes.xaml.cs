using EF;
using EF.service.impl;
using eHospital.AdminPages;
using eHospital.Forms;
using NLog;
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

namespace eHospital.PatientPages
{
    /// <summary>
    /// Interaction logic for PatientNotes.xaml
    /// </summary>
    public partial class PatientNotes : Page
    {
        public string patientName { get; set; } = "Олег";
        public int currentPage = 1;
        public int itemsPerPage;
        public int totalPages;

        private readonly AppointmentServiceImpl appointmentService;
        List<Member> members;
        Appointment nearestAppointment;
        Brush activeBrush;
        Brush defaultBrush;

        private static Logger logger = LogManager.GetCurrentClassLogger();


        private void CheckAndShowEllipsis()
        {
            if (totalPages > 3)
            {
                if (currentPage > 2 && currentPage < totalPages - 1)
                {
                    ellipsisPanel1.Visibility = Visibility.Visible;
                    ellipsisPanel2.Visibility = Visibility.Visible;
                }
                else
                {
                    if (currentPage < 3)
                    {
                        ellipsisPanel1.Visibility = Visibility.Collapsed;
                        ellipsisPanel2.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        ellipsisPanel1.Visibility = Visibility.Visible; // Ховаємо стек панель із "..."
                        ellipsisPanel2.Visibility = Visibility.Collapsed; // Ховаємо стек панель із "..."
                    }
                }
            }

        }
        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {

            if (currentPage < totalPages - 1)
            {
                currentPage++;
                int skip = (currentPage - 1) * itemsPerPage;
                currentPageButton.Background = activeBrush;
                firstPageButton.Background = defaultBrush;
                membersDataGrid.ItemsSource = members.Skip(skip).Take(itemsPerPage);
                currentPageButton.Content = currentPage.ToString();
                currentPageButton.InvalidateVisual();
                CheckAndShowEllipsis();
                logger.Info($"Пацієнт успішно перейшов до наступної сторінки записів");

            }
            else
            {
                if (currentPage == totalPages - 1)
                {
                    currentPage++;
                    int skip = (currentPage - 1) * itemsPerPage;
                    currentPageButton.Background = defaultBrush;
                    lastPageButton.Background = activeBrush;

                    membersDataGrid.ItemsSource = members.Skip(skip).Take(itemsPerPage);
                    //currentPageButton.Content = currentPage.ToString();
                    currentPageButton.InvalidateVisual();
                    CheckAndShowEllipsis();
                    logger.Info($"Пацієнт успішно перейшов до наступної сторінки записів");

                }
            }



        }
        private void PreviousPageButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage == totalPages)
            {
                currentPage--;
                int skip = (currentPage - 1) * itemsPerPage;
                membersDataGrid.ItemsSource = members.Skip(skip).Take(itemsPerPage);
                currentPageButton.Content = currentPage.ToString();
                currentPageButton.Background = activeBrush;
                lastPageButton.Background = defaultBrush;
                //nextPageButton.Content = (currentPage + 1).ToString();
                currentPageButton.InvalidateVisual();
                CheckAndShowEllipsis();
                logger.Info($"Пацієнт успішно попередньої до наступної сторінки записів");

            }
            else
            {
                if (currentPage > 2)
                {
                    currentPage--;
                    int skip = (currentPage - 1) * itemsPerPage;
                    membersDataGrid.ItemsSource = members.Skip(skip).Take(itemsPerPage);
                    currentPageButton.Content = currentPage.ToString();
                    //nextPageButton.Content = (currentPage + 1).ToString();
                    currentPageButton.InvalidateVisual();
                    CheckAndShowEllipsis();
                    logger.Info($"Пацієнт успішно попередньої до наступної сторінки записів");

                }
                else
                {
                    if (currentPage == 2)
                    {
                        currentPage--;
                        int skip = currentPage * itemsPerPage;
                        membersDataGrid.ItemsSource = members.Take(itemsPerPage);
                        firstPageButton.Background = activeBrush;
                        currentPageButton.Background = defaultBrush;
                        currentPageButton.InvalidateVisual();
                        CheckAndShowEllipsis();
                        logger.Info($"Пацієнт успішно попередньої до наступної сторінки записів");

                    }
                }

            }
        }
        public PatientNotes()
        {
            InitializeComponent();
            this.appointmentService = new AppointmentServiceImpl(new EF.context.NeondbContext());
            currentPage = 1;

            List<Appointment> appointments = appointmentService.GetAppointmentsByUserId(App.UserId);
            logger.Info($"Успішно отрмано список зі записами пацієнта {App.UserId}");

            appointments = GetSortedAppointments(appointments);
            logger.Info($"Успішно отрмано список зі відсортованими записами пацієнта {App.UserId}");

            members = MapToMemberList(appointments);

            this.nearestAppointment = GetNearestAppointment(appointments);
            SetNearestAppointment(nearestAppointment);
            ColorConverter converter = new ColorConverter();
            Color color = (Color)ColorConverter.ConvertFromString("#9E9E9E");
            Color color2 = (Color)ColorConverter.ConvertFromString("#AD99DA");

            // Створення кисті з вказаним кольором
            activeBrush = new SolidColorBrush(color);
            defaultBrush = new SolidColorBrush(color2);
            itemsPerPage = 4;
            int totalItems = members.Count();

            totalPages = (int)Math.Ceiling((double)totalItems / itemsPerPage);
            if (totalPages > 3)
            {
                ellipsisPanel1.Visibility = Visibility.Collapsed;
                ellipsisPanel2.Visibility = Visibility.Visible;
            }
            else
            {
                ellipsisPanel1.Visibility = Visibility.Collapsed;
                ellipsisPanel2.Visibility = Visibility.Collapsed;
            }

            firstPageButton.Content = currentPage.ToString();
            firstPageButton.Background = activeBrush;
            currentPageButton.Content = (currentPage + 1).ToString();
            firstPageButton.InvalidateVisual();
            currentPageButton.InvalidateVisual();
            lastPageButton.Content = totalPages.ToString();

            lastPageButton.InvalidateVisual();
            membersDataGrid.ItemsSource = members.Take(itemsPerPage);

            txtSearch.TextChanged += txtSearch_TextChanged;
            logger.Info("Сторніка із записами успішно відобразилась");

        }
        private List<Appointment> GetSortedAppointments(List<Appointment> appointments)
        {
            logger.Info($"Отримано найближчий запис для пацієнта {App.UserId}");

            return appointments = appointments
                .OrderByDescending(appointment => appointment.DateAndTime.Date)
                .ThenByDescending(appointment => appointment.DateAndTime.TimeOfDay)
                .ToList();
        }
        private List<Member> MapToMemberList(List<Appointment> appointments)
        {
            List<Member> members = new List<Member>();
            foreach (var appointment in appointments)
            {
                Member newMember = new Member();
                newMember.Id = appointment.AppointmentId;
                newMember.Name = appointment.DoctorRefNavigation.FirstName + " " + appointment.DoctorRefNavigation.LastName + " " + appointment.DoctorRefNavigation.Patronymic;
                newMember.Status = appointment.Status;
                newMember.Type = appointment.DoctorRefNavigation.Type;
                if(appointment.Message != null)
                {
                    newMember.Comment = appointment.Message;
                }
                else
                {
                    newMember.Comment = " ";
                }
               
                newMember.Date = appointment.DateAndTime.ToShortDateString();
                newMember.Time = appointment.DateAndTime.ToShortTimeString() + "-" + appointment.DateAndTime.AddHours(1).ToShortTimeString();
                members.Add(newMember);
            }
            List<Member> sortedMembers = members.OrderByDescending(member => DateTime.Parse(member.Date)).ToList();
            logger.Info("Успішно отрмано список записів для таблиці");

            return sortedMembers;
        }
        private Appointment GetNearestAppointment(List<Appointment> appointments)
        {
            DateTime now = DateTime.Now;

            var nearestAppointment = appointments
                .Where(appointment => appointment.Status == "активний")
                .OrderBy(appointment => Math.Abs((appointment.DateAndTime - now).TotalMinutes))
                .FirstOrDefault();

            return nearestAppointment;
        }
        private void SetNearestAppointment(Appointment appointment)
        {
            if (appointment != null)
            {
                NearestAppointmentDoctor.Text = appointment.DoctorRefNavigation.FirstName + " " + appointment.DoctorRefNavigation.LastName + " " + appointment.DoctorRefNavigation.Patronymic;
                NearesAppointmentType.Text = appointment.DoctorRefNavigation.Type;
                NearestAppointmentDate.Text = appointment.DateAndTime.ToShortDateString();
                NearestAppointmentTime.Text = appointment.DateAndTime.ToShortTimeString() + "-" + appointment.DateAndTime.AddHours(1).ToShortTimeString();
                NearestAppointmentComment.Text = appointment.Message;
                logger.Info("Успішно встановлено інформацію про найближчий запис");

            }
            else
            {
                NearestAppointmentDoctor.Text = "Найближчий запис відсутній!";
                NearestAppointmentDate.Text = "";
                NearestAppointmentTime.Text = "";
                NearestAppointmentComment.Text = "";
                NearesAppointmentType.Text = "";
                logger.Info("Успішно встановлено інформацію про найближчий запис");


            }

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
            //nextPageButton.Content = (currentPage + 1).ToString();

            int skip = (currentPage - 1) * itemsPerPage;
            membersDataGrid.ItemsSource = filteredMembers.Skip(skip).Take(itemsPerPage);
            logger.Info("Зміна списку із записами відповідно до пошуку");

        }
        public void ShowPatientProfile_click(object sender, RoutedEventArgs e)
        {

            Profile childWindow = new Profile();
            Window parentWindow = Window.GetWindow((DependencyObject)sender);

            parentWindow.Opacity = 0.25;
            childWindow.Closed += (s, args) =>
            {
                parentWindow.Opacity = 1.0;
            };
            childWindow.Owner = parentWindow;
            childWindow.ShowDialog();
        }

        public void CreateAnAppointment_click(object sender, RoutedEventArgs e)
        {

            PatientNewAppointment patientNewAppointmentWindow = new PatientNewAppointment();
            Window parentWindow = Window.GetWindow((DependencyObject)sender);

            parentWindow.Opacity = 0.25;
            patientNewAppointmentWindow.Closed += (s, args) =>
            {
                parentWindow.Opacity = 1.0;
            };
            patientNewAppointmentWindow.Owner = parentWindow;
            patientNewAppointmentWindow.ShowDialog();
        }

        public class Member
        {
            public long Id { get; set; }
            public string Name { get; set; } = null!;
            public string Type { get; set; } = null!;
            public string Date { get; set; } = null!;
            public string Time { get; set; } = null!;
            public string Comment { get; set; } = null!;
            public string Status { get; set; } = null!;

        }
    }
}
