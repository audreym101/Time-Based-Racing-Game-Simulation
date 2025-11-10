using System;
using System.Linq;
using System.Windows;
using System.Windows.Threading;
using TimeBasedRacingGame.Models;

namespace TimeBasedRacingGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RaceManager raceManager;
        private DispatcherTimer gameTimer;

        /// <summary>
        /// Initializes a new instance of the MainWindow class
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            InitializeGame();
        }

        /// <summary>
        /// Initializes the game components and UI
        /// </summary>
        private void InitializeGame()
        {
            raceManager = new RaceManager();
            
            // Setup car selection
            CarSelectionComboBox.ItemsSource = raceManager.AvailableCars;
            CarSelectionComboBox.DisplayMemberPath = "Name";
            
            // Setup game timer for real-time updates
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = TimeSpan.FromSeconds(1);
            gameTimer.Tick += GameTimer_Tick;
            
            UpdateUI();
        }

        /// <summary>
        /// Handles car selection change event
        /// </summary>
        private void CarSelectionComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (CarSelectionComboBox.SelectedItem is Car selectedCar)
            {
                raceManager.SelectedCar = selectedCar;
                CarInfoLabel.Text = $"{selectedCar.Name} - Max Speed: {selectedCar.MaxSpeed} km/h, " +
                                   $"Fuel Capacity: {selectedCar.MaxFuel}L, Consumption: {selectedCar.FuelConsumption}L/action";
                StartRaceButton.IsEnabled = true;
            }
        }

        /// <summary>
        /// Handles start race button click event
        /// </summary>
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
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Handles speed up button click event
        /// </summary>
        private void SpeedUpButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteRaceAction(RaceAction.SpeedUp);
        }

        /// <summary>
        /// Handles maintain speed button click event
        /// </summary>
        private void MaintainSpeedButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteRaceAction(RaceAction.MaintainSpeed);
        }

        /// <summary>
        /// Handles pit stop button click event
        /// </summary>
        private void PitStopButton_Click(object sender, RoutedEventArgs e)
        {
            ExecuteRaceAction(RaceAction.PitStop);
        }

        /// <summary>
        /// Executes a race action and handles exceptions
        /// </summary>
        /// <param name="action">The race action to execute</param>
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
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Action Failed", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        /// <summary>
        /// Handles game timer tick for real-time updates
        /// </summary>
        private void GameTimer_Tick(object sender, EventArgs e)
        {
            if (raceManager.IsRaceActive)
            {
                // Slight time decay for realism
                raceManager.TimeRemaining = Math.Max(0, raceManager.TimeRemaining - 0.5);
                UpdateUI();
                
                if (raceManager.TimeRemaining <= 0)
                {
                    raceManager.ExecuteAction(RaceAction.MaintainSpeed); // Trigger time check
                }
            }
        }

        /// <summary>
        /// Updates all UI elements with current game state
        /// </summary>
        private void UpdateUI()
        {
            if (raceManager.SelectedCar != null)
            {
                // Update lap information
                CurrentLapLabel.Text = raceManager.Track.IsRaceCompleted() 
                    ? $"{raceManager.Track.TotalLaps}/{raceManager.Track.TotalLaps}" 
                    : $"{raceManager.Track.CurrentLap}/{raceManager.Track.TotalLaps}";

                // Update fuel
                double fuelPercentage = raceManager.SelectedCar.GetFuelPercentage();
                FuelProgressBar.Value = fuelPercentage;
                FuelPercentageLabel.Text = $"{fuelPercentage:F1}%";

                // Update time
                double timePercentage = raceManager.GetTimePercentage();
                TimeProgressBar.Value = timePercentage;
                TimeRemainingLabel.Text = FormatTime(raceManager.TimeRemaining);

                // Update speed
                CurrentSpeedLabel.Text = $"{raceManager.SelectedCar.CurrentSpeed} km/h";

                // Update progress indicator
                ProgressIndicatorLabel.Text = raceManager.Track.GetProgressIndicator();

                // Update status
                if (raceManager.IsRaceActive)
                {
                    RaceStatusLabel.Text = "Race in progress...";
                }
                else if (!string.IsNullOrEmpty(raceManager.RaceResult))
                {
                    RaceStatusLabel.Text = "Race finished";
                    RaceResultLabel.Text = raceManager.RaceResult;
                }
            }
        }

        /// <summary>
        /// Formats time in MM:SS format
        /// </summary>
        /// <param name="seconds">Time in seconds</param>
        /// <returns>Formatted time string</returns>
        private string FormatTime(double seconds)
        {
            int minutes = (int)(seconds / 60);
            int secs = (int)(seconds % 60);
            return $"{minutes}:{secs:D2}";
        }

        /// <summary>
        /// Enables or disables action buttons
        /// </summary>
        /// <param name="enabled">Whether buttons should be enabled</param>
        private void EnableActionButtons(bool enabled)
        {
            SpeedUpButton.IsEnabled = enabled;
            MaintainSpeedButton.IsEnabled = enabled;
            PitStopButton.IsEnabled = enabled;
        }

        /// <summary>
        /// Handles race end cleanup
        /// </summary>
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