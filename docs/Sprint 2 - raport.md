# Sprint 2 - obliczenia
Grupa 16-pl-13 - Michał Berent, Marcin Chudy, Mateusz Jabłoński, Maciej Lewinski

### Lista zadań

| Zadanie                                                                      | Priorytet | Osoba | Szacowany  koszt(h) | Rzeczywisty  koszt(h) |
|------------------------------------------------------------------------------|-----------|-------|---------------------|-----------------------|
| Parsowanie formatu DVRP i możliwość podania ścieżki jako parametru w konsoli | Normalny  | MJ    | 6                   |8                      |
| Automatyczne testy dla TaskSolvera DVRP                                      | Wysoki    | MB/ML | 8                   |6                      |
| Stworzenie mechanizmu dynamicznego ładowania dll TaskSolverów (pluginy)      | Normalny  | MC    | 5                   |3                      |
| Implementacja algorytmu dla jednego wątku                                    | Wysoki    | MB/ML | 15                  |10                     |
| Zrównoleglenie algorytmu na wiele wątków                                     | Normalny  | MC/MB | 5                   |10                     |
| Obsługa kolejki serwerów backupowych (Zaległe)                               | Wysoki    | MC    | 6                   |5                      |
| Optymalizacja działania algorytmu                                            | Niski     | MB/ML | 20                  |7 (wyk. częściowo)     |

### Szacunki czasowe oraz planowanie zadań
Szacunki czasowe były znacznie dokładniejsze niż w przypadku sprintu 1. Powodem tego był mniejszy oraz bardziej doprecyzowany obowiązujący zakres wymaganych funkcjonalności. Największy problem 
sprawiła implementacja wystarczająco wydajnego Również podział zadań okazał się
łatwiejszy niż w przypadku poprzedniego sprintu i nie było konieczne (poza bieżącymi poprawkami - hotfixami) dodawanie nowych lub istotne zmienianie utworzonych zadań. Podstawowa architektura aplikacji
nie wymagała znaczących zmian, natomiast dopuszczalna jest jej znacząca przebudowa w ramach kolejnego sprintu. Ponadto planowana jest dalsza optymalizacja działania algorytmu.

### Komunikacja w zespole
Większość komunikacji w zespole odbywała się zdalnie oraz podczas laboratoriów. Ponadto do organizacji pracy używaliśmy aplikacji Trello - umożliwiła ona łatwy i szybki sposób utrwalania bieżących problemów, 
spostrzeżeń czy przydatnych linków. Ponadto odbyło się krótkie spotkanie zespołu w celu podziału pracy oraz dyskusji nad algorytmem rozwiązywania problemu DVRP.

### Zgodność z XP
Dla części klas Task Solvera testy były pisane równolegle z kodem, w zgodności z praktyką Test Driven Development. Architektura Task Solvera nie została ściśle zaplanowana z góry i uległa dużej refaktoryzacji 
podczas dostosowywania Task Solvera, aby działała we współpracy z istniejącymi komponentami (tj. Węzłami Obliczeniowymi i Menedżerami Zadań). Jak zakłada programowanie ekstremalne, podczas
prac nad określonymi komponentami wprowadzane zostały również poprawki w innych częściach systemu, związanych np. z komunikacją sieciową.

### Działanie i osiągnięta architektura
Stworzony system oparty na pluginach umożliwia łatwe rozszerzanie aplikacji o możliwość rozwiązywania innych problemów. Konieczne jest jedynie dostarczenie dll z klasą będącą klasą pochodną
`UCCluster.UCCTaskSolver` oraz umieszczenie jej w folderze `taskSolvers` w lokalizacji zawierającej pliki wykonywalne Węzła Obliczeniowego lub Menedżera Zadań (domyślnie `build`). Klient może być wywołany następująco dla problemu zdefiniowanego w pliku `problem.vrp`:
```
> .\ComputationalCluster.Client.exe -address server_address -port server_port problem.vrp
```
Program po otrzymaniu rozwiązania wypisuje je na ekran oraz zapisuje do pliku tekstowego.