# C# Speed Rush - Time-Based Racing Game Simulation

A turn-based racing simulation game built with .NET WPF that challenges players to manage fuel, time, and speed across multiple laps.

## Game Description

C# Speed Rush is a strategic racing simulation where players select from different car types and make tactical decisions each turn. The game focuses on resource management rather than real-time action, requiring players to balance speed with fuel consumption while racing against time.

## How to Play

1. **Car Selection**: Choose from three different car types:
   - **Lightning Bolt (Sports Car)**: High speed (120 km/h), moderate fuel consumption
   - **Green Machine (Eco Car)**: Moderate speed (80 km/h), low fuel consumption, high capacity
   - **Speed Demon (Race Car)**: Highest speed (150 km/h), high fuel consumption

2. **Race Actions**: Each turn, choose one action:
   - **Speed Up**: Increase speed by 20 km/h (max speed limit applies), consumes more fuel
   - **Maintain Speed**: Keep current speed, normal fuel consumption
   - **Pit Stop**: Refuel to full capacity, resets speed to 0, takes more time

3. **Win Conditions**: Complete 5 laps before running out of fuel or time

4. **Lose Conditions**: 
   - Run out of fuel
   - Time expires (5 minutes total)

## How to Run the Project

### Prerequisites
- .NET 6.0 or later
- Visual Studio 2022 or Visual Studio Code with C# extension

### Running the Application
1. Clone the repository
2. Open the project in Visual Studio or navigate to the project directory
3. Build the solution: `dotnet build`
4. Run the application: `dotnet run`

### Running Tests
Execute unit tests with: `dotnet test`

## Game Features

- **WPF Interface**: Clean, responsive UI with progress bars and real-time updates
- **Turn-Based System**: Strategic decision-making with immediate feedback
- **Resource Management**: Balance fuel consumption with speed and time
- **Multiple Car Types**: Different strategies based on car characteristics
- **Real-Time Updates**: DispatcherTimer provides continuous time decay
- **Progress Tracking**: Visual lap progress indicator and percentage completion
- **Exception Handling**: Robust error handling for invalid actions

## Technical Implementation

### Architecture
- **Model-View Pattern**: Separation of game logic from UI
- **Object-Oriented Design**: Modular classes with single responsibilities
- **Data Structures**: Lists for car collection, enums for actions and car types
- **Exception Handling**: Custom exceptions for game state validation

### Key Classes
- `Car`: Manages vehicle properties and fuel consumption
- `Track`: Handles lap progression and race completion
- `RaceManager`: Coordinates game state and rule enforcement
- `MainWindow`: WPF interface and user interaction

## Project Structure
```
TimeBasedRacingGame/
├── Models/
│   ├── Car.cs
│   ├── Track.cs
│   └── RaceManager.cs
├── UnitTests/
│   ├── CarTests.cs
│   ├── TrackTests.cs
│   └── RaceManagerTests.cs
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── App.xaml
└── README.md
```

## Testing Strategy

The project includes comprehensive unit tests covering:
- Fuel consumption and refueling mechanics
- Lap progression and completion detection  
- Race initialization and action execution
- Exception handling for invalid operations

All tests use MSTest framework and can be run via `dotnet test` command.