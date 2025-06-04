using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace HealthDesctop.source.Animations
{

    public class Animations
    {
        // Анимации плавного Появления Панели регистрации / логина
        public static void FadeInRegOrLogMenu(Border border)
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double targetLeft = Canvas.GetLeft(border); // туда, куда хотим приехать

            // Сначала ставим за границу экрана (справа)
            Canvas.SetLeft(border, screenWidth);

            // Анимация Left от screenWidth до targetLeft
            var animation = new DoubleAnimation
            {
                From = screenWidth,
                To = targetLeft,
                Duration = TimeSpan.FromSeconds(0.450),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            // Применяем анимацию
            var storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, border);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Canvas.Left)"));

            storyboard.Begin();
        }
        
        // Анимация плавного Исчезновения Панели регистрации / логина
        public static void FadeOutRegOrLogMenu(Border border, Action onComplete)
        {
            double screenWidth = SystemParameters.PrimaryScreenWidth;
            double currentLeft = Canvas.GetLeft(border);

            var animation = new DoubleAnimation
            {
                From = currentLeft,
                To = screenWidth,
                Duration = TimeSpan.FromSeconds(0.450),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };

            animation.Completed += (s, e) => onComplete?.Invoke();

            var storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, border);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Canvas.Left)"));

            storyboard.Begin();
        }

        
        // Анимация появления панельки с кнопками Регистрации и Входа
        public static void ShowButtonsPanelFromLeft(Border border)
        {
            double targetLeft = Canvas.GetLeft(border);
            if (double.IsNaN(targetLeft)) targetLeft = 0; // Фикс для безопасного старта

            double panelWidth = border.ActualWidth > 0 ? border.ActualWidth : border.Width;

            // Сначала ставим панель за левый край
            Canvas.SetLeft(border, -panelWidth);

            var animation = new DoubleAnimation
            {
                From = -panelWidth,
                To = targetLeft,
                Duration = TimeSpan.FromSeconds(0.3),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseOut }
            };

            var storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, border);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Canvas.Left)"));
            storyboard.Begin();
        }
        // Анимация Исчезновения панельки с кнопками Регистрации и Входа
        public static void HideButtonsPanelToLeft(Border border, Action DeInitialization)
        {
            double currentLeft = Canvas.GetLeft(border);
            double panelWidth = border.ActualWidth > 0 ? border.ActualWidth : border.Width;
            double offscreenLeft = -panelWidth;

            var animation = new DoubleAnimation
            {
                From = currentLeft,
                To = offscreenLeft,
                Duration = TimeSpan.FromSeconds(2.2),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseIn }
            };

            animation.Completed += (s, e) => DeInitialization?.Invoke();

            var storyboard = new Storyboard();
            storyboard.Children.Add(animation);
            Storyboard.SetTarget(animation, border);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(Canvas.Left)"));
            storyboard.Begin();
        }
        
        private static Rectangle whiteOverlay = null;

        public static void FadeInWhite(Canvas canvas, Action onComplete = null)
        {
            double width = SystemParameters.PrimaryScreenWidth;
            double height = SystemParameters.PrimaryScreenHeight;

            var whiteOverlay = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = new SolidColorBrush(Colors.White),
                Opacity = 0
            };

            Panel.SetZIndex(whiteOverlay, 999);
            canvas.Children.Add(whiteOverlay);

            var fadeIn = new DoubleAnimation
            {
                From = 0,
                To = 1,
                Duration = TimeSpan.FromSeconds(1.2),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseIn }
            };

            fadeIn.Completed += (s, e) => onComplete?.Invoke();

            whiteOverlay.BeginAnimation(UIElement.OpacityProperty, fadeIn);
        }

        public static void FadeOutWhite(Canvas canvas, Action onComplete = null)
        {
            double width = SystemParameters.PrimaryScreenWidth;
            double height = SystemParameters.PrimaryScreenHeight;

            var whiteOverlay = new Rectangle
            {
                Width = width,
                Height = height,
                Fill = new SolidColorBrush(Colors.White),
                Opacity = 1
            };

            Panel.SetZIndex(whiteOverlay, 999);
            canvas.Children.Add(whiteOverlay);

            var fadeOut = new DoubleAnimation
            {
                From = 1,
                To = 0,
                Duration = TimeSpan.FromSeconds(2.0),
                EasingFunction = new QuadraticEase { EasingMode = EasingMode.EaseOut }
            };

            fadeOut.Completed += (s, e) =>
            {
                canvas.Children.Remove(whiteOverlay);
                onComplete?.Invoke();
            };

            whiteOverlay.BeginAnimation(UIElement.OpacityProperty, fadeOut);
        }

    }
}