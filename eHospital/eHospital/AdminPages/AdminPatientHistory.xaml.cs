﻿using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace eHospital.AdminPages
{
    public partial class AdminPatientHistory : Page
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

        public AdminPatientHistory()
        {
            InitializeComponent();

            // Додаємо три прикладових рядки
            Records.Add(new Record { Name = "Сі Ян Цук 1", Type = "1488", Number = DateTime.Now });
            Records.Add(new Record { Name = "Сі Ян Цук 3", Type = "1488", Number = DateTime.Now.AddDays(1) });
            Records.Add(new Record { Name = "Сі Ян Цук 3", Type = "1488", Number = DateTime.Now.AddDays(2) });
            Records.Add(new Record { Name = "Сі Ян Цук 4", Type = "1488", Number = DateTime.Now });
            Records.Add(new Record { Name = "Сі Ян Цук 2", Type = "1488", Number = DateTime.Now.AddDays(1) });
            Records.Add(new Record { Name = "Сі Ян Цук 3", Type = "1488", Number = DateTime.Now.AddDays(2) });
            Records.Add(new Record { Name = "Сі Ян Цук 10", Type = "1488", Number = DateTime.Now });
            Records.Add(new Record { Name = "Сі Ян Цук 2", Type = "1488", Number = DateTime.Now.AddDays(1) });


            // Прив'язка даних до DataGrid
            membersDataGrid.ItemsSource = Records;
        }

        private void membersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Обробка події вибору елемента в таблиці (якщо потрібно)
        }
    }
}