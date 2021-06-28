using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    class Game
    {
        private readonly List<Pixel> _snake;
        private Pixel _food;
        private bool _isGameEnable; 
        private Keys _direction;
        private int _points; 

        private readonly Label _label;
        private readonly PictureBox _canvas;
        private readonly Timer _timer;

        private readonly int _maxXPos;
        private readonly int _maxYPos;

        public Game(Label label, PictureBox canvas, Timer timer)
        {
            _snake = new List<Pixel>();
            _isGameEnable = true;
            _direction = Keys.None;
            _points = 0;

            _label = label;
            _canvas = canvas;
            _timer = timer;

            _maxXPos = _canvas.Size.Width / 15;
            _maxYPos = _canvas.Size.Height / 15;

            timer.Interval = 70;
            timer.Start();

            StartGame();
        }

        public void StartGame()
        {
            Pixel pixel = new Pixel() { X = 20, Y = 20 };
            _snake.Add(pixel);

            SpawnFood();
        }

        private void SpawnFood()
        {
            var random = new Random();

            Pixel pixel = new Pixel() { X = random.Next(0, _maxXPos), Y = random.Next(0, _maxYPos) };
            _food = pixel;
        }

        public void KeyInput(Keys key)
        {
            if (key is Keys.Up && _direction != Keys.Down || key is Keys.Down && _direction != Keys.Up || key is Keys.Right && _direction != Keys.Left || key is Keys.Left && _direction != Keys.Right)
                _direction = key;
        }

        public void TimeTick()
        {
            if (_isGameEnable) 
                MovePlayer();

            _canvas.Invalidate();
        }

        private void MovePlayer()
        {
            for (int i = _snake.Count - 1; i >= 0; i--)
            {
                if (i is 0)
                {
                    switch (_direction)
                    {
                        case Keys.Up:
                            _snake[i].Y--;
                            break;
                        case Keys.Down:
                            _snake[i].Y++;
                            break;
                        case Keys.Right:
                            _snake[i].X++;
                            break;
                        case Keys.Left:
                            _snake[i].X--;
                            break;
                    }

                    //Collision with borders
                    if (_snake[i].X >= _maxXPos || _snake[i].X <= 0 || _snake[i].Y >= _maxYPos || _snake[i].Y <= 0)
                    {
                        if (_snake[i].X <= -1)
                            _snake[i].X = _maxXPos;
                        else if (_snake[i].X >= _maxXPos + 2)
                            _snake[i].X = 0;
                        else if (_snake[i].Y <= -1)
                            _snake[i].Y = _maxYPos;
                        else if (_snake[i].Y >= _maxYPos + 2)
                            _snake[i].Y = 0;
                    }

                    //Collision with body
                    for (int j = 1; j < _snake.Count; j++)
                    {
                        if (_snake[i].X == _snake[j].X && _snake[i].Y == _snake[j].Y)
                            EndGame();
                    }

                    //Collision with food
                    if (_food.X == _snake[i].X && _food.Y == _snake[i].Y)
                        EatFood();
                }
                else
                {
                    _snake[i].X = _snake[i - 1].X;
                    _snake[i].Y = _snake[i - 1].Y;
                }
            }
        }

        public void PaintCanvas(PaintEventArgs e)
        {
            if (_isGameEnable)
            {
                Brush snakeColor;
                var graphics = e.Graphics;
                for (int i = 0; i < _snake.Count; i++)
                {
                    snakeColor = (i is 0) ? Brushes.Black : Brushes.White;

                    int multiplicationValue = 15;

                    graphics.FillRectangle(snakeColor, new Rectangle(_snake[i].X * multiplicationValue, _snake[i].Y * multiplicationValue, multiplicationValue, multiplicationValue));
                    graphics.FillEllipse(Brushes.Red, new Rectangle(_food.X * multiplicationValue, _food.Y * multiplicationValue, multiplicationValue, multiplicationValue));
                }
            }
        }

        private void EatFood()
        {
            _snake.Add(_food);
            IncreaseScore();
            SpawnFood();
        }

        private void IncreaseScore()
        {
            _points++;
            _label.Text = $"Points: {_points}";
        }

        private void EndGame()
        {
            _isGameEnable = !_isGameEnable;
            MessageBox.Show($"The game end!\nYou scored {_points} points!");
            RestartGame();
        }

        private void RestartGame()
        {
            _snake.Clear();
            _points = 0;
            _direction = Keys.None;
            _isGameEnable = !_isGameEnable;

            StartGame();
        }
    }
}
