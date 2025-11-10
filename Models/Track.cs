using System;

namespace TimeBasedRacingGame.Models
{
    /// <summary>
    /// Represents the race track with lap management
    /// </summary>
    public class Track
    {
        /// <summary>
        /// Gets the total number of laps in the race
        /// </summary>
        public int TotalLaps { get; }

        /// <summary>
        /// Gets the distance per lap in kilometers
        /// </summary>
        public double LapDistance { get; }

        /// <summary>
        /// Gets or sets the current lap number
        /// </summary>
        public int CurrentLap { get; set; }

        /// <summary>
        /// Gets or sets the progress within the current lap (0-100%)
        /// </summary>
        public double LapProgress { get; set; }

        /// <summary>
        /// Initializes a new instance of the Track class
        /// </summary>
        /// <param name="totalLaps">Total number of laps</param>
        /// <param name="lapDistance">Distance per lap in kilometers</param>
        public Track(int totalLaps = 5, double lapDistance = 10.0)
        {
            TotalLaps = totalLaps;
            LapDistance = lapDistance;
            CurrentLap = 1;
            LapProgress = 0;
        }

        /// <summary>
        /// Advances progress based on speed
        /// </summary>
        /// <param name="speed">Current speed</param>
        /// <returns>True if lap completed, false otherwise</returns>
        public bool AdvanceProgress(int speed)
        {
            double progressIncrement = (speed * 2.0); // Speed affects progress
            LapProgress += progressIncrement;

            if (LapProgress >= 100)
            {
                double overflow = LapProgress - 100;
                LapProgress = overflow;
                CurrentLap++;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Checks if the race is completed
        /// </summary>
        /// <returns>True if all laps are completed</returns>
        public bool IsRaceCompleted()
        {
            return CurrentLap > TotalLaps;
        }

        /// <summary>
        /// Gets the overall race progress percentage
        /// </summary>
        /// <returns>Overall progress (0-100%)</returns>
        public double GetOverallProgress()
        {
            if (IsRaceCompleted()) return 100;
            
            double completedLaps = CurrentLap - 1;
            double currentLapProgress = LapProgress / 100.0;
            return ((completedLaps + currentLapProgress) / TotalLaps) * 100;
        }

        /// <summary>
        /// Gets a visual progress indicator
        /// </summary>
        /// <returns>String representation of progress</returns>
        public string GetProgressIndicator()
        {
            int progressBars = Math.Max(0, Math.Min(10, (int)(LapProgress / 10)));
            int spaces = Math.Max(0, 10 - progressBars);
            return "[" + new string('=', progressBars) + ">" + new string(' ', spaces) + "]";
        }
    }
}