using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

// Подключаем классы для работы с профилем пользователя
using HealthDesctop.source.User;

// Подключаем класс Фабрика для быстрого создания элементов Интерфейса
using HealthDesctop.source.Fabric;

// Подключаем анимции
using HealthDesctop.source.Animations;


namespace HealthDesctop
{
    public partial class MainWindow : Window
    {
        private Canvas mainCanvas; // Главный Канвас

        private Canvas buttonsRegLogMenuCanvas = null; // Канвас, на котором расположены кнопки Рег / Лог
        private Border buttonsRegLogMenuBorder = null; // Граница с канвас рег-лог
        
        private Canvas registerCanvas = null; // Канвас, который встречает пользователя впервые
        private Border registerCanvasBorder = null; // Граница с канвасом регистрации
        private Canvas loginCanvas = null; // Канвас, который открывает проход пользователю
        private Border loginCanvasBorder = null; // Граница с канвасом входа
        private Boolean IsRegOrLogMenuOpen = false; // Переменная показывающая открыто ли меню рег-лог сейчас

        
        private Canvas registrationForm = null; // Канвас формы регистрации нового пользователя
        private Border registrationFormBorder = null; // Граница с формой регистрации нового пользователя
        

        private Canvas profileCanvas; // Панель с профилем пользователя
        private Canvas aimCanvas; // Панель с целью пользователя

        private Canvas settingCanvas; // Панель с настройками приложения

        private Canvas dishCanvas; // Панель с вашими блюдами
        private Canvas createDishCanvas; // Добавить новое блюда

        private Canvas MainPageCanvas; // Панель, содержащая все основные элементы главного меню
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
        
        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Создание главной панели
            mainCanvas = Fabric.CreateCanvas((Int32)this.Height, (Int32)this.Width);
            mainCanvas.Background = new SolidColorBrush(Colors.AliceBlue);
            this.Content = mainCanvas;

            // Инициализация кнопочной панели (но не анимируем сразу!)
            InitializeRegOrLogMenu();

            // Ждём полной загрузки UI
            await Dispatcher.InvokeAsync(() => { }, System.Windows.Threading.DispatcherPriority.Loaded);

            // Ждём ещё 1 секунду
            await Task.Delay(1000);
            
            
            buttonsRegLogMenuBorder.Visibility = Visibility.Visible;
            // После этого запускаем анимацию
            Animations.ShowButtonsPanelFromLeft(buttonsRegLogMenuBorder);
        }
        
        // Функция для создания Панели с кнопками Регистрации и Входа
        private void InitializeRegOrLogMenu()
        {
            // Создание панели с кнопками Регистрации и Входа
            buttonsRegLogMenuCanvas = Fabric.CreateCanvas((Int32)this.Height, 240 + 21);
            buttonsRegLogMenuCanvas.Background = new SolidColorBrush(Colors.LightGray);

            buttonsRegLogMenuBorder = Fabric.CreateBorder(
                Fabric.CreateThickness(0, 0, 1, 0), Brushes.Black
                );
            buttonsRegLogMenuBorder.Child = buttonsRegLogMenuCanvas;
            Canvas.SetLeft(buttonsRegLogMenuBorder, 0); 
            buttonsRegLogMenuBorder.Visibility = Visibility.Hidden;
            mainCanvas.Children.Add(buttonsRegLogMenuBorder);
            
            // Создание кнопки зарегистрировать
            String regBtnText = "Зарегистрировать нового пользователя";
            Button btnRegisterNewAccount = Fabric.CreateButton(regBtnText, 240, 60, 10, 30);
            buttonsRegLogMenuCanvas.Children.Add(btnRegisterNewAccount);
            
            // Создание кнопки входа
            String logBtnText = "Войти в существующий аккаунт";
            Button btnLoginAccount = Fabric.CreateButton(logBtnText, 240, 60, 10, 30 + 90 + 30);
            buttonsRegLogMenuCanvas.Children.Add(btnLoginAccount);
            
            btnRegisterNewAccount.Click += RegisterNewUser; // Подключение срабатывания функции Регистрации на клик
            btnLoginAccount.Click += LoginUserFunction; // Подключение срабатывания функции Входа на клик
        }
        // Функция для удаления Панели с кнопками Регистрации и Входа
        private void DeInitializeRegOrLogMenu()
        {
            if (buttonsRegLogMenuBorder != null)
            {
                buttonsRegLogMenuBorder.Child = null;
                mainCanvas.Children.Remove(buttonsRegLogMenuBorder);
                buttonsRegLogMenuBorder = null;
            }

            buttonsRegLogMenuCanvas = null;
        }
        
        // Функция, которая отчищает весь главный экран
        private void ClearRegOrLogScreen()
        {
            if (buttonsRegLogMenuBorder != null)
            {
                Animations.HideButtonsPanelToLeft(buttonsRegLogMenuBorder, () =>
                {
                    // Скрываем панель регистрации, если она есть
                    if (registerCanvasBorder != null)
                    {
                        Animations.FadeOutRegOrLogMenu(registerCanvasBorder, () =>
                        {
                            DeInitializeRegisterCanvas();
                        });
                    }

                    // Скрываем панель входа, если она есть
                    if (loginCanvasBorder != null)
                    {
                        Animations.FadeOutRegOrLogMenu(loginCanvasBorder, () =>
                        {
                            DeInitializeLoginCanvas();
                        });
                    }

                    // Скрываем панель Рег-Лог
                    DeInitializeRegOrLogMenu();
                    IsRegOrLogMenuOpen = false;
                    
                    // Запускаем анимацию белого экрана in
                    Animations.FadeInWhite(mainCanvas, () =>
                    {
                        // Очищаем главный Канвас
                        mainCanvas.Children.Clear();

                        // Появляется главное меню
                        MainMenu();

                        // Запускаем анимацию белого экрана out
                        Animations.FadeOutWhite(mainCanvas);
                    });
                });
            }
        }
        
        
        // Функция для создания / об-NULL-ения панели регистрации нового пользователя 
        private void RegisterNewUser(object sender, RoutedEventArgs e)
        {
            if (registerCanvas == null)
            {
                if (loginCanvas != null)
                {
                    DeInitializeLoginCanvas();
                    InitializeRegisterCanvas();
                }
                else
                {
                    InitializeRegisterCanvas();
                    Animations.FadeInRegOrLogMenu(registerCanvasBorder);
                    IsRegOrLogMenuOpen = true;
                }
            }
            else
            {
                Animations.FadeOutRegOrLogMenu(registerCanvasBorder, () =>
                {
                    DeInitializeRegisterCanvas();
                });
                IsRegOrLogMenuOpen = false;
            }
        }
        // обНуление панели регистрации
        private void DeInitializeRegisterCanvas()
        {
            registrationForm.Children.Clear();
            registerCanvasBorder.Child = null;
            registerCanvasBorder = null;
            mainCanvas.Children.Remove(registerCanvas);
            registerCanvas = null;
        }
        // Создание панели регистрации
        void InitializeRegisterCanvas()
        {
            // Создание панели регистрации
            registerCanvas = Fabric.CreateCanvas((int)this.Height, 1400, 0, 0);
            registerCanvas.Background = new SolidColorBrush(Colors.IndianRed);

            registrationForm = Fabric.CreateCanvas((int)this.Height - 80, 1320, 0, 0);
            registrationForm.Background = new SolidColorBrush(Colors.SlateGray);

            int fieldLeft = 260;
            int startTop = 100;
            int spacingY = 80; // Вертикальное расстояние между блоками
            int labelOffset = 0;
            int inputOffset = 5;
            int currentY = startTop;

            void AddInputField(string labelText)
            {
                var label = Fabric.CreateTextBlock(labelText, 24, 10, currentY + labelOffset, fieldLeft);
                registrationForm.Children.Add(label);

                var textbox = Fabric.CreateTextBox(2, 24, 320, 36, currentY + inputOffset, 2 * fieldLeft);
                registrationForm.Children.Add(textbox);

                currentY += spacingY;
            }

            // поля формы
            AddInputField("Введите имя:");
            AddInputField("Введите фамилию:");
            AddInputField("Введите ваш вес:");
            AddInputField("Введите ваш рост:");
            AddInputField("Введите ваш возраст:");
            AddInputField("Дата рождения:");
            
            // Кнопка регистрации
            Button btnRegUser = Fabric.CreateButton("Зарегистрировать", 240, 60,
                (Int32)(registrationForm.Width / 2 - 240), currentY + 20);
            registrationForm.Children.Add(btnRegUser);
            
            registrationFormBorder = Fabric.CreateBorder(
                Fabric.CreateThickness(2, 2, 0, 2), Brushes.Black, 140, 40
            );
            registrationFormBorder.Child = registrationForm;

            registerCanvas.Children.Add(registrationFormBorder);
            
            registerCanvasBorder = Fabric.CreateBorder(
                Fabric.CreateThickness(3, 0, 0, 0), Brushes.Black, 260, 0
            );
            registerCanvasBorder.Child = registerCanvas;

            mainCanvas.Children.Add(registerCanvasBorder);
        }

        
        // Функция для создания / об-NULL-ения панели входа пользователей
        private void LoginUserFunction(object sender, RoutedEventArgs e)
        {
            if (loginCanvas == null)
            {
                if (registerCanvas != null)
                {
                    DeInitializeRegisterCanvas();
                    InitializeLoginCanvas();
                }
                else
                {
                    InitializeLoginCanvas();
                    Animations.FadeInRegOrLogMenu(loginCanvasBorder);
                    IsRegOrLogMenuOpen = true;
                }
            }
            else
            {
                Animations.FadeOutRegOrLogMenu(loginCanvasBorder, () =>
                {
                    DeInitializeLoginCanvas();
                });
                IsRegOrLogMenuOpen = false;
            }
        }
        // обНулление панели входа
        private void DeInitializeLoginCanvas()
        {
            loginCanvasBorder.Child = null;
            loginCanvasBorder = null;
            mainCanvas.Children.Remove(loginCanvas);
            loginCanvas = null;
        }
        // Создание панели входа
        private void InitializeLoginCanvas()
        {
            // Создание панели входа (Canvas)
            loginCanvas = Fabric.CreateCanvas((Int32)this.Height, 1400, 0, 0);
            loginCanvas.Background = new SolidColorBrush(Colors.Pink);

            // Создание кнопки входа в аккаунт
            Button btnLogInAccount = Fabric.CreateButton(
                "Войти", 240, 60, (Int32)loginCanvas.Width / 2 - 120, (Int32)(loginCanvas.Height * 0.75) - 30
                );
            loginCanvas.Children.Add(btnLogInAccount);
            
            // Создание границы (только слева, 5px, чёрная)
            loginCanvasBorder = Fabric.CreateBorder(
                Fabric.CreateThickness(3, 0, 0, 0), Brushes.Black, 240 + 20, 0
            );

            // Устанавливаем loginCanvas внутрь границы
            loginCanvasBorder.Child = loginCanvas;

            // Добавляем границу в основную канву
            mainCanvas.Children.Add(loginCanvasBorder);
            
            // Добавляем событие при нажатии на кнопку входа
            btnLogInAccount.Click += (object sender, RoutedEventArgs e) =>
            {
                ClearRegOrLogScreen();
            };
        }
        
        
        // Функция для вызова главного меню
        private void MainMenu()
        {
            MainPageCanvas = Fabric.CreateCanvas((Int32)this.Height, (Int32)this.Width, 0, 0);
            MainPageCanvas.Background = new SolidColorBrush(Colors.MediumVioletRed);
            mainCanvas.Children.Add(MainPageCanvas);
        }
    }
}