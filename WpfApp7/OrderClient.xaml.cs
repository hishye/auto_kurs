using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace WpfApp7
{
    public partial class OrderClient : Window
    {
        private string connectionString = "data source=stud-mssql.sttec.yar.ru,38325;user id=user279_db;password=user279;MultipleActiveResultSets=True;App=EntityFramework";
        private int clientId;

        public OrderClient(int clientId)
        {
            InitializeComponent();
            this.clientId = clientId;
            LoadOrders();
        }

        private void LoadOrders()
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT o.id AS OrderId, s.name AS Service, o.date_order AS OrderDate, o.id_status AS Status " +
                               "FROM Auto_Заказ o " +
                               "JOIN Auto_Услуги s ON o.id_uslugi = s.id " +
                               "WHERE o.id_car IN (SELECT id FROM Auto_Машина WHERE id_client = @clientId)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@clientId", clientId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                OrderId = (int)reader["OrderId"],
                                Service = reader["Service"].ToString(),
                                OrderDate = ((DateTime)reader["OrderDate"]).ToString("dd.MM.yyyy"),
                                Status = ((int)reader["Status"]).ToString() // Здесь можно преобразовать статус в строку
                            });
                        }
                    }
                }
            }

            OrdersListView.ItemsSource = orders;
        }

        private void DeclineOrderButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика для отказа от заказа
            if (OrdersListView.SelectedItem is Order selectedOrder)
            {
                // Запрос на удаление заказа из базы данных
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "DELETE FROM Auto_Заказ WHERE id = @orderId";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@orderId", selectedOrder.OrderId);

                        try
                        {
                            int rowsAffected = command.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show($"Вы отказались от заказа ID: {selectedOrder.OrderId}");
                                LoadOrders(); // Обновляем список заказов
                            }
                            else
                            {
                                MessageBox.Show("Ошибка: Заказ не найден.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при отказе от заказа: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите заказ для отказа.");
            }
        }

        public class Order
        {
            public int OrderId { get; set; }
            public string Service { get; set; }
            public string OrderDate { get; set; }
            public string Status { get; set; }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            CarDetailsWindow CarDetailsWindow = new CarDetailsWindow();
            CarDetailsWindow.Show();
            this.Close(); // Закрыть текущее окно
        }
    }
}