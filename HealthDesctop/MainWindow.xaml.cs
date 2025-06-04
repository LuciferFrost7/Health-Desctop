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

// Подключаем классы для работы с профилем пользователя
using HealthDesctop.source.User;

// Подключаем класс Фабрика для быстрого создания элементов Интерфейса
using HealthDesctop.source.Fabric;

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
            // Создание главной Панели, на которой и будет держаться всё приложение
            mainCanvas = Fabric.CreateCanvas((Int32)this.Height, (Int32)this.Width);
            mainCanvas.Background = new SolidColorBrush(Colors.AliceBlue);
            this.Content = mainCanvas;

            // Создание кнопки зарегистрировать
            String regBtnText = "Зарегистрировать нового пользователя";
            Button btnRegisterNewAccount = Fabric.CreateButton(regBtnText, 60, 240, 10, 30);
            mainCanvas.Children.Add(btnRegisterNewAccount);
            
            // Создание кнопки входа
            String logBtnText = "Войти в существующий аккаунт";
            Button btnLoginAccount = Fabric.CreateButton(logBtnText, 60, 240, 10, 30 + 90 + 30);
            mainCanvas.Children.Add(btnLoginAccount);
            
            btnRegisterNewAccount.Click += RegisterNewUser; // Подключение срабатывания функции Регистрации на клик
            btnLoginAccount.Click += LoginUserFunction; // Подключение срабатывания функции Входа на клик
        }

        
        // Функция для создания / об-NULL-ения панели регистрации нового пользователя 
        private void RegisterNewUser(object sender, RoutedEventArgs e)
        {
            if (registerCanvas == null)
            {
                if (loginCanvas != null)
                {
                    DeInitializeLoginCanvas();
                }
                InitializeRegisterCanvas();
            }
            else
            {
                DeInitializeRegisterCanvas();
            }
        }
        // обНуление панели регистрации
        private void DeInitializeRegisterCanvas()
        {
            mainCanvas.Children.Remove(registerCanvas);
            registerCanvas = null;
        }
        // Создание панели регистрации
        void InitializeRegisterCanvas()
        {
            // Создание панели регистрации
            registerCanvas = Fabric.CreateCanvas(1400, (Int32)this.Height, 0, 0);
            registerCanvas.Background = new SolidColorBrush(Colors.IndianRed);

            // Создание границы (только слева, 5px, чёрная)
            Border border = Fabric.CreateBorder(
                Fabric.CreateThickness(3, 0, 0, 0), Brushes.Black, 240 + 20, 0
            );

            border.Child = registerCanvas;

            // Добавляем именно border, чтобы граница отобразилась
            mainCanvas.Children.Add(border);
        }

        
        // Функция для создания / об-NULL-ения панели входа пользователей
        private void LoginUserFunction(object sender, RoutedEventArgs e)
        {
            if (loginCanvas == null)
            {
                if (registerCanvas != null)
                {
                    DeInitializeRegisterCanvas();
                }
                InitializeLoginCanvas();
            }
            else
            {
                DeInitializeLoginCanvas();
            }
        }
        // обНулление панели входа
        private void DeInitializeLoginCanvas()
        {
            mainCanvas.Children.Remove(loginCanvas);
            loginCanvas = null;
        }
        // Создание панели входа
        private void InitializeLoginCanvas()
        {
            // Создание панели входа (Canvas)
            loginCanvas = Fabric.CreateCanvas(1400, (int)this.Height, 0, 0);
            loginCanvas.Background = new SolidColorBrush(Colors.Pink);

            // Создание границы (только слева, 5px, чёрная)
            Border border = Fabric.CreateBorder(
                Fabric.CreateThickness(3, 0, 0, 0), Brushes.Black, 240 + 20, 0
            );

            // Устанавливаем loginCanvas внутрь границы
            border.Child = loginCanvas;

            // Добавляем границу в основную канву
            mainCanvas.Children.Add(border);
        }

        
        
    }
}