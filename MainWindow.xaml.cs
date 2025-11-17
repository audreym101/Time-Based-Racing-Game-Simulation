using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Threading;
using TimeBasedRacingGame.Models;

namespace TimeBasedRacingGame
{
    public partial class MainWindow : Window
    {
        private RaceManager raceManager;
        private DispatcherTimer gameTimer;

        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        private void InitializeGame()
        {
            raceManager = new RaceManager();
            CarSelectionComboBox.ItemsSource = raceManager.AvailableCars;
            CarSelectionComboBox.DisplayMemberPath = "Name";
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromSeconds(1);
            gameTimer.Tick += GameTimer_Tick;
            UpdateUI();
        }

        private void CarSelectionComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (CarSelectionComboBox.SelectedItem is Car selectedCar)
            {
                raceManager.SelectedCar = selectedCar;
                CarInfoLabel.Text = selectedCar.GetCarInfo();
                StartRaceButton.IsEnabled = true;
            }
        }

        private void StartRaceButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                raceManager.StartRace();
                StartRaceButton.IsEnabled = false;
                CarSelectionComboBox.IsEnabled = false;
                EnableActionButtons(true);
                gameTimer.Start();
                UpdateUI();
            }
            catch (RaceException ex)
            {
                MessageBox.Show(ex.Message, "Race Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SpeedUpButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteRaceAction(RaceAction.SpeedUp);
        }

        private void MaintainSpeedButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteRaceAction(RaceAction.MaintainSpeed);
        }

        private void PitStopButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteRaceAction(RaceAction.PitStop);
        }

        private void ExecuteRaceAction(RaceAction action)
        {
            try
            {
                raceManager.ExecuteAction(action);
                UpdateUI();
                if (!raceManager.IsRaceActive)
                {
                    EndRace();
                }
            }
            catch (RaceException ex)
            {
                MessageBox.Show(ex.Message, "Race Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (raceManager.IsRaceActive)
            {
                raceManager.TimeRemaining = Math.Max(0, raceManager.TimeRemaining - 0.5);
                UpdateUI();
                if (raceManager.TimeRemaining <= 0)
                {
                    raceManager.ExecuteAction(RaceAction.MaintainSpeed);
                }
            }
        }

        private void UpdateUI()
        {
            if (raceManager.SelectedCar != null)
            {
                CurrentLapLabel.Text = raceManager.Track.IsRaceCompleted() 
                    ? $"{raceManager.Track.TotalLaps}/{raceManager.Track.TotalLaps}" 
                    : $"{raceManager.Track.CurrentLap}/{raceManager.Track.TotalLaps}";

                double fuelPercentage = raceManager.SelectedCar.GetFuelPercentage();
                FuelProgressBar.Value = fuelPercentage;
                FuelPercentageLabel.Text = $"{fuelPercentage:F1}%";

                double timePercentage = raceManager.GetTimePercentage();
                TimeProgressBar.Value = timePercentage;
                TimeRemainingLabel.Text = FormatTime(raceManager.TimeRemaining);

                CurrentSpeedLabel.Text = $"{raceManager.SelectedCar.CurrentSpeed} km/h";
                ProgressIndicatorLabel.Text = raceManager.Track.GetProgressIndicator();
                PositionLabel.Text = raceManager.GetPlayerPosition();
                ActionLogTextBlock.Text = raceManager.RaceState.GetActionLogText();

                if (raceManager.IsRaceActive)
                {
                    CarInfoLabel.Text = GetCurrentCarStatus();
                    RaceStatusLabel.Text = "Race in progress...";
                }
                else if (!string.IsNullOrEmpty(raceManager.RaceResult))
                {
                    RaceStatusLabel.Text = "Race finished";
                    RaceResultLabel.Text = raceManager.RaceResult;
                    RaceResultBorder.Visibility = Visibility.Visible;
                    
                    if (raceManager.RaceResult.Contains("won") || raceManager.RaceResult.Contains("completed"))
                    {
                        RaceResultBorder.Background = new SolidColorBrush(Color.FromRgb(76, 175, 80));
                    }
                    else
                    {
                        RaceResultBorder.Background = new SolidColorBrush(Color.FromRgb(244, 67, 54));
                    }
                }
                else
                {
                    RaceResultBorder.Visibility = Visibility.Collapsed;
                }
            }
        }

        private string FormatTime(double seconds)
        {
            int minutes = (int)(seconds / 60);
            int secs = (int)(seconds % 60);
            return $"{minutes}:{secs:D2}";
        }

        private void EnableActionButtons(bool enabled)
        {
            SpeedUpButton.IsEnabled = enabled;
            MaintainSpeedButton.IsEnabled = enabled;
            PitStopButton.IsEnabled = enabled;
        }

        private string GetCurrentCarStatus()
        {
            if (raceManager.SelectedCar == null) return "";
            
            var car = raceManager.SelectedCar;
            string fuelStatus = car.GetFuelPercentage() switch
            {
                > 75 => "🟢 FULL",
                > 50 => "🟡 GOOD", 
                > 25 => "🟠 LOW",
                _ => "🔴 CRITICAL"
            };
            
            string speedStatus = car.CurrentSpeed switch
            {
                0 => "⏸️ STOPPED",
                < 50 => "🐌 SLOW",
                < 100 => "🚗 MODERATE",
                _ => "🏎️ FAST"
            };
            
            return $"🏎️ {car.Name} | Speed: {car.CurrentSpeed}/{car.MaxSpeed} km/h {speedStatus} | " +
                   $"Fuel: {car.CurrentFuel:F1}/{car.MaxFuel}L {fuelStatus}";
        }

        private void EndRace()
        {
            gameTimer.Stop();
            EnableActionButtons(false);
            StartRaceButton.IsEnabled = true;
            CarSelectionComboBox.IsEnabled = true;
            UpdateUI();
        }
    }
}