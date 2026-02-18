using FPad;
using Panlingo.LanguageIdentification.Whatlang;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace Helper;

public record ScoreCalcStrings(string GoodChars, string BadChars);

public static class Program
{
    static Mutex mutex;
    static EventWaitHandle eventHandle;
    static object consoleSyncObj = new();

    public static void Main()
    {
        Console.WriteLine("5");

        //string encodingManagerLines = string.Join(Environment.NewLine, GenerateScoreCalculatorLines());

        //TestEvents();

        //MakeIcon();

        //WatchFiles();

        PacketSaveAll();
    }

    private static void PacketSaveAll()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Encoding encWin1250 = Encoding.GetEncoding(1250);
        Encoding encOem852 = Encoding.GetEncoding(852);

        Encoding encWin1257 = Encoding.GetEncoding(1257);
        Encoding encOem775 = Encoding.GetEncoding(775);

        Encoding encWin1252 = Encoding.GetEncoding(1252);

        Encoding encWin1251 = Encoding.GetEncoding(1251);
        Encoding encOem866 = Encoding.GetEncoding(866);
        Encoding encKoi8R = Encoding.GetEncoding("koi8-r");

        PacketSave("lv", [encWin1257, encOem775, Encoding.UTF8], "ChatGPT darbībai ir vairāki ierobežojoši faktori un apzinātas problēmas. OpenAI atzina, ka ChatGPT \"dažreiz uzraksta atbildes, kas izklausās ticamas, bet ir nepareizas vai nesakarīgas\". Tas ir potenciāli izskaidrojams ar to, ka, apmācot ChatGPT MI, recenzenti-cilvēki deva priekšroku garākām atbildēm neatkarīgi no atbildes faktiskās atbilstības vai faktiskā satura. Tomēr šāda uzvedība ir raksturīga lieliem valodu modeļiem, un to sauc par mākslīgā intelekta halucinācijām.");
        PacketSave("lt", [encWin1257, encOem775, Encoding.UTF8], "ChatGPT – pokalbių roboto programa, atsakinėjanti į tekstines užklausas. Tai yra OpenAI sukurtas didelis kalbos modelis, kuris yra mokomas analizuoti ir generuoti tekstą įvairiuose kontekstuose. Modelis naudoja gilaus mokymosi (deep learning) technologijas, kad suprastų ir atsakytų į vartotojų klausimus ar komandas. Tai gali būti naudojama įvairiose srityse, pavyzdžiui, virtualių asistentų, klientų aptarnavimo ar automatizuotų atsakiklių sistemose.");
        PacketSave("ee", [encWin1257, encOem775, Encoding.UTF8], "ChatGPT suudab luua loogilist ja veenvalt kirjutatud teksti, mis sarnaneb inimeste kirjutatud tekstiga, olles sageli ka stiililt väga inimlik. Juturobot on võimeline jätkama teksti nagu päris inimene ja tunnistab oma vigu, kui neid tema vastustes leidub. Lisaks lihtsamale jutuajamisele suudab ChatGPT kirjutada koodijuppe, koostada laulu ja luulesalme, mängida mõningaid mänge ja muudki. Mis teeb ChatGPT eriliseks, on see, et ta suudab koostada vastuseid, mis ei pruugi olla otseselt juhtunud, vaid tuginevad hüpoteetilistele olukordadele või loogilistele järeldustele, mis on kooskõlas tänapäeva teaduslike või sotsiaalsete teguritega. GPT-4 suudab veelgi paremini mõista konteksti ja kohandada vastuseid keerukamate ülesannete põhjal.");

        PacketSave("cz", [encWin1250, encOem852, Encoding.UTF8], "ChatGPT má zatím rovněž potíže se zpracováním matematických úloh. Důvodem je, že je trénován na rozpoznávání a generování textu na základě předchozích vzorků. V současné době není schopen plně řešit matematická zadání, jako by to udělal člověk, protože matematika vyžaduje logické myšlení a abstrakci. Umělá inteligence například v podobě ChatGPT má tendenci spoléhat se na naučené modely a předem dané algoritmy a není zatím schopna pochopit matematiku na hlubší úrovni jako člověk. Ke zlepšení matematických schopností mělo dojít po 30. lednu 2023, kdy byl proveden upgrade ChatGPT. V placené verzi ChatGPT lze k řešení matematických úloh využít plugin od Wolfram Alpha.");
        PacketSave("pl", [encWin1250, encOem852, Encoding.UTF8], "Jeden z głównych konkurentów ChatGPT, Gemini, został stworzony przez Google (Alphabet), która wstępnie udostępniła go w marcu 2023, umożliwiając użytkownikom w USA i Wielkiej Brytanii dołączanie do listy oczekujących. W maju, jeszcze pod nazwą Bard, był dostępny już w 180 państwach, z wyłączeniem większości państw europejskich, takich jak Polska, Niemcy i Francja. Bard został udostępniony w Unii Europejskiej 13 lipca 2023[36]. W kontekście strategii biznesowej, Bard jest częścią większej rywalizacji między Google a Microsoftem, który zintegrował ChatGPT z własną wyszukiwarką Bing.");
        PacketSave("sk", [encWin1250, encOem852, Encoding.UTF8], "ChatGPT má svoju webovú stránku, ktorá bola formou textovej konverzácie (chatu) sprístupnená verejnosti 30. novembra 2022.[4] ChatGPT je navrhnutý tak, aby bol nezávislý od jazyka. V súčasnosti chatbot podporuje viacero jazykov vrátane angličtiny, španielčiny, francúzštiny, nemčiny, portugalčiny, taliančiny, holandčiny, ruštiny, arabčiny a čínštiny. Chatbot využíva technológiu spracovania prirodzeného jazyka na interpretáciu a odpovedanie na otázky používateľov v rôznych jazykoch. Medzi prvými osemdesiatimi jazykmi bola aj slovenčina.");
        PacketSave("sl", [encWin1250, encOem852, Encoding.UTF8], "ChatGPT je v svojem osnovnem namenu klepetalni robot (angleško chatbot), katerega glavna funkcija je posnemanje človeškega pogovarjanja. Kljub temu so nekateri novinarji opazili njegovo vsestranskost in sposobnost improvizacije. Sistem lahko opravlja naloge, kot so pisanje besedil, odpravljanje napak v računalniških programih in skladanje glasbe. Njegova sposobnost pisanja besedil vključuje scenarije, poezijo, pesmi, pravljice in eseje. Na vprašanja, ki se pojavljajo v šolskih izpitih, ChatGPT odgovori na način, ki je primerljiv s človeškim vedenjem med opravljanjem tovrstnih nalog, pri nekaterih izpitih je celo učinkovitejši v primerjavi s povprečnim človeškim izpitnim kandidatom. Lahko posnema delovanje operacijskega sistema Linux ali bankomata ter potek nekaterih iger ali pogovarjanja v spletni klepetalnici.");
        PacketSave("hu", [encWin1250, encOem852, Encoding.UTF8], "A rendszer működése nagyméretű neurális hálózaton alapul, amelyet hatalmas mennyiségű szöveges adat feldolgozásával tanítottak be. A ChatGPT-t egyaránt alkalmazzák oktatásban, üzleti tervezésben, szoftverfejlesztésben, tartalomkészítésben, ügyfélszolgálati környezetben és kutatási célokra. Elterjedése jelentős társadalmi, technológiai és gazdasági hatásokat váltott ki, és széles körű vitákat indított a mesterséges intelligencia jogi, etikai és szabályozási kérdéseiről.");
        PacketSave("hr", [encWin1250, encOem852, Encoding.UTF8], "Program je koji se koristi tzv. „dubokim učenjem” kako bi odgovorio na tekstovne upite koje mu korisnici upućuju. Korisnici mogu postavljati pitanja ili tražiti savjete o bilo kojoj temi, a ChatGPT bi im trebao dati odgovor koji je jezičnom strukturom statistički najbliži onomu što bi prosječni čovjek odgovorilo. Model se temelji na dubokom učenju i velikim količinama podataka koje su prikupljene s interneta. Model se sastoji od nekoliko slojeva neuronske mreže, koja je trenirana na ogromnoj količini teksta, među ostalim i Wikipedijinim člancima.");
        PacketSave("ro", [encWin1250, encOem852, Encoding.UTF8], "În comparație cu predecesorul său, InstructGPT, ChatGPT încearcă să reducă răspunsurile dăunătoare și înșelătoare. [24] Într-un exemplu, în timp ce InstructGPT acceptă premisa mesajului „Spune-mi despre când a venit Cristofor Columb în SUA în 2015” ca fiind adevărată, ChatGPT recunoaște natura contrafăcută a întrebării și își încadrează răspunsul ca o considerație ipotetică a ceea ce s-ar putea întâmpla dacă Columb ar veni în SUA în 2015, folosind informații despre călătoriile lui Cristofor Columb și fapte despre lumea modernă – inclusiv percepțiile moderne despre acțiunile lui Columb.");

        PacketSave("fr", [encWin1252, Encoding.UTF8], "Face à ces nouvelles fonctionnalités pensées pour mieux répondre aux attentes des utilisateurs, OpenAI, maison-mère de ChatGPT, a pris en compte les préoccupations liées à la protection des données personnelles. L’entreprise souligne que l’historique permettant de relier plusieurs recherches afin d’en conserver le contexte peut être désactivé à tout moment, et qu’il est également possible d’utiliser une fenêtre de navigation privée qui déconnecte ChatGPT[45]. Bien que ces précisions visent à rassurer le public, elles doivent être replacées dans un climat de vigilance: en janvier 2024, Antoine Boutet, enseignant-chercheur à l’INSA Lyon, et Alexis Léautier, ingénieur à la CNIL, ont rappelé dans The Conversation que le modèle avait déjà divulgué certaines informations personnelles d’utilisateurs. Des données confidentielles saisies par un usager pourraient, selon eux, réapparaître involontairement dans les réponses adressées à d’autres");
        PacketSave("sv", [encWin1252, Encoding.UTF8], "ChatGPT kan besvara faktafrågor, författa användbara texter i olika genrer, och samtala på ett människoliknande sätt på många olika språk. Sanningsenligheten i svaren varierar dock. ChatGPT är ett exempel på generativ AI. Den kan användas för att generera sammanfattningar, översättningar, rapporter, uppsatser, annonser, manus, presentationer, berättelser, poesi och sångtexter. Den kan komponera musik, skriva, felsöka och översätta datorprogram mellan olika programspråk. Till lärare kan den föreslå kursplaner, skoluppgifter, uppsatsämnen och betyg för elevernas svar. Skolelever kan använda den som en hjälplärare som förklarar svåra begrepp, eller som en spökskrivare för att bolla hur skoluppgifter och prov kan besvaras (ibland, beroende på provet, på en nivå över den genomsnittliga mänskliga testtagaren) i den mån skolan tillåter och uppmuntrar det arbetssättet.");
        PacketSave("fi", [encWin1252, Encoding.UTF8], "Kuten aiemmat GPT-versiot, GPT-4 on autoregressiivinen malli, mikä rajoittaa sen kykyä suunnitella asioita etukäteen. Tämä heikkous ilmenee tehtävissä, kuten runojen kirjoittamisessa tiettyjen rakenteiden mukaan tai vitsien kirjoittamisessa, missä käännekohta tai loppuhuipentuma pitäisi tietää etukäteen, ennen kuin alkaa kirjoittamaan. Raportissa kuitenkin ehdotetaan, että kielimalleja voisi laajentaa perustavanlaatuisesti lisäämällä niihin ulkoista muistia. Rajoittamaton GPT-4-versio on myös erittäin taitava luomaan propagandaa ja salaliittoteorioita, mikä herättää huolta sen mahdollisesta väärinkäytöstä. Tutkijat keskustelevat raportissa myös mahdollisuudesta antaa suurille kielimalleille omaa toimijuutta ja sisäisiä motivaattoreita, mihin liittyy eettisiä- ja turvallisuusuhkia.");
        PacketSave("de", [encWin1252, Encoding.UTF8], "Die nächste Entwicklungsstufe stellte Mitte Mai 2024 das Model GPT-4o dar. Das „o“ steht für „omni“ (lateinisch alles), da der Nutzer deutlich besser verschiedene Inhaltsarten in der Ein- und Ausgabe verwenden kann. Durch eine optimierte Verarbeitung sprachlicher Eingaben kann sich der Nutzer erstmals mit ChatGPT fast in Echtzeit unterhalten (englisch Advanced Voice Mode). Auch die Erkennung von Bildern und Videos wurde laut OpenAI deutlich verbessert. Anders als GPT-4 ist dies kostenlos. Der Advanced Voice Mode wurde ab Ende Juli 2024 für Plus-Nutzer freigeschaltet und sollte ab Herbst allen zahlenden Kunden zur Verfügung stehen.");
        PacketSave("es", [encWin1252, Encoding.UTF8], "En marzo de 2023, un fallo permitió a algunos usuarios ver los títulos de las conversaciones de otros usuarios.​ El consejero delegado de OpenAI, Sam Altman, declaró que los usuarios no podían ver el contenido de las conversaciones. Poco después de que se corrigiera el fallo, los usuarios no pudieron ver su historial de conversaciones.​ Informes posteriores mostraron que el fallo era mucho más grave de lo que se creía en un principio, y OpenAI informó de que se había filtrado el «nombre y apellidos, dirección de correo electrónico, dirección de pago, los cuatro últimos dígitos (únicamente) de un número de tarjeta de crédito y la fecha de caducidad de la tarjeta de crédito» de los usuarios.");
        PacketSave("pt", [encWin1252, Encoding.UTF8], "O ChatGPT está disponível para uso online em três versões, uma construída em GPT-4, outra em GPT-4o e outra em GPT-o mini, todas membros da série proprietária de modelos transformadores generativos pré-treinados (GPT) da OpenAI, com base na arquitetura de transformador desenvolvida do Google[7] - e é ajustada para aplicações de conversação usando uma combinação de aprendizagem supervisionada e aprendizagem por reforço.[5] O ChatGPT foi lançado como uma pré-visualização de investigação disponível gratuitamente, mas devido à sua popularidade, a OpenAI agora opera o serviço num modelo freemium. Ele permite que os utilizadores no seu nível gratuito acessem a versão baseada em GPT-4o mini, enquanto que as versões mais avançadas baseadas em GPT-4 e GPT-4o e o acesso prioritário aos recursos mais recentes são fornecidos aos assinantes pagos sob o nome comercial \"ChatGPT Plus\".");
        PacketSave("it", [encWin1252, Encoding.UTF8], "Durante il 2020, gli indicatori climatici in Africa sono stati caratterizzati da un continuo aumento delle temperature, accelerazione dell'innalzamento del livello del mare, eventi meteorologici e climatici estremi (come inondazioni e siccità) e relativi impatti ambientali; il rapido restringimento dei ghiacciai nell'Africa orientale, che rischiano lo scioglimento, è un segnale importante dei cambiamenti climatici in atto.");
        PacketSave("af", [encWin1252, Encoding.UTF8], "Die reëngordel beweeg nie volgens riglyne nie, omdat dit nie deur topografiese kenmerke gelei ward nie. Daarom is die begin van die reënseisoen moeilik voorspelbaar een jaar is dit vroeg, die volgende jaar laat. Soms val daar baie reën, dan weer minder. Die sones met wisselende droë en nat seisoene beweeg geleidelik oor na die woestyngebiede waar 'n stortbui baie selde voorkom.");
        PacketSave("ca", [encWin1252, Encoding.UTF8], "El clima d'Àfrica va d'un clima tropical a un de subàrtic als seus pics més alts. La meitat septentrional és principalment deserta o àrida, mentre que les parts central i meridional contenen tant planes de sabana com regions de jungla molt espessa. Entremig, hi ha una convergència dominada per patrons de vegetació, com ara Sahel i estepa.\r\n\r\nÀfrica té possiblement la combinació més gran del món de densitat i \"llibertat\" de poblacions i diversitat de poblacions d'animals salvatges, amb poblacions salvatges de grans carnívors (com ara lleons, hienes i guepards) i herbívors (com ara búfals, cérvols, elefants, camells i girafes), que viuen principalment en planes privades i no privades. També hi viuen una gran varietat de criatures de la jungla (incloent-hi serps i primats) i vida aquàtica (incloent-hi cocodrils i amfibis).");
        PacketSave("no", [encWin1252, Encoding.UTF8], "Afrika er en verdensdel som strekker seg i alt 8 000 km fra nord til sør med ekvator omtrent på midten og med hav på alle kanter bortsett fra ved Suezkanalen i Egypt. Klimaet er tropisk rundt ekvator og subtropisk lengst i nord og sør. Middeltemperaturen er derfor høy over hele kontinentet, men nedbørsmengden og -mønsteret varierer stort fra regnskog til ørkenområder, fra kyst til innland, fra nord til sør. Afrika har flere stater enn noe annet kontinent, med sine 48 kontinentale stater og seks øystater. Afrika omfatter cirka 20 % av den totale landmassen og cirka 18 % av den totale folkemengden på Jorden.");
        PacketSave("da", [encWin1252, Encoding.UTF8], "Afrikanske danse er en vigtig tilstand af kommunikation, og danserne bruger gæster, masker, kostumer, kropsmaling og et antal visuelle genstande. De grundlæggende bevægelser er nogen gange enkle og lægger vægt på kun overkroppen eller maveregionen eller fødderne. Sådanne bevægelser er nogen gange komplekse og involverer koordination af forskellige kropsdele. Dansere udfører dansen nogen gange alene eller i små grupper på to eller tre personer. Danse med mange udøvere udføres også med forskellige formationer, som lineære, cirkulære, bugtende og så videre.");
        PacketSave("nl", [encWin1252, Encoding.UTF8], "Midden-Australië heeft een echt woestijnklimaat. In de zomer kunnen de temperaturen zeer hoog oplopen. In de winterperiode kan het 's nachts flink afkoelen. Hier valt minder dan 250 mm neerslag per jaar en het droge seizoen duurt meer dan acht maanden.\r\n\r\nHet noorden van New South Wales en het zuiden van Queensland hebben een subtropisch klimaat met het hele jaar door aangenaam weer, hoewel het in de zomer behoorlijk warm kan worden. Grote steden als Sydney, Perth en Adelaide hebben een mediterraan klimaat met warme zomers en milde winters.\r\n\r\nVictoria en Tasmanië in het zuiden hebben een gematigd klimaat. In de winter kan het vrij koud worden met sneeuwval in de hogere gebieden. De rest van het jaar is het zonnig en warm met kans op hittegolven in Victoria.");

        PacketSave("ru", [encWin1251, encOem866, encKoi8R, Encoding.UTF8], "ChatGPT был доработан поверх GPT-3.5 с использованием методов обучения как с учителем, так и с подкреплением. В обоих подходах использовались люди-тренеры для улучшения производительности модели. В случае обучения с учителем модель была снабжена беседами, в которых тренеры играли обе стороны: пользователя и помощника по искусственному интеллекту. На этапе подкрепления инструкторы-люди сначала оценивали ответы, которые модель создала в предыдущем разговоре. Эти оценки были использованы для создания моделей вознаграждения, на которых модель была дополнительно доработана с использованием нескольких итераций Proximal Policy Optimization[19][20]. Алгоритмы Proximal Policy Optimization имеют преимущество по затратам по сравнению с алгоритмами Region Policy Optimization; они сводят на нет многие дорогостоящие в вычислительном отношении операции с более высокой производительностью[21][22]. Модели были обучены в сотрудничестве с Microsoft на их суперкомпьютерной инфраструктуре Azure.");
        PacketSave("by", [encWin1251, encOem866, Encoding.UTF8], "Нягледзячы на тое, што асноўная функцыя чат-бота — імітацыя чалавека-суразмоўцы, ChatGPT з’яўляецца універсальным. Напрыклад, ён мае магчымасць пісаць і адладжваць камп’ютарныя праграмы; складаць музыку, тэлеспектаклі, казкі, сачыненні; адказваць на пытанні тэстаў (часам — у залежнасці ад тэсту — на ўзроўні, вышэйшым за ўзровень сярэдняга чалавека, які праходзіць тэст);[13] пісаць вершы і тэксты песень;[14] эмуліраваць сістэму Linux; мадэляваць увесь чат; гуляць у такія гульні, як крыжыкі-нулікі; і мадэляваць банкамат.[15] Навучальныя даныя ChatGPT уключаюць старонкі кіраўніцтва і інфармацыю аб з’явах Інтэрнэту і мовах праграмавання, такіх як сістэмы дошак аб’яў ці мова праграмавання Python.");
        PacketSave("ua", [encWin1251, encOem866, Encoding.UTF8], "Порівняно зі своїм попередником InstructGPT, ChatGPT намагається зменшити кількість шкідливих і оманливих відповідей. Так, наприклад, InstructGPT сприймає запит «Розкажи мені, як Христофор Колумб прибув до США у 2015 році» як такий, що містить правдиву інформацію, а ChatGPT аналізує історичну інформацію про подорожі Колумба, уявлення про його особистість та інформацію про сучасний світ і на основі цього будує відповідь, у якій описані припущення, — що б сталось, якби Христофор Колумб прибув до США у 2015 році[21]. До даних, що використовуються для навчання ChatGPT, входять довідникові сторінки, інформація про інтернет-меми і мови програмування[25].");
        PacketSave("bg", [encWin1251, encOem866, Encoding.UTF8], "ChatGPT е пуснат на 30 ноември 2022 г. от OpenAI – компания, базирана в Сан Франциско, САЩ. Първоначално услугата е безплатна за обществеността, с планове за монетизиране по-късно.[17] До 4 декември 2022 г. ChatGPT вече има над един милион потребители.[18] През януари 2023 г. ChatGPT достига над 100 милиона потребители, което го прави най-бързо развиващото се потребителско приложение до момента.[19] Услугата работи най-добре на английски, но може да функционира и на някои други езици с различна успеваемост.");
        PacketSave("sr", [encWin1251, Encoding.UTF8], "На северу Африке живи бројно становништво које не припада црначкој популацији. Народи северне Африке у највећем броју говоре афро-азијским језицима. У ове народе спадају и древни Египћани, Бербери, Нубијци који су проширили цивилизацију из северне Африке по античком свету. У 600. години, Арапи муслимани су са истока прешли у Африку и освојили читав регион. Бербери су остали у мањини у Мароку и Алжиру, док један број Бербера живи и у Тунису и Либији. Туарези и други номадски народи су већинско становништво у сахарским државама.");
        PacketSave("mk", [encWin1251, Encoding.UTF8], "Климата во Африка е тесно поврзана со географската положба. Низ Африка поминува екваторот, кој го дели континентот на северен и јужен дел. Тоа условува рамномерен распоред на климатските појаси. Околу екваторот се простира екваторскиот појас, а потоа северно и јужно од него се простираат тропски, суптропски и умерени климатски појаси. Екваторскиот појас се одликува со: температури од околу 24-25°С и годишно колебање од само 2-3°С, и големи количества врнежи, кои во Гвинејскиот Залив изнесуваат околу 12 000мм годишно, а паѓаат секојдневно, обично во попладневните часови. Изразита екваторска клима има во Горна Гвинеја и во басенот на реката Конго, односно во појасот од 0-100 северна и јужна географска широчина. Таму се развиени многу бујни тропски шуми.");

        // PacketSave("", [Encoding.UTF8], "");
    }

    private static void PacketSave(string langId, IEnumerable<Encoding> encodings, string text)
    {
        foreach (Encoding encoding in encodings)
        {
            byte[] bytes = encoding.GetBytes(text);
            string fileName = $"text_{langId}_{encoding.WebName}.txt";
            File.WriteAllBytes(fileName, bytes);
        }
    }

    private static void WatchFiles()
    {
        string path = @"D:\Bn\Src\FPad\Local\watcher.txt";
        FileWatcher watcher = new FileWatcher(path);
        watcher.FileModified += Watcher_FileModified;

        Console.ReadLine();

        Console.WriteLine($"Write start");
        watcher.SaveWrapper(() =>
        {
            File.WriteAllText(path, "Overwritten!1", Encoding.Unicode);
        });
        Console.WriteLine($"Write end");

        Console.ReadLine();

        watcher.Dispose();
        Console.WriteLine($"Disposed");

        Console.ReadLine();
    }

    private static void Watcher_FileModified(object sender, EventArgs e)
    {
        Console.WriteLine($"Modified");
    }

    private static void MakeIcon()
    {
        string folder = @"D:\Bn\Src\FPad\Local";
        //string[] srcFiles = ["fpad16_bilinear.png", "fpad32_.png", "fpad48_.png", "fpad64_.png"];
        string[] srcFiles = ["new16.png", "new32.png", "new48.png", "new64.png"];
        int[] sizes = srcFiles.Select(x => int.Parse(new string(x.ToCharArray().Where(c => char.IsAsciiDigit(c)).ToArray()))).ToArray();

        List<byte[]> bytes = new();
        foreach(string fileName in srcFiles)
        {
            string path = Path.Combine(folder, fileName);
            bytes.Add(File.ReadAllBytes(path));
        }

        int[] offsets = new int[srcFiles.Length];
        offsets[0] = 6 + srcFiles.Length * 16;
        for (int i=1; i< srcFiles.Length; i++)
        {
            offsets[i] = offsets[i - 1] + bytes[i - 1].Length;
        }

        string outPath = Path.Combine(folder, "new.ico");
        using (FileStream fs = new FileStream(outPath, FileMode.Create, FileAccess.Write, FileShare.None, 262144))
        {
            using (BinaryWriter bw = new BinaryWriter(fs))
            {
                bw.Write((short)0);
                bw.Write((short)1);
                bw.Write((short)srcFiles.Length);

                for (int i = 0; i < srcFiles.Length; i++)
                {
                    bw.Write((byte)sizes[i]);
                    bw.Write((byte)sizes[i]);
                    bw.Write((byte)0);
                    bw.Write((byte)0);
                    bw.Write((short)0); // wPlanes - no idea
                    bw.Write((short)0); // bits per pixel - no idea
                    bw.Write(bytes[i].Length);
                    bw.Write(offsets[i]);
                }

                for (int i = 0; i < srcFiles.Length; i++)
                {
                    bw.Write(bytes[i]);
                }
            }
        }
    }

    private static void TestEvents()
    {
        mutex = new Mutex(false);
        eventHandle = new EventWaitHandle(false, EventResetMode.ManualReset, "test_named_event_1");

        for (int i = 0; i < 8; i++)
        {
            string threadName = $"T{i + 1}"; // capture to local variable so each thread uses unique string
            Thread thr = new Thread(() => ThreadProc(threadName));
            thr.Start();
        }

        while (true)
        {
            ConsoleWriteLine("type S to send the event, exit for exit");
            string input = Console.ReadLine().Trim().ToUpperInvariant();
            if (input == "S")
            {
                mutex.WaitOne();

                eventHandle.Set();
                eventHandle.Reset();
                Thread.Sleep(200);
                eventHandle.Set();
                eventHandle.Reset();

                // Result: 1. waiting threads always register an event
                // 2. some threads register 2 events, most of threads register 1 event

                Thread.Sleep(1000);

                mutex.ReleaseMutex();

                ConsoleWriteLine("sent");
            }
            else if (input == "EXIT")
            {
                isCanceled = true;
                eventHandle.Set();
                eventHandle.Reset();
                return;
            }
        }
    }

    static bool isCanceled = false;
    private static void ThreadProc(string threadName)
    {
        ConsoleWriteLine($"{threadName} started");

        while (true)
        {
            eventHandle.WaitOne();
            if (isCanceled)
                return;

            mutex.WaitOne();
            Thread.Sleep(50);

            ConsoleWriteLine($"{threadName} received the event");

            Thread.Sleep(50);
            mutex.ReleaseMutex();
        }
    }

    private static void ConsoleWriteLine(string str)
    {
        lock (consoleSyncObj)
        {
            Console.WriteLine(str);
        }
    }

    private static List<string> GenerateScoreCalculatorLines()
    {
        Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        Encoding encWin1251 = Encoding.GetEncoding(1251);
        Encoding encOem866 = Encoding.GetEncoding(866);
        Encoding encKoi8R = Encoding.GetEncoding("koi8-r");

        Encoding encWin1257 = Encoding.GetEncoding(1257);
        Encoding encOem775 = Encoding.GetEncoding(775);
        Encoding encWin1250 = Encoding.GetEncoding(1250);
        Encoding encOem852 = Encoding.GetEncoding(852);

        Encoding encWin1252 = Encoding.GetEncoding(1252);

        List<string> result = new();
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Est, WhatlangScript.Latn, "ÕÄÖÜ", [encWin1257, encOem775]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Lav, WhatlangScript.Latn, "āčēģīķļņšūž", [encWin1257, encOem775]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Lit, WhatlangScript.Latn, "ĄČĘĖĮŠŲŪŽ", [encWin1257, encOem775]));

        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Ces, WhatlangScript.Latn, "áčďéěíňóřšťúůýž", [encWin1250, encOem852]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Pol, WhatlangScript.Latn, "ąćęłńóśźż", [encWin1250, encOem852]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Slk, WhatlangScript.Latn, "áäčďéíĺľňóôŕšťúýž", [encWin1250, encOem852]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Slv, WhatlangScript.Latn, "čšž", [encWin1250, encOem852]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Hun, WhatlangScript.Latn, "áéíóöőúüű", [encWin1250, encOem852]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Srp, WhatlangScript.Latn, "čćđšž", [encWin1250, encOem852]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Srp, WhatlangScript.Cyrl, "абвгдђежзијклљмнњопрстћуфхцчџш", [encWin1251]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Hrv, WhatlangScript.Latn, "čćđšž", [encWin1250, encOem852]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Ron, WhatlangScript.Latn, "ăâîșț", [encWin1250, encOem852]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Tuk, WhatlangScript.Latn, "çäňöşüý", [encWin1250, encOem852]));

        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Rus, WhatlangScript.Cyrl, "абвгдеёжзийклмнопрстуфхцчшщъыьэюя", [encWin1251, encOem866, encKoi8R]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Bel, WhatlangScript.Cyrl, "абвгдеёжзійклмнопрстуўфхцчшыьэюя", [encWin1251, encOem866]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Ukr, WhatlangScript.Cyrl, "абвгґдеєжзиіїйклмнопрстуфхцчшщьюя", [encWin1251, encOem866]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Bul, WhatlangScript.Cyrl, "абвгдежзийклмнопрстуфхцчшщъьюя", [encWin1251, encOem866]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Mkd, WhatlangScript.Cyrl, "абвгдѓежзѕијклљмнњопрстќуфхцчџш", [encWin1251]));

        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Afr, WhatlangScript.Latn, "áäéèêëíîïŉóôöúûüý", [encWin1252]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Cat, WhatlangScript.Latn, "àéèíïóòúüç", [encWin1252]));
        // Indonesian: none
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Ita, WhatlangScript.Latn, "éàèìòùî", [encWin1252]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Nob, WhatlangScript.Latn, "æøåòôéó", [encWin1252]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Por, WhatlangScript.Latn, "áâãàçéêíóôõú", [encWin1252]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Spa, WhatlangScript.Latn, "ñáéíóú¿¡", [encWin1252]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Swe, WhatlangScript.Latn, "åäöé", [encWin1252]));
        // Tagalog: diacritics not really used
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Dan, WhatlangScript.Latn, "æøåé", [encWin1252]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Nld, WhatlangScript.Latn, "ĳáéíóúëïöüè", [encWin1252]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Fra, WhatlangScript.Latn, "çœæàèùéâêîôûëïüÿîû", [encWin1252]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Deu, WhatlangScript.Latn, "äöüß", [encWin1252]));
        result.Add(GenerateScoreCalculatorLine(WhatlangLanguage.Fin, WhatlangScript.Latn, "åäö", [encWin1252]));

        return result;
    }

    /// <summary>
    /// Generate code line for EncodingManager.
    /// </summary>
    /// <param name="lang"></param>
    /// <param name="symbolsBeyondAscii">symbols outside of ASCII range, which the language has, in any case. no spaces.</param>
    /// <param name="encodings">ANSI encodings suitable for this language</param>
    /// <returns></returns>
    private static string GenerateScoreCalculatorLine(WhatlangLanguage lang, WhatlangScript alphabet, string symbolsBeyondAscii,
        ICollection<Encoding> encodings)
    {
        ScoreCalcStrings scoreCalcStrings = GetScoreCalcStrings(lang, symbolsBeyondAscii, encodings);
        string result = $"scoreCalculators.Add(new LangKey(WhatlangLanguage.{lang}, WhatlangScript.{alphabet}), new LangScoreCalculator(\"{scoreCalcStrings.GoodChars}\", \"{scoreCalcStrings.BadChars}\"));";
        return result;
    }

    private static ScoreCalcStrings GetScoreCalcStrings(WhatlangLanguage lang, string symbolsBeyondAscii,
        ICollection<Encoding> encodings)
    {
        string goodSymbols = GetBothCasesSymbols(symbolsBeyondAscii);
        HashSet<char> badSymbolsSet = new();
        StringBuilder badSymbolsSb = new();

        // We handle cases "UTF-8 misinterpreted as ANSI", but not "ANSI misinterpreted as UTF-8",
        // because such misenterpretation would give limitless number of incorrect symbols.
        List<Encoding> sourceEncodings = encodings.ToList();
        sourceEncodings.Add(Encoding.UTF8);
        foreach (Encoding enc in sourceEncodings)
        {
            byte[] encodedBytes = enc.GetBytes(goodSymbols);
            // Try to decode using wrong page, and add whatever we see to "bad symbols"
            foreach (Encoding wrongEnc in encodings.Where(x => x != enc))
            {
                string badResult = wrongEnc.GetString(encodedBytes);
                for (int i = 0; i < badResult.Length; i++)
                {
                    char c = badResult[i];
                    if ((c > 127) // don't punish for symbols from ascii range (control symbols: handled independently)
                        && !goodSymbols.Contains(c)
                        && !badSymbolsSet.Contains(c))
                    {
                        badSymbolsSet.Add(c);
                        badSymbolsSb.Append(c);
                    }
                }
            }
        }

        return new ScoreCalcStrings(goodSymbols, badSymbolsSb.ToString());
    }

    private static string GetBothCasesSymbols(string symbolsBeyondAsciiAnyCase)
    {
        string symbolsBeyondAsciiLower = symbolsBeyondAsciiAnyCase.Normalize().ToLowerInvariant();
        HashSet<char> uniqueSymbols = new();
        for (int i = 0; i < symbolsBeyondAsciiLower.Length; i++)
        {
            char c = symbolsBeyondAsciiLower[i];
            if (!uniqueSymbols.Contains(c))
                uniqueSymbols.Add(c);
        }

        StringBuilder sb = new();
        foreach (char c in uniqueSymbols.OrderBy(c => c))
        {
            sb.Append(c);
        }

        sb.Append(sb.ToString().ToUpperInvariant());
        return sb.ToString();
    }
}
