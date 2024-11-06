using System;
using System.Data.SqlClient;
using System.Windows;

namespace WpfApp7
{
    public partial class CarDetailsWindow : Window
    {
        private string connectionString = "data source=stud-mssql.sttec.yar.ru,38325;user id=user279_db;password=user279;MultipleActiveResultSets=True;App=EntityFramework";
        private int clientId;

        // Конструктор без параметров (можно удалить, если не используется)
        public CarDetailsWindow()
        {
            InitializeComponent();
        }

        // Конструктор с параметром clientId
        public CarDetailsWindow(int clientId)
        {
            InitializeComponent();
            this.clientId = clientId; // Сохраняем id клиента для использования при добавлении машины
        }

        private void AddCarButton_Click(object sender, RoutedEventArgs e)
        {
            string carName = CarNameTextBox.Text;
            string model = ModelTextBox.Text;
            DateTime? dateOfIssue = DateOfIssuePicker.SelectedDate;
            string vin = VINTextBox.Text;

            // Проверка на заполнение всех полей
            if (string.IsNullOrEmpty(carName) || string.IsNullOrEmpty(model) || !dateOfIssue.HasValue || string.IsNullOrEmpty(vin))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            // Проверка существования клиента
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string checkClientQuery = "SELECT COUNT(*) FROM Auto_Клиент WHERE id = @id_client";

                using (SqlCommand checkCommand = new SqlCommand(checkClientQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@id_client", clientId);
                    int clientExists = (int)checkCommand.ExecuteScalar();

                    if (clientExists == 0)
                    {
                        MessageBox.Show("Клиент с указанным ID не существует.");
                        return;
                    }
                }

                // Если клиент существует, продолжаем добавление машины
                string query = "INSERT INTO Auto_Машина (id_client, name, model, date_of_issue, VIN) VALUES (@id_client, @name, @model, @date_of_issue, @VIN)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_client", clientId);
                    command.Parameters.AddWithValue("@name", carName);
                    command.Parameters.AddWithValue("@model", model);
                    command.Parameters.AddWithValue("@date_of_issue", dateOfIssue.Value);
                    command.Parameters.AddWithValue("@VIN", vin);

                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Машина успешно добавлена!");

                        // Переход на окно оформления заказа после добавления машины
                        OrderWindow orderWindow = new OrderWindow(clientId); // Передаем id клиента в окно заказа
                        orderWindow.Show();
                        this.Close(); // Закрыть текущее окно добавления машины
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при добавлении машины: " + ex.Message);
                    }
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close(); // Закрыть текущее окно
        }

        private void Order_ClicK(object sender, RoutedEventArgs e)
        {
            OrderWindow orderWindow = new OrderWindow(clientId);
            orderWindow.Show();
            this.Close(); // Закрыть текущее окно
        }
    }
}