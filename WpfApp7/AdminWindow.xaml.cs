using System.Windows;

namespace WpfApp7
{
    public partial class AdminWindow : Window
    {
        public AdminWindow()
        {
            InitializeComponent();
        }

        private void ManageClientsButton_Click(object sender, RoutedEventArgs e)
        {
            ClientsManagementWindow clientsManagementWindow = new ClientsManagementWindow();
            clientsManagementWindow.Show();
            this.Close(); // Закрыть админ панель
        }

        private void ManageEmployeesButton_Click(object sender, RoutedEventArgs e)
        {
            EmployeeManagementWindow employeeManagementWindow = new EmployeeManagementWindow();
            employeeManagementWindow.Show();
            this.Close(); // Закрыть админ панель
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show(); // Показываем предыдущее окно
            this.Close();
        }
    }
}