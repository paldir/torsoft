using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;
using System.Web.UI;
using System.Drawing;

namespace czynsze.Kontrolki
{
    public class Table : System.Web.UI.WebControls.Table
    {
        public Table(string klasaCss, List<string[]> wiersze, string[] wierszNagłówkowy, InformacjeOSortowalnościTablicy informacje, string prefiks, List<int> indeksyNumerycznychKolumn, List<int> indeksyKolumnZPodsumowaniem, string Id)
        {
            float[] szerokościKolumn = new float[wierszNagłówkowy.Length];
            Bitmap bitmapa = new Bitmap(500, 200);
            Graphics grafika = Graphics.FromImage(bitmapa);
            grafika.PageUnit = GraphicsUnit.Pixel;
            Font czcionka = new Font("Times New Roman", 12);
            decimal[] podsumowania = new decimal[wierszNagłówkowy.Length];
            float szerokośćKolumny;
            string tekstKomórki;
            ID = Id;

            for (int i = 0; i < szerokościKolumn.Length; i++)
            {
                szerokościKolumn[i] = 0;
                podsumowania[i] = 0;
            }

            CssClass = klasaCss;

            foreach (string[] row in wiersze)
            {
                TableRow wiersz = new TableRow();
                wiersz.CssClass = "tableRow";
                wiersz.ID = prefiks + row[0] + "_row";
                TableCell komórka = new TableCell();
                komórka.CssClass = "tableCell";
                tekstKomórki = row[1];

                if (indeksyNumerycznychKolumn.Contains(1))
                {
                    tekstKomórki = HandleNumericCell(tekstKomórki);
                    komórka.CssClass += " numericTableCell";
                }

                if (indeksyKolumnZPodsumowaniem.Contains(1) && !String.IsNullOrEmpty(tekstKomórki))
                    podsumowania[0] += Decimal.Parse(tekstKomórki);

                komórka.Controls.Add(new RadioButton("radioButton", prefiks + row[0], "id"));
                komórka.Controls.Add(new Label("label", row[0], row[1], String.Empty));
                wiersz.Cells.Add(komórka);
                Rows.Add(wiersz);

                szerokośćKolumny = grafika.MeasureString(row[1], czcionka).Width;

                if (szerokośćKolumny > szerokościKolumn[0])
                    szerokościKolumn[0] = szerokośćKolumny;
            }

            for (int i = 0; i < wiersze.Count(); i++)
            {
                for (int j = 2; j < wiersze.ElementAt(i).Count(); j++)
                {
                    TableCell komórka = new TableCell();
                    komórka.CssClass = "tableCell";
                    tekstKomórki = wiersze.ElementAt(i).ElementAt(j);

                    if (indeksyNumerycznychKolumn.Contains(j))
                    {
                        tekstKomórki = HandleNumericCell(tekstKomórki);
                        komórka.CssClass += " numericTableCell";
                    }

                    if (indeksyKolumnZPodsumowaniem.Contains(j) && !String.IsNullOrEmpty(tekstKomórki))
                        podsumowania[j - 1] += Decimal.Parse(tekstKomórki);

                    komórka.Controls.Add(new Label("label", wiersze.ElementAt(i).ElementAt(0), tekstKomórki, String.Empty));
                    Rows[i].Cells.Add(komórka);

                    szerokośćKolumny = grafika.MeasureString(wiersze.ElementAt(i).ElementAt(j), czcionka).Width;

                    if (szerokośćKolumny > szerokościKolumn[j - 1])
                        szerokościKolumn[j - 1] = szerokośćKolumny;
                }
            }

            TableHeaderRow wierszNagłówkowyTabeli = new TableHeaderRow();
            wierszNagłówkowyTabeli.CssClass = "tableHeaderRow";
            wierszNagłówkowyTabeli.TableSection = TableRowSection.TableHeader;

            for (int i = 0; i < wierszNagłówkowy.Length; i++)
            {
                TableHeaderCell komórkaNagłówka = new TableHeaderCell();
                komórkaNagłówka.CssClass = "tableHeaderCell";

                if (informacje.Sortowalna)
                {
                    string klasa = "sortLink";

                    if (i == informacje.IndeksKolumnySortującej)
                        klasa += " asc";

                    komórkaNagłówka.Controls.Add(new LiteralControl(String.Format("<a href=\"#\" id=\"{0}\" class=\"{1}\">{2}</a>", i, klasa, wierszNagłówkowy[i])));
                }
                else
                    komórkaNagłówka.Controls.Add(new LiteralControl(wierszNagłówkowy[i]));

                szerokośćKolumny = grafika.MeasureString(wierszNagłówkowy[i], czcionka).Width;

                if (szerokośćKolumny > szerokościKolumn[i])
                    szerokościKolumn[i] = szerokośćKolumny;

                wierszNagłówkowyTabeli.Cells.Add(komórkaNagłówka);
            }

            Rows.AddAt(0, wierszNagłówkowyTabeli);

            TableFooterRow wierszStopki = new TableFooterRow();
            wierszStopki.CssClass = "tableFooterRow";
            wierszStopki.TableSection = TableRowSection.TableFooter;

            for (int i = 0; i < wierszNagłówkowy.Length; i++)
            {
                TableCell komórka = new TableCell();
                komórka.CssClass = "tableFooterCell numericTableCell";

                if (!indeksyKolumnZPodsumowaniem.Contains(i + 1))
                    komórka.Text = String.Empty;
                else
                    komórka.Text = HandleNumericCell(podsumowania[i].ToString());

                szerokośćKolumny = grafika.MeasureString(komórka.Text, czcionka).Width;

                if (szerokośćKolumny > szerokościKolumn[i])
                    szerokościKolumn[i] = szerokośćKolumny;

                wierszStopki.Cells.Add(komórka);
            }

            Rows.Add(wierszStopki);

            if (Rows.Count > 1)
                for (int i = 0; i < wierszNagłówkowy.Length; i++)
                    Rows[0].Cells[i].Width = Rows[1].Cells[i].Width = Rows[Rows.Count - 1].Cells[i].Width = new Unit(szerokościKolumn[i]);
        }

        static string HandleNumericCell(string cellText)
        {
            if (String.IsNullOrEmpty(cellText))
                return cellText;
            else
            {
                string format;

                if (!cellText.Contains(','))
                    format = "{0:N0}";
                else
                    format = "{0:N}";

                return String.Format(format, Single.Parse(cellText));
            }
        }
    }
}