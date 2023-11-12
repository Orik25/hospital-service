    using System;
    using System.Collections.ObjectModel;
    using System.Windows;
    using System.Windows.Controls;

    namespace eHospital.AdminPages
    {
        public partial class AdminDoctorHistory : Page
        {
            // Клас для представлення даних в таблиці
            public class Record
            {
                public string Name { get; set; }
                public string Type { get; set; }
                public DateTime Number { get; set; }
            }

            // Колекція даних для таблиці
            public ObservableCollection<Record> Records { get; set; } = new ObservableCollection<Record>();

            public AdminDoctorHistory()
            {
                InitializeComponent();

                // Додаємо три прикладових рядки
                Records.Add(new Record { Name = "Доктор 1", Type = "Тип 1", Number = DateTime.Now });
                Records.Add(new Record { Name = "Доктор 3", Type = "Тип 2", Number = DateTime.Now.AddDays(1) });
                Records.Add(new Record { Name = "Доктор 3", Type = "Тип 3", Number = DateTime.Now.AddDays(2) });
                Records.Add(new Record { Name = "Доктор 4", Type = "Тип 1", Number = DateTime.Now });
                Records.Add(new Record { Name = "Доктор 2", Type = "Тип 2", Number = DateTime.Now.AddDays(1) });
                Records.Add(new Record { Name = "Доктор 3", Type = "Тип 3", Number = DateTime.Now.AddDays(2) });
            Records.Add(new Record { Name = "Доктор 10", Type = "Тип 1", Number = DateTime.Now });
            Records.Add(new Record { Name = "Доктор 2", Type = "Тип 2", Number = DateTime.Now.AddDays(1) });
            Records.Add(new Record { Name = "Доктор 3", Type = "Тип 3", Number = DateTime.Now.AddDays(2) });
            Records.Add(new Record { Name = "Доктор 47", Type = "Тип 1", Number = DateTime.Now });
            Records.Add(new Record { Name = "Доктор 2", Type = "Тип 2", Number = DateTime.Now.AddDays(1) });
            Records.Add(new Record { Name = "Доктор 3", Type = "Тип 3", Number = DateTime.Now.AddDays(2) });
            Records.Add(new Record { Name = "Доктор 1", Type = "Тип 1", Number = DateTime.Now });
            Records.Add(new Record { Name = "Доктор 57", Type = "Тип 2", Number = DateTime.Now.AddDays(1) });
            Records.Add(new Record { Name = "Доктор 3", Type = "Тип 3", Number = DateTime.Now.AddDays(2) });
            Records.Add(new Record { Name = "Доктор 1", Type = "Тип 1", Number = DateTime.Now });
            Records.Add(new Record { Name = "Доктор 7", Type = "Тип 2", Number = DateTime.Now.AddDays(1) });
            Records.Add(new Record { Name = "Доктор 3", Type = "Тип 3", Number = DateTime.Now.AddDays(2) });

            // Прив'язка даних до DataGrid
            membersDataGrid.ItemsSource = Records;
            }

            private void membersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
            {
                // Обробка події вибору елемента в таблиці (якщо потрібно)
            }
        }
    }