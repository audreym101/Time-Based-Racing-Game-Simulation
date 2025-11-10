using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TimeBasedRacingGame.Models;

namespace TimeBasedRacingGame.UnitTests
{
    /// <summary>
    /// Unit tests for the Car class
    /// </summary>
    [TestClass]
    public class CarTests
    {
        /// <summary>
        /// Tests fuel consumption functionality
        /// </summary>
        [TestMethod]
        public void ConsumeFuel_ValidConsumption_ReducesFuelCorrectly()
        {
            // Arrange
            var car = new Car(CarType.SportsCar, "Test Car", 100, 5.0, 50.0);
            double initialFuel = car.CurrentFuel;

            // Act
            car.ConsumeFuel(1.0);

            // Assert
            Assert.AreEqual(initialFuel - 5.0, car.CurrentFuel);
        }

        /// <summary>
        /// Tests fuel consumption with insufficient fuel
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ConsumeFuel_InsufficientFuel_ThrowsException()
        {
            // Arrange
            var car = new Car(CarType.SportsCar, "Test Car", 100, 10.0, 50.0);
            car.CurrentFuel = 5.0; // Less than consumption rate

            // Act
            car.ConsumeFuel(1.0);

            // Assert - Exception expected
        }

        /// <summary>
        /// Tests refuel functionality
        /// </summary>
        [TestMethod]
        public void Refuel_PartialFuel_FillsToMaxCapacity()
        {
            // Arrange
            var car = new Car(CarType.EcoCar, "Eco Test", 80, 4.0, 60.0);
            car.CurrentFuel = 30.0;

            // Act
            car.Refuel();

            // Assert
            Assert.AreEqual(car.MaxFuel, car.CurrentFuel);
        }
    }
}