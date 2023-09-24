using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;


namespace AVTOMAT
{
    /// <summary>
    /// я сегодня признался девушке в любви она ответила взаимностью, её зовут Лена, она нравилась мне 2 года,
    ///и я пригласил её погулять, пошли в шаурмичную поели, она очень много есть и не толстеет,пошли
    /// к ней домой её родителей не было ну и вот так вот я отдал ей свой первый поцелуй а
    /// потом обнимались смотрели фильм Анчартед классный фильмец, а дальше я лишился девственности
    /// захотели покушать я приготовил очень вкусную лапшу которую вешаю тебе на уши
    /// </summary>
    public partial class MainWindow : Window
    {
        public readonly Dictionary<GridValue, ImageSource> GridValToImage = new()
        {
            { GridValue.Empty, Images.Empty },
            { GridValue.SnakeA, Images.Body },
            { GridValue.Food, Images.Food }
        };

        public readonly Dictionary<Direction, int> DirToRotation = new()
        {
            {Direction.Up, 0},
            {Direction.Down, 180},
            {Direction.Left, 270},
            {Direction.Right, 90}
        };
        
        public readonly int Rows = 25, Cols = 25;
        public readonly Image[,] GridImages;
        private GameState _gameState;
        private bool _gameRunning;
        
        
        public MainWindow()
        {
            InitializeComponent();
            GridImages = SetupGrid();
            _gameState = new GameState(Rows, Cols);

        }
        
        private async Task RunGame()
        {
            await ShowCountDown();
            Draw();
            OverlayText.Visibility = Visibility.Hidden;
            await GameLoop();
            await ShowGameOver();
            _gameState = new GameState(Rows, Cols);
        }
        private async void MainWindow_OnPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (OverlayText.Visibility == Visibility.Visible)
            {
                e.Handled = true;
            }

            if (!_gameRunning)
            {
                _gameRunning = true;
                await RunGame();
                _gameRunning = false;
            }
        }
        
        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (_gameState.GameOver)
            {
                return;
            }

            switch (e.Key)
            {
                case Key.Left:
                    _gameState.ChangeDirection(Direction.Left);
                    break;
                case Key.Right:
                    _gameState.ChangeDirection(Direction.Right);
                    break;
                case Key.Up:
                    _gameState.ChangeDirection(Direction.Up);
                    break;
                case Key.Down:
                    _gameState.ChangeDirection(Direction.Down);
                    break;
            }
        }
        
        private async Task GameLoop()
        {
            while (!_gameState.GameOver)
            {
                await Task.Delay(100);
                _gameState.Move();
                Draw();
            }
        }
        

        
        
        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[Rows, Cols];
            GameGrid.Rows = Rows;
            GameGrid.Columns = Cols;
            
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    Image image = new Image
                    {
                        Source = Images.Empty,
                        RenderTransformOrigin = new Point(0.5, 0.5)
                    };
                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                }    
            }
            
            return images;
        }

        private void Draw()
        {
            DrawGrid();
            DrawSnakeAHead();
            ScoreText.Text = $"SCORE {_gameState.Score}";
        }
        
        
        private void DrawGrid()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    GridValue gridVal = _gameState.Grid[r, c];
                    GridImages[r, c].Source = GridValToImage[gridVal];
                    GridImages[r,c].RenderTransform = Transform.Identity;
                }
            }
        }

        private void DrawSnakeAHead()
        {
            Position headPos = _gameState.HeadPosition();
            Image image = GridImages[headPos.Row, headPos.Col];
            image.Source = Images.Head;

            int rotation = DirToRotation[_gameState.Dir];
            image.RenderTransform = new RotateTransform(rotation);
        }

        private async Task DrawDeadSnakeA()
        {
            List<Position> positions = new List<Position>(_gameState.SnakeAPositions());


            for (int i = 0; i < positions.Count; i++)
            {
                Position pos = positions[i];
                ImageSource source = (i == 0) ? Images.DeadHead : Images.DeadBody;
                GridImages[pos.Row, pos.Col].Source = source;
                await Task.Delay(100);
                
            }
        }
        
        
        
        private async Task ShowCountDown()
        {
            for (int i = 5; i >= 1; i--)
            {
                OverlayTextBlock.Text = i.ToString();
                await Task.Delay(1000);
            }
        }

        private async Task ShowGameOver()
        {
            await DrawDeadSnakeA();
            await Task.Delay(100);
            OverlayText.Visibility = Visibility.Visible;
            OverlayTextBlock.Text = "НАЖМИТЕ ЛЮБУЮ КНОПКУ";
        }
        
    }
}