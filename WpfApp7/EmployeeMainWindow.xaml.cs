using System.Windows;

namespace WpfApp7
{
    public partial class EmployeeMainWindow : Window
    {
        private string employeeName;
        private string employeeSurname;
        private string employeePosition;

        public EmployeeMainWindow(string name, string surname, string position)
        {
            InitializeComponent();
            employeeName = name;
            employeeSurname = surname;
            employeePosition = position;

            // Отображение информации о сотруднике
            EmployeeInfoTextBlock.Text = $"{employeeName} {employeeSurname}";
            EmployeePositionTextBlock.Text = $"Должность: {employeePosition}";
        }

        private void ViewOrdersButton_Click(object sender, RoutedEventArgs e)
        {
            OrdersWindow ordersWindow = new OrdersWindow(employeeName, employeeSurname, employeePosition);
            ordersWindow.Show();
            this.Close(); // Закрыть текущее окно
        }

        private void ManageServicesButton_Click(object sender, RoutedEventArgs e)
        {
            ManageServicesWindow manageServicesWindow = new ManageServicesWindow(employeeName, employeeSurname, employeePosition);
            manageServicesWindow.Show();
            this.Close(); // Закрыть текущее окно
        }

        private void ManageServiceMaterialsButton_Click(object sender, RoutedEventArgs e)
        {
            ManageServiceMaterialsWindow manageServiceMaterialsWindow = new ManageServiceMaterialsWindow(employeeName, employeeSurname, employeePosition);
            manageServiceMaterialsWindow.Show();
            this.Close(); // Закрыть текущее окно
        }

        private void ManageMaterialsButton_Click(object sender, RoutedEventArgs e)
        {
            ManageMaterialsWindow manageMaterialsWindow = new ManageMaterialsWindow(employeeName, employeeSurname, employeePosition);
            manageMaterialsWindow.Show();
            this.Close(); // Закрыть текущее окно
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show(); // Показываем предыдущее окно
            this.Close();
        }
    }
}