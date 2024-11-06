using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace WpfApp7
{
    public partial class ChangeStatusWindow : Window
    {
        private string connectionString = "data source=stud-mssql.sttec.yar.ru,38325;user id=user279_db;password=user279;MultipleActiveResultSets=True;App=EntityFramework";

        private int orderId;

        private string employeeName;
        private string employeeSurname;
        private string employeePosition;

        public ChangeStatusWindow(int orderId, string employeeName, string employeeSurname, string employeePosition)
        {
            InitializeComponent();
            this.orderId = orderId;
            this.employeeName = employeeName;
            this.employeeSurname = employeeSurname;
            this.employeePosition = employeePosition;

            // Установка текста для отображения имени и должности
            EmployeeInfoTextBlock.Text = $"{employeeSurname} {employeeName}";
            EmployeePositionTextBlock.Text = $"Должность: {employeePosition}";

            LoadStatuses(); // Call to load statuses
        }

        // Method to load statuses from the database
        private void LoadStatuses()
        {
            List<Status> statuses = new List<Status>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id, status FROM Auto_СтатусЗаказа";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            statuses.Add(new Status
                            {
                                Id = reader.GetInt32(0),
                                StatusName = reader.GetString(1)
                            });
                        }
                    }
                }
            }

            StatusComboBox.ItemsSource = statuses;
            StatusComboBox.DisplayMemberPath = "StatusName";
            StatusComboBox.SelectedValuePath = "Id";
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (StatusComboBox.SelectedValue != null)
            {
                int selectedStatusId = (int)StatusComboBox.SelectedValue;

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Auto_Заказ SET id_status = @status WHERE id = @orderId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@status", selectedStatusId);
                        command.Parameters.AddWithValue("@orderId", orderId);
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Статус заказа успешно изменен!");
                this.Close(); // Закрыть окно после сохранения статуса
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите статус.");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            OrdersWindow ordersWindow = new OrdersWindow(employeeName, employeeSurname, employeePosition);
            ordersWindow.Show(); // Показываем предыдущее окно
            this.Close(); // Закрыть текущее окно
        }
    }

    public class Status
    {
        public int Id { get; set; }
        public string StatusName { get; set; }
    }
}