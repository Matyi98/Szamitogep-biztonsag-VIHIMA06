# Szamitogep-biztonsag-VIHIMA06

## Követelmények

### Funkcionális követelmények

Az alábbi lista tartalmazza a funkcionális követelményeket:

- Felhasználóknak kell tudni regisztrálni és belépni
- Felhasználóknak kell tudni CAFF fájlt feltölteni, letölteni és keresni
- Más felhasználók CAFF fájl letöltéséhez meg kell vásárolni a fájlt, de az áruházban minden ingyen letölthető.
- Felhasználóknak kell tudni CAFF fájlhoz megjegyzést hozzáfűzni
- A rendszerben kell legyen adminisztrátor felhasználó, aki tud adatokat módosítani és törölni.

![use-case](/img/use-case.png)

### Biztonsági követelmények és célok

A funkcionális követelmények ismeretében nagy vonalakban fel tudjuk vázolni a rendszert és annak környezetét, ahogy azt a következő Data Flow ábra mutatja.

![integrity](/img/integrity.png)

A CAFF webáruházat az alapvető felhasználók és adminisztrátorok használhatják., velük kerülhet interakcióba a rendszer. Mivel a tőlük érkező kéréseket, a viselkedésüket nem tudjuk kontrollálni, ezért a velük történő interakció bizalmi kéréseket vet fel. Az előző ábrán ezt szaggatott piros vonallal jelezzük. A rendszer működtetéséhez szükség lesz a felhasználók és adminisztrátorok adatainak tárolására, a CAFF fájlok tárolására, illetve a fájlokhoz tartozó kommentek elmentésére. A tárolt adatok között kapcsolatot is kell létesíteni, a felhasználót össze kell kötni a saját feltöltött CAFF fájljaival, a hozzáadott kommentjeivel és azzal, hogy milyen CAFF fájlokat vásárolt meg.

A CAFF webáruház, mint szoftver, biztonsági követelményeit hat nagy kategóriába soroljuk: CIA és AAA. Az egyes kategóriákhoz az alábbi biztonsági követelményeket határozhatjuk meg.

- Bizalmasság (confidentiality)

  - A felhasználók felhasználó neve mindenki számára látható, a többi személyes adathoz csak saját maguk és az adminisztrátorok férhetnek hozzá.

  - A felhasználók CAFF-ok alá tűzött kommentjeit mindenki láthatja.

  - A felhasználók meg tudják nézni, hogy mely CAFF-okat vásárolták meg, de csak a sajátjukat. Az adminisztrátorok bárki megvásárolt CAFF-jait láthatják.

- Integritás (integrity)

  - A felhasználók adatait csak saját maguk, vagy az adminisztrátorok módosíthatják.

  - A felhasználók csak a saját megjegyzéseiket tudják törölni és szerkeszteni. Az adminisztrátorok bárki hozzászólását tudják törölni.

- Elérhetőség (availability)

  - Karbantartási időn kívül, a rendszer legyen mindig elérhető.

  - A törölt CAFF-ok akkor sem lesznek elérhetőek az oldalon, ha valaki megvette.

- Autentikáció (authentication)

  - A felhasználók bejelentkezés nélkül tudnak a CAFF-ok között böngészni, keresni.

  - A felhasználók csak bejelentkezés után tudnak CAFF-ot feltölteni, CAFF alá megjegyzést írni és CAFF-ot megvenni.

- Autorizáció (authorization)

  - CAFF törléséhez csak a feltöltőnek és az adminisztrátoroknak van joga.

  - Megjegyzés módosításához csak a megjegyzés írójának van joga. Törléshez az adminisztrátoroknak is.

- Auditálás (auditing)

  - A felhasználó és adminisztrátori tevékenységeket naplózni kell.

TODO

![use-case-2](/img/use-case-2.png)

TODO: make similar use-case for admin

### Threat Assessment

A threat assessment részt két részre tagoljuk: az assetek azonosítása és assetekre leselkedő veszélyek (threat) azonosítása. Az assetek azonosításához iteratívan elemezzük a rendszer use case-eit, figyelembe véve a biztonsági követelményeket. A veszélyek megállapításához a STRIDE keretrendszer segítségét vettük igénybe.

#### Assetek megállapítása

A funkcionális és biztonsági követelmények ismeretében pontosítjuk a kezdeti adatfolyam diagramot és részletezzük a rendszer komponenseit. A Threat, Risk, Vulnerability Analysis során háromfajta asset halmazt különítünk el: (TODO: ezt még ki kell egészíteni)

- Fizikai assetek: gépek, amin a szerverek futnak
- Emberi assetek: felhasználók, adminisztrátorok
- Logikai assetek: adatok (CAFF fájlok)

TODO: A következőt kell végig csinálni (Két use-case-el végig -> felhasználó és admin)

- Megnézni, hogy egy use-case hogy bővül, ha belevisszük a biztonsági követelményeket és kell a bővített use-case diagram
- Leírni milyen emberi asset van benne(triviális: felhasználó vagy admin)
- Leírni, hogy milyen logikai assetek tartoztank egy use-case (ezek a különböző komponensek pl: Authentikációs komponens és hozzá az adatfolyam ábra)

#### Támadó modell kidolgozása

TODO

Az asseteken végighaladva megnézhetjük, hogy milyen veszélyek fenyegethetik és ebből tudjuk levezetni a támadásokat, amik ezeket érhetik. Ezeket, hogy megállítsuk, szükségünk van többfajta védelemre.


## Architektúra tervek

TODO

## Tesztelési terv

TODO: Normális leírás, technológiák átbeszélése

Statikus analízis cucc:

- C++ parser: cppcheck? (open-source, könnyen használható)
- Full stack rész(C#): Sonarqube szerintem itt

Fuzzer:

- C++ parser: afl fuzzer, valgrind bugok megtalálásához
- Full stack rész(C#): elvileg van afl fork ehhez, de nem tudom, hogy szükséges-e

Unit tesztek?
