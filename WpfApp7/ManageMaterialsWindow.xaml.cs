using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Windows;
using System.Xml.Linq;

namespace WpfApp7
{
    public partial class ManageMaterialsWindow : Window
    {
        private string connectionString = "data source=stud-mssql.sttec.yar.ru,38325;user id=user279_db;password=user279;MultipleActiveResultSets=True;App=EntityFramework";

        private string employeeName;
        private string employeeSurname;
        private string employeePosition;

        public ManageMaterialsWindow(string name, string surname, string position)
        {
            InitializeComponent();
            employeeName = name;
            employeeSurname = surname;
            employeePosition = position;

            // Установка текста для отображения имени и должности

            LoadMaterials();
        }

        private void LoadMaterials()
        {
            List<MaterialItem> materials = new List<MaterialItem>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT id, name, kolvo_sklad FROM Auto_Материал";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            materials.Add(new MaterialItem
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1),
                                QuantityInStock = reader.GetInt32(2)
                            });
                        }
                    }
                }
            }

            MaterialsListView.ItemsSource = materials;
        }

        private void AddMaterialButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            if (int.TryParse(QuantityTextBox.Text, out int quantity))
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string query = "INSERT INTO Auto_Материал (name, kolvo_sklad) VALUES (@name, @quantity)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@quantity", quantity);
                        command.ExecuteNonQuery();
                    }
                }

                LoadMaterials(); // Обновляем список материалов
                MessageBox.Show("Материал успешно добавлен!");
            }
            else
            {
                MessageBox.Show("Пожалуйста, введите корректное количество.");
            }
        }

        private void UpdateMaterialButton_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialsListView.SelectedItem is MaterialItem selectedMaterial)
            {
                string name = NameTextBox.Text;
                if (int.TryParse(QuantityTextBox.Text, out int quantity))
                {
                    using (SqlConnection connection = new SqlConnection(connectionString))
                    {
                        connection.Open();
                        string query = "UPDATE Auto_Материал SET name = @name, kolvo_sklad = @quantity WHERE id = @id";

                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@id", selectedMaterial.Id);
                            command.Parameters.AddWithValue("@name", name);
                            command.Parameters.AddWithValue("@quantity", quantity);
                            command.ExecuteNonQuery();
                        }
                    }

                    LoadMaterials(); // Обновляем список материалов
                    MessageBox.Show("Материал успешно обновлен!");
                }
                else
                {
                    MessageBox.Show("Пожалуйста, введите корректное количество.");
                }
            }
        }

        private void DeleteMaterialButton_Click(object sender, RoutedEventArgs e)
        {
            if (MaterialsListView.SelectedItem is MaterialItem selectedMaterial)
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // Сначала удаляем зависимости
                    string deleteDependenciesQuery = "DELETE FROM Auto_УслугиМатериалы WHERE id_material = @id";
                    using (SqlCommand command = new SqlCommand(deleteDependenciesQuery, connection))
                    {
                        command.Parameters.AddWithValue("@id", selectedMaterial.Id);
                        command.ExecuteNonQuery();
                    }

                    // Теперь удаляем материал
                    string query = "DELETE FROM Auto_Материал WHERE id = @id";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", selectedMaterial.Id);
                        command.ExecuteNonQuery();
                    }
                }

                LoadMaterials(); // Обновляем список материалов
                MessageBox.Show("Материал успешно удален!");
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeMainWindow employeeMainWindow = new EmployeeMainWindow(employeeName, employeeSurname, employeePosition);
            employeeMainWindow.Show();
            this.Close(); // Закрыть текущее окно
        }
    }

    public class MaterialItem // Переименованный класс
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int QuantityInStock { get; set; } // Поле для количества на складе
    }
}