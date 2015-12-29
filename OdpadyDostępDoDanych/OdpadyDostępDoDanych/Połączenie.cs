using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.IO;
using System.Reflection;
using System.Configuration;
using System.Collections.Specialized;

namespace OdpadyDostępDoDanych
{
    public class Połączenie : IDisposable
    {
        const int KodBłęduPz = 144000;
        static readonly Dictionary<Type, string> _typRekorduNaNazwęTabeli;
        
        FbConnection _połączenie;
        string _ścieżkaPlikuBazy;

        public Połączenie()
        {
            NameValueCollection ustawienia = ConfigurationManager.AppSettings;
            _ścieżkaPlikuBazy = ustawienia["Database"];

            string parametry =
            "User=SYSDBA;" +
            "Password=masterkey;" +
            "Database=" + _ścieżkaPlikuBazy + ";" +
            "DataSource=" + ustawienia["DataSource"] + ";" +
            "Port=3050;" +
            "Dialect=3;" +
            "Charset=NONE;" +
            "Role=;" +
            "Connection lifetime=15;" +
            "Pooling=true;" +
            "MinPoolSize=0;" +
            "MaxPoolSize=50;" +
            "Packet Size=8192;" +
            "ServerType=0";

            _połączenie = new FbConnection(parametry);

            try
            {
                _połączenie.Open();
            }
            catch (FbException wyjątek)
            {
                ZapiszWyjątekDoLogu(wyjątek, "Otwarcie połączenia.");
            }
        }

        static Połączenie()
        {
            _typRekorduNaNazwęTabeli = new Dictionary<Type, string>();

            _typRekorduNaNazwęTabeli.Add(typeof(Kontrahent), "KONTRAHENCI");
            _typRekorduNaNazwęTabeli.Add(typeof(Oddział), "ODDZIALY");
        }

        public List<T> PobierzWszystkie<T>() where T : Rekord
        {
            List<T> rekordy = new List<T>();
            Type typRekordu = typeof(T);
            IEnumerable<PropertyInfo> właściwości = typRekordu.GetProperties().Where(w => w.GetSetMethod() != null);
            StringBuilder budowniczyZapytania = new StringBuilder("SELECT ");
            int liczbaPól = właściwości.Count();

            foreach (PropertyInfo właściwość in właściwości)
                budowniczyZapytania.AppendFormat("{0}, ", właściwość.Name);

            budowniczyZapytania.Remove(budowniczyZapytania.Length - 2, 1);
            budowniczyZapytania.AppendFormat("FROM {0};", _typRekorduNaNazwęTabeli[typRekordu]);

            string zapytanie = budowniczyZapytania.ToString();

            try
            {
                using (FbCommand komenda = new FbCommand(zapytanie, _połączenie))
                using (FbDataReader czytacz = komenda.ExecuteReader())
                    while (czytacz.Read())
                    {
                        T rekord = Activator.CreateInstance<T>();

                        for (int i = 0; i < liczbaPól; i++)
                        {
                            object wartość = czytacz.GetValue(i);

                            if (wartość != DBNull.Value)
                                właściwości.ElementAt(i).SetValue(rekord, wartość, null);
                        }

                        rekordy.Add(rekord);
                    }

                return rekordy;
            }
            catch (FbException wyjątek)
            {
                ZapiszWyjątekDoLogu(wyjątek, zapytanie);

                return null;
            }
        }

        public int Dodaj<T>(T nowyRekord) where T : Rekord
        {
            Type typRekordu = typeof(T);
            IEnumerable<PropertyInfo> właściwości = typRekordu.GetProperties().Where(w => w.Name != "ID");
            StringBuilder budowniczyZapytania = new StringBuilder("INSERT INTO ");
            int liczbaPól = właściwości.Count();

            budowniczyZapytania.AppendFormat("{0} (", _typRekorduNaNazwęTabeli[typRekordu]);

            foreach (PropertyInfo właściwość in właściwości)
                budowniczyZapytania.AppendFormat("{0}, ", właściwość.Name);

            budowniczyZapytania.Remove(budowniczyZapytania.Length - 2, 1);
            budowniczyZapytania.Append(") VALUES (");

            foreach (PropertyInfo właściwość in właściwości)
            {
                object wartość = właściwość.GetValue(nowyRekord, null);
                string format;

                if (wartość == null)
                {
                    format = "{0}, ";
                    wartość = "null";
                }
                else
                    format = "'{0}', ";

                budowniczyZapytania.AppendFormat(format, wartość);
            }

            budowniczyZapytania.Remove(budowniczyZapytania.Length - 2, 1);
            budowniczyZapytania.Append(");");

            string zapytanie = budowniczyZapytania.ToString();

            try
            {
                using (FbCommand komenda = new FbCommand(zapytanie, _połączenie))
                    return komenda.ExecuteNonQuery();
            }
            catch (FbException wyjątek)
            {
                ZapiszWyjątekDoLogu(wyjątek, zapytanie);

                return KodBłęduPz;
            }
        }

        public T Pobierz<T>(long id) where T : Rekord
        {
            T rekord = PobierzWszystkie<T>().SingleOrDefault(p => p.ID == id);

            return rekord;
        }

        public int Aktualizuj<T>(long id, T nowaWersja) where T : Rekord
        {
            Type typRekordu = typeof(T);
            IEnumerable<PropertyInfo> właściwości = typRekordu.GetProperties().Where(w => w.Name != "ID");
            StringBuilder budowniczyZapytania = new StringBuilder("UPDATE ");

            budowniczyZapytania.AppendFormat("{0} SET ", _typRekorduNaNazwęTabeli[typRekordu]);

            foreach (PropertyInfo właściwość in właściwości)
            {
                object wartość = właściwość.GetValue(nowaWersja, null);

                string format;

                if (wartość == null)
                {
                    format = "{0}={1}, ";
                    wartość = "null";
                }
                else
                    format = "{0}='{1}', ";

                budowniczyZapytania.AppendFormat(format, właściwość.Name, wartość);
            }

            budowniczyZapytania.Remove(budowniczyZapytania.Length - 2, 1);
            budowniczyZapytania.AppendFormat("WHERE ID={0};", id);

            string zapytanie = budowniczyZapytania.ToString();

            try
            {
                using (FbCommand komenda = new FbCommand(zapytanie, _połączenie))
                    return komenda.ExecuteNonQuery();
            }
            catch (FbException wyjątek)
            {
                ZapiszWyjątekDoLogu(wyjątek, zapytanie);

                return KodBłęduPz;
            }
        }

        public int Aktualizuj<T>(T nowaWersja) where T : Rekord
        {
            long id = nowaWersja.ID;

            if (id == 0)
                throw new Exception("Rekord nie istnieje. - PZ");

            return Aktualizuj<T>(id, nowaWersja);
        }

        public int Usuń<T>(long id) where T : Rekord
        {
            string zapytanie = String.Format("DELETE FROM {0} WHERE ID={1}", _typRekorduNaNazwęTabeli[typeof(T)], id);

            try
            {
                using (FbCommand komenda = new FbCommand(zapytanie, _połączenie))
                    return komenda.ExecuteNonQuery();
            }
            catch (FbException wyjątek)
            {
                ZapiszWyjątekDoLogu(wyjątek, zapytanie);

                return KodBłęduPz;
            }
        }

        public int Usuń<T>(T rekord) where T : Rekord
        {
            long id = rekord.ID;

            if (id == 0)
                throw new Exception("Rekord nie istnieje. - PZ");

            return Usuń<T>(id);
        }

        public void UtwórzKlasęNaPodstawieTabeli(string nazwaTabeli, string nazwaKlasy)
        {
            StringBuilder budowniczyNapisu = new StringBuilder();
            DataTable kolumny = _połączenie.GetSchema("Columns", new string[] { null, null, nazwaTabeli.ToUpper() });

            budowniczyNapisu.AppendLine("using System;");
            budowniczyNapisu.AppendLine();
            budowniczyNapisu.AppendFormat("namespace OdpadyDostępDoDanych {{ public class {0}:Rekord {{", nazwaKlasy);

            foreach (DataRow wiersz in kolumny.Rows)
            {
                string nazwaPola = wiersz["COLUMN_NAME"].ToString();

                if (!String.Equals(nazwaPola, "ID"))
                {
                    string typPola = wiersz["COLUMN_DATA_TYPE"].ToString();
                    bool wartościowy;

                    switch (typPola)
                    {
                        case "bigint":
                            typPola = "long";
                            wartościowy = true;

                            break;

                        case "varchar":
                            typPola = "string";
                            wartościowy = false;

                            break;

                        case "integer":
                            typPola = "int";
                            wartościowy = true;

                            break;

                        case "smallint":
                            typPola = "short";
                            wartościowy = true;

                            break;

                        default:
                            throw new Exception("Brak mapowania typów.");
                    }

                    if (wartościowy)
                        typPola = String.Format("Nullable<{0}>", typPola);

                    budowniczyNapisu.AppendFormat("public {0} {1} {{get;set;}}", typPola, nazwaPola);
                    budowniczyNapisu.AppendLine();
                }
            }

            string ścieżkaPliku = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "OdpadyDostępDoDanych", String.Concat(nazwaKlasy, ".cs"));

            budowniczyNapisu.Append("}}");
            File.WriteAllText(ścieżkaPliku, budowniczyNapisu.ToString());
        }

        public void Dispose()
        {
            _połączenie.Dispose();
        }

        void ZapiszWyjątekDoLogu(FbException wyjątekFb, string zapytanie)
        {
            StringBuilder budowniczy = new StringBuilder();
            Exception wyjątek = wyjątekFb;

            budowniczy.AppendFormat("{0} {1} ", DateTime.Now, zapytanie);
            budowniczy.AppendLine();

            while (wyjątek != null)
            {
                budowniczy.AppendLine(wyjątek.Message);

                wyjątek = wyjątek.InnerException;
            }

            budowniczy.AppendLine();
            budowniczy.AppendLine();
            budowniczy.AppendLine();

            File.AppendAllText(Path.ChangeExtension(_ścieżkaPlikuBazy, "log.txt"), budowniczy.ToString());
        }
    }
}