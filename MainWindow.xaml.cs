using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snake
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int snakeSquareSize = 30;

        private readonly SolidColorBrush snakeColor = Brushes.Green;
        private readonly SolidColorBrush foodColor = Brushes.Red;

        private Rectangle snakeHead;
        private Point foodPosition;

        private readonly static Random randomFoodPosition = new Random();
        public MainWindow()
        {
            InitializeComponent();
            gameCanvas.Loaded += GameCanvasLoaded;
        }

        private void GameCanvasLoaded(object sender, RoutedEventArgs e)
        {
            snakeHead = CreateSnakeSegment(new Point(5, 5));
            gameCanvas.Children.Add(snakeHead);

            PlaceFood();
        }

        private void PlaceFood()
        {
            int maxX = (int)(gameCanvas.ActualWidth / snakeSquareSize);
            int maxY = (int)(gameCanvas.ActualHeight / snakeSquareSize);

            int foodX = randomFoodPosition.Next(0, maxX);
            int foodY = randomFoodPosition.Next(0, maxY);

            foodPosition = new Point(foodX, foodY);

            Image foodImage = new Image
            {
                Width = snakeSquareSize, Height = snakeSquareSize,
                Source = new BitmapImage(new Uri("C:\\Users\\Asus\\OneDrive\\Рабочий стол\\KASU\\C#\\Проектики\\WPF\\Snake\\Images\\Food.png"))
            };

            Canvas.SetLeft(foodImage, foodX * snakeSquareSize);
            Canvas.SetTop(foodImage, foodY * snakeSquareSize);

            gameCanvas.Children.Add(foodImage);
        }

        private Rectangle CreateSnakeSegment(Point position)
        {
            Rectangle rectangle = new Rectangle
            {
                Width = snakeSquareSize,
                Height = snakeSquareSize,
                Fill = snakeColor
            };

            Canvas.SetLeft(rectangle, position.X *  snakeSquareSize);
            Canvas.SetTop(rectangle, position.Y * snakeSquareSize);

            return rectangle;   
        }
    }
}
