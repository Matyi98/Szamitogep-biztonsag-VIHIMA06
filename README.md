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

<p align = "center">
1. ábra. A rednszer felhasználási szcenáriói</p>

### Biztonsági követelmények és célok

A funkcionális követelmények ismeretében nagy vonalakban fel tudjuk vázolni a rendszert és annak környezetét, ahogy azt a következő Data Flow ábra mutatja.

![integrity](/img/integrity.png)

<p align = "center">
2. ábra. A CAFF webshop és környezete</p>

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

### Threat Assessment

(TODO: KÖRÜLTEKINTŐ ÁTNÉZÉS, az adafolyam ábrát sokféleképpen lehet elképzelni és nem mentem bele nagyon részletekbe, illetve a parsert innen még kihagytam, mert talán csak az architektúra tervbe kell, de idk, talán ide is belelehet rakni. Illetve az idő részt sem tudom, hogy kellene-e valamihez.)

A threat assessment részt két részre tagoljuk: az assetek azonosítása és assetekre leselkedő veszélyek (threat) azonosítása. Az assetek azonosításához iteratívan elemezzük a rendszer use case-eit, figyelembe véve a biztonsági követelményeket. A veszélyek megállapításához a STRIDE keretrendszer segítségét vettük igénybe.

#### Assetek megállapítása

A funkcionális és biztonsági követelmények ismeretében pontosítjuk a kezdeti adatfolyam diagramot és részletezzük a rendszer komponenseit. A Threat, Risk, Vulnerability Analysis során háromfajta asset halmazt különítünk el:

- Fizikai assetek
- Emberi assetek
- Logikai assetek.

A fizikai assetek meghatározása itt most triviális a use case-ek alapján. Szervergépek kellenek, amin a szoftverek fognak futni, illetve hálózati eszközök, amikkel az internetre csatlakoznak. Az emberi assetek csak kettő lesz, a felhasználó és az admin. A logikai assetek meghatározása nagyobb vizsgálatot igényel, ezzel foglalkozik a következő kettő alfejezet.

##### Felhasználói use-case-ek vizsgálata

A felhasználók többfajta use-case-ben is megjelennek, a biztonsági követelmények kielégítéséhez azonban újabb use case-eket kell felvenni. Az egyes tevékenységek között megkötéseket is felveszünk. A CAFF megtekintésén és a regisztráción kívül a többi folyamathoz belépés szükséges, illetve a belépést megelőzheti a regisztráció. A leírt dolgokat ábrázolja a következő use case diagram.

![use-case-extended-with-securityreqs](/img/use-case-extended-with-securityreqs.png)

<p align = "center">
X. ábra. A felhasználókhoz és adminisztrátorokhoz köthető use-case-ek kiegészítve</p>

A logikai assetek meghatározásához létrehozunk egy adatfolyamot, ami az előző use case megvalósításához szükséges. A megrendelő szeretne távoli elérést a rendszerhez, ezért úgy döntöttünk, hogy a rendszer felületét böngészőben fogjuk megjeleníteni a felhasználók számára. Mivel a legtöbb tevékenység bejelentkezéshez kötött, szükség van egy autentikációt megvalósító komponensre. Ennek a komponensnek szüksége van a felhasználó adataira, ezt egy adatbázisban fogjuk tárolni. A felhasználóknak biztosítani kell, hogy megnézhessék vagy módosíthassák a felhasználói adataikat.

A felhasználók a webáruházban több dolgot is csinálhatnak a CAFF fájlokkal (megtekintés/feltöltés/letöltés/vásárlás). Ezt a CAFF-kezelő logikai asset fogja kezelni. A CAFF-kezelő összeköti a CAFF fájlokat a hozzátartozó megjegyzésekkel, felhasználókkal. Az elmentett CAFF fájlokat a CAFF adatbázisból fogja olvasni.

A felhasználók tudnak a CAFF fájlokhoz megjegyzést hozzáadni/módosítani/sajátot törölni. Ezt a megjegyzés-kezelő asset kezeli. A megjegyzéseket a megjegyzés adatbázisból szedi ki, és összerendeli azokat a felhasználókkal.

Mindegyik adatkezelő logikai asset felhasználja a hozzáférés-védelmi komponenst. Ez a tevékenységeket vagy engedi vagy tiltja attól függően, hogy a felhasználó be van-e jelentkezve, illetve van-e joga az adott művelethez (például: csak saját kommentet törölhet). Az asseteket összefoglaló adatfolyam ábra látható alább.

> Megjegyzés: Az adatfolyam bonyolultsága miatt néhány adatfolyam nyíl számmal van ellátva. Ezek megmutatják, hogy melyik nyíl eleje, melyik nyíl véghez tartozik.

![Data-flow-extended-with-users](/img/Data-flow-extended-with-users.png)

<p align = "center">
X. ábra. A rendszer adatfolyam ábrája a felhasználói interakciók elemzése után</p>

##### Adminisztrátori use-case-ek vizsgálata

Az adminisztrátorok itt most felhasználók, csak több joguk van ahhoz, hogy milyen adatot tudnak módosítani és törölni. A biztonsági követelmények ugyanúgy meghatározzák, hogy ehhez először be kell lépniük, ez a módosítás látható a fentebbi use case ábrán.
Az adminisztrátori folyamatok nem sokat tesznek hozzá az adatfolyam diagramhoz. Az hogy milyen adatot törölhetnek, módosíthatnak, csak a szerepköröktől függ, amire a hozzáférés-védelmi komponens figyel. Ezért a végső ábra csak az adminisztrátor emberi assettel egészül ki.

![data-flow-2](/img/data-flow-2.png)

<p align = "center">
X. ábra. A rendszer adatfolyam ábrája az adminisztrátori interakciók elemzése után</p>

##### Végső assetek meghatározása

- Fizikai assetek: szervergépek, hálózati eszközök
- Emberi assetek: felhasználók, adminisztrátorok
- Logikai assetek: Hozzáférés-védelmi komponens, webszerver, autentikációs komponens, CAFF-kezelő, Megjegyzés-kezelő, felhasználói adatkezelő, CAFF fájlok, hozzászólások, felhasználói adatok.

#### Támadó modell kidolgozása

TODO(átnézés, további lehetséges támadások a többi veszélyforráshoz)

(Az asseteken végighaladva megnézhetjük, hogy milyen veszélyek fenyegethetik és ebből tudjuk levezetni a támadásokat, amik ezeket érhetik. Ezeket, hogy megállítsuk, szükségünk van többfajta védelemre.)

A támadó modell kidolgozásához számba kell vennünk az egyes assetek potenciális gyengeségeit és az azokra leselkedő veszélyeket. A veszélyforrások rendszerezéséhez segítséget nyújt a STRIDE keretrendszer, melynek elemei a következők:

- Megszemélyesítés (spoofing)
- Hamisítás (tampering)
- Tevékenységek letagadása (repudiation)
- Információ szivárgás (information disclosure)
- Szolgáltatás-megtagadás (denial of service)
- Jogosultsági szint emelése (elevation of privilege)

Az egyes veszélyforrás kategóriák könnyen összerendelhetők az adatfolyam diagram egyes elemeivel, pl. információ szivárgás veszélyeztethet belső folyamatokat, tárolt adatokat és adatfolyamokat. A támadó modell összeállításához különböző támadási szcenáriókat, ún. abuse case-eket is fel kell sorolnunk.

Megszemélyesítéssel kapcsolatos veszélyek forrása lehet bármelyik külső szereplő, akivel interakcióba lépünk: elképzelhető, hogy a támadók felhasználónak vagy adminisztrátornak adják ki magukat és az ő nevükben próbálnak meg kéréseket intézni a rendszerhez. Néhány lehetséges támadás:

- Egy felhasználó megpróbál hozzáférni egy másik felhasználó adataihoz, pedig ehhez csak az adott felhasználó és az adminisztrátorok férhetnek hozzá
- Egy felhasználó megpróbál adatokat törölni vagy módosítani, pedig ezt csak az adminisztrátorok tehetnék meg

Megszemélyesítéssel a belső folyamatok között is számolni kell. Amennyiben az autentikációt külön fizikai szerveren valósítjuk meg, akkor hálózati kéréseket kell egymásnak küldenie a webszervernek és az autentikációs szervernek. Amennyiben a támadók hozzáférnek pl. a webszerver és az autentikációs szerver közötti hálózathoz, megpróbálhatják valamelyik komponenst megszemélyesíteni.

Hamisítással kapcsolatos veszélyekre kell felkészülnünk a belső folyamatok, adattárak és adatfolyamok megvalósítása során. A belső folyamatokat sokféleképpen támadhatják, pl. implementációs sérülékenységek kihasználásával megváltoztathatják a támadók a komponensek viselkedését (példa: SQL injection?). Az adattárak kompromittálása során hamis adatokat helyezhetnek el az adatbázisokban, pl. jogtalanul megváltoztathatják az egyes felhasználók szerepköreit vagy meg nem vásárolt CAFF-okat rendelnek a felhasználókhoz.Az adatfolyamok esetében a komponensek közötti kérések és válaszok manipulációjáról beszélhetünk.

TODO: jobb leírások

Szolgáltatás-megtagadás: DOS támadás

Információ szivárgás: veszélyeztethet belső folyamatokat, tárolt adatokat és adatfolyamokat. HTTPS használata?

Tevékenységek letagadása: Nem megfelelő logolás, támadó letudja tagadni a tettét?

Jogosultsági szint emelése: Saját magának beállítja, hogy admin

### Szükséges biztonsági funkcionalitások

TODO(átnézés)

A biztonsági követelmények kielégítéséhez többféle biztonsági funkcionalitást kell megterveznünk, implementálnunk és tesztelnünk. A szükséges biztonsági funkcionalitások listáját a biztonsági követelmények és abuse case-ek elemzésével kaphatjuk meg.

A webshop használatához(feltötlés, letöltés) autentikációt kell megvalósítanunk. Általánosságban az autentikáció lehet jelszó alapú, hardver token alapú vagy biometrikus azonosításra épülő autentikáció. Mivel a tervezett rendszerrel böngészőn keresztül léphetnek kapcsolatba a felhasználók, érdemes jelszó alapú autentikációt választani.

A felhasználókhoz kétféle szerepkört tudunk meghatározni: felhasználó és adminisztrátor. Az egyes tevékenységek csak bizonyos szerepkörbe tartozó felhasználók számára elérhetőek, ezért a tevékenységek elvégzése előtt ellenőriznünk kell, hogy az adott felhasználó jogosult-e a tevékenységre. Ehhez szerep alapú autorizációs mechanizmust kell implementálnunk.

Az adminisztrátorok tevékenységét nem igazán korlátozzák a biztonsági követelmények: hozzáférhetnek személyes adatokhoz, adatokat törölhetnek, módosíthatnak, CAFF-okat tölthetnek fel illetve le. Az ő elszámoltathatóságához fontos a tevékenységek naplózása.

A felhasználók személyes adatait és az autentikációhoz szükséges jelszót védenünk kell szivárgás és illetéktelen hozzáférés ellen. Ezért a személyes adatokat titkosítanunk kell tárolás és átvitel során, a jelszavakat pedig biztonságosan kell tárolnunk (hashelés és salt-olás). A felhasználói fiókok biztonságához jelszó policy-t dolgozhatunk ki (pl. hány és milyen típusú karakterből álljon a jelszó).

## Architektúra tervek

A követelmények ismeretében elkezdhetjük megtervezni a tanulmányi rendszert. A terveket többféle UML diagram segítségével vizualizálhatjuk, mi a rendszer a struktúráját komponens diagramon, viselkedését pedig szekvencia diagrammon keresztül szemléltetjük.

### Rendszer struktúrája

A rendszer több komponensből áll, amiket a X. ábra szemléltet. A rendszer összesen 9 interfészt nyújt a felhasználók számára.

- Regisztráció: Ezen az interfészen keresztül tudnak a felhasználók felhasználói fiókot létrehozni saját maguknak.
- Bejelentkezés: A felhasználók ezen az interfészen keresztül tudnak bejelentkezni a rendszerbe.
- Felhasználói adat módosítása: A felhasználók ezen az interfészen keresztül tudják módosítani személyes adataikat és jelszavukat.
- Felhasználói adat lekérdezése?: Ezen az interfészen keresztül lehet lekérni egy-egy felhasználó adatait.
- CAFF keresése: Ezen az interfészen keresztül tudnak keresni CAFF-okat a felhasználók.
- CAFF megvásárlása: Ezen az interfészen keresztül tudnak vásárolni CAFF-ot a felhasználók.
- CAFF letöltése: Ezen az interfészen keresztül tudnak CAFF-ot letölteni a felhasználók.
- CAFF feltöltése: Ezen az interfészen keresztül tudnak CAFF-ot feltölteni a felhasználók.
- CAFF-hoz megjegyzés fűzése: Ezen az interfészen keresztül tudnak CAFF-hoz megjegyzést fűzni a felhasználók.
- Megjegyzés módosítása? (vagy ez mehet a megjegyzés fűzéséhez)
- Admin adat módosítás, törlés?

![Component-diagram](/img/Component-diagram.png)

<p align = "center">
X. ábra. A CAFF webshop rendszer komponens diagramja</p>

A felhasználói és személyes adatokat a felhasználói adatbázisban tároljuk. Az adatok kezeléséhez egy különálló logikai komponenst fogunk megvalósítani. A CAFF-ok, megjegyzések tárolása és kezelése ugyanilyen séma szerint lesz implementálva.

TODO: architektúra leírása a komponens diagram alapján folyt köv.

### A rendszer viselkedése

A rendszer viselkedésének tervezéséhez minden use-case-hez külön-külön 
diagrammot készítünk az átláthatóság kedvéért.

TODO: Szekvencia diagrammok

#### Regisztráció

![sequence-registration](/img/sequence-registration.png)

#### CAFF letöltése

![seq-diagram-upload](/img/sequence-caffload.png)

#### CAFF feltöltése

![seq-diagram-upload](/img/seq-diagram-upload.png)

#### Felhasználói adat módosítása

![user-data-modification](/img/User_data_modification.png)

> Megjegyzés: Az admin adat módosítása is ugyanígy történik, ahhoz nem készült külön szekvencia diagram. 

#### Felhasználói adat lekérdezése

![user-data_query](/img/User_data_query.png)

## Tesztelési terv

### Funkcionális tesztelés

- A fontosabb funkciókhoz unit teszteket készítünk (például a parserhez, CAFF kezelőhöz).
- Integrációs tesztelést végzünk (például CAFF kezelőnél).
- Az elkészült alkalmazáson manuális teszteléseket végzünk, az összes funkciót kipróbáljuk.
- Ellenőrizzük, hogy az elkészült funkciók megvalósítják-e a követelményekben leírtakat (verifikáció).

### Biztonsági tesztelés

#### Kódolási szabványok:

- Ne legyen benne memory leak a C++ részben
- Az adatbázis eléréséhez entity frameworkot használunk, ezzel számos potenciális biztonsági kockázatot elkerülve (pl. sql injection)
- Szálkezelés implementálására async-await-et használunk, ezzel leegyszerűsítve a szálkezelést.
- HTML sanitizert használunk XSS és egyéb támadások ellen.
- Alkalmazzuk a fontosabb OOP elveket (pl. encapsulation)
- Statikus ellenőrzőket használunk (Pl. Visual Studio code analysis, cppcheck, sonarqube-ot, roslyn analyzer) szerveroldalon és C++ kódnál, a parserhez is.
- A natív C++ CAFF parser dinamikus tesztelését az afl fuzzer program segítségével végezzük el. Ezzel különböző bemenetekkel végigmehetünk a parser útvonalain, elmentve mikor crashel vagy akad el a program. A talált hibákat pedig a valgrind program segítségével detektáljuk és javítjuk ki.
