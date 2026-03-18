# CLAUDE.md - Project Context for Claude Code

## What is CLAUDE.md?

This file is automatically read by Claude Code at the start of every session. It gives Claude the context it needs to understand your project — like a "project briefing" — so you don't have to re-explain things each time you ask for help.

---

## Project Overview

This is a **Basic Social Media Application** called **"Sweeter"**, built as a CS 408 course project. It works like a simplified version of Twitter: users post short messages called "sweets", can follow and block other users, and view different feeds.

The application is split into **two separate programs** that communicate over a network:
- A **Server** — manages all data, handles business logic, and coordinates connected users
- A **Client** — provides the GUI that a user interacts with

Both programs are Windows desktop applications built with **C# and Windows Forms**.

---

## Project Structure

```
├── client/                        # The client-side application (what users see)
│   ├── client.sln                # Visual Studio solution file — open this to work on the client
│   └── client/
│       ├── Form1.cs              # All client logic: connecting, sending messages, button handlers
│       ├── Form1.Designer.cs     # Auto-generated UI layout code (do not edit manually)
│       ├── Form1.resx            # UI resource file (icons, strings, etc.)
│       ├── Program.cs            # App entry point — just launches Form1
│       ├── client.csproj         # Project configuration file
│       ├── user-db.txt           # List of valid usernames (shared knowledge with server)
│       └── Properties/           # Assembly metadata, settings, resources
│
├── server/                        # The server-side application (runs in the background)
│   ├── server.sln                # Visual Studio solution file — open this to work on the server
│   └── server/
│       ├── Form1.cs              # All server logic: accepting connections, handling requests, file I/O
│       ├── Form1.Designer.cs     # Auto-generated UI layout code (do not edit manually)
│       ├── Form1.resx            # UI resource file
│       ├── Program.cs            # App entry point — just launches Form1
│       ├── server.csproj         # Project configuration file
│       ├── sweets.txt            # Database of all posted sweets (appended over time)
│       ├── following.txt         # Tracks who follows whom
│       ├── blocked.txt           # Tracks who has blocked whom
│       └── Properties/           # Assembly metadata, settings, resources
│
└── README.md                     # Brief project description
```

---

## Technology Stack

| Layer | Technology | Details |
|-------|-----------|---------|
| Language | C# | .NET Framework (not .NET Core/5+) |
| UI | Windows Forms | Drag-and-drop GUI designer in Visual Studio |
| Networking | TCP Sockets | `System.Net.Sockets` — raw socket programming, no HTTP |
| Data Storage | Plain text files | No database — everything stored in `.txt` files |
| Threading | `System.Threading` | Background threads for non-blocking socket receive loops |

---

## How to Build and Run

### Prerequisites
- **Windows** (Windows Forms is Windows-only)
- **Visual Studio** (2017 or later recommended) with the ".NET desktop development" workload

### Step-by-step

**Always start the server before the client.**

1. Open `server/server.sln` in Visual Studio
2. Press **F5** to build and run the server
3. In the server window, enter a port number (e.g. `5000`) and click **Listen**
4. Open `client/client.sln` in a **second** Visual Studio window
5. Press **F5** to build and run the client
6. In the client window, enter the server IP (e.g. `127.0.0.1` for local) and same port, then click **Connect**
7. Enter a username from `user-db.txt` and click **Login**

### Using MSBuild (Command Line)
```bash
# Build server
msbuild server/server.sln /p:Configuration=Release

# Build client
msbuild client/client.sln /p:Configuration=Release
```

### Important: Hardcoded File Paths
The server code (`server/server/Form1.cs`) has **hardcoded absolute file paths** pointing to a specific developer's machine. Before running, you must update all occurrences of paths like:
```
C:\Users\berka\Desktop\proj\server\server\sweets.txt
C:\Users\berka\Desktop\proj\client\client\user-db.txt
```
...to match the actual paths on your machine.

---

## Architecture: How Client and Server Talk

The two programs communicate using **raw TCP sockets**. Here's the flow:

```
CLIENT                              SERVER
  |                                   |
  |--- Connect (TCP handshake) ------>|
  |--- Send username string --------->|  (Server checks user-db.txt)
  |<-- "username has logged in" ------|
  |                                   |
  |--- "I want all the sweets" ------>|  (Server reads sweets.txt)
  |<-- sweet line 1 ------------------|
  |<-- sweet line 2 ------------------|
  |<-- ...                            |
  |                                   |
  |--- "Hello world" (sweet post) --->|  (Server writes to sweets.txt)
  |<-- "Your sweet: ..." -------------|
  |                                   |
  |--- "I want to disconnect" ------->|
  |    (socket closed on both ends)   |
```

### Threading Model
- The server runs an `Accept()` loop on a background thread — this waits for new clients to connect
- Each connected client gets its **own dedicated `Receive()` thread** on the server side
- The client also runs a `Receive()` thread in the background to receive server responses without freezing the UI
- `Control.CheckForIllegalCrossThreadCalls = false` is set to allow background threads to update UI elements directly (a quick workaround, not best practice)

---

## Communication Protocol

All messages are plain text strings, encoded with `Encoding.Default` and sent as byte arrays.

### Client → Server Messages

| Message String | What it does |
|---------------|-------------|
| `"I want to disconnect"` | Gracefully disconnects the client |
| `"I want all the sweets"` | Returns all sweets (excluding sweets from users who blocked you) |
| `"I want all the users"` | Returns a list of all registered usernames |
| `"I want to follow <username>"` | Adds a follow relationship |
| `"I want to block <username>"` | Adds a block relationship; also removes the blocked user's follow |
| `"I want all the followed sweets"` | Returns sweets only from users you follow |
| `"I want all my followers"` | Returns list of users who follow you |
| `"I want all following"` | Returns list of users you follow |
| `"I want all my sweets"` | Returns only your own sweets |
| `"I want to delete <sweet_id>"` | Deletes one of your sweets by its ID number |
| Anything else | Treated as a new sweet — saved to `sweets.txt` |

### Server → Client Messages

| Message Pattern | What it means |
|----------------|--------------|
| `"<username> has logged in to Sweeter."` | Login confirmed |
| `"User already logged in\n"` | Duplicate login attempt — client is disconnected |
| `"The client has disconnected\n"` | Server rejected the client (invalid username) |
| `"Your sweet: ..."` | Echo of a sweet you just posted |
| `"Username: <name>"` | One entry in the users list |
| `"You are followed by: <name>"` | One entry in the followers list |
| `"You are following: <name>"` | One entry in the following list |
| `"You can't follow yourself!"` | Error message |
| `"The user you are trying to follow has blocked you!"` | Error message |
| `"<user> has already blocked <user>"` | Error message |

### Buffer Sizes
- Client receive buffer: **128 bytes**
- Server receive buffer: **64 bytes**
- Max sweet length: **64 characters** (enforced on client side before sending)
- Null bytes (`\0`) are trimmed from received strings

---

## Data File Formats

All data is stored as plain text. There is no database.

### `user-db.txt` (inside `client/client/`)
One username per line. This is the list of valid accounts.
```
alice
bob
charlie
```

### `sweets.txt` (inside `server/server/`)
One sweet per line. New sweets are appended. IDs are sequential integers.
```
Username: alice Sweet: Hello world! Sweet Id: 1 Time: 01/01/2024 10:00:00
Username: bob Sweet: Good morning! Sweet Id: 2 Time: 01/01/2024 10:05:00
```

### `following.txt` (inside `server/server/`)
Each line starts with a username, followed by space-separated names of users they follow.
```
alice bob charlie
bob alice
```
This means: alice follows bob and charlie; bob follows alice.

### `blocked.txt` (inside `server/server/`)
Same format as `following.txt` — first name is the blocker, rest are blocked users.
```
alice charlie
```
This means: alice has blocked charlie.

---

## Key Business Logic

### Login Flow
1. Client connects via TCP
2. Client sends username string as first message
3. Server checks `user-db.txt` for the username
4. If not found → send disconnect message, close socket
5. If found but already online → send "already logged in", close socket
6. If found and not online → confirm login, add to `usernameList`, enter message loop

### Posting a Sweet
1. Client sends any string that doesn't match a known command
2. Server reads the last line of `sweets.txt` to get the last ID, then increments it
3. Server formats: `Username: <user> Sweet: <text> Sweet Id: <n> Time: <timestamp>`
4. Server appends this line to `sweets.txt`
5. Server echoes back `"Your sweet: ..."` to the client

### Block Logic
When user A blocks user B:
- B is added to A's row in `blocked.txt`
- B's follow of A is automatically removed from `following.txt`
- B can no longer follow A (follow request will be rejected)
- A's sweets will no longer appear in B's feed (and vice versa for "all sweets" view)

### Delete Logic
1. Client sends `"I want to delete <id>"`
2. Server scans `sweets.txt` for a sweet with that ID
3. Checks that the sweet belongs to the requesting user
4. If yes: rewrites `sweets.txt` excluding that line

---

## Known Limitations and Gotchas

- **Hardcoded file paths**: Must be updated before running on any machine other than the original developer's
- **No encryption**: All messages are sent as plain text over the network
- **No persistence of online users**: If the server restarts, `usernameList` is cleared (no crash recovery)
- **Buffer overflow risk**: The 64-byte server receive buffer can silently truncate messages longer than 64 bytes
- **No unfollow feature**: Users can follow but cannot unfollow
- **Race conditions**: Multiple threads read/write text files simultaneously with no locking mechanism
- **`CheckForIllegalCrossThreadCalls = false`**: Suppresses thread-safety exceptions instead of handling them properly — a quick hack common in student projects
- **ID generation**: Sweet IDs are determined by reading the last line of the file, which can break if the file is empty or manually edited

---

## Adding New Users

To add a new user, simply add their username on a new line in:
```
client/client/user-db.txt
```
No restart is required — the server reads this file on each login attempt.

---

## Educational Notes (CS 408 Context)

This project demonstrates several networking and systems concepts:
- **TCP socket lifecycle**: `bind()` → `listen()` → `accept()` → `send()`/`recv()` → `close()`
- **Multi-threaded server**: One thread per client connection
- **Client-server protocol design**: Fixed string commands vs. data payloads
- **Stateful sessions**: Server tracks connected users in memory (`usernameList`)
- **File I/O as a database**: Reading and rewriting text files to persist state
