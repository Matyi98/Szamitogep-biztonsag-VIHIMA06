# Szamitogep-biztonsag-VIHIMA06

## Követelmények

### Funkcionális követelmények

Az alábbi lista tartalmazza a funkcionális követelményeket:

- Felhasználóknak kell tudni regisztrálni és belépni.
- Felhasználóknak kell tudni CAFF fájlt feltölteni, letölteni és keresni.
- Más felhasználók CAFF fájl letöltéséhez meg kell vásárolni a fájlt, de az áruházban minden ingyen letölthető.
- Felhasználóknak kell tudni CAFF fájlhoz megjegyzést hozzáfűzni.
- A rendszerben kell legyen adminisztrátor felhasználó, aki tud adatokat módosítani és törölni.

![use-case](/img/use-case.png)

<p align = "center">
1. ábra. A rednszer felhasználási szcenáriói</p>

### Biztonsági követelmények és célok

![integrity](/img/integrity.png)

<p align = "center">
2. ábra. A CAFF webshop és környezete</p>

A CAFF webáruházat az alapvető felhasználók és adminisztrátorok használhatják, velük kerülhet interakcióba a rendszer. Mivel a tőlük érkező kéréseket, a viselkedésüket nem tudjuk kontrollálni, ezért a velük történő interakciók bizalmi kérdéseket vetnek fel. Az előző ábrán ezt szaggatott piros vonallal jeleztük. A rendszer működtetéséhez szükség lesz a felhasználók és adminisztrátorok adatainak tárolására, a CAFF fájlok tárolására, illetve a fájlokhoz tartozó kommentek elmentésére. A tárolt adatok között kapcsolatot is kell létesíteni, a felhasználót össze kell kötni a saját feltöltött CAFF fájljaival, a hozzáadott kommentjeivel és azzal, hogy milyen CAFF fájlokat vásárolt meg.

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

A felhasználók a webáruházban több dolgot is csinálhatnak a CAFF fájlokkal (megtekintés/feltöltés/vásárlás). Ezt a CAFF-kezelő modul fogja kezelni. A CAFF-kezelő összeköti a CAFF fájlokat a hozzátartozó megjegyzésekkel, felhasználókkal. Az elmentett CAFF fájlokat a CAFF adatbázisból fogja olvasni.

A felhasználók tudnak a CAFF fájlokhoz megjegyzést hozzáadni/módosítani/sajátot törölni. Ezt a megjegyzés-kezelő asset kezeli. A megjegyzéseket a megjegyzés adatbázisból szedi ki, és összerendeli azokat a felhasználókkal.

Mindegyik adatkezelő logikai asset felhasználja a hozzáférés-védelmi komponenst. Ez a tevékenységeket vagy engedi vagy tiltja attól függően, hogy a felhasználó be van-e jelentkezve, illetve van-e joga az adott művelethez (például: csak saját kommentet törölhet). Az asseteket összefoglaló adatfolyam ábra látható alább.

> Megjegyzés: Az adatfolyam bonyolultsága miatt néhány adatfolyam-nyíl számmal van ellátva. Ezek megmutatják, hogy melyik nyíl eleje, melyik nyíl véghez tartozik.

![Data-flow-extended-with-users](/img/Data-flow-extended-with-users.png)

<p align = "center">
X. ábra. A rendszer adatfolyam ábrája a felhasználói interakciók elemzése után</p>

##### Adminisztrátori use-case-ek vizsgálata

Az adminisztrátorok itt most felhasználók, csak több joguk van ahhoz, hogy milyen adatot tudnak módosítani és törölni. A biztonsági követelmények ugyanúgy meghatározzák, hogy ehhez először be kell lépniük, ez a módosítás látható a fentebbi use case diagrammon.

Az adminisztrátori folyamatok nem sokat tesznek hozzá az adatfolyam diagramhoz. Az hogy milyen adatot törölhetnek, módosíthatnak, csak a szerepköröktől függ, amire a hozzáférés-védelmi komponens figyel. Ezért a végső ábra csak az adminisztrátor emberi assettel egészül ki.

![data-flow-2](/img/data-flow-2.png)

<p align = "center">
X. ábra. A rendszer adatfolyam ábrája az adminisztrátori interakciók elemzése után</p>

##### Végső assetek meghatározása

- Fizikai assetek: szervergépek, hálózati eszközök
- Emberi assetek: felhasználók, adminisztrátorok
- Logikai assetek: Hozzáférés-védelmi komponens, webszerver, autentikációs komponens, CAFF-kezelő, Megjegyzés-kezelő, felhasználói adatkezelő, CAFF fájlok, hozzászólások, felhasználói adatok.

#### Támadó modell kidolgozása

Potenciálisan sérülékeny assettek:

- szervergép: A szervergépet jelszóval tervezzük védeni. Fizikai védelemre nem készülünk. Egy ilyen kis jelentőségű, nem kritikus rendszer esetén ez szükségtelen.

- Emberi assettek:
  - A felhasználók esetén potenciális veszély a felhasználói fiók elvesztése, bejelentkezve maradás publikus gépen, megtippelhető jelszó választása. Ez információ jogosulatlan hozzáférést okozhat, ez ellen kellően erős jelszó megkövetelésével és lejáró bejelentkezéssel védekezünk.
  - Adminok esetén is fent állnak ugyan ezek a problémák. A session token ellopása ellen védekezünk a lejárati idő beállításával. Az admin fiókot megfelelő jelszóval fogjuk védeni. A választott keretrendszer támogatja a kétfaktoros autentikációt is, de házi feladatban ezt nem fogjuk bekapcsolni.

- Szoftveres assettek:
  - CAFF kezelő: implementációs hiba esetén lehetséges, hogy a támadó olyan CAFF fájlt módosít, amihez nincs joga.
  - Megjegyzések: A megjegyzések egy potenciális XSS és SQL injection támadási front, hiszen felhasználói input fog adatbázisba íródni, majd a weboldalon újra betöltődni. Az XSS ellen HTML sanitizerrel fogunk védekezni az SQL injection ellen megfelelő keretrendszerrel és query paraméterekkel.
  - CAFF kezelő: túl nagy és túl sok feltöltéssel DOS támadást lehet intézni

Abuse-case-ek kategorizálva:

- Megszemélyesítés (spoofing)

  - Lehetséges támadások, amelyek helyes hozzáférés védelemmel könnyedén kivédhetőek:

    - Egy felhasználó megpróbál hozzáférni egy másik felhasználó adataihoz, pedig ehhez csak az adott felhasználó és az adminisztrátorok férhetnek hozzá.

    - Egy felhasználó megpróbál adatokat törölni vagy módosítani, pedig ezt csak az adminisztrátorok tehetnék meg.

    - Egy felhasználó más nevében próbál meg felölteni CAFF-ot, ezzel például lejáratva az eredeti művészt.

  - Szükséges megfelelő autentikáció, hogy ne lehessen mást hamisan megszemélyesíteni. Helyesen implementált autentikáció és autorizáció esetén az egyetlen megszemélyesítési támadási lehetőség a bejelentkezési adatok ellopása, vagy bejelentkezve maradt fiókhoz illetéktelen hozzáférés. Felhasználók esetén ez ellen oly szinten kívánunk védekezni, hogy megfelelő erősségű jelszót várunk el, lehetőséget biztosítunk a jelszó megváltoztatására automatikusan kijelentkeztetjük egy idő után. Ezen kívül a felhasználóknak maguknak kell figyelniük arra, hogy ne adják ki adataikat.

- Hamisítás (tampering)

  - A kliens és a webszerver között lehetséges ilyen támadás, de ezek ellen a HTTPS véd.

  - A rendszer többi komponensére azt feltételezzük, hogy egy megbízható védett hálózaton helyezkednek el. A belső hálózatról feltételezzük, hogy nem férnek hozzá támadók.

- Tevékenységek letagadása (repudiation)

  - Egy admin fiókkal rosszindulatú módosítást hajtanak végre. Naplózni fogunk minden adatmódosítással járó eseményt.

- Információ szivárgás (information disclosure)

  - Valaki nyílt wifiről jelentkezik be és ezáltal kiszivároghat a jelszava. Ez ellen HTTPS-sel fogunk védekezni. Úgy fogjuk konfigurálni a webszervert, hogy mindig átirányít HTTPS-re.

  - A szolgáltatás komponensei egy védett hálózaton fognak majd egymással kommunikálni.

  - Egy admin fiók kompromitálódása esetén a támadó hozzáférhet a felhasználók adataihoz. A felhasználókról nem fogunk érzékeny adatokat tárolni. A jelszavakat megfelelően fogjuk tárolni: (salt + hash).

- Szolgáltatás-megtagadás (denial of service)

  - Kis szakmai tudású és kevés erőforrással rendelkező támadók ellen kívánunk védekezni. Lényegében néhány inkognító ablakból ne tudja egy támadó leterhelni a rendszert. Ez ellen úgy kívánunk védekezni, hogy be kelljen jelentkezni a feltöltéshez letöltéshez. Limitáljuk a feltöltési méretet és feltölthető CAFF-ok számát.

  - Sok erőforrással rendelkező összehangolt DDOS támadás ellen nem védekezünk. Egy ilyen kis jelentőségű rendszer esetén ez túl költséges lenne.

- Jogosultsági szint emelése (elevation of privilege)

  - Egy felhasználó megpróbál admin jogokat szerezni, pl kliens oldalon user id módosításával. Ez megfelelő jogosultságkezeléssel és szerver oldali ellenőrzéssel könnyedén kivédhető.

### Szükséges biztonsági funkcionalitások

Jelszó alapú autentikációt fogunk megvalósítani. A választott keretrendszer támogat kétfaktoros autentikációt is, viszont ezt a házi feladatban nem fogjuk bekapcsolni.

A felhasználókhoz kétféle szerepkört tudunk meghatározni: felhasználó és adminisztrátor. Az egyes tevékenységek csak bizonyos szerepkörbe tartozó felhasználók számára elérhetőek, ezért a tevékenységek elvégzése előtt ellenőriznünk kell, hogy az adott felhasználó jogosult-e a tevékenységre. Ehhez szerep alapú autorizációs mechanizmust kell implementálnunk. Ezen kívül szükség lesz még egyedi autorizációs szabályokra, hiszen vannak olyan műveletek amiket mindenki el tud általánosan végezni, de csak a saját adatain. Minden jogosultság ellenőrzést szerver oldalon kell elvégezni.

Az adminisztrátorok tevékenységét nem igazán korlátozzák a biztonsági követelmények: hozzáférhetnek személyes adatokhoz, adatokat törölhetnek, módosíthatnak, CAFF-okat tölthetnek fel illetve le. Az ő elszámoltathatóságához fontos a tevékenységek naplózása.

A felhasználók személyes adatait és az autentikációhoz szükséges jelszót védenünk kell szivárgás és illetéktelen hozzáférés ellen. Ezért a személyes adatokat titkosítanunk kell tárolás és átvitel során, a jelszavakat pedig biztonságosan kell tárolnunk (hashelés és salt-olás). Átvitel során HTTPS-sel titkosítunk. Tárolásnál pedig nem tárolunk érzékeny személyes adatot.

A megjegyzések írása egy XSS támadási front. Itt HTML sanitizert fogunk alkalmazni.

A CAFF fájlokat feltöltéskor validálni fogjuk. Minden feltöltött CAFF fájlt ellenőrzünk, hogy megfelel-e a szabványnak.

Minden műveletet szerver oldalon ellenőrzünk.

A session érvényességét szerver oldalon ellenőrizzük. Figyelünk, hogy ne lehessen meghamisítani, de a session lopás ellen nem tervezünk védekezni.

## Architektúra tervek

### Rendszer struktúrája

A rendszer több komponensből áll, amiket a X. ábra szemléltet. A rendszer összesen 9 interfészt nyújt a felhasználók számára.

- Regisztráció: Ezen az interfészen keresztül tudnak a felhasználók felhasználói fiókot létrehozni saját maguknak.
- Bejelentkezés: A felhasználók ezen az interfészen keresztül tudnak bejelentkezni a rendszerbe.
- Felhasználói adat módosítása: A felhasználók ezen az interfészen keresztül tudják módosítani személyes adataikat és jelszavakat.
- Felhasználói adat lekérdezése?: Ezen az interfészen keresztül lehet lekérni egy-egy felhasználó adatait.
- Admin adat módosítás, törlés: Ezen az interfészen keresztül tudnak az adminisztrátorok adatot módosítani, törölni.
- CAFF letöltése: Ezen az interfészen keresztül tudnak CAFF-ot letölteni a felhasználók. (Ez egyben a vásárlás is)
- CAFF feltöltése: Ezen az interfészen keresztül tudnak CAFF-ot feltölteni a felhasználók.
- CAFF-hoz megjegyzés fűzése: Ezen az interfészen keresztül tudnak CAFF-hoz megjegyzést fűzni a felhasználók.
- CAFF keresése: Ezen az interfészen keresztül tudnak keresni CAFF fájlokat a felhasználók.

A felhasználói és személyes adatokat a felhasználói adatbázisban tároljuk. Az adatok kezeléséhez egy különálló logikai komponenst fogunk megvalósítani. A CAFF-ok és a hozzátartozó megjegyzések ugyanilyen séma szerint lesznek implementálva (CAFF adatkezelő, CAFF adatbázis). Mivel a CAFF fájlokhoz és a hozzátartozó megjegyzéseket lekérdezésnél össze kell rendelni a megfelelő felhasználóval, ezért létrehozunk egy azonosító lekérése nevű interfészt a két logikai komponens között. A CAFF fájlok és megjegyzésekért felelős CAFF adatkezelő komponensnek szerepkörhöz kötötten kell döntenie az egyes tevékenységek engedélyezéséről vagy tiltásáról. A felhasználók szerepkörét az erre dedikált interfészén keresztül kérdezheti le. Mindkét adatkezelő komponens naplózza az elvégzett tevékenységeket. A CAFF adatkezelő ezen kívül a CAFF fájl megjelenítéséhez és validációjához felhasználja a natív CAFF parser komponenst. Ezt a parser által nyújtott dedikált interfészen keresztül éri el.

![Component-diagram](/img/Component-diagram.png)

<p align = "center">
X. ábra. A CAFF webshop rendszer komponens diagramja</p>

A rendszer komponens diagramján az UMLsec segítségével jelenítünk meg biztonsági követelményeket, ahogy az a X. ábrán látható. A rendszer szempontjából kritikus fontosságú erőforrások az adatbázisok és a napló, ezért ezeket \<\<critical\>\> sztereotípiával látjuk el. A felhasználói adatbázis és a CAFF adatbázis esetében tagged value-kal jelezzük a kritikus fontosságú aspektusokat, ezek látszanak az ábrán a kommentes részekben.

Az adatbázist és naplót használó komponensek dependenciákon keresztül mutatják függésüket az erőforrásoktól. Ezeken szintén sztereotípiákkal jelezzük a felhasználás során biztosítandó biztonsági követelményeket. A felhasználók számára elérhető interfészeken tagged value-k mutatják, hogy az azt megvalósító komponenseknek a rendszer milyen megvalósító komponenseknek a rendszer milyen aspektusait kell megvédeniük, ezek szintén látszanak az ábrán a komment szekciókban.

![Component-diagram-umlsec](/img/Component-diagram-umlsec.png)

<p align = "center">
X. ábra. A CAFF webshop rendszer komponens diagramja</p>

### A rendszer viselkedése

A rendszer viselkedésének tervezéséhez minden use-case-hez külön-külön
diagrammot készítettünk.

#### Regisztráció

![sequence-registration](/img/sequence-registration.png)

#### Bejelentkezés

![login-squence-diagram](/img/login.png)

<p align = "center">
X. ábra. A CAFF webshop rendszer komponens diagramja</p>

#### CAFF letöltése

![seq-diagram-upload](/img/sequence-caffload.png)

<p align = "center">
X. ábra. A CAFF webshop rendszer komponens diagramja</p>

#### CAFF feltöltése

![seq-diagram-upload](/img/seq-diagram-upload.png)

<p align = "center">
X. ábra. A CAFF webshop rendszer komponens diagramja</p>

#### CAFF keresés

![caff-search](/img/Caff_search.png)

<p align = "center">
X. ábra. A CAFF webshop rendszer komponens diagramja</p>

#### Felhasználói adat módosítása

![user-data-modification](/img/User_data_modification.png)

<p align = "center">
X. ábra. A CAFF webshop rendszer komponens diagramja</p>

> Megjegyzés: Az admin adat módosítása is ugyanígy történik, ahhoz nem készült külön szekvencia diagram.

#### Felhasználói adat lekérdezése

![user-data_query](/img/User_data_query.png)

<p align = "center">
X. ábra. A CAFF webshop rendszer komponens diagramja</p>

## Tesztelési terv

### Funkcionális tesztelés

- A fontosabb funkciókhoz unit teszteket készítünk (például a parserhez, CAFF kezelőhöz).
- Integrációs tesztelést végzünk (például CAFF kezelőnél).
- Az elkészült alkalmazáson manuális teszteléseket végzünk, az összes funkciót kipróbáljuk.
- Ellenőrizzük, hogy az elkészült funkciók megvalósítják-e a követelményekben leírtakat (verifikáció).

### Biztonsági tesztelés

#### Coding standard

- Ne legyen benne memory leak a C++ részben
- Ne legyen out of bound memory read
- Az adatbázis eléréséhez entity frameworkot használunk, ezzel számos potenciális biztonsági kockázatot elkerülve (pl. sql injection)
- Szálkezelés implementálására async-await mintát használunk, ezzel leegyszerűsítve a szálkezelést.
- HTML sanitizert használunk XSS és egyéb támadások ellen.
- Alkalmazzuk a fontosabb OOP elveket (pl. encapsulation)

#### Parser dinamikus tesztelése

- Statikus ellenőrzőket használunk (Pl. Visual Studio code analysis, cppcheck, sonarqube, roslyn analyzer) a szerverkódhoz és a C++ kódhoz is.
- A natív C++ CAFF parser dinamikus tesztelését az afl fuzzer program segítségével végezzük el. Ezzel különböző bemenetekkel végigmehetünk a parser útvonalain, elmentve mikor crashel vagy akad el a program. A talált hibákat pedig a valgrind program segítségével detektáljuk és javítjuk majd ki.
