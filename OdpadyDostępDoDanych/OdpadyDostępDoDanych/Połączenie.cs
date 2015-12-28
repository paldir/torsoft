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

namespace OdpadyDostępDoDanych
{
    public class Połączenie : IDisposable
    {
        FbConnection _połączenie;
        Dictionary<Type, string> _typRekorduNaNazwęTabeli;

        public Połączenie()
        {
            string parametry =
            "User=SYSDBA;" +
            "Password=masterkey;" +
            "Database=" + Path.Combine(Environment.CurrentDirectory, "BazaDanych", "odpady.fdb;") +
            "DataSource=localhost;" +
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
            _typRekorduNaNazwęTabeli = new Dictionary<Type, string>();

            _typRekorduNaNazwęTabeli.Add(typeof(Kontrahent), "KONTRAHENCI");

            _połączenie.Open();
        }

        public List<T> PobierzWszystkie<T>() where T : Rekord
        {
            List<T> rekordy = new List<T>();
            Type typRekordu = typeof(T);
            PropertyInfo[] właściwości = typRekordu.GetProperties();
            StringBuilder budowniczyZapytania = new StringBuilder("SELECT ");
            int liczbaPól = właściwości.Length;

            foreach (PropertyInfo właściwość in właściwości)
                budowniczyZapytania.AppendFormat("{0}, ", właściwość.Name);

            budowniczyZapytania.Remove(budowniczyZapytania.Length - 2, 1);
            budowniczyZapytania.AppendFormat("FROM {0};", _typRekorduNaNazwęTabeli[typRekordu]);

            using (FbCommand komenda = new FbCommand(budowniczyZapytania.ToString(), _połączenie))
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

            for (int i = 0; i < liczbaPól; i++)
            {
                object wartość = właściwości.ElementAt(i).GetValue(nowyRekord, null);
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

            using (FbCommand komenda = new FbCommand(budowniczyZapytania.ToString(), _połączenie))
                return komenda.ExecuteNonQuery();
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

            throw new Exception("WHERE");

            return 0;
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
                    bool nullable = (bool)wiersz["IS_NULLABLE"];
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

                    if (nullable && wartościowy)
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
    }
}