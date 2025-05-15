# Solo Scheduler: Gamified Productivity App inspired by Solo Leveling

![Project Status](https://img.shields.io/badge/status-in%20progress-yellow)
![Built With](https://img.shields.io/badge/built%20with-Godot%20%7C%20Go%20%7C%20Firebase%20%7C%20SQL-blue)

## 🚀 Overview

**Solo Scheduler** is a gamified productivity app inspired by Solo Leveling that aims to transform mundane daily tasks into exciting quests. The app uses traditional RPG elements like levels, experience points (EXP), stats, skills, items and more to provide visually appealing and engaging markers of progression, thereby instilling a sense of satisfaction and accomplishment in users. This in turn motivates users to take control of their lives, boosting productivity and cutting down on digital procrastination.

---

## 🎯 Motivation

The rise of TikTok, YouTube Shorts and Instagram Reels has contributed to attention fragmentation and dopamine-driven distractions. Solo Scheduler offers a fun and focused alternative that rewards productivity and discourages endless scrolling.

---

## 🧩 Core Features

| Feature            | Description |
|--------------------|-------------|
| **Login System**   | Social login via Google/Facebook or guest mode. |
| **Character Page** | RPG-style status page with avatar, gear, stats and rank. |
| **Daily Quests**   | To-do list system with rewards (EXP, gold) and penalties for missed tasks. |
| **Passive Dungeon**| Lock phone for a set period to earn rewards. Early exit yields partial rewards. |
| **Active Dungeon** | A 2D rogue-like game mode where users battle enemies with earned stats and items. |
| **Reward System**  | Levels, ranks, items, stat points, gold, skills, and titles. |
| **Art & Sound**    | Free online assets used for polished visuals and sound design. |

---

## 🎮 Gameplay Loop

1. **Create Quests** – Set daily goals via customisable to-do lists.
2. **Enter Dungeons** – Focus using Passive Dungeons or unwind with Active Dungeons.
3. **Earn Rewards** – Gain EXP, gold, titles and loot for real-life productivity.
4. **Level Up** – Allocate stat points, unlock new abilities and rise through hunter ranks.

---

## ⚙️ Tech Stack

| Layer         | Tech                          |
|---------------|-------------------------------|
| Game Engine   | [Godot](https://godotengine.org/) (C#) |
| Backend       | [Go](https://go.dev/)         |
| Auth Service  | [Firebase Authentication](https://firebase.google.com/) |
| Database      | SQL (PostgreSQL / MySQL)      |
| Version Control | Git + GitHub + GitHub Actions |
| CI Tools      | StyleCop, golangci-lint       |

---

## 🧱 Architecture & Design Principles

- **Design Patterns:** Observer, Singleton
- **Architecture:** Clean architecture with modular managers
- **Engineering Practices:** CI with GitHub Actions, Kanban via Miro
- **SOLID Principles:** SRP, OCP, LSP, ISP, DIP applied throughout systems
- **Performance:** Lazy loading, memoization, caching and optimised data structures

---

## 🗓️ Development Timeline 

| Milestone | Key Focus Areas |
|----------|-----------------|
| M0 (14 May) | UI prototyping, login/auth setup |
| M1 (2 Jun) | Responsive UI, CRUD for quests, Passive Dungeon logic |
| M2 (30 Jun) | Stats, items, reward balancing, Active Dungeon prototype |
| M3 (28 Jul) | Polishing art, combat mechanics, full feature integration |

---

## 👥 Team

| Name                | Background |
|---------------------|------------|
| **Andrew Soon Qian** | CS @ NUS + Performing Arts minor, IEEE Hackathon finalist |
| **Maverick Lim Qi Xun** | CS @ NUS + Korean Studies minor, CS50X/SQL certified|

---

## 📂 Folder Structure

> (to be updated when project is pushed)

## 🕵️‍♀️ Acknowledgements
- 🎨 Sprites: [The Spriters Resource](https://www.spriters-resource.com/)
- 🔊 Sound: [Sonniss GameAudioGDC](https://sonniss.com/gameaudiogdc/)
- 📚 Inspired by: [Solo Leveling Wiki](https://en.wikipedia.org/wiki/Solo_Leveling)
- 👀 Tilemap and Player character: [Simple Dungeon Crawler 16x16 Pixel Art Asset Pack](https://o-lobster.itch.io/simple-dungeon-crawler-16x16-pixel-pack?download). Done by [0_LOBSTER](https://itch.io/profile/o-lobster)
