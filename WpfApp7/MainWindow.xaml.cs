using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions; // Импортируем пространство имен для регулярных выражений
using System.Windows;

namespace WpfApp7
{
    public partial class MainWindow : Window
    {
        private string connectionString = "data source=stud-mssql.sttec.yar.ru,38325;user id=user279_db;password=user279;MultipleActiveResultSets=True;App=EntityFramework";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string login = LoginTextBox.Text;
            string password = PasswordBox.Password;

            // Проверка на специальные символы
            if (!IsValidInput(login) || !IsValidInput(password))
            {
                ErrorMessage.Text = "Логин и пароль не должны содержать специальные символы.";
                return;
            }

            // Проверка длины логина и пароля
            if (login.Length < 3 || password.Length < 5 || login.Length > 15 || password.Length > 15)
            {
                ErrorMessage.Text = "Логин должен быть от 3 до 15 символов, а пароль от 5 до 15 символов.";
                return;
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                bool userFound = false; // Флаг для отслеживания найденного пользователя

                // Проверка клиента
                string clientQuery = "SELECT id, id_role FROM Auto_Клиент WHERE login = @login AND password = @password";

                using (SqlCommand command = new SqlCommand(clientQuery, connection))
                {
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userFound = true; // Пользователь найден
                            int clientId = reader.GetInt32(0); // Получаем clientId
                            int roleIdInt = reader.GetInt32(1); // Получаем id_role

                            if (roleIdInt == 1) // Если роль администратора
                            {
                                AdminWindow adminWindow = new AdminWindow();
                                adminWindow.Show();
                                this.Close(); // Закрыть текущее окно авторизации
                                return; // Выход из метода
                            }
                            else if (roleIdInt == 2) // Если роль сотрудника
                            {
                                CarDetailsWindow carDetailsWindow = new CarDetailsWindow(clientId); // Передаем clientId
                                carDetailsWindow.Show();
                                this.Close(); // Закрыть текущее окно авторизации
                                return; // Выход из метода
                            }
                        }
                    }
                }

                // Проверка сотрудника (если клиент не найден)
                string employeeQuery = "SELECT s.id, s.name, s.surname, ds.name AS position, r.id FROM Auto_Сотрудники s " +
                                       "INNER JOIN Auto_Роль r ON s.id_dlzhnst = r.id_worker " +
                                       "INNER JOIN Auto_ДолжностьСотрудника ds ON s.id_dlzhnst = ds.id " +
                                       "WHERE s.login = @login AND s.password = @password";

                using (SqlCommand command = new SqlCommand(employeeQuery, connection))
                {
                    command.Parameters.AddWithValue("@login", login);
                    command.Parameters.AddWithValue("@password", password);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            userFound = true; // Пользователь найден
                            string name = reader["name"].ToString();
                            string surname = reader["surname"].ToString();
                            string position = reader["position"].ToString();
                            int roleIdEmployee = Convert.ToInt32(reader["id"]);

                            EmployeeMainWindow employeeMainWindow = new EmployeeMainWindow(name, surname, position);
                            employeeMainWindow.Show();
                            this.Close(); // Закрыть текущее окно авторизации
                            return; // Выход из метода
                        }
                    }
                }

                // Если ни один из запросов не нашел пользователя
                if (!userFound)
                {
                    ErrorMessage.Text = "Неверный логин или пароль.";
                }
            }
        }

        private bool IsValidInput(string input)
        {
            // Регулярное выражение для проверки наличия специальных символов
            string pattern = @"^[a-zA-Z0-9]*$"; // Разрешаем только буквы и цифры
            return Regex.IsMatch(input, pattern);
        }
    }
}