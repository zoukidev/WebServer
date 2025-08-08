# WebServer C#

Un micro serveur web développé en C# avec ASP.NET Core, conçu pour être simple, léger et facilement extensible.

## Fonctionnalités

- Authentification JWT
- Gestion des utilisateurs
- Base de données SQLite intégrée
- Architecture modulaire (Handlers, Helpers, Models)
- Configuration via `appsettings.json`

## Prérequis

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- (Optionnel) SQLite pour la gestion de la base de données

## Installation

1. Clonez le dépôt :
	```bash
	git clone https://github.com/zoukidev/WebServer.git
	cd WebServer
	```

2. Restaurez les dépendances :
	```bash
	dotnet restore
	```

3. Lancez le serveur :
	```bash
	dotnet run
	```

## Structure du projet

- `Program.cs` : Point d’entrée de l’application
- `Routes.cs` : Définition des routes HTTP
- `Data/` : Contexte de base de données et migrations
- `Handlers/` : Gestionnaires pour l’authentification et les utilisateurs
- `Helpers/` : Utilitaires (ex : génération de tokens JWT)
- `Models/` : Modèles de données

## Configuration

Modifiez les fichiers `appsettings.json` ou `appsettings.Development.json` pour adapter la configuration (port, chaîne de connexion, etc.).

## Exemple de requête

Authentification :
```http
POST /auth/login
Content-Type: application/json

{
  "username": "admin",
  "password": "motdepasse"
}
```


