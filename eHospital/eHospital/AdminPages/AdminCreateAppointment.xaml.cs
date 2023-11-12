using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace eHospital.AdminPages
{
    public partial class AdminCreateAppointment : Page
    {
        public AdminCreateAppointment()
        {
            InitializeComponent();

            // Ваш код для ініціалізації інших компонентів сторінки

            // Приклад додавання даних до випадаючого списку пацієнтів
            Patients.Add(new Patient { FirstName = "Ім'я1", LastName = "Прізвище1", Patronymic = "По-батькові1" });
            Patients.Add(new Patient { FirstName = "Ім'я2", LastName = "Прізвище2", Patronymic = "По-батькові2" });
            Patients.Add(new Patient { FirstName = "Ім'я3", LastName = "Прізвище3", Patronymic = "По-батькові3" });

            // Прив'язка даних до випадаючого списку пацієнтів
            patientComboBox.ItemsSource = Patients;
        }

        // Ваші дані пацієнтів (замість List<T> може бути ваша колекція)
        public List<Patient> Patients { get; set; } = new List<Patient>();

        // Клас для представлення даних пацієнта
        public class Patient
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Patronymic { get; set; }
        }
    }
}
