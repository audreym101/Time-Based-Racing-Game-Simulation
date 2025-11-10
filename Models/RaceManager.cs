using System;
using System.Collections.Generic;
using System.Linq;

namespace TimeBasedRacingGame.Models
{
    /// <summary>
    /// Represents different race actions a player can take
    /// </summary>
    public enum RaceAction
    {
        /// <summary>Increase speed and consume more fuel</summary>
        SpeedUp,
        /// <summary>Maintain current speed with normal fuel consumption</summary>
        MaintainSpeed,
        /// <summary>Stop to refuel and reset speed to zero</summary>
        PitStop
    }

    /// <summary>
    /// Manages the race simulation including cars, track, and game state
    /// </summary>
    public class RaceManager
    {
        /// <summary>
        /// Gets the available cars for selection
        /// </summary>
        public List<Car> AvailableCars { get; }

        /// <summary>
        /// Gets or sets the selected car
        /// </summary>
        public Car SelectedCar { get; set; }

        /// <summary>
        /// Gets the race track
        /// </summary>
        public Track Track { get; }

        /// <summary>
        /// Gets or sets the remaining time in seconds
        /// </summary>
        public double TimeRemaining { get; set; }

        /// <summary>
        /// Gets the maximum race time
        /// </summary>
        public double MaxTime { get; }

        /// <summary>
        /// Gets whether the race is active
        /// </summary>
        public bool IsRaceActive { get; private set; }

        /// <summary>
        /// Gets the race result message
        /// </summary>
        public string RaceResult { get; private set; }

        /// <summary>
        /// Initializes a new instance of the RaceManager class
        /// </summary>
        /// <param name="maxTime">Maximum race time in seconds</param>
        public RaceManager(double maxTime = 300)
        {
            MaxTime = maxTime;
            TimeRemaining = maxTime;
            Track = new Track();
            IsRaceActive = false;
            RaceResult = "";

            AvailableCars = new List<Car>
            {
                new Car(CarType.SportsCar, "Lightning Bolt", 120, 8.0, 60),
                new Car(CarType.EcoCar, "Green Machine", 80, 4.0, 80),
                new Car(CarType.RaceCar, "Speed Demon", 150, 12.0, 50)
            };
        }

        /// <summary>
        /// Starts the race with the selected car
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when no car is selected</exception>
        public void StartRace()
        {
            if (SelectedCar == null)
                throw new InvalidOperationException("No car selected");

            IsRaceActive = true;
            RaceResult = "";
            TimeRemaining = MaxTime;
            Track.CurrentLap = 1;
            Track.LapProgress = 0;
            SelectedCar.CurrentSpeed = 0;
            SelectedCar.CurrentFuel = SelectedCar.MaxFuel;
        }

        /// <summary>
        /// Executes a race action and updates game state
        /// </summary>
        /// <param name="action">The action to perform</param>
        /// <exception cref="InvalidOperationException">Thrown when race is not active or action is invalid</exception>
        public void ExecuteAction(RaceAction action)
        {
            if (!IsRaceActive)
                throw new InvalidOperationException("Race is not active");

            try
            {
                switch (action)
                {
                    case RaceAction.SpeedUp:
                        SpeedUp();
                        break;
                    case RaceAction.MaintainSpeed:
                        MaintainSpeed();
                        break;
                    case RaceAction.PitStop:
                        PitStop();
                        break;
                }

                UpdateGameState();
            }
            catch (InvalidOperationException)
            {
                EndRace("Out of fuel!");
            }
        }

        /// <summary>
        /// Increases car speed and consumes fuel
        /// </summary>
        private void SpeedUp()
        {
            if (SelectedCar.CurrentSpeed < SelectedCar.MaxSpeed)
            {
                SelectedCar.CurrentSpeed = Math.Min(SelectedCar.MaxSpeed, SelectedCar.CurrentSpeed + 20);
            }
            SelectedCar.ConsumeFuel(1.5);
            TimeRemaining -= 5;
        }

        /// <summary>
        /// Maintains current speed with normal fuel consumption
        /// </summary>
        private void MaintainSpeed()
        {
            SelectedCar.ConsumeFuel(1.0);
            TimeRemaining -= 3;
        }

        /// <summary>
        /// Performs a pit stop to refuel
        /// </summary>
        private void PitStop()
        {
            SelectedCar.Refuel();
            SelectedCar.CurrentSpeed = 0;
            TimeRemaining -= 15;
        }

        /// <summary>
        /// Updates the game state after an action
        /// </summary>
        private void UpdateGameState()
        {
            if (SelectedCar.CurrentSpeed > 0)
            {
                bool lapCompleted = Track.AdvanceProgress(SelectedCar.CurrentSpeed);
                if (lapCompleted && !Track.IsRaceCompleted())
                {
                    // Lap completed notification could be added here
                }
            }

            CheckRaceConditions();
        }

        /// <summary>
        /// Checks for race end conditions
        /// </summary>
        private void CheckRaceConditions()
        {
            if (Track.IsRaceCompleted())
            {
                EndRace("Race completed! You won!");
            }
            else if (TimeRemaining <= 0)
            {
                EndRace("Time's up! Race over.");
            }
            else if (SelectedCar.CurrentFuel <= 0)
            {
                EndRace("Out of fuel! Race over.");
            }
        }

        /// <summary>
        /// Ends the race with a result message
        /// </summary>
        /// <param name="result">The race result message</param>
        private void EndRace(string result)
        {
            IsRaceActive = false;
            RaceResult = result;
        }

        /// <summary>
        /// Gets the time remaining percentage
        /// </summary>
        /// <returns>Time percentage (0-100)</returns>
        public double GetTimePercentage()
        {
            return (TimeRemaining / MaxTime) * 100;
        }
    }
}