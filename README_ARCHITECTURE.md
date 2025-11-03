# Bingo MAUI App - Architecture Overview

## Project Structure

This Bingo app follows the MVVM (Model-View-ViewModel) pattern with the following structure:

### Models
- **BingoBoard.cs**: Core data model containing board properties (Id, Name, Size, Content, Marked state)

### Services
- **IBingoBoardService.cs**: Interface defining board persistence operations
- **BingoBoardService.cs**: Implementation using JSON file storage in app data directory
  - Stores all boards in `bingoboards.json`
  - Supports CRUD operations (Create, Read, Update, Delete)

### ViewModels
- **MainPageViewModel.cs**: Manages the list of saved boards
- Commands: AddNewBoard, BoardTapped, Refresh
  - Loads boards on initialization
  
- **NewBoardPageViewModel.cs**: Handles board creation
  - Dynamically generates cell input fields based on size
  - Validates that all fields are filled before enabling Create button
  - Creates board and navigates to BoardViewPage
  
- **BoardViewPageViewModel.cs**: Manages board display and interaction
  - Loads board data by ID from query parameters
  - Handles cell toggling and persists marked state
  - Contains nested BingoCellViewModel for individual cell state

### Views
- **MainPage.xaml**: Displays list of saved boards with "Add New" button
- **NewBoardPage.xaml**: Form for creating new boards (name, size, cell values)
- **BoardViewPage.xaml**: Interactive grid of toggle buttons for playing bingo

### Navigation
- Uses Shell navigation with registered routes:
  - `MainPage` (default)
  - `NewBoardPage`
  - `BoardViewPage` (accepts boardId query parameter)

### Dependency Injection
All services, ViewModels, and Views are registered in `MauiProgram.cs` for DI.

## Features Implemented

1. ? Main page with "Add New" button and list of saved boards
2. ? New board page with name, size, and dynamic cell input fields
3. ? Board view page with interactive grid of toggle buttons
4. ? Persistent storage using JSON files (survives app restarts)
5. ? MVVM architecture with proper separation of concerns
6. ? Visual feedback (marked cells turn light green)

## How to Use

1. **Create a Board**: Tap "Add New Board" ? Enter name and size ? Fill all cell values ? Tap "Create Board"
2. **View Boards**: Return to main page to see all saved boards
3. **Play Bingo**: Tap on a board ? Tap cells to mark them (they turn green)
4. **Persistence**: All boards and their marked states are automatically saved

## Data Storage Location
Boards are stored at: `FileSystem.AppDataDirectory/bingoboards.json`
