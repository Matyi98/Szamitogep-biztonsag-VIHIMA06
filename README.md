# Szamitogep-biztonsag-VIHIMA06

## Követelmények

### Funkcionális követelmények

Az alábbi lista tartalmazza a funkcionális követelményeket:

- Felhasználóknak kell tudni regisztrálni és belépni
- Felhasználóknak kell tudni CAFF fájlt feltölteni, letölteni és keresni
- Más felhasználók CAFF fájl letöltéséhez meg kell vásárolni a fájlt, de az áruházban minden ingyen letölthető.
- Felhasználóknak kell tudni CAFF fájlhoz megjegyzést hozzáfűzni
- A rendszerben kell legyen adminisztrátor felhasználó, aki tud adatokat módosítani és törölni.

![use-case](/img/use-case.drawio.png)

### Biztonsági követelmények és célok

A funkcionális követelmények ismeretében nagy vonalakban fel tudjuk vázolni a rendszert és annak környezetét, ahogy azt a következő Data Flow ábra mutatja.

![integrity](/img/integrity.drawio.png)

A CAFF webáruházat az alapvető felhasználók és adminisztrátorok használhatják., velük kerülhet interakcióba a rendszer. Mivel a tőlük érkező kéréseket, a viselkedésüket nem tudjuk kontrollálni, ezért a velük történő interakció bizalmi kéréseket vet fel. Az előző ábrán ezt szaggatott piros vonallal jelezzük. A rendszer működtetéséhez szükség lesz a felhasználók és adminisztrátorok adatainak tárolására, a CAFF fájlok tárolására, illetve a fájlokhoz tartozó kommentek elmentésére. A tárolt adatok között kapcsolatot is kell létesíteni, a felhasználót össze kell kötni a saját feltöltött CAFF fájljaival, a hozzáadott kommentjeivel és azzal, hogy milyen CAFF fájlokat vásárolt meg.
A CAFF webáruház, mint szoftver, biztonsági követelményeit hat nagy kategóriába soroljuk: CIA és AAA. Az egyes kategóriákhoz az alábbi biztonsági követelményeket határozhatjuk meg.

- Bizalmasság (confidentiality)

  - ...

- Integritás (integrity)

  - ...

- Elérhetőség (availability)

  - Karbantartási időn kívül, a rendszer legyen mindig elérhető.

- Autentikáció (authentication)

  - ...

- Autorizáció (authorization)

  - ...

- Auditálás (auditing)

  - ...

### Threat Assessment

A threat assessment részt két részre tagoljuk: az assetek azonosítása és assetekre leselkedő veszélyek (threat) azonosítása. Az assetek azonosításához iteratívan elemezzük a rendszer use casei-eit, figyelembe véve a biztonsági követelményeket. A veszélyek megállapításához a STRIDE keretrendszer segítségét vettük igénybe.

#### Assetek megállapítása

A funkcionális és biztonsági követelmények ismeretében pontosítjuk a kezdeti adatfolyam diagramot és részletezhzük a rendszer komponenseit. A Thre-
at, Risk, Vulnerability Analysis során háromfajta assethalmazt különítünk el: (TODO: ezt még ki kell egészíteni)

- Fizikai assetek: gépek, amin a szerverek futnak
- Emberi assetek: felhasználók, adminisztrátorok
- Logikai assetek: adatok
