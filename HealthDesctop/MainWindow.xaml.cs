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
        private Canvas registrationForm = null; // Канвас формы регистрации нового пользователя
        private Border registrationFormBorder = null; // Граница с формой регистрации нового пользователя
        
        private Canvas loginCanvas = null; // Канвас, который открывает проход пользователю
        private Border loginCanvasBorder = null; // Граница с канвасом входа
        private Boolean IsRegOrLogMenuOpen = false; // Переменная показывающая открыто ли меню рег-лог сейчас
        private ComboBox usersComboBox = null; // Выпадающий список пользователей
        

        

        private Canvas profileCanvas; // Панель с профилем пользователя
        private Canvas settingCanvas; // Панель с настройками приложения
        
        private Canvas aimsCanvas; // Панель с целью пользователя
        private Canvas dishCanvas; // Панель с вашими блюдами
        // Можно добавить какую-то панельку в главное меню
        
        

        private Canvas MainPageCanvas = null; // Панель, содержащая все основные элементы главного меню
        private static Canvas breakfastCanvas = null; // Панелька с утренним питанием
        private static Canvas lunchCanvas = null; // Панелька с дневным питанием
        private static Canvas dinnerCanvas = null; // Панелька с вечерним питанием

        private Canvas weekAndDayCanvas = null; // Панелька, на которой будет находиться день / неделя меню
        private Canvas daysCanvas = null; // Панель с днями недели
        private Canvas weekCanvas = null; // Панель с недельным планом
        
        private String[] weekDays = { "Понедельник", "Вторник", "Среда", "Четверг", "Пятница", "Суббота", "Воскресенье" } ;
        private Int32 dayOfWeek = (Int32)DateTime.Now.DayOfWeek;
        
        private Canvas FoodCanvas = null; // Панель приёмов пищи
        private Canvas[] FoodType; // Массив Канвасов с приёмами пищи
        private Button btnLeftFood = null;
        private Button btnRightFood = null;

        private Int32[] foodType = { 0, 1, 0 }; // Массив с включёнными тремя приёмами пищи
        
        

        

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
            int spacingY = 80;
            int labelOffset = 0;
            int inputOffset = 5;
            int currentY = startTop;

            void AddInputField(string labelText, string tag)
            {
                var label = Fabric.CreateTextBlock(labelText, 24, 10, currentY + labelOffset, fieldLeft);
                registrationForm.Children.Add(label);

                var textbox = Fabric.CreateTextBox(2, 24, 320, 36, currentY + inputOffset, 2 * fieldLeft);
                textbox.Tag = tag;
                registrationForm.Children.Add(textbox);

                currentY += spacingY;
            }

            // поля формы
            AddInputField("Введите имя:", "firstName");
            AddInputField("Введите фамилию:", "lastName");
            AddInputField("Введите ваш вес:", "weight");
            AddInputField("Введите ваш рост:", "height");
            AddInputField("Введите ваш возраст:", "age");
            AddInputField("Дата рождения:", "birthDate");

            // Кнопка регистрации
            Button btnRegUser = Fabric.CreateButton("Зарегистрировать", 240, 60,
                (int)(registrationForm.Width / 2 - 240), currentY + 20);
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

            // Обработка события нажатия
            btnRegUser.Click += BtnRegUserOnClick;
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

            // Создаём выпадающий список пользователей
            usersComboBox = Fabric.CreateComboBox(40, 240, (Int32)this.Height / 2,(Int32)loginCanvas.Width / 2 - 240);
            List<User> users = User.GetUsers();
            if (users.Count == 0)
            { }
            else
            {
                foreach (User user in users)
                {
                    usersComboBox.Items.Add(user); // Добавляем сам объект User
                }
            }
            loginCanvas.Children.Add(usersComboBox);
            
            // Подключаем событие выбора пользователя
            usersComboBox.SelectionChanged += OnUserSelected;

            
            
            
            
            
            
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
                if (currentUser != null)
                {
                    ClearRegOrLogScreen();
                }
            };
        }

        private User currentUser = null;
        private void OnUserSelected(object sender, SelectionChangedEventArgs e)
        {
            if (usersComboBox.SelectedItem is User selectedUser)
            {
                // Пример действия: вывод в консоль или сохранение в переменную
                Console.WriteLine($"Выбран пользователь: {selectedUser}");

                // Можешь сохранить его в поле, если надо использовать позже
                currentUser = selectedUser;
            }
        }
        
        
        // Функция регистрации пользователя
        private void BtnRegUserOnClick(object sender, RoutedEventArgs e)
        {
            try
            {
                TextBox tbFirstName = GetTextBoxByTag("firstName");
                TextBox tbLastName = GetTextBoxByTag("lastName");
                TextBox tbWeight = GetTextBoxByTag("weight");
                TextBox tbHeight = GetTextBoxByTag("height");
                TextBox tbAge = GetTextBoxByTag("age");
                TextBox tbBirthDate = GetTextBoxByTag("birthDate");

                TextBox[] tBoxes = { tbFirstName, tbLastName, tbWeight, tbHeight, tbAge, tbBirthDate };

                foreach (var tBox in tBoxes)
                {
                    if (string.IsNullOrWhiteSpace(tBox.Text))
                    {
                        MessageBox.Show($"Поле {tBox.Tag} пустое!", "Lost Field", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }
                }

                User user = new User();
                user.FirstName = tbFirstName.Text;
                user.LastName = tbLastName.Text;
                user.Weight = Convert.ToDouble(tbWeight.Text);
                user.Height = Convert.ToDouble(tbHeight.Text);
                user.Age = Convert.ToInt32(tbAge.Text);

                if (!DateOnly.TryParse(tbBirthDate.Text, out DateOnly birthDate))
                {
                    MessageBox.Show("Неверный формат даты рождения. Используй формат: гггг-мм-дд",
                        "Ошибка ввода", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                user.BirthDate = birthDate;

                User.RegisterNewUser(user);
                MessageBox.Show("Пользователь зарегистрирован!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Произошла ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        
        private TextBox GetTextBoxByTag(String tag)
        {
            TextBox tBox = registrationForm.Children
                .OfType<TextBox>()
                .FirstOrDefault(tb => (string)tb.Tag == tag);

            if (tBox != null)
            {
                return tBox;
            }
            else
            {
                throw new Exception($"Поле с тегом '{tag}' для ввода не найдено");
            }
        }
        
        
        
        
        
        
        
        
        
        
        
        // Функция для вызова главного меню
        private void MainMenu()
        {
            // Создание главного фона
            MainPageCanvas = Fabric.CreateCanvas((Int32)this.Height, (Int32)this.Width, 0, 0);
            MainPageCanvas.Background = new SolidColorBrush(Colors.LightGreen);
            mainCanvas.Children.Add(MainPageCanvas);
            
            // Создание Панели День-Неделя
            InitializeDayWeekCanvas();
        }
        
        
        // Функция для создания Панели День-Неделя
        private void InitializeDayWeekCanvas()
        {
            weekAndDayCanvas = Fabric.CreateCanvas(116, (Int32)this.Width - 140, 0, 0);
            weekAndDayCanvas.Background = new SolidColorBrush(Colors.LightGray);

            Button btnWeek = Fabric.CreateButton("Неделя", (Int32)weekAndDayCanvas.Width / 2 - 34, 46, 9, 10);
            btnWeek.ClearValue(Button.BackgroundProperty);
            weekAndDayCanvas.Children.Add(btnWeek);
            
            Button btnDays = Fabric.CreateButton("Дни недели", (Int32)weekAndDayCanvas.Width / 2 - 34, 46, (Int32)weekAndDayCanvas.Width / 2 + 9 + 6, 10);
            btnDays.Background = new SolidColorBrush(Colors.AliceBlue);
            weekAndDayCanvas.Children.Add(btnDays);
            
            mainCanvas.Children.Add(weekAndDayCanvas);
            
            InitializeDaysCanvas();
            weekAndDayCanvas.Children.Add(daysCanvas);
            
            // Добавляем событие при нажатии на кнопку Неделя
            btnWeek.Click += BtnWeek_Click;
            
            // Добавляем событие при нажатии на кнопку дней нежели
            btnDays.Click += BtnDays_Click;
        }
        private void BtnWeek_Click(object sender, RoutedEventArgs e)
        {
            ResetToggleButtons();
            ((Button)sender).Background = new SolidColorBrush(Colors.AliceBlue);
            
            // Убираем панель по дням, если она открыта
            if (daysCanvas != null)
            {
                DeInitializeDaysCanvas();
            }

            // Добавляем панель по неделе
            InitializeWeekCanvas();
            weekAndDayCanvas.Children.Add(weekCanvas);
        }
        private void BtnDays_Click(object sender, RoutedEventArgs e)
        {
            ResetToggleButtons();
            ((Button)sender).Background = new SolidColorBrush(Colors.AliceBlue);

            // Убираем панель недели, если она открыта
            if (weekCanvas != null)
            {
                DeInitializeWeekCanvas();
            }
            
            // Создаём панель дней
            InitializeDaysCanvas();
            weekAndDayCanvas.Children.Add(daysCanvas);
        }
        private void ResetToggleButtons()
        {
            foreach (UIElement element in weekAndDayCanvas.Children)
            {
                if (element is Button btn)
                {
                    btn.ClearValue(Button.BackgroundProperty);
                }
            }
        }

        
        // Функция для создания панели Дней
        private void InitializeDaysCanvas()
        {
            daysCanvas = Fabric.CreateCanvas(50, (Int32)weekAndDayCanvas.Width - 40, 46 + 10 + 5, 14);
            daysCanvas.Background = new SolidColorBrush(Colors.Gray);
            
            Int32 standartSpace = 8;
            Int32 buttonWeight = Convert.ToInt32((daysCanvas.Width - ((weekDays.Length + 1) * standartSpace)) / weekDays.Length);
            
            for (int i = 0; i < weekDays.Length; i++)
            {
                Button btn = Fabric.CreateButton(weekDays[i], buttonWeight, 42, i * buttonWeight + 5 * (i + 1), 4);
                btn.Background = new SolidColorBrush(Colors.LightGray);
                btn.Tag = weekDays[i];
                daysCanvas.Children.Add(btn);
                
                btn.Click += DayBtnOnClick;
            }
            
            // просматриваем кнопки в поиске кнопки с текущим днём недели
            foreach (Button btn in daysCanvas.Children)
            {
                if ((string)btn.Tag == weekDays[dayOfWeek])
                {
                    DayBtnOnClick(btn, null); // Симулируем клик
                    break;
                }
            }
            InitializeFoodCanvas();
            MainPageCanvas.Children.Add(FoodCanvas);
        }
        // Функция для очистки панели дней
        private void DeInitializeDaysCanvas()
        {
            if (daysCanvas != null)
            {
                MainPageCanvas.Children.Remove(FoodCanvas);
                DeInitializeFoodCanvas();
                daysCanvas.Children.Clear();
                daysCanvas = null;
            }
        }
        private void DayBtnOnClick(object sender, RoutedEventArgs e)
        {
            Button clickedBtn = sender as Button;
            if (clickedBtn == null) return;

            // Сброс цвета всех кнопок
            foreach (Button btn in daysCanvas.Children)
            {
                btn.Background = new SolidColorBrush(Colors.LightGray);

                if ((String)btn.Tag == weekDays[dayOfWeek])
                {
                    btn.Background = new SolidColorBrush(Colors.AliceBlue);
                }
            }

            // Выделяем нажатую кнопку
            clickedBtn.Background = new SolidColorBrush(Colors.MediumSpringGreen);
        }

        
        // Функция для создания панели недели
        private void InitializeWeekCanvas()
        {
            weekCanvas = Fabric.CreateCanvas((Int32)(this.Height * 0.75), (Int32)this.Width - 60 - 140,
                (Int32)(this.Height * 0.18), 25);
            weekCanvas.Background = new SolidColorBrush(Colors.LightGray);
        }
        // Функция для очистки панели дней
        private void DeInitializeWeekCanvas()
        {
            if(weekCanvas != null){
                weekAndDayCanvas.Children.Remove(weekCanvas);
                weekCanvas.Children.Clear();
                weekCanvas = null;
            }
        }

        
        // Функция для создания панели приёмов пищи
        private void InitializeFoodCanvas()
        {
            FoodCanvas = Fabric.CreateCanvas((Int32)(this.Height * 0.75), (Int32)(this.Width * 0.8),
                (Int32)(this.Height * 0.18), 25);
            FoodCanvas.Background = new SolidColorBrush(Colors.LightGray);
            
            // Добавляем Панель Завтраков
            InitializeBreakfastCanvas();
            FoodCanvas.Children.Add(breakfastCanvas);
            
            // Добавляем Панель Обедов
            InitializeLunchCanvas();
            FoodCanvas.Children.Add(lunchCanvas);
            
            // Добавляем Панель Ужинов
            InitializeDinnerCanvas();
            FoodCanvas.Children.Add(dinnerCanvas);
            
            FoodType = new Canvas[] { breakfastCanvas, lunchCanvas, dinnerCanvas };
            
            // Создание Левой кнопки переключения приёмов пищи
            btnLeftFood = Fabric.CreateButton("<", 40, 40, 5, (Int32)(FoodCanvas.Height / 2) - 20);
            btnLeftFood.FontWeight = FontWeights.Bold;
            btnLeftFood.FontSize = 20;
            btnLeftFood.Tag = "leftButton";
            FoodCanvas.Children.Add(btnLeftFood);
            
            // Создание Правой кнопки переключения приёмов пищи
            btnRightFood = Fabric.CreateButton(">", 40, 40, (Int32)(FoodCanvas.Width - 40 - 5), (Int32)(FoodCanvas.Height / 2) - 20);
            btnRightFood.FontWeight = FontWeights.Bold;
            btnRightFood.FontSize = 20;
            btnRightFood.Tag = "rightButton";
            FoodCanvas.Children.Add(btnRightFood);
            
            FoodCanvasUpdateVisibility();
            
            // Добавляем событие на нажатие левой кнопки
            btnLeftFood.Click += BtnLeftFoodOnClick;
            
            // Добавляем событие на нажатие правой кнопки
            btnRightFood.Click += BtnRightFoodOnClick;
        }

        private void BtnLeftFoodOnClick(object sender, RoutedEventArgs e)
        {
            if (foodType[0] == 1)
            {
            }
            else if (foodType[1] == 1)
            {
                foodType[0] = 1;
                foodType[1] = 0;
            }
            else if (foodType[2] == 1)
            {
                foodType[1] = 1;
                foodType[2] = 0;
            }
            FoodCanvasUpdateVisibility();
        }
        
        private void BtnRightFoodOnClick(object sender, RoutedEventArgs e) {
            if (foodType[0] == 1)
            {
                foodType[0] = 0;
                foodType[1] = 1;
            }
            else if (foodType[1] == 1)
            {
                foodType[1] = 0;
                foodType[2] = 1;
            }
            else if (foodType[2] == 1)
            {
            }
            FoodCanvasUpdateVisibility();
        }

        private void FoodCanvasUpdateVisibility()
        {
            btnLeftFood.Foreground = new SolidColorBrush(foodType[0] == 1 ? Colors.Gray : Colors.Black);
            btnRightFood.Foreground = new SolidColorBrush(foodType[2] == 1 ? Colors.Gray : Colors.Black);
   
            for (Int32 i = 0; i < foodType.Length; i++)
            {
                if (foodType[i] == 1)
                {
                    if (FoodType[i] != null)
                    {
                        FoodType[i].Visibility = Visibility.Visible;
                    }
                }
                else
                {
                    if (FoodType[i] != null)
                    {
                        FoodType[i].Visibility = Visibility.Hidden;
                    }
                }
            }
        }

        // Функция для очистки панели приёмов пищи
        private void DeInitializeFoodCanvas()
        {
            if(FoodCanvas != null)
            {
                btnLeftFood = null;
                btnRightFood = null;
                DeInitializeBreakfastCanvas();
                DeInitializeLunchCanvas();
                DeInitializeDinnerCanvas();

                FoodCanvas.Children.Clear();
                FoodCanvas = null;
            }
        }
        
        // Функция для создания панели завтрака
        private void InitializeBreakfastCanvas()
        {
            Int32 height = (Int32)(FoodCanvas.Height);
            Int32 width = (Int32)(FoodCanvas.Width - 80 - 10 - 10);
            breakfastCanvas = Fabric.CreateCanvas((Int32)(height * 0.9), width, (Int32)(height * 0.05), 5 + 40 + 5);
            breakfastCanvas.Background = new SolidColorBrush(Colors.DeepSkyBlue);
            breakfastCanvas.Visibility = Visibility.Hidden;
        }
        // Функция для очистки панели завтрака
        private void DeInitializeBreakfastCanvas()
        {
            if (breakfastCanvas != null)
            {
                breakfastCanvas.Children.Clear();
                breakfastCanvas = null;
            }
        }
        
        // Функция для создания панели Обеда
        private void InitializeLunchCanvas()
        {
            Int32 height = (Int32)(FoodCanvas.Height);
            Int32 width = (Int32)(FoodCanvas.Width - 80 - 10 - 10);
            lunchCanvas = Fabric.CreateCanvas((Int32)(height * 0.9), width, (Int32)(height * 0.05), 5 + 40 + 5);
            lunchCanvas.Background = new SolidColorBrush(Colors.Gray);
            lunchCanvas.Visibility = Visibility.Hidden;
        }
        // Функция для очистки панели обеда
        private void DeInitializeLunchCanvas()
        {
            if (lunchCanvas != null)
            {
                lunchCanvas.Children.Clear();
                lunchCanvas = null;
            }
        }
        
        
        // Функция для создания панели ужина
        private void InitializeDinnerCanvas()
        {
            Int32 height = (Int32)(FoodCanvas.Height);
            Int32 width = (Int32)(FoodCanvas.Width - 80 - 10 - 10);
            dinnerCanvas = Fabric.CreateCanvas((Int32)(height * 0.9), width, (Int32)(height * 0.05), 5 + 40 + 5);
            dinnerCanvas.Background = new SolidColorBrush(Colors.PaleVioletRed);
            dinnerCanvas.Visibility = Visibility.Hidden;
        }
        // Функция для очистки панели ужина
        private void DeInitializeDinnerCanvas()
        {
            if (dinnerCanvas != null)
            {
                dinnerCanvas.Children.Clear();
                dinnerCanvas = null;
            }
        }
    }
}