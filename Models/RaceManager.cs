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
        /// Gets the current race state
        /// </summary>
        public RaceState RaceState { get; private set; }

        /// <summary>
        /// Gets the list of all racers
        /// </summary>
        public List<Racer> Racers { get; private set; }

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
            RaceState = new RaceState();

            // Initialize racers
            Racers = new List<Racer>
            {
                new Racer("You", true),
                new Racer("Speed Racer"),
                new Racer("Lightning McQueen"),
                new Racer("Turbo Tom")
            };

            AvailableCars = new List<Car>
            {
                new Car(CarType.SportsCar, "NEON STRIKER", 120, 8.0, 60),
                new Car(CarType.EcoCar, "CYBER CRUISER", 80, 4.0, 80),
                new Car(CarType.RaceCar, "PLASMA PHANTOM", 150, 12.0, 50)
            };
        }

        /// <summary>
        /// Starts the race with the selected car
        /// </summary>
        /// <exception cref="RaceException">Thrown when no car is selected</exception>
        public void StartRace()
        {
            if (SelectedCar == null)
                throw new RaceException("No car selected for race start");

            IsRaceActive = true;
            RaceResult = "";
            TimeRemaining = MaxTime;
            Track.CurrentLap = 1;
            Track.LapProgress = 0;
            SelectedCar.CurrentSpeed = 0;
            SelectedCar.CurrentFuel = SelectedCar.MaxFuel;
            
            // Update race state
            RaceState.IsActive = true;
            RaceState.CurrentLap = 1;
            RaceState.LapProgress = 0;
            RaceState.TimeRemaining = MaxTime;
            RaceState.CurrentFuel = SelectedCar.CurrentFuel;
            RaceState.CurrentSpeed = 0;
            RaceState.LogAction($"Race started with {SelectedCar.Name}");
        }

        /// <summary>
        /// Executes a race action and updates game state
        /// </summary>
        /// <param name="action">The action to perform</param>
        /// <exception cref="RaceException">Thrown when race is not active or action is invalid</exception>
        public void ExecuteAction(RaceAction action)
        {
            if (!IsRaceActive)
                throw new RaceException("Cannot execute action - race is not active");

            try
            {
                switch (action)
                {
                    case RaceAction.SpeedUp:
                        SpeedUp();
                        RaceState.LogAction("Speed increased");
                        break;
                    case RaceAction.MaintainSpeed:
                        MaintainSpeed();
                        RaceState.LogAction("Speed maintained");
                        break;
                    case RaceAction.PitStop:
                        PitStop();
                        RaceState.LogAction("Pit stop completed");
                        break;
                }

                UpdateGameState();
            }
            catch (InvalidOperationException ex)
            {
                RaceState.LogAction($"Action failed: {ex.Message}");
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
        /// <exception cref="RaceException">Thrown when fuel tank is already full</exception>
        private void PitStop()
        {
            if (SelectedCar.CurrentFuel >= SelectedCar.MaxFuel * 0.95) // 95% full threshold
                throw new RaceException("Cannot pit stop - fuel tank is already full");
                
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
                    RaceState.LogAction($"Lap {Track.CurrentLap - 1} completed!");
                }
            }

            // Update race state
            RaceState.CurrentLap = Track.CurrentLap;
            RaceState.LapProgress = Track.LapProgress;
            RaceState.TimeRemaining = TimeRemaining;
            RaceState.CurrentFuel = SelectedCar.CurrentFuel;
            RaceState.CurrentSpeed = SelectedCar.CurrentSpeed;

            // Update player position
            UpdatePositions();

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

        /// <summary>
        /// Updates racer positions based on progress
        /// </summary>
        private void UpdatePositions()
        {
            var playerRacer = Racers.First(r => r.IsPlayer);
            playerRacer.CurrentLap = Track.CurrentLap;
            playerRacer.LapProgress = Track.LapProgress;

            // Simulate AI progress
            var random = new Random();
            foreach (var racer in Racers.Where(r => !r.IsPlayer))
            {
                if (IsRaceActive)
                {
                    racer.LapProgress += random.Next(8, 15);
                    if (racer.LapProgress >= 100)
                    {
                        racer.LapProgress = 0;
                        racer.CurrentLap++;
                    }
                }
            }

            // Calculate positions
            var sortedRacers = Racers.OrderByDescending(r => r.GetTotalProgress()).ToList();
            for (int i = 0; i < sortedRacers.Count; i++)
            {
                sortedRacers[i].Position = i + 1;
            }
        }

        /// <summary>
        /// Gets the player's current position
        /// </summary>
        /// <returns>Player position (1st, 2nd, etc.)</returns>
        public string GetPlayerPosition()
        {
            var playerRacer = Racers.First(r => r.IsPlayer);
            return playerRacer.Position switch
            {
                1 => "1st",
                2 => "2nd",
                3 => "3rd",
                _ => $"{playerRacer.Position}th"
            };
        }
    }
}