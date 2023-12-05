using EF.service.impl;
using EF;
using eHospital.Forms;
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
using NLog;

namespace eHospital.AdminPages
{
    /// <summary>
    /// Interaction logic for AdminPatients.xaml
    /// </summary>
    public partial class AdminPatients : Page
    {
        public int currentPage = 1;
        public int itemsPerPage;
        public int totalPages;
        List<Member> members;
        List<User> patients;
        private readonly UserServiceImpl userService;
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
                logger.Info($"Адміністратор успішно наступної до попередньої сторінки пацієнтів");

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
                    logger.Info($"Адміністратор успішно наступної до попередньої сторінки пацієнтів");

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
                logger.Info($"Адміністратор успішно перейшов до попередньої сторінки пацієнтів");

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
                    logger.Info($"Адміністратор успішно перейшов до попередньої сторінки пацієнтів");

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
                        logger.Info($"Адміністратор успішно перейшов до попередньої сторінки пацієнтів");

                    }
                }

            }

        }
        public AdminPatients()
        {
            InitializeComponent();

            currentPage = 1;

            this.userService = new UserServiceImpl(new EF.context.NeondbContext());

            this.patients = userService.GetPatients();
            logger.Info("Успіщно отрмано список пацієнтів");

            this.members = MapDoctorsToMembers(this.patients);


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
            logger.Info("Сторінка із пацієнтами успішно відобразилась");

        }
        private List<Member> MapDoctorsToMembers(List<User> patients)
        {
            List<Member> returnMembers = new List<Member>();
            foreach (var patient in patients)
            {
                Member newMember = new Member();
                newMember.Id = patient.UserId;
                newMember.Name = patient.FirstName + " " + patient.LastName;
                newMember.Email = patient.Email;
                newMember.PhoneNumber = patient.Phone;
                returnMembers.Add(newMember);
            }
            List<Member> sortedMembersByName = returnMembers.OrderBy(member => member.Name).ToList();
            logger.Info("Успішно отримано список пацієнтів для таблиці");

            return sortedMembersByName;
        }

        private void membersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
        
        public class Member
        {
            public long Id { get; set; }
            public string Name { get; set; }
            public string PhoneNumber { get; set; }
            public string Email { get; set; }



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
        public void ShowAddNewPatient_click(object sender, RoutedEventArgs e)
        {

            AddNewPatient childWindow = new AddNewPatient();
            Window parentWindow = Window.GetWindow((DependencyObject)sender);

            parentWindow.Opacity = 0.25;
            childWindow.Closed += (s, args) =>
            {
                parentWindow.Opacity = 1.0;
            };
            childWindow.Owner = parentWindow;
            childWindow.ShowDialog();
        }
        private void ShowPatientHistory_click(object sender, RoutedEventArgs e)
        {
            Button editButton = (Button)sender;
            long memberId = (long)editButton.Tag;

            AdminDoctorHistory childWindow = new AdminDoctorHistory(memberId);
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

            AreYouSure childWindow = new AreYouSure("Patient", memberId);
            Window parentWindow = Window.GetWindow((DependencyObject)sender);

            parentWindow.Opacity = 0.25;
            childWindow.Closed += (s, args) =>
            {
                parentWindow.Opacity = 1.0;
            };
            childWindow.Owner = parentWindow;
            childWindow.ShowDialog();
        }
        private void ShowEditPatient_click(object sender, RoutedEventArgs e)
        {
            Button editButton = (Button)sender;
            long memberIdToEdit = (long)editButton.Tag;

            EditPatient childWindow = new EditPatient(memberIdToEdit);
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
                logger.Info("Адміністратора успішно перенаправлено на сторінку із записами");

            }
        }
        public void NavigateToAdminDoctors_click(object sender, RoutedEventArgs e)
        {

            AdminDoctors doctorNotesPage = new AdminDoctors();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
            {
                mainFrame.Navigate(doctorNotesPage);
                logger.Info("Адміністратора успішно перенаправлено на сторінку із лікарями");

            }
        }
        public void NavigateToAdminStatus_click(object sender, RoutedEventArgs e)
        {

            AdminStatus doctorNotesPage = new AdminStatus();
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null && mainWindow.FindName("mainFrame") is Frame mainFrame)
            {
                mainFrame.Navigate(doctorNotesPage);
                logger.Info("Адміністратора успішно перенаправлено на сторінку із записами");

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

            int skip = (currentPage - 1) * itemsPerPage;
            membersDataGrid.ItemsSource = filteredMembers.Skip(skip).Take(itemsPerPage);
            logger.Info("Зміна списку із пацієнтами відповідно до пошуку");

        }
    }
}
