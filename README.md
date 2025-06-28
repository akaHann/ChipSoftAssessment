# Verantwoording Ontwerpkeuzes

### 1. Clean Architecture
**Keuze:** De applicatie volgt een clean architecture met de volgende lagen:
- Models
- Configurations
- Interfaces
- Repositories
- Services
- Console App voor UI

**Reden:** Deze structuur zorgt voor een duidelijke scheiding van verantwoordelijkheden, wat de code beter onderhoudbaar en testbaar maakt. Omdat het een kleine applicatie is, zijn de lagen georganiseerd in mappen binnen één project in plaats van in aparte projecten. Dit bespaart tijd en overhead, terwijl de voordelen van een logische scheiding behouden blijven.

### 2. Repository Pattern
**Keuze:** Gebruik van het Repository Pattern voor data-access.

**Reden:**
- Abstractie van de data-access laag
- Eenvoudig wisselen van dataopslag (bijv. van SQLite naar SQL Server)
- Betere testbaarheid door middel van mock repositories

### 3. Dependency Injection
**Keuze:** Toepassing van Dependency Injection voor het beheren van afhankelijkheden.

**Reden:**
- Minder gekoppelde code
- Eenvoudiger testen door het kunnen injecteren van mock-objecten
- Centraal beheer van objectlevenscycli

## SOLID Principes

### 1. Single Responsibility Principle (SRP)
Elke klasse heeft één duidelijke verantwoordelijkheid:
- Services bevatten bedrijfslogica
- Repositories zorgen alleen voor data-access
- Modellen bevatten alleen eigenschappen en validatie

### 2. Open/Closed Principle (OCP)
- De generieke `Repository<T>` klasse is open voor uitbreiding maar gesloten voor wijzigingen
- Nieuwe functionaliteit kan worden toegevoegd door nieuwe services te maken in plaats van bestaande code aan te passen

### 3. Liskov Substitution Principle (LSP)
- Afgeleide klassen (zoals `PatientRepository`) kunnen zonder problemen hun basisklasse (`Repository<T>`) vervangen

### 4. Interface Segregation Principle (ISP)
- Specifieke interfaces voor elke service (`IPatientService`, `IPhysicianService`)
- Kleine, gerichte interfaces in plaats van één grote interface

### 5. Dependency Inversion Principle (DIP)
- Hoge-niveau modules (services) zijn niet afhankelijk van lage-niveau modules (repositories), maar van abstracties (interfaces)


### Async/Await
**Keuze:** Overal waar mogelijk gebruik gemaakt van async/await.

**Reden:**
- Betere schaalbaarheid
- Voorkomt dat de UI vastloopt bij database-operaties

## Voorbeeld Verbeterpunten

### 1. Validatielaag
**Aanbeveling:** Toevoegen van een aparte validatielaag met FluentValidation.

**Voordeel:**
- Scheiding van validatielogica
- Herbruikbare validatieregels
- Betere leesbaarheid

### 2. DTO's (Data Transfer Objects)
**Aanbeveling:** Toevoegen van DTO's voor communicatie tussen lagen.

**Voordeel:**
- Betere scheiding van lagen
- Minder afhankelijkheden
- Betere beveiliging door alleen noodzakelijke data te tonen

### 3. Logging
**Aanbeveling:** Toevoegen van logging met bijvoorbeeld Serilog of NLog.

**Voordeel:**
- Betere foutopsporing

### 4. SOLID in Program.cs
**Aanbeveling:** code kan worden verbeterd om beter te voldoen aan SOLID principes en Clean Architecture. De huidige Program.cs bevat te veel verantwoordelijkheden.


## Tijdsbesteding

| Activiteit | Tijd (min) |
|------------|------------|
| Projectopzet en architectuur | 30 |
| Domeinmodellen ontwerpen | 20 |
| Repositories implementeren | 30 |
| Services implementeren | 45 |
| Console UI bouwen | 45 |
| Testen en debuggen | 10 |
| **Totaal** | **180** (3uur) |
