using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace HealthDesctop.source.Fabric
{
    // Класс использующий паттерн Фабрика, для создания разных элементов Интерфейса
    public class Fabric
    {
        // Фабричное создание канвасов
        public static Canvas CreateCanvas(Int32 height, Int32 width, Int32 top=0, Int32 left=0)
        {
            Canvas canvas = new Canvas();
            canvas.Width = width;
            canvas.Height = height;
            
            Canvas.SetLeft(canvas, left);
            Canvas.SetTop(canvas, top);
            
            return canvas;
        }

        
        // Фабричное создание кнопок
        public static Button CreateButton(String content, Int32 width, Int32 height, Int32 left=0, Int32 top=0)
        {
            Button button = new Button();
            button.Width = width;
            button.Height = height;
            button.Content = content;
            
            Canvas.SetLeft(button, left);
            Canvas.SetTop(button, top);
            
            return button;
        }
        
        
        // Фабричное создание изображений
        public static Image CreateImage(String source, Int32 width, Int32 height, Int32 left=0, Int32 top=0)
        {
            Image image = new Image();
            image.Width = width;
            image.Height = height;
            image.Source = new BitmapImage(new Uri(source, UriKind.RelativeOrAbsolute)); // Подключаем изображение по пути
            RenderOptions.SetBitmapScalingMode(image, BitmapScalingMode.HighQuality); // Выключаем сглаживание фото
            
            Canvas.SetLeft(image, left);
            Canvas.SetTop(image, top);
            
            return image;
        }

        
        // Фабричное создание обводки для границы
        public static Thickness CreateThickness(Int32 left, Int32 top, Int32 right, Int32 bottom)
        {
            return new Thickness(left, top, right, bottom);
        }
        // Фабричное создание границы
        public static Border CreateBorder(Thickness thickness, Brush borderColor, Int32 left=0, Int32 top=0)
        {
            Border border = new Border();
            border.BorderBrush = borderColor;
            border.BorderThickness = thickness;
            
            Canvas.SetLeft(border, left);
            Canvas.SetTop(border, top);
            
            return border;
        }
        
        
        // Фабричное создание Текстового Поля
        public static TextBox CreateTextBox(Int32 thickness, Int32 fontSize, Int32 width, Int32 height, Int32 top=0, Int32 left=0)
        {
            TextBox textBox = new TextBox()
            {
                Width = width,
                Height = height,
                FontSize = fontSize,
                Margin = new Thickness(thickness),
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top
            };
            Canvas.SetLeft(textBox, left);
            Canvas.SetTop(textBox, top);

            return textBox;
        }
        
        
        //Фабричное создание Поля для ввода
        public static TextBlock CreateTextBlock(String content, Int32 fontSize, Int32 thickness, Int32 top, Int32 left)
        {
            TextBlock textBlock = new TextBlock
            {
                Text = content,
                FontSize = fontSize,
                Foreground = Brushes.Black,
                TextWrapping = TextWrapping.Wrap,
                TextAlignment = TextAlignment.Center,
                Margin = new Thickness(thickness)
            };
            
            Canvas.SetLeft(textBlock, left);
            Canvas.SetTop(textBlock, top);
            
            return textBlock;
        }
        
        
        // Фабричное создание Выпадающих списков
        public static ComboBox CreateComboBox(Int32 height,Int32 width, Int32 top, Int32 left)
        {
            ComboBox comboBox = new ComboBox();
            comboBox.Width = width;
            comboBox.Height = height;
            comboBox.Margin = new Thickness(10);
            
            Canvas.SetLeft(comboBox, left);
            Canvas.SetTop(comboBox, top);
            
            return comboBox;
        }
    }
}