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

## Phase 1 Views Specification & Checklist

### Global Appearance Requirements (All Views)
**Full Screen Utilization:**
- [ ] App must utilize full screen height and width on all devices
- [ ] No black/empty areas at top or bottom of screen
- [ ] Proper handling of device safe areas (notches, status bars)
- [ ] Responsive layout that adapts to different screen sizes
- [ ] Content should fill available screen real estate appropriately

**Cross-Platform Compatibility:**
- [ ] UI elements must render correctly on iOS and Android
- [ ] Consistent layout behavior across different screen densities
- [ ] Proper scaling for tablets and phones
- [ ] Handle landscape and portrait orientations
- [ ] Account for platform-specific UI conventions (iOS safe areas, Android navigation bars)

**MAUI Layout Requirements:**
- [ ] Use proper MAUI layout containers (Grid, StackLayout, etc.)
- [ ] Set appropriate VerticalOptions and HorizontalOptions (Fill, FillAndExpand)
- [ ] Configure proper margins and padding for platform consistency
- [ ] Implement proper view sizing and constraints
- [ ] Handle keyboard appearance/disappearance gracefully

### 1. Authentication Views

#### 1.1 Welcome/Splash Screen
**Content:**
- [ ] App logo and branding
- [ ] App name "TagHunt"
- [ ] Loading indicator
- [ ] Automatic navigation to login/registration

**Functionality:**
- [ ] Check for existing authentication token
- [ ] Auto-navigate to main app if logged in
- [ ] Navigate to login screen if not authenticated
- [ ] Handle app initialization and setup
- [ ] Show loading states during Firebase connection

#### 1.2 Login Screen
**Content:**
- [ ] Email/username input field
- [ ] Password input field
- [ ] "Login" button
- [ ] "Forgot Password?" link
- [ ] "Don't have an account? Sign up" navigation
- [ ] Social login options (Google, Apple - optional)

**Functionality:**
- [ ] Input validation (email format, required fields)
- [ ] Firebase Authentication integration
- [ ] Remember login option
- [ ] Error handling with user-friendly messages
- [ ] Loading states during authentication
- [ ] Keyboard handling and input focus management
- [ ] Navigate to main app on successful login

#### 1.3 Registration Screen
**Content:**
- [ ] Full name input field
- [ ] Email input field
- [ ] Username input field (unique)
- [ ] Password input field
- [ ] Confirm password input field
- [ ] Terms of service checkbox
- [ ] Privacy policy checkbox
- [ ] "Create Account" button
- [ ] "Already have an account? Login" navigation

**Functionality:**
- [ ] Real-time input validation
- [ ] Username availability checking
- [ ] Password strength validation
- [ ] Email verification process
- [ ] Firebase user creation
- [ ] Profile creation in Firestore
- [ ] Auto-login after successful registration
- [ ] Error handling for duplicate accounts

### 2. Main Dashboard/Home Screen

#### 2.1 Dashboard View
**Content:**
- [ ] User avatar and welcome message
- [ ] Location sharing status indicator
- [ ] Quick action buttons (Share Location, Find Friends)
- [ ] Active location sharing sessions list
- [ ] Recent activity feed
- [ ] Navigation menu access

**Functionality:**
- [ ] Real-time updates from Firebase
- [ ] Quick access to main app features
- [ ] Location permission status checking
- [ ] Navigation to detailed views
- [ ] Pull-to-refresh functionality
- [ ] Handle offline states gracefully

### 3. Location Sharing Views

#### 3.1 Share Location Screen
**Content:**
- [ ] Friend selection list/search
- [ ] Time limit selector (1hr, 4hrs, 8hrs, unlimited)
- [ ] Privacy settings toggle
- [ ] Share location button
- [ ] Current location preview
- [ ] Active sharing sessions list

**Functionality:**
- [ ] Friend list from Firebase
- [ ] Multiple friend selection
- [ ] GPS location access and display
- [ ] Time limit configuration
- [ ] Send sharing requests via Firebase
- [ ] Confirmation dialogs
- [ ] Loading states and success feedback

#### 3.2 Location Requests Screen
**Content:**
- [ ] Incoming requests list
- [ ] Request details (sender, duration, timestamp)
- [ ] Accept/Deny action buttons
- [ ] Outgoing requests list
- [ ] Request status indicators
- [ ] Cancel request option

**Functionality:**
- [ ] Real-time request updates from Firebase
- [ ] Accept/deny request handling
- [ ] Request cancellation
- [ ] Push notifications for new requests
- [ ] Update request status in database
- [ ] Navigate to map view on acceptance

### 4. Map Views

#### 4.1 Main Map Screen
**Content:**
- [ ] Interactive map with user locations
- [ ] Current user location marker
- [ ] Friend location markers (with avatars)
- [ ] Location accuracy indicators
- [ ] Map controls (zoom, locate me)
- [ ] Bottom sheet with location details

**Functionality:**
- [ ] Real-time location updates
- [ ] Map centering and zoom controls
- [ ] Location sharing session management
- [ ] Marker tap interactions
- [ ] Location permission handling
- [ ] Offline map caching (optional)
- [ ] Battery optimization for location tracking

#### 4.2 Location Detail View
**Content:**
- [ ] Selected user's information
- [ ] Location sharing duration remaining
- [ ] Distance from current user
- [ ] Last updated timestamp
- [ ] Stop sharing button
- [ ] Message/call action buttons (future)

**Functionality:**
- [ ] Real-time duration countdown
- [ ] Distance calculation and updates
- [ ] Stop sharing functionality
- [ ] Return to map on dismissal
- [ ] Handle sharing session expiration

### 5. Friend Management Views

#### 5.1 Friends List Screen
**Content:**
- [ ] Search bar for friend filtering
- [ ] Friends list with avatars and names
- [ ] Online/offline status indicators
- [ ] Add friend button
- [ ] Friend management actions (remove, block)
- [ ] Empty state for no friends

**Functionality:**
- [ ] Real-time friend status updates
- [ ] Friend search and filtering
- [ ] Add friend functionality
- [ ] Remove/block friend actions
- [ ] Navigate to friend profile
- [ ] Sync with Firebase friends collection

#### 5.2 Add Friend Screen
**Content:**
- [ ] Username search input
- [ ] Search results list
- [ ] User profile previews
- [ ] Send friend request button
- [ ] Pending requests status
- [ ] Recent searches/suggestions

**Functionality:**
- [ ] Username search in Firebase
- [ ] Send friend requests
- [ ] Handle duplicate requests
- [ ] Show request status
- [ ] Navigate back on success
- [ ] Real-time search results

#### 5.3 Friend Profile Screen
**Content:**
- [ ] Friend's avatar and name
- [ ] Current sharing status
- [ ] Sharing history/statistics
- [ ] Actions menu (share location, remove friend)
- [ ] Block/report options
- [ ] Last seen information

**Functionality:**
- [ ] Load friend data from Firebase
- [ ] Quick location sharing initiation
- [ ] Friend management actions
- [ ] Navigate to sharing screens
- [ ] Update friend data in real-time

### 6. Settings & Profile Views

#### 6.1 User Profile Screen
**Content:**
- [ ] User avatar with edit option
- [ ] Username and email display
- [ ] Profile statistics
- [ ] Account settings access
- [ ] Privacy settings toggle
- [ ] Logout button

**Functionality:**
- [ ] Avatar image selection and upload
- [ ] Profile information editing
- [ ] Settings navigation
- [ ] Logout with confirmation
- [ ] Profile data sync with Firebase

#### 6.2 Settings Screen
**Content:**
- [ ] Location sharing preferences
- [ ] Notification settings
- [ ] Privacy controls
- [ ] App theme selection
- [ ] About/help section
- [ ] Account management

**Functionality:**
- [ ] Settings persistence with Preferences
- [ ] Real-time setting updates
- [ ] Permission management
- [ ] Theme switching
- [ ] Navigate to detailed setting screens

### Implementation Priority Checklist
**Phase 1.1 - Core Authentication**
- [ ] Welcome/Splash Screen
- [ ] Login Screen
- [ ] Registration Screen
- [ ] Basic Dashboard

**Phase 1.2 - Location Features**
- [ ] Share Location Screen
- [ ] Location Requests Screen
- [ ] Main Map Screen
- [ ] Location permissions handling

**Phase 1.3 - Friend Management**
- [ ] Friends List Screen
- [ ] Add Friend Screen
- [ ] Friend Profile Screen

**Phase 1.4 - Polish & Settings**
- [ ] User Profile Screen
- [ ] Settings Screen
- [ ] Error handling and loading states
- [ ] UI/UX refinements

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