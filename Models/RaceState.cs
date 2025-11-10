using System;
using System.Collections.Generic;

namespace TimeBasedRacingGame.Models
{
    /// <summary>
    /// Represents the current state of the race including progress and action history
    /// </summary>
    public class RaceState
    {
        /// <summary>
        /// Gets or sets the current lap number
        /// </summary>
        public int CurrentLap { get; set; }

        /// <summary>
        /// Gets or sets the progress within the current lap (0-100%)
        /// </summary>
        public double LapProgress { get; set; }

        /// <summary>
        /// Gets or sets the remaining time in seconds
        /// </summary>
        public double TimeRemaining { get; set; }

        /// <summary>
        /// Gets or sets the current fuel level
        /// </summary>
        public double CurrentFuel { get; set; }

        /// <summary>
        /// Gets or sets the current speed
        /// </summary>
        public int CurrentSpeed { get; set; }

        /// <summary>
        /// Gets the action history log
        /// </summary>
        public Queue<string> ActionLog { get; }

        /// <summary>
        /// Gets whether the race is currently active
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Initializes a new instance of the RaceState class
        /// </summary>
        public RaceState()
        {
            ActionLog = new Queue<string>();
            CurrentLap = 1;
            LapProgress = 0;
            TimeRemaining = 300;
            CurrentFuel = 0;
            CurrentSpeed = 0;
            IsActive = false;
        }

        /// <summary>
        /// Logs an action to the history queue
        /// </summary>
        /// <param name="action">The action description to log</param>
        public void LogAction(string action)
        {
            ActionLog.Enqueue($"{DateTime.Now:HH:mm:ss} - {action}");
            
            // Keep only last 10 actions
            while (ActionLog.Count > 10)
            {
                ActionLog.Dequeue();
            }
        }

        /// <summary>
        /// Gets the most recent action log entries as a formatted string
        /// </summary>
        /// <returns>Formatted action log</returns>
        public string GetActionLogText()
        {
            return string.Join("\n", ActionLog);
        }
    }
}