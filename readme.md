# WSEI Backend Lab

## Autor
- Mateusz Skrzypczak Nr Albumu: 15789
- Piotr Wydymus Nr Albumu: 15760

## Lista zrealizowanych funkcji
Aplikacja została oparta na strukturze **Clean Architecture** z oddzielnymi warstwami (ApplicationCore, Infrastructure, Web, UnitTests).

Oto zrealizowane zadania:
- **Rozszerzenie architektury:** 
  - Dodano encje `Organization` i powiązanie z `Person` (Członkowie / Members).
  - Klasa `Contact` obsługuje polimorfizm dziedziczenia w bazie danych (TPH - Table-Per-Hierarchy).
- **Zarządzanie kontaktami i klasa PESEL:**
  - Dodano możliwość wyszukiwania kontaktów po przynależności do organizacji, firmy oraz po domenie adresu email.
  - Zaimplementowano klasę typu `ValueObject` reprezentującą PESEL (z testami jednostkowymi weryfikującymi datę urodzenia, płeć i cyfrę kontrolną).
  - Zdefiniowano odpowiednią klasę konwersji w Entity Framework Core, aby prawidłowo mapować klasę PESEL na typ `TEXT` w bazie danych.
- **Bezpieczeństwo i autoryzacja:**
  - Wdrożono Identity (użytkownicy i role: m.in. Administrator, SalesManager, itp.).
  - Zabezpieczono dostęp za pomocą autoryzacji opartej o tokeny JWT z obsługą odświeżania (`Refresh Token`).
  - Rozszerzono `Contact` o pole `OwnerId`, tak by aplikacja rejestrowała, kto dodaje kontakt. Zabezpieczono metody modyfikujące (edycję, usuwanie) – teraz akcje te mogą być realizowane wyłącznie przez właściciela zasobu lub administratora.
- **Baza danych:**
  - Implementacja Entity Framework Core używa SQLite (`crm.db`).
  - Użyto mechanizmu Seedera do wygenerowania początkowych danych testowych i operacyjnych (m.in. konta użytkowników i przypisanie własności stworzonych encji kontaktów).
- **Jakość kodu i testy:**
  - Zdefiniowano logiczny podział i zastosowano Dependency Injection do wstrzykiwania repozytoriów, serwisów (Memory i EF Core) oraz konwerterów Auth.
  - Zdefiniowano projekt i napisano podstawę dla testów integracyjnych `App.IntegrationTests`.

## Link do repozytorium (GitHub)
- [Mateusz2051/wsei-backend-lab](https://github.com/Mateusz2051/wsei-backend-lab)
