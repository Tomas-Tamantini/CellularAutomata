using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;
using CellularAutomataModels;

namespace CellularAutomataDesktop
{
    public partial class MainWindow : Window
    {
        private ICelullarAutomaton automaton;
        private DispatcherTimer dispatcherTimer;
        private bool animationIsPaused = true;
        private int frameDurationMs = 150;
        private int cellSize = 10;
        private int cellPadding = 1;
        private int wireWorldCellStatus = (int)WireWorldStatus.wire;
        public MainWindow()
        {
            InitializeComponent();
            this.automaton = Samples.Diode();
            this.DrawGame();
            this.dispatcherTimer = new DispatcherTimer();
            this.dispatcherTimer.Tick += new EventHandler(StepAnimation);
            this.dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, frameDurationMs);
            this.dispatcherTimer.Start();
        }

        private void StepAnimation(object sender, EventArgs e)
        {
            if (this.animationIsPaused) return;
            this.automaton.Step();
            this.DrawGame();
        }


        private void DrawGame()
        {
            animationCanvas.Children.Clear();
            foreach (var c in this.automaton.Cells) DrawCell(c);
        }

        private void DrawCell(Cell c)
        {
            var fill = Brushes.DarkGray;
            var stroke = Brushes.Black;

            if (this.automaton is GameOfLife && c.status != (int)GameOfLifeStatus.alive) return;

            else if (this.automaton is WireWorld)
            {
                if (c.status == (int)WireWorldStatus.wire)
                {
                    fill = Brushes.DarkOrange;
                    stroke = Brushes.DarkRed;
                }
                else if (c.status == (int)WireWorldStatus.electronHead)
                {
                    fill = Brushes.Blue;
                    stroke = Brushes.DarkBlue;
                }
                else if (c.status == (int)WireWorldStatus.electronTail)
                {
                    fill = Brushes.IndianRed;
                    stroke = Brushes.DarkRed;
                }
                else return;
            }

            var (i, j) = c.position;
            var cellElement = new Ellipse()
            {
                Width = cellSize,
                Height = cellSize,
                Stroke = stroke,
                Fill = fill
            };

            Canvas.SetLeft(cellElement, i * (cellSize + cellPadding));
            Canvas.SetTop(cellElement, j * (cellSize + cellPadding));
            animationCanvas.Children.Add(cellElement);
        }

        private void PlayPauseAnimation(object sender, RoutedEventArgs e)
        {
            this.animationIsPaused = !this.animationIsPaused;
        }

        private void ToggleGame(object sender, RoutedEventArgs e)
        {
            this.animationIsPaused = true;
            if (this.automaton is GameOfLife)
            {
                this.automaton = Samples.Diode();
                wireWorldCellType.Visibility = Visibility.Visible;
            }
            else
            {
                this.automaton = Samples.DieHard();
                wireWorldCellType.Visibility = Visibility.Collapsed;
            }
            this.DrawGame();
        }

        private (int, int) CellPositionFromClick()
        {
            var mouseX = Mouse.GetPosition(animationCanvas).X;
            var mouseY = Mouse.GetPosition(animationCanvas).Y;
            var xPos = (int)Math.Floor(mouseX / (cellSize + cellPadding));
            var yPos = (int)Math.Floor(mouseY / (cellSize + cellPadding));
            return (xPos, yPos);
        }

        private void AddOrRemoveCell(object sender, MouseButtonEventArgs e)
        {
            if (!animationIsPaused) return;
            var (xPos, yPos) = CellPositionFromClick();
            var (deadStatus, aliveStatus) = GetNewCellStatus();
            bool cellWasRemoved = automaton.ChangeCellStatus(xPos, yPos, deadStatus);
            if (!cellWasRemoved) automaton.ChangeCellStatus(xPos, yPos, aliveStatus);
            this.DrawGame();
            return;
        }

        private (int, int) GetNewCellStatus()
        {
            int deadStatus, aliveStatus;
            if (this.automaton is GameOfLife)
            {
                deadStatus = (int)GameOfLifeStatus.dead;
                aliveStatus = (int)GameOfLifeStatus.alive;
                return (deadStatus, aliveStatus);
            }
            deadStatus = (int)WireWorldStatus.empty;
            aliveStatus = this.wireWorldCellStatus;
            return (deadStatus, aliveStatus);
            
        }

        private void wireWorldRadioBtnChecked(object sender, RoutedEventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Name == "rbHead") wireWorldCellStatus = (int)WireWorldStatus.electronHead;
            else if (rb.Name == "rbTail") wireWorldCellStatus = (int)WireWorldStatus.electronTail;
            else if (rb.Name == "rbWire") wireWorldCellStatus = (int)WireWorldStatus.wire;
        }
    }
}
