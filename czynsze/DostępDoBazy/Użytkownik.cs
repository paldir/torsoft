using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace czynsze.DostępDoBazy
{
    [Table("fk_tuz", Schema = "public")]
    public class Użytkownik : IRekord
    {
        [Key, Column("__record")]
        public int __record { get; set; }

        [Column("symbol")]
        public string symbol { get; set; }

        [Column("nazwisko")]
        public string nazwisko { get; set; }

        [Column("imie")]
        public string imie { get; set; }

        [Column("uzytkownik")]
        public string uzytkownik { get; set; }

        [Column("haslo")]
        public string haslo { get; set; }

        public string[] PolaDoTabeli()
        {
            return new string[]
            {
                __record.ToString(),
                symbol,
                nazwisko,
                imie
            };
        }

        public string[] WszystkiePola()
        {
            return new string[]
            {
                __record.ToString(),
                symbol.Trim(),
                nazwisko.Trim(),
                imie.Trim(),
                uzytkownik.Trim(),
                haslo.Trim()
            };
        }

        public void Ustaw(string[] rekord)
        {
            symbol = rekord[1];
            nazwisko = rekord[2];
            imie = rekord[3];
            uzytkownik = rekord[2] + " " + rekord[3];
            haslo = Encoding.UTF8.GetString(Encoding.UTF8.GetBytes(rekord[4]).Select(b => (byte)(b + 10)).ToArray());
        }

        public string Waliduj(Enumeratory.Akcja akcja, string[] rekord)
        {
            string wynik = String.Empty;
            List<string> listaRekordów = rekord.ToList();

            switch (akcja)
            {
                case Enumeratory.Akcja.Dodaj:
                    if (rekord[2].Length > 0 && rekord[3].Length > 0)
                    {
                        using (DostępDoBazy.CzynszeKontekst db = new CzynszeKontekst())
                            if (db.Użytkownicy.ToList().Any(u => u.nazwisko.Trim() == listaRekordów.ElementAt(1) && u.imie.Trim() == listaRekordów.ElementAt(2)))
                                wynik += "Użytkownik o podanym nazwisku i imieniu już istnieje! <br />";
                    }
                    else
                        wynik += "Należy podać nazwisko i imię! <br />";

                    break;
            }

            if (akcja != Enumeratory.Akcja.Usuń)
            {
                if (rekord[4].Length > 0)
                {
                    if (rekord[4] != rekord[5])
                        wynik += "Podane hasła nie są identyczne! <br />";
                }
                else
                    wynik += "Hasło nie może być puste! <br />";
            }

            return wynik;
        }
    }
}