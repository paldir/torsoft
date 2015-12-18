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
    /// Interaction logic for DanePacjenta.xaml
    /// </summary>
    public partial class DanePacjenta : Window
    {
        Pacjent Pacjent { get; set; }
        MainWindow Parent { get; set; }

        List<DwaStringi> ListaKasChorych = ObslugaXml.WczytajKasyChorych();

        public DanePacjenta(Pacjent pacjent, MainWindow parent)
        {
            InitializeComponent();

            if(pacjent==null) Pacjent=new Pacjent();
            else Pacjent = pacjent;

            Parent = parent;

            WypelnijPola();
        }

        private void WypelnijPola()
        {
            NazwiskoBox.Text = Pacjent.nazwisko;
            ImieBox.Text = Pacjent.imie;
            PeselBox.Text = Pacjent.pesel;
            if (Pacjent.dataUrodzenia != null) DataUrodzeniaDate.Text = Pacjent.dataUrodzenia.Value.ToString();
            PlecBox.ItemsSource = new List<string> {"", "K", "M"};
            PlecBox.Text = Pacjent.plec;
            MiejscowoscBox.Text = Pacjent.miejscowosc;
            UlicaBox.Text = Pacjent.ulica;
            NrDomuBox.Text = Pacjent.nrDomu;
            KodPocztowyBox.Text = Pacjent.kodPocztowy;
            PocztaBox.Text = Pacjent.poczta;
            GminaBox.Text = Pacjent.gmina;

            foreach (var kasa in ListaKasChorych)
                KasaChorychBox.Items.Add(kasa.Nazwa);
            KasaChorychBox.Text = DwaStringi.ZnajdzNazwePolaListy(Pacjent.kasaChorych, ListaKasChorych);

            //Pacjent.branza = ""; //branza
            NrUbezpieczeniaBox.Text = Pacjent.nrUbezpieczenia;
            DataPierWizyDate.Text = (Pacjent.dataPierwszejWizyty!=null)?Pacjent.dataPierwszejWizyty.ToString():null;
            PoborowyBox.IsChecked = Pacjent.poborowy == true;
            //Pacjent.grupaOpiekuncza = ""; //grupaOpiekuncza
            //Pacjent.obcy = null; //obcy 
            //Pacjent.oddzial = ""; //oddzial
        }

        private void AnulujClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ZapiszClick(object sender, RoutedEventArgs e)
        {
            bool zakoncz = true;

            if (Pacjent.Id == 0) zakoncz=DodajPacjenta();
            else zakoncz=UpdatePacjenta();

            Parent.SzukajPacjenta(null, null);
            Parent.PacjenciDataGrid.SelectedItem = Pacjent;
            
            if(zakoncz) this.Close();
        }

        private bool DodajPacjenta()
        {
            Pacjent.Id = (from pacjent in Record.DataBase.Pacjenci orderby pacjent.Id descending select pacjent.Id).FirstOrDefault() + 1;
            Pacjent.nazwisko = NazwiskoBox.Text;
            Pacjent.imie = ImieBox.Text;
            Pacjent.imieOjca = ""; //imieOjca
            Pacjent.pesel = PeselBox.Text;
            Pacjent.dataUrodzenia = Record.ConvertIfPossible<DateTime>(DataUrodzeniaDate.Text);
            Pacjent.plec = PlecBox.Text;
            Pacjent.miejscowosc = MiejscowoscBox.Text;
            Pacjent.ulica = UlicaBox.Text;
            Pacjent.nrDomu = NrDomuBox.Text;
            Pacjent.kodPocztowy = KodPocztowyBox.Text;
            Pacjent.poczta = PocztaBox.Text;
            Pacjent.gmina = GminaBox.Text;
            Pacjent.kasaChorych = DwaStringi.ZnajdzWartoscPolaListy(KasaChorychBox.Text, ListaKasChorych);
            Pacjent.branza = ""; //branza
            Pacjent.nrUbezpieczenia = NrUbezpieczeniaBox.Text;
            Pacjent.dataPierwszejWizyty = Record.ConvertIfPossible<DateTime>(DataPierWizyDate.Text);
            Pacjent.poborowy = PoborowyBox.IsChecked;
            Pacjent.grupaOpiekuncza = ""; //grupaOpiekuncza
            Pacjent.obcy = null; //obcy 
            Pacjent.oddzial = ""; //oddzial

            if (ListaDanych() == null) return false;
            Pacjent.Add();
            return true;
        }

        private bool UpdatePacjenta()
        {
            List<string> lista = ListaDanych();

            if (lista == null) return false;
            Pacjent.Set(lista.ToArray());
            Pacjent.Update();
            return true;
        }

        private List<string> ListaDanych()
        {
            List<string> lista = new List<string>()
            {
                "", //Id
                NazwiskoBox.Text,
                ImieBox.Text,
                "", //imieOjca
                PeselBox.Text,
                DataUrodzeniaDate.Text,
                PlecBox.Text,
                MiejscowoscBox.Text,
                UlicaBox.Text,
                NrDomuBox.Text,
                KodPocztowyBox.Text,
                PocztaBox.Text,
                GminaBox.Text,
                DwaStringi.ZnajdzWartoscPolaListy(KasaChorychBox.Text, ListaKasChorych),
                "", //branza
                NrUbezpieczeniaBox.Text,
                DataPierWizyDate.Text,
                PoborowyBox.IsChecked.ToString(),
                "", //grupaOpiekuncza
                "", //obcy 
                "", //oddzial
            };

            string msg = Pacjent.Validate(lista);
            if (String.IsNullOrEmpty(msg)) return lista;
            
            MessageBox.Show(msg);
            return null;
            
        }

        private void WpisywaniePeselu(object sender, KeyEventArgs e)
        {
            if (PeselBox.Text.Count() != 11) return;

            string wynik = PeselBox.Text.Substring(0, 2);

            switch (Convert.ToInt32(PeselBox.Text.Substring(2, 2)) / 20)
            {
                case 0:
                    wynik = "19" + wynik; break;
                case 1:
                    wynik = "20" + wynik; break;
                case 2:
                    wynik = "21" + wynik; break;
                case 3:
                    wynik = "22" + wynik; break;
                case 4:
                    wynik = "18" + wynik; break;
            }

            wynik += "-" + (Convert.ToInt32(PeselBox.Text.Substring(2, 2)) % 20) + "-" + PeselBox.Text.Substring(4, 2);

            DataUrodzeniaDate.Text = wynik;
        }
    }
}
