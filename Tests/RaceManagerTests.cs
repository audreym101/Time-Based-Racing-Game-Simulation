using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TimeBasedRacingGame.Models;

namespace TimeBasedRacingGame.UnitTests
{
    /// <summary>
    /// Unit tests for the RaceManager class
    /// </summary>
    [TestClass]
    public class RaceManagerTests
    {
        /// <summary>
        /// Tests race start functionality
        /// </summary>
        [TestMethod]
        public void StartRace_ValidCar_InitializesRaceCorrectly()
        {
            // Arrange
            var raceManager = new RaceManager();
            raceManager.SelectedCar = raceManager.AvailableCars[0];

            // Act
            raceManager.StartRace();

            // Assert
            Assert.IsTrue(raceManager.IsRaceActive);
            Assert.AreEqual(1, raceManager.Track.CurrentLap);
            Assert.AreEqual(0, raceManager.SelectedCar.CurrentSpeed);
        }

        /// <summary>
        /// Tests race start without car selection
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(RaceException))]
        public void StartRace_NoCar_ThrowsException()
        {
            // Arrange
            var raceManager = new RaceManager();
            // No car selected

            // Act
            raceManager.StartRace();

            // Assert - Exception expected
        }

        /// <summary>
        /// Tests speed up action
        /// </summary>
        [TestMethod]
        public void ExecuteAction_SpeedUp_IncreasesSpeedAndConsumesFuel()
        {
            // Arrange
            var raceManager = new RaceManager();
            raceManager.SelectedCar = raceManager.AvailableCars[0];
            raceManager.StartRace();
            double initialFuel = raceManager.SelectedCar.CurrentFuel;

            // Act
            raceManager.ExecuteAction(RaceAction.SpeedUp);

            // Assert
            Assert.AreEqual(20, raceManager.SelectedCar.CurrentSpeed);
            Assert.IsTrue(raceManager.SelectedCar.CurrentFuel < initialFuel);
        }
    }
}