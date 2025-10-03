# 🧟‍♂️ Zombie Defense API

API construida en **.NET 8** siguiendo principios de **arquitectura hexagonal (Clean Architecture)** para resolver la prueba técnica de defensa contra hordas de zombies.  
El sistema evalúa estrategias óptimas de eliminación de zombies según recursos limitados de **balas** y **tiempo disponible**, maximizando el puntaje y respetando los criterios de amenaza.

---

## 🚀 Tecnologías utilizadas
- **.NET 8 Web API**
- **Carter** → Endpoints minimalistas
- **MediatR** → CQRS y orquestación de casos de uso
- **Entity Framework Core (SQL Server)**
- **Ardalis.Specification** → Patrón Specification + Repositorios genéricos
- **Swagger/OpenAPI** → Documentación interactiva
- **Middleware personalizado** → Autenticación por API Key

---

## 📂 Arquitectura del proyecto

```
ZombieDefense
 ┣ ZombieDefense.API                → Capa de presentación (endpoints con Carter)
 ┣ ZombieDefense.Application        → Casos de uso (Handlers, DTOs, Queries, lógica de estrategia)
 ┣ ZombieDefense.Domain             → Entidades y contratos del dominio
 ┣ ZombieDefense.Infrastructure     → Persistencia (DbContext, Repositorios, Dependency Injection)
```

### Principios
- **Domain**: Entidades puras (`Zombie`, `Simulation`, `Eliminated`).
- **Application**: Casos de uso (`OptimalStrategyQueryHandler`), DTOs y lógica de negocio.
- **Infrastructure**: Repositorios concretos y acceso a SQL Server.
- **API**: Exposición de endpoints REST.

---

## 🧠 Funcionalidad principal

El sistema calcula la **estrategia óptima de defensa** resolviendo una variante del **problema de mochila (Knapsack)** con dos restricciones:
- Balas disponibles (`Bullets`)
- Tiempo disponible (`SecondsAvailable`)

### Criterios de la estrategia:
- Evaluar combinaciones posibles de zombies.
- No exceder balas ni segundos.
- Maximizar el puntaje total (`Score`).
- Ordenar zombies eliminados por nivel de amenaza (`ThreatLevel`) descendente.

---

## 🔑 Seguridad
La API está protegida mediante un **API Key Middleware**.  
Todos los requests deben incluir el header:

```
X-API-Key: ZombieApiKEY
```

El valor se configura en `appsettings.json`:

```json
"ApiKeySettings": {
  "Key": "ZombieApiKEY"
}
```

---

## 🔗 Endpoints

### 1. Obtener estrategia óptima
`GET /defense/optimal-strategy`

#### Parámetros (QueryString)
- `bullets` (int) → Número de balas disponibles
- `secondsAvailable` (int) → Tiempo disponible en segundos

#### Ejemplo de request
```
GET https://localhost:5001/defense/optimal-strategy?bullets=6&secondsAvailable=8
X-API-Key: MI_SUPER_API_KEY_SECRETA
```

#### Ejemplo de respuesta
```json
{
  "totalScore": 60,
  "bulletsUsed": 5,
  "timeUsed": 6,
  "zombiesEliminated": [
    {
      "id": "4bbdd267-fe5c-4ffb-8fe1-9ea9b3de4921",
      "zombieType": "Exploder",
      "shotTimeSeconds": 4,
      "bulletsRequired": 3,
      "score": 40,
      "threatLevel": 4
    },
    {
      "id": "99865567-a970-499d-badb-fa705ec1d550",
      "zombieType": "Runner",
      "shotTimeSeconds": 2,
      "bulletsRequired": 2,
      "score": 20,
      "threatLevel": 2
    }
  ]
}
```

---

## 🗄️ Base de datos

La API usa **SQL Server**.  

Tablas principales:
- **Zombies** → catálogo de tipos de zombies.
- **Simulations** → ejecuciones de estrategias.
- **Eliminated** → zombies eliminados en una simulación.

### Script inicial

```sql



CREATE DATABASE ZombieDefenseDB;

USE ZombieDefenseDB;

CREATE TABLE Zombies(
Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
ZombieType NVARCHAR(20) NOT NULL,
ShotTimeSeconds INT NOT NULL,
BulletsRequired INT NOT NULL,
Score INT NOT NULL,
ThreatLevel INT NOT NULL,
CreateAt NVARCHAR(100) NOT NULL DEFAULT SYSTEM_USER,
CreateDate DATETIME NOT NULL DEFAULT GETDATE(),
ModifierAt NVARCHAR(100) NULL DEFAULT SYSTEM_USER,
ModifierDate DATETIME NULL ,
IsActive BIT NOT NULL
);

CREATE TABLE Simulations(
Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
SimulationDate DATETIME NOT NULL,
TimeAvailableSeconds INT NOT NULL,
BulletsAvailable INT NOT NULL,
CreateAt NVARCHAR(100) NOT NULL DEFAULT SYSTEM_USER,
CreateDate DATETIME NOT NULL DEFAULT GETDATE(),
ModifierAt NVARCHAR(100) NULL DEFAULT SYSTEM_USER,
ModifierDate DATETIME NULL,
IsActive BIT NOT NULL
);


CREATE TABLE Eliminated(
Id UNIQUEIDENTIFIER PRIMARY KEY DEFAULT NEWID(),
ZombieId UNIQUEIDENTIFIER NOT NULL,
SimulationId UNIQUEIDENTIFIER NOT NULL,
PointsEarned INT NOT NULL,
EliminatedAt DATETIME NOT NULL,
CreateAt NVARCHAR(100) NOT NULL DEFAULT SYSTEM_USER,
CreateDate DATETIME NOT NULL DEFAULT GETDATE(),
ModifierAt NVARCHAR(100) NULL DEFAULT SYSTEM_USER,
ModifierDate DATETIME NULL,
IsActive BIT NOT NULL

CONSTRAINT FK_Eliminated_Zombie FOREIGN KEY (ZombieId) REFERENCES Zombies(Id),
CONSTRAINT FK_Eliminated_Simulations FOREIGN KEY (SimulationId) REFERENCES Simulations(Id)
);


CREATE TABLE AuditLogs(

	Id INT PRIMARY KEY IDENTITY(1,1),
	EliminatedId UNIQUEIDENTIFIER NOT NULL,
	ActionType NVARCHAR(10) NOT NULL,
	ActionDate DATETIME NOT NULL DEFAULT GETDATE(),
	CreateAt NVARCHAR(100) NOT NULL DEFAULT SYSTEM_USER,
	OldValues NVARCHAR(MAX) NOT NULL,
	NewValues NVARCHAR(MAX) NOT NULL);
	
	
SELECT  
	s.Id as SimulationId,
	z.ZombieType,
	COUNT(e.Id) as ZombiesEliminated,
	SUM(e.PointsEarned) AS TotalPoints
FROM Eliminated e
INNER JOIN Zombies z ON e.ZombieId=z.Id
INNER JOIN Simulations s ON e.SimulationId=s.Id
GROUP BY s.Id, z.ZombieType,
ORDER BY s.Id, z.ZombieType;

INSERT INTO Zombies (ZombieType, ShotTimeSeconds, BulletsRequired, Score, ThreatLevel) VALUES ('Walker', 3, 1, 10, 1), ('Runner', 2, 2, 20, 2), ('Tank', 5, 5, 50, 3), ('Exploder', 4, 3, 40, 4);





---

## ▶️ Ejecución

1. Clonar el repositorio
   ```bash
   git clone https://github.com/usuario/ZombieDefense.git
   cd ZombieDefense
   ```

2. Configurar la conexión a SQL Server en `appsettings.json`
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=.;Database=ZombieDefenseDB;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

3. Restaurar dependencias
   ```bash
   dotnet restore
   ```

4. Ejecutar migraciones (si usas EF Migrations)
   ```bash
   dotnet ef database update --project ZombieDefense.Infrastructure --startup-project ZombieDefense.API
   ```

5. Levantar la API
   ```bash
   dotnet run --project ZombieDefense.API
   ```

6. Abrir Swagger
   ```
   https://localhost:5001/swagger
   ```

---

## 📌 Próximos pasos / Mejoras
- Registrar cada simulación y zombies eliminados en la tabla `Simulations` y `Eliminated`.  
- Agregar autenticación JWT además de API Key.  
- Extender la lógica para soportar múltiples escenarios de defensa en paralelo.  
