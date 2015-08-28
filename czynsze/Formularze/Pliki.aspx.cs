using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.IO;

namespace czynsze.Formularze
{
    public partial class Pliki : Strona
    {
        FileUpload _plik;

        protected void Page_Load(object sender, EventArgs e)
        {
            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                int id_obiektu = PobierzWartośćParametru<int>("id_obiektu");
                int idPliku = PobierzWartośćParametru<int>("id");
                Enumeratory.Tabela tabela = PobierzWartośćParametru<Enumeratory.Tabela>("tabela");
                bool otwieraćOknoDodawania = !String.IsNullOrEmpty(PobierzWartośćParametru<string>("otwarcieOknaDodawania"));
                bool usunąćPlik = !String.IsNullOrEmpty(PobierzWartośćParametru<string>("delete"));
                bool dodaćPlik = !String.IsNullOrEmpty(PobierzWartośćParametru<string>("potwierdźDodawanie"));
                List<DostępDoBazy.Plik> pliki = WartościSesji.Pliki;

                form.Controls.Add(new Kontrolki.HtmlInputHidden("tabela", tabela));
                form.Controls.Add(new Kontrolki.HtmlInputHidden("id_obiektu", id_obiektu));

                List<string[]> wiersze = new List<string[]>();
                int liczbaPorządkowa = 1;

                if (otwieraćOknoDodawania)
                {
                    _plik = new FileUpload();
                    _plik.ID = "zawartość";
                    Button potwierdzanieDodawnia = new Kontrolki.Button("field", "potwierdźDodawanie", "Zapisz", String.Empty);

                    miejsceOknaDodawania.Controls.Add(new LiteralControl("<br />"));
                    miejsceOknaDodawania.Controls.Add(_plik);
                    miejsceOknaDodawania.Controls.Add(new LiteralControl("<br /><br />"));
                    miejsceOknaDodawania.Controls.Add(new Kontrolki.Label("field", "opis", "Opis: ", String.Empty));
                    miejsceOknaDodawania.Controls.Add(new Kontrolki.TextBox("field", "opis", Kontrolki.TextBox.TextBoxMode.KilkaLinii, 100, 4, true));
                    miejsceOknaDodawania.Controls.Add(new LiteralControl("<br />"));
                    miejsceOknaDodawania.Controls.Add(potwierdzanieDodawnia);
                    miejsceOknaDodawania.Controls.Add(new Kontrolki.Button("field", "anuluj", "Anuluj", "javascript: history.back()"));
                }
                else
                {
                    Kontrolki.Button przyciskPrzeglądania = new Kontrolki.Button("button", "browse", "Przeglądaj", "Plik.aspx");

                    miejscePrzycisków.Controls.Add(new Kontrolki.Button("button", "otwarcieOknaDodawania", "Dodaj", String.Empty));
                    miejscePrzycisków.Controls.Add(new Kontrolki.Button("button", "delete", "Usuń", String.Empty));
                    miejscePrzycisków.Controls.Add(przyciskPrzeglądania);
                }

                if (dodaćPlik)
                {
                    HttpPostedFile zawartośćPliku = Request.Files["zawartość"];
                    DostępDoBazy.Plik plik = null;

                    switch(tabela)
                    {
                        case Enumeratory.Tabela.AktywneLokale:
                        case Enumeratory.Tabela.NieaktywneLokale:
                            plik = new DostępDoBazy.PlikLokalu();

                            break;

                        case Enumeratory.Tabela.Budynki:
                            plik = new DostępDoBazy.PlikBudynku();

                            break;
                    }

                    plik.id_obiektu = id_obiektu;
                    plik.nazwa_pliku = Path.GetFileName(zawartośćPliku.FileName);
                    plik.opis = PobierzWartośćParametru<string>("opis");
                    Stream strumień = zawartośćPliku.InputStream;
                    List<byte> bajty = new List<byte>();

                    for (int i = 0; i < strumień.Length; i++)
                        bajty.Add(Convert.ToByte(strumień.ReadByte()));

                    plik.plik = Convert.ToBase64String(bajty.ToArray());
                    int id;

                    if (pliki.Any())
                        id = pliki.Max(p => p.__record) + 1;
                    else
                        id = 1;

                    plik.__record = id;

                    pliki.Add(plik);
                }

                if (usunąćPlik)
                {
                    DostępDoBazy.Plik plikDoUsunięcia = pliki.Single(p => p.__record == idPliku);

                    pliki.Remove(plikDoUsunięcia);
                }

                foreach (DostępDoBazy.Plik plik in pliki)
                {
                    string[] wiersz = new string[] { plik.__record.ToString(), liczbaPorządkowa.ToString(), plik.nazwa_pliku, plik.opis };
                    liczbaPorządkowa++;

                    wiersze.Add(wiersz);
                }

                miejsceTabeli.Controls.Add(new Kontrolki.Table("mainTable tabTable", wiersze, new string[] { "L.p.", "Nazwa pliku", "Opis" }, false, String.Empty, new List<int>() { 1 }, new List<int>()));
            }
        }
    }
}