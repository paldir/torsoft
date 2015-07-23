using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace czynsze.Formularze
{
    public partial class SaldoNajemcy : Strona
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<int> indexesOfNumericColumns = new List<int>() { 1, 2, 3 };
            int id = PobierzWartośćParametru<int>("id");
            string nagłówek;

            using (DostępDoBazy.CzynszeKontekst db = new DostępDoBazy.CzynszeKontekst())
            {
                IEnumerable<DostępDoBazy.Należność> receivables = null;
                IEnumerable<DostępDoBazy.Obrót> turnovers = null;
                DostępDoBazy.Najemca tenant = db.AktywniNajemcy.FirstOrDefault(t => t.nr_kontr == id);
                nagłówek = Title = "Saldo najemcy " + tenant.nazwisko + " " + tenant.imie;

                switch (Start.AktywnyZbiór)
                {
                    case Enumeratory.Zbiór.Czynsze:
                        receivables = db.Należności1.Where(r => r.nr_kontr == id).AsEnumerable<DostępDoBazy.Należność>();
                        turnovers = db.Obroty1.Where(r => r.nr_kontr == id).AsEnumerable<DostępDoBazy.Obrót>();

                        break;

                    case Enumeratory.Zbiór.Drugi:
                        receivables = db.Należności2.Where(r => r.nr_kontr == id).AsEnumerable<DostępDoBazy.Należność>();
                        turnovers = db.Obroty2.Where(r => r.nr_kontr == id).AsEnumerable<DostępDoBazy.Obrót>();

                        break;

                    case Enumeratory.Zbiór.Trzeci:
                        receivables = db.Należności3.Where(r => r.nr_kontr == id).AsEnumerable<DostępDoBazy.Należność>();
                        turnovers = db.Obroty3.Where(r => r.nr_kontr == id).AsEnumerable<DostępDoBazy.Obrót>();

                        break;
                }

                List<string[]> rowsOfReceivablesAndTurnovers = receivables.Select(r => r.WażnePolaDoNależnościIObrotówNajemcy()).Concat(turnovers.Select(t => t.WażnePolaDoNależnościIObrotówNajemcy())).ToList();
                decimal wnAmount = rowsOfReceivablesAndTurnovers.Sum(r => String.IsNullOrEmpty(r[1]) ? 0 : Decimal.Parse(r[1]));
                decimal maAmount = rowsOfReceivablesAndTurnovers.Sum(r => String.IsNullOrEmpty(r[2]) ? 0 : Decimal.Parse(r[2]));
                List<string[]> rowsOfReceivablesAndTurnoversToDate = receivables.Where(r => r.data_nal <= Start.Data).Select(r => r.WażnePolaDoNależnościIObrotówNajemcy()).Concat(turnovers.Where(t => t.data_obr <= Start.Data).Select(r => r.WażnePolaDoNależnościIObrotówNajemcy())).ToList();
                decimal wnAmountToDay = rowsOfReceivablesAndTurnoversToDate.Sum(r => String.IsNullOrEmpty(r[1]) ? 0 : Decimal.Parse(r[1]));
                decimal maAmountToDay = rowsOfReceivablesAndTurnoversToDate.Sum(r => String.IsNullOrEmpty(r[2]) ? 0 : Decimal.Parse(r[2]));
                DostępDoBazy.Konfiguracja configuration = db.Konfiguracje.FirstOrDefault();
                List<string[]> rowsOfInterestNotes = turnovers.Where(t => t.kod_wplat == configuration.p_20 || t.kod_wplat == configuration.p_37).Select(t => t.WażnePolaDoNależnościIObrotówNajemcy()).ToList();
                decimal wnOfInterestNotes = rowsOfInterestNotes.Sum(r => String.IsNullOrEmpty(r[1]) ? 0 : Decimal.Parse(r[1]));
                decimal maOfInterestNotes = rowsOfInterestNotes.Sum(r => String.IsNullOrEmpty(r[2]) ? 0 : Decimal.Parse(r[2]));
                dzień.InnerText = DateTime.Now.ToShortDateString();
                saldo.InnerText = (maAmount - wnAmount).ToString("N");
                saldoNaDzień.InnerText = (maAmountToDay - wnAmountToDay).ToString("N");
                noty.InnerText = (maOfInterestNotes - wnOfInterestNotes).ToString("N");

               // Start.ŚcieżkaStrony.Add(nagłówek);
            }
        }
    }
}