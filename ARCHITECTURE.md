# Architecture Document - C# Speed Rush

## Application Architecture

### Design Pattern: Model-View Pattern
The application follows a Model-View architectural pattern with clear separation of concerns:

- **Models**: Core business logic (Car, Track, RaceManager)
- **View**: WPF UI layer (MainWindow.xaml/cs)
- **No Controller**: Direct event handling in code-behind for simplicity

### Class Diagram
```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│      Car        │    │     Track       │    │  RaceManager    │
├─────────────────┤    ├─────────────────┤    ├─────────────────┤
│ + Type          │    │ + TotalLaps     │    │ + AvailableCars │
│ + Name          │    │ + CurrentLap    │    │ + SelectedCar   │
│ + MaxSpeed      │    │ + LapProgress   │    │ + Track         │
│ + CurrentSpeed  │    │ + LapDistance   │    │ + TimeRemaining │
│ + CurrentFuel   │    ├─────────────────┤    │ + IsRaceActive  │
│ + MaxFuel       │    │ + AdvanceProgress│   ├─────────────────┤
├─────────────────┤    │ + IsCompleted   │    │ + StartRace     │
│ + ConsumeFuel   │    │ + GetProgress   │    │ + ExecuteAction │
│ + Refuel        │    │ + GetIndicator  │    │ + CheckConditions│
│ + GetFuelPct    │    └─────────────────┘    └─────────────────┘
└─────────────────┘              │                      │
        │                        │                      │
        └────────────────────────┼──────────────────────┘
                                 │
                    ┌─────────────────┐
                    │   MainWindow    │
                    ├─────────────────┤
                    │ - raceManager   │
                    │ - gameTimer     │
                    ├─────────────────┤
                    │ + UpdateUI      │
                    │ + HandleEvents  │
                    └─────────────────┘
```

## Data Structure Justification

### List<Car> for Available Cars
- **Why**: Dynamic collection of car objects with easy iteration
- **Alternative**: Array - rejected due to fixed size limitation
- **Benefit**: Supports LINQ operations and easy binding to ComboBox

### Enum for CarType and RaceAction
- **Why**: Type-safe constants with clear semantic meaning
- **Alternative**: String constants - rejected due to no compile-time checking
- **Benefit**: IntelliSense support and prevents invalid values

### Properties with Getters/Setters
- **Why**: Encapsulation with controlled access to internal state
- **Alternative**: Public fields - rejected due to no validation capability
- **Benefit**: Future extensibility for validation and change notifications

## Programming Patterns Used

### 1. Factory Pattern (Implicit)
```csharp
// RaceManager constructor creates predefined car instances
AvailableCars = new List<Car>
{
    new Car(CarType.SportsCar, "Lightning Bolt", 120, 8.0, 60),
    new Car(CarType.EcoCar, "Green Machine", 80, 4.0, 80),
    new Car(CarType.RaceCar, "Speed Demon", 150, 12.0, 50)
};
```

### 2. State Pattern (Implicit)
- Race states: Not Started → Active → Completed/Failed
- Managed through `IsRaceActive` boolean and `RaceResult` string

### 3. Observer Pattern (WPF Events)
- UI automatically updates when user actions trigger state changes
- DispatcherTimer provides periodic updates

## Technical Decisions

### WPF over Console Application
- **Reason**: Assignment requires form-based interface
- **Benefit**: Rich UI with progress bars, buttons, and real-time updates

### DispatcherTimer for Real-Time Updates
- **Reason**: Simulates continuous time passage during race
- **Alternative**: Manual updates only - rejected for poor user experience
- **Implementation**: 1-second intervals with 0.5-second time decay

### Exception Handling Strategy
- **Custom Exceptions**: InvalidOperationException for business rule violations
- **User-Friendly Messages**: MessageBox displays for user errors
- **Graceful Degradation**: Game continues after handling exceptions

### Testing Framework Choice
- **MSTest**: Chosen for Visual Studio integration and simplicity
- **Alternative**: NUnit/xUnit - rejected due to additional dependencies
- **Coverage**: 9 tests covering critical paths and edge cases

## Performance Considerations

### Memory Management
- Minimal object creation during gameplay
- Reuse of existing car instances
- No memory leaks from event handlers

### UI Responsiveness
- Timer operations on UI thread (acceptable for simple game)
- Immediate UI updates after user actions
- No blocking operations during gameplay

## Scalability & Extensibility

### Easy Extensions
- Add new car types by extending CarType enum
- Add new actions by extending RaceAction enum
- Modify race parameters through constructor parameters

### Potential Improvements
- Implement MVVM pattern for better testability
- Add configuration file for car specifications
- Implement save/load game state functionality
- Add multiplayer support through network communication