using System;
using System.Collections.Generic;
using System.Linq;
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

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        int clickCount;
        Boolean gameOver;
        Tile[,] tiles;
        Grid grid;
        int[,] gridTemp;
        string dificulty;

        int flags;
        int numBombs;
        int bombsFound;
        int size;
        int bestNormal, bestEasy, bestHard;

        System.Windows.Threading.DispatcherTimer dispatcherTimer;
        int time;
        Boolean gameStarted;

        public MainWindow()
        {
            numBombs = 1;
            bombsFound = 0;
            clickCount = 0;
            gameOver = false;
            flags = 0;
            size = 8;
            gameStarted = false;
            gridTemp = new int[size, size];
            dificulty = "easy";

            bestNormal = 999999;
            bestEasy = 999999;
            bestHard = 999999;

            tiles = new Tile[size, size];

            dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
            dispatcherTimer.Tick += dispatcherTimer_Tick;
            dispatcherTimer.Interval = new TimeSpan(0, 0, 1);

            do
            {

                initGrid(ref gridTemp, ref tiles, size);

                addBombs(ref gridTemp, ref tiles, size, numBombs);

                //gridTemp = grida;

            } while (calculate3BV(ref gridTemp, size) > 200);

            //printGrid(ref grid);

            //Console.WriteLine("Size: " + numRows + "x" + numColumns + "\tNumber of Bombs: " + numBombs);


            // print out count
            //Console.WriteLine("Count = " + count);

            //Console.ReadKey();

            // start of working/ end of console program code

            InitializeComponent();

            showGrid();

        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            time++;
            gameTime.Text = time.ToString();
        }

        public void showGrid()
        {
            grid = new Grid();

            grid.Width = 500;
            grid.Height = 500;
            grid.HorizontalAlignment = HorizontalAlignment.Center;
            grid.VerticalAlignment = VerticalAlignment.Center;
            for (int i = 0; i < size; i++)
            {
                ColumnDefinition col = new ColumnDefinition();
                grid.ColumnDefinitions.Add(col);
            }
            for (int j = 0; j < size; j++)
            {
                RowDefinition row = new RowDefinition();
                grid.RowDefinitions.Add(row);
            }
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                {
                    //if (i == 4 && j == 4)
                    //    continue;
                    //Button button = new Button();
                    //tiles[i, j] = new Tile(i, j);

                    tiles[i, j].button.PreviewMouseLeftButtonUp += MouseUpHandler;
                    //this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);

                    tiles[i, j].button.MouseRightButtonDown += MouseRightButtonDown;



                    // black flag
                    //char blackFlag = '\u2691';

                    //char c = '\u2605';
                    //star symbol '\u2605'
                    tiles[i,j].textBlock.VerticalAlignment = VerticalAlignment.Center;
                    tiles[i, j].textBlock.FontWeight = FontWeights.UltraBold;

                    switch (tiles[i, j].getIdentifier())
                    {

                        case -1:
                            tiles[i, j].textBlock.Text = "X";
                            tiles[i, j].textBlock.Foreground = Brushes.Red;
                            break;
                        case 9:
                            //MessageBox.Show("9");
                            tiles[i, j].textBlock.Text = " ";
                            break;
                        default:
                            //button.Control.FontWeight ="Bold";
                            //tiles[i, j].button.Content = gridTemp[i,j];
                            tiles[i, j].textBlock.Text = tiles[i, j].getIdentifier().ToString();




                            switch (tiles[i, j].getIdentifier())
                            {
                                case 1:
                                    tiles[i, j].textBlock.Foreground = Brushes.Blue;
                                    break;
                                case 2:
                                    tiles[i, j].textBlock.Foreground = Brushes.Green;
                                    break;
                                case 3:
                                    tiles[i, j].textBlock.Foreground = Brushes.Red;
                                    break;
                                case 4:
                                    tiles[i, j].textBlock.Foreground = Brushes.DarkBlue;
                                    break;
                                case 5:
                                    tiles[i, j].textBlock.Foreground = Brushes.DarkRed;
                                    break;
                                case 6:
                                    tiles[i, j].textBlock.Foreground = Brushes.Cyan;
                                    break;
                                case 7:
                                    tiles[i, j].textBlock.Foreground = Brushes.Purple;
                                    break;
                                case 8:
                                    tiles[i, j].textBlock.Foreground = Brushes.Gray;
                                    break;
                            }

                            break;

                    }

                    //TextBlock textBlock = new TextBlock();
                    tiles[i, j].textBlock.TextAlignment = TextAlignment.Center;

                    Grid.SetColumn(tiles[i, j].button, i);
                    Grid.SetRow(tiles[i, j].button, j);
                    Grid.SetColumn(tiles[i, j].textBlock, i);
                    Grid.SetRow(tiles[i, j].textBlock, j);


                    grid.Children.Add(tiles[i, j].textBlock);
                    grid.Children.Add(tiles[i, j].button);
                }
            //myArea.Children.Clear();
            myArea.Children.Add(grid);
        }

        void MouseRightButtonDown(Object sender, RoutedEventArgs e)
        {
            Button temp = (Button)sender;
            char blackFlag = '\u2691';

            int row = (int)temp.GetValue(Grid.RowProperty);
            int column = (int)temp.GetValue(Grid.ColumnProperty);

            //MessageBox.Show("content of empty button: " + temp.Content);
            if (tiles[column, row].getFlag())
            {
                flags--;
                temp.Content = " ";
                tiles[column, row].toggleFlag();
                bombCount.Text = (numBombs - flags).ToString();

                if (tiles[column, row].isTileBomb())
                {
                    bombsFound--;
                }

            }
            else
            {
                flags++;
                temp.Content = blackFlag;
                tiles[column, row].toggleFlag();
                bombCount.Text = (numBombs - flags).ToString();
                if (tiles[column, row].isTileBomb())
                {
                    bombsFound++;
                }

                
            }

            if (bombsFound == numBombs)
            {
                for (int i = 0; i < size; i++)
                {
                    for (int z = 0; z < size; z++)
                    {
                        if (!tiles[i, z].getVisibility() && !tiles[i, z].getFlag())
                        {
                            //MessageBox.Show("There is still a tile that is not uncovered in cell: " + i + " ," + z);
                            return;
                        }
                    }
                }
                gameWon();
            }
        }
        
        void MouseUpHandler(Object sender, RoutedEventArgs e)
        {

            if (gameOver)
            {
                MessageBox.Show("Restart to play again.");
                return;
            }

            if (!gameStarted)
            {
                dispatcherTimer.Start();
                gameStarted = true;
            }

            Button temp = (Button)sender;

            int row = (int)temp.GetValue(Grid.RowProperty);
            int column = (int)temp.GetValue(Grid.ColumnProperty);

            //MessageBox.Show("The visibility of this tile is: " + tiles[column, row].getVisibility());

            //clicked tile is now visible
            tiles[column, row].setVisibility();

            //clickCount += 1;
            //count.Text = clickCount.ToString();

            // selected tile is blank
            if (tiles[column, row].getIdentifier() == 0)
            {
                // show all zero boxes
                revealTiles(temp, ref tiles, column, row, size);

            }
            else
            {
                // tile was not blank
                temp.Visibility = Visibility.Collapsed;

            }

            if (bombsFound == numBombs)
            {
                for (int i = 0; i < size; i++)
                {
                    for (int z = 0; z < size; z++)
                    {
                        if (!tiles[i, z].getVisibility() && !tiles[i, z].getFlag())
                        {
                            //MessageBox.Show("There is still a tile that is not uncovered in cell: " + i + " ," + z);
                            return;
                        }
                    }
                }
                gameWon();
            }

            // selected tile is bomb
            if (tiles[column, row].getIdentifier() == -1)
            {
                // game over
                dispatcherTimer.Stop();

                MessageBox.Show("GAME OVER");
                
                gameOver = true;

            }

        }

        public class Tile {

            public Button button;
            private int row, column, identifier;
            public TextBlock textBlock;
            Boolean flagged;
            Boolean isBomb;
            Boolean visible;


            public Tile(int row, int column)
            {
                button = new Button();
                textBlock = new TextBlock();
                isBomb = false;
                visible = false;

                this.row = row;
                this.column = column;
                flagged = false;
            }

            public int getIdentifier()
            {
                return identifier;
            }

            public void setIdentifier(int identifier)
            {
                this.identifier = identifier;
            }

            public int getRow()
            {
                return row;
            }

            public int getColumn()
            {
                return column;
            }

            public void incrementIdentifier()
            {
                identifier++;
            }

            public void toggleFlag()
            {
                if (flagged)
                {
                    flagged = false;
                }
                else
                {
                    flagged = true;
                }
            }

            public Boolean getFlag()
            {
                return flagged;
            }

            public Boolean isTileBomb()
            {
                return isBomb;
            }

            public void setBomb()
            {
                isBomb = true;
            }

            public void setVisibility()
            {
                visible = true;
            }

            public Boolean getVisibility()
            {
                return visible;
            }

        }



        public static int calculate3BV(ref int[,] gridTemp, int size)
        {
            int count = 0;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (gridTemp[i, j] == 0)
                    {
                        count++;

                        recursiveFunction(ref gridTemp, i, j, size);
                    }
                    else if (gridTemp[i, j] != -1 && gridTemp[i, j] != 9)
                    {
                        count++;
                    }
                }
            }

            return count;

        }

        public static void initGrid(ref int[,] grid, ref Tile[,] tiles, int size)
        {
            // fill array with zeros
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    grid[i, j] = 0;
                    tiles[i, j] = new Tile(i, j);
                    tiles[i, j].setIdentifier(0);
                }
            }
        }

        public static void printGrid(ref int[,] grid, int size)
        {
            // print out grid
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (grid[i, j] == -1)
                    {
                        Console.Write("B ");
                    }
                    else
                    {
                        Console.Write(grid[i, j] + " ");
                    }


                }

                Console.WriteLine();

            }
        }

        public static void addBombs(ref int[,] grid, ref Tile[,] tiles, int size, int numBombs)
        {
            Random rnd = new Random();
            int x, y;
            Boolean success;
            // add bombs
            for (int i = 0; i < numBombs; i++)
            {
                success = false;

                do
                {
                    // generate x and y
                    x = rnd.Next(0, size);     //ROW
                    y = rnd.Next(0, size);     //COLUMN

                    if (tiles[x, y].getIdentifier() == 0)
                    {
                        grid[x, y] = -1;
                        tiles[x, y].setIdentifier(-1);
                        tiles[x, y].setBomb();

                        // add numbers to surrounding tiles

                        //top left
                        if (x - 1 >= 0 && y - 1 >= 0)
                        {

                            if (tiles[x - 1, y - 1].getIdentifier() != -1)
                            {
                                tiles[x - 1, y - 1].incrementIdentifier();
                                // Console.WriteLine(grid[0, 0] + "!!!!!!!!!!!!!!!!!");

                            }
                        }

                        //bottom
                        if (x + 1 < size)
                        {

                            if (tiles[x + 1, y].getIdentifier() != -1)
                            {
                                tiles[x + 1, y].incrementIdentifier();

                            }
                        }

                        //top right
                        if (x - 1 >= 0 && y + 1 < size)
                        {

                            if (tiles[x - 1, y + 1].getIdentifier() != -1)
                            {
                                tiles[x - 1, y + 1].incrementIdentifier();

                            }
                        }

                        //right
                        if (y + 1 < size)
                        {

                            if (tiles[x, y + 1].getIdentifier() != -1)
                            {
                                tiles[x, y + 1].incrementIdentifier();

                            }
                        }

                        //bottom right
                        if (x + 1 < size && y + 1 < size)
                        {

                            if (tiles[x + 1, y + 1].getIdentifier() != -1)
                            {
                                tiles[x + 1, y + 1].incrementIdentifier();

                            }
                        }

                        //top
                        if (x - 1 >= 0)
                        {

                            if (tiles[x - 1, y].getIdentifier() != -1)
                            {
                                tiles[x - 1, y].incrementIdentifier();

                            }
                        }

                        //bottom left
                        if (x + 1 < size && y - 1 >= 0)
                        {

                            if (tiles[x + 1, y - 1].getIdentifier() != -1)
                            {
                                tiles[x + 1, y - 1].incrementIdentifier();

                            }
                        }

                        //left
                        if (y - 1 >= 0)
                        {

                            if (tiles[x, y - 1].getIdentifier() != -1)
                            {
                                tiles[x, y - 1].incrementIdentifier();

                            }
                        }


                        success = true;
                    }

                }
                while (!success);

                //check if the grid already has a bomb there

            }
        }

        public static void recursiveFunction(ref int[,] gridTemp, int i, int j, int size)
        {
            //Console.Write("recursive function working");
            if (gridTemp[i, j] == 0)
            {
                gridTemp[i, j] = 9;

                //check if squares around are also zero

                //top
                if (i - 1 >= 0)
                {

                    if (gridTemp[i - 1, j] == 0)
                    {
                        recursiveFunction(ref gridTemp, i - 1, j, size);

                    }
                }

                //bottom
                if (i + 1 < size)
                {

                    if (gridTemp[i + 1, j] == 0)
                    {
                        recursiveFunction(ref gridTemp, i + 1, j, size);

                    }
                }

                //left
                if (j - 1 >= 0)
                {

                    if (gridTemp[i, j - 1] == 0)
                    {
                        recursiveFunction(ref gridTemp, i, j - 1, size);

                    }
                }

                //right
                if (j + 1 < size)
                {

                    if (gridTemp[i, j + 1] == 0)
                    {
                        recursiveFunction(ref gridTemp, i, j + 1, size);

                    }
                }

            }

        }

        public static void revealTiles(Button tile, ref Tile[,] tiles, int i, int j, int size)
        {

            //Console.Write("recursive function working");
            if (tiles[i, j].getIdentifier() == 0)
            {
                tiles[i, j].setIdentifier(9);
                tiles[i, j].setVisibility();
                tile.Visibility = Visibility.Collapsed;
                //MessageBox.Show("tile at " + i + ", " + j + " is uncovered");

                //top
                if (i - 1 >= 0)
                {

                    if (tiles[i - 1, j].getIdentifier() == 0)
                    {
                        revealTiles(tiles[i - 1, j].button, ref tiles, i - 1, j, size);

                    }
                    else
                    {
                        tiles[i - 1, j].button.Visibility = Visibility.Collapsed;
                        tiles[i - 1, j].setVisibility();
                    }
                }

                //top left
                if (i - 1 >= 0 && j - 1 >= 0)
                {

                    if (tiles[i - 1, j - 1].getIdentifier() == 0)
                    {
                        revealTiles(tiles[i - 1, j - 1].button, ref tiles, i - 1, j - 1, size);

                    }
                    else
                    {
                        tiles[i - 1, j - 1].button.Visibility = Visibility.Collapsed;
                        tiles[i - 1, j - 1].setVisibility();
                    }
                }

                //top right
                if (i - 1 >= 0 && j + 1 < size)
                {

                    if (tiles[i - 1, j + 1].getIdentifier() == 0)
                    {
                        revealTiles(tiles[i - 1, j + 1].button, ref tiles, i - 1, j + 1, size);

                    }
                    else
                    {
                        tiles[i - 1, j + 1].button.Visibility = Visibility.Collapsed;
                        tiles[i - 1, j + 1].setVisibility();
                    }
                }

                //bottom
                if (i + 1 < size)
                {

                    if (tiles[i + 1, j].getIdentifier() == 0)
                    {
                        revealTiles(tiles[i + 1, j].button, ref tiles, i + 1, j, size);

                    }
                    else
                    {
                        tiles[i + 1, j].button.Visibility = Visibility.Collapsed;
                        tiles[i + 1, j].setVisibility();
                    }
                }

                // bottom left
                if (i + 1 < size && j - 1 >= 0)
                {

                    if (tiles[i + 1, j - 1].getIdentifier() == 0)
                    {
                        revealTiles(tiles[i + 1, j - 1].button, ref tiles, i + 1, j - 1, size);

                    }
                    else
                    {
                        tiles[i + 1, j - 1].button.Visibility = Visibility.Collapsed;
                        tiles[i + 1, j - 1].setVisibility();
                    }
                }

                // bottom right
                if (i + 1 < size && j + 1 < size)
                {

                    if (tiles[i + 1, j + 1].getIdentifier() == 0)
                    {
                        revealTiles(tiles[i + 1, j + 1].button, ref tiles, i + 1, j + 1, size);

                    }
                    else
                    {
                        tiles[i + 1, j + 1].button.Visibility = Visibility.Collapsed;
                        tiles[i + 1, j + 1].setVisibility();
                    }
                }

                //left
                if (j - 1 >= 0)
                {

                    if (tiles[i, j - 1].getIdentifier() == 0)
                    {
                        revealTiles(tiles[i, j - 1].button, ref tiles, i, j - 1, size);

                    }
                    else
                    {
                        tiles[i, j - 1].button.Visibility = Visibility.Collapsed;
                        tiles[i, j - 1].setVisibility();
                    }

                }

                //right
                if (j + 1 < size)
                {

                    if (tiles[i, j + 1].getIdentifier() == 0)
                    {
                        revealTiles(tiles[i, j + 1].button, ref tiles, i, j + 1, size);

                    }
                    else
                    {
                        tiles[i, j + 1].button.Visibility = Visibility.Collapsed;
                        tiles[i, j + 1].setVisibility();
                    }
                }

            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            restartGame();
        }

        private void MenuItem_Easy(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Easy");

            dificulty = "easy";

            size = 8;
            numBombs = 5;

            gridTemp = new int[size, size];

            tiles = new Tile[size, size];

            restartGame();
        }
        private void MenuItem_Normal(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Normal");

            dificulty = "normal";

            size = 16;
            numBombs = 8;

            gridTemp = new int[size, size];

            tiles = new Tile[size, size];

            restartGame();
        }
        private void MenuItem_Hard(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show("Hard");

            dificulty = "hard";

            size = 20;
            numBombs = 10;

            gridTemp = new int[size, size];

            tiles = new Tile[size, size];

            restartGame();
        }

        private void hint(object sender, RoutedEventArgs e)
        {
            char star = '\u2605';

            for (int i = 0; i < size; i++)
            {
                for (int z = 0; z < size; z++)
                {
                    if (!tiles[i, z].getVisibility() && tiles[i, z].isTileBomb() && !tiles[i, z].getFlag())
                    {
                        tiles[i, z].button.Content = star;
                        hintButton.Visibility = Visibility.Collapsed;
                        return;
                    }
                }
            }

        }

        public void gameWon()
        {
            dispatcherTimer.Stop();
            MessageBox.Show("You Won! Your score was " + time);
            gameStarted = false;
            showTiles();

            if(dificulty.Equals("easy"))
            {
                if (time < bestEasy)
                    easy.Text = "Easy " + time.ToString();
            }else if(dificulty.Equals("normal"))
            {
                if (time < bestNormal)
                    normal.Text = "Normal " + time.ToString();
            }
            else if (dificulty.Equals("hard"))
            {
                if (time < bestHard)
                    hard.Text = "Hard " + time.ToString();
            }
        }

        public void showTiles()
        {
            for (int i = 0; i < size; i++)
                for (int z = 0; z < size; z++)
                    tiles[i, z].button.Visibility = Visibility.Collapsed;
        }

        public void restartGame()
        {
            grid.Children.Clear();
            gameOver = false;
            myArea.Children.Clear();

            hintButton.Visibility = Visibility.Visible;


            do
            {

                initGrid(ref gridTemp, ref tiles, size);

                addBombs(ref gridTemp, ref tiles, size, numBombs);

                //gridTemp = grida;

            } while (calculate3BV(ref gridTemp, size) > 200);

            showGrid();

            flags = 0;
            bombsFound = 0;

            bombCount.Text = (numBombs - flags).ToString();

            time = 0;
            dispatcherTimer.Stop();
            gameStarted = false;

            gameTime.Text = "0";
        }
    }
}
