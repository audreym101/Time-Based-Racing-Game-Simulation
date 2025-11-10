using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeBasedRacingGame.Models;

namespace TimeBasedRacingGame.UnitTests
{
    /// <summary>
    /// Unit tests for the Track class
    /// </summary>
    [TestClass]
    public class TrackTests
    {
        /// <summary>
        /// Tests lap progression functionality
        /// </summary>
        [TestMethod]
        public void AdvanceProgress_NormalSpeed_UpdatesProgressCorrectly()
        {
            // Arrange
            var track = new Track(5, 10.0);
            int speed = 50;

            // Act
            bool lapCompleted = track.AdvanceProgress(speed);

            // Assert
            Assert.IsTrue(lapCompleted);
            Assert.AreEqual(0.0, track.LapProgress); // 50 * 2.0 = 100, lap completed, progress resets
        }

        /// <summary>
        /// Tests lap completion
        /// </summary>
        [TestMethod]
        public void AdvanceProgress_HighSpeed_CompletesLap()
        {
            // Arrange
            var track = new Track(5, 10.0);
            track.LapProgress = 80.0;
            int speed = 25; // 25 * 2.0 = 50, total = 130 > 100

            // Act
            bool lapCompleted = track.AdvanceProgress(speed);

            // Assert
            Assert.IsTrue(lapCompleted);
            Assert.AreEqual(2, track.CurrentLap);
            Assert.AreEqual(30.0, track.LapProgress); // 130 - 100 = 30 overflow
        }

        /// <summary>
        /// Tests race completion detection
        /// </summary>
        [TestMethod]
        public void IsRaceCompleted_AllLapsFinished_ReturnsTrue()
        {
            // Arrange
            var track = new Track(3, 10.0);
            track.CurrentLap = 4; // Beyond total laps

            // Act
            bool isCompleted = track.IsRaceCompleted();

            // Assert
            Assert.IsTrue(isCompleted);
        }
    }
}