using System;
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

// Подключае работу с Реляционной Локальной БД
using HealthDesctop.source.LocalDB;
using HealthDesctop.source.LocalDB.Tables;


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
        
        // 2

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
        
        // 3
        


        

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
            
            Button btnAddProduct = Fabric.CreateButton("Добавить продукт", 180, 60, (Int32)this.Width - 90 - 180, 150);
            MainPageCanvas.Children.Add(btnAddProduct);
            btnAddProduct.Click += (s, e) => InitializeFullProductAndCategoryCanvas();
            
            var btnCreateCategory = Fabric.CreateButton("Создать категорию", 180, 40, (Int32)this.Width - 90 - 180, 300); // под кнопкой продукта
            btnCreateCategory.Click += (s, e) =>
            {
                ShowCreateCategoryFullCanvas(); // метод формы создания категории
            };

            MainPageCanvas.Children.Add(btnCreateCategory);

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
        
        
        
        
        
        
    private void InitializeFullProductAndCategoryCanvas()
    {
        Canvas canvas = Fabric.CreateCanvas((Int32)this.Height, 700, 0, 0);
        canvas.Background = new SolidColorBrush(Colors.WhiteSmoke);

        int y = 20;

        // --- Поля продукта ---
        var fields = new List<(string Label, string Tag)>
        {
            ("Название", "Name"),
            ("Калории", "Calories"),
            ("Белки", "Proteins"),
            ("Жиры", "Fats"),
            ("Углеводы", "Carbs")
        };

        foreach (var field in fields)
        {
            var label = Fabric.CreateTextBlock(field.Label, 16, 10, y, 10);
            var textbox = Fabric.CreateTextBox(2, 16, 300, 30, y + 30, 10);
            textbox.Tag = field.Tag;
            canvas.Children.Add(label);
            canvas.Children.Add(textbox);
            y += 70;
        }

        // --- Категории ---
        var categories = DbCommands.GetAllCategories();

        var cbCat1 = new ComboBox { Width = 300, Height = 30, Margin = new Thickness(10, y, 0, 0) };
        cbCat1.ItemsSource = categories;
        cbCat1.DisplayMemberPath = "Name";
        canvas.Children.Add(cbCat1);
        y += 50;

        var cbCat2 = new ComboBox { Width = 300, Height = 30, Margin = new Thickness(10, y, 0, 0) };
        cbCat2.ItemsSource = categories;
        cbCat2.DisplayMemberPath = "Name";
        canvas.Children.Add(cbCat2);
        y += 60;

        // --- Кнопка создания продукта ---
        var btnCreate = Fabric.CreateButton("Создать продукт", 160, 40, 10, y);
        btnCreate.Click += (s, e) =>
        {
            try
            {
                string name = GetTextFromCanvas(canvas, "Name");
                int cal = int.Parse(GetTextFromCanvas(canvas, "Calories"));
                int prot = int.Parse(GetTextFromCanvas(canvas, "Proteins"));
                int fat = int.Parse(GetTextFromCanvas(canvas, "Fats"));
                int carb = int.Parse(GetTextFromCanvas(canvas, "Carbs"));

                var product = DbCommands.AddProduct(name, cal, prot, fat, carb);

                tbl_Category cat1 = cbCat1.SelectedItem as tbl_Category;
                tbl_Category cat2 = cbCat2.SelectedItem as tbl_Category;

                if (cat1 != null)
                    DbCommands.AddListEntry(product.Id, cat1.Id);

                if (cat2 != null && cat2.Id != cat1?.Id)
                    DbCommands.AddListEntry(product.Id, cat2.Id);

                MessageBox.Show("Продукт создан!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        };
        canvas.Children.Add(btnCreate);

        // --- Кнопка "Создать категорию" ---
        var btnCreateCategory = Fabric.CreateButton("Создать категорию", 160, 40, 200, y);
        btnCreateCategory.Click += (s, e) =>
        {
            weekAndDayCanvas.Children.Remove(canvas);
            ShowAllProductsCanvasWithComboBox(); // вызывает отдельно форму для просмотра продуктов
        };
        canvas.Children.Add(btnCreateCategory);

        y += 70;

        // --- ComboBox со всеми продуктами ---
        var comboBox = new ComboBox
        {
            Width = 360,
            Height = 60,
            Margin = new Thickness(10, y, 0, 0),
            HorizontalContentAlignment = HorizontalAlignment.Left,
            VerticalContentAlignment = VerticalAlignment.Top,
            FontSize = 14
        };

        var products = DbCommands.GetAllProducts();
        foreach (var product in products)
        {
            var item = new ComboBoxItem
            {
                Content = $"{product.Name} — {product.Calories} ккал\n" +
                          $"Б: {product.Proteins} Ж: {product.Fats} У: {product.Carbohydrates}",
                Tag = product.Id
            };
            comboBox.Items.Add(item);
        }

        canvas.Children.Add(comboBox);
        y += 80;

        // --- Инфо о продукте + категориях ---
        var tbInfo = new TextBlock
        {
            FontSize = 14,
            Margin = new Thickness(10, y, 0, 0),
            TextWrapping = TextWrapping.Wrap,
            Width = 760,
            Height = 100
        };
        canvas.Children.Add(tbInfo);
        y += 70;

        comboBox.SelectionChanged += (s, e) =>
        {
            if (comboBox.SelectedItem is ComboBoxItem selected)
            {
                int id = (int)selected.Tag;
                var product = DbCommands.GetProductById(id);
                var categoriesList = DbCommands.GetAllListEntries()
                    .Where(l => l.Fk_ProductId == id)
                    .Select(l => l.Category.Name);

                tbInfo.Text =
                    $"Название: {product.Name}\nКкал: {product.Calories}\nБ: {product.Proteins}, Ж: {product.Fats}, У: {product.Carbohydrates}\n" +
                    $"Категории: {string.Join(", ", categoriesList)}";
            }
        };

        // --- Кнопка удаления ---
        var btnDelete = Fabric.CreateButton("Удалить продукт", 160, 40, 10, y);
        btnDelete.Click += (s, e) =>
        {
            if (comboBox.SelectedItem is ComboBoxItem selected)
            {
                int id = (int)selected.Tag;
                DbCommands.DeleteProduct(id);
                MessageBox.Show("Продукт удалён.");
                MainPageCanvas.Children.Remove(canvas);
            }
            else
            {
                MessageBox.Show("Выберите продукт.");
            }
        };
        canvas.Children.Add(btnDelete);

        Border borderCanvas = Fabric.CreateBorder(Fabric.CreateThickness(2, 2, 2, 2), Brushes.Black, 50, 50);
        borderCanvas.Child = canvas;
        
        // --- Кнопка закрытия ---
        var btnClose = Fabric.CreateButton("Закрыть", 160, 40, 200, y);
        btnClose.Click += (s, e) => weekAndDayCanvas.Children.Remove(borderCanvas);
        canvas.Children.Add(btnClose);

        weekAndDayCanvas.Children.Add(borderCanvas);
    }
  
    private void ShowAllProductsCanvasWithComboBox() {
        var canvas = Fabric.CreateCanvas(400, 550, 0, 0);
        canvas.Background = new SolidColorBrush(Colors.WhiteSmoke);

        var comboBox = new ComboBox
        {
            Width = 360,
            Height = 60,
            Margin = new Thickness(20, 20, 0, 0),
            HorizontalContentAlignment = HorizontalAlignment.Left,
            VerticalContentAlignment = VerticalAlignment.Top,
            FontSize = 14
        };

        var products = DbCommands.GetAllProducts();

        foreach (var product in products)
        {
            var item = new ComboBoxItem
            {
                Content = $"{product.Name} — {product.Calories} ккал\n" +
                          $"Б: {product.Proteins} Ж: {product.Fats} У: {product.Carbohydrates}",
                Tag = product.Id
            };
            comboBox.Items.Add(item);
        }

        canvas.Children.Add(comboBox);

        // Кнопка удаления
        var btnDelete = Fabric.CreateButton("Удалить продукт", 160, 40, 120, 100);
        btnDelete.Click += (s, e) =>
        {
            if (comboBox.SelectedItem is ComboBoxItem selected)
            {
                int id = (int)selected.Tag;
                DbCommands.DeleteProduct(id);
                MessageBox.Show("Продукт удалён.");
                MainPageCanvas.Children.Remove(canvas);
            }
            else
            {
                MessageBox.Show("Выберите продукт.");
            }
        };
        
        Border canvasBorder = Fabric.CreateBorder(Fabric.CreateThickness(3, 3, 3, 3), Brushes.Black, 250, 250);
        canvasBorder.Child = canvas;

        // Кнопка закрытия
        var btnClose = Fabric.CreateButton("Закрыть", 100, 30, 140, 160);
        btnClose.Click += (s, e) => MainPageCanvas.Children.Remove(canvasBorder);

        canvas.Children.Add(btnDelete);
        canvas.Children.Add(btnClose);
        
        MainPageCanvas.Children.Add(canvasBorder);
    }

    private string GetTextFromCanvas(Canvas canvas, string tag) 
    {
        return canvas.Children
            .OfType<TextBox>()
            .First(tb => (string)tb.Tag == tag)
            .Text;
    }


    private void ShowCreateCategoryFullCanvas()
    {
        createCategoryCanvas = Fabric.CreateCanvas(320, 560, 0, 0);
        createCategoryCanvas.Background = new SolidColorBrush(Colors.WhiteSmoke);
        Panel.SetZIndex(createCategoryCanvas, 999); // поверх всех

        int y = 30;

        // Название категории
        var lblCategoryName = Fabric.CreateTextBlock("Название категории:", 16, 20, y - 15, 10);
        var tbCategoryName = Fabric.CreateTextBox(2, 16, 300, 30, y + 30, 10);
        createCategoryCanvas.Children.Add(lblCategoryName);
        createCategoryCanvas.Children.Add(tbCategoryName);

        y += 80;

        // Выбор цвета из существующих
        var lblColor = Fabric.CreateTextBlock("Цвет категории:", 16, 20, y + 20, 10);
        var cbColors = new ComboBox { Width = 300, Height = 30, Margin = new Thickness(10, y + 40, 0, 0) };
        var colors = DbCommands.GetAllCategoryColors();
        foreach (var color in colors)
        {
            var item = new ComboBoxItem
            {
                Content = color.Name,
                Tag = color.Id
            };
            cbColors.Items.Add(item);
        }

        createCategoryCanvas.Children.Add(lblColor);
        createCategoryCanvas.Children.Add(cbColors);

        y += 100;

        // Кнопка "Создать цвет"
        var btnCreateColor = Fabric.CreateButton("Создать цвет", 160, 30, 10, y);
        btnCreateColor.Click += (s, e) =>
        {
            ShowCreateColorCanvas(); // форма создания нового цвета
        };
        createCategoryCanvas.Children.Add(btnCreateColor);

        // Кнопка "Создать категорию"
        var btnCreate = Fabric.CreateButton("Создать категорию", 180, 40, 200, y);
        btnCreate.Click += (s, e) =>
        {
            try
            {
                string catName = tbCategoryName.Text;
                if (cbColors.SelectedItem is ComboBoxItem selected)
                {
                    int colorId = (int)selected.Tag;
                    DbCommands.AddCategory(catName, colorId);
                    MessageBox.Show("Категория создана.");
                    MainPageCanvas.Children.Remove(createCategoryCanvas);
                }
                else
                {
                    MessageBox.Show("Выберите цвет.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        };
        createCategoryCanvas.Children.Add(btnCreate);

        borderCreateCategoryCanvas = Fabric.CreateBorder(Fabric.CreateThickness(2, 2, 2, 2), Brushes.Black, 200, 250);
        borderCreateCategoryCanvas.Child = createCategoryCanvas;
        
        // Кнопка закрытия
        var btnClose = Fabric.CreateButton("Закрыть", 100, 30, 400, y);
        btnClose.Click += (s, e) => MainPageCanvas.Children.Remove(borderCreateCategoryCanvas);
        createCategoryCanvas.Children.Add(btnClose);
        
        MainPageCanvas.Children.Add(borderCreateCategoryCanvas);
    }
    
    private Border borderCreateCategoryCanvas = null;
    private Canvas createCategoryCanvas = null;
    private void ShowCreateColorCanvas()
    {
        var canvas = Fabric.CreateCanvas(400, 300, 200, 150);
        canvas.Background = new SolidColorBrush(Colors.WhiteSmoke);
        Panel.SetZIndex(canvas, 999);

        var tbColorName = Fabric.CreateTextBox(2, 16, 300, 30, 20, 10);
        var tbR = Fabric.CreateTextBox(1, 16, 80, 30, 60, 10); 
        var tbG = Fabric.CreateTextBox(1, 16, 80, 30, 60, 100); 
        var tbB = Fabric.CreateTextBox(1, 16, 80, 30, 60, 190);

        var btnCreate = Fabric.CreateButton("Создать цвет", 160, 30, 10, 110);
        btnCreate.Click += (s, e) =>
        {
            try
            {
                string name = tbColorName.Text;
                byte r = byte.Parse(tbR.Text);
                byte g = byte.Parse(tbG.Text);
                byte b = byte.Parse(tbB.Text);

                DbCommands.AddCategoryColor(name, r, g, b);
                MessageBox.Show("Цвет создан!");
                MainPageCanvas.Children.Remove(canvas);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        };

        var btnClose = Fabric.CreateButton("Закрыть", 100, 30, 200, 110);
        btnClose.Click += (s, e) => createCategoryCanvas.Children.Clear();

        canvas.Children.Add(tbColorName);
        canvas.Children.Add(tbR);
        canvas.Children.Add(tbG);
        canvas.Children.Add(tbB);
        canvas.Children.Add(btnCreate);
        canvas.Children.Add(btnClose);

        createCategoryCanvas.Children.Add(canvas);
    }

    }
}