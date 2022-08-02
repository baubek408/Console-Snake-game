using System;
using System.Collections.Generic;

namespace Snake_Console
{   class Pos
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
    class Program
    {
        static char[][] grid =  new char [20] [];   // 2 dimenzionální souřadnice x a y
        static int width = 50;
        static int height = 20;
        static bool gameover = false;
        static int snake_x = 25;   // počáteční pozice hada
        static int snake_y = 9;
        static List<Pos> snake = new List<Pos>(); // seznam pro tělo hada, který bude obsahovat znaky [o] a [O] (tělo a hlava)
        enum direction      // Volby pochybu
        {
                UP = 1,
                DOWN = 2,
                LEFT = 3,
                RIGHT = 4
        }
        static direction current = direction.RIGHT; 
        static int lengthtimer = 0;
        static int lengthtime = 5;
        static int snakelength = 5;
        static int target_x;
        static int target_y;
        static int score = 0;
        




        static void Main()   // hlavní funkce 
        {
            InitFrame();
            DrawFrame();
            InitSnake();
            SetTarget();
            InitScore();

            while (!gameover)  // Začátek hry s cyklem while;
            {
                DrawSnakeHead();
                if (TargetTaken())
                {
                    SetTarget();
                    UpdateScore();
                }

                Pause();
                ReadKeys();
                DrawSnakeBodyOnHeadPosition();
                MoveSnakeHead();
                if (isGameover())
                    gameover = true;
                IncreaseSnakeLength();
                DeleteSnakeTail();
            }
            DrawSnakeHead();
            Console.SetCursorPosition(0, 20);
            Console.WriteLine("Game Over");
        }
        static void InitFrame()   // Vytváření hranice zdí
        {
            Console.CursorVisible = false;

            for (int i = 0; i < height; i++)
                grid[i] = new char[width];

            grid[0][0] = '(';              //Rohy stěn
            grid[0][width-1] = ')';
            grid[height-1][0] = ')';
            grid[height - 1][width - 1] = '(';

            for (int i = 1; i < height - 1; i++)  // Hranice
            {
                grid[i][0] = '|';
                grid[i][width - 1] = '|';
            }
            for (int i = 1; i < width - 1; i++)
            {
                grid[0][i] = '-';
                grid[height-1][0] = '-';
            }
            for (int y = 1; y < height - 1; y++)
                for (int x = 1; x < width - 1; x++)
                    grid[y][x] = ' ';
        }

        static void DrawFrame()   // Přidání barvy 
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Clear();

            for (int y =0;y<height;y++)
                for(int x = 0; x < width; x++)
                {
                    Console.SetCursorPosition(x, y);
                    Console.Write(grid[y][x].ToString());
                }
        }

        static void InitSnake()       //  začíná tělo hada s délkou = 5;
        {
            snake.Add(new Pos() { X = 21, Y = 9 });
            snake.Add(new Pos() { X = 22, Y = 9 });
            snake.Add(new Pos() { X = 23, Y = 9 });
            snake.Add(new Pos() { X = 24, Y = 9 });
            snake.Add(new Pos() { X = 25, Y = 9 });
            foreach (Pos pos in snake)
                grid[pos.Y][pos.X ]= 'o'; // to nám pomůže zkontrolovat, zda had zasáhl sám sobe;
        }

        static void DrawSnake()
        {
            int count = 0;

            Console.ForegroundColor = ConsoleColor.Yellow;

            foreach (Pos snakepart in snake)
            {
                Console.SetCursorPosition(snakepart.X, snakepart.Y);   // Kreslení hada v naší pole
                count++;
                if (count < 5)
                    Console.Write('o');
                else
                    Console.Write('O');   // Hlava hada;
            }

        }

        static void MoveSnakeHead()   // Změnou aktuální pozice prvků ve Listu hada, pohybujeme tělem hada
        {
            grid[snake_y][snake_x] = 'o';

            switch (current)
            {
                case direction.UP:
                    snake_y--;
                    break;
                case direction.DOWN:
                    snake_y++;
                    break;
                case direction.LEFT:
                    snake_x--;
                    break;
                case direction.RIGHT:
                    snake_x++;
                    break;
            }

            snake.Add(new Pos() { X = snake_x, Y = snake_y });
        }

        static void DrawSnakeBodyOnHeadPosition()  //Telo hada
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(snake_x, snake_y);
            Console.Write('o');

        }

        static void DrawSnakeHead()         //Hlava hada
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(snake_x, snake_y);
            Console.Write('O');
        }

        static void DeleteSnakeTail()   // Odstranění ocasu. Přidání prázdného znaku na konec pozice
        {
            Console.SetCursorPosition(snake[0].X, snake[0].Y);
            Console.Write(' ');
            if (snake.Count != snakelength)
            {
                grid[snake[0].Y][snake[0].X] = ' ';
                snake.RemoveAt(0);
            }

        }

        static void Pause()
        {
            System.Threading.Thread.Sleep(100);
        }

        static void ReadKeys()    //Čtení z klávesnice
        {
            ConsoleKeyInfo s;
            if (Console.KeyAvailable)
            {
                s = Console.ReadKey();

                switch (s.Key)
                {
                    case ConsoleKey.UpArrow:
                        if (current != direction.DOWN)  // Zde zjišťujeme, zda je možné se přesunout do aktuálního směru. Například není možné se pohybovat dolů, pokud had jde nahoru.
                        {
                            current = direction.UP;   
                        }
                        break;
                    case ConsoleKey.DownArrow:
                        if (current != direction.UP)
                        {
                            current = direction.DOWN;
                        }
                        break;
                    case ConsoleKey.LeftArrow:
                        if (current != direction.RIGHT)
                        {
                            current = direction.LEFT;
                        }
                        break;
                    case ConsoleKey.RightArrow:
                        if (current != direction.LEFT)
                        {
                            current = direction.RIGHT;
                        }
                        break;
                }
            }

        }

        static void IncreaseSnakeLength()   // Přidání velikosti (char) pro aktuální velikost hada
        {
            lengthtimer++;

            if(lengthtimer == lengthtime)
            {
                lengthtimer = 0;
                snakelength++;
            }
        }

        static bool isGameover()   // Pokud narazíme na zeď nebo na sebe, hra končí;
        {
            bool value = false;

            if (grid[snake_y][snake_x] != ' ')
                value = true;
            return value;
        }

        static bool TargetTaken()   // Kontrola, zda had sežral cíl. Pokud se pozice hlavy hada a cíle rovnají {x1=x2, y1=y2}, je True
        {
            return snake_x == target_x && snake_y == target_y; 
        }

        static void SetTarget()  // Náhodné přidání cíle;
        {
            Random rnd = new Random();
            int x = 0;
            int y = 0;

            while(grid[y][x] != ' ')
            {
                x = rnd.Next(1, width - 1);
                y = rnd.Next(1, height - 1);
            }

            target_x = x;
            target_y = y;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(x, y);
            Console.Write('X');

        }

        static void InitScore()         // zobrazení počátečního skóre
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.SetCursorPosition(51, 0);
            Console.Write("Score : 0 ");
        }

        static void UpdateScore()  // Aktualizace skóre    
        {
            score++;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.SetCursorPosition(59, 0);
            Console.Write(score + " ");
        }

    }
    
}
