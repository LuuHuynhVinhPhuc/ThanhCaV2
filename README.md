# 🎵 Holy Song V2 - Professional Hymn Projection System

Welcome to **Holy Song V2**, a modern application designed to manage and project lyrics for church services with a smooth, PowerPoint-like experience but tailored specifically for hymns.

## ✨ Key Features

- **PowerPoint-Style Projection**: Elegant projection interface with formal Times New Roman fonts, high contrast, and optimization for large screens.
- **Smart Projection Sequencing**: Allows users to intuitively click and select the order of sections (Verse 1, Chorus, Verse 2...) to avoid mistakes.
- **Unique ID Identification**: Completely solves the confusion between sections with identical labels by using unique identifiers.
- **Mass Playlist Management**: Pre-select hymns for a specific ceremony and switch between them quickly.
- **Intelligent Search**: Powerful hymn filtering that ignores case sensitivity and fully supports accented characters.
- **Modern Dark Mode UI**: Built with Tailwind CSS v4 featuring a premium Glassmorphism style for the management area.

## ⌨️ Projection Shortcuts

While in projection mode, you can use the following keys:

| Action | Shortcut |
| :--- | :--- |
| **Next Section** | `Right Arrow`, `Space`, `Enter`, `Page Down` |
| **Previous Section** | `Left Arrow`, `Page Up` |
| **Blackout Screen** | `B` or `.` |
| **Title Slide** | Automatically shown at the start (Title & Author) |
| **Exit Projection** | `Esc` |

## 🚀 Getting Started

### Using Docker (Recommended)
The easiest way to run the app with persistent data:
```bash
docker-compose up -d --build
```
The app will be available at `http://localhost:5000`.

### Manual Setup
1. **Prerequisite**: Install [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).
2. **Clone the repo**: `git clone <repository-url>`
3. **Run the app**:
   ```bash
   cd ThanhCaV2.Blazor
   dotnet run
   ```
4. **Access**: Open your browser at `http://localhost:5000`.

## 🛠 Technology Stack

- **Frontend**: Blazor Server (Interactive Mode)
- **Styling**: Tailwind CSS v4 (via Play CDN)
- **Backend**: .NET 8, MediatR, Mapster
- **Database**: SQLite (Entity Framework Core)

---
*May your services be blessed and seamless with Holy Song V2!*
