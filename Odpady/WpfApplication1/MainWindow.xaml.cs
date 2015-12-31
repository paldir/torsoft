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

namespace WpfApplication1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            List<Osoba> osoby = new List<Osoba>();
            grid.AutoGenerateColumns = false;

            osoby.Add(new Osoba() { Nazwisko = "Kowalski", Telefon = new NrTelefonu() { Kierunkowy = 48, Właściwy = 0700 } });
            osoby.Add(new Osoba() { Nazwisko = "Nowak", Telefon = new NrTelefonu() { Kierunkowy = 54, Właściwy = 880 } });

            DataGridTextColumn nazwisko = new DataGridTextColumn();
            nazwisko.Header = "Nazwisko";
            nazwisko.Binding = new Binding("Nazwisko");

            DataGridTextColumn kierunkowy = new DataGridTextColumn();
            kierunkowy.Header = "Kierunkowy";
            kierunkowy.Binding = new Binding("Telefon.Kierunkowy");

            DataGridTextColumn właściwy = new DataGridTextColumn();
            właściwy.Header = "Właściwy";
            właściwy.Binding = new Binding("Telefon.Właściwy");

            grid.Columns.Clear();
            grid.Columns.Add(nazwisko);
            grid.Columns.Add(kierunkowy);
            grid.Columns.Add(właściwy);

            grid.ItemsSource = osoby;
        }
    }
}