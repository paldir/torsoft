using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FirebirdSql.Data.FirebirdClient;
using System.Data;
using System.IO;
using System.Reflection;
using System.Configuration;
using System.Collections.Specialized;
using System.Globalization;

namespace Odpady.DostępDoDanych
{
    public class Połączenie : IDisposable
    {
        static readonly Dictionary<Type, string> TypRekorduNaNazwęTabeli;
        static readonly string Parametry;
        static readonly string ŚcieżkaPlikuBazy;
        static readonly CultureInfo Kultura;

        public const int KodBłęduPz = 144000;

        readonly FbConnection _połączenie;

        public ConnectionState Stan
        {
            get { return _połączenie.State; }
        }

        public Połączenie()
        {
            _połączenie = new FbConnection(Parametry);

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
            ŚcieżkaPlikuBazy = ustawienia["Database"];
            TypRekorduNaNazwęTabeli = new Dictionary<Type, string>();

            Parametry = "User=SYSDBA;" +
                        "Password=masterkey;" +
                        "Database=" + ŚcieżkaPlikuBazy + ";" +
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

            Kultura = CultureInfo.CurrentCulture.Clone() as CultureInfo;

            if (Kultura != null)
                Kultura.NumberFormat.NumberDecimalSeparator = ".";

            TypRekorduNaNazwęTabeli.Add(typeof (Kontrahent), "KONTRAHENCI");
            TypRekorduNaNazwęTabeli.Add(typeof (Oddział), "ODDZIALY");
            TypRekorduNaNazwęTabeli.Add(typeof (Użytkownik), "UZYTKOWNICY");
            TypRekorduNaNazwęTabeli.Add(typeof (RodzajOdpadów), "RODZAJE_ODPADOW");
            TypRekorduNaNazwęTabeli.Add(typeof (Limit), "LIMITY");
            TypRekorduNaNazwęTabeli.Add(typeof (JednostkaMiary), "JEDNOSTKI_MIARY");
            TypRekorduNaNazwęTabeli.Add(typeof (Szpieg), "ESPIONAGE");
            TypRekorduNaNazwęTabeli.Add(typeof (Dostawa), "DOSTAWY");
            TypRekorduNaNazwęTabeli.Add(typeof (SzczegółDostawy), "SZCZEGOLY_DOSTAW");
        }

        public List<T> PobierzWszystkie<T>() where T : Rekord
        {
            return PobierzWszystkieRekordy<T>(null);
        }

        public List<T> PobierzWszystkie<T>(List<WarunekZapytania> warunki) where T : Rekord
        {
            return PobierzWszystkieRekordy<T>(warunki);
        }

        public T Pobierz<T>(long id) where T : Rekord
        {
            List<T> rekordy = PobierzWszystkieRekordy<T>(new List<WarunekZapytania>() {new WarunekZapytania("ID", ZnakPorównania.RównaSię, id)});

            return rekordy.SingleOrDefault();
        }

        public long Dodaj<T>(T nowyRekord) where T : Rekord
        {
            Type typRekordu = typeof (T);
            PropertyInfo[] właściwości = typRekordu.GetProperties().Where(w => w.Name != "ID" && w.GetSetMethod() != null).ToArray();
            StringBuilder budowniczyZapytania = new StringBuilder("INSERT INTO ");

            budowniczyZapytania.AppendFormat("{0} (", TypRekorduNaNazwęTabeli[typRekordu]);

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
                {
                    format = "'{0}', ";

                    if (wartość is DateTime)
                        wartość = Convert.ToDateTime(wartość).ToShortDateString();
                }

                budowniczyZapytania.AppendFormat(Kultura, format, wartość);
            }

            budowniczyZapytania.Remove(budowniczyZapytania.Length - 2, 1);
            budowniczyZapytania.Append(") RETURNING ID;");

            string zapytanie = budowniczyZapytania.ToString();

            try
            {
                using (FbCommand komenda = new FbCommand(zapytanie, _połączenie))
                {
                    FbParameter parametr = new FbParameter("ID", FbDbType.BigInt)
                    {
                        Direction = ParameterDirection.Output
                    };

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
            Type typRekordu = typeof (T);
            IEnumerable<PropertyInfo> właściwości = typRekordu.GetProperties().Where(w => w.Name != "ID" && w.GetSetMethod() != null);
            StringBuilder budowniczyZapytania = new StringBuilder("UPDATE ");

            budowniczyZapytania.AppendFormat("{0} SET ", TypRekorduNaNazwęTabeli[typRekordu]);

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
                {
                    format = "{0}='{1}', ";

                    if (wartość is DateTime)
                        wartość = Convert.ToDateTime(wartość).ToShortDateString();
                }

                budowniczyZapytania.AppendFormat(Kultura, format, właściwość.Name, wartość);
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

            return Aktualizuj(id, nowaWersja);
        }

        public int Usuń<T>(long id) where T : Rekord
        {
            string zapytanie = String.Format("DELETE FROM {0} WHERE ID={1}", TypRekorduNaNazwęTabeli[typeof (T)], id);

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

            using (DataTable kolumny = _połączenie.GetSchema("Columns", new[] {null, null, nazwaTabeli.ToUpper()}))
            {
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

                            case "numeric":
                                typPola = "decimal";
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
                            typPola = String.Format("{0}?", typPola);

                        budowniczyNapisu.AppendFormat("public {0} {1} {{get;set;}}", typPola, nazwaPola);
                    }

                    budowniczyNapisu.AppendLine();
                }
            }

            DirectoryInfo folderProjektuExe = Directory.GetParent(Environment.CurrentDirectory).Parent;

            if (folderProjektuExe != null)
            {
                DirectoryInfo folderSolucji = folderProjektuExe.Parent;

                if (folderSolucji != null)
                {
                    string ścieżkaPliku = Path.Combine(folderSolucji.FullName, "Odpady.DostępDoDanych", "Encje", String.Concat(nazwaKlasy, ".cs"));

                    budowniczyNapisu.Append("}}");
                    File.WriteAllText(ścieżkaPliku, budowniczyNapisu.ToString());
                }
            }
        }

        List<T> PobierzWszystkieRekordy<T>(List<WarunekZapytania> warunki) where T : Rekord
        {
            PropertyInfo[] właściwości;
            StringBuilder budowniczyZapytania = ZbudujSelect<T>(out właściwości);

            if (warunki != null && warunki.Any())
            {
                int liczbaWarunków = warunki.Count;

                budowniczyZapytania.Append(" WHERE ");
                warunki[0].GenerujWarunek(budowniczyZapytania);

                for (int i = 1; i < liczbaWarunków; i++)
                {
                    budowniczyZapytania.Append(" AND ");
                    warunki[i].GenerujWarunek(budowniczyZapytania);
                }
            }

            string zapytanie = budowniczyZapytania.ToString();

            try
            {
                List<T> rekordy = WykonajZapytanie<T>(zapytanie, właściwości);

                return rekordy;
            }
            catch (FbException wyjątek)
            {
                ZapiszWyjątekDoLogu(wyjątek, zapytanie);

                return null;
            }
        }

        List<T> WykonajZapytanie<T>(string zapytanie, PropertyInfo[] właściwości) where T : Rekord
        {
            List<T> rekordy = new List<T>();
            int liczbaPól = właściwości.Length;

            using (FbCommand komenda = new FbCommand(zapytanie, _połączenie))
            using (FbDataReader czytacz = komenda.ExecuteReader())
                while (czytacz.Read())
                {
                    T rekord = Activator.CreateInstance<T>();

                    for (int i = 0; i < liczbaPól; i++)
                    {
                        object wartość = czytacz.GetValue(i);

                        if (wartość != DBNull.Value)
                            właściwości[i].SetValue(rekord, wartość, null);
                    }

                    rekordy.Add(rekord);
                }

            return rekordy;
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

            File.AppendAllText(Path.ChangeExtension(ŚcieżkaPlikuBazy, "log.txt"), budowniczy.ToString());
        }

        static StringBuilder ZbudujSelect<T>(out PropertyInfo[] właściwości) where T : Rekord
        {
            Type typRekordu = typeof(T);
            StringBuilder budowniczyZapytania = new StringBuilder("SELECT ");
            właściwości = typRekordu.GetProperties().Where(w => w.GetSetMethod() != null).ToArray();

            foreach (PropertyInfo właściwość in właściwości)
                budowniczyZapytania.AppendFormat("{0}, ", właściwość.Name);

            budowniczyZapytania.Remove(budowniczyZapytania.Length - 2, 1);
            budowniczyZapytania.AppendFormat("FROM {0}", TypRekorduNaNazwęTabeli[typRekordu]);

            return budowniczyZapytania;
        }
    }
}