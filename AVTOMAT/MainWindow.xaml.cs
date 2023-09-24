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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        public readonly int rows = 15, cols = 15;
        public readonly Image[,] gridImages;
        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetupGrid();

        }

        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[rows, cols];
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;
            
            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Image image = new Image
                    {
                        Source = Images.Empty
                    };
                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                }    
            }

            return images;
        }
    }
}