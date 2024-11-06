using System;
using System.Data.SqlClient;
using System.Windows;

namespace WpfApp7
{
    public partial class OrderWindow : Window
    {
        private string connectionString = "data source=stud-mssql.sttec.yar.ru,38325;user id=user279_db;password=user279;MultipleActiveResultSets=True;App=EntityFramework";
        private int clientId;

        public OrderWindow(int clientId)
        {
            InitializeComponent();
            this.clientId = clientId; // Сохраняем переданное значение
            LoadServices();
            LoadCars(clientId); // Загружаем автомобили клиента
        }

        private void LoadServices()
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id, name FROM Auto_Услуги";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ServiceComboBox.Items.Add(new ServiceItem
                            {
                                Id = (int)reader["id"],
                                Name = reader["name"].ToString()
                            });
                        }
                    }
                }
            }
        }

        private void LoadCars(int clientId)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id, name FROM Auto_Машина WHERE id_client = @id_client";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_client", clientId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            CarComboBox.Items.Add(new CarItem
                            {
                                Id = (int)reader["id"],
                                Name = reader["name"].ToString()
                            });
                        }
                    }
                }
            }
        }

        private void PlaceOrderButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedService = ServiceComboBox.SelectedItem as dynamic;
            var selectedCar = CarComboBox.SelectedItem as dynamic;

            if (selectedService == null || selectedCar == null || !OrderDatePicker.SelectedDate.HasValue)
            {
                MessageBox.Show("Пожалуйста, выберите услугу и автомобиль и укажите дату.");
                return;
            }

            int serviceId = selectedService.Id;
            int carId = selectedCar.Id;
            DateTime orderDate = OrderDatePicker.SelectedDate.Value;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Auto_Заказ (id_uslugi, date_order, id_status, comment, id_worker, id_car) " +
                               "VALUES (@id_uslugi, @date_order, @id_status, @comment, @id_worker, @id_car)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id_uslugi", serviceId);
                    command.Parameters.AddWithValue("@date_order", orderDate);
                    command.Parameters.AddWithValue("@id_status", 1);
                    command.Parameters.AddWithValue("@comment", "");
                    command.Parameters.AddWithValue("@id_worker", 1);
                    command.Parameters.AddWithValue("@id_car", carId);

                    try
                    {
                        command.ExecuteNonQuery();
                        SuccessWindow successWindow = new SuccessWindow(clientId); // Передаем clientId
                        successWindow.ShowDialog(); // Открываем окно как модальное
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при оформлении заказа: " + ex.Message);
                    }
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            CarDetailsWindow carDetailsWindow = new CarDetailsWindow();
            carDetailsWindow.Show(); // Показываем предыдущее окно
            this.Close();
        }

        private void ServiceComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            // Обработчик события изменения выбора услуги
            var selectedService = ServiceComboBox.SelectedItem as ServiceItem;
            if (selectedService != null)
            {
                MessageBox.Show($"Вы выбрали услугу: {selectedService.Name}");
            }
        }

        public class ServiceItem
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Name; // Указываем, что нужно отображать имя
            }
        }

        public class CarItem
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public override string ToString()
            {
                return Name; // Указываем, что нужно отображать имя
            }
        }
    }
}