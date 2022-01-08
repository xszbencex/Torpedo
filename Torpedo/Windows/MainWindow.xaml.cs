﻿using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Torpedo.GameElement;
using Torpedo.Settings;
using Vector = Torpedo.Model.Vector;

namespace Torpedo.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameSession _gameSession;

        private int _numberOfRounds;
        public int NumberOfRounds
        {
            get { return _numberOfRounds; }
            set
            {
                _numberOfRounds = value;
                numberOfRoundsTextBlock.Text = $"{_numberOfRounds}";
            }
        }

        private int _player1Hits;
        public int Player1Hits
        {
            get { return _player1Hits; }
            set
            {
                _player1Hits = value;
                player1HitsTextBlock.Text = $"{_player1Hits}";
            }
        }

        private int _player2Hits;
        public int Player2Hits
        {
            get { return _player2Hits; }
            set
            {
                _player2Hits = value;
                player2HitsTextBlock.Text = $"{_player2Hits}";
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            // DummyInitializeGame();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            var inputDialog = new NameInputDialog();
            inputDialog.ShowDialog();

            if (inputDialog.Player1 != null)
            {
                StartGame(inputDialog.Player1, inputDialog.Player2);
            }
        }

        private void Scores_Click(object sender, RoutedEventArgs e)
        {
            var inputDialog = new GameScoresDialog();
            inputDialog.ShowDialog();
        }

        private void StartGame(Player player1, Player player2)
        {
            ConstructGameSession(player1, player2);
            SetTextBlocks();
            ChangePage();
            RenderPlayerFields();
        }

        private void ConstructGameSession(Player player1, Player player2)
        {
            var random = new Random();
            var randomInt = random.Next(1);
            _gameSession = new GameSession(player1, player2, randomInt == 0 ? player1 : player2);
        }

        private void SetTextBlocks()
        {
            player1Name.Text = _gameSession.Player1.Name;
            player2Name.Text = _gameSession.Player2.Name;
            this.NumberOfRounds = 0;
            this.Player1Hits = 0;
            this.Player2Hits = 0;
        }

        private void ChangePage()
        {
            startingPageGrid.Visibility = startingPageGrid.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            gamePageGrid.Visibility = gamePageGrid.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        private void RenderPlayerFields()
        {
            RenderField(player1Canvas);
            RenderField(player2Canvas);
        }

        private void RenderField(Canvas canvas)
        {
            for (int i = 0; i < MainSettings.GridHeight; i++)
            {
                for (int j = 0; j < MainSettings.GridWidth; j++)
                {
                    var field = new Rectangle
                    {
                        Fill = Brushes.Transparent,
                        Stroke = Brushes.Black,
                        StrokeThickness = 0.5
                    };
                    var unitY = canvas.Width / MainSettings.GridWidth;
                    var unitX = canvas.Height / MainSettings.GridHeight;
                    field.Width = unitY;
                    field.Height = unitX;
                    Canvas.SetTop(field, unitY * i);
                    Canvas.SetLeft(field, unitX * j);
                    // field.MouseEnter += OnRectangleHover;
                    canvas.Children.Add(field);
                }
            }
        }

        private void OnCanvasClick(object sender, MouseButtonEventArgs e)
        {
            Canvas clickedCanvas = (Canvas)sender;
            Vector vectorOfClick = DetermineVectorByClick(clickedCanvas, e);
            if (_gameSession.IsPuttingDownPhase)
            {
                if (clickedCanvas.Equals(GetActualPlayerCanvas()))
                {
                    HandlePuttingDown(vectorOfClick);
                }
                else
                {
                    MessageBox.Show("Click on your field!");
                }
            }
            else
            {
                if (clickedCanvas.Equals(GetEnemyPlayerCanvas()))
                {
                    HandleShot(vectorOfClick);
                }
                else
                {
                    MessageBox.Show("Click on enemy field!");
                }
            }
        }

        private void HandlePuttingDown(Vector vectorOfClick)
        {
            if (_gameSession.ShipStartPoint == null)
            {
                _gameSession.ShipStartPoint = vectorOfClick;
                Rectangle field = GetRectangleFromVector(vectorOfClick, GetActualPlayerCanvas());
                field.Fill = Brushes.Aqua;
            }
            else
            {
                try
                {
                    _gameSession.ActualPlayerPutsDownShip((Vector)_gameSession.ShipStartPoint, vectorOfClick);
                    RenderState();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void HandleShot(Vector vectorOfClick)
        {
            _gameSession.ActualPlayerTakeAShot(vectorOfClick);
            RenderState();
        }

        private Rectangle GetRectangleFromVector(Vector vector, Canvas canvas)
        {
            int indexOfChild = GetCanvasChildIndexFromVector(vector);
            Rectangle rectangle = (Rectangle)canvas.Children[indexOfChild];
            return rectangle;
        }

        private int GetCanvasChildIndexFromVector(Vector vector)
        {
            return vector.Y * MainSettings.GridHeight + vector.X;
        }

        private Vector DetermineVectorByClick(Canvas canvas, MouseButtonEventArgs e)
        {
            var point = e.GetPosition(canvas);
            var x = Math.Floor(point.X / 30);
            var y = Math.Floor(point.Y / 30);
            return new Vector((int)x, (int)y);
        }

        private void RenderState()
        {
            if (_gameSession.IsPuttingDownPhase)
            {
                DrawActualPlayerShips();
            }
            else
            {
                DrawActualPlayerShips();
                DrawActualPlayerShots();
                DrawEnemyPlayerShots();
            }
        }

        private void DrawActualPlayerShips()
        {
            _gameSession.ActualPlayer.ShipsCoordinate.ForEach(shipPart =>
            {
                GetRectangleFromVector(shipPart.Coordinate.GetValueOrDefault(), GetActualPlayerCanvas()).Fill = Brushes.Aqua;
            });
        }

        private void DrawAIShips()
        {
            _gameSession.Player2.ShipsCoordinate.ForEach(shipPart =>
            {
                GetRectangleFromVector(shipPart.Coordinate.GetValueOrDefault(), GetEnemyPlayerCanvas()).Fill = Brushes.Aqua;
            });
        }

        private void DrawActualPlayerShots()
        {
            _gameSession.ActualPlayer.FiredShots.ForEach(shot =>
            {
                GetRectangleFromVector(shot.Coordinate, GetEnemyPlayerCanvas()).Fill = Brushes.DarkGray;
            });
        }

        private void DrawEnemyPlayerShots()
        {
            var enemy = _gameSession.ActualPlayer.Equals(_gameSession.Player1) ? _gameSession.Player2 : _gameSession.Player1;
            enemy.FiredShots.ForEach(shot =>
            {
                GetRectangleFromVector(shot.Coordinate, GetActualPlayerCanvas()).Fill = Brushes.DarkGray;
            });
        }

        /*private void OnRectangleHover(object sender, MouseEventArgs e)
        {
            if (_gameSession.ShipStartPoint != null)
            {
                var point = new Point(Canvas.GetLeft((UIElement)sender), Canvas.GetTop((UIElement)sender));
                var x = Math.Floor(point.X / 30);
                var y = Math.Floor(point.Y / 30);
                var vectorOfHover = new Vector((int)x, (int)y);
                if (vectorOfHover.Y == _gameSession.ShipStartPoint?.Y || vectorOfHover.X == _gameSession.ShipStartPoint?.X)
                {
                    GetRectangleFromVector(vectorOfHover).Fill = Brushes.Aqua;
                }
            }
        }*/

        private Canvas GetActualPlayerCanvas()
        {
            return _gameSession.ActualPlayer.Equals(_gameSession.Player1) ? player1Canvas : player2Canvas;
        }

        private Canvas GetEnemyPlayerCanvas()
        {
            return _gameSession.ActualPlayer.Equals(_gameSession.Player1) ? player2Canvas : player1Canvas;
        }

        private void WindowKeyUp(object sender, KeyEventArgs e)
        {
            var keyPressed = e.Key;
            // TODO Show enemy table (if AI)
        }

        private void DummyInitializeGame()
        {
            var player1 = new RealPlayer("Joci");
            _gameSession = new GameSession(player1, new RealPlayer("Bence"), player1);
            SetTextBlocks();
            RenderPlayerFields();
        }
    }
}