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

namespace Odpady.DostępDoDanych
{
    public class Połączenie : IDisposable
    {
        static readonly Dictionary<Type, string> _typRekorduNaNazwęTabeli;
        static string _parametry;
        static string _ścieżkaPlikuBazy;

        public const int KodBłęduPz = 144000;

        FbConnection _połączenie;
        
        public ConnectionState Stan
        {
            get { return _połączenie.State; }
        }

        public Połączenie()
        {
            _połączenie = new FbConnection(_parametry);

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
            NameValueCollection ustawienia = ConfigurationManager.AppSettings;
            _ścieżkaPlikuBazy = ustawienia["Database"];
            _typRekorduNaNazwęTabeli = new Dictionary<Type, string>();

            _parametry = "User=SYSDBA;" +
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

            _typRekorduNaNazwęTabeli.Add(typeof(Kontrahent), "KONTRAHENCI");
            _typRekorduNaNazwęTabeli.Add(typeof(Oddział), "ODDZIALY");
            _typRekorduNaNazwęTabeli.Add(typeof(Użytkownik), "UZYTKOWNICY");
            _typRekorduNaNazwęTabeli.Add(typeof(RodzajOdpadów), "RODZAJE_ODPADOW");
            _typRekorduNaNazwęTabeli.Add(typeof(Limit), "LIMITY");
            _typRekorduNaNazwęTabeli.Add(typeof(JednostkaMiary), "JEDNOSTKI_MIARY");
            _typRekorduNaNazwęTabeli.Add(typeof(Szpieg), "ESPIONAGE");
        }

        public List<T> PobierzWszystkie<T>() where T : Rekord
        {
            List<T> rekordy = new List<T>();
            Type typRekordu = typeof(T);
            IEnumerable<PropertyInfo> właściwości;
            string zapytanie = ZbudujSelect(typRekordu, out właściwości).ToString();
            int liczbaPól = właściwości.Count();

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

        public T Pobierz<T>(long id) where T : Rekord
        {
            Type typRekordu = typeof(T);
            IEnumerable<PropertyInfo> właściwości;
            StringBuilder budowniczyZapytania = ZbudujSelect(typRekordu, out właściwości);
            int liczbaPól = właściwości.Count();

            budowniczyZapytania.AppendFormat(" WHERE ID={0};", id);

            string zapytanie = budowniczyZapytania.ToString();
            T rekord = null;

            try
            {
                using (FbCommand komenda = new FbCommand(zapytanie, _połączenie))
                using (FbDataReader czytacz = komenda.ExecuteReader())
                    if (czytacz.Read())
                    {
                        rekord = Activator.CreateInstance<T>();

                        for (int i = 0; i < liczbaPól; i++)
                        {
                            object wartość = czytacz.GetValue(i);

                            if (wartość != DBNull.Value)
                                właściwości.ElementAt(i).SetValue(rekord, wartość, null);
                        }
                    }
            }
            catch (FbException wyjątek)
            {
                ZapiszWyjątekDoLogu(wyjątek, zapytanie);
            }

            return rekord;
        }

        public long Dodaj<T>(T nowyRekord) where T : Rekord
        {
            Type typRekordu = typeof(T);
            IEnumerable<PropertyInfo> właściwości = typRekordu.GetProperties().Where(w => w.Name != "ID" && w.GetSetMethod() != null);
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
            budowniczyZapytania.Append(") RETURNING ID;");

            string zapytanie = budowniczyZapytania.ToString();

            try
            {
                using (FbCommand komenda = new FbCommand(zapytanie, _połączenie))
                {
                    FbParameter parametr = new FbParameter("ID", FbDbType.BigInt);
                    parametr.Direction = ParameterDirection.Output;

                    komenda.Parameters.Add(parametr);

                    object skalar = komenda.ExecuteScalar();
                    nowyRekord.ID = Convert.ToInt64(skalar);

                    return 1;
                }
            }
            catch (FbException wyjątek)
            {
                ZapiszWyjątekDoLogu(wyjątek, zapytanie);

                return KodBłęduPz;
            }
        }

        public int Aktualizuj<T>(long id, T nowaWersja) where T : Rekord
        {
            Type typRekordu = typeof(T);
            IEnumerable<PropertyInfo> właściwości = typRekordu.GetProperties().Where(w => w.Name != "ID" && w.GetSetMethod() != null);
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

        public void Dispose()
        {
            _połączenie.Dispose();
        }

        public void UtwórzKlasęNaPodstawieTabeli(string nazwaTabeli, string nazwaKlasy)
        {
            StringBuilder budowniczyNapisu = new StringBuilder();
            DataTable kolumny = _połączenie.GetSchema("Columns", new string[] { null, null, nazwaTabeli.ToUpper() });

            budowniczyNapisu.AppendLine("using System;");
            budowniczyNapisu.AppendLine();
            budowniczyNapisu.AppendFormat("namespace Odpady.DostępDoDanych {{ public class {0}:Rekord {{", nazwaKlasy);

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

                        case "float":
                            typPola = "float";
                            wartościowy = true;

                            break;

                        case "date":
                            typPola = "DateTime";
                            wartościowy = true;

                            break;

                        case "time":
                            typPola = "TimeSpan";
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

            string ścieżkaPliku = Path.Combine(Directory.GetParent(Environment.CurrentDirectory).Parent.Parent.FullName, "OdpadyDostępDoDanych", "Encje", String.Concat(nazwaKlasy, ".cs"));

            budowniczyNapisu.Append("}}");
            File.WriteAllText(ścieżkaPliku, budowniczyNapisu.ToString());
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

        StringBuilder ZbudujSelect(Type typRekordu, out IEnumerable<PropertyInfo> właściwości)
        {
            StringBuilder budowniczyZapytania = new StringBuilder("SELECT ");
            właściwości = typRekordu.GetProperties().Where(w => w.GetSetMethod() != null);

            foreach (PropertyInfo właściwość in właściwości)
                budowniczyZapytania.AppendFormat("{0}, ", właściwość.Name);

            budowniczyZapytania.Remove(budowniczyZapytania.Length - 2, 1);
            budowniczyZapytania.AppendFormat("FROM {0}", _typRekorduNaNazwęTabeli[typRekordu]);

            return budowniczyZapytania;
        }
    }
}