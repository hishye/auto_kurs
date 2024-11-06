using System.Windows;

namespace WpfApp7
{
    public partial class SuccessWindow : Window
    {
        private int clientId; // Поле для хранения clientId

        public SuccessWindow(int clientId) // Конструктор с параметром
        {
            InitializeComponent();
            this.clientId = clientId; // Сохраняем переданное значение
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            OrderClient orderClientWindow = new OrderClient(clientId); // Передаем clientId
            orderClientWindow.Show(); // Открываем окно с заказами
            this.Close(); // Закрываем текущее окно
        }
    }
}