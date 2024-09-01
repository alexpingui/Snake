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
using System.Windows.Threading;

namespace Snake
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const int snakeSquareSize = 30;

        private readonly SolidColorBrush snakeColor = Brushes.Green;

        private enum Direcrion
        {
            Left, Right, Top, Bottom
        }

        private Direcrion direcrion = Direcrion.Right;

        private const int timerInterval = 100;

        private DispatcherTimer timer;

        private Rectangle snakeHead;
        private Point foodPosition;

        private readonly static Random randomFoodPosition = new Random();

        private List<Rectangle> snake = new List<Rectangle>();

        private int score = 0;
        public MainWindow()
        {
            InitializeComponent(); 
        }

        private void GameCanvasLoaded(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            snakeHead = CreateSnakeSegment(new Point(5, 5));
            snake.Add(snakeHead);

            gameCanvas.Children.Add(snakeHead);

            PlaceFood();

            timer = new DispatcherTimer();
            timer.Tick += TimerTick;
            timer.Interval = TimeSpan.FromMilliseconds(timerInterval);
            timer.Start();
        }

        private void TimerTick(object sender, EventArgs e)
        {
            Point newHeadPosition = CalculateNewHeadPosition();

            if(newHeadPosition == foodPosition)
            {
                EatFood();
                PlaceFood();
            }

            if(newHeadPosition.X < 0 || newHeadPosition.Y < 0 
                || newHeadPosition.X >= gameCanvas.ActualWidth / snakeSquareSize
                || newHeadPosition.Y >= gameCanvas.ActualHeight / snakeSquareSize)
            {
                EndGame();
                return;
            }

            if (snake.Count >= 4)
            {
                for(int i = 0; i < snake.Count; i++)
                {
                    Point currentPos = new Point(Canvas.GetLeft(snake[i]), Canvas.GetTop(snake[i]));

                    for (int j = i + 1; j < snake.Count; j++)
                    {
                        Point nextCurrentPos = new Point(Canvas.GetLeft(snake[j]), Canvas.GetTop(snake[j]));
                        if (currentPos == nextCurrentPos)
                        {
                            EndGame();
                        }
                    }
                }
            }

            for(int i = snake.Count - 1; i > 0; i--)
            {
                Canvas.SetLeft(snake[i], Canvas.GetLeft(snake[i - 1]));
                Canvas.SetTop(snake[i], Canvas.GetTop(snake[i - 1]));
            }

            Canvas.SetLeft(snakeHead, newHeadPosition.X * snakeSquareSize);
            Canvas.SetTop(snakeHead, newHeadPosition.Y * snakeSquareSize);
        }

        private void EndGame()
        {
            timer.Stop();
            restartBtn.Visibility = Visibility.Visible;
        }

        private void EatFood()
        {
            score++;

            Rectangle newSnake = CreateSnakeSegment(foodPosition);
            snake.Add(newSnake);
            gameCanvas.Children.Add(newSnake);

            gameCanvas.Children.Remove(gameCanvas.Children.OfType<Image>().FirstOrDefault());

            scoreTextBlock.Text = $"Score: {score}";
        }

        private Point CalculateNewHeadPosition()
        {
            double left = Canvas.GetLeft(snakeHead) / snakeSquareSize;
            double top = Canvas.GetTop(snakeHead) / snakeSquareSize;

            Point headCurrentPos = new Point(left, top);
            Point newHeadPosition = new Point();

            switch(direcrion)
            {
                case Direcrion.Left:
                    newHeadPosition = new Point(headCurrentPos.X - 1, headCurrentPos.Y); break;
                case Direcrion.Right:
                    newHeadPosition = new Point(headCurrentPos.X + 1, headCurrentPos.Y); break;
                case Direcrion.Top:
                    newHeadPosition = new Point(headCurrentPos.X, headCurrentPos.Y - 1); break;
                case Direcrion.Bottom:
                    newHeadPosition = new Point(headCurrentPos.X, headCurrentPos.Y + 1); break;
            }

            return newHeadPosition;
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

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch(e.Key)
            {
                case Key.Up:
                    if(direcrion != Direcrion.Bottom)
                    direcrion = Direcrion.Top; break;

                case Key.Down:
                if(direcrion != Direcrion.Top)
                direcrion = Direcrion.Bottom; break;

                case Key.Left:
                    if(direcrion != Direcrion.Right)
                        direcrion = Direcrion.Left; break;

                case Key.Right:
                if(direcrion!= Direcrion.Left)
                    direcrion= Direcrion.Right; break;
            }
        }

        private void restartBtn_Click(object sender, RoutedEventArgs e)
        {
            score = 0;
            scoreTextBlock.Text = $"Score: {score}";
            snake.Clear();
            gameCanvas.Children.Clear();
            restartBtn.Visibility = Visibility.Collapsed;

            StartGame();
        }

        private void startBtn_Click(object sender, RoutedEventArgs e)
        {
            StartGame();
            startBtn.Visibility = Visibility.Collapsed;

        }
    }
}
