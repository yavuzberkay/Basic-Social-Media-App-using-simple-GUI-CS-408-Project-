# CLAUDE.md - Project Context for Claude Code

## Project Overview

This is a **Basic Social Media Application** (called "Sweeter") built with C# Windows Forms. It uses a client-server architecture with TCP sockets for real-time communication. Users can post "sweets" (like tweets), follow other users, block users, and view feeds.

## Project Structure

```
├── client/                    # Client application
│   ├── client.sln            # Visual Studio solution file
│   └── client/
│       ├── Form1.cs          # Main client UI and socket logic
│       ├── Form1.Designer.cs # Windows Forms designer code
│       ├── Program.cs        # Entry point
│       └── user-db.txt       # User database (usernames)
│
├── server/                    # Server application
│   ├── server.sln            # Visual Studio solution file
│   └── server/
│       ├── Form1.cs          # Main server logic and socket handling
│       ├── Form1.Designer.cs # Windows Forms designer code
│       ├── Program.cs        # Entry point
│       ├── sweets.txt        # Stored messages/posts
│       ├── following.txt     # User follow relationships
│       └── blocked.txt       # User block relationships
```

## Technology Stack

- **Language**: C# (.NET Framework)
- **UI Framework**: Windows Forms
- **Networking**: TCP Sockets (`System.Net.Sockets`)
- **Data Storage**: Plain text files

## Build & Run

### Using Visual Studio
1. Open `server/server.sln` in Visual Studio
2. Build and run the server (F5)
3. Open `client/client.sln` in a separate Visual Studio instance
4. Build and run the client (F5)

### Using Command Line (MSBuild)
```bash
# Build server
msbuild server/server.sln

# Build client
msbuild client/client.sln
```

## Key Features

- **Authentication**: Users log in with a username from `user-db.txt`
- **Sweets**: Post messages (max 64 characters)
- **Follow/Unfollow**: Follow other users to see their sweets
- **Block**: Block users to hide their content and prevent them from following you
- **Feed Views**: All sweets, followed users' sweets, or own sweets
- **Delete**: Remove own sweets by ID

## Communication Protocol

Client-server communication uses simple string messages:
- `"I want to disconnect"` - Client disconnect request
- `"I want all the sweets"` - Request all sweets feed
- `"I want all the users"` - Request user list
- `"I want to follow <username>"` - Follow a user
- `"I want to block <username>"` - Block a user
- `"I want all the followed sweets"` - Request followed users' sweets
- `"I want all my followers"` - Request followers list
- `"I want all following"` - Request following list
- `"I want all my sweets"` - Request own sweets
- `"I want to delete <sweet_id>"` - Delete a sweet
- Any other message is treated as a new sweet

## Data File Formats

- **user-db.txt**: One username per line
- **sweets.txt**: `Username: <user> Sweet: <message> Sweet Id: <id> Time: <timestamp>`
- **following.txt**: `<username> <followed1> <followed2> ...`
- **blocked.txt**: `<username> <blocked1> <blocked2> ...`

## Important Notes

- Buffer size is 64-128 bytes for messages
- Server listens on a configurable port (default max 6 connections)
- File paths in server code are hardcoded and may need updating for your environment
- No encryption - this is an educational project
