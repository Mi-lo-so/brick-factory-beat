# BrickFactoryBeat
Repository for BrickFactoryBeat, which handles equipment states, orders, and overview.

## Table of Contents

- [Backend](#backend)
- - [Equipment API](#equipment-api)
- [Frontend](#frontend)

## Backend
.Net C#
This is where the business logic, APIs, and 🗄 SQL magic happens.
The Backend handles Equipment creation, updates, as well as updates to the orders, and statehistory.
### Equipment API
This is a REST API for managing the equipment. 
This API also handles:
- starting orders (on an piece of equipment by id)
- changing state on equipment
- viewing the state history and associated orders
- getting orders (primarily for a piece of equipment)
- viewing the orders
All operations are handled through the IEquipmentService

The repository generally follows the clean code architecture and is divided into:
- BrickFactoryBeat.Domain
- - Which contains the domain classes used throughout the backend project.
- BrickFactoryBeat.Application
- - Which contains the IEquipmentService, which handles the core business logic.
- BrickFactoryBeat.Infrastructure
- - Which contains the database context, including migrations and repositories
- BrickFactoryBeat.WebApi
- - Which contains the API controller definition and is the main entry point for the backend application.

## Frontend
Made using React Typescript. See general readme for usage instructions.
This is the visual representation of the data.

<img width="1220" height="855" alt="image" src="https://github.com/user-attachments/assets/3880808d-20b1-4f4e-8c82-ca97008bad90" />

<img width="1226" height="1241" alt="image" src="https://github.com/user-attachments/assets/f36a901c-c671-4cce-a961-6f64173596c0" />
