using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Przychodnia.Class.XML;
using WotuwDataAccess;

namespace Przychodnia.XAML
{
    /// <summary>
    /// Interaction logic for RealizacjaUslugi.xaml
    /// </summary>
    public partial class RealizacjaUslugi : Window
    {
        Pacjent Pacjent { get; set; }
        Swiadczenie Swiadczenie { get; set; }
        MainWindow Parent { get; set; }
        List<DwaStringi> RealizujaceOsoby = ObslugaXml.WczytajRealizujaceOsoby();
        List<DwaStringi> RodzajeSwiadczen = ObslugaXml.WczytajRodzajSwiadczenia();
        List<DwaStringi> Platnicy = ObslugaXml.WczytajPlatnicy();
        List<DwaStringi> Poradnie = ObslugaXml.WczytajPoradnie();


        public RealizacjaUslugi(Pacjent pacjent, Swiadczenie swiadczenie, MainWindow parent)
        {
            InitializeComponent();

            Pacjent = pacjent;
            Swiadczenie = swiadczenie;
            Parent = parent;

            if (swiadczenie == null) Swiadczenie = new Swiadczenie();
            if (pacjent == null) Pacjent = new Pacjent();

            WypelnijPola();
        }

        private void WypelnijPola()
        {
            DanePacjenta.Text = Pacjent.nazwisko + " " + Pacjent.imie + " " + Pacjent.pesel;
            NrHistoriiChorobyBox.Text = Swiadczenie.kartoteka;

            foreach (var poradnia in Poradnie)
                RealizujacyKomorkaBox.Items.Add(poradnia.Nazwa);
            RealizujacyKomorkaBox.Text = DwaStringi.ZnajdzNazwePolaListy(Swiadczenie.realizujacyKomorka.ToString(), Poradnie);

            foreach (var osoba in RealizujaceOsoby)
                RealizujacyOsobaBox.Items.Add(osoba.Nazwa);
            RealizujacyOsobaBox.Text = DwaStringi.ZnajdzNazwePolaListy(Swiadczenie.realizujacyOsoba.ToString(), RealizujaceOsoby);

            foreach (var swiadczenie in RodzajeSwiadczen)
                RodzajSwiadczeniaBox.Items.Add(swiadczenie.Nazwa);
            RodzajSwiadczeniaBox.Text = DwaStringi.ZnajdzNazwePolaListy(Swiadczenie.rodzajSwiadczenia.ToString(), RodzajeSwiadczen);

            KlasyfikacjaBox.Text = Swiadczenie.klasyfikacjaIcd;

            foreach (var platnik in Platnicy)
                PlatnikBox.Items.Add(platnik.Nazwa);
            PlatnikBox.Text = DwaStringi.ZnajdzNazwePolaListy(Swiadczenie.podstawaPlatnosci, Platnicy);

            OplataPacjentaBox.Text = Swiadczenie.oplataPacjenta;
            DoplataKasyBox.Text = Swiadczenie.doplataKasy;
            DataRealizacjiDate.Text = Swiadczenie.dataRealizacji.ToString();
            DataRealizacjiDoDate.Text = Swiadczenie.dataRealizacjiDo.ToString();
            OsobaZlecajacaBox.Text = Swiadczenie.zlecajacyOsoba;
            InstytucjaZlecajacaBox.Text = Swiadczenie.zlecajacyFirma;
            DataWystaZleceniaDate.Text = Swiadczenie.dataZlecenia.ToString();
        }

        private void AnulujClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ZapiszClick(object sender, RoutedEventArgs e)
        {
            bool zakoncz = true;

            if (Swiadczenie.Id == 0) zakoncz = DodajSwiadczenie();
            else zakoncz = UpdateSwiadczenia();

            Parent.ZaznaczeniePacjenta(null, null);
            Parent.SwiadczeniaDataGrid.SelectedItem = Swiadczenie;
            Parent.Zaznaczenieswiadczenia(null, null);

            if (zakoncz) this.Close();
        }

        private bool DodajSwiadczenie()
        {
            Swiadczenie.Id = (from swiadczenie in Record.DataBase.Swiadczenia orderby swiadczenie.Id descending select swiadczenie.Id).FirstOrDefault() + 1;
            Swiadczenie.idKontaktu = (from swiadczenie in Record.DataBase.Swiadczenia orderby swiadczenie.idKontaktu descending select swiadczenie.idKontaktu).FirstOrDefault() + 1;
            Swiadczenie.idOsoby = Pacjent.Id;
            Swiadczenie.rodzajSwiadczenia = Record.ConvertIfPossible<int>(DwaStringi.ZnajdzWartoscPolaListy(RodzajSwiadczeniaBox.Text, RodzajeSwiadczen));
            Swiadczenie.klasyfikacjaIcd = KlasyfikacjaBox.Text;
            Swiadczenie.podstawaPlatnosci = DwaStringi.ZnajdzWartoscPolaListy(PlatnikBox.Text, Platnicy);
            Swiadczenie.oplataPacjenta = OplataPacjentaBox.Text;
            Swiadczenie.doplataKasy = DoplataKasyBox.Text;
            Swiadczenie.dataRealizacji = Record.ConvertIfPossible<DateTime>(DataRealizacjiDate.Text);
            Swiadczenie.dataRealizacjiDo = Record.ConvertIfPossible<DateTime>(DataRealizacjiDoDate.Text);
            Swiadczenie.zlecajacyOsoba = OsobaZlecajacaBox.Text;
            Swiadczenie.zlecajacyFirma = InstytucjaZlecajacaBox.Text;
            Swiadczenie.dataZlecenia = Record.ConvertIfPossible<DateTime>(DataWystaZleceniaDate.Text);
            Swiadczenie.realizujacyOsoba = Record.ConvertIfPossible<long>(DwaStringi.ZnajdzWartoscPolaListy(RealizujacyOsobaBox.Text, RealizujaceOsoby));
            Swiadczenie.realizujacyKomorka = Record.ConvertIfPossible<int>(DwaStringi.ZnajdzWartoscPolaListy(RealizujacyKomorkaBox.Text, Poradnie));
            Swiadczenie.kartoteka = NrHistoriiChorobyBox.Text;
            Swiadczenie.dataWpisu = null; //?????????????????????
            Swiadczenie.kasa = Pacjent.kasaChorych;

            if (ListaDanych() == null) return false;
            Swiadczenie.Add();
            return true;
        }

        private bool UpdateSwiadczenia()
        {
            List<string> lista = ListaDanych();

            if (lista == null) return false;
            Swiadczenie.Set(lista.ToArray());
            Swiadczenie.Update();
            return true;
        }

        private List<string> ListaDanych()
        {
            List<string> lista = new List<string>()
            {
                "", //Id
                "", //idKontaktu
                Pacjent.Id.ToString(),
                DwaStringi.ZnajdzWartoscPolaListy(RodzajSwiadczeniaBox.Text, RodzajeSwiadczen),
                KlasyfikacjaBox.Text,
                DwaStringi.ZnajdzWartoscPolaListy(PlatnikBox.Text, Platnicy),
                OplataPacjentaBox.Text,
                DoplataKasyBox.Text,
                DataRealizacjiDate.Text,
                DataRealizacjiDoDate.Text,
                OsobaZlecajacaBox.Text,
                InstytucjaZlecajacaBox.Text,
                DataWystaZleceniaDate.Text,
                DwaStringi.ZnajdzWartoscPolaListy(RealizujacyOsobaBox.Text, RealizujaceOsoby),
                DwaStringi.ZnajdzWartoscPolaListy(RealizujacyKomorkaBox.Text, Poradnie), //realizujacyKomorka
                NrHistoriiChorobyBox.Text,
                "", //Swiadczenie.dataWpisu
                Pacjent.kasaChorych,
            };

            string msg = Swiadczenie.Validate(lista);
            if (String.IsNullOrEmpty(msg)) return lista;

            MessageBox.Show(msg);
            return null;
        }
    }
}
