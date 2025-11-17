using System;

namespace TimeBasedRacingGame.Models
{
    /// <summary>
    /// Represents different car types with unique characteristics
    /// </summary>
    public enum CarType
    {
        /// <summary>Sports car with high speed and moderate fuel consumption</summary>
        SportsCar,
        /// <summary>Eco-friendly car with low fuel consumption</summary>
        EcoCar,
        /// <summary>High-performance race car with maximum speed</summary>
        RaceCar
    }

    /// <summary>
    /// Represents a racing car with fuel, speed, and performance characteristics
    /// </summary>
    public class Car
    {
        /// <summary>
        /// Gets the car type
        /// </summary>
        public CarType Type { get; }

        /// <summary>
        /// Gets the car name
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the maximum speed of the car
        /// </summary>
        public int MaxSpeed { get; }

        /// <summary>
        /// Gets the fuel consumption rate per action
        /// </summary>
        public double FuelConsumption { get; }

        /// <summary>
        /// Gets the maximum fuel capacity
        /// </summary>
        public double MaxFuel { get; }

        /// <summary>
        /// Gets or sets the current fuel level
        /// </summary>
        public double CurrentFuel { get; set; }

        /// <summary>
        /// Gets or sets the current speed
        /// </summary>
        public int CurrentSpeed { get; set; }

        /// <summary>
        /// Initializes a new instance of the Car class
        /// </summary>
        /// <param name="type">The car type</param>
        /// <param name="name">The car name</param>
        /// <param name="maxSpeed">Maximum speed</param>
        /// <param name="fuelConsumption">Fuel consumption rate</param>
        /// <param name="maxFuel">Maximum fuel capacity</param>
        public Car(CarType type, string name, int maxSpeed, double fuelConsumption, double maxFuel)
        {
            Type = type;
            Name = name;
            MaxSpeed = maxSpeed;
            FuelConsumption = fuelConsumption;
            MaxFuel = maxFuel;
            CurrentFuel = maxFuel;
            CurrentSpeed = 0;
        }

        /// <summary>
        /// Consumes fuel based on current speed and action
        /// </summary>
        /// <param name="speedMultiplier">Speed multiplier for fuel consumption</param>
        /// <exception cref="InvalidOperationException">Thrown when there's insufficient fuel</exception>
        public void ConsumeFuel(double speedMultiplier = 1.0)
        {
            double consumption = FuelConsumption * speedMultiplier;
            if (CurrentFuel < consumption)
                throw new InvalidOperationException("Insufficient fuel for this action");
            
            CurrentFuel = Math.Max(0, CurrentFuel - consumption);
        }

        /// <summary>
        /// Refuels the car to maximum capacity
        /// </summary>
        public void Refuel()
        {
            CurrentFuel = MaxFuel;
        }

        /// <summary>
        /// Gets the fuel percentage remaining
        /// </summary>
        /// <returns>Fuel percentage (0-100)</returns>
        public double GetFuelPercentage()
        {
            return (CurrentFuel / MaxFuel) * 100;
        }

        /// <summary>
        /// Gets detailed car information for display
        /// </summary>
        /// <returns>Formatted car information string</returns>
        public string GetCarInfo()
        {
            string strategy = Type switch
            {
                CarType.SportsCar => "⚡ BALANCED PERFORMANCE - Good speed with moderate fuel efficiency",
                CarType.EcoCar => "🌱 ECO STRATEGY - Lower speed but excellent fuel economy and high capacity", 
                CarType.RaceCar => "🏁 SPEED DEMON - Maximum speed but high fuel consumption and low capacity",
                _ => "Unknown car type"
            };
            
            return $"🏎️ {Name}\n" +
                   $"Max Speed: {MaxSpeed} km/h | Fuel Capacity: {MaxFuel}L | Consumption: {FuelConsumption}L/action\n" +
                   $"{strategy}";
        }
    }
}