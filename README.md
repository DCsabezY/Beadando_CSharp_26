
## Beadando_CSharp_26
# Movie Processor

Egy egyszerű konzolos C# alkalmazás, amely filmeket tárol és dolgoz fel producer-consumer minta segítségével.

## Mit csinál?

A program két szálat futtat párhuzamosan. A főszál kezeli a menüt és fogadja a filmeket, a háttérszál pedig feldolgozza őket. A filmeket vagy kézzel lehet felvenni, vagy egy pontosvesszővel tagolt `.txt` fájlból lehet betölteni.

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

## Miért ezeket használtam

A feladat 3 konkrét .NET eszköz (`ConcurrentQueue`, `AutoResetEvent`, `EventWaitHandle`) használatát írta elő, saját wrapper osztályokon keresztül (`OwnConcurrentQueue`, `OwnAutoResetEvent`, `OwnEventWaitHandle`).

- **`ConcurrentQueue`** – mivel több szál is olvassa/írja ugyanazt a sort, ez teszi lehetővé a szálbiztos hozzáférést lock használata nélkül.
- **`AutoResetEvent`** – a producer (főszál) ezzel jelzi a consumernek (háttérszál), hogy van feldolgozandó film. Mivel csak egy consumer szál fut, minden jelzésnek pontosan egy ébresztést kell kiváltania, automatikus visszaállással — erre való az AutoResetEvent.

A leállítás sorrendje is fontos: `Stop()` → `Join()` → `Close()`, hogy a consumer szál veszteség nélkül tudja befejezni a munkáját, mielőtt az erőforrások felszabadulnak.

## Futtatás

```
dotnet run
```

