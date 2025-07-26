# Multi-User System Refactor

**Date**: January 2025
**Status**: Proposed
**Components**: Home Page, Save System, User Management

## Context and Problem

The current system has several limitations:
1. **Single User Experience** - Only one game progress can exist at a time
2. **Password-Based Login** - Unnecessary complexity for a local game with no authentication needs
3. **Hardcoded Save Paths** - All managers save to fixed file paths (`user://player_progression.json`)
4. **No User Discovery** - Cannot browse or switch between different game saves
5. **Poor Family/Shared Device Support** - Multiple players on same device must overwrite each other's progress

### Current Architecture Issues
- `LoginPage.cs` asks for username but doesn't persist or utilize it for save separation
- `BaseStatManager<TData>` implementations use hardcoded save file paths
- No mechanism to list existing users or create new ones
- Home page assumes single user context

## Design Decisions

### 1. Save System Architecture: File Prefix Approach (Option 1 vs Option 2)

#### **Decision**: Username-Prefixed File Names (Option 1)
Save files organized by username prefix:
```
user://andrew_player_progression.json
user://andrew_player_stats.json
user://andrew_player_inventory.json
user://andrew_quest_saves/quest_log.json

user://sarah_player_progression.json
user://sarah_player_stats.json
user://sarah_player_inventory.json
user://sarah_quest_saves/quest_log.json
```

**Rejected Alternative** (Option 2): JSON Field Approach
```json
{
  "username": "andrew",
  "progression": {...},
  "stats": {...}
}
```

**Rationale for File Prefix Approach**:
- **Complete Data Isolation** - Each user has entirely separate save files
- **Simple Implementation** - No need to modify existing JSON structures
- **Easy User Discovery** - Scan filesystem for username prefixes to list users
- **Backward Compatibility** - Existing saves can be migrated to default user
- **Performance** - No need to load all users' data to find one user
- **Reliability** - File corruption affects only one user, not all users

**Risks Mitigated**:
- **File System Pollution**: Acceptable tradeoff for simplicity and isolation
- **Duplicate Logic**: `UserManager` centralizes user operations

### 2. User Management System

#### **Decision**: Centralized UserManager Singleton
```csharp
public partial class UserManager : Node
{
    private string _currentUsername;

    public List<string> GetAllUsers()
    public void CreateNewUser(string username)
    public void SetCurrentUser(string username)
    public string GetCurrentUser()
    public bool UserExists(string username)
}
```

**Rationale**:
- **Single Source of Truth** - All user operations centralized
- **Global Accessibility** - Available to all managers as autoload singleton
- **Encapsulation** - Hides file system details from other components

#### **Decision**: Modified BaseStatManager for Dynamic Paths
```csharp
public abstract partial class BaseStatManager<TData> : Node where TData : class, new()
{
    protected override string SaveFilePath => GetUserPrefixedPath(BaseSaveFileName);
    protected abstract string BaseSaveFileName { get; }

    private string GetUserPrefixedPath(string baseFileName)
    {
        var userManager = GetNode<UserManager>("/root/UserManager");
        string username = userManager.GetCurrentUser();
        return $"user://{username}_{baseFileName}";
    }
}
```

**Benefits**:
- **Minimal Changes** - Existing managers only need to define `BaseSaveFileName`
- **Automatic Prefixing** - User prefix applied transparently
- **Maintains Polymorphism** - No breaking changes to existing inheritance hierarchy

### 3. Home Page UI/UX Design

#### **Decision**: User Selection → Home Flow
```
UserSelectionPage (new) → Home (existing) → Game Modes
     ↓
[User1 Card]
[User2 Card]
[User3 Card]
[+ Add User Button] → AddUserDialog → Creates user → Return to selection
```

**Component Architecture**:
- **`UserSelectionPage.cs`** - Main page with ScrollContainer
- **`UserCard.cs`** - Reusable component for each user row
- **`AddUserDialog.cs`** - Modal for new user creation

**Rationale**:
- **Familiar Pattern** - Common in games (Minecraft, Terraria save selection)
- **Visual Clarity** - Clear separation between user selection and gameplay
- **Extensible** - Easy to add features like user deletion, renaming
- **Touch-Friendly** - Large clickable areas for each user

#### **Decision**: Replace Password Login System
**Removed**: `LoginPage.cs` password input
**Added**: Visual user cards with labels showing username

**Rationale**:
- **Local Game Context** - No network security concerns
- **User Experience** - Faster access, no typing required
- **Family-Friendly** - Kids can recognize their name visually
- **Reduced Complexity** - Eliminates unnecessary authentication layer

### 4. Data Migration Strategy

#### **Decision**: No Legacy Data Migration
**Removed**: Automatic migration functionality for existing save files

**Rationale**:
- **Clean Implementation** - No legacy code complexity
- **Fresh Start** - All users begin with new user-prefixed save system
- **Simplified Maintenance** - No migration edge cases to handle
- **Clear Separation** - Complete break from old save file format

## Technical Implementation Benefits

### 1. System Integration
- **Leverages Existing Patterns** - Uses established `BaseStatManager` inheritance
- **Minimal Disruption** - Existing game logic unchanged
- **Autoload Integration** - `UserManager` joins existing global managers

### 2. Maintainability
- **Single Responsibility** - Each component has clear role
- **Encapsulation** - User management logic centralized
- **Testability** - Components can be unit tested independently

### 3. Performance
- **Lazy Loading** - Only current user's data loaded into memory
- **File System Efficiency** - Direct file access without parsing multiple users
- **Memory Footprint** - No change to memory usage patterns

### 4. User Experience
- **Immediate Feedback** - User list loads instantly
- **Visual Recognition** - No typing required for user selection
- **Error Prevention** - Invalid usernames rejected at creation time
- **Discoverability** - All available users visible at once

## Risks and Mitigations

### Risk: File System Pollution
**Mitigation**: Accept tradeoff for simplicity. Future cleanup tools possible if needed.

### Risk: Username Conflicts
**Mitigation**: Validation at user creation prevents duplicate usernames.

### Risk: Save File Corruption
**Mitigation**: Individual user isolation means corruption affects only one user.

### Risk: Migration Failures
**Mitigation**: Backup existing files before migration, provide error logging.

## Future Considerations

### Potential Enhancements
- **User Avatars**: Visual customization for user cards
- **Last Played Timestamps**: Show when each user last played
- **User Deletion**: Safe removal of user data
- **Import/Export**: Backup and restore individual user data
- **User Statistics**: Summary stats on user selection screen

### Scalability
- **Directory Organization**: If users become numerous, consider user subdirectories
- **Database Migration**: System designed to eventually support SQLite if needed
- **Cloud Saves**: Architecture supports future cloud save integration

This design provides a solid foundation for multi-user support while maintaining simplicity and leveraging existing system patterns.
