# 🧠 GuessGame - WPF C# Application

A modern implementation of the classic **Memory game** using **C#** and **WPF (Windows Presentation Foundation)**, developed with **MVVM architecture** and **data binding**. This project blends gameplay, user management, game persistence, and statistics into an interactive and user-friendly application.

Inspired by: [webgamesonline.com/memory](https://www.webgamesonline.com/memory/)

## 📌 Features

### 🧑 User Management
- **Sign In/Create Account** screen
- Each user is associated with a **profile picture** (JPG, PNG, or GIF)
- User data is stored using **relative paths** to ensure portability
- Data is saved using a convenient format (JSON/XML/Text)
- **Delete User** functionality removes all associated data:
  - Profile image link
  - Saved games
  - Game statistics

### 🎮 Gameplay
- Memory game with card-matching mechanics
- Randomized card arrangement every new game
- **Two modes**:
  - **Standard**: 4x4 grid
  - **Custom**: MxN grid (from 2x2 to 6x6) with even number of cards
- Selectable **image category** (3 predefined sets)
- **Time-limited gameplay**: user sets a countdown timer before starting
- Cards flip and match logic:
  - Matching cards become inactive or disappear
  - Non-matching cards flip back after 2 cards are shown
- Prevents selecting the same card twice consecutively

### 📀 Game Persistence
- Save and Load games with:
  - Selected image category
  - Card arrangement
  - Remaining time
  - Elapsed time
- One saved game per user (or multiple — developer choice)
- Only the original user can load their saved game

### 📊 Statistics
- Tracks total games played and games won per user
- Statistics window displays all users in the format:
  ```
  Username – Games Played – Games Won
  ```

### 🛠 Menu Options
- **File**
  - `Category` – Choose card image category
  - `New Game` – Start a fresh game
  - `Open Game` – Load previously saved game
  - `Save Game` – Save current game progress
  - `Statistics` – View all users' gameplay stats
  - `Exit` – Return to login screen

- **Options**
  - `Standard` – 4x4 board
  - `Custom` – User-defined grid (M x N)

- **Help**
  - `About` – Displays developer information (name, email, group, specialization)

## 🏠 Architecture & Technologies

- **C# with WPF (Windows Presentation Foundation)**
- **MVVM Design Pattern** – clean separation of UI, logic, and data
- **Data Binding** – seamless UI and data synchronization
- **Serialization** – saving/loading data with text/JSON/XML formats

## 📁 Project Structure (Actual)

```
/GuessGame/
│
├── Models/                  # Data classes for User and Game Save
│   ├── GameSave.cs
│   └── User.cs
│
├── Resources/               # Static images and game assets
│   ├── Avatars/             # User profile pictures
│   └── GameImages/          # Card images by category
│       ├── Animals/
│       ├── Flowers/
│       └── Numbers/
│
├── Services/                # Business logic and persistence
│   ├── GameSaveService.cs
│   ├── StatisticsService.cs
│   └── UserDataService.cs
│
├── ViewModels/              # MVVM ViewModel layer
│   ├── GameViewModel.cs
│   └── LoginViewModel.cs
│
├── Views/                   # UI views and their code-behind
│   ├── GameWindow.xaml
│   ├── GameWindow.xaml.cs
│   ├── LoadGameWindow.xaml
│   ├── LoadGameWindow.xaml.cs
│   ├── LoginView.xaml
│   └── LoginView.xaml.cs
│
├── App.xaml                 # Application startup and resources
├── App.xaml.cs
├── MainWindow.xaml
├── RelayCommand.cs          # MVVM helper for ICommand
├── AssemblyInfo.cs
└── users.json               # User data persistence (JSON format)
```

## ✅ Requirements
- .NET Framework or .NET Core (WPF compatible)
- Visual Studio 2019/2022 or newer
- Windows OS (WPF-based)
---

**Let the memory challenge begin! 🧠🂏**
