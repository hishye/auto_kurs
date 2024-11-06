using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Xml.Linq;

namespace WpfApp7
{
    public partial class ManageServiceMaterialsWindow : Window
    {
        private string connectionString = "data source=stud-mssql.sttec.yar.ru,38325;user id=user279_db;password=user279;MultipleActiveResultSets=True;App=EntityFramework";

        private string employeeName;
        private string employeeSurname;
        private string employeePosition;
        public ManageServiceMaterialsWindow(string name, string surname, string position)
        {
            InitializeComponent();
            employeeName = name;
            employeeSurname = surname;
            employeePosition = position;
            LoadServices();
            LoadMaterials();
        }

        private void LoadServices()
        {
            List<ServiceItem> services = new List<ServiceItem>();

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
                            services.Add(new ServiceItem
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                    }
                }
            }

            ServicesComboBox.ItemsSource = services;
            ServicesComboBox.DisplayMemberPath = "Name";
            ServicesComboBox.SelectedValuePath = "Id";
        }

        private void LoadMaterials()
        {
            List<Material> materials = new List<Material>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id, name FROM Auto_Материал";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            materials.Add(new Material
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            });
                        }
                    }
                }
            }

            MaterialsComboBox.ItemsSource = materials;
            MaterialsComboBox.DisplayMemberPath = "Name";
            MaterialsComboBox.SelectedValuePath = "Id";
        }

        private void AddMaterialToServiceButton_Click(object sender, RoutedEventArgs e)
        {
            if (ServicesComboBox.SelectedValue is int serviceId &&
                MaterialsComboBox.SelectedValue is int materialId &&
                int.TryParse(QuantityTextBox.Text, out int quantity))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Auto_УслугиМатериалы (id_material, id_uslugi, kolvo_zakaz) VALUES (@materialId, @serviceId, @quantity)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@materialId", materialId);
                        command.Parameters.AddWithValue("@serviceId", serviceId);
                        command.Parameters.AddWithValue("@quantity", quantity);

                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Материал успешно добавлен к услуге!");
                QuantityTextBox.Clear(); // Очистить поле ввода количества
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите услугу и материал и укажите количество.");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeMainWindow employeeMainWindow = new EmployeeMainWindow(employeeName, employeeSurname, employeePosition);
            employeeMainWindow.Show();
            this.Close(); // Закрыть текущее окно
        }
    }

    public class ServiceItem // Переименованный класс для избежания конфликта
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}