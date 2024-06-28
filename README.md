# Narrative Unity Game

Narrative Unity Game is an immersive, story-driven game built with Unity, featuring a rich interaction system, dynamic NPC behaviors, and a comprehensive quest system. This project showcases advanced Unity scripting techniques, including service locators, custom editors, and player mechanics. 🎮📖✨

<div align="center">
  <video src="[myvideo.mp4](https://github.com/ahmedafifiabodu/Narrative-Unity-Game/assets/74466733/70c44136-a2f2-4d4b-9b65-ccaae22d76de)" width="400" />
</div>

## Features

- Service Locator Pattern
- Input Management
- Interactable System
- NPC Navigation
- Player Mechanics

## Scripts Overview

### Core Systems

#### [Service Locator](#service-locatorcs-context)
A central registry for accessing global services. It prevents tight coupling between components and services.

#### [Input Manager](#input-managercs-context)
Handles all player inputs, making it easy to manage and update input bindings.

### Interaction System

#### [Interactable](#interactablecs-context)
Base class for all interactable objects in the game, providing a framework for interaction events.

#### [EventOnlyInteractables](#eventonlyinteractablescs-context)
A specialized version of `Interactable` that triggers events without standard interaction prompts.

#### [InteractableEditor](#interactableeditorcs-context)
Custom editor script for `Interactable` objects, enhancing the Unity Editor experience.

#### [Interaction Events](#interaction-eventscs-context)
Defines Unity Events that can be invoked by the `Interactable` system.

### NPC System

#### [NPC NavMesh](#npc-navmeshcs-context)
Utilizes Unity's NavMesh system to control NPC movement and behavior.

### Player System

#### [Player Animation](#player-animationcs-context)
Manages all player animations, synchronizing them with player actions and movements.

#### [Player Interact](#player-interactcs-context)
Handles player interactions with `Interactable` objects in the game world.

#### [Player Location](#player-locationcs-context)
A utility script for tracking and managing the player's location.

#### [Player Movement](#player-movementcs-context)
Controls player movement, including walking, running, and jumping.

#### [Player Spawner](#player-spawnercs-context)
Manages player spawning and respawning mechanics.

### Quest System

#### [Goal Content Panel](#goal-content-panelcs-context)
Displays individual goals within the UI.

#### [Goal Content](#goal-contentcs-context)
Manages the display of quest goals in the UI.

#### [Goal](#goalcs-context)
Defines individual quest goals.

#### [Quest Content UI](#quest-content-uics-context)
Handles the UI display of quest information.

#### [Quest Custom Editor](#quest-custom-editorcs-context)
Custom editor for quests, improving the setup process within the Unity Editor.

#### [Quest Giver](#quest-givercs-context)
Defines NPCs that can give quests to the player.

#### [Quest Manager Custom Editor](#quest-manager-custom-editorcs-context)
Custom editor for the `Quest Manager`, streamlining quest management.

#### [Quest Manager](#quest-managercs-context)
Central system for managing quests within the game.

#### [Quest NPC](#quest-npccs-context)
Defines NPCs that are part of the quest system.

#### [Quest State](#quest-statecs-context)
Enumerates the possible states of a quest.

#### [Quest](#questcs-context)
Defines the structure and properties of quests.

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes. See deployment for notes on how to deploy the project on a live system.

### Prerequisites

Before you begin, ensure you have the following software installed on your computer:

- [Unity](https://unity.com/) (Version 2020.3 LTS or later recommended)
- [Visual Studio](https://visualstudio.microsoft.com/) or another compatible IDE for Unity development
- Git for version control

### Installing

To get a development environment running:

1. Clone the repository to your local machine using Git:

2. Open Unity Hub and add the cloned project by clicking on the 'Add' button and selecting the project folder.

3. Open the project in Unity by clicking on it in Unity Hub.

4. Once Unity loads the project, navigate to the `Assets` folder in the Project tab.

5. Open the `Scenes` folder and double-click on the `Main` scene to open it.

6. Press the Play button in Unity to run the game in the editor.

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details.
