using EF;
using EF.service.impl;
using eHospital.Forms;
using NLog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Metrics;
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

namespace eHospital.AdminPages
{
    /// <summary>
    /// Interaction logic for AdminDoctors.xaml
    /// </summary>
    /// 
    
    public partial class AdminDoctors : Page
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
        public int currentPage = 1;
        public int itemsPerPage;
        public int totalPages;
        List<Member> members;
        List<User> doctors;
        private readonly UserServiceImpl userService;
        Brush activeBrush;
        Brush defaultBrush;



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
                        ellipsisPanel1.Visibility = Visibility.Visible;
                        ellipsisPanel2.Visibility = Visibility.Collapsed;
                    }
                }
            }

        }
        private void NextPageButton_Click(object sender, RoutedEventArgs e)
        {

            if (currentPage < totalPages - 1)
            {
                logger.Info($"Адміністратор успішно перейшов до наступної сторінки");
                currentPage++;
                int skip = (currentPage - 1) * itemsPerPage;
                currentPageButton.Background = activeBrush;
                firstPageButton.Background = defaultBrush;
                membersDataGrid.ItemsSource = members.Skip(skip).Take(itemsPerPage);
                currentPageButton.Content = currentPage.ToString();
                currentPageButton.InvalidateVisual();
                CheckAndShowEllipsis();
                
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
                    }
                }

            }



        }

        public AdminDoctors()
        {
            InitializeComponent();
            currentPage = 1;
            this.userService = new UserServiceImpl(new EF.context.NeondbContext());

            this.doctors = userService.GetDoctors();
            this.members = MapDoctorsToMembers(this.doctors);


            ColorConverter converter = new ColorConverter();
            Color color = (Color)ColorConverter.ConvertFromString("#9E9E9E");
            Color color2 = (Color)ColorConverter.ConvertFromString("#AD99DA");

            // Створення кисті з вказаним кольором
            activeBrush = new SolidColorBrush(color);
            defaultBrush = new SolidColorBrush(color2);
            itemsPerPage = 5;
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
        }
        private List<Member> MapDoctorsToMembers(List<User> doctors)
        {
            List<Member> returnMembers = new List<Member>();
            foreach (var doctor in doctors)
            {
                Member newMember = new Member();
                newMember.Id = doctor.UserId;
                newMember.Name = doctor.FirstName + " " + doctor.LastName + " " + doctor.Patronymic;
                newMember.Type = doctor.Type;
                newMember.Email = doctor.Email;
                newMember.PhoneNumber = doctor.Phone;
                returnMembers.Add(newMember);
            }
            List<Member> sortedMembersByName = returnMembers.OrderBy(member => member.Name).ToList();
            return sortedMembersByName;
        }
        public void ShowAdminProfile_click(object sender, RoutedEventArgs e)
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
        public void NavigateToAdminNotes_click(object sender, RoutedEventArgs e)
        {

            AdminNotes doctorNotesPage = new AdminNotes();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
            {
                mainFrame.Navigate(doctorNotesPage);
            }
        }
        public void NavigateToAdminPatients_click(object sender, RoutedEventArgs e)
        {

            AdminPatients doctorNotesPage = new AdminPatients();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
            {
                mainFrame.Navigate(doctorNotesPage);
            }
        }
        public void NavigateToAdminStatus_click(object sender, RoutedEventArgs e)
        {

            AdminStatus doctorNotesPage = new AdminStatus();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
            {
                mainFrame.Navigate(doctorNotesPage);
            }
        }

        public void ShowAddNewDoctor_click(object sender, RoutedEventArgs e)
        {

            AddNewDoctor childWindow = new AddNewDoctor();
            Window parentWindow = Window.GetWindow((DependencyObject)sender);

            parentWindow.Opacity = 0.25;
            childWindow.Closed += (s, args) =>
            {
                parentWindow.Opacity = 1.0;
            };
            childWindow.Owner = parentWindow;
            childWindow.ShowDialog();
        }

        private void ShowEditDoctor_click(object sender, RoutedEventArgs e)
        {
            Button editButton = (Button)sender;
            long memberIdToEdit = (long)editButton.Tag;

            EditDoctor childWindow = new EditDoctor(memberIdToEdit);
            Window parentWindow = Window.GetWindow((DependencyObject)sender);

            parentWindow.Opacity = 0.25;
            childWindow.Closed += (s, args) =>
            {
                parentWindow.Opacity = 1.0;
            };
            childWindow.Owner = parentWindow;
            childWindow.ShowDialog();
        }
        private void ShowDoctorHistory_click(object sender, RoutedEventArgs e)
        {
            Button editButton = (Button)sender;
            long memberId = (long)editButton.Tag;

            AdminPatientHistory childWindow = new AdminPatientHistory(memberId);
            Window parentWindow = Window.GetWindow((DependencyObject)sender);

            parentWindow.Opacity = 0.25;
            childWindow.Closed += (s, args) =>
            {
                parentWindow.Opacity = 1.0;
            };
            childWindow.Owner = parentWindow;
            childWindow.ShowDialog();
        }
        private void ShowConfirm_click(object sender, RoutedEventArgs e)
        {
            Button editButton = (Button)sender;
            long memberId = (long)editButton.Tag;

            AreYouSure childWindow = new AreYouSure("Doctor",memberId);
            Window parentWindow = Window.GetWindow((DependencyObject)sender);

            parentWindow.Opacity = 0.25;
            childWindow.Closed += (s, args) =>
            {
                parentWindow.Opacity = 1.0;
            };
            childWindow.Owner = parentWindow;
            childWindow.ShowDialog();
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
        }
    }
    public class Member
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }

        

    }
    
    
    

}
