# Dastarkhan Disaster
 
**A 1-4 player co-op cooking chaos game set in the world of Kazakh cuisine and hospitality.**
 
Prepare traditional Kazakh dishes — beshbarmak, baursaki, kurt, and more — for increasingly demanding guests across yurts, nomadic camps, and Nauryz celebration feasts.
 
> *"Overcooked set in a Kazakh feast — and grandma is watching."*
 
---
 
## Table of Contents
 
- [About](#about)
- [Core Mechanics](#core-mechanics)
- [Dishes](#dishes)
- [Characters](#characters)
- [Level Progression](#level-progression)
- [Controls](#controls)
- [Tech Stack](#tech-stack)
- [Project Structure](#project-structure)
- [Development Roadmap](#development-roadmap)
 
---
 
## About
 
**Dastarkhan** is the traditional Kazakh feast cloth laid on the ground or table, covered with food and tea, representing hospitality, family, and celebration. Failing to fill it properly is a cultural disaster — and the premise of this game.
 
Players scramble across the kitchen — chopping, boiling, frying, and assembling dishes — while the kitchen fights back with fires, broken equipment, wandering sheep, and limited counter space.
 
| Detail       | Value                   |
|--------------|-------------------------|
| Genre        | Co-op Arcade            |
| Players      | 1-4 (Local & Online)    |
| Platform     | PC (Steam)              |
| Engine       | Unity (C#)              |
| Art Pipeline | Blender, Figma, GIMP    |
| Price        | $9.99                   |
 
---
 
## Core Mechanics
 
### Samovar System
A traditional brass tea urn sits in every level. It must stay filled with hot tea at all times. If it empties, all guest Respect meters begin dropping — creating a dedicated "tea runner" role in co-op.
 
### Respect Meter
Each guest seat has a Respect bar. Correct dishes served quickly increase it; wrong dishes decrease it. If any guest hits zero Respect, the round fails. Cultural respect is mechanically mandatory — you cannot brute-force speed at the cost of quality.
 
### Elder Demands
A wise Elder NPC observes the feast and periodically issues priority demands that override the current order queue. These are randomized interrupt events that create emergent pressure.
 
### Tandoor Blind Bake
The clay tandoor oven has no visible timer. Players rely on audio pitch and smoke color to judge doneness. Too long = burned. Too early = raw.
 
### Kurt Drying Station
Kurt (dried cheese balls) require a shaping minigame followed by a real-time drying delay. They cannot be rushed — teaching pipeline planning and resource management.
 
### Kitchen Events (Chaos Modifiers)
- Sheep wanders into the kitchen, bumping carried dishes
- Tandoor fire flares — nearby stations catch fire
- Pot boils over, blocking floor tiles
- Samovar falls and must be righted and refilled
- Storm wind blows food off unprotected stations
 
---
 
## Dishes
 
| Dish | Difficulty | Key Steps |
|------|-----------|-----------|
| **Beshbarmak** | Hard | Knead dough, boil noodles, boil lamb, layer on plate, pour tuzdyk gravy |
| **Baursaki** | Medium | Shape dough balls, fry in oil/tandoor, plate 3x, deliver |
| **Kurt** | Easy (time-intensive) | Shape at station, place on drying rack, wait, bag 3x, deliver |
| **Chai** | Ongoing | Fill kettle, heat on samovar burner, pour into teapot, keep samovar full |
 
Full release adds: kazy, sorpa, shelpek, shubat.
 
---
 
## Level Progression
 
| # | Level | Setting | Introduces |
|---|-------|---------|------------|
| 1 | First Guest | Small yurt | Pickup/deliver, baursaki, chai |
| 2 | Family Arrives | Larger yurt | Beshbarmak, samovar timer |
| 3 | Nomadic Camp | Outdoor fires | Tandoor station, blind bake |
| 4 | Wedding Feast | Wedding tent | Respect meter, Elder demands |
| 5 | Nauryz Festival | Festival grounds | All dishes, kitchen events |
| 6 | The Elder's Table | Elder's home | All mechanics, perfection required |
| B | Blizzard Camp | Steppe blizzard | Stations freeze, must be thawed |
 
---
 
## Controls
 
| Input | Action |
|-------|--------|
| WASD / Arrow Keys | Move |
| E / Space | Pick up, interact, place |
| Hold E | Chop, knead, shape |
| Q / Shift+E | Throw item to teammate |
| Tab | View current orders |
 
Supports keyboard and gamepad.
 
---
 
## Tech Stack
 
| Tool | Usage |
|------|-------|
| **Unity** | Game engine (C#) |
| **Blender** | 3D modeling and asset creation |
| **Figma** | UI/UX design |
| **GIMP** | Texture and sprite editing |
| **Mirror / Fishnet** | Networking (local co-op first, online later) |
 
---
 
## Project Structure
 
```
Assets/
  Scenes/          - Level scenes (Yurt, Camp, Wedding, etc.)
  Scripts/
    Core/          - Game manager, input, scoring
    Cooking/       - Recipe validation, ingredient state machines
    Stations/      - Samovar, tandoor, drying rack, chopping board
    Characters/    - Player controller, abilities, carry system
    Guests/        - Respect meter, order queue, Elder AI
    Events/        - Kitchen chaos events (fire, sheep, spills)
    UI/            - HUD, order display, score screen
    Networking/    - Co-op session management
  Prefabs/         - Dishes, stations, characters, props
  Art/             - Sprites, textures, UI assets
  Audio/           - Dombra tracks, SFX, voice lines
  ScriptableObjects/
    Recipes/       - Dish definitions and step sequences
    Characters/    - Character stats and abilities
    Levels/        - Level configuration and event pools
```
 
---
 
## Development Roadmap
 
### MVP (3 Months)
- 2 playable levels (yurt, camp) with tutorial
- 3 dishes: baursaki, chai, beshbarmak
- 2 characters: Apa, Daniyar
- Core mechanics: pickup/place, recipe validation, samovar, Respect meter
- 1 kitchen event: pot boil-over
- Local co-op (same PC)
