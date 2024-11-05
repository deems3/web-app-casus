# VICTUZ Webapplicatie

Deze webapplicatie is ontwikkeld door:

- **Aaron Stepanian**
- **Demi Bruls**
- **Martijn Wening**
- **Mees Hoevenaars**

Als onderdeel van de HBO-ICT opleiding aan Hogeschool Zuyd in het jaar 2024.



## Over de Applicatie

Het doel van de VICTUZ webapplicatie is om een platform te bieden waar leden van de studievereniging eenvoudig geautomatiseerde processen kunnen uitvoeren. Gebruikers kunnen zich aanmelden voor het lidmaatschap, zich inschrijven voor activiteiten, activiteiten voorstellen en merchandise kopen via de webshop. 

Naast de leden biedt de applicatie ook functionaliteiten voor gebruikers met extra rechten, zoals bestuursleden. Zij kunnen verschillende modules binnen de applicatie beheren, waaronder activiteiten, de webshop en gebruikersaccounts. 

Een belangrijk aspect van dit project is de focus op gebruiksvriendelijkheid, zodat de gebruikers zich betrokken voelen bij het gebruik van de website.



## Features

### 1. Activiteiten
- **Beschrijving**: Leden kunnen zich eenvoudig inschrijven voor en zich afmelden van activiteiten die door studenten worden georganiseerd.
- **Functionaliteiten**:
  - Activiteiten aanmaken, inzien, wijzigen, goed- en afkeuren en verwijderen.
  - Activiteiten voorstellen door leden, met de vereiste dat er minimaal twee andere leden als organisator moeten worden toegevoegd.
  - Emailnotificaties voor leden bij updates over activiteiten.

### 2. Mailer
- **Beschrijving**: Een geïntegreerde mailservice die leden op de hoogte houdt van belangrijke updates en meldingen.
- **Functionaliteiten**:
  - Automatische e-mails naar leden bij inschrijvingen, afmeldingen en andere belangrijke gebeurtenissen.
  - Update e-mails indien de status van een voorgestelde activiteit geüpdate wordt.

### 3. Webshop
- **Beschrijving**: Een webshop waar leden merchandise kunnen kopen met betrekking tot de studievereniging.
- **Functionaliteiten**:
  - Producten toevoegen, wijzigen en verwijderen door bestuursleden.
  - Categorieën voor producten beheren door bestuursleden.
  - Volledige orderverwerking, inclusief bestellingen inzien, wijzigen en annuleren.

### 4. Ledenbeheer
- **Beschrijving**: Een modulair systeem voor het beheren van leden van de vereniging.
- **Functionaliteiten**:
  - Registratie van nieuwe leden en beheer van bestaande leden.
  - Bestuursleden kunnen leden blacklisten bij overtreding van de vereniging zijn regels.

### 5. Authenticatie
- **Beschrijving**: Beveiliging van de applicatie door middel van gebruikersauthenticatie en rolgebaseerde toegang.
- **Functionaliteiten**:
  - Ondersteuning voor verschillende gebruikersrollen (Lid en Bestuurslid) met specifieke rechten en toegangsniveaus.
  - Rolgebaseerde functionaliteit die bepaalt welke acties gebruikers kunnen uitvoeren binnen de applicatie.
  - Toegang tot gevoelige gegevens is alleen toegestaan voor bestuursleden, ter bescherming van de privacy van alle leden.



## Proces

Hier is een overzicht van het ontwikkelingsproces voor deze webapplicatie, weergegeven door middel van verschillende diagrammen en user stories. Deze elementen helpen bij het begrijpen van de vereisten, de structuur en de interacties binnen de applicatie.

1. **User Stories**  
   Dit diagram toont de verschillende gebruikers van de applicatie en hun behoeften. User stories helpen om de functionaliteit vanuit het perspectief van de gebruiker te definiëren, waardoor we beter kunnen inspelen op hun verwachtingen en vereisten.

   ![User Stories](https://github.com/deems3/web-app-casus/blob/main/User-Stories1.0.png)

2. **Requirements**  
   Dit document geeft een overzicht van de vereisten voor de applicatie, inclusief zowel functionele eisen (wat de applicatie moet doen) als niet-functionele eisen (hoe de applicatie zich moet gedragen, zoals prestaties en beveiliging). Het helpt bij het vaststellen van de scope van het project en zorgt ervoor dat alle belanghebbenden op dezelfde lijn zitten.

   ![Requirements](https://github.com/deems3/web-app-casus/blob/main/Requirements1.1.png)

3. **Use-Case Diagram**  
   Dit diagram illustreert de interacties tussen de verschillende gebruikers en het systeem. Het geeft inzicht in de verschillende use cases die de applicatie ondersteunt, evenals de rollen van de gebruikers die betrokken zijn bij deze interacties. Dit helpt om te begrijpen hoe gebruikers de applicatie zullen gebruiken en welke functionaliteiten essentieel zijn.

   ![Use-Case Diagram](https://github.com/deems3/web-app-casus/blob/main/Usecase_Diagram1.1.png)

4. **Class Diagram**  
   Dit diagram biedt een gedetailleerd overzicht van de klassen binnen de applicatie, inclusief hun attributen en methoden, evenals de relaties tussen deze klassen. Het is een cruciaal hulpmiddel voor het ontwerp van de applicatiestructuur en helpt bij het implementeren van een goed georganiseerde en onderhoudbare codebasis.

   ![Class Diagram](https://github.com/deems3/web-app-casus/blob/main/Class-Diagram4.0.png)



## Installatie

Volg de onderstaande stappen om de webapplicatie lokaal te installeren en uit te voeren met Visual Studio en .NET 8.0:

1. **Vereisten**
   - Zorg ervoor dat je de volgende software op je systeem hebt geïnstalleerd:
     - [Visual Studio 2022 of hoger](https://visualstudio.microsoft.com/) (met .NET 8.0 SDK)
     - [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
     - [SQL Server Management Studio 19](https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15)

2. **Project Klonen**
   - Open een terminal of opdrachtprompt.
   - Kloneer de repository naar je lokale machine met het volgende commando:
     ```bash
     git clone https://github.com/deems3/web-app-casus.git
     ```
   - Navigeer naar de map van het gekloonde project:
     ```bash
     cd web-app-casus
     ```

3. **Project openen in Visual Studio**
   - Start Visual Studio.
   - Klik op "Open Project" of "Open Solution" en navigeer naar de map van het gekloonde project. Selecteer het `.sln` bestand om het project te openen.

4. **Afhankelijkheden installeren**
   - Controleer of de volgende NuGet-pakketten zijn geïnstalleerd:
     - `Microsoft.AspNetCore.Diagnostics.EntityFramework`
     - `Microsoft.AspNetCore.Identity.EntityFrameworkCore`
     - `Microsoft.AspNetCore.Identity.UI`
     - `Microsoft.EntityFrameworkCore.Sqlite`
     - `Microsoft.EntityFrameworkCore.SqlServer`
     - `Microsoft.EntityFrameworkCore.Tools`
     - `Microsoft.VisualStudio.Web.CodeGeneration.Design`
     - `Swashbuckle.AspNetCore`
   - Als de pakketten nog niet zijn geïnstalleerd, open dan de NuGet Package Manager:
     - Ga naar **Tools** > **NuGet Package Manager** > **Manage NuGet Packages for Solution**.
     - Zoek naar de genoemde pakketten en installeer dezen.

**Database configureren**
   - Het project maakt gebruik van een database. Om deze correct te configureren, moet je de database-update uitvoeren. Open de NuGet Package Manager Console in Visual Studio:
     - Ga naar **Tools** > **NuGet Package Manager** > **Package Manager Console**.
   - Voer de volgende opdracht uit om de database bij te werken:
     ```bash
     update-database
     ```

6. **Applicatie uitvoeren**
   - In Visual Studio, zorg ervoor dat de juiste startup-project is ingesteld.
   - Druk op `F5` of klik op de groene "Start" knop om de applicatie te bouwen en uit te voeren. De applicatie zou nu moeten starten in je webbrowser.



## Licentie

Deze webapplicatie is ontwikkeld voor educatieve doeleinden en mag niet zonder schriftelijke toestemming van de auteurs worden gebruikt, gekopieerd of gedistribueerd. Voor toestemming of meer informatie, neem contact op met de auteurs via Github.


