using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace WpfApp7
{
    public partial class ManageServicesWindow : Window
    {
        private string connectionString = "data source=stud-mssql.sttec.yar.ru,38325;user id=user279_db;password=user279;MultipleActiveResultSets=True;App=EntityFramework";

        private string employeeName;
        private string employeeSurname;
        private string employeePosition;

        public ManageServicesWindow(string name, string surname, string position)
        {
            InitializeComponent();
            employeeName = name;
            employeeSurname = surname;
            employeePosition = position;
            LoadServices();
        }

        private void LoadServices()
        {
            List<Service> services = new List<Service>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id, name, description, price, execution_time FROM Auto_Услуги";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            services.Add(new Service
                            {
                                Id = reader.GetInt32(0), // Извлечение ID как int
                                Name = reader.GetString(1), // Извлечение имени как string
                                Description = reader.GetString(2), // Извлечение описания как string
                                Price = reader.GetInt32(3), // Извлечение цены как decimal
                                ExecutionTime = reader.GetInt32(4) // Извлечение времени выполнения как int
                            });
                        }
                    }
                }
            }

            ServicesListView.ItemsSource = services; // Установка источника данных для списка услуг
        }

        private void AddServiceButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string description = DescriptionTextBox.Text;
            decimal price;
            int executionTime;

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(description) ||
                !decimal.TryParse(PriceTextBox.Text, out price) ||
                !int.TryParse(ExecutionTimeTextBox.Text, out executionTime))
            {
                MessageBox.Show("Пожалуйста, заполните все поля корректно.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Auto_Услуги (name, description, price, execution_time) VALUES (@name, @description, @price, @execution_time)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@description", description);
                    command.Parameters.AddWithValue("@price", price);
                    command.Parameters.AddWithValue("@execution_time", executionTime);

                    command.ExecuteNonQuery();
                }
            }

            LoadServices(); // Обновляем список услуг
        }

        private void UpdateServiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (ServicesListView.SelectedItem is Service selectedService)
            {
                string name = NameTextBox.Text;
                string description = DescriptionTextBox.Text;
                decimal price;
                int executionTime;

                if (string.IsNullOrWhiteSpace(name) ||
                    string.IsNullOrWhiteSpace(description) ||
                    !decimal.TryParse(PriceTextBox.Text, out price) ||
                    !int.TryParse(ExecutionTimeTextBox.Text, out executionTime))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля корректно.");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Auto_Услуги SET name = @name, description = @description, price = @price, execution_time = @execution_time WHERE id = @id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", selectedService.Id);
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@description", description);
                        command.Parameters.AddWithValue("@price", price);
                        command.Parameters.AddWithValue("@execution_time", executionTime);

                        command.ExecuteNonQuery();
                    }
                }

                LoadServices(); // Обновляем список услуг
            }
        }

        private void DeleteServiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (ServicesListView.SelectedItem is Service selectedService)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Сначала удаляем зависимости
                    string deleteDependenciesQuery = "DELETE FROM Auto_УслугиМатериалы WHERE id_uslugi = @id";
                    using (SqlCommand command = new SqlCommand(deleteDependenciesQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", selectedService.Id);
                        command.ExecuteNonQuery();
                    }

                    // Теперь удаляем услугу
                    string query = "DELETE FROM Auto_Услуги WHERE id = @id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", selectedService.Id);
                        command.ExecuteNonQuery();
                    }
                }

                LoadServices(); // Обновляем список услуг
                MessageBox.Show("Услуга успешно удалена!");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeMainWindow employeeMainWindow = new EmployeeMainWindow(employeeName, employeeSurname, employeePosition);
            employeeMainWindow.Show();
            this.Close(); // Закрыть текущее окно
        }
    }

    public class Service
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int ExecutionTime { get; set; }
    }
}