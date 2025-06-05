# TagHunt Mobile App

## Overview
TagHunt is a location-based multiplayer mobile game built using .NET MAUI for iOS and Android platforms. The game brings the classic game of tag into the digital age, using GPS coordinates and real-time tracking for an exciting chase across real-world locations.

## Development Setup

### Prerequisites
- .NET 8.0 SDK or later
- Visual Studio 2022 or JetBrains Rider with .NET MAUI support
- For iOS development:
  - macOS with Xcode 13 or later
  - iOS development certificates and provisioning profiles
- For Android development:
  - Android SDK
  - Android emulator or physical device

### Building the Project
1. Clone the repository
2. Open the solution in your IDE
3. Restore NuGet packages
4. Build the project for your target platform (iOS or Android)

### Running the App
- **iOS**: 
  - Connect an iOS device or start an iOS simulator
  - Select the iOS target and run the project
- **Android**:
  - Connect an Android device or start an Android emulator
  - Select the Android target and run the project

## Project Structure
- `/Platforms/iOS/` - iOS-specific implementation
- `/Platforms/Android/` - Android-specific implementation
- `/Resources/` - Shared resources (images, fonts, etc.)
- `/Services/` - Core services and business logic
- `/Models/` - Data models
- `/ViewModels/` - MVVM view models
- `/Views/` - XAML views

## Technical Details
- Target Frameworks: 
  - net8.0-ios
  - net8.0-android
- Minimum OS Versions:
  - iOS: 11.0+
  - Android: API 21+

## Contributing
See the main [README.md](../README.md) in the root directory for contribution guidelines.

## License
This project is licensed under the MIT License - see the [LICENSE](../LICENSE) file for details.
