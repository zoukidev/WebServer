# WebServer C#

Un micro serveur web développé en C# avec ASP.NET Core, conçu pour être simple, léger et facilement extensible.

## Client associé

- [Vite+React WebServer Client](https://github.com/zoukidev/WebServer-Client)

## Fonctionnalités

- Authentification JWT sécurisée
- Gestion des utilisateurs (inscription, connexion, liste)
- Base de données SQLite intégrée (Entity Framework Core)
- Architecture modulaire : séparation claire entre Handlers, Helpers, Models, Data
- Configuration flexible via `appsettings.json`
- CORS activé pour le développement front-end
- Prêt pour déploiement local ou cloud

## Prérequis

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- (Optionnel) [SQLite](https://www.sqlite.org/download.html) pour la gestion directe de la base de données

## Installation

1. **Cloner le dépôt**
    ```bash
    git clone https://github.com/zoukidev/WebServer.git
    cd WebServer
    ```

2. **Restaurer les dépendances**
    ```bash
    dotnet restore
    ```

3. **Lancer le serveur**
    ```bash
    dotnet run
    ```

4. **(Optionnel) Modifier la configuration**
    - Les fichiers `appsettings.json` et `appsettings.Development.json` permettent d’ajuster les paramètres (port, niveau de logs, etc.).

## Structure du projet

- [`Program.cs`](Program.cs) : Point d’entrée de l’application, configuration des services et des routes principales
- [`Routes.cs`](Routes.cs) : Définition des routes HTTP personnalisées
- [`Data/`](Data/) : Contexte de base de données ([`AppDbContext`](Data/AppDbContext.cs))
- [`Handlers/`](Handlers/) : Gestionnaires pour l’authentification et les utilisateurs
- [`Helpers/`](Helpers/) : Utilitaires (ex : [`JwtTokenHelper`](Helpers/JwtTokenHelper.cs) pour la génération de tokens JWT)
- [`Models/`](Models/) : Modèles de données (ex : [`User`](Models/User.cs))

## Configuration

- Modifiez `appsettings.json` ou `appsettings.Development.json` pour :
    - Le niveau de logs
    - Les hôtes autorisés
    - (À compléter selon vos besoins : chaîne de connexion, clés secrètes, etc.)

## Exemples de requêtes

### Authentification

**Inscription**
```http
POST /register
Content-Type: application/json

{
  "username": "admin",
  "name": "Administrateur",
  "password": "motdepasse"
}
```

**Connexion**
```http
POST /login
Content-Type: application/json

{
  "username": "admin",
  "password": "motdepasse"
}
```

**Vérification du token**
```http
GET /check-token
Authorization: Bearer <votre_token_jwt>
```

**Liste des utilisateurs**
```http
GET /users
Authorization: Bearer <votre_token_jwt>
```

## Sécurité

- Le mot de passe utilisateur est stocké sous forme de hash BCrypt.
- Les routes sensibles nécessitent un token JWT valide dans l’en-tête `Authorization`.

## Développement

- Le projet utilise Entity Framework Core avec migration automatique à la création.
- Pour modifier la structure de la base, modifiez les modèles dans [`Models/`](Models/) puis appliquez les migrations si besoin.
