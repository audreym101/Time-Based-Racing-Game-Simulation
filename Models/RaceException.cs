using System;

namespace TimeBasedRacingGame.Models
{
    /// <summary>
    /// Custom exception for race-related errors and invalid operations
    /// </summary>
    public class RaceException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the RaceException class
        /// </summary>
        public RaceException() : base() { }

        /// <summary>
        /// Initializes a new instance of the RaceException class with a specified error message
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        public RaceException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the RaceException class with a specified error message and inner exception
        /// </summary>
        /// <param name="message">The message that describes the error</param>
        /// <param name="innerException">The exception that is the cause of the current exception</param>
        public RaceException(string message, Exception innerException) : base(message, innerException) { }
    }
}