# TagHunt - Product Requirements Document

## Development Workflow Rules
1. Code Changes: Direct file changes and edits are allowed without explicit permission
2. Git Commits: **MUST** ask for approval before committing or pushing changes to GitHub

## Project Overview
Create a location-based multiplayer mobile game called "TagHunt" - a real-world tag game using GPS coordinates.

## Progress Tracking
Last Updated: Phase 1 Setup
### Completed Tasks
1. **Initial Project Setup**
   - ✅ Project structure created with recommended folders (ViewModels/, Views/, Services/, Models/)
   - ✅ Firebase project configuration completed
   - ✅ Android package name identified: com.companyname.ai_test_1
   - ✅ iOS bundle ID identified: com.companyname.ai_test_1
   - ✅ Map integration planned (Microsoft.Maui.Controls.Maps)

### Next Steps
1. **User Authentication System** (NEXT UP)
   - [ ] Create user authentication models and services
   - [ ] Build login/registration UI
   - [ ] Implement Firebase Authentication
   - [ ] Set up user profile structure in Firebase

2. **Location Sharing System** (PENDING)
   - [ ] Implement location sharing request system
   - [ ] Build UI for managing requests
   - [ ] Set up Firebase real-time updates

3. **Map Integration** (PENDING)
   - [ ] Implement GPS location access
   - [ ] Add map display
   - [ ] Set up real-time location updates

4. **Friend Management** (PENDING)
   - [ ] Implement contact management
   - [ ] Build UI for managing connections

### Current Status
- Environment: .NET MAUI project with Firebase
- Development Phase: Phase 1 (Basic Proof of Concept)
- Current Focus: Preparing to implement User Authentication

## Phase 1: Basic Proof of Concept
Create a simple location-sharing app with the following core features:

### Core Features
1. **User Registration/Login System**
   - Username-based user search
   - Secure authentication
   - Basic user profiles

2. **Location Sharing Request System**
   - Send/receive location sharing requests
   - Accept/deny incoming requests
   - Permission-based location sharing (explicit consent required)

3. **Real-time GPS Location Display**
   - Map integration (using native platform maps)
   - Real-time location updates
   - User location display on map
   - Permission-based visibility

4. **Basic Friend/Contact Management**
   - View active sharing connections
   - Manage sharing permissions
   - User search functionality

### Technical Requirements for POC
- **Platform:** .NET MAUI mobile application
- **Communication:** Firebase Realtime Database for client communication
- **Authentication:** Firebase Authentication
- **Map Integration:** Microsoft.Maui.Controls.Maps
- **Local Storage:**
  - SecureStorage for authentication tokens
  - Preferences for user settings
- **Permissions:** Location and internet access

## Phase 2: Full TagHunt Game

### Core Game Concept
A multiplayer location-based tag game where players physically move in the real world within a defined geographic boundary.

### Game Features

#### 1. Lobby System
- Players join game lobbies before starting
- Lobby creator sets game parameters
- Minimum/maximum player limits
- Game area selection on map

#### 2. Player Roles
- **Hunters:** One or more players who start as "it"
- **Hunted:** Remaining players who must avoid being caught
- **Role switching:** When caught, hunted players become hunters OR are eliminated (configurable)

#### 3. Game Mechanics
- **Capture System:** Hunter must stay within Y radius of hunted player for X seconds to "tag"
- **Geographic Boundaries:** Players cannot leave predefined game area
- **Time Limit:** Configurable game duration
- **Win Conditions:**
  - Hunters win when all players are caught
  - Hunted win when time expires with uncaught players remaining

#### 4. Tracking & Radar System
- **Hunter View:** Alien-style radar showing direction to hunted players
- Real-time GPS tracking within game boundaries
- Boundary enforcement with alerts/penalties

#### 5. Power-ups
- **Hunter Power-ups:**
  - Full map view (temporary)
  - Speed boost
  - Extended capture radius
- **Hunted Power-ups:**
  - Invisibility (temporary radar hiding)
  - Decoy creation
  - Speed boost
  - Temporary boundary expansion

### Technical Architecture Requirements
- **Backend:** Firebase Realtime Database
- **Frontend:** .NET MAUI mobile app
- **Location Services:** High-accuracy GPS tracking
- **Real-time Communication:** Firebase real-time updates
- **Map Services:** Microsoft.Maui.Controls.Maps
- **Push Notifications:** Game alerts and updates

### Privacy & Safety Considerations
- Explicit location sharing consent
- Game area restrictions (no private property)
- Player safety features
- Data encryption
- User blocking/reporting system

### User Interface Requirements
- Clean, intuitive mobile interface
- Real-time map display
- Radar visualization for hunters
- Game statistics and leaderboards
- Lobby management interface
- Settings for accessibility and safety

## Development Approach
1. Start with basic location sharing POC
2. Add real-time multiplayer infrastructure
3. Implement core game mechanics
4. Add power-up system
5. Polish UI/UX
6. Implement safety and privacy features
7. Beta testing and refinement

## Success Metrics
- User engagement and retention
- Game completion rates
- User safety (zero incidents)
- Technical performance (low latency, accurate GPS)
- Positive user feedback 