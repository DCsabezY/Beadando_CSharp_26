## Beadando_CSharp_26
# Movie Processor

Egy egyszerű konzoles C# alkalmazás, amely filmeket tárol és dolgoz fel producer-consumer minta segítségével.

## Mit csinál?

A program két szálat futtat párhuzamosan. A főszál kezeli a menüt és fogadja a filmeket, a háttérszál pedig feldolgozza őket. A filmeket vagy kézzel lehet felvenni, vagy egy pontosvesszővel tagolt `.txt` fájlból lehet betölteni (`Cím;Hossz;Műfaj` formátumban).

## Funkciók

- Film hozzáadása manuálisan
- Filmek betöltése fájlból
- Összes film listázása
- Szűrés műfaj vagy hossz szerint
- Filmek megszámlálása
- Sor törlése

## Fájl formátuma

```
Son of Saul;107;historical drama
Kontroll;105;thriller, dark comedy
```

## Technológiák

- `ConcurrentQueue` – szálbiztos filmsor
- `AutoResetEvent` – szálak közötti jelzés
- `Thread` – párhuzamos feldolgozás

