using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class NaleznosciIObrotyNajemcy : Strona
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string[] headers = new string[] { "Kwota Wn", "Kwota Ma", "Data", "Operacja" };
            List<int> indexesOfNumericColumns = new List<int>() { 1, 2 };
            string summary;
            int __record = PobierzWartośćParametru<int>("id");
            string nagłówek;

            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                DostępDoBazy.Najemca tenant = db.AktywniNajemcy.Find(__record);
                IEnumerable<DostępDoBazy.Należność> receivables = null;
                IEnumerable<DostępDoBazy.Obrót> turnovers = null;
                nagłówek = Title = "Należności  i obroty najemcy " + tenant.nazwisko + " " + tenant.imie;
                int nrKontr = tenant.nr_kontr;

                switch (Start.AktywnyZbiór)
                {
                    case Enumeratory.Zbiór.Czynsze:
                        receivables = db.Należności1.Where(r => r.nr_kontr == nrKontr).AsEnumerable<DostępDoBazy.Należność>();
                        turnovers = db.Obroty1.Where(t => t.nr_kontr == nrKontr).AsEnumerable<DostępDoBazy.Obrót>();

                        break;

                    case Enumeratory.Zbiór.Drugi:
                        receivables = db.Należności2.Where(r => r.nr_kontr == nrKontr).AsEnumerable<DostępDoBazy.Należność>();
                        turnovers = db.Obroty2.Where(t => t.nr_kontr == nrKontr).AsEnumerable<DostępDoBazy.Obrót>();

                        break;

                    case Enumeratory.Zbiór.Trzeci:
                        receivables = db.Należności3.Where(r => r.nr_kontr == nrKontr).AsEnumerable<DostępDoBazy.Należność>();
                        turnovers = db.Obroty3.Where(t => t.nr_kontr == nrKontr).AsEnumerable<DostępDoBazy.Obrót>();

                        break;
                }

                List<string[]> rekordyTabeli = receivables.Select(r => r.WażnePolaDoNależnościIObrotówNajemcy()).Concat(turnovers.Select(t => t.WażnePolaDoNależnościIObrotówNajemcy())).OrderBy(r => DateTime.Parse(r[3])).ToList();
                decimal wnAmount = rekordyTabeli.Sum(r => String.IsNullOrEmpty(r[1]) ? 0 : Decimal.Parse(r[1]));
                decimal maAmount = rekordyTabeli.Sum(r => String.IsNullOrEmpty(r[2]) ? 0 : Decimal.Parse(r[2]));
                List<string[]> rowsOfPastReceivables = receivables.Where(r => r.data_nal < Start.Data).Select(r => r.WażnePolaDoNależnościIObrotówNajemcy()).Concat(turnovers.Where(t => t.data_obr < Start.Data).Select(t => t.WażnePolaDoNależnościIObrotówNajemcy())).ToList();
                decimal wnAmountOfPastReceivables = rowsOfPastReceivables.Sum(r => String.IsNullOrEmpty(r[1]) ? 0 : Decimal.Parse(r[1]));
                summary = @"
                                <table class='additionalTable'>
                                    <tr>
                                        <td>Suma Wn: </td>
                                        <td class='numericTableCell'>" + String.Format("{0:N}", wnAmount) + @"</td>                                                                        
                                    </tr>
                                    <tr>
                                        <td>Suma Ma: </td>
                                        <td class='numericTableCell'>" + String.Format("{0:N}", maAmount) + @"</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td><hr /></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td class='numericTableCell'>" + String.Format("{0:N}", maAmount - wnAmount) + @"</td>
                                    </tr>
                                </table>
                                <table class='additionalTable'>
                                    <tr>
                                        <td>Należności przeterminowane: </td>
                                        <td class='numericTableCell'>" + String.Format("{0:N}", wnAmountOfPastReceivables) + @"</td>                                                                        
                                    </tr>
                                    <tr>
                                        <td>Suma Ma: </td>
                                        <td class='numericTableCell'>" + String.Format("{0:N}", maAmount) + @"</td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td><hr /></td>
                                    </tr>
                                    <tr>
                                        <td></td>
                                        <td class='numericTableCell'>" + String.Format("{0:N}", maAmount - wnAmountOfPastReceivables) + @"</td>
                                    </tr>
                                </table>";

                miejsceTabeli.Controls.Add(new Kontrolki.Table("mainTable", rekordyTabeli, headers, new Kontrolki.InformacjeOSortowalnościTablicy(), String.Empty, indexesOfNumericColumns, new List<int>(), String.Empty));
            }

            miejscePodTabelą.Controls.Add(new LiteralControl(summary));
            miejscePrzycisków.Controls.Add(new Kontrolki.Button("button", Enumeratory.Raport.SumyMiesięczneSkładnika.ToString(), "Sumy miesięczne składnika", "KonfiguracjaRaportu.aspx", "raport"));
            miejscePrzycisków.Controls.Add(new Kontrolki.Button("button", Enumeratory.Raport.WydrukNależnościIObrotów.ToString(), "Wydruk należności i obrotów", "KonfiguracjaRaportu.aspx", "raport"));
            miejscePrzycisków.Controls.Add(new Kontrolki.Button("button", Enumeratory.Raport.AnalizaMiesięczna.ToString(), "Analiza miesięczna", "KonfiguracjaRaportu.aspx", "raport"));
            miejscePrzycisków.Controls.Add(new Kontrolki.Button("button", Enumeratory.Raport.AnalizaSzczegółowa.ToString(), "Analiza szczegółowa", "KonfiguracjaRaportu.aspx", "raport"));
            Start.ŚcieżkaStrony.Dodaj(nagłówek, ŚcieżkaIQuery);
        }
    }
}