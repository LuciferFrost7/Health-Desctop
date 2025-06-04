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

namespace HealthDesctop;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        
        this.WindowState = WindowState.Maximized; // Устанавливаем полноэкранный режим
        
        Loaded += MainWindow_Loaded; // Подключаем событие при загрузки десктопа
    }

    private void MainWindow_Loaded(object sender, RoutedEventArgs e)
    {
        
    }
}