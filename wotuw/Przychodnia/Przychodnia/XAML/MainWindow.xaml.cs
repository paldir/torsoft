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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Przychodnia.Class.XML;
using Przychodnia.XAML;
using WotuwDataAccess;

namespace Przychodnia
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Window _okno;

        public MainWindow()
        {
            InitializeComponent();

            PokazUkryj(0);
        }

        #region Widok Startowy

        private void PokazUkryj(int ind)
        {
            var lista = new List<GroupBox>() { GroupLogowanie, GroupKartoteki };

            foreach (var temp in lista)
            {
                temp.IsEnabled = false;
                temp.Visibility = Visibility.Hidden;
            }

            lista[ind].IsEnabled = true;
            lista[ind].Visibility = Visibility.Visible;
        }
        private void WcisniecieEnter(object sender, KeyEventArgs args)
        {
            if (args.Key != Key.Return) return;

            if (FkTuz.AccessGranted(LoginBox.Text, HasloBox.Password))
            {
                Uzytkownik.RegisterUserIn(LoginBox.Text, LoginBox.Text);
                PokazUkryj(1);
                UzupełnijPacjenciDataGrid((from pacjent in Record.DataBase.Pacjenci select pacjent).ToList());
            }
            else
            {
                Uzytkownik.RegisterUserIn(LoginBox.Text, LoginBox.Text);
                Uzytkownik.CurrentUser.uwagi = "Nieudana proba logowania";
                Uzytkownik.RegisterUserOut();

                MessageBox.Show("Błędny login lub haslo");
            }
        }
        #endregion

        #region Przyciski/Buttony Od Pacjentow i Wizyt

        private void NowyPacjentClick(object sender, RoutedEventArgs e)
        {
            _okno = new DanePacjenta(new Pacjent(), this);
            _okno.Show();
        }
        private void ZmianaDanychPacjentaClick(object sender, RoutedEventArgs e)
        {
            _okno = new DanePacjenta((Pacjent)PacjenciDataGrid.SelectedItem, this);
            _okno.Show();
        }

        private void NowaWizytaClick(object sender, RoutedEventArgs e)
        {
            if ((Pacjent)PacjenciDataGrid.SelectedItem != null)
            {
                _okno = new RealizacjaUslugi((Pacjent)PacjenciDataGrid.SelectedItem, new Swiadczenie(), this);
                _okno.Show();
            }
            else
            {
                MessageBox.Show("Nie wybrano pacjenta");
            }
        }

        private void ZmianaDanychUslugiClick(object sender, RoutedEventArgs e)
        {
            if ((Pacjent)PacjenciDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Nie wybrano pacjenta");
                return;
            }
            if ((Swiadczenie)SwiadczeniaDataGrid.SelectedItem == null)
            {
                MessageBox.Show("Nie wybrano swiadczenia/wizyty");
                return;
            }
            _okno = new RealizacjaUslugi((Pacjent)PacjenciDataGrid.SelectedItem, (Swiadczenie)SwiadczeniaDataGrid.SelectedItem, this);
            _okno.Show();
        }

        private void KoniecClick(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void KoniecClick(object sender, EventArgs e)
        {
            if (Uzytkownik.CurrentUser == null)
            {
                Uzytkownik.RegisterUserIn(null, null);
                Uzytkownik.CurrentUser.uwagi = "Nieudana proba logowania";
            }

            Uzytkownik.RegisterUserOut();
        }

        #endregion

        #region Zaznaczenia i Wypisanie Danych

        public void ZaznaczeniePacjenta(object sender, SelectionChangedEventArgs e)
        {
            if (PacjenciDataGrid.SelectedItem == null) return;

            WypiszDanePacjenta((Pacjent)PacjenciDataGrid.SelectedItem);

            IQueryable<WotuwDataAccess.Swiadczenie> listaSwiadczen =
                from swiadczenie in WotuwDataAccess.Record.DataBase.Swiadczenia
                where swiadczenie.idOsoby == ((Pacjent)PacjenciDataGrid.SelectedItem).Id
                select swiadczenie;
            UzupelnijSwiadczeniaDataGrid(listaSwiadczen.ToList());
        }

        public void Zaznaczenieswiadczenia(object sender, SelectionChangedEventArgs e)
        {
            WypiszDaneSwiadczenia((Swiadczenie)SwiadczeniaDataGrid.SelectedItem);
        }

        private void WypiszDanePacjenta(Pacjent pacjent)
        {
            NazwiskoBox.Text = "Nazwisko: " + ((pacjent != null) ? pacjent.nazwisko : null);
            ImieBox.Text = "Imie: " + ((pacjent != null) ? pacjent.imie : null);
            DataUrodzeniaBox.Text = "Data Urodzenia: " + ((pacjent != null && pacjent.dataUrodzenia!=null) ? pacjent.dataUrodzenia.Value.ToShortDateString() : null);
            PeselBox.Text = "PESEL: " + ((pacjent != null) ? pacjent.pesel : null);
            MiejscowoscBox.Text = "Miejscowość: " + ((pacjent != null) ? pacjent.miejscowosc : null);
            KasaChorychPacjentBox.Text = "Kasa Chorych: " + ((pacjent != null) ? pacjent.kasaChorych : null);
        }

        private void WypiszDaneSwiadczenia(Swiadczenie swiadczenie)
        {
            PoradniaBox.Text = "Poradnia: \n" + ((swiadczenie != null) ? "   " + DwaStringi.ZnajdzNazwePolaListy(swiadczenie.realizujacyKomorka.ToString(), ObslugaXml.WczytajPoradnie()) : null); //realizujacy komorka
            RealizujacyBox.Text = "Realizujący: \n" + ((swiadczenie != null) ? "    " + DwaStringi.ZnajdzNazwePolaListy(swiadczenie.realizujacyOsoba.ToString(), ObslugaXml.WczytajRealizujaceOsoby()) : null); ; //realizujacy osoba
            DataSwiadczeniaBox.Text = "Data świadczenia: " + ((swiadczenie != null && swiadczenie.dataRealizacji != null) ? swiadczenie.dataRealizacji.Value.ToShortDateString() : null);
            RozpoznanieBox.Text = "Rozpoznanie: " + ((swiadczenie != null) ? swiadczenie.klasyfikacjaIcd : null);
            KartotekaBox.Text = "Kartoteka: " + ((swiadczenie != null) ? swiadczenie.kartoteka : null);
            KasaChorychSwiadczenieBox.Text = "Kasa chorych: " + ((swiadczenie != null) ? swiadczenie.kasa : null);
        }

        #endregion

        #region Wypelnienie DataGridow

        private void UzupełnijPacjenciDataGrid(List<Pacjent> listaPacjentow)
        {
            PacjenciDataGrid.AutoGenerateColumns = false;
            PacjenciDataGrid.Columns.Clear();
            PacjenciDataGrid.ColumnWidth = new DataGridLength(135);
            PacjenciDataGrid.Columns.Add(new DataGridTextColumn() { Header = "Nazwisko", Binding = new Binding("nazwisko"), });
            PacjenciDataGrid.Columns.Add(new DataGridTextColumn() { Header = "Imię", Binding = new Binding("imie") });
            PacjenciDataGrid.Columns.Add(new DataGridTextColumn() { Header = "PESEL", Binding = new Binding("pesel") });

            PacjenciDataGrid.ItemsSource = null;
            PacjenciDataGrid.ItemsSource = listaPacjentow;
        }

        private void UzupelnijSwiadczeniaDataGrid(List<Swiadczenie> listaSwiadczen)
        {
            SwiadczeniaDataGrid.AutoGenerateColumns = false;
            SwiadczeniaDataGrid.Columns.Clear();
            SwiadczeniaDataGrid.ColumnWidth = new DataGridLength(120);
            SwiadczeniaDataGrid.Columns.Add(new DataGridTextColumn() { Header = "Data", Binding = new Binding("dataRealizacji"){StringFormat = "yyyy-MM-dd"}, });

            SwiadczeniaDataGrid.ItemsSource = null;
            SwiadczeniaDataGrid.ItemsSource = listaSwiadczen;
        }

        #endregion

        #region Wyszukiwanie i Czyszczenie Wyszukiwania

        public void SzukajPacjenta(object sender, KeyEventArgs args)
        {
            IQueryable<WotuwDataAccess.Pacjent> listaPacjentow =
                from pacjent in WotuwDataAccess.Record.DataBase.Pacjenci
                where (pacjent.nazwisko != null && pacjent.nazwisko.Contains(SzukajNazwiska.Text)) &&
                      (pacjent.imie != null && pacjent.imie.Contains(SzukajImienia.Text)) &&
                      (pacjent.pesel != null && pacjent.pesel.Contains(SzukajPesel.Text))
                select pacjent;

            UzupełnijPacjenciDataGrid(listaPacjentow.ToList());
        }

        private void SzukajKartotekiKey(object sender, KeyEventArgs args)
        {
            if ((args != null) && (args.Key != Key.Return)) return;

            List<long?> listaIdOsob = (from swiadczenie in WotuwDataAccess.Record.DataBase.Swiadczenia
                                          where (swiadczenie.kartoteka != null && swiadczenie.kartoteka.Contains(SzukajKartotekiBox.Text))
                                          select swiadczenie.idOsoby).ToList();

            listaIdOsob = listaIdOsob.Distinct().ToList();

            List<Pacjent> listaPacjentow = (from pacjent in Record.DataBase.Pacjenci where listaIdOsob.Contains(pacjent.Id) select pacjent).ToList();
            UzupełnijPacjenciDataGrid(listaPacjentow);
        }

        private void CzyscClick(object sender, RoutedEventArgs e)
        {
            SzukajNazwiska.Text = "";
            SzukajImienia.Text = "";
            SzukajPesel.Text = "";
            SzukajKartotekiBox.Text = "";
            UzupełnijPacjenciDataGrid((from pacjent in Record.DataBase.Pacjenci select pacjent).ToList());
        }

        #endregion
    }
}
