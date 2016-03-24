# Sprint 1 - komunikacja
Grupa 16-pl-13 - Michał Berent, Marcin Chudy, Mateusz Jabłoński, Maciej Lewinski
## Lista zadań

| Zadanie                                                                                               | Priorytet | Osoba | Koszt(h) | Rzeczywisty czas(h) |
|-------------------------------------------------------------------------------------------------------|-----------|-------|----------|---------------------|
| Obsługa pliku konfiguracyjnego w serwerze                                                             | Wysoki    | MJ    | 2        | 2                   |
| Obsługa pliku konfiguracyjnego w pozostałych komponentach                                             | Wysoki    | MB    | 2        | 2                   |
| Obsługa parametrów z linii poleceń                                                                    | Niski     | ML    | 2        | 4                   |
| Stworzenie klas reprezentujących wiadomości                                                           | Wysoki    | MJ    | 2        | 4                   |
| Mechanizm przesyłania wiadomości                                                                      | Wysoki    | MC    | 8        | 20                  |
| Obsługa awarii węzła                                                                                  | Normalny  | MJ    | 6        | 4                   |
| Dodanie trybu backup dla serwera                                                                      | Niski     | MC    | 5        | 7                   |
| Obsługa awarii serwera                                                                                | Normalny  | ML    | 8        | 15                  |
| Stworzenie mocków umożliwiających testowanie przepływu przykładowych wiadomości pomiędzy komponentami | Wysoki    | ML    | 6        | 7                   |
| Testy integracyjne komunikacji między komponentami                                                    | Wysoki    | MB    | 6        | 5                   |
| Dodanie trybu verbose                                                                                 | Niski     | MB    | 3        | 2                   |

### Szacunki czasowe
Część oszacowań znacząco odbiegła od rzeczywistego kosztu pracy. Powodem tego były liczne komplikacje, często związane z asynchronicznością wykonywanych operacji. Ponadto konieczne było uwzględnienie wszystkich zmian w specyfikacji i dostosowanie się do nich.

## Spotkania zespołu
Podczas sprintu odbyło się spotkanie w celu omówienia dotychczasowego postępu, wyjaśnienia dotychczasowej architektury systemu i zaplanowania pracy na kolejne dni. Do organizacji pracy i śledzenia zadań na bieżąco używaliśmy aplikacji Trello.

## Zgodność z XP
Podczas prac nad sprintem dużą rolę odegrało w szczególności dążenie do możliwie prostej architektury wystarczającej do poprawnego działania wymaganych funkcjonalności oraz ciągła refaktoryzacja kodu. Architektura została w dużym stopniu przebudowana po oddaniu wersji testowej.
Test Driven Development nie został zastosowany w pełni, jedynie dla kluczowych elementów architektury został położony nacisk na pisanie testow przed pisaniem kodu. Dla części logiki przepływu wiadomości, ze względu na ciągłe istotne zmiany w architekturze i związany z tym brak czasu na częste poprawianie i przepisywanie testów, zostały one napisane nieco później.
