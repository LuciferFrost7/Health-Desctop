using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using HealthDesctop.source;

namespace HealthDesctop
{
    public partial class MainWindow : Window
    {
        private Canvas mainCanvas; // Главный Канвас
        private Canvas registerCanvas = null; // Канвас, который встречает пользователя впервые
        private Canvas loginCanvas = null; // Канвас, который встречает пользователя впервые

        private Canvas profileCanvas; // Панель с профилем пользователя
        private Canvas aimCanvas; // Панель с целью пользователя

        private Canvas settingCanvas; // Панель с настройками приложения

        private Canvas dishCanvas; // Панель с вашими блюдами
        private Canvas createDishCanvas; // Добавить новое блюда

        private Canvas breakfastCanvas; // Панелька с утренним питанием
        private Canvas lunchCanvas; // Панелька с дневным питанием
        private Canvas dinnerCanvas; // Панелька с вечерним питанием

        private Canvas weekCanvas; // Панель с недельным планом

        public MainWindow()
        {
            InitializeComponent();

            this.WindowState = WindowState.Maximized; // Устанавливаем полноэкранный режим

            Loaded += MainWindow_Loaded; // Подключаем событие при загрузки десктопа
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            mainCanvas = new Canvas();
            mainCanvas.Width = this.Width;
            mainCanvas.Height = this.Height;
            mainCanvas.Background = new SolidColorBrush(Colors.AliceBlue);
            this.Content = mainCanvas;
            
            Button btnRegisterNewAccount = new Button();
            btnRegisterNewAccount.Height = 60;
            btnRegisterNewAccount.Width = 240;
            btnRegisterNewAccount.Content = "Зарегистрировать нового пользователя";
            
            mainCanvas.Children.Add(btnRegisterNewAccount);
            Canvas.SetLeft(btnRegisterNewAccount, 10);
            Canvas.SetTop(btnRegisterNewAccount, 30);
            
            btnRegisterNewAccount.Click += RegisterNewUser; // Подключение срабатывания функции на клик
            
            
            Button btnLoginAccount = new Button();
            btnLoginAccount.Height = 60;
            btnLoginAccount.Width = 240;
            btnLoginAccount.Content = "Войти в существующий аккаунт";
            
            mainCanvas.Children.Add(btnLoginAccount);
            Canvas.SetLeft(btnLoginAccount, 10);
            Canvas.SetTop(btnLoginAccount, 30 + 90 + 30);

            btnLoginAccount.Click += LoginUserFunction; // Подключение срабатывания функции на клик
        }

        private void RegisterNewUser(object sender, RoutedEventArgs e)
        {
            if (registerCanvas == null)
            {
                InitializeRegisterCanvas();
            }
        }
        
        void InitializeRegisterCanvas()
        {
            registerCanvas = new Canvas();
            registerCanvas.Width = 1400;
            registerCanvas.Height = this.Height;
            registerCanvas.Background = new SolidColorBrush(Colors.IndianRed);
            
            Canvas.SetLeft(registerCanvas, 240 + 20);
            Canvas.SetTop(registerCanvas, 0);

            mainCanvas.Children.Add(registerCanvas);
        }

        private void LoginUserFunction(object sender, RoutedEventArgs e)
        {
            if (loginCanvas == null)
            {
                InitializeLoginCanvas();
            }
        }

        private void InitializeLoginCanvas()
        {
            loginCanvas = new Canvas();
            loginCanvas.Width = 1400;
            loginCanvas.Height = this.Height;
            loginCanvas.Background = new SolidColorBrush(Colors.Pink);
            
            Canvas.SetLeft(loginCanvas, 240 + 20);
            Canvas.SetTop(loginCanvas, 0);
            
            mainCanvas.Children.Add(loginCanvas);
        }
    }
}