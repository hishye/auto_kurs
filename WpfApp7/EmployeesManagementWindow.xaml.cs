using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace WpfApp7
{
    public partial class EmployeeManagementWindow : Window
    {
        private string connectionString = "data source=stud-mssql.sttec.yar.ru,38325;user id=user279_db;password=user279;MultipleActiveResultSets=True;App=EntityFramework";

        public EmployeeManagementWindow()
        {
            InitializeComponent();
            LoadEmployees();
        }

        private void LoadEmployees()
        {
            List<Employee> employees = new List<Employee>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id, name, surname FROM Auto_Сотрудники";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            employees.Add(new Employee
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                Surname = reader.GetString(2)
                            });
                        }
                    }
                }
            }

            EmployeesListView.ItemsSource = employees;
        }

        private void AddEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string surname = SurnameTextBox.Text;

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Auto_Сотрудники (name, surname) VALUES (@name, @surname)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@surname", surname);

                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Сотрудник успешно добавлен!");
                        LoadEmployees(); // Обновляем список сотрудников
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при добавлении сотрудника: " + ex.Message);
                    }
                }
            }
        }

        private void EditEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesListView.SelectedItem is Employee selectedEmployee)
            {
                string name = NameTextBox.Text;
                string surname = SurnameTextBox.Text;

                if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Auto_Сотрудники SET name = @name, surname = @surname WHERE id = @id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", selectedEmployee.Id);
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@surname", surname);

                        try
                        {
                            command.ExecuteNonQuery();
                            MessageBox.Show("Сотрудник успешно изменен!");
                            LoadEmployees(); // Обновляем список сотрудников
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при изменении сотрудника: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите сотрудника для изменения.");
            }
        }

        private void DeleteEmployeeButton_Click(object sender, RoutedEventArgs e)
        {
            if (EmployeesListView.SelectedItem is Employee selectedEmployee)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить этого сотрудника?", "Подтверждение удаления", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM Auto_Сотрудники WHERE id = @id";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", selectedEmployee.Id);

                            try
                            {
                                command.ExecuteNonQuery();
                                MessageBox.Show("Сотрудник успешно удален!");
                                LoadEmployees(); // Обновляем список сотрудников
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Ошибка при удалении сотрудника: " + ex.Message);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите сотрудника для удаления.");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show(); // Показываем предыдущее окно
            this.Close();
        }
    }

    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
    }
}