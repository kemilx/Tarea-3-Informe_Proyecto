# Catálogo de Anime API

API REST CRUD desarrollada con ASP.NET Core, Entity Framework Core y SQL
Server LocalDB. Permite registrar, consultar, actualizar y eliminar anime.

## Requisitos

- .NET SDK 10
- SQL Server LocalDB

## Ejecutar el proyecto

```powershell
dotnet restore
dotnet run
```

Al iniciar en modo de desarrollo, Swagger estará disponible en:

```text
http://localhost:5164/swagger
```

La aplicación aplica automáticamente las migraciones pendientes y crea la base
de datos `AnimeCatalogDb`.

## Endpoints

| Método | Ruta | Descripción |
| --- | --- | --- |
| GET | `/api/animes` | Lista el catálogo |
| GET | `/api/animes/{id}` | Obtiene un anime por id |
| POST | `/api/animes` | Crea un anime |
| PUT | `/api/animes/{id}` | Actualiza un anime |
| DELETE | `/api/animes/{id}` | Elimina un anime |

El listado admite los filtros opcionales `search` y `genre`:

```text
GET /api/animes?search=Frieren
GET /api/animes?genre=fantasía
```

## Ejemplo para crear un anime

```json
{
  "title": "Steins;Gate",
  "genre": "Ciencia ficción y suspenso",
  "studio": "White Fox",
  "synopsis": "Un grupo de amigos descubre una forma de enviar mensajes al pasado.",
  "releaseYear": 2011,
  "episodes": 24,
  "rating": 9.0,
  "status": "Finished"
}
```

Estados admitidos: `Announced`, `Airing`, `Finished`, `Hiatus` y `Cancelled`.

## Ramas propuestas para la tarea

- `feature/create-anime`
- `feature/list-animes`
- `feature/update-anime`
- `feature/delete-anime`
- `feature/anime-validation`
