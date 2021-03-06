using System;
using System.Collections.Generic;
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

        private bool _isAIShipsVisible = false;

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

        private string _actualPlayerName;
        public string ActualPlayerName
        {
            get { return _actualPlayerName; }
            set
            {
                _actualPlayerName = value;
                actualPlayerText.Text = _actualPlayerName;
            }
        }

        private string _actualPhase;
        public string ActualPhase
        {
            get { return _actualPhase; }
            set
            {
                _actualPhase = value;
                actualPhaseText.Text = _actualPhase;
            }
        }

        private int _nextShipSize;
        public int NextShipSize
        {
            get { return _nextShipSize; }
            set
            {
                _nextShipSize = value;
                nextShipSize.Text = $"{_nextShipSize}";
            }
        }

        private string _shipsDestroyed;
        public string ShipsDestroyed
        {
            get { return _shipsDestroyed; }
            set
            {
                _shipsDestroyed = value;
                shipsDestroyed.Text = _shipsDestroyed;
            }
        }

        private string _shipsRemains;
        public string ShipsRemains
        {
            get { return _shipsRemains; }
            set
            {
                _shipsRemains = value;
                shipsRemains.Text = _shipsRemains;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
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
            SetTextBlocks();
        }

        private void WindowKeyUp(object sender, KeyEventArgs e)
        {
            var keyPressed = e.Key;
            if (keyPressed == Key.H && _gameSession.Player2 is AIPlayer && !_gameSession.IsPuttingDownPhase)
            {
                if (_isAIShipsVisible)
                {
                    RenderState();
                    _isAIShipsVisible = false;
                }
                else
                {
                    DrawAIShips();
                    _isAIShipsVisible = true;
                }
            }
        }

        private void HandlePuttingDown(Vector vectorOfClick)
        {
            if (_gameSession.ShipStartPoint == null)
            {
                _gameSession.ShipStartPoint = vectorOfClick;
                Rectangle field = GetRectangleFromVector(vectorOfClick, GetActualPlayerCanvas());
                field.Fill = MainSettings.ShipColor;
                SetHoverListeners(GetActualPlayerCanvas());
            }
            else
            {
                RemoveHoverListeners(GetActualPlayerCanvas());
                try
                {
                    _gameSession.ActualPlayerPutsDownShip((Vector)_gameSession.ShipStartPoint, vectorOfClick);
                    RenderState();
                    _gameSession.ShipStartPoint = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    RenderState();
                    _gameSession.ShipStartPoint = null;
                }
            }
        }

        private void HandleShot(Vector vectorOfClick)
        {
            try
            {
                _gameSession.ActualPlayerTakeAShot(vectorOfClick);
                RenderState();
                _isAIShipsVisible = false;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message);
                RenderState();
                _isAIShipsVisible = false;
            }
            catch (GameOverExeption ex)
            {
                MessageBox.Show(ex.Message);
                EndGame();
            }
        }

        private void RenderState()
        {
            ClearFields(GetActualPlayerCanvas());
            ClearFields(GetEnemyPlayerCanvas());
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

        private void ConstructGameSession(Player player1, Player player2)
        {
            _gameSession = new GameSession(player1, player2);
        }

        private void ChangePage()
        {
            startingPageContainer.Visibility = startingPageContainer.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
            gamePageContainer.Visibility = gamePageContainer.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;
        }

        private void RenderPlayerFields()
        {
            RenderField(player1Canvas);
            RenderField(player2Canvas);
        }

        private void EndGame()
        {
            player1Canvas.Children.Clear();
            player2Canvas.Children.Clear();
            ChangePage();
        }

        private void RenderField(Canvas canvas)
        {
            for (int i = 0; i < MainSettings.GridHeight; i++)
            {
                for (int j = 0; j < MainSettings.GridWidth; j++)
                {
                    var field = new Rectangle
                    {
                        Fill = MainSettings.DefaultFieldColor,
                        Stroke = MainSettings.DefaultFieldStrokeColor,
                        StrokeThickness = 0.5
                    };
                    var unitY = canvas.Width / MainSettings.GridWidth;
                    var unitX = canvas.Height / MainSettings.GridHeight;
                    field.Width = unitY;
                    field.Height = unitX;
                    Canvas.SetTop(field, unitY * i);
                    Canvas.SetLeft(field, unitX * j);
                    canvas.Children.Add(field);
                }
            }
        }

        private void SetTextBlocks()
        {
            player1Name.Text = _gameSession.Player1.Name;
            player2Name.Text = _gameSession.Player2.Name;
            this.NumberOfRounds = _gameSession.RoundNumber;
            this.Player1Hits = _gameSession.Player1.FiredShots.FindAll(shot => shot.Hit).Count;
            this.Player2Hits = _gameSession.Player2.FiredShots.FindAll(shot => shot.Hit).Count;
            this.ActualPlayerName = _gameSession.ActualPlayer.Name;
            showAIShipsLabel.Visibility = _gameSession.Player2 is AIPlayer ? Visibility.Visible : Visibility.Hidden;
            this.ActualPhase = _gameSession.IsPuttingDownPhase ? "Putting down" : "Shooting";
            var destroyedShips = _gameSession.GetOtherPlayer().Ships.FindAll(ship => ship.IsDestroyed).ConvertAll(ship => ship.Length);
            var remainingShips = _gameSession.GetOtherPlayer().Ships.FindAll(ship => !ship.IsDestroyed).ConvertAll(ship => ship.Length);
            this.ShipsDestroyed = destroyedShips is null ? string.Empty : string.Join(", ", destroyedShips);
            this.ShipsRemains = remainingShips is null ? string.Empty : string.Join(", ", remainingShips);
            if (_gameSession.IsPuttingDownPhase && _gameSession.ActualPlayer.ShipCount < MainSettings.PlayableShipsLength.Length)
            {
                this.NextShipSize = MainSettings.PlayableShipsLength[_gameSession.ActualPlayer.ShipCount];
                nextShipSizeLabel.Visibility = Visibility.Visible;
                nextShipSize.Visibility = Visibility.Visible;
            }
            else
            {
                nextShipSizeLabel.Visibility = Visibility.Hidden;
                nextShipSize.Visibility = Visibility.Hidden;
            }
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

        private void DrawActualPlayerShips()
        {
            _gameSession.ActualPlayer.ShipsCoordinate.ForEach(shipPart =>
            {
                GetRectangleFromVector(shipPart.Coordinate, GetActualPlayerCanvas()).Fill = MainSettings.ShipColor;
            });
        }

        private void DrawAIShips()
        {
            _gameSession.Player2.ShipsCoordinate.ForEach(shipPart =>
            {
                GetRectangleFromVector(shipPart.Coordinate, GetEnemyPlayerCanvas()).Fill = MainSettings.ShipColor;
            });
        }

        private void DrawActualPlayerShots()
        {
            _gameSession.ActualPlayer.FiredShots.ForEach(shot =>
            {
                var color = shot.Hit ? MainSettings.HitColor : MainSettings.MissColor;
                GetRectangleFromVector(shot.Coordinate, GetEnemyPlayerCanvas()).Fill = color;
            });
        }

        private void DrawEnemyPlayerShots()
        {
            var enemy = _gameSession.ActualPlayer.Equals(_gameSession.Player1) ? _gameSession.Player2 : _gameSession.Player1;
            enemy.FiredShots.ForEach(shot =>
            {
                var color = shot.Hit ? MainSettings.HitColor : MainSettings.MissColor;
                GetRectangleFromVector(shot.Coordinate, GetActualPlayerCanvas()).Fill = color;
            });
        }

        private void ClearFields(Canvas canvas)
        {
            foreach (var i in canvas.Children)
            {
                var rectangle = (Rectangle)i;
                rectangle.Fill = Brushes.Transparent;
            }
        }

        private void SetHoverListeners(Canvas canvas)
        {
            foreach (var i in canvas.Children)
            {
                var rectangle = (Rectangle)i;
                rectangle.MouseEnter += OnRectangleHover;
                rectangle.MouseLeave += OnRectangleLeave;
            }
        }

        private void RemoveHoverListeners(Canvas canvas)
        {
            foreach (var i in canvas.Children)
            {
                var rectangle = (Rectangle)i;
                rectangle.MouseEnter -= OnRectangleHover;
                rectangle.MouseLeave -= OnRectangleLeave;
            }
        }

        private void OnRectangleHover(object sender, MouseEventArgs e)
        {
            if (_gameSession.ShipStartPoint != null)
            {
                var rectangle = (Rectangle)sender;
                if (rectangle.Parent == GetActualPlayerCanvas())
                {
                    var point = new Point(Canvas.GetLeft(rectangle), Canvas.GetTop(rectangle));
                    var x = Math.Floor(point.X / 30);
                    var y = Math.Floor(point.Y / 30);
                    var vectorOfHover = new Vector((int)x, (int)y);
                    var vectorOfShipStart = _gameSession.ShipStartPoint.GetValueOrDefault();
                    if (vectorOfHover.Y == vectorOfShipStart.Y)
                    {
                        var smaller = vectorOfHover.X < vectorOfShipStart.X ? vectorOfHover.X :vectorOfShipStart.X;
                        var bigger = smaller == vectorOfHover.X ? vectorOfShipStart.X : vectorOfHover.X;
                        for (int i = smaller; i <= bigger; i++)
                        {
                            GetRectangleFromVector(new Vector(i, vectorOfHover.Y), GetActualPlayerCanvas()).Fill = MainSettings.ShipColor;
                        }
                    }
                    else if (vectorOfHover.X == vectorOfShipStart.X)
                    {
                        var smaller = vectorOfHover.Y < vectorOfShipStart.Y ? vectorOfHover.Y : vectorOfShipStart.Y;
                        var bigger = smaller == vectorOfHover.Y ? vectorOfShipStart.Y : vectorOfHover.Y;
                        for (int i = smaller; i <= bigger; i++)
                        {
                            GetRectangleFromVector(new Vector(vectorOfHover.X, i), GetActualPlayerCanvas()).Fill = MainSettings.ShipColor;
                        }
                    }
                    else
                    {
                        ClearFields(GetActualPlayerCanvas());
                        DrawActualPlayerShips();
                        GetRectangleFromVector(_gameSession.ShipStartPoint.GetValueOrDefault(), GetActualPlayerCanvas()).Fill = MainSettings.ShipColor;
                    }
                }
            }
        }

        private void OnRectangleLeave(object sender, MouseEventArgs e)
        {
            var rectangle = (Rectangle)sender;
            if (rectangle.Parent == GetActualPlayerCanvas())
            {
                var point = new Point(Canvas.GetLeft(rectangle), Canvas.GetTop(rectangle));
                var x = Math.Floor(point.X / 30);
                var y = Math.Floor(point.Y / 30);
                var vectorOfHover = new Vector((int)x, (int)y);
                if (_gameSession.ActualPlayer.ShipsCoordinate.Find(shipPart => shipPart.Coordinate == vectorOfHover) != null)
                {
                    rectangle.Fill = MainSettings.ShipColor;
                }
                else
                {
                    rectangle.Fill = MainSettings.DefaultFieldColor;
                }
            }
        }

        private Canvas GetActualPlayerCanvas()
        {
            return _gameSession.ActualPlayer.Equals(_gameSession.Player1) ? player1Canvas : player2Canvas;
        }

        private Canvas GetEnemyPlayerCanvas()
        {
            return _gameSession.ActualPlayer.Equals(_gameSession.Player1) ? player2Canvas : player1Canvas;
        }
    }
}
