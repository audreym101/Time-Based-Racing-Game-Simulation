using Microsoft.VisualStudio.TestTools.UnitTesting;
using TimeBasedRacingGame.Models;

namespace TimeBasedRacingGame.UnitTests
{
    /// <summary>
    /// Unit tests for the RaceState class
    /// </summary>
    [TestClass]
    public class RaceStateTests
    {
        /// <summary>
        /// Tests action logging functionality
        /// </summary>
        [TestMethod]
        public void LogAction_ValidAction_AddsToQueue()
        {
            // Arrange
            var raceState = new RaceState();
            string testAction = "Test action performed";

            // Act
            raceState.LogAction(testAction);

            // Assert
            Assert.AreEqual(1, raceState.ActionLog.Count);
            Assert.IsTrue(raceState.GetActionLogText().Contains(testAction));
        }

        /// <summary>
        /// Tests queue size limitation
        /// </summary>
        [TestMethod]
        public void LogAction_ExceedsMaxSize_MaintainsLimit()
        {
            // Arrange
            var raceState = new RaceState();

            // Act - Add 15 actions (more than 10 limit)
            for (int i = 1; i <= 15; i++)
            {
                raceState.LogAction($"Action {i}");
            }

            // Assert
            Assert.AreEqual(10, raceState.ActionLog.Count);
            Assert.IsTrue(raceState.GetActionLogText().Contains("Action 15"));
            Assert.IsFalse(raceState.GetActionLogText().Contains("Action 5")); // Earlier actions should be removed
        }
    }
}