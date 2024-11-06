using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Xml.Linq;

namespace WpfApp7
{
    public partial class OrdersWindow : Window
    {
        private string connectionString = "data source=stud-mssql.sttec.yar.ru,38325;user id=user279_db;password=user279;MultipleActiveResultSets=True;App=EntityFramework";

        private string employeeName;
        private string employeeSurname;
        private string employeePosition;

         public OrdersWindow(string name, string surname, string position)
        {
            InitializeComponent();
            employeeName = name;
            employeeSurname = surname;
            employeePosition = position;

            LoadOrders(); // Call method to load orders
        }

        private void LoadOrders()
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT o.id, u.name AS ServiceName, o.date_order, s.status " +
                               "FROM Auto_Заказ o " +
                               "INNER JOIN Auto_Услуги u ON o.id_uslugi = u.id " +
                               "INNER JOIN Auto_СтатусЗаказа s ON o.id_status = s.id";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orders.Add(new Order
                            {
                                Id = reader.GetInt32(0),
                                ServiceName = reader.GetString(1),
                                DateOrder = reader.GetDateTime(2),
                                Status = reader.GetString(3)
                            });
                        }
                    }
                }
            }

            OrdersListView.ItemsSource = orders;
        }

        private void TakeOrderButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика для взятия заказа
            if (OrdersListView.SelectedItem is Order selectedOrder)
            {
                // Обновить статус заказа на "Взято"
                UpdateOrderStatus(selectedOrder.Id, 2); // Предположим, что 2 - это статус "Взято"
                LoadOrders(); // Перезагрузить заказы

                // Оповещение об успешном принятии заказа
                MessageBox.Show("Заказ успешно принят!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void DeclineOrderButton_Click(object sender, RoutedEventArgs e)
        {
            // Логика для отказа от заказа
            if (OrdersListView.SelectedItem is Order selectedOrder)
            {
                // Обновить статус заказа на "Отказано"
                UpdateOrderStatus(selectedOrder.Id, 3); // Предположим, что 3 - это статус "Отказано"
                LoadOrders(); // Перезагрузить заказы

                // Оповещение об успешном отказе от заказа
                MessageBox.Show("Заказ успешно отклонен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void ChangeStatusButton_Click(object sender, RoutedEventArgs e)
        {
            if (OrdersListView.SelectedItem is Order selectedOrder)
            {
                // Используйте реальные значения для имени и фамилии сотрудника
                ChangeStatusWindow changeStatusWindow = new ChangeStatusWindow(selectedOrder.Id, employeeName, employeeSurname, employeePosition);
                changeStatusWindow.ShowDialog(); // Ожидание закрытия окна
                LoadOrders(); // Перезагрузить заказы после изменения статуса
            }
        }

        private void UpdateOrderStatus(int orderId, int newStatusId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                // Проверяем существование нового статуса
                string checkStatusQuery = "SELECT COUNT(*) FROM Auto_СтатусЗаказа WHERE id = @newStatusId";
                using (SqlCommand checkCommand = new SqlCommand(checkStatusQuery, connection))
                {
                    checkCommand.Parameters.AddWithValue("@newStatusId", newStatusId);
                    int count = (int)checkCommand.ExecuteScalar();

                    // Отладочное сообщение
                    MessageBox.Show($"Проверяем статус: {newStatusId}, найдено: {count}");

                    if (count == 0)
                    {
                        MessageBox.Show("Указанный статус не существует.");
                        return; // Выход из метода, если статус не найден
                    }
                }

                // Если статус существует, выполняем обновление
                string query = "UPDATE Auto_Заказ SET id_status = @newStatus WHERE id = @orderId";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@newStatus", newStatusId);
                    command.Parameters.AddWithValue("@orderId", orderId);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeMainWindow employeeMainWindow = new EmployeeMainWindow(employeeName, employeeSurname, employeePosition);
            employeeMainWindow.Show();
            this.Close(); // Закрыть текущее окно
        }
    }

    public class Order
    {
        public int Id { get; set; }
        public string ServiceName { get; set; }
        public DateTime DateOrder { get; set; }
        public string Status { get; set; }
    }
}