using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Threading;

namespace CzynszeBazy
{
    class Program
    {
        const string katalogKartSt = "kart_st";
        const string tymczasowyKatalogBaz = "bazyTmp";
        const string filtrWyszukiwaniaDbf = "*.dbf";
        const string filtrWyszukiwaniaNtx = "*.ntx";
        const string plikKonfig = "konfig.jk";

        static void Main(string[] args)
        {
            string wynik;

            try
            {
                System.Configuration.ConfigXmlDocument konfiguracja = new System.Configuration.ConfigXmlDocument();

                konfiguracja.Load("konfig.xml");

                string ścieżkaBaz = konfiguracja.SelectSingleNode("/konfiguracja/ścieżki/bazy").InnerText;
                string ścieżkaCzynszy = konfiguracja.SelectSingleNode("/konfiguracja/ścieżki/czynsze").InnerText;
                string ścieżkaSqlBrowse = konfiguracja.SelectSingleNode("/konfiguracja/ścieżki/sqlBrowse").InnerText;
                string ścieżkaKartStWCzynszach = Path.Combine(ścieżkaCzynszy, katalogKartSt);

                if (Directory.Exists(tymczasowyKatalogBaz))
                    Directory.Delete(tymczasowyKatalogBaz, true);

                Directory.CreateDirectory(tymczasowyKatalogBaz);
                Directory.CreateDirectory(Path.Combine(tymczasowyKatalogBaz, katalogKartSt));
                KopiujPlikiDoTymczasowegoKatalogu(Directory.GetFiles(ścieżkaCzynszy, filtrWyszukiwaniaDbf));
                KopiujPlikiDoTymczasowegoKatalogu(Directory.GetFiles(ścieżkaKartStWCzynszach, filtrWyszukiwaniaDbf));
                KopiujPlikiDoTymczasowegoKatalogu(Directory.GetFiles(ścieżkaCzynszy, filtrWyszukiwaniaNtx));
                KopiujPlikiDoTymczasowegoKatalogu(Directory.GetFiles(ścieżkaKartStWCzynszach, filtrWyszukiwaniaNtx));

                if (Directory.Exists(ścieżkaBaz))
                    Directory.Delete(ścieżkaBaz, true);

                File.Copy(plikKonfig, Path.Combine(tymczasowyKatalogBaz, plikKonfig), true);
                Directory.Move(tymczasowyKatalogBaz, ścieżkaBaz);

                wynik = "Sukces.";
            }
            catch (Exception e) { wynik = e.ToString(); }

            if (!Directory.Exists("log"))
                Directory.CreateDirectory("log");
            
            File.WriteAllText(Path.Combine("log", String.Concat(DateTime.Today.ToShortDateString(), ".txt")), wynik);

            //new System.Threading.Thread(() => System.Diagnostics.Process.Start(ścieżkaSqlBrowse)).Start();
        }

        static void KopiujPlikiDoTymczasowegoKatalogu(string[] ścieżki)
        {
            foreach (string ścieżka in ścieżki)
            {
                string ścieżkaTymczasowa;
                string nazwaPliku = Path.GetFileName(ścieżka);

                if (Directory.GetParent(ścieżka).Name.ToLower() == katalogKartSt)
                    ścieżkaTymczasowa = Path.Combine(tymczasowyKatalogBaz, katalogKartSt, nazwaPliku);
                else
                    ścieżkaTymczasowa = Path.Combine(tymczasowyKatalogBaz, nazwaPliku);

                File.Copy(ścieżka, ścieżkaTymczasowa);
            }
        }
    }
}