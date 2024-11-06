using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;

namespace WpfApp7
{
    public partial class ClientsManagementWindow : Window
    {
        private string connectionString = "data source=stud-mssql.sttec.yar.ru,38325;user id=user279_db;password=user279;MultipleActiveResultSets=True;App=EntityFramework";

        public ClientsManagementWindow()
        {
            InitializeComponent();
            LoadClients();
        }

        private void LoadClients()
        {
            List<Client> clients = new List<Client>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id, login, password FROM Auto_Клиент";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clients.Add(new Client
                            {
                                Id = reader.GetInt32(0),
                                Login = reader.GetString(1),
                                Password = reader.GetString(2)
                            });
                        }
                    }
                }
            }

            ClientsListView.ItemsSource = clients;
        }

        private void AddClientButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordTextBox.Text;

            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Пожалуйста, заполните все поля.");
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Auto_Клиент (login, password) VALUES (@login, @password)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", password);

                    try
                    {
                        command.ExecuteNonQuery();
                        MessageBox.Show("Клиент успешно добавлен!");
                        LoadClients(); // Обновляем список клиентов
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при добавлении клиента: " + ex.Message);
                    }
                }
            }
        }

        private void EditClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsListView.SelectedItem is Client selectedClient)
            {
                string login = LoginTextBox.Text;
                string password = PasswordTextBox.Text;

                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля.");
                    return;
                }

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "UPDATE Auto_Клиент SET login = @login, password = @password WHERE id = @id";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", selectedClient.Id);
                        command.Parameters.AddWithValue("@login", login);
                        command.Parameters.AddWithValue("@password", password);

                        try
                        {
                            command.ExecuteNonQuery();
                            MessageBox.Show("Клиент успешно изменен!");
                            LoadClients(); // Обновляем список клиентов
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Ошибка при изменении клиента: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для изменения.");
            }
        }

        private void DeleteClientButton_Click(object sender, RoutedEventArgs e)
        {
            if (ClientsListView.SelectedItem is Client selectedClient)
            {
                var result = MessageBox.Show("Вы уверены, что хотите удалить этого клиента?", "Подтверждение удаления", MessageBoxButton.YesNo);

                if (result == MessageBoxResult.Yes)
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "DELETE FROM Auto_Клиент WHERE id = @id";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", selectedClient.Id);

                            try
                            {
                                command.ExecuteNonQuery();
                                MessageBox.Show("Клиент успешно удален!");
                                LoadClients(); // Обновляем список клиентов
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show("Ошибка при удалении клиента: " + ex.Message);
                            }
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите клиента для удаления.");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            AdminWindow adminWindow = new AdminWindow();
            adminWindow.Show(); // Показываем предыдущее окно
            this.Close();
        }
    }

    public class Client
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    }
}