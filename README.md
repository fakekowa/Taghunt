# TagHunt 🎯

TagHunt is an innovative location-based multiplayer mobile game that brings the classic game of tag into the digital age. Using GPS coordinates and real-time tracking, players engage in an exciting chase across real-world locations, switching between hunter and hunted roles in this dynamic and interactive experience.

## Project Progress

### Phase 1: Basic Proof of Concept
```
Overall Progress     [============>........] 70%

✅ Project Setup & Infrastructure
├── ✅ .NET MAUI project structure
├── ✅ Firebase project configuration  
├── ✅ Dependency injection setup
├── ✅ Custom behaviors (SafeAreaBehavior)
├── ✅ Theme & styling system
└── ✅ Cross-platform configuration

✅ User Authentication System
├── ✅ Firebase Authentication service
├── ✅ Login page with validation
├── ✅ Registration page with validation
├── ✅ AuthViewModel with error handling
├── ✅ Secure token storage
└── ✅ Auto-login functionality

✅ Core UI Components
├── ✅ Dashboard/Home screen
├── ✅ Account Settings page
├── ✅ Modern UI with safe area handling
├── ✅ Theme-aware styling (light/dark)
├── ✅ Responsive layouts
└── ✅ Cross-platform compatibility

🔄 Backend Services
├── ✅ Firebase Realtime Database service
├── ✅ Location services implementation
├── ✅ Configuration management
├── ✅ User profile management
├── 🔄 Location sharing system (in progress)
└── ⏳ Friend management (pending)

🔄 Data Models
├── ✅ User model
├── ✅ Location model
├── ✅ Sharing request model
├── ✅ Game model (for future phases)
└── ✅ Firebase configuration model

⏳ Map Integration [=====>...............] 25%
├── ✅ Location services setup
├── ⏳ Map display implementation
├── ⏳ Real-time location updates
└── ⏳ Location sharing visualization

⏳ Friend Management [...................] 0%
├── ⏳ Friends list screen
├── ⏳ Add friend functionality
├── ⏳ Friend requests system
└── ⏳ Friend profile screens
```

### Phase 2: Full Game Implementation
```
Phase 2 Progress    [...................] 0%

⏳ Game Mechanics
├── ⏳ Lobby system
├── ⏳ Hunter/Hunted roles
├── ⏳ Capture mechanics
├── ⏳ Geographic boundaries
└── ⏳ Game timer system

⏳ Power-up System
├── ⏳ Hunter abilities
├── ⏳ Hunted abilities
├── ⏳ Power-up distribution
└── ⏳ Visual effects

⏳ Multiplayer Features
├── ⏳ Real-time game sessions
├── ⏳ Player synchronization
├── ⏳ Game state management
└── ⏳ Leaderboards
```

## 🎮 Game Features

### Core Gameplay
- **Dynamic Role System**: Players switch between Hunter and Hunted roles
- **Real-time GPS Tracking**: Live location updates within game boundaries
- **Capture Mechanics**: Proximity-based tagging system
- **Geographic Boundaries**: Defined play areas with enforcement
- **Time-based Matches**: Configurable game duration and win conditions

### Power-up System
- **Hunter Abilities**:
  - Temporary full map visibility
  - Speed boosts
  - Extended capture radius
- **Hunted Abilities**:
  - Temporary invisibility
  - Decoy deployment
  - Speed boosts
  - Boundary expansion

### Multiplayer Features
- **Lobby System**: Create and join game sessions
- **Customizable Parameters**: Adjust player limits and game settings
- **Real-time Communication**: Instant updates and player tracking
- **Leaderboards**: Track performance and statistics

## 🛡️ Privacy & Safety

TagHunt is built with user privacy and safety as top priorities:
- Explicit consent required for location sharing
- Restricted game areas (no private property)
- Built-in safety features and alerts
- Secure data encryption
- User reporting system
- Comprehensive blocking capabilities

## 🔧 Technical Stack

- **Frontend**: .NET MAUI mobile application
- **Backend**: Firebase Realtime Database
- **Authentication**: Firebase Authentication
- **Maps**: Microsoft.Maui.Controls.Maps
- **Location Services**: High-accuracy GPS tracking
- **Push Notifications**: Real-time game alerts

## 🚀 Current Implementation Status

### ✅ Completed Features
- **User Authentication**: Full Firebase auth implementation with secure storage
- **Modern UI**: Theme-aware interface with safe area handling
- **Dashboard**: Main application hub with user profile management
- **Account Settings**: User profile editing and preference management
- **Backend Services**: Firebase integration for database and authentication
- **Cross-platform Support**: iOS and Android compatibility

### 🔄 In Progress
- **Location Sharing**: Basic location services implemented, UI in development
- **Map Integration**: Location services ready, map display implementation pending

### ⏳ Next Steps
1. **Complete Map Integration**
   - Implement map display with Microsoft.Maui.Controls.Maps
   - Add real-time location updates and sharing visualization
   - Create location request management system

2. **Friend Management System**
   - Build friend list and search functionality
   - Implement friend request system
   - Create friend profile screens

3. **Polish Phase 1 Features**
   - Add comprehensive error handling
   - Implement offline support
   - Add push notifications for location requests

## 🔜 Coming Soon

- Comprehensive game tutorials
- Advanced power-up mechanics
- Team-based game modes
- Special events and challenges
- Cross-platform support
- Social features and achievements

## 📱 Platform Support

- iOS
- Android

## 🤝 Contributing

We welcome contributions! If you're interested in improving TagHunt, please:
1. Fork the repository
2. Create a feature branch
3. Submit a pull request with your changes

## 📄 License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details. This license was chosen to be compatible with:
- .NET MAUI (MIT License)
- Firebase .NET SDK (MIT License)
- Other NuGet dependencies (all MIT-compatible)

---

*TagHunt is currently in development. Features and specifications are subject to change as we continue to enhance and refine the gaming experience.* 